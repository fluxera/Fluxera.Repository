namespace Fluxera.Repository.Specifications
{
	using JetBrains.Annotations;

	/// <summary>
	///     Represents the base class for composite specifications.
	/// </summary>
	/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
	[PublicAPI]
	public abstract class CompositeSpecification<T> : Specification<T>
		where T : class
	{
		/// <summary>
		///     Constructs a new instance of <see cref="CompositeSpecification{T}" /> class.
		/// </summary>
		/// <param name="left">The left specification.</param>
		/// <param name="right">The right specification.</param>
		protected CompositeSpecification(ISpecification<T> left, ISpecification<T> right)
		{
			this.Left = left;
			this.Right = right;
		}

		/// <summary>
		///     Gets the first specification.
		/// </summary>
		public ISpecification<T> Left { get; }

		/// <summary>
		///     Gets the second specification.
		/// </summary>
		public ISpecification<T> Right { get; }
	}
}
