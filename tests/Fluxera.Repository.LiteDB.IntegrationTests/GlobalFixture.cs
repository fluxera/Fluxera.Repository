namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System.IO;
	using System.Threading.Tasks;
	using NUnit.Framework;

	[SetUpFixture]
	public class GlobalFixture
	{
		[OneTimeSetUp]
		public Task OneTimeSetUp()
		{
			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}

			return Task.CompletedTask;
		}

		[OneTimeTearDown]
		public Task OneTimeTearDown()
		{
			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}

			return Task.CompletedTask;
		}
	}
}
