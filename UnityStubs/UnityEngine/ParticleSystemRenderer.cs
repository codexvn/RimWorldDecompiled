using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityStubs.Internal;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace UnityEngine;

public class ParticleSystemRenderer : Renderer
{
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	private struct ParticleVertex
	{
		[FieldOffset(0)]
		public Vector3 Position;

		[FieldOffset(12)]
		public uint Color;

		[FieldOffset(16)]
		public float U;

		[FieldOffset(20)]
		public float V;

		[FieldOffset(24)]
		public float Z;

		[FieldOffset(28)]
		public float W;
	}

	private readonly List<ParticleSystemVertexStream> _activeVertexStreams;

	private int _stableRandomComponents;

	private static readonly VertexLayout ParticleLayout = new VertexLayout(new global::_003C_003Ez__ReadOnlyArray<VertexAttribute>(new VertexAttribute[3]
	{
		new VertexAttribute(VertexAttributeSemantic.Position, VertexAttributeFormat.Float32, 3, 0, 0),
		new VertexAttribute(VertexAttributeSemantic.Color, VertexAttributeFormat.UNorm8, 4, 0, 12),
		new VertexAttribute(VertexAttributeSemantic.TexCoord0, VertexAttributeFormat.Float32, 4, 0, 16)
	}));

	private Waylith.Graphics.Meshes.Mesh _particleMesh;

	public ParticleSystemRenderMode renderMode { get; set; }

	public ParticleSystemSortMode sortMode { get; set; }

	public ParticleSystemRenderSpace alignment { get; set; }

	public float lengthScale { get; set; } = 2f;

	public float velocityScale { get; set; }

	public float cameraVelocityScale { get; set; }

	public float normalDirection { get; set; } = 1f;

	public float sortingFudge { get; set; }

	public float minParticleSize { get; set; }

	public float maxParticleSize { get; set; } = 0.5f;

	public Vector3 pivot { get; set; }

	public Vector3 flip { get; set; }

	public Mesh mesh { get; set; }

	public int meshCount => (mesh != null) ? 1 : 0;

	public Material trailMaterial { get; set; }

	public bool enableGPUInstancing { get; set; }

	public bool allowRoll { get; set; } = true;

	public bool freeformStretching { get; set; }

	public bool rotateWithStretchDirection { get; set; } = true;

	public int activeVertexStreamsCount => _activeVertexStreams.Count;

	public void GetActiveVertexStreams(List<ParticleSystemVertexStream> streams)
	{
		streams.Clear();
		streams.AddRange(_activeVertexStreams);
	}

