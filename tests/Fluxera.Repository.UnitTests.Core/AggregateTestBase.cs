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
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sumInt = await this.EmployeeRepository.SumAsync(x => x.SalaryInt);
			sumInt.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumLong()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sumLong = await this.EmployeeRepository.SumAsync(x => x.SalaryLong);
			sumLong.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumDecimal()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sumDecimal = await this.EmployeeRepository.SumAsync(x => x.SalaryDecimal);
			sumDecimal.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumFloat()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sumFloat = await this.EmployeeRepository.SumAsync(x => x.SalaryFloat);
			sumFloat.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumDouble()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sumDouble = await this.EmployeeRepository.SumAsync(x => x.SalaryDouble);
			sumDouble.Should().Be(12_000);
		}

		[Test]
		public async Task ShouldSumIntWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sumInt = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryInt);
			sumInt.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumLongWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sumLong = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryLong);
			sumLong.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumDecimalWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sumDecimal = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryDecimal);
			sumDecimal.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumFloatWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sumFloat = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryFloat);
			sumFloat.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumDoubleWithPredicate()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sumDouble = await this.EmployeeRepository.SumAsync(x => x.Name.EndsWith("2"), x => x.SalaryDouble);
			sumDouble.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumIntWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			int sumInt = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryInt);
			sumInt.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumLongWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			long sumLong = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryLong);
			sumLong.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumDecimalWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			decimal sumDecimal = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryDecimal);
			sumDecimal.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumFloatWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			float sumFloat = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryFloat);
			sumFloat.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldSumDoubleWithSpecification()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double sumDouble = await this.EmployeeRepository.SumAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryDouble);
			sumDouble.Should().Be(9_000);
		}

		[Test]
		public async Task ShouldAverageInt()
		{
			Employee[] employees = this.GetSumEmployees();

			await this.EmployeeRepository.AddRangeAsync(employees);
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
			employees.ForEach(x => x.ID.Should().NotBeNull());
			employees.ForEach(x => x.ID.Value.Should().NotBeEmpty());

			double average = await this.EmployeeRepository.AverageAsync(new Specification<Employee>(x => x.Name.EndsWith("2")), x => x.SalaryDouble);
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
					SalaryDouble = 3_500
				},
				new Employee
				{
					Name = "Tester26",
					SalaryInt = 3_000,
					SalaryLong = 3_000,
					SalaryDecimal = 3_000,
					SalaryFloat = 3_000,
					SalaryDouble = 3_000
				},
				new Employee
				{
					Name = "Tester32",
					SalaryInt = 5_500,
					SalaryLong = 5_500,
					SalaryDecimal = 5_500,
					SalaryFloat = 5_500,
					SalaryDouble = 5_500
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
