using System;

namespace UnityEngine;

[AttributeUsage(AttributeTargets.Class)]
public class DefaultExecutionOrder : Attribute
{
	public int order { get; }

	public DefaultExecutionOrder(int order)
	{
		this.order = order;
	}
}
