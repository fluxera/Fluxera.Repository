namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System.IO;
	using System.Threading.Tasks;
	using NUnit.Framework;

	[SetUpFixture]
	public class GlobalFixture
	{
		[OneTimeSetUp]
		public async Task OneTimeSetUp()
		{
			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}
		}

		[OneTimeTearDown]
		public async Task OneTimeTearDown()
		{
			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}
		}
	}
}
