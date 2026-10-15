using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompEggLayer : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__20 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompEggLayer _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__20(int _003C_003E1__state)
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
			CompEggLayer CS_0024_003C_003E8__locals1 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: LayEgg",
						action = delegate
						{
							CS_0024_003C_003E8__locals1.eggProgress = 1f;
						}
					};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__20(0)
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

	private const int EggTickInterval = 2500;

	private float eggProgress;

	private int fertilizationCount;

	private Pawn fertilizedBy;

	private bool Active
	{
		get
		{
			Pawn pawn = parent as Pawn;
			if (Props.eggLayFemaleOnly && pawn != null && pawn.gender != Gender.Female)
			{
				return false;
			}
			if (pawn != null && !pawn.ageTracker.CurLifeStage.milkable)
			{
				return false;
			}
			if (pawn.Sterile())
			{
				return false;
			}
			if (ModsConfig.AnomalyActive && pawn.IsShambler)
			{
				return false;
			}
			return true;
		}
	}

	public bool CanLayNow
	{
		get
		{
			if (Active)
			{
				return eggProgress >= 1f;
			}
			return false;
		}
	}

	public bool FullyFertilized => fertilizationCount >= Props.eggFertilizationCountMax;

	private bool ProgressStoppedBecauseUnfertilized
	{
		get
		{
			if (Props.eggProgressUnfertilizedMax < 1f && fertilizationCount == 0)
			{
				return eggProgress >= Props.eggProgressUnfertilizedMax;
			}
			return false;
		}
	}

	public CompProperties_EggLayer Props => (CompProperties_EggLayer)props;

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref eggProgress, "eggProgress", 0f);
		Scribe_Values.Look(ref fertilizationCount, "fertilizationCount", 0);
		Scribe_References.Look(ref fertilizedBy, "fertilizedBy");
	}

	public override void CompTick()
	{
		if (parent.IsHashIntervalTick(2500) && Active)
		{
			float num = 2500f / (Props.eggLayIntervalDays * 60000f);
			if (parent is Pawn pawn)
			{
				num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
			}
			eggProgress += num;
			if (eggProgress > 1f)
			{
				eggProgress = 1f;
			}
			if (ProgressStoppedBecauseUnfertilized)
			{
				eggProgress = Props.eggProgressUnfertilizedMax;
			}
		}
	}

	public void Fertilize(Pawn male)
	{
		fertilizationCount = Props.eggFertilizationCountMax;
		fertilizedBy = male;
	}

	public ThingDef NextEggType()
	{
		if (fertilizationCount > 0)
		{
			return Props.eggFertilizedDef;
		}
		return Props.eggUnfertilizedDef;
	}

	public virtual Thing ProduceEgg()
	{
		if (!Active)
		{
			Log.Error("LayEgg while not Active: " + parent);
		}
		eggProgress = 0f;
		int randomInRange = Props.eggCountRange.RandomInRange;
		if (randomInRange == 0)
		{
			return null;
		}
		Thing thing = null;
		if (fertilizationCount > 0)
		{
			thing = ThingMaker.MakeThing(Props.eggFertilizedDef);
			fertilizationCount = Mathf.Max(0, fertilizationCount - randomInRange);
		}
		else
		{
			thing = ThingMaker.MakeThing(Props.eggUnfertilizedDef);
		}
		thing.stackCount = randomInRange;
		CompHatcher compHatcher = thing.TryGetComp<CompHatcher>();
		if (compHatcher != null)
		{
			compHatcher.hatcheeFaction = parent.Faction;
			if (parent is Pawn hatcheeParent)
			{
				compHatcher.hatcheeParent = hatcheeParent;
			}
			if (fertilizedBy != null)
			{
				compHatcher.otherParent = fertilizedBy;
			}
		}
		return thing;
	}

	public override string CompInspectStringExtra()
	{
		if (!Active)
		{
			return null;
		}
		string text = "EggProgress".Translate() + ": " + eggProgress.ToStringPercent();
		if (fertilizationCount > 0)
		{
			text += "\n" + "Fertilized".Translate();
		}
		else if (ProgressStoppedBecauseUnfertilized)
		{
			text += "\n" + "ProgressStoppedUntilFertilized".Translate();
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__20))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}
}
