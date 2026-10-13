using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityStubs.Internal;

internal sealed class MeshRaycastBvh
{
	private struct Node
	{
		public Vector3 Min;

		public Vector3 Max;

		public int LeftOrFirst;

		public int Count;
	}

	private const int LeafSize = 8;

	private readonly Vector3[] vertices;

	private readonly int[] triangles;

	private readonly int[] order;

	private Node[] nodes;

	private int nodeCount;

	private int depth;

	public bool IsFor(Vector3[] v, int[] t)
	{
		if (v == vertices)
		{
			return t == triangles;
		}
		return false;
	}

	public MeshRaycastBvh(Vector3[] vertices, int[] triangles)
	{
		this.vertices = vertices;
		this.triangles = triangles;
		int num = triangles.Length / 3;
		order = new int[num];
		float[] array = new float[num * 9];
		for (int i = 0; i < num; i++)
		{
			order[i] = i;
			Vector3 vector = vertices[triangles[i * 3]];
			Vector3 vector2 = vertices[triangles[i * 3 + 1]];
			Vector3 vector3 = vertices[triangles[i * 3 + 2]];
			int num2 = i * 9;
			array[num2] = MathF.Min(vector.x, MathF.Min(vector2.x, vector3.x));
			array[num2 + 1] = MathF.Min(vector.y, MathF.Min(vector2.y, vector3.y));
			array[num2 + 2] = MathF.Min(vector.z, MathF.Min(vector2.z, vector3.z));
			array[num2 + 3] = MathF.Max(vector.x, MathF.Max(vector2.x, vector3.x));
			array[num2 + 4] = MathF.Max(vector.y, MathF.Max(vector2.y, vector3.y));
			array[num2 + 5] = MathF.Max(vector.z, MathF.Max(vector2.z, vector3.z));
			array[num2 + 6] = (vector.x + vector2.x + vector3.x) / 3f;
			array[num2 + 7] = (vector.y + vector2.y + vector3.y) / 3f;
			array[num2 + 8] = (vector.z + vector2.z + vector3.z) / 3f;
		}
		nodes = new Node[Math.Max(1, 2 * num / 8 + 2)];
		nodeCount = 1;
		Build(0, 0, num, array, 1);
	}

