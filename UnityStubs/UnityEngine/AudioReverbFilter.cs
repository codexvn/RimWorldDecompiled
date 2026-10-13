using Waylith.Audio;

namespace UnityEngine;

public class AudioReverbFilter : AudioFilterBehaviour
{
	private static readonly float[,] Presets = new float[27, 10]
	{
		{ -10000f, -10000f, 1f, 1f, -2602f, 0.007f, 200f, 0.011f, 0f, 0f },
		{ -1000f, -100f, 1.49f, 0.83f, -2602f, 0.007f, 200f, 0.011f, 100f, 100f },
		{ -1000f, -6000f, 0.17f, 0.1f, -1204f, 0.001f, 207f, 0.002f, 100f, 100f },
		{ -1000f, -454f, 0.4f, 0.83f, -1646f, 0.002f, 53f, 0.003f, 100f, 100f },
		{ -1000f, -1200f, 1.49f, 0.54f, -370f, 0.007f, 1030f, 0.011f, 100f, 60f },
		{ -1000f, -6000f, 0.5f, 0.1f, -1376f, 0.003f, -1104f, 0.004f, 100f, 100f },
		{ -1000f, -300f, 2.31f, 0.64f, -711f, 0.012f, 83f, 0.017f, 100f, 100f },
		{ -1000f, -476f, 4.32f, 0.59f, -789f, 0.02f, -289f, 0.03f, 100f, 100f },
		{ -1000f, -500f, 3.92f, 0.7f, -1230f, 0.02f, -2f, 0.029f, 100f, 100f },
		{ -1000f, 0f, 2.91f, 1.3f, -602f, 0.015f, -302f, 0.022f, 100f, 100f },
		{ -1000f, -698f, 7.24f, 0.33f, -1166f, 0.02f, 16f, 0.03f, 100f, 100f },
		{ -1000f, -1000f, 10.05f, 0.23f, -602f, 0.02f, 198f, 0.03f, 100f, 100f },
		{ -1000f, -4000f, 0.3f, 0.1f, -1831f, 0.002f, -1630f, 0.03f, 100f, 100f },
		{ -1000f, -300f, 1.49f, 0.59f, -1219f, 0.007f, 441f, 0.011f, 100f, 100f },
		{ -1000f, -237f, 2.7f, 0.79f, -1214f, 0.013f, 395f, 0.02f, 100f, 100f },
		{ -1000f, -270f, 1.49f, 0.86f, -1204f, 0.007f, -4f, 0.011f, 100f, 100f },
		{ -1000f, -3300f, 1.49f, 0.54f, -2560f, 0.162f, -229f, 0.088f, 79f, 100f },
		{ -1000f, -800f, 1.49f, 0.67f, -2273f, 0.007f, -1691f, 0.011f, 50f, 100f },
		{ -1000f, -2500f, 1.49f, 0.21f, -2780f, 0.3f, -1434f, 0.1f, 27f, 100f },
		{ -1000f, -1000f, 1.49f, 0.83f, -10000f, 0.061f, 500f, 0.025f, 100f, 100f },
		{ -1000f, -2000f, 1.49f, 0.5f, -2466f, 0.179f, -1926f, 0.1f, 21f, 100f },
		{ -1000f, 0f, 1.65f, 1.5f, -1363f, 0.008f, -1153f, 0.012f, 100f, 100f },
		{ -1000f, -1000f, 2.81f, 0.14f, 429f, 0.014f, 1023f, 0.021f, 80f, 60f },
		{ -1000f, -4000f, 1.49f, 0.1f, -449f, 0.007f, 1700f, 0.011f, 100f, 100f },
		{ -1000f, 0f, 8.39f, 1.39f, -115f, 0.002f, 985f, 0.03f, 100f, 100f },
		{ -1000f, -400f, 17.23f, 0.56f, -1713f, 0.02f, -613f, 0.03f, 100f, 100f },
		{ -1000f, -151f, 7.56f, 0.91f, -626f, 0.02f, 774f, 0.03f, 100f, 100f }
	};

	private float _dryLevel;

	private float _room;

	private float _roomHF;

	private float _roomLF;

	private float _decayTime = 1f;

	private float _decayHFRatio = 0.5f;

	private float _reflectionsLevel = -10000f;

	private float _reflectionsDelay;

	private float _reverbLevel;

	private float _reverbDelay = 0.04f;

	private float _hfReference = 5000f;

