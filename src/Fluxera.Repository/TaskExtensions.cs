namespace Fluxera.Repository
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	///     Contains extension methods for <see cref="Task" /> type.
	/// </summary>
	public static class TaskExtensions
	{
		/// <summary>
		///     Checks the source task for faults and cancellations and sets
		///     the result of the <see cref="TaskCompletionSource" /> accordingly.
		/// </summary>
		/// <param name="sourceTask"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public static Task Then(this Task sourceTask, CancellationToken cancellationToken)
		{
			TaskCompletionSource taskCompletionSource = new TaskCompletionSource();

			sourceTask.ContinueWith(task =>
			{
				if(task.IsFaulted)
				{
					// ReSharper disable once PossibleNullReferenceException
					taskCompletionSource.TrySetException(task.Exception.InnerExceptions);
				}
				else if(task.IsCanceled)
				{
					taskCompletionSource.TrySetCanceled();
				}
				else
				{
					try
					{
						taskCompletionSource.TrySetResult();
					}
					catch(Exception ex)
					{
						taskCompletionSource.TrySetException(ex);
					}
				}
			}, cancellationToken);

			return taskCompletionSource.Task;
		}
	}
}
