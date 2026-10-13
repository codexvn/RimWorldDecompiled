namespace Waylith.Jobs;

public interface IWaylithParallelForJob
{
	void Execute(int index);
}
