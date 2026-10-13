using System;
using Unity.Collections;
using UnityStubs.Internal;

namespace UnityEngine;

public sealed class TrailRenderer : Renderer
{
	private readonly LineRibbon _ribbon = new LineRibbon();

	private Vector3[] _positions = new Vector3[16];

	private float[] _birthTimes = new float[16];

	private float[] _emittedDistance = new float[16];

	private int _count;

	private float _emitted;

	private bool _hadPositions;

	[Obsolete("Use positionCount instead (UnityUpgradable) -> positionCount", false)]
	public int numPositions => positionCount;

	public float time { get; set; } = 5f;

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

	public bool autodestruct { get; set; }

	public bool emitting { get; set; } = true;

	public int numCornerVertices { get; set; }

	public int numCapVertices { get; set; }

	public float minVertexDistance { get; set; } = 0.1f;

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

	public int positionCount => _count;

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

	public void SetPosition(int index, Vector3 position)
	{
		if ((uint)index < (uint)_count)
		{
			_positions[index] = position;
		}
	}

	public Vector3 GetPosition(int index)
	{
		if ((uint)index >= (uint)_count)
		{
			return default(Vector3);
		}
		return _positions[index];
	}

	public void Clear()
	{
		_count = 0;
	}

	public void BakeMesh(Mesh mesh, bool useTransform = false)
	{
		BakeMesh(mesh, Camera.main, useTransform);
	}

	public void BakeMesh(Mesh mesh, Camera camera, bool useTransform = false)
	{
		StubWarner.LogStub("BakeMesh", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TrailRenderer.cs", 40);
	}

	public int GetPositions(Vector3[] positions)
	{
		ArgumentNullException.ThrowIfNull(positions, "positions");
		int num = Math.Min(positions.Length, _count);
		Array.Copy(_positions, positions, num);
		return num;
	}

	public int GetVisiblePositions(Vector3[] positions)
	{
		return GetPositions(positions);
	}

	public void SetPositions(Vector3[] positions)
	{
		ArgumentNullException.ThrowIfNull(positions, "positions");
		_count = 0;
		AddPositions(positions);
	}

	public void AddPosition(Vector3 position)
	{
		Append(position, Time.time);
	}

	public void AddPositions(Vector3[] positions)
	{
		ArgumentNullException.ThrowIfNull(positions, "positions");
		foreach (Vector3 position in positions)
		{
			AddPosition(position);
		}
	}

	public void SetPositions(NativeArray<Vector3> positions)
	{
		_count = 0;
		AddPositions(positions);
	}

	public void SetPositions(NativeSlice<Vector3> positions)
	{
		_count = 0;
		AddPositions(positions);
	}

	public int GetPositions(NativeArray<Vector3> positions)
	{
		int num = Math.Min(positions.Length, _count);
		for (int i = 0; i < num; i++)
		{
			positions[i] = _positions[i];
		}
		return num;
	}

	public int GetPositions(NativeSlice<Vector3> positions)
	{
		int num = Math.Min(positions.Length, _count);
		for (int i = 0; i < num; i++)
		{
			positions[i] = _positions[i];
		}
		return num;
	}

	public int GetVisiblePositions(NativeArray<Vector3> positions)
	{
		return GetPositions(positions);
	}

	public int GetVisiblePositions(NativeSlice<Vector3> positions)
	{
		return GetPositions(positions);
	}

	public void AddPositions(NativeArray<Vector3> positions)
	{
		for (int i = 0; i < positions.Length; i++)
		{
			AddPosition(positions[i]);
		}
	}

	public void AddPositions(NativeSlice<Vector3> positions)
	{
		for (int i = 0; i < positions.Length; i++)
		{
			AddPosition(positions[i]);
		}
	}

	private void Append(Vector3 position, float now)
	{
		if (_count == _positions.Length)
		{
			Array.Resize(ref _positions, _count * 2);
			Array.Resize(ref _birthTimes, _count * 2);
			Array.Resize(ref _emittedDistance, _count * 2);
		}
		if (_count > 0)
		{
			_emitted += Vector3.Distance(_positions[_count - 1], position);
		}
		_positions[_count] = position;
		_birthTimes[_count] = now;
		_emittedDistance[_count] = _emitted;
		_count++;
		_hadPositions = true;
	}

	private void Expire(float now)
	{
		int i;
		for (i = 0; i < _count && now - _birthTimes[i] > time; i++)
		{
		}
		if (i != 0)
		{
			_count -= i;
			Array.Copy(_positions, i, _positions, 0, _count);
			Array.Copy(_birthTimes, i, _birthTimes, 0, _count);
			Array.Copy(_emittedDistance, i, _emittedDistance, 0, _count);
		}
	}

	internal void ReleaseRendering()
	{
		_ribbon.Release();
	}

	internal void FrameUpdate(Camera camera)
	{
		if (!base.enabled || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		float now = Time.time;
		Vector3 position = base.transform.position;
		float num = Math.Max(minVertexDistance, 0.0001f);
		if (emitting && (_count == 0 || (position - _positions[_count - 1]).sqrMagnitude >= num * num))
		{
			Append(position, now);
		}
		Expire(now);
		if (autodestruct && _count == 0 && _hadPositions)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		bool flag = emitting && _count > 0 && position != _positions[_count - 1];
		int num2 = _count + (flag ? 1 : 0);
		if (num2 >= 2)
		{
			_ribbon.Reserve(num2);
			int num3 = 0;
			if (flag)
			{
				_ribbon.Points[num3] = position;
				_ribbon.StaticU[num3++] = _emitted + Vector3.Distance(position, _positions[_count - 1]);
			}
			for (int num4 = _count - 1; num4 >= 0; num4--)
			{
				_ribbon.Points[num3] = _positions[num4];
				_ribbon.StaticU[num3++] = _emittedDistance[num4];
			}
			_ribbon.Draw(this, camera, num2, widthMultiplier, textureMode, textureScale, alignment, useStaticU: true);
		}
	}
}
