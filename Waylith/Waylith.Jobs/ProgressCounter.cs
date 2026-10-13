using System.Threading;

namespace Waylith.Jobs;

public sealed class ProgressCounter
{
	private int _done;

	public int Total { get; }

	public int Done => Volatile.Read(in _done);

	public ProgressCounter(int total)
	{
		Total = total;
	}

	public void Increment()
	{
		Interlocked.Increment(ref _done);
	}

	public void Add(int count)
	{
		Interlocked.Add(ref _done, count);
	}
}
