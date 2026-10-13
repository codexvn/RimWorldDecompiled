using System;
using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine;
using Waylith.Graphics.Meshes;
using Waylith.Rendering;

namespace UnityStubs.Internal;

internal sealed class LineRibbon
{
	[StructLayout(LayoutKind.Explicit, Size = 32)]
	private struct RibbonVertex
	{
		[FieldOffset(0)]
		public UnityEngine.Vector3 Position;

		[FieldOffset(12)]
		public uint Color;

		[FieldOffset(16)]
		public float U;

		[FieldOffset(20)]
		public float V;
	}

	private static readonly VertexLayout RibbonLayout = new VertexLayout(new global::_003C_003Ez__ReadOnlyArray<VertexAttribute>(new VertexAttribute[3]
	{
		new VertexAttribute(VertexAttributeSemantic.Position, VertexAttributeFormat.Float32, 3, 0, 0),
		new VertexAttribute(VertexAttributeSemantic.Color, VertexAttributeFormat.UNorm8, 4, 0, 12),
		new VertexAttribute(VertexAttributeSemantic.TexCoord0, VertexAttributeFormat.Float32, 4, 0, 16)
	}));

	private AnimationCurve _widthCurve = LinearCurve(1f, 1f);

	private Gradient _colorGradient = new Gradient();

	private Waylith.Graphics.Meshes.Mesh _mesh;

	private float[] _distances = new float[16];

	internal UnityEngine.Vector3[] Points = new UnityEngine.Vector3[16];

	internal float[] StaticU = new float[16];

	public AnimationCurve WidthCurve
	{
		get
		{
			return CopyCurve(_widthCurve);
		}
		set
		{
			_widthCurve = CopyCurve(value ?? throw new ArgumentNullException("value"));
		}
	}

	public Gradient ColorGradient
	{
		get
		{
			return CopyGradient(_colorGradient);
		}
		set
		{
			_colorGradient = CopyGradient(value ?? throw new ArgumentNullException("value"));
		}
	}

	public float GetWidth(bool end)
	{
		if (_widthCurve.length != 0)
		{
			return _widthCurve[end ? (_widthCurve.length - 1) : 0].value;
		}
		return 0f;
	}

	public void SetWidth(bool end, float width)
	{
		Keyframe[] keys = _widthCurve.keys;
		if (keys.Length < 2)
		{
			float num = ((keys.Length == 1) ? keys[0].value : width);
			_widthCurve = (end ? LinearCurve(num, width) : LinearCurve(width, num));
			return;
		}
		keys[end ? (keys.Length - 1) : 0].value = width;
		if (keys.Length == 2)
		{
			Linearise(ref keys[0], ref keys[1]);
		}
		_widthCurve.keys = keys;
	}

	public Color GetColor(bool end)
	{
		GradientColorKey[] colorKeys = _colorGradient.colorKeys;
		GradientAlphaKey[] alphaKeys = _colorGradient.alphaKeys;
		Color result = ((colorKeys.Length == 0) ? Color.white : colorKeys[end ? (colorKeys.Length - 1) : 0].color);
		result.a = ((alphaKeys.Length == 0) ? 1f : alphaKeys[end ? (alphaKeys.Length - 1) : 0].alpha);
		return result;
	}

	public void SetColor(bool end, Color color)
	{
		GradientColorKey[] array = _colorGradient.colorKeys;
		GradientAlphaKey[] array2 = _colorGradient.alphaKeys;
		if (array.Length < 2)
		{
			array = new GradientColorKey[2]
			{
				new GradientColorKey(GetColor(end: false), 0f),
				new GradientColorKey(GetColor(end: true), 1f)
			};
		}
		if (array2.Length < 2)
		{
			array2 = new GradientAlphaKey[2]
			{
				new GradientAlphaKey(GetColor(end: false).a, 0f),
				new GradientAlphaKey(GetColor(end: true).a, 1f)
			};
		}
		array[end ? (array.Length - 1) : 0].color = color;
		array2[end ? (array2.Length - 1) : 0].alpha = color.a;
		_colorGradient.SetKeys(array, array2);
	}

	internal void Reserve(int points)
	{
		if (Points.Length < points)
		{
			int newSize = Math.Max(points, Points.Length * 2);
			Array.Resize(ref Points, newSize);
			Array.Resize(ref StaticU, newSize);
			Array.Resize(ref _distances, newSize);
		}
	}

	internal void Release()
	{
		_mesh?.Dispose();
		_mesh = null;
	}

