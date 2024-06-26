[![Build Status](https://dev.azure.com/fluxera/Foundation/_apis/build/status/GitHub/fluxera.Fluxera.Repository?branchName=main&stageName=BuildAndTest)](https://dev.azure.com/fluxera/Foundation/_build/latest?definitionId=88&branchName=main)

# Fluxera.Repository
A generic repository implementation.

This repository implementation hides the actual storage implementation from the user.
The only part where the abstraction leaks storage specifics is the configuration of
a storage specific repository implementation.

The repository can be used with or without a specialized implementation, f.e. one can
just use one of the provided interfaces ```IRepository``` or ```IReadOnlyRepository```.
The repository implementation is async from top to bottom.

```C#
public class PersonController : ControllerBase 
{
    private readonly IRepository<Person> repository;

    public PersonController(IRepository<Person> repository)
    {
        this.repository = repository
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        Person result = await this.repository.GetAsync(id);
        return this.Ok(result);
    }
}
```

If you prefer to create a specialized interface and implementation you can do so, but you
have to register the service yourself.

```C#
public interface IPersonRepository : IRepository<Person, Guid>
{
}

public class PersonRepository : Repository<Person, Guid>, IPersonRepository
{
    public PersonRepository(IRepository<Person, Guid> innerRepository)
        : base(innerRepository)
    {
    }
}
```

As you can see, you have to provide the ```IRepository<T, TKey>``` as dependency to the constructor
or your specialized repository. The overall architecture of the repository uses the decorator pattern
heavily to split up the many features around the storage implementation, to keep things simple and
have every decorator layer only impolement a single responseibility. Your specialized repository then
acts as the outermost layout of decorators.

## Supported Storages

- Entity Framework Core
- LiteDB
- MongoDB
- In-Memory (for testing and prototyping only)

## Additional Features

Besides to being able top perform CRUD operation with the underlying data storage, the repository
implementation provides several additional features.

### Traits

Sometimes you don't want to expose the complete repository interface to you specialized implementations
and sometimes even the ```IReadOnlyRepository``` may be too much. For those cases you can just use 
the trait interfaces yo want to support.

- **```ICanAdd```**
  Provides methods to add items.
- **```ICanAggregate```**
  Provides methods to aggregate results, like ```Average``` and ```Sum```.
- **```ICanFind```**
  Provides methods to find single and multiple results.
- **```ICanGet```**
  Provides methods to get items by ID.
- **```ICanRemove```**
  Provides methods to remove items.
- **```ICanUpdate```**
  Provides methods to update items.

Using this set of interfaces you cann create a specialized repository interface how you see fit.

```C#
public interface IPersonRepository : ICanGet<Person, Guid>, ICanAggregate<Person, Guid>
{
}
```

### Specifications

In the most basic form you can execute find queries using expressions that provide the predicate
to satify by the result items. But this may lead to queries cluttered around your application,
maybe duplicating code in several places. Updating queries may then become cumbersome in the future.
To prevent this from happening you can create specialized specification classes that encapsulate
queries, or parts of queries and can be re-used in your application. Any specification class can 
be combines with others using operations like ```And```, ```Or```, ```Not```.

You can f.e. have a specification that finds all persons by name and another one that finds all
persons by age.

```C#
public sealed class PersonByNameSpecification : Specification<Person>
{
    private readonly string name;

    public PersonByNameSpecification(string name)
    {
        this.name = name;
    }

    protected override Expression<Func<Person, bool>> BuildQuery()
    {
        return x => x.Name == this.name;
    }
}

public sealed class PersonByAgeSpecification : Specification<Person>
{
    private readonly int age;

    public PersonByAgeSpecification(int age)
    {
        this.age = age;
    }

    protected override Expression<Func<Person, bool>> BuildQuery()
    {
        return x => x.Age == this.age;
    }
}
```

You can combine those to find any person with a specific name and age.

```C#
PersonByNameSpecification byNameQuery = new PersonByNameSpecification("Tester");
PersonByAgeSpecification byAgeQuery = new PersonByAgeSpecification(35);
ISpecification<Person> query = byNameQuery.And(byAgeQuery);
```

### Interception

Sometimes you may want to execute actions before or after you execute methods of the repository.
You can do that using the ```IInterceptor``` interface. All you have to do is implement this interface
and register the interceptor when configuring the repository. Your interceptor will then execute the
methods before and after repository calls.

You can use this feature f.e. to set audit timesstamps (CreatedAt, UpdatedAt, ...) or to implement
more complex szenarios like multi-tenecy based on a discriminator colum. You can modify the queries
that should be sent to the underlying storage. If the interceptor feature is enabled (i.e at least
one custom interceptor is registered) it makes sure that any query by ID is redirected to a predicate
based method, so you are sure that even a get-by-id will benefit from you modifiing the predicate.

### Query Options

To control how you query data is returned, you can use the ```QueryOptions``` to create sorting and
paging options that will be applied to the base-query on execution.

## Repository Decorators Hierarchy

The layers of decorators a executed in the following order.

- **Diagnostics**
  This decorator produces diagnostic events using ```System.Diagnostic``` that can be instrumented by telemetry systems.
- **Exception Logging**
  This decorator just catches every exception that may occur then logs the exception
  and throws it again.
- **Guards**
  This decorator checks the inputs for sane values and checks if the repository instance was
  disposed and throws corresponding exception.
- **Validation**
  This decorator validates the inputs to ```Add``` and ```Update``` methods for validity using
  the configures validators.
- **Caching**
  If the caching is enabled this decorator manages the handling of the cache. It tries to get
  the result of a query from the cache and if none was found executes the query and stores the
  result, in the cache.
- **Domain Events**
  This decorator dispatches added domain events after an item was added, updated or removed. 
  The exact time of execution depends on the configuration of the repository. If UoW is enabled 
  the domain events are dispatched to a outbox queue and flushed to the event handlers when the
  UoW saves the work. If the UoW is disabled the domain events are immediately dispatched to the
  event handlers after the item was added, updated or removed.
- **Data Storage**
  This is the base layer around wich all decorators are configures. This is the storage specific
  repository implementation.

## Unit of Work

The Unit of Work (UoW) pattern is disabled by default an  can be enabled using the ```EnableUnitOfWork``` method
of the ```IRepositoryOptionsBuilder```.

When enabled, a simple call to, f.e. ```AddAsync(item)``` will not persist the given item instantly. The 
add operation is added to the UoW instance and is executed when the UoW for the repository saves the changes.

```C#
await this.repository.AddAsync(new Company
{
    Name = "First Company",
    LegalType = LegalType.LimitedLiabilityCompany
});

await this.repository.AddAsync(new Company
{
    Name = "Second Company",
    LegalType = LegalType.Corporation
});

await this.unitOfWork.SaveChangesAsync();
```

Due to the fact that this library supports multiple, different repositories at the same time, a UoW 
instance can not be obtained directly using dependency injection. You can get a UoW instance from
the ```IUnitOfWorkFactory``` with the name of the repository.

```C#
this.unitOfWork = unitOfWorkFactory.CreateUnitOfWork("MongoDB");
```

## OpenTelemetry

The repository produces ```Activity``` events using ```System.Diagnistic```. Those events are used
my the OpenTelemetry integration to support diagnostic insights. To enable the support for OpenTelemetry
just add the package ```Fluxera.Repository.OpenTelemetry``` to your OpenTelemetry enabled application
and add the instrumentation for the Repository shown below.

```C#
// Configure important OpenTelemetry settings, the console exporter, and automatic instrumentation.
builder.Services.AddOpenTelemetryTracing(builder =>
{
builder
    .AddConsoleExporter()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("WebApplication1", "1.0.0"))
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddMongoDBInstrumentation()
    // Add the instrumentation for the Repository.
    .AddRepositoryInstrumentation();
});
```

## Usage

You can configure different repositories for different aggregate root types, f.e. you can have persons
in a MongoDB and invoices in a SQL database. You can choose to omit the repository name using overloads
that do not accept a name parameter. In this case, the default name "Default" is used for the repository.

```C#
// Add the repository services to the service collection and configure the repositories.
services.AddRepository(builder =>
{
    // Add default services and the repositories.
    builder.AddMongoRepository<SampleMongoContext>("MongoDB", options =>
    {
        // Enable UoW for this repository.
        options.EnableUnitOfWork();

        // Configure for what aggregate root types this repository uses.
        options.UseFor<Person>();

        // Enable the domain events (optional).
        options.EnableDomainEventHandling();

        // Enable validation providers (optional).
        options.EnableValidation(validation =>
        {
            validation.AddValidatorsFromAssembly(typeof(Person).Assembly);
        });

        // Enable caching (optional).
        options.EnableCaching((caching =>
        {
            caching
                .UseStandard()
                .UseTimeoutFor<Person>(TimeSpan.FromSeconds(20));
        });

        // Enable the interceptors (optional).
        options.EnableInterception(interception =>
        {
            interception.AddInterceptorsFromAssembly(typeof(Person).Assembly);
        });

        // Set storage specific settings.
        options.AddSetting("Mongo.ConnectionString", "mongodb://localhost:27017");
        options.AddSetting("Mongo.Database", "test");
    });
});
```

Storage-specific options are configure using a repository-specific context class. 
The following example shows the configuration of a MongoDB repository.

```C#
public class SampleMongoContext : MongoContext
{
    protected override void ConfigureOptions(MongoContextOptions options)
    {
        options.ConnectionString = "mongodb://localhost:27017";
        options.Database = "sample";
    }
}
```

The context types are registered as scoped services in the container. The ```void ConfigureOptions(MongoContextOptions options)```
method is called whenever an instance of the context is created. In a web application this will occur for
every request. You can then modify, f.e. the connection strings or database names to use for this context instance.

This comes in handy if you plan on implementing "database-per-tenant" data isolation in SaaS szenarios.

## References

The [OpenTelemetry](https://opentelemetry.io/) project.

The [MongoDB C# Driver](https://github.com/mongodb/mongo-csharp-driver) project.

The [Entity Framework Core](https://github.com/dotnet/efcore) project.

The [LiteDB](https://github.com/mbdavid/LiteDB) project.
