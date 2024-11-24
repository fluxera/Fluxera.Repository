namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.IO;
	using Fluxera.Repository.UnitTests.Core;
	using NUnit.Framework;

	[TestFixture(true, true)]
	[TestFixture(true, false)]
	[TestFixture(false, true)]
	[TestFixture(false, false)]
	public class ReferenceTests : ReferenceTestsBase
	{
		private readonly bool isPersistent;

		/// <inheritdoc />
		public ReferenceTests(bool isUnitOfWorkEnabled, bool isPersistent)
			: base(isUnitOfWorkEnabled)
		{
			this.isPersistent = isPersistent;
		}

		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}

			if(this.isPersistent)
			{
				repositoryBuilder.AddLiteRepository<RepositoryLiteContext>(repositoryName, configureOptions);
			}
			else
			{
				repositoryBuilder.AddLiteRepository<RepositoryLiteContextInMemory>(repositoryName, configureOptions);
			}
		}
	}
}
