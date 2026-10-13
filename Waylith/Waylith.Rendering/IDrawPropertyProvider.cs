using System;
using System.Collections.Generic;

namespace Waylith.Rendering;

public interface IDrawPropertyProvider
{
	int PropertyCount { get; }

	bool HasStorageBindings => false;

	int CopyTo(Span<DrawProperty> destination);

	void CollectStorageBindings(List<InstanceStorageBinding> output)
	{
	}
}
