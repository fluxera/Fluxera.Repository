﻿namespace Fluxera.Repository.LiteDB.IntegrationTests
{
	using System;
	using System.IO;
	using Fluxera.Repository.UnitTests.Core;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class ReferenceTests : ReferenceTestsBase
	{
		/// <inheritdoc />
		protected override void AddRepositoryUnderTest(IRepositoryBuilder repositoryBuilder,
			string repositoryName, Action<IRepositoryOptionsBuilder> configureOptions)
		{
			foreach(string file in Directory.EnumerateFiles(".", "*.db"))
			{
				File.Delete(file);
			}

			repositoryBuilder.Services.AddLiteContext(serviceProvider =>
			{
				DatabaseProvider databaseProvider = serviceProvider.GetRequiredService<DatabaseProvider>();
				IRepositoryRegistry repositoryRegistry = serviceProvider.GetRequiredService<IRepositoryRegistry>();
				IDatabaseNameProvider databaseNameProvider = serviceProvider.GetService<IDatabaseNameProvider>();

				return new RepositoryLiteContext(repositoryName, databaseProvider, repositoryRegistry, databaseNameProvider);
			});

			repositoryBuilder.AddLiteRepository<RepositoryLiteContext>(repositoryName, options =>
			{
				options.AddSetting("Lite.Database", $"{Guid.NewGuid():N}.db");

				configureOptions.Invoke(options);
			});
		}
	}
}