	public void SetActiveVertexStreams(List<ParticleSystemVertexStream> streams)
	{
		_activeVertexStreams.Clear();
		_activeVertexStreams.AddRange(streams);
		_stableRandomComponents = 0;
		bool flag = false;
		using List<ParticleSystemVertexStream>.Enumerator enumerator = streams.GetEnumerator();
		while (enumerator.MoveNext())
		{
			switch (enumerator.Current)
			{
			case ParticleSystemVertexStream.UV:
				if (!flag)
				{
					flag = true;
					continue;
				}
				break;
			case ParticleSystemVertexStream.StableRandomX:
				if (flag && _stableRandomComponents == 0)
				{
					_stableRandomComponents = 1;
					continue;
				}
				break;
			case ParticleSystemVertexStream.StableRandomXY:
				if (flag && _stableRandomComponents == 0)
				{
					_stableRandomComponents = 2;
					continue;
				}
				break;
			case ParticleSystemVertexStream.Position:
			case ParticleSystemVertexStream.Normal:
			case ParticleSystemVertexStream.Color:
				continue;
			}
			StubWarner.LogStub("SetActiveVertexStreams", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticleSystemRenderer.cs", 68);
			break;
		}
	}

	internal void ReleaseMesh()
	{
		_particleMesh?.Dispose();
		_particleMesh = null;
	}

	internal unsafe void Draw(ParticleSystem system, Quaternion viewRotation)
	{
		int count = system._count;
		Material material = base.renderMaterial;
		if (!base.enabled || count == 0 || material == null || renderMode == ParticleSystemRenderMode.None)
		{
			return;
		}
		GetQuadAxes(system, viewRotation, out var right, out var up);
		EnsureCapacity(count);
		bool flag = system.mainState.simulationSpace == ParticleSystemSimulationSpace.Local;
		Matrix4x4 matrix4x = (flag ? system.transform.localToWorldMatrix : Matrix4x4.identity);
		float num = SizeScale(system);
		ParticleSystem.TextureSheetAnimationState textureSheetState = system.textureSheetState;
		bool flag2 = textureSheetState.enabled && textureSheetState.mode == ParticleSystemAnimationMode.Grid;
		int num2 = Math.Max(1, textureSheetState.numTilesX);
		int num3 = Math.Max(1, textureSheetState.numTilesY);
		int stableRandomComponents = _stableRandomComponents;
		Vector3 lhs = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 lhs2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		CpuMeshData cpuStorage = _particleMesh.CpuStorage;
		ParticleVertex* ptr = (ParticleVertex*)cpuStorage.GetVertexStream(0).Pointer;
		for (int i = 0; i < count; i++)
		{
			Vector3 vector = (flag ? matrix4x.MultiplyPoint3x4(system._position[i]) : system._position[i]);
			Vector2 vector2 = system._currentSize[i] * num;
			float num4 = MathF.Cos(system._rotation[i]);
			float num5 = MathF.Sin(system._rotation[i]);
			Vector3 vector3 = (right * num4 - up * num5) * (vector2.x * 0.5f);
			Vector3 vector4 = (up * num4 + right * num5) * (vector2.y * 0.5f);
			Color32 color = system._currentColor[i];
			uint color2 = (uint)(color.r | (color.g << 8) | (color.b << 16) | (color.a << 24));
			float u = 0f;
			float u2 = 1f;
			float v = 0f;
			float v2 = 1f;
			if (flag2)
			{
				int num6 = system._frame[i];
				int num7 = num6 % num2;
				int num8 = num6 / num2;
				u = (float)num7 / (float)num2;
				u2 = (float)(num7 + 1) / (float)num2;
				v = 1f - (float)(num8 + 1) / (float)num3;
				v2 = 1f - (float)num8 / (float)num3;
			}
			float z = 0f;
			float w = 0f;
			if (stableRandomComponents > 0)
			{
				z = ParticleSystem.SeedFloat(system._seed[i], 21u);
				if (stableRandomComponents > 1)
				{
					w = ParticleSystem.SeedFloat(system._seed[i], 22u);
				}
			}
			*ptr = new ParticleVertex
			{
				Position = vector - vector3 - vector4,
				Color = color2,
				U = u,
				V = v,
				Z = z,
				W = w
			};
			ptr[1] = new ParticleVertex
			{
				Position = vector - vector3 + vector4,
				Color = color2,
				U = u,
				V = v2,
				Z = z,
				W = w
			};
			ptr[2] = new ParticleVertex
			{
				Position = vector + vector3 + vector4,
				Color = color2,
				U = u2,
				V = v2,
				Z = z,
				W = w
			};
			ptr[3] = new ParticleVertex
			{
				Position = vector + vector3 - vector4,
				Color = color2,
				U = u2,
				V = v,
				Z = z,
				W = w
			};
			Vector3 vector5 = Vector3.Max(Abs(vector3 + vector4), Abs(vector3 - vector4));
			lhs = Vector3.Min(lhs, vector - vector5);
			lhs2 = Vector3.Max(lhs2, vector + vector5);
			ptr += 4;
		}
		MeshBounds meshBounds = new MeshBounds(lhs.x, lhs.y, lhs.z, lhs2.x, lhs2.y, lhs2.z);
		cpuStorage.SetSubMesh(0, new SubMesh(0, count * 6, Waylith.Graphics.Meshes.MeshTopology.Triangles, 0, meshBounds));
		cpuStorage.Bounds = meshBounds;
		_particleMesh.MarkCpuStorageChanged();
		int layer = base.gameObject.layer;
		MeshDrawSubmission.Enqueue(new MeshDrawCommand(_particleMesh, material.engineMaterial, default(DrawPropertyRange), System.Numerics.Matrix4x4.Identity, 0, LayerPassRouter.Resolve(layer), null, 0, layer));
	}

	private static Vector3 Abs(Vector3 v)
	{
		return new Vector3(MathF.Abs(v.x), MathF.Abs(v.y), MathF.Abs(v.z));
	}

	private static float SizeScale(ParticleSystem system)
	{
		Transform transform = system.transform;
		Vector3 vector;
		switch (system.mainState.scalingMode)
		{
		case ParticleSystemScalingMode.Shape:
			return 1f;
		case ParticleSystemScalingMode.Local:
			vector = transform.localScale;
			break;
		default:
		{
			Matrix4x4 localToWorldMatrix = transform.localToWorldMatrix;
			vector = new Vector3(((Vector3)localToWorldMatrix.GetColumn(0)).magnitude, ((Vector3)localToWorldMatrix.GetColumn(1)).magnitude, ((Vector3)localToWorldMatrix.GetColumn(2)).magnitude);
			break;
		}
		}
		return (MathF.Abs(vector.x) + MathF.Abs(vector.y) + MathF.Abs(vector.z)) / 3f;
	}

	private void GetQuadAxes(ParticleSystem system, Quaternion viewRotation, out Vector3 right, out Vector3 up)
	{
		switch (renderMode)
		{
		case ParticleSystemRenderMode.HorizontalBillboard:
			right = Vector3.right;
			up = Vector3.forward;
			return;
		case ParticleSystemRenderMode.VerticalBillboard:
			right = viewRotation * Vector3.right;
			right.y = 0f;
			right = ((right.sqrMagnitude > 0f) ? right.normalized : Vector3.right);
			up = Vector3.up;
			return;
		case ParticleSystemRenderMode.Stretch:
		case ParticleSystemRenderMode.Mesh:
			StubWarner.LogStub("GetQuadAxes", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticleSystemRenderer.cs", 210);
			break;
		}
		Quaternion quaternion = alignment switch
		{
			ParticleSystemRenderSpace.World => Quaternion.identity, 
			ParticleSystemRenderSpace.Local => system.transform.rotation, 
			_ => viewRotation, 
		};
		if (alignment == ParticleSystemRenderSpace.Velocity)
		{
			StubWarner.LogStub("GetQuadAxes", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ParticleSystemRenderer.cs", 221);
		}
		right = quaternion * Vector3.right;
		up = quaternion * Vector3.up;
	}

	private unsafe void EnsureCapacity(int particles)
	{
		if (_particleMesh == null)
		{
			_particleMesh = new Waylith.Graphics.Meshes.Mesh
			{
				DebugName = "ParticleSystem " + base.gameObject.name
			};
			_particleMesh.MarkDynamic();
		}
		CpuMeshData cpuStorage = _particleMesh.CpuStorage;
		if (cpuStorage.VertexCount < particles * 4)
		{
			int num = Math.Max(particles, Math.Max(64, cpuStorage.VertexCount / 2));
			cpuStorage.SetVertexLayout(ParticleLayout, num * 4);
			cpuStorage.SetIndexBufferParams(num * 6, MeshIndexFormat.UInt32);
			cpuStorage.SetSubMeshCount(1);
			uint* ptr = (uint*)cpuStorage.GetIndexBuffer().Pointer;
			for (uint num2 = 0u; num2 < num; num2++)
			{
				uint num3 = (*ptr = num2 * 4);
				ptr[1] = num3 + 1;
				ptr[2] = num3 + 2;
				ptr[3] = num3;
				ptr[4] = num3 + 2;
				ptr[5] = num3 + 3;
				ptr += 6;
			}
		}
	}

	public ParticleSystemRenderer()
	{
		int num = 4;
		List<ParticleSystemVertexStream> list = new List<ParticleSystemVertexStream>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<ParticleSystemVertexStream> span = CollectionsMarshal.AsSpan(list);
		span[0] = ParticleSystemVertexStream.Position;
		span[1] = ParticleSystemVertexStream.Normal;
		span[2] = ParticleSystemVertexStream.Color;
		span[3] = ParticleSystemVertexStream.UV;
		_activeVertexStreams = list;
		base._002Ector();
	}
}
