using System;

namespace UnityEngine;

public class Gradient : IEquatable<Gradient>
{
	private GradientColorKey[] _colorKeys = new GradientColorKey[2]
	{
		new GradientColorKey(Color.white, 0f),
		new GradientColorKey(Color.white, 1f)
	};

	private GradientAlphaKey[] _alphaKeys = new GradientAlphaKey[2]
	{
		new GradientAlphaKey(1f, 0f),
		new GradientAlphaKey(1f, 1f)
	};

	public GradientMode mode { get; set; }

	public GradientColorKey[] colorKeys
	{
		get
		{
			return (GradientColorKey[])_colorKeys.Clone();
		}
		set
		{
			_colorKeys = (GradientColorKey[])(value ?? Array.Empty<GradientColorKey>()).Clone();
			Array.Sort(_colorKeys, (GradientColorKey a, GradientColorKey b) => a.time.CompareTo(b.time));
		}
	}

	public GradientAlphaKey[] alphaKeys
	{
		get
		{
			return (GradientAlphaKey[])_alphaKeys.Clone();
		}
		set
		{
			_alphaKeys = (GradientAlphaKey[])(value ?? Array.Empty<GradientAlphaKey>()).Clone();
			Array.Sort(_alphaKeys, (GradientAlphaKey a, GradientAlphaKey b) => a.time.CompareTo(b.time));
		}
	}

	public void SetKeys(GradientColorKey[] colorKeys, GradientAlphaKey[] alphaKeys)
	{
		this.colorKeys = colorKeys;
		this.alphaKeys = alphaKeys;
	}

	public Color Evaluate(float time)
	{
		time = Mathf.Clamp01(time);
		Color result = EvaluateColor(time);
		result.a = EvaluateAlpha(time);
		return result;
	}

	private Color EvaluateColor(float time)
	{
		if (_colorKeys.Length == 0)
		{
			return Color.white;
		}
		if (time <= _colorKeys[0].time)
		{
			return _colorKeys[0].color;
		}
		for (int i = 1; i < _colorKeys.Length; i++)
		{
			if (!(time > _colorKeys[i].time))
			{
				if (mode == GradientMode.Fixed)
				{
					return _colorKeys[i].color;
				}
				float num = _colorKeys[i].time - _colorKeys[i - 1].time;
				float t = ((num <= 0f) ? 1f : ((time - _colorKeys[i - 1].time) / num));
				return Color.Lerp(_colorKeys[i - 1].color, _colorKeys[i].color, t);
			}
		}
		return _colorKeys[^1].color;
	}

	private float EvaluateAlpha(float time)
	{
		if (_alphaKeys.Length == 0)
		{
			return 1f;
		}
		if (time <= _alphaKeys[0].time)
		{
			return _alphaKeys[0].alpha;
		}
		for (int i = 1; i < _alphaKeys.Length; i++)
		{
			if (!(time > _alphaKeys[i].time))
			{
				if (mode == GradientMode.Fixed)
				{
					return _alphaKeys[i].alpha;
				}
				float num = _alphaKeys[i].time - _alphaKeys[i - 1].time;
				float t = ((num <= 0f) ? 1f : ((time - _alphaKeys[i - 1].time) / num));
				return Mathf.Lerp(_alphaKeys[i - 1].alpha, _alphaKeys[i].alpha, t);
			}
		}
		return _alphaKeys[^1].alpha;
	}

	public bool Equals(Gradient other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (mode != other.mode || _colorKeys.Length != other._colorKeys.Length || _alphaKeys.Length != other._alphaKeys.Length)
		{
			return false;
		}
		for (int i = 0; i < _colorKeys.Length; i++)
		{
			if (_colorKeys[i].color != other._colorKeys[i].color || _colorKeys[i].time != other._colorKeys[i].time)
			{
				return false;
			}
		}
		for (int j = 0; j < _alphaKeys.Length; j++)
		{
			if (_alphaKeys[j].alpha != other._alphaKeys[j].alpha || _alphaKeys[j].time != other._alphaKeys[j].time)
			{
				return false;
			}
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj is Gradient other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add((int)mode);
		GradientColorKey[] array = _colorKeys;
		for (int i = 0; i < array.Length; i++)
		{
			GradientColorKey gradientColorKey = array[i];
			hashCode.Add(gradientColorKey.color);
			hashCode.Add(gradientColorKey.time);
		}
		GradientAlphaKey[] array2 = _alphaKeys;
		for (int i = 0; i < array2.Length; i++)
		{
			GradientAlphaKey gradientAlphaKey = array2[i];
			hashCode.Add(gradientAlphaKey.alpha);
			hashCode.Add(gradientAlphaKey.time);
		}
		return hashCode.ToHashCode();
	}
}