	private float _lfReference = 250f;

	private float _diffusion = 100f;

	private float _density = 100f;

	private AudioReverbPreset _reverbPreset = AudioReverbPreset.User;

	public float dryLevel
	{
		get
		{
			return _dryLevel;
		}
		set
		{
			_dryLevel = value;
			Changed();
		}
	}

	public float room
	{
		get
		{
			return _room;
		}
		set
		{
			_room = value;
			Changed();
		}
	}

	public float roomHF
	{
		get
		{
			return _roomHF;
		}
		set
		{
			_roomHF = value;
			Changed();
		}
	}

	public float roomLF
	{
		get
		{
			return _roomLF;
		}
		set
		{
			_roomLF = value;
			Changed();
		}
	}

	public float decayTime
	{
		get
		{
			return _decayTime;
		}
		set
		{
			_decayTime = value;
			Changed();
		}
	}

	public float decayHFRatio
	{
		get
		{
			return _decayHFRatio;
		}
		set
		{
			_decayHFRatio = value;
			Changed();
		}
	}

	public float reflectionsLevel
	{
		get
		{
			return _reflectionsLevel;
		}
		set
		{
			_reflectionsLevel = value;
			Changed();
		}
	}

	public float reflectionsDelay
	{
		get
		{
			return _reflectionsDelay;
		}
		set
		{
			_reflectionsDelay = value;
			Changed();
		}
	}

	public float reverbLevel
	{
		get
		{
			return _reverbLevel;
		}
		set
		{
			_reverbLevel = value;
			Changed();
		}
	}

	public float reverbDelay
	{
		get
		{
			return _reverbDelay;
		}
		set
		{
			_reverbDelay = value;
			Changed();
		}
	}

	public float hfReference
	{
		get
		{
			return _hfReference;
		}
		set
		{
			_hfReference = value;
			Changed();
		}
	}

	public float lfReference
	{
		get
		{
			return _lfReference;
		}
		set
		{
			_lfReference = value;
			Changed();
		}
	}

	public float diffusion
	{
		get
		{
			return _diffusion;
		}
		set
		{
			_diffusion = value;
			Changed();
		}
	}

	public float density
	{
		get
		{
			return _density;
		}
		set
		{
			_density = value;
			Changed();
		}
	}

	public AudioReverbPreset reverbPreset
	{
		get
		{
			return _reverbPreset;
		}
		set
		{
			if (value < AudioReverbPreset.Off || (int)value >= Presets.GetLength(0))
			{
				_reverbPreset = AudioReverbPreset.User;
				return;
			}
			_dryLevel = 0f;
			_room = Presets[(int)value, 0];
			_roomHF = Presets[(int)value, 1];
			_roomLF = 0f;
			_decayTime = Presets[(int)value, 2];
			_decayHFRatio = Presets[(int)value, 3];
			_reflectionsLevel = Presets[(int)value, 4];
			_reflectionsDelay = Presets[(int)value, 5];
			_reverbLevel = Presets[(int)value, 6];
			_reverbDelay = Presets[(int)value, 7];
			_hfReference = 5000f;
			_lfReference = 250f;
			_diffusion = Presets[(int)value, 8];
			_density = Presets[(int)value, 9];
			Push();
			_reverbPreset = value;
		}
	}

	private void Changed()
	{
		_reverbPreset = AudioReverbPreset.User;
		Push();
	}

	internal override bool Apply(IAudioVoice voice, bool enabled)
	{
		ReverbParams reverbParams = default(ReverbParams);
		reverbParams.dryLevelMb = _dryLevel;
		reverbParams.roomMb = _room;
		reverbParams.roomHfMb = _roomHF;
		reverbParams.roomLfMb = _roomLF;
		reverbParams.decayTimeSeconds = _decayTime;
		reverbParams.decayHfRatio = _decayHFRatio;
		reverbParams.reflectionsLevelMb = _reflectionsLevel;
		reverbParams.reflectionsDelaySeconds = _reflectionsDelay;
		reverbParams.reverbLevelMb = _reverbLevel;
		reverbParams.reverbDelaySeconds = _reverbDelay;
		reverbParams.hfReferenceHz = _hfReference;
		reverbParams.lfReferenceHz = _lfReference;
		reverbParams.diffusionPercent = _diffusion;
		reverbParams.densityPercent = _density;
		ReverbParams parameters = reverbParams;
		return voice.SetReverb(enabled, in parameters);
	}
}
