using System;
using System.Numerics;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace RimWorldWaylith.Rendering;

internal sealed class TransparentDepthOrder
{
	private const int OpaqueQueueMax = 2500;

	private ushort[] ranks = Array.Empty<ushort>();

	private float[] sortKeys = Array.Empty<float>();

	private int[] sortIndices = Array.Empty<int>();

	public ushort this[int index] => ranks[index];

	public void Compute(MeshDrawCommand[] commands, int count, in Matrix4x4 view)
	{
		if (ranks.Length < count)
		{
			int num = Math.Max(count, ranks.Length * 2);
			ranks = new ushort[num];
			sortKeys = new float[num];
			sortIndices = new int[num];
		}
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			ranks[i] = 0;
			ref MeshDrawCommand reference = ref commands[i];
			if (((reference.RenderQueueOverride > 0) ? reference.RenderQueueOverride : reference.Material.RenderQueue) > 2500)
			{
				Vector3? vector = reference.Instancing?.SortCentre;
				Vector3 p;
				if (vector.HasValue)
				{
					p = vector.GetValueOrDefault();
				}
				else
				{
					bool sectionSubmesh;
					MeshBounds meshBounds = SortBounds(in reference, out sectionSubmesh);
					float y = (sectionSubmesh ? meshBounds.MinY : ((meshBounds.MinY + meshBounds.MaxY) * 0.5f));
					p = TransformPoint(reference.Matrix, new Vector3((meshBounds.MinX + meshBounds.MaxX) * 0.5f, y, (meshBounds.MinZ + meshBounds.MaxZ) * 0.5f));
				}
				float num3 = MathF.Abs(TransformPoint(in view, p).Z);
				if (float.IsFinite(num3))
				{
					num3 = MathF.Round(num3 * 4096f) / 4096f;
					sortKeys[num2] = 0f - num3;
					sortIndices[num2] = i;
					num2++;
				}
			}
		}
		if (num2 == 0)
		{
			return;
		}
		Array.Sort(sortKeys, sortIndices, 0, num2);
		int num4 = 1;
		for (int j = 1; j < num2; j++)
		{
			if (sortKeys[j] != sortKeys[j - 1])
			{
				num4++;
			}
		}
		float num5 = ((num4 > 65535) ? (65535f / (float)(num4 - 1)) : 1f);
		int num6 = 0;
		for (int k = 0; k < num2; k++)
		{
			if (k > 0 && sortKeys[k] != sortKeys[k - 1])
			{
				num6++;
			}
			ranks[sortIndices[k]] = (ushort)((float)num6 * num5);
		}
	}

	private static MeshBounds SortBounds(in MeshDrawCommand command, out bool sectionSubmesh)
	{
		sectionSubmesh = false;
		SubMesh[] subMeshes = command.Mesh.CpuStorage.SubMeshes;
		if ((uint)command.SubmeshIndex < (uint)subMeshes.Length)
		{
			MeshBounds bounds = subMeshes[command.SubmeshIndex].Bounds;
			if (!bounds.IsEmpty && (bounds.MinX != 0f || bounds.MinY != 0f || bounds.MinZ != 0f || bounds.MaxX != 0f || bounds.MaxY != 0f || bounds.MaxZ != 0f))
			{
				MeshBounds bounds2 = command.Mesh.Bounds;
				sectionSubmesh = bounds2.MinY != bounds.MinY || bounds2.MaxY != bounds.MaxY;
				return bounds;
			}
		}
		return command.Mesh.Bounds;
	}

	private static Vector3 TransformPoint(in Matrix4x4 m, Vector3 p)
	{
		return new Vector3(m.M11 * p.X + m.M12 * p.Y + m.M13 * p.Z + m.M14, m.M21 * p.X + m.M22 * p.Y + m.M23 * p.Z + m.M24, m.M31 * p.X + m.M32 * p.Y + m.M33 * p.Z + m.M34);
	}
}
