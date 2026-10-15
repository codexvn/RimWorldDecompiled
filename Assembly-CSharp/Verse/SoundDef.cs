using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse;

public class SoundDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__23 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SoundDef _003C_003E4__this;

		private List<SoundDef> _003Cdefs_003E5__2;

		private int _003Ci_003E5__3;

		private SubSoundDef _003Cs_003E5__4;

		private IEnumerator<string> _003C_003E7__wrap4;

		private int _003Cj_003E5__6;

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
		public _003CConfigErrors_003Ed__23(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 8)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cdefs_003E5__2 = null;
			_003Cs_003E5__4 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				SoundDef soundDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (soundDef.slot != "" && !soundDef.HasSubSoundsOnCamera)
					{
						_003C_003E2__current = "Sound slots only work for on-camera sounds.";
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0084;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0084;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00b5;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00e5;
				case 4:
					_003C_003E1__state = -1;
					goto IL_010e;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0146;
				case 6:
					_003C_003E1__state = -1;
					goto IL_017b;
				case 7:
					_003C_003E1__state = -1;
					goto IL_01ce;
				case 8:
					_003C_003E1__state = -3;
					goto IL_02c8;
				case 9:
					{
						_003C_003E1__state = -1;
						goto IL_03c9;
					}
					IL_017b:
					if (!soundDef.sustain)
					{
						_003Ci_003E5__3 = 0;
						goto IL_01de;
					}
					goto IL_01f1;
					IL_03d9:
					if (_003Cj_003E5__6 < _003Cdefs_003E5__2[_003Ci_003E5__3].eventNames.Count)
					{
						if (_003Cdefs_003E5__2[_003Ci_003E5__3].eventNames[_003Cj_003E5__6] == soundDef.defName)
						{
							_003C_003E2__current = soundDef.defName + " is also defined in the eventNames list for " + _003Cdefs_003E5__2[_003Ci_003E5__3];
							_003C_003E1__state = 9;
							return true;
						}
						goto IL_03c9;
					}
					goto IL_03ff;
					IL_0084:
					if (soundDef.HasSubSoundsInWorld && soundDef.context != SoundContext.MapOnly)
					{
						_003C_003E2__current = "Sounds with non-on-camera subsounds should use MapOnly context.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00b5;
					IL_01de:
					if (_003Ci_003E5__3 < soundDef.subSounds.Count)
					{
						if (soundDef.subSounds[_003Ci_003E5__3].startDelayRange.TrueMax > 0.001f)
						{
							_003C_003E2__current = "startDelayRange is only supported on sustainers.";
							_003C_003E1__state = 7;
							return true;
						}
						goto IL_01ce;
					}
					goto IL_01f1;
					IL_0312:
					_003Cdefs_003E5__2 = DefDatabase<SoundDef>.AllDefsListForReading;
					_003Ci_003E5__3 = 0;
					goto IL_040f;
					IL_00b5:
					if (soundDef.priorityMode == VoicePriorityMode.PrioritizeNewest && soundDef.sustain)
					{
						_003C_003E2__current = "PrioritizeNewest is not supported with sustainers.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00e5;
					IL_040f:
					if (_003Ci_003E5__3 < _003Cdefs_003E5__2.Count)
					{
						if (!_003Cdefs_003E5__2[_003Ci_003E5__3].eventNames.NullOrEmpty())
						{
							_003Cj_003E5__6 = 0;
							goto IL_03d9;
						}
						goto IL_03ff;
					}
					return false;
					IL_01ce:
					_003Ci_003E5__3++;
					goto IL_01de;
					IL_00e5:
					if (soundDef.maxVoices < 1)
					{
						_003C_003E2__current = "Max voices is less than 1.";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_010e;
					IL_01f1:
					if (!soundDef.subSounds.NullOrEmpty())
					{
						_003Ci_003E5__3 = 0;
						goto IL_02fc;
					}
					goto IL_0312;
					IL_010e:
					if (!soundDef.sustain && (soundDef.sustainStartSound != null || soundDef.sustainStopSound != null))
					{
						_003C_003E2__current = "Sustainer start and end sounds only work with sounds defined as sustainers.";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0146;
					IL_03c9:
					_003Cj_003E5__6++;
					goto IL_03d9;
					IL_02fc:
					if (_003Ci_003E5__3 < soundDef.subSounds.Count)
					{
						_003Cs_003E5__4 = soundDef.subSounds[_003Ci_003E5__3];
						_003C_003E7__wrap4 = _003Cs_003E5__4.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_02c8;
					}
					goto IL_0312;
					IL_0146:
					if (soundDef.sustainFadeoutStartSound != null && soundDef.sustainFadeoutTime <= 0f)
					{
						_003C_003E2__current = "Sustainer fadeout sound is set, but fadeout time is not set.";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_017b;
					IL_02c8:
					if (_003C_003E7__wrap4.MoveNext())
					{
						string current = _003C_003E7__wrap4.Current;
						_003C_003E2__current = $"SubSound[{_003Ci_003E5__3}] ({_003Cs_003E5__4}): {current}";
						_003C_003E1__state = 8;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = null;
					_003Cs_003E5__4 = null;
					_003Ci_003E5__3++;
					goto IL_02fc;
					IL_03ff:
					_003Ci_003E5__3++;
					goto IL_040f;
				}
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
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__23(0)
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

	[Description("If checked, this sound is a sustainer.\n\nSustainers are used for sounds with a defined beginning and end (as opposed to OneShots, which just fire at a given instant).\n\nThis value must match what the game expects from the SubSoundDef with this name.")]
	[DefaultValue(false)]
	public bool sustain;

	[Description("When the sound is allowed to play: only when the map view is active, only when the world view is active, or always (map + world + main menu).")]
	[DefaultValue(SoundContext.Any)]
	public SoundContext context;

	[Description("Event names for this sound. \n\nThe code will look up sounds to play them according to their name. If the code finds the event name it wants in this list, it will trigger this sound.\n\nThe Def name is also used as an event name. Obsolete")]
	public List<string> eventNames = new List<string>();

	[Description("For one-shots, this is the number of individual sounds from this Def than can be playing at a time.\n\n For sustainers, this is the number of sustainers that can be running with this sound (each of which can have sub-sounds). Sustainers can fade in and out as you move the camera or objects move, to keep the nearest ones audible.\n\nThis setting may not work for on-camera sounds.")]
	[DefaultValue(4)]
	public int maxVoices = 4;

	[Description("The number of instances of this sound that can play at almost exactly the same moment. Handles cases like six gunners all firing their identical guns at the same time because a target came into view of all of them at the same time. Ordinarily this would make a painfully loud sound, but you can reduce it with this.")]
	[DefaultValue(3)]
	public int maxSimultaneous = 3;

	[Description("If the system has to not play some instances of this sound because of maxVoices, this determines which ones are ignored.\n\nYou should use PrioritizeNewest for things like gunshots, so older still-playing samples are overridden by newer, more important ones.\n\nSustained sounds should usually prioritize nearest, so if a new fire starts burning nearby it can override a more distant one.")]
	[DefaultValue(VoicePriorityMode.PrioritizeNewest)]
	public VoicePriorityMode priorityMode;

	[Description("The special sound slot this sound takes. If a sound with this slot is playing, new sounds in this slot will not play.\n\nOnly works for on-camera sounds.")]
	[DefaultValue("")]
	public string slot = "";

	[LoadAlias("sustainerStartSound")]
	[Description("The name of the SoundDef that will be played when this sustainer starts.")]
	[DefaultValue("")]
	public SoundDef sustainStartSound;

	[LoadAlias("sustainerStopSound")]
	[Description("The name of the SoundDef that will be played when this sustainer ends.")]
	[DefaultValue("")]
	public SoundDef sustainStopSound;

	[Description("After a sustainer is ended, the sound will fade out over this many real-time seconds.")]
	[DefaultValue(0f)]
	public float sustainFadeoutTime;

	[LoadAlias("sustainerFadeoutStartSound")]
	[Description("The name of the SoundDef that will be played when this sustainer starts to fade out.")]
	[DefaultValue("")]
	public SoundDef sustainFadeoutStartSound;

	[Description("All the sounds that will play when this set is triggered.")]
	public List<SubSoundDef> subSounds = new List<SubSoundDef>();

	[Unsaved(false)]
	public bool isUndefined;

	[Unsaved(false)]
	public Sustainer testSustainer;

	private static Dictionary<string, SoundDef> undefinedSoundDefs = new Dictionary<string, SoundDef>();

	private static object undefinedSoundDefsLock = new object();

	private bool HasSubSoundsOnCamera
	{
		get
		{
			for (int i = 0; i < subSounds.Count; i++)
			{
				if (subSounds[i].onCamera)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool HasSubSoundsInWorld
	{
		get
		{
			for (int i = 0; i < subSounds.Count; i++)
			{
				if (!subSounds[i].onCamera)
				{
					return true;
				}
			}
			return false;
		}
	}

	public int MaxSimultaneousSamples => maxSimultaneous * subSounds.Count;

	public FloatRange Duration
	{
		get
		{
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			foreach (SubSoundDef subSound in subSounds)
			{
				num = Mathf.Min(num, subSound.Duration.min);
				num2 = Mathf.Max(num2, subSound.Duration.max);
			}
			return new FloatRange((num == float.PositiveInfinity) ? 0f : num, (num2 == float.NegativeInfinity) ? 0f : num2);
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		for (int i = 0; i < subSounds.Count; i++)
		{
			subSounds[i].parentDef = this;
			if (subSounds[i].name == "UnnamedSubSoundDef")
			{
				subSounds[i].name = defName + "_" + i;
			}
			subSounds[i].ResolveReferences();
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__23))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void DoEditWidgets(WidgetRow widgetRow)
	{
		if (testSustainer == null)
		{
			if (!widgetRow.ButtonIcon(TexButton.Play, null, null, null, null))
			{
				return;
			}
			ResolveReferences();
			SoundInfo info;
			if (HasSubSoundsInWorld)
			{
				IntVec3 mapPosition = Find.CameraDriver.MapPosition;
				info = SoundInfo.InMap(new TargetInfo(mapPosition, Find.CurrentMap), MaintenanceType.PerFrame);
				for (int i = 0; i < 5; i++)
				{
					FleckMaker.ThrowDustPuff(mapPosition, Find.CurrentMap, 1.5f);
				}
			}
			else
			{
				info = SoundInfo.OnCamera(MaintenanceType.PerFrame);
			}
			info.testPlay = true;
			if (sustain)
			{
				testSustainer = this.TrySpawnSustainer(info);
			}
			else
			{
				this.PlayOneShot(info);
			}
		}
		else
		{
			testSustainer.Maintain();
			if (widgetRow.ButtonIcon(TexButton.Stop, null, null, null, null))
			{
				testSustainer.End();
				testSustainer = null;
			}
		}
	}

	public static SoundDef Named(string defName)
	{
		SoundDef namedSilentFail = DefDatabase<SoundDef>.GetNamedSilentFail(defName);
		if (namedSilentFail != null)
		{
			return namedSilentFail;
		}
		if (!Prefs.DevMode)
		{
			lock (undefinedSoundDefsLock)
			{
				if (undefinedSoundDefs.ContainsKey(defName))
				{
					return UndefinedDefNamed(defName);
				}
			}
		}
		List<SoundDef> allDefsListForReading = DefDatabase<SoundDef>.AllDefsListForReading;
		for (int i = 0; i < allDefsListForReading.Count; i++)
		{
			if (allDefsListForReading[i].eventNames.Count <= 0)
			{
				continue;
			}
			for (int j = 0; j < allDefsListForReading[i].eventNames.Count; j++)
			{
				if (allDefsListForReading[i].eventNames[j] == defName)
				{
					return allDefsListForReading[i];
				}
			}
		}
		if (DefDatabase<SoundDef>.DefCount == 0)
		{
			Log.Warning("Tried to get SoundDef named " + defName + ", but sound defs aren't loaded yet (is it a static variable initialized before play data?).");
		}
		return UndefinedDefNamed(defName);
	}

	private static SoundDef UndefinedDefNamed(string defName)
	{
		SoundDef value;
		lock (undefinedSoundDefsLock)
		{
			if (!undefinedSoundDefs.TryGetValue(defName, out value))
			{
				value = new SoundDef();
				value.isUndefined = true;
				value.defName = defName;
				value.ResolveDefNameHash();
				undefinedSoundDefs.Add(defName, value);
			}
		}
		return value;
	}
}
