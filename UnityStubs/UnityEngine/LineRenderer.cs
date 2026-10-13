using System;
using Unity.Collections;
using UnityStubs.Internal;

namespace UnityEngine;

public sealed class LineRenderer : Renderer
{
	private readonly LineRibbon _ribbon = new LineRibbon();

	private Vector3[] _positions = Array.Empty<Vector3>();

	[Obsolete("Use positionCount instead (UnityUpgradable) -> positionCount", false)]
	public int numPositions
	{
		get
		{
			return positionCount;
		}
		set
		{
			positionCount = value;
		}
	}

	public float startWidth
	{
		get
		{
			return _ribbon.GetWidth(end: false);
		}
		set
		{
			_ribbon.SetWidth(end: false, value);
		}
	}

	public float endWidth
	{
		get
		{
			return _ribbon.GetWidth(end: true);
		}
		set
		{
			_ribbon.SetWidth(end: true, value);
		}
	}

	public float widthMultiplier { get; set; } = 1f;

	public int numCornerVertices { get; set; }

	public int numCapVertices { get; set; }

	public bool useWorldSpace { get; set; } = true;

	public bool loop { get; set; }

	public Color startColor
	{
		get
		{
			return _ribbon.GetColor(end: false);
		}
		set
		{
			_ribbon.SetColor(end: false, value);
		}
	}

	public Color endColor
	{
		get
		{
			return _ribbon.GetColor(end: true);
		}
		set
		{
			_ribbon.SetColor(end: true, value);
		}
	}

	public int positionCount
	{
		get
		{
			return _positions.Length;
		}
		set
		{
			Array.Resize(ref _positions, Math.Max(0, value));
		}
	}

	public Vector2 textureScale { get; set; } = Vector2.one;

	public float shadowBias { get; set; } = 0.5f;

	public bool generateLightingData { get; set; }

	public LineTextureMode textureMode { get; set; }

	public LineAlignment alignment { get; set; }

	public SpriteMaskInteraction maskInteraction { get; set; }

	public AnimationCurve widthCurve
	{
		get
		{
			return _ribbon.WidthCurve;
		}
		set
		{
			_ribbon.WidthCurve = value;
		}
	}

	public Gradient colorGradient
	{
		get
		{
			return _ribbon.ColorGradient;
		}
		set
		{
			_ribbon.ColorGradient = value;
		}
	}

	[Obsolete("Use startWidth, endWidth or widthCurve instead.", false)]
	public void SetWidth(float start, float end)
	{
		startWidth = start;
		endWidth = end;
	}

	[Obsolete("Use startColor, endColor or colorGradient instead.", false)]
	public void SetColors(Color start, Color end)
	{
		startColor = start;
		endColor = end;
	}

	[Obsolete("Use positionCount instead.", false)]
	public void SetVertexCount(int count)
	{
		positionCount = count;
	}

	public void SetPosition(int index, Vector3 position)
	{
		if ((uint)index < (uint)_positions.Length)
		{
			_positions[index] = position;
		}
	}

	public Vector3 GetPosition(int index)
	{
		if ((uint)index >= (uint)_positions.Length)
		{
			return default(Vector3);
		}
		return _positions[index];
	}

	public void Simplify(float tolerance)
	{
		StubWarner.LogStub("Simplify", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\LineRenderer.cs", 48);
	}

	public void BakeMesh(Mesh mesh, bool useTransform = false)
	{
		BakeMesh(mesh, Camera.main, useTransform);
	}

	public void BakeMesh(Mesh mesh, Camera camera, bool useTransform = false)
	{
		StubWarner.LogStub("BakeMesh", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\LineRenderer.cs", 50);
	}

	public int GetPositions(Vector3[] positions)
	{
		ArgumentNullException.ThrowIfNull(positions, "positions");
		int num = Math.Min(positions.Length, _positions.Length);
		Array.Copy(_positions, positions, num);
		return num;
	}

	public void SetPositions(Vector3[] positions)
	{
		ArgumentNullException.ThrowIfNull(positions, "positions");
		_positions = (Vector3[])positions.Clone();
	}

	public void SetPositions(NativeArray<Vector3> positions)
	{
		Vector3[] array = new Vector3[positions.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = positions[i];
		}
		_positions = array;
	}

	public void SetPositions(NativeSlice<Vector3> positions)
	{
		Vector3[] array = new Vector3[positions.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = positions[i];
		}
		_positions = array;
	}

	public int GetPositions(NativeArray<Vector3> positions)
	{
		int num = Math.Min(positions.Length, _positions.Length);
		for (int i = 0; i < num; i++)
		{
			positions[i] = _positions[i];
		}
		return num;
	}

	public int GetPositions(NativeSlice<Vector3> positions)
	{
		int num = Math.Min(positions.Length, _positions.Length);
		for (int i = 0; i < num; i++)
		{
			positions[i] = _positions[i];
		}
		return num;
	}

	internal void ReleaseRendering()
	{
		_ribbon.Release();
	}

	internal void FrameUpdate(Camera camera)
	{
		int num = _positions.Length;
		if (base.enabled && num >= 2 && base.gameObject.activeInHierarchy)
		{
			int num2 = (loop ? (num + 1) : num);
			_ribbon.Reserve(num2);
			Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
			for (int i = 0; i < num2; i++)
			{
				Vector3 vector = _positions[i % num];
				_ribbon.Points[i] = (useWorldSpace ? vector : localToWorldMatrix.MultiplyPoint3x4(vector));
			}
			_ribbon.Draw(this, camera, num2, widthMultiplier, textureMode, textureScale, alignment, useStaticU: false);
		}
	}
}
