using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompCauseGameCondition_PsychicEmanation : CompCauseGameCondition
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__15 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompCauseGameCondition_PsychicEmanation _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__15(int _003C_003E1__state)
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
			CompCauseGameCondition_PsychicEmanation CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!Prefs.DevMode || !DebugSettings.godMode)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = CS_0024_003C_003E8__locals8.gender.GetLabel(),
					action = delegate
					{
						if (CS_0024_003C_003E8__locals8.gender == Gender.Female)
						{
							CS_0024_003C_003E8__locals8.gender = Gender.Male;
						}
						else
						{
							CS_0024_003C_003E8__locals8.gender = Gender.Female;
						}
						CS_0024_003C_003E8__locals8.ReSetupAllConditions();
					},
					hotKey = KeyBindingDefOf.Misc1
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = CS_0024_003C_003E8__locals8.droneLevel.GetLabel(),
					action = delegate
					{
						CS_0024_003C_003E8__locals8.IncreaseDroneLevel();
						CS_0024_003C_003E8__locals8.ReSetupAllConditions();
					},
					hotKey = KeyBindingDefOf.Misc2
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public Gender gender;

	private int ticksToIncreaseDroneLevel;

	private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;

	public new CompProperties_CausesGameCondition_PsychicEmanation Props => (CompProperties_CausesGameCondition_PsychicEmanation)props;

	public PsychicDroneLevel Level => droneLevel;

	private bool DroneLevelIncreases => Props.droneLevelIncreaseInterval != int.MinValue;

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		gender = (Rand.Bool ? Gender.Male : Gender.Female);
		droneLevel = Props.droneLevel;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && DroneLevelIncreases && !parent.BeingTransportedOnGravship)
		{
			ticksToIncreaseDroneLevel = Props.droneLevelIncreaseInterval;
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(parent.Map);
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		if (parent.Spawned && DroneLevelIncreases && Active)
		{
			ticksToIncreaseDroneLevel--;
			if (ticksToIncreaseDroneLevel <= 0)
			{
				IncreaseDroneLevel();
				ticksToIncreaseDroneLevel = Props.droneLevelIncreaseInterval;
			}
		}
	}

	private void IncreaseDroneLevel()
	{
		if (droneLevel != PsychicDroneLevel.BadExtreme)
		{
			droneLevel++;
			TaggedString text = "LetterPsychicDroneLevelIncreased".Translate(gender.GetLabel());
			Find.LetterStack.ReceiveLetter("LetterLabelPsychicDroneLevelIncreased".Translate(), text, LetterDefOf.NegativeEvent);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(parent.Map);
			ReSetupAllConditions();
		}
	}

	protected override void SetupCondition(GameCondition condition, Map map)
	{
		base.SetupCondition(condition, map);
		GameCondition_PsychicEmanation obj = (GameCondition_PsychicEmanation)condition;
		obj.gender = gender;
		obj.level = Level;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref gender, "gender", Gender.None);
		Scribe_Values.Look(ref ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0);
		Scribe_Values.Look(ref droneLevel, "droneLevel", PsychicDroneLevel.None);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__15))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		return text + ("AffectedGender".Translate() + ": " + gender.GetLabel().CapitalizeFirst() + "\n" + "PsychicDroneLevel".Translate(droneLevel.GetLabelCap()));
	}

	public override void RandomizeSettings(Site site)
	{
		gender = (Rand.Bool ? Gender.Male : Gender.Female);
		if (site.ActualThreatPoints < 800f)
		{
			droneLevel = PsychicDroneLevel.BadLow;
		}
		else if (site.ActualThreatPoints < 2000f)
		{
			droneLevel = PsychicDroneLevel.BadMedium;
		}
		else
		{
			droneLevel = PsychicDroneLevel.BadHigh;
		}
	}
}
