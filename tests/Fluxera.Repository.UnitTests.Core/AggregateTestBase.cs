namespace Fluxera.Repository.UnitTests.Core
{
	using System.Threading.Tasks;
	using FluentAssertions;
	using Fluxera.Repository.Specifications;
	using Fluxera.Repository.UnitTests.Core.EmployeeAggregate;
	using Fluxera.Repository.UnitTests.Core.PersonAggregate;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;
	using NUnit.Framework;

	[PublicAPI]
	public abstract class AggregateTestBase : RepositoryTestBase
	{
		[Test]
		public async Task ShouldCount()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester12"
				},
				new Person
				{
					Name = "Tester26"
				},
				new Person
				{
					Name = "Tester32"
				}
			};
			await this.PersonRepository.AddRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			long count = await this.PersonRepository.CountAsync();
			count.Should().Be(persons.Length);
		}

		[Test]
		public async Task ShouldCountWithPredicate()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester12"
				},
				new Person
				{
					Name = "Tester26"
				},
				new Person
				{
					Name = "Tester32"
				}
			};
			await this.PersonRepository.AddRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			long count = await this.PersonRepository.CountAsync(x => x.Name.EndsWith("2"));
			count.Should().Be(2);
		}

		[Test]
		public async Task ShouldCountWithSpecification()
		{
			Person[] persons =
			{
				new Person
				{
					Name = "Tester12"
				},
				new Person
				{
					Name = "Tester26"
				},
				new Person
				{
					Name = "Tester32"
				}
			};
			await this.PersonRepository.AddRangeAsync(persons);
			await this.UnitOfWork.SaveChangesAsync();

			persons.ForEach(x => x.ID.Should().NotBeEmpty());

			long count = await this.PersonRepository.CountAsync(new Specification<Person>(x => x.Name.EndsWith("2")));
			count.Should().Be(2);
		}

		[Test]
		public async Task ShouldCountWithStronglyTypedId()
		{
			Employee[] employees =
			{
				new Employee
				{
					Name = "Tester12"
				},
				new Employee
				{
					Name = "Tester26"
				},
				new Employee
				{
					Name = "Tester32"
				}
			};
			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long count = await this.EmployeeRepository.CountAsync();
			count.Should().Be(employees.Length);
		}

		[Test]
		public async Task ShouldCountWithPredicateWithStronglyTypedId()
		{
			Employee[] employees =
			{
				new Employee
				{
					Name = "Tester12"
				},
				new Employee
				{
					Name = "Tester26"
				},
				new Employee
				{
					Name = "Tester32"
				}
			};
			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long count = await this.EmployeeRepository.CountAsync(x => x.Name.EndsWith("2"));
			count.Should().Be(2);
		}

		[Test]
		public async Task ShouldSumInt()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sum = await this.EmployeeRepository.SumAsync(x => x.SalaryInt);
			sum.Should().Be(12_000);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumLong()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sum = await this.EmployeeRepository.SumAsync(x => x.SalaryLong);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumDecimal()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sum = await this.EmployeeRepository.SumAsync(x => x.SalaryDecimal);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumFloat()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sum = await this.EmployeeRepository.SumAsync(x => x.SalaryFloat);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumDouble()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sum = await this.EmployeeRepository.SumAsync(x => x.SalaryDouble);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumIntWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryInt);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumLongWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryLong);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumDecimalWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryDecimal);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumFloatWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryFloat);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumDoubleWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryDouble);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumIntWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryInt);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumLongWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryLong);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumDecimalWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryDecimal);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumFloatWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryFloat);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumDoubleWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryDouble);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldAverageInt()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.SalaryInt);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageLong()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.SalaryLong);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageDecimal()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal average = await this.EmployeeRepository.AverageAsync(x => x.SalaryDecimal);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageFloat()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float average = await this.EmployeeRepository.AverageAsync(x => x.SalaryFloat);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageDouble()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.SalaryDouble);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageIntWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryInt);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageLongWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryLong);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageDecimalWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryDecimal);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageFloatWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryFloat);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageDoubleWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryDouble);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageIntWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryInt);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageLongWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryLong);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageDecimalWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryDecimal);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageFloatWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryFloat);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageDoubleWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryDouble);
			average.Should().Be(4_500);
		}


		[Test]
		public async Task ShouldSumNullableInt()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sum = await this.EmployeeRepository.SumAsync(x => x.SalaryNullableInt);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumNullableLong()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sum = await this.EmployeeRepository.SumAsync(x => x.SalaryNullableLong);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumNullableDecimal()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sum = await this.EmployeeRepository.SumAsync(x => x.SalaryNullableDecimal);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumNullableFloat()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sum = await this.EmployeeRepository.SumAsync(x => x.SalaryNullableFloat);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumNullableDouble()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sum = await this.EmployeeRepository.SumAsync(x => x.SalaryNullableDouble);
			sum.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumNullableIntWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableInt);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableLongWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableLong);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableDecimalWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableDecimal);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableFloatWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableFloat);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableDoubleWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sum = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableDouble);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableIntWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableInt);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableLongWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableLong);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableDecimalWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableDecimal);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableFloatWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableFloat);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumNullableDoubleWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sum = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableDouble);
			sum.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldAverageNullableInt()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.SalaryNullableInt);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageNullableLong()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.SalaryNullableLong);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageNullableDecimal()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal average = await this.EmployeeRepository.AverageAsync(x => x.SalaryNullableDecimal);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageNullableFloat()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float average = await this.EmployeeRepository.AverageAsync(x => x.SalaryNullableFloat);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageNullableDouble()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.SalaryNullableDouble);
			average.Should().Be(4_000);
		}

		[Test]
		public async Task ShouldAverageNullableIntWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableInt);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableLongWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableLong);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableDecimalWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableDecimal);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableFloatWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableFloat);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableDoubleWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(x => x.Name.EndsWith("2"), x => x.SalaryNullableDouble);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableIntWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableInt);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableLongWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableLong);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableDecimalWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableDecimal);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableFloatWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableFloat);
			average.Should().Be(4_500);
		}

		[Test]
		public async Task ShouldAverageNullableDoubleWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			await this.UnitOfWork.SaveChangesAsync();

			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryNullableDouble);
			average.Should().Be(4_500);
		}

		private Employee[] GetSumEmployees()
		{
			Employee[] employees =
			{
				new Employee
				{
					Name = "Tester12",
					SalaryInt = 3_500,
					SalaryLong = 3_500,
					SalaryDecimal = 3_500,
					SalaryFloat = 3_500,
					SalaryDouble = 3_500,
					SalaryNullableInt = 3_500,
					SalaryNullableLong = 3_500,
					SalaryNullableDecimal = 3_500,
					SalaryNullableFloat = 3_500,
					SalaryNullableDouble = 3_500
				},
				new Employee
				{
					Name = "Tester26",
					SalaryInt = 3_000,
					SalaryLong = 3_000,
					SalaryDecimal = 3_000,
					SalaryFloat = 3_000,
					SalaryDouble = 3_000,
					SalaryNullableInt = 3_000,
					SalaryNullableLong = 3_000,
					SalaryNullableDecimal = 3_000,
					SalaryNullableFloat = 3_000,
					SalaryNullableDouble = 3_000
				},
				new Employee
				{
					Name = "Tester32",
					SalaryInt = 5_500,
					SalaryLong = 5_500,
					SalaryDecimal = 5_500,
					SalaryFloat = 5_500,
					SalaryDouble = 5_500,
					SalaryNullableInt = 5_500,
					SalaryNullableLong = 5_500,
					SalaryNullableDecimal = 5_500,
					SalaryNullableFloat = 5_500,
					SalaryNullableDouble = 5_500
				}
			};

			return employees;
		}

		private Employee[] GetAverageEmployees()
		{
			return this.GetSumEmployees();
		}
	}
}
