# Fluxera.Repository
A generic repository implementation.



## Repository Decorators Hierarchy

```plaintext
-> Exception Logging
    -> Guards
        -> Validation
            -> Caching
                -> Domain Events
                    -> Store Specific Repository
```


## Supported Storages

- In-Memory
- Entity Framework Core (untested)
- LiteDB
- MongoDB
- OData (untested)

## Usage

```C#
services.AddRepository(builder => 
{
    // TODO ...
});
```