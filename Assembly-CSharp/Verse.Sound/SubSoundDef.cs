using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse.Sound;

public class SubSoundDef : Editable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass38_0
	{
		public Type neededFilter;

		internal bool _003CConfigErrors_003Eb__0(SoundFilter fil)
		{
			return fil.GetType() == neededFilter;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__38 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SubSoundDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<SoundParameterMapping>.Enumerator _003C_003E7__wrap2;

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
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 6:
			case 7:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<SoundParameterMapping>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SubSoundDef subSoundDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Editable)subSoundDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0086;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ec;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0124;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0151;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0189;
				case 6:
					_003C_003E1__state = -4;
					break;
				case 7:
					{
						_003C_003E1__state = -4;
						goto IL_0271;
					}
					IL_0086:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (subSoundDef.sustainAttack + subSoundDef.sustainRelease > subSoundDef.sustainLoopDurationRange.TrueMin && subSoundDef.sustainLoopDurationRange != FloatRange.Zero)
					{
						_003C_003E2__current = "Attack + release < min loop duration. Sustain samples will cut off.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ec;
					IL_0151:
					if (subSoundDef.gameSpeedRange.min > subSoundDef.gameSpeedRange.max)
					{
						_003C_003E2__current = "gameSpeedRange min/max are reversed.";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0189;
					IL_0189:
					_003C_003E7__wrap2 = subSoundDef.paramMappings.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0271;
					IL_00ec:
					if (subSoundDef.distRange.min > subSoundDef.distRange.max)
					{
						_003C_003E2__current = "Dist range min/max are reversed.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0124;
					IL_0271:
					while (_003C_003E7__wrap2.MoveNext())
					{
						SoundParameterMapping current2 = _003C_003E7__wrap2.Current;
						_003C_003Ec__DisplayClass38_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass38_0();
						if (current2.inParam == null || current2.outParam == null)
						{
							_003C_003E2__current = "At least one parameter mapping is missing an in or out parameter.";
							_003C_003E1__state = 6;
							return true;
						}
						CS_0024_003C_003E8__locals4.neededFilter = current2.outParam?.NeededFilterType;
						if (CS_0024_003C_003E8__locals4.neededFilter != null && !subSoundDef.filters.Any((SoundFilter fil) => fil.GetType() == CS_0024_003C_003E8__locals4.neededFilter))
						{
							_003C_003E2__current = "A parameter wants to modify the " + CS_0024_003C_003E8__locals4.neededFilter?.ToString() + " filter, but this sound doesn't have it.";
							_003C_003E1__state = 7;
							return true;
						}
					}
					break;
					IL_0124:
					if (subSoundDef.gameSpeedRange.max == 0)
					{
						_003C_003E2__current = "gameSpeedRange should have max value greater than 0";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0151;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<SoundParameterMapping>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
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

	public const string DefaultName = "UnnamedSubSoundDef";

	[Description("A name to help you identify the sound.")]
	[DefaultValue("UnnamedSubSoundDef")]
	[MayTranslate]
	public string name = "UnnamedSubSoundDef";

	[Description("Whether this sound plays on the camera or in the world.\n\nThis must match what the game expects from the sound Def with this name.")]
	[DefaultValue(false)]
	public bool onCamera;

	[Description("Whether to mute this subSound while the game is paused (either by the pausing in play or by opening a menu)")]
	[DefaultValue(false)]
	public bool muteWhenPaused;

	[Description("Whether this subSound's tempo should be affected by the current tick rate.")]
	[DefaultValue(false)]
	public bool tempoAffectedByGameSpeed;

	[Description("The sound grains used for this sample. The game will choose one of these randomly when the sound plays. Sustainers choose one for each sample as it begins.")]
	public List<AudioGrain> grains = new List<AudioGrain>();

	[EditSliderRange(0f, 100f)]
	[Description("This sound will play at a random volume inside this range.\n\nSustainers will choose a different random volume for each sample.")]
	[DefaultFloatRange(50f, 50f)]
	public FloatRange volumeRange = new FloatRange(50f, 50f);

	[EditSliderRange(0.05f, 2f)]
	[Description("This sound will play at a random pitch inside this range.\n\nSustainers will choose a different random pitch for each sample.")]
	[DefaultFloatRange(1f, 1f)]
	public FloatRange pitchRange = FloatRange.One;

	[EditSliderRange(0f, 200f)]
	[Description("This sound will play max volume when it is under minDistance from the camera.\n\nIt will fade out linearly until the camera distance reaches its max.")]
	[DefaultFloatRange(25f, 70f)]
	public FloatRange distRange = new FloatRange(25f, 70f);

	[Description("When the sound chooses the next grain, you may use this setting to have it avoid repeating the last grain, or avoid repeating any of the grains in the last X played, X being half the total number of grains defined.")]
	[DefaultValue(RepeatSelectMode.NeverLastHalf)]
	public RepeatSelectMode repeatMode = RepeatSelectMode.NeverLastHalf;

	[Description("Mappings between game parameters (like fire size or wind speed) and properties of the sound.")]
	[DefaultEmptyList(typeof(SoundParameterMapping))]
	public List<SoundParameterMapping> paramMappings = new List<SoundParameterMapping>();

	[Description("The filters to be applied to this sound.")]
	[DefaultEmptyList(typeof(SoundFilter))]
	public List<SoundFilter> filters = new List<SoundFilter>();

	[Description("A range of possible times between when this sound is triggered and when it will actually start playing.")]
	[DefaultFloatRange(0f, 0f)]
	public FloatRange startDelayRange = FloatRange.Zero;

	[Description("A range of game speeds this sound can be played on.")]
	public IntRange gameSpeedRange = new IntRange(0, 999);

	[Description("One shots sharing the same tag are treated as the same sound when determining importance.")]
	[NoTranslate]
	public string tag;

	[Description("If true, each sample in the sustainer will be looped and ended only after sustainerLoopDurationRange. If not, the sounds will just play once and end after their own length.")]
	[DefaultValue(true)]
	public bool sustainLoop = true;

	[EditSliderRange(0f, 10f)]
	[Description("The range of durations that individual looped samples in the sustainer will have. Each sample ends after a time randomly chosen in this range.\n\nOnly used if the sustainer is looped.")]
	[DefaultFloatRange(9999f, 9999f)]
	public FloatRange sustainLoopDurationRange = new FloatRange(9999f, 9999f);

	[EditSliderRange(-2f, 2f)]
	[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
	[LoadAlias("sustainInterval")]
	[DefaultFloatRange(0f, 0f)]
	public FloatRange sustainIntervalRange = FloatRange.Zero;

	[Description("The time between when one sample ends and the next starts.\n\nSet to negative if you wish samples to overlap.")]
	[LoadAlias("sustainIntervalFactorByAggregateSize")]
	public SimpleCurve sustainIntervalFactorByAggregateSize;

	[EditSliderRange(0f, 2f)]
	[Description("The fade-in time of each sample. The sample will start at 0 volume and fade in over this number of seconds.")]
	[DefaultValue(0f)]
	public float sustainAttack;

	[Description("Skip the attack on the first sustainer sample.")]
	[DefaultValue(true)]
	public bool sustainSkipFirstAttack = true;

	[EditSliderRange(0f, 2f)]
	[Description("The fade-out time of each sample. At this number of seconds before the sample ends, it will start fading out. Its volume will be zero at the moment it finishes fading out.")]
	[DefaultValue(0f)]
	public float sustainRelease;

	[Description("Whether to start the sample at a random point in the sound.")]
	[DefaultValue(false)]
	public bool randomStartPoint;

	[DefaultValue(true)]
	public bool canVacuumDampen = true;

	[Unsaved(false)]
	public SoundDef parentDef;

	[Unsaved(false)]
	private List<ResolvedGrain> resolvedGrains = new List<ResolvedGrain>();

	[Unsaved(false)]
	private ResolvedGrain lastPlayedResolvedGrain;

	[Unsaved(false)]
	private int numToAvoid;

	[Unsaved(false)]
	private int distinctResolvedGrainsCount;

	[Unsaved(false)]
	private Queue<ResolvedGrain> recentlyPlayedResolvedGrains = new Queue<ResolvedGrain>();

	public FloatRange Duration
	{
		get
		{
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			foreach (ResolvedGrain resolvedGrain in resolvedGrains)
			{
				num = Mathf.Min(num, resolvedGrain.duration);
				num2 = Mathf.Max(num2, resolvedGrain.duration);
			}
			if (num == float.PositiveInfinity || num2 == float.NegativeInfinity)
			{
				return new FloatRange(0f, 0f);
			}
			return new FloatRange(num / Mathf.Abs(pitchRange.min), num2 / Mathf.Abs(pitchRange.max));
		}
	}

	public virtual void TryPlay(SoundInfo info)
	{
		if (resolvedGrains.Count == 0)
		{
			Log.Error("Cannot play " + parentDef?.ToString() + " (subSound " + this?.ToString() + "_: No resolved grains.");
		}
		else
		{
			if (!Find.SoundRoot.oneShotManager.CanAddPlayingOneShot(parentDef, info) || (Current.Game != null && !gameSpeedRange.Includes((int)Find.TickManager.CurTimeSpeed)))
			{
				return;
			}
			ResolvedGrain resolvedGrain = RandomizedResolvedGrain();
			if (resolvedGrain is ResolvedGrain_Clip resolvedGrain_Clip)
			{
				if (SampleOneShot.TryMakeAndPlay(this, resolvedGrain_Clip.clip, info) == null)
				{
					return;
				}
				SoundSlotManager.Notify_Played(parentDef.slot, resolvedGrain_Clip.clip.length);
			}
			Notify_GrainPlayed(resolvedGrain);
		}
	}

	public void Notify_GrainPlayed(ResolvedGrain chosenGrain)
	{
		if (distinctResolvedGrainsCount <= 1)
		{
			return;
		}
		if (repeatMode == RepeatSelectMode.NeverLastHalf)
		{
			while (recentlyPlayedResolvedGrains.Count >= numToAvoid)
			{
				recentlyPlayedResolvedGrains.Dequeue();
			}
			if (recentlyPlayedResolvedGrains.Count < numToAvoid)
			{
				recentlyPlayedResolvedGrains.Enqueue(chosenGrain);
			}
		}
		else if (repeatMode == RepeatSelectMode.NeverTwice)
		{
			lastPlayedResolvedGrain = chosenGrain;
		}
	}

	public bool IsSameOrHasSameTag(SubSoundDef other)
	{
		if (this == other)
		{
			return true;
		}
		if (tag.NullOrEmpty() || other.tag.NullOrEmpty())
		{
			return false;
		}
		return tag == other.tag;
	}

	public ResolvedGrain RandomizedResolvedGrain()
	{
		ResolvedGrain chosenGrain = null;
		while (true)
		{
			chosenGrain = resolvedGrains.RandomElement();
			if (distinctResolvedGrainsCount <= 1)
			{
				break;
			}
			if (repeatMode == RepeatSelectMode.NeverLastHalf)
			{
				if (!recentlyPlayedResolvedGrains.Any((ResolvedGrain g) => g.Equals(chosenGrain)))
				{
					break;
				}
			}
			else if (repeatMode != RepeatSelectMode.NeverTwice || !chosenGrain.Equals(lastPlayedResolvedGrain))
			{
				break;
			}
		}
		return chosenGrain;
	}

	public float RandomizedVolume()
	{
		return volumeRange.RandomInRange / 100f;
	}

	public override void ResolveReferences()
	{
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			resolvedGrains.Clear();
			foreach (AudioGrain grain in grains)
			{
				foreach (ResolvedGrain resolvedGrain in grain.GetResolvedGrains())
				{
					resolvedGrains.Add(resolvedGrain);
				}
			}
			distinctResolvedGrainsCount = resolvedGrains.Distinct().Count();
			numToAvoid = Mathf.FloorToInt((float)distinctResolvedGrainsCount / 2f);
			if (distinctResolvedGrainsCount >= 6)
			{
				numToAvoid++;
			}
			if (resolvedGrains.Count == 0)
			{
				Log.Error($"{parentDef.defName} SubSound {this} has no resolvedGrains.");
			}
		});
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

	public override string ToString()
	{
		return name;
	}
}
