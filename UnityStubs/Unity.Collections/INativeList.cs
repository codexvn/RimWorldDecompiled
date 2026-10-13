namespace Unity.Collections;

public interface INativeList<T> : IIndexable<T>
{
	int Capacity { get; set; }

	bool IsEmpty { get; }

	T this[int index] { get; set; }

	void Clear();
}
