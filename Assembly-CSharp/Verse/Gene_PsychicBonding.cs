using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class Gene_PsychicBonding : Gene
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__8 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Gene_PsychicBonding _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__8(int _003C_003E1__state)
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
			Gene_PsychicBonding CS_0024_003C_003E8__locals6 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos || !CS_0024_003C_003E8__locals6.CanBondToNewPawn || !CS_0024_003C_003E8__locals6.pawn.Spawned)
				{
					break;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Bond to random pawn",
					action = delegate
					{
						if ((from x in CS_0024_003C_003E8__locals6.pawn.Map.mapPawns.SpawnedPawnsInFaction(CS_0024_003C_003E8__locals6.pawn.Faction)
							where x.RaceProps.Humanlike && x != CS_0024_003C_003E8__locals6.pawn
							select x).TryRandomElement(out var result))
						{
							CS_0024_003C_003E8__locals6.BondTo(result);
						}
					}
				};
				_003C_003E1__state = 1;
				return true;
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
			_003CGetGizmos_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__8(0)
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

	private Pawn bondedPawn;

	public bool CanBondToNewPawn
	{
		get
		{
			if (bondedPawn != null)
			{
				return false;
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicBondTorn))
			{
				return false;
			}
			if (pawn.needs?.mood?.thoughts?.memories?.GetFirstMemoryOfDef(ThoughtDefOf.PsychicBondTorn) != null)
			{
				return false;
			}
			return true;
		}
	}

	public override void PostAdd()
	{
		base.PostAdd();
		if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond) is Hediff_PsychicBond hediff_PsychicBond)
		{
			bondedPawn = (Pawn)hediff_PsychicBond.target;
		}
	}

	public override void PostRemove()
	{
		base.PostRemove();
		Notify_MyOrPartnersGeneRemoved();
	}

	public void BondTo(Pawn newBond)
	{
		if (!ModLister.CheckBiotech("Psychic bonding") || newBond == null || bondedPawn == newBond)
		{
			return;
		}
		if (bondedPawn != null)
		{
			Log.Error("Tried to bond to more than one pawn.");
			return;
		}
		bondedPawn = newBond;
		pawn.needs?.mood?.thoughts?.memories?.RemoveMemoriesOfDefIf(ThoughtDefOf.PsychicBondTorn, (Thought_Memory m) => m.otherPawn == bondedPawn);
		bondedPawn.needs?.mood?.thoughts?.memories?.RemoveMemoriesOfDefIf(ThoughtDefOf.PsychicBondTorn, (Thought_Memory m) => m.otherPawn == pawn);
		Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn);
		if (firstHediffOfDef != null)
		{
			pawn.health.RemoveHediff(firstHediffOfDef);
		}
		Hediff firstHediffOfDef2 = bondedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn);
		if (firstHediffOfDef2 != null)
		{
			bondedPawn.health.RemoveHediff(firstHediffOfDef2);
		}
		Hediff_PsychicBond hediff_PsychicBond = (Hediff_PsychicBond)HediffMaker.MakeHediff(HediffDefOf.PsychicBond, pawn);
		hediff_PsychicBond.target = bondedPawn;
		pawn.health.AddHediff(hediff_PsychicBond, null, null);
		Gene_PsychicBonding gene_PsychicBonding = bondedPawn.genes?.GetFirstGeneOfType<Gene_PsychicBonding>();
		if (gene_PsychicBonding != null)
		{
			gene_PsychicBonding.BondTo(pawn);
			return;
		}
		Hediff_PsychicBond hediff_PsychicBond2 = (Hediff_PsychicBond)HediffMaker.MakeHediff(HediffDefOf.PsychicBond, bondedPawn);
		hediff_PsychicBond2.target = pawn;
		bondedPawn.health.AddHediff(hediff_PsychicBond2, null, null);
	}

	public void RemoveBond()
	{
		if (bondedPawn == null)
		{
			return;
		}
		base.pawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.PsychicBondTorn, bondedPawn);
		bondedPawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.PsychicBondTorn, base.pawn);
		Pawn pawn = bondedPawn;
		bondedPawn = null;
		if (base.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond) is Hediff_PsychicBond hediff_PsychicBond && hediff_PsychicBond.target == pawn)
		{
			base.pawn.health.RemoveHediff(hediff_PsychicBond);
		}
		if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond) is Hediff_PsychicBond hediff)
		{
			pawn.health.RemoveHediff(hediff);
		}
		pawn.genes?.GetFirstGeneOfType<Gene_PsychicBonding>()?.RemoveBond();
		if (!base.pawn.Dead)
		{
			if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBondTorn) == null)
			{
				Hediff_PsychicBondTorn hediff_PsychicBondTorn = (Hediff_PsychicBondTorn)HediffMaker.MakeHediff(HediffDefOf.PsychicBondTorn, pawn);
				hediff_PsychicBondTorn.target = base.pawn;
				pawn.health.AddHediff(hediff_PsychicBondTorn, null, null);
			}
			if (DefDatabase<MentalBreakDef>.AllDefsListForReading.Where((MentalBreakDef d) => d.intensity == MentalBreakIntensity.Extreme && d.Worker.BreakCanOccur(base.pawn)).TryRandomElementByWeight((MentalBreakDef d) => d.Worker.CommonalityFor(base.pawn, moodCaused: true), out var result))
			{
				result.Worker.TryStart(base.pawn, "MentalStateReason_BondedHumanDeath".Translate(pawn), causedByMood: false);
			}
		}
	}

	public void Notify_MyOrPartnersGeneRemoved()
	{
		if (bondedPawn != null && bondedPawn.genes?.GetFirstGeneOfType<Gene_PsychicBonding>() == null)
		{
			Pawn pawn = bondedPawn;
			bondedPawn = null;
			if (base.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond) is Hediff_PsychicBond hediff_PsychicBond && hediff_PsychicBond.target == pawn)
			{
				base.pawn.health.RemoveHediff(hediff_PsychicBond);
			}
			if (pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond) is Hediff_PsychicBond hediff)
			{
				pawn.health.RemoveHediff(hediff);
			}
			pawn.genes?.GetFirstGeneOfType<Gene_PsychicBonding>()?.Notify_MyOrPartnersGeneRemoved();
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__8))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref bondedPawn, "bondedPawn");
	}
}
