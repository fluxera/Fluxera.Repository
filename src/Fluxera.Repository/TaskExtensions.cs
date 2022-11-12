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
		public static Task Then(this Task sourceTask, CancellationToken cancellationToken)
		{
			TaskCompletionSource taskCompletionSource = new TaskCompletionSource();

			sourceTask.ContinueWith(task =>
			{
				if(task.IsFaulted)
				{
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
