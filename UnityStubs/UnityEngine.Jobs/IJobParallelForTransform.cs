namespace UnityEngine.Jobs;

public interface IJobParallelForTransform
{
	void Execute(int index, TransformAccess transform);
}