	private unsafe void Build(int nodeIndex, int first, int count, float[] scratch, int level)
	{
		depth = Math.Max(depth, level);
		byte* intPtr = stackalloc byte[12];
		ReadOnlySpan<float> readOnlySpan = RuntimeHelpers.CreateSpan<float>((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		// IL cpblk instruction
		Unsafe.CopyBlockUnaligned(intPtr, ref readOnlySpan[0], 12);
		Span<float> span = new Span<float>(intPtr, 3);
		byte* intPtr2 = stackalloc byte[12];
		readOnlySpan = RuntimeHelpers.CreateSpan<float>((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		// IL cpblk instruction
		Unsafe.CopyBlockUnaligned(intPtr2, ref readOnlySpan[0], 12);
		Span<float> span2 = new Span<float>(intPtr2, 3);
		byte* intPtr3 = stackalloc byte[12];
		readOnlySpan = RuntimeHelpers.CreateSpan<float>((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		// IL cpblk instruction
		Unsafe.CopyBlockUnaligned(intPtr3, ref readOnlySpan[0], 12);
		Span<float> span3 = new Span<float>(intPtr3, 3);
		byte* intPtr4 = stackalloc byte[12];
		readOnlySpan = RuntimeHelpers.CreateSpan<float>((RuntimeFieldHandle)/*OpCode not supported: LdMemberToken*/);
		// IL cpblk instruction
		Unsafe.CopyBlockUnaligned(intPtr4, ref readOnlySpan[0], 12);
		Span<float> span4 = new Span<float>(intPtr4, 3);
		for (int i = first; i < first + count; i++)
		{
			int num = order[i] * 9;
			for (int j = 0; j < 3; j++)
			{
				if (scratch[num + j] < span[j])
				{
					span[j] = scratch[num + j];
				}
				if (scratch[num + 3 + j] > span2[j])
				{
					span2[j] = scratch[num + 3 + j];
				}
				float num2 = scratch[num + 6 + j];
				if (num2 < span3[j])
				{
					span3[j] = num2;
				}
				if (num2 > span4[j])
				{
					span4[j] = num2;
				}
			}
		}
		Vector3 min = new Vector3(span[0], span[1], span[2]);
		Vector3 max = new Vector3(span2[0], span2[1], span2[2]);
		float num3 = span4[0] - span3[0];
		float num4 = span4[1] - span3[1];
		float num5 = span4[2] - span3[2];
		int num6 = ((!(num3 > num4)) ? ((num4 > num5) ? 1 : 2) : ((!(num3 > num5)) ? 2 : 0));
		float num7 = (span3[num6] + span4[num6]) * 0.5f;
		int num8 = first;
		if (count > 8 && span4[num6] - span3[num6] > 0f)
		{
			int num9 = first + count - 1;
			while (num8 <= num9)
			{
				if (scratch[order[num8] * 9 + 6 + num6] < num7)
				{
					num8++;
					continue;
				}
				ref int reference = ref order[num8];
				ref int reference2 = ref order[num9];
				int num10 = order[num9--];
				int num11 = order[num8];
				reference = num10;
				reference2 = num11;
			}
		}
		int num12 = num8 - first;
		if (num12 == 0 || num12 == count)
		{
			nodes[nodeIndex] = new Node
			{
				Min = min,
				Max = max,
				LeftOrFirst = first,
				Count = count
			};
			return;
		}
		if (nodeCount + 2 > nodes.Length)
		{
			Array.Resize(ref nodes, nodes.Length * 2);
		}
		int num13 = nodeCount;
		nodeCount += 2;
		nodes[nodeIndex] = new Node
		{
			Min = min,
			Max = max,
			LeftOrFirst = num13,
			Count = 0
		};
		Build(num13, first, num12, scratch, level + 1);
		Build(num13 + 1, num8, count - num12, scratch, level + 1);
	}

	public bool Raycast(Vector3 origin, Vector3 dir, float minT, float maxT, out float bestT, out int bestTriangle)
	{
		bestT = maxT;
		bestTriangle = -1;
		if (order.Length == 0)
		{
			return false;
		}
		Vector3 inv = new Vector3(1f / dir.x, 1f / dir.y, 1f / dir.z);
		Span<int> span = ((depth >= 120) ? ((Span<int>)new int[depth + 8]) : stackalloc int[128]);
		Span<int> span2 = span;
		int num = 0;
		span2[num++] = 0;
		while (num > 0)
		{
			ref Node reference = ref nodes[span2[--num]];
			if (!HitsBox(origin, inv, reference.Min, reference.Max, bestT))
			{
				continue;
			}
			if (reference.Count == 0)
			{
				span2[num++] = reference.LeftOrFirst;
				span2[num++] = reference.LeftOrFirst + 1;
				continue;
			}
			for (int i = reference.LeftOrFirst; i < reference.LeftOrFirst + reference.Count; i++)
			{
				int num2 = order[i];
				if (MeshRaycastUtility.IntersectTriangle(origin, dir, vertices[triangles[num2 * 3]], vertices[triangles[num2 * 3 + 1]], vertices[triangles[num2 * 3 + 2]], out var t) && t >= minT && (t < bestT || (t == bestT && num2 < bestTriangle)))
				{
					bestT = t;
					bestTriangle = num2;
				}
			}
		}
		return bestTriangle >= 0;
	}

	private static bool HitsBox(Vector3 origin, Vector3 inv, Vector3 min, Vector3 max, float maxT)
	{
		float num = 0f;
		float num2 = maxT;
		for (int i = 0; i < 3; i++)
		{
			float num3 = (min[i] - origin[i]) * inv[i];
			float num4 = (max[i] - origin[i]) * inv[i];
			if (num3 > num4)
			{
				float num5 = num4;
				num4 = num3;
				num3 = num5;
			}
			if (num3 > num)
			{
				num = num3;
			}
			if (num4 < num2)
			{
				num2 = num4;
			}
			if (num > num2)
			{
				return false;
			}
		}
		return true;
	}
}
