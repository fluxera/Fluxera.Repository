namespace Fluxera.Repository
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using Fluxera.Extensions.Validation;
	using Fluxera.Utilities.Extensions;
	using JetBrains.Annotations;

	[PublicAPI]
	internal static class Errors
	{
		public static Exception ItemNotValid(IEnumerable<ValidationError> errors, Exception innerException = null)
		{
			const string message = "The item is not valid. See validation errors for details.";
			ValidationException exception = new ValidationException(message, innerException);
			exception.Errors.AddRange(errors);
			return exception;
		}

		public static Exception NoRepositoryFound(string aggregateName)
		{
			string message = $"No repository was found for aggregate type '{aggregateName}'.";
			return new InvalidOperationException(message);
		}

		public static Exception NoRepositoryConfigurationFound(string repositoryName)
		{
			string message = $"No repository configuration was found for repository '{repositoryName}'.";
			return new InvalidOperationException(message);
		}

		public static Exception RepositoryNameAlreadyUsed(string repositoryName)
		{
			string message = $"The repository name '{repositoryName}' was already used when registering a repository.";
			return new InvalidOperationException(message);
		}

		public static Exception ConfigurationMethodAlreadyUsed([CallerMemberName] string methodName = null)
		{
			string message = $"The configuration method '{methodName}' can only be used once.";
			return new InvalidOperationException(message);
		}

		public static Exception NoRepositoryNameAvailable(Type type)
		{
			string message = $"The repository name was not found for type '{type.Name}'. Maybe a default repository is not configured.";
			return new InvalidOperationException(message);
		}

		public static Exception NoRepositoryOptionsAvailable(RepositoryName repositoryName)
		{
			string message = $"The repository options were found for repository '{repositoryName}'.";
			return new InvalidOperationException(message);
		}
	}
}
