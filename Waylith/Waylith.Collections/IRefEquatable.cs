namespace Waylith.Collections;

public interface IRefEquatable<T>
{
	bool Equals(in T other);
}
