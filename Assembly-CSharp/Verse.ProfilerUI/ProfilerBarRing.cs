using System;

namespace Verse.ProfilerUI;

public sealed class ProfilerBarRing
{
	private float[] values = Array.Empty<float>();

	private int newestIndex = -1;

	private int count;

	public int Count => count;

	public float this[int newestOffset]
	{
		get
		{
			int num = newestIndex - newestOffset;
			if (num < 0)
			{
				num += values.Length;
			}
			return values[num];
		}
	}

	public void Clear()
	{
		newestIndex = -1;
		count = 0;
	}

	public void EnsureCapacity(int capacity)
	{
		if (values.Length != capacity)
		{
			values = new float[capacity];
			Clear();
		}
	}

	public void Push(float value)
	{
		newestIndex = ((newestIndex + 1 != values.Length) ? (newestIndex + 1) : 0);
		values[newestIndex] = value;
		if (count < values.Length)
		{
			count++;
		}
	}
}
