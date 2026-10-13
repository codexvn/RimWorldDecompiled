namespace Unity.Collections;

public interface IIndexable<T>
{
	int Length { get; set; }

	T ElementAt(int index);
}
