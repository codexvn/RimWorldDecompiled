namespace UnityEngine;

public struct Keyframe
{
	private float _time;

	private float _value;

	private float _inTangent;

	private float _outTangent;

	private float _inWeight;

	private float _outWeight;

	private WeightedMode _weightedMode;

	public float inTangent
	{
		get
		{
			return _inTangent;
		}
		set
		{
			_inTangent = value;
		}
	}

	public float inWeight
	{
		get
		{
			return _inWeight;
		}
		set
		{
			_inWeight = value;
		}
	}

	public float outTangent
	{
		get
		{
			return _outTangent;
		}
		set
		{
			_outTangent = value;
		}
	}

	public float outWeight
	{
		get
		{
			return _outWeight;
		}
		set
		{
			_outWeight = value;
		}
	}

	public WeightedMode weightedMode
	{
		get
		{
			return _weightedMode;
		}
		set
		{
			_weightedMode = value;
		}
	}

	public float time
	{
		get
		{
			return _time;
		}
		set
		{
			_time = value;
		}
	}

	public float value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public Keyframe(float time, float value)
		: this(time, value, 0f, 0f, 0f, 0f)
	{
	}

	public Keyframe(float time, float value, float inTangent, float outTangent)
		: this(time, value, inTangent, outTangent, 0f, 0f)
	{
	}

	public Keyframe(float time, float value, float inTangent, float outTangent, float inWeight, float outWeight)
	{
		_weightedMode = WeightedMode.None;
		_time = time;
		_value = value;
		_inTangent = inTangent;
		_outTangent = outTangent;
		_inWeight = inWeight;
		_outWeight = outWeight;
	}
}