	internal unsafe void Draw(Renderer owner, Camera camera, int count, float widthMultiplier, LineTextureMode textureMode, UnityEngine.Vector2 textureScale, LineAlignment alignment, bool useStaticU)
	{
		UnityEngine.Material renderMaterial = owner.renderMaterial;
		if (renderMaterial == null || count < 2)
		{
			return;
		}
		EnsureCapacity(count);
		float num = 0f;
		_distances[0] = 0f;
		for (int i = 1; i < count; i++)
		{
			num += UnityEngine.Vector3.Distance(Points[i - 1], Points[i]);
			_distances[i] = num;
		}
		bool flag = alignment == LineAlignment.View && camera != null && !camera.orthographic;
		UnityEngine.Vector3 vector = ((camera != null) ? camera.transform.position : default(UnityEngine.Vector3));
		UnityEngine.Vector3 vector2 = ((alignment == LineAlignment.Local) ? owner.transform.forward : ((camera != null) ? camera.transform.forward : UnityEngine.Vector3.down));
		UnityEngine.Vector3 lhs = new UnityEngine.Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		UnityEngine.Vector3 lhs2 = new UnityEngine.Vector3(float.MinValue, float.MinValue, float.MinValue);
		UnityEngine.Vector3 vector3 = UnityEngine.Vector3.right;
		CpuMeshData cpuStorage = _mesh.CpuStorage;
		RibbonVertex* ptr = (RibbonVertex*)cpuStorage.GetVertexStream(0).Pointer;
		UnityEngine.Vector3 vector4;
		UnityEngine.Vector3 vector6;
		uint color2;
		float x;
		float num3;
		float u;
		for (int j = 0; j < count; u = x * num3, *ptr = new RibbonVertex
		{
			Position = vector4 - vector6,
			Color = color2,
			U = u,
			V = 0f
		}, ptr[1] = new RibbonVertex
		{
			Position = vector4 + vector6,
			Color = color2,
			U = u,
			V = textureScale.y
		}, lhs = UnityEngine.Vector3.Min(lhs, UnityEngine.Vector3.Min(ptr->Position, ptr[1].Position)), lhs2 = UnityEngine.Vector3.Max(lhs2, UnityEngine.Vector3.Max(ptr->Position, ptr[1].Position)), ptr += 2, j++)
		{
			vector4 = Points[j];
			UnityEngine.Vector3 vector5 = UnityEngine.Vector3.Cross(((j > 0) ? (vector4 - Points[j - 1]).normalized : default(UnityEngine.Vector3)) + ((j < count - 1) ? (Points[j + 1] - vector4).normalized : default(UnityEngine.Vector3)), flag ? (vector4 - vector) : vector2);
			if (vector5.sqrMagnitude > 1E-12f)
			{
				vector3 = vector5.normalized;
			}
			float num2 = ((num > 0f) ? (_distances[j] / num) : 0f);
			vector6 = vector3 * (_widthCurve.Evaluate(num2) * widthMultiplier * 0.5f);
			Color32 color = _colorGradient.Evaluate(num2);
			color2 = (uint)(color.r | (color.g << 8) | (color.b << 16) | (color.a << 24));
			x = textureScale.x;
			switch (textureMode)
			{
			case LineTextureMode.Stretch:
				num3 = num2;
				continue;
			case LineTextureMode.DistributePerSegment:
				num3 = (float)j / (float)(count - 1);
				continue;
			case LineTextureMode.RepeatPerSegment:
				num3 = j;
				continue;
			case LineTextureMode.Static:
				if (useStaticU)
				{
					num3 = StaticU[j];
					continue;
				}
				break;
			}
			num3 = _distances[j];
		}
		MeshBounds bounds = new MeshBounds(lhs.x, lhs.y, lhs.z, lhs2.x, lhs2.y, lhs2.z);
		cpuStorage.SetSubMesh(0, new SubMesh(0, (count - 1) * 6, Waylith.Graphics.Meshes.MeshTopology.Triangles, 0, bounds));
		cpuStorage.Bounds = bounds;
		_mesh.MarkCpuStorageChanged();
		int layer = owner.gameObject.layer;
		MeshDrawSubmission.Enqueue(new MeshDrawCommand(_mesh, renderMaterial.engineMaterial, default(DrawPropertyRange), System.Numerics.Matrix4x4.Identity, 0, LayerPassRouter.Resolve(layer), null, 0, layer));
	}

	private unsafe void EnsureCapacity(int points)
	{
		if (_mesh == null)
		{
			_mesh = new Waylith.Graphics.Meshes.Mesh
			{
				DebugName = "LineRibbon"
			};
			_mesh.MarkDynamic();
		}
		CpuMeshData cpuStorage = _mesh.CpuStorage;
		if (cpuStorage.VertexCount < points * 2)
		{
			int num = Math.Max(points, Math.Max(32, cpuStorage.VertexCount));
			cpuStorage.SetVertexLayout(RibbonLayout, num * 2);
			cpuStorage.SetIndexBufferParams((num - 1) * 6, MeshIndexFormat.UInt32);
			cpuStorage.SetSubMeshCount(1);
			uint* ptr = (uint*)cpuStorage.GetIndexBuffer().Pointer;
			for (uint num2 = 0u; num2 < num - 1; num2++)
			{
				uint num3 = (*ptr = num2 * 2);
				ptr[1] = num3 + 2;
				ptr[2] = num3 + 3;
				ptr[3] = num3;
				ptr[4] = num3 + 3;
				ptr[5] = num3 + 1;
				ptr += 6;
			}
		}
	}

	private static AnimationCurve LinearCurve(float start, float end)
	{
		Keyframe[] array = new Keyframe[2]
		{
			new Keyframe(0f, start),
			new Keyframe(1f, end)
		};
		Linearise(ref array[0], ref array[1]);
		return new AnimationCurve(array);
	}

	private static void Linearise(ref Keyframe first, ref Keyframe second)
	{
		float num = second.time - first.time;
		float num2 = ((num > 0f) ? ((second.value - first.value) / num) : 0f);
		float inTangent = (first.outTangent = num2);
		first.inTangent = inTangent;
		inTangent = (second.outTangent = num2);
		second.inTangent = inTangent;
	}

	private static AnimationCurve CopyCurve(AnimationCurve curve)
	{
		return new AnimationCurve(curve.keys)
		{
			preWrapMode = curve.preWrapMode,
			postWrapMode = curve.postWrapMode
		};
	}

	private static Gradient CopyGradient(Gradient gradient)
	{
		return new Gradient
		{
			mode = gradient.mode,
			colorKeys = gradient.colorKeys,
			alphaKeys = gradient.alphaKeys
		};
	}
}
