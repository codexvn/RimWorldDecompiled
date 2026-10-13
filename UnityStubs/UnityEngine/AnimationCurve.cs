using System;
using System.Collections.Generic;

namespace UnityEngine;

public class AnimationCurve : IEquatable<AnimationCurve>
{
	private readonly List<Keyframe> _keys = new List<Keyframe>();

	public Keyframe[] keys
	{
		get
		{
			return _keys.ToArray();
		}
		set
		{
			_keys.Clear();
			if (value != null)
			{
				_keys.AddRange(value);
				SortKeys();
			}
		}
	}

	public int length => _keys.Count;

	public Keyframe this[int index] => _keys[index];

	public WrapMode postWrapMode { get; set; }

	public WrapMode preWrapMode { get; set; }

	public AnimationCurve(params Keyframe[] keys)
	{
		preWrapMode = WrapMode.ClampForever;
		postWrapMode = WrapMode.ClampForever;
		this.keys = keys;
	}

	public AnimationCurve()
		: this(Array.Empty<Keyframe>())
	{
	}

	public int AddKey(float time, float value)
	{
		int num = AddKey(new Keyframe(time, value));
		RecalculateSplineSlope(num);
		return num;
	}

	public int AddKey(Keyframe key)
	{
		int num = FindInsertIndex(key.time);
		_keys.Insert(num, key);
		return num;
	}

	public int MoveKey(int index, Keyframe key)
	{
		ValidateIndex(index);
		_keys.RemoveAt(index);
		return AddKey(key);
	}

	public void RemoveKey(int index)
	{
		ValidateIndex(index);
		_keys.RemoveAt(index);
	}

	public void SmoothTangents(int index, float weight)
	{
		ValidateIndex(index);
		if (_keys.Count > 1)
		{
			Keyframe keyframe = _keys[index];
			bool flag = index > 0;
			bool flag2 = index < _keys.Count - 1;
			float num;
			if (!(flag && flag2))
			{
				num = ((!flag) ? Slope(keyframe, _keys[index + 1]) : Slope(_keys[index - 1], keyframe));
			}
			else
			{
				Keyframe left = _keys[index - 1];
				Keyframe right = _keys[index + 1];
				num = Slope(left, right);
			}
			float outTangent = (keyframe.inTangent = num * weight);
			keyframe.outTangent = outTangent;
			_keys[index] = keyframe;
		}
	}

	private void RecalculateSplineSlope(int index)
	{
		if (_keys.Count >= 2)
		{
			float num = ((index == 0) ? Slope(_keys[0], _keys[1]) : ((index != _keys.Count - 1) ? (0.5f * Slope(_keys[index - 1], _keys[index]) + 0.5f * Slope(_keys[index], _keys[index + 1])) : Slope(_keys[index - 1], _keys[index])));
			Keyframe value = _keys[index];
			value.inTangent = num;
			value.outTangent = num;
			_keys[index] = value;
		}
	}

	public float Evaluate(float time)
	{
		if (_keys.Count == 0)
		{
			return 0f;
		}
		if (_keys.Count == 1)
		{
			return _keys[0].value;
		}
		Keyframe keyframe = _keys[0];
		List<Keyframe> list = _keys;
		Keyframe keyframe2 = list[list.Count - 1];
		if (time < keyframe.time)
		{
			float num = ApplyWrap(time, keyframe.time, keyframe2.time, preWrapMode);
			if (float.IsNaN(num))
			{
				return keyframe.value;
			}
			time = num;
		}
		else if (time > keyframe2.time)
		{
			float num2 = ApplyWrap(time, keyframe.time, keyframe2.time, postWrapMode);
			if (float.IsNaN(num2))
			{
				return keyframe2.value;
			}
			time = num2;
		}
		if (time <= keyframe.time)
		{
			return keyframe.value;
		}
		if (time >= keyframe2.time)
		{
			return keyframe2.value;
		}
		for (int i = 0; i < _keys.Count - 1; i++)
		{
			Keyframe left = _keys[i];
			Keyframe right = _keys[i + 1];
			if (time < right.time || i == _keys.Count - 2)
			{
				return EvaluateSegment(left, right, time);
			}
		}
		return keyframe2.value;
	}

	public static AnimationCurve Constant(float timeStart, float timeEnd, float value)
	{
		return new AnimationCurve(new Keyframe(timeStart, value), new Keyframe(timeEnd, value));
	}

