namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests
{
	using JetBrains.Annotations;
	using Microsoft.EntityFrameworkCore;

	[PublicAPI]
	public sealed class WrongBaseClassContext : DbContext
	{
	}
}
