using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class WeatherDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__38 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WeatherDef _003C_003E4__this;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CConfigErrors_003Ed__38(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			WeatherDef weatherDef = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (weatherDef.skyColorsDay.saturation == 0f || weatherDef.skyColorsDusk.saturation == 0f || weatherDef.skyColorsNightMid.saturation == 0f || weatherDef.skyColorsNightEdge.saturation == 0f)
				{
					_003C_003E2__current = "a sky color has saturation of 0";
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__38 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__38(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public IntRange durationRange = new IntRange(16000, 160000);

	public bool repeatable;

	public bool isBad;

	public bool canOccurAsRandomForcedEvent = true;

	public Favorability favorability = Favorability.Neutral;

	public FloatRange temperatureRange = new FloatRange(-999f, 999f);

	public SimpleCurve commonalityRainfallFactor;

	public int transitionTicksOverride = int.MaxValue;

	public int minMonolithLevel;

	public bool canOccurInAmbientHorror;

	[MustTranslate]
	public string letterText;

	[MustTranslate]
	public string letterLabel;

	public LetterDef letterDef;

	public float rainRate;

	public float snowRate;

	public float sandRate;

	public float windSpeedFactor = 1f;

	public float windSpeedOffset;

	public float moveSpeedMultiplier = 1f;

	public float accuracyMultiplier = 1f;

	public float maxRangeCap = -1f;

	public float perceivePriority;

	public bool doToxicBuildup;

	public ThoughtDef weatherThought;

	public float maxGlow = 1f;

	public bool preventSkygaze;

	public bool preventsShuttleLaunch;

	public List<SoundDef> ambientSounds = new List<SoundDef>();

	public List<WeatherEventMaker> eventMakers = new List<WeatherEventMaker>();

	public List<Type> overlayClasses = new List<Type>();

	public SkyColorSet skyColorsNightMid;

	public SkyColorSet skyColorsNightEdge;

	public SkyColorSet skyColorsDay;

	public SkyColorSet skyColorsDusk;

	public Type workerClass = typeof(WeatherWorker);

	[Unsaved(false)]
	private WeatherWorker workerInt;

	public WeatherWorker Worker
	{
		get
		{
			if (workerInt == null)
			{
				workerInt = (WeatherWorker)Activator.CreateInstance(workerClass, this);
			}
			return workerInt;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__38))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__38(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static WeatherDef Named(string defName)
	{
		return DefDatabase<WeatherDef>.GetNamed(defName);
	}
}