	public static AnimationCurve EaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
	{
		return new AnimationCurve(new Keyframe(timeStart, valueStart, 0f, 0f), new Keyframe(timeEnd, valueEnd, 0f, 0f));
	}

	public static AnimationCurve Linear(float timeStart, float valueStart, float timeEnd, float valueEnd)
	{
		float num = Slope(new Keyframe(timeStart, valueStart), new Keyframe(timeEnd, valueEnd));
		return new AnimationCurve(new Keyframe(timeStart, valueStart, num, num), new Keyframe(timeEnd, valueEnd, num, num));
	}

	public bool Equals(AnimationCurve other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (preWrapMode != other.preWrapMode || postWrapMode != other.postWrapMode || _keys.Count != other._keys.Count)
		{
			return false;
		}
		for (int i = 0; i < _keys.Count; i++)
		{
			Keyframe keyframe = _keys[i];
			Keyframe keyframe2 = other._keys[i];
			if (!Mathf.Approximately(keyframe.time, keyframe2.time) || !Mathf.Approximately(keyframe.value, keyframe2.value) || !Mathf.Approximately(keyframe.inTangent, keyframe2.inTangent) || !Mathf.Approximately(keyframe.outTangent, keyframe2.outTangent) || !Mathf.Approximately(keyframe.inWeight, keyframe2.inWeight) || !Mathf.Approximately(keyframe.outWeight, keyframe2.outWeight))
			{
				return false;
			}
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj is AnimationCurve other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		hashCode.Add((int)preWrapMode);
		hashCode.Add((int)postWrapMode);
		for (int i = 0; i < _keys.Count; i++)
		{
			Keyframe keyframe = _keys[i];
			hashCode.Add(keyframe.time);
			hashCode.Add(keyframe.value);
			hashCode.Add(keyframe.inTangent);
			hashCode.Add(keyframe.outTangent);
			hashCode.Add(keyframe.inWeight);
			hashCode.Add(keyframe.outWeight);
		}
		return hashCode.ToHashCode();
	}

	private static float EvaluateSegment(Keyframe left, Keyframe right, float time)
	{
		float num = right.time - left.time;
		if (Mathf.Approximately(num, 0f))
		{
			return right.value;
		}
		float num2 = (time - left.time) / num;
		float num3 = num2 * num2;
		float num4 = num3 * num2;
		float num5 = 2f * num4 - 3f * num3 + 1f;
		float num6 = num4 - 2f * num3 + num2;
		float num7 = -2f * num4 + 3f * num3;
		float num8 = num4 - num3;
		float num9 = left.outTangent * num;
		float num10 = right.inTangent * num;
		return num5 * left.value + num6 * num9 + num7 * right.value + num8 * num10;
	}

	private static float ApplyWrap(float time, float startTime, float endTime, WrapMode wrapMode)
	{
		return wrapMode switch
		{
			WrapMode.Loop => WrapLoop(time, startTime, endTime), 
			WrapMode.PingPong => WrapPingPong(time, startTime, endTime), 
			_ => float.NaN, 
		};
	}

	private static float WrapLoop(float time, float startTime, float endTime)
	{
		float a = endTime - startTime;
		if (Mathf.Approximately(a, 0f))
		{
			return startTime;
		}
		return startTime + Mathf.Repeat(time - startTime, a);
	}

	private static float WrapPingPong(float time, float startTime, float endTime)
	{
		float a = endTime - startTime;
		if (Mathf.Approximately(a, 0f))
		{
			return startTime;
		}
		return startTime + Mathf.PingPong(time - startTime, a);
	}

	private int FindInsertIndex(float time)
	{
		for (int i = 0; i < _keys.Count; i++)
		{
			if (time < _keys[i].time)
			{
				return i;
			}
		}
		return _keys.Count;
	}

	private void SortKeys()
	{
		_keys.Sort((Keyframe left, Keyframe right) => left.time.CompareTo(right.time));
	}

	private void ValidateIndex(int index)
	{
		if ((uint)index >= (uint)_keys.Count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
	}

	private static float Slope(Keyframe left, Keyframe right)
	{
		float num = right.time - left.time;
		if (Mathf.Approximately(num, 0f))
		{
			return 0f;
		}
		return (right.value - left.value) / num;
	}
}
