using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class CompPsylinkable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public Pawn pawn;

		public CompPsylinkable _003C_003E4__this;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			Precept_Ritual precept_Ritual = null;
			for (int i = 0; i < pawn.Ideo.PreceptsListForReading.Count; i++)
			{
				if (pawn.Ideo.PreceptsListForReading[i].def == PreceptDefOf.AnimaTreeLinking)
				{
					precept_Ritual = (Precept_Ritual)pawn.Ideo.PreceptsListForReading[i];
					break;
				}
			}
			if (precept_Ritual != null)
			{
				Find.WindowStack.Add(precept_Ritual.GetRitualBeginWindow(_003C_003E4__this.parent, null, null, null, null, pawn));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__13 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		public CompPsylinkable _003C_003E4__this;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CCompFloatMenuOptions_003Ed__13(int _003C_003E1__state)
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
			CompPsylinkable compPsylinkable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass13_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass13_0
				{
					pawn = pawn,
					_003C_003E4__this = _003C_003E4__this
				};
				if (CS_0024_003C_003E8__locals8.pawn.Dead || CS_0024_003C_003E8__locals8.pawn.Drafted)
				{
					return false;
				}
				string text = "BeginLinkingRitualFloatMenu".Translate();
				AcceptanceReport acceptanceReport = compPsylinkable.CanPsylink(CS_0024_003C_003E8__locals8.pawn, null);
				if (!acceptanceReport.Accepted && !string.IsNullOrWhiteSpace(acceptanceReport.Reason))
				{
					text = text + ": " + acceptanceReport.Reason;
				}
				_003C_003E2__current = new FloatMenuOption(text, delegate
				{
					Precept_Ritual precept_Ritual = null;
					for (int i = 0; i < CS_0024_003C_003E8__locals8.pawn.Ideo.PreceptsListForReading.Count; i++)
					{
						if (CS_0024_003C_003E8__locals8.pawn.Ideo.PreceptsListForReading[i].def == PreceptDefOf.AnimaTreeLinking)
						{
							precept_Ritual = (Precept_Ritual)CS_0024_003C_003E8__locals8.pawn.Ideo.PreceptsListForReading[i];
							break;
						}
					}
					if (precept_Ritual != null)
					{
						Find.WindowStack.Add(precept_Ritual.GetRitualBeginWindow(CS_0024_003C_003E8__locals8._003C_003E4__this.parent, null, null, null, null, CS_0024_003C_003E8__locals8.pawn));
					}
				})
				{
					Disabled = !acceptanceReport.Accepted
				};
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__13 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.pawn = _003C_003E3__pawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private List<Pawn> pawnsThatCanPsylinkLastGrassGrow = new List<Pawn>();

	public const float MaxDistance = 3.9f;

	public CompProperties_Psylinkable Props => (CompProperties_Psylinkable)props;

	public CompSpawnSubplant CompSubplant => parent.TryGetComp<CompSpawnSubplant>();

	private IEnumerable<Pawn> GetPawnsThatCanPsylink(int level = -1)
	{
		return PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Where((Pawn p) => Props.requiredFocus.CanPawnUse(p) && GetRequiredPlantCount(p) <= CompSubplant.SubplantsForReading.Count && (level == -1 || p.GetPsylinkLevel() == level));
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		CompSubplant.onGrassGrown = OnGrassGrown;
	}

	private void OnGrassGrown()
	{
		bool flag = false;
		foreach (Pawn item in GetPawnsThatCanPsylink())
		{
			if (!pawnsThatCanPsylinkLastGrassGrow.Contains(item))
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			CompSpawnSubplant compSpawnSubplant = parent.TryGetComp<CompSpawnSubplant>();
			string text = "";
			for (int i = 0; i < Props.requiredSubplantCountPerPsylinkLevel.Count; i++)
			{
				IEnumerable<string> enumerable = from p in GetPawnsThatCanPsylink(i)
					select p.LabelShort;
				if (enumerable.Count() > 0)
				{
					text = string.Concat(text, "- " + "Level".Translate().CapitalizeFirst() + " ", (i + 1).ToString(), ": ", Props.requiredSubplantCountPerPsylinkLevel[i].ToString(), " ", compSpawnSubplant.Props.subplant.label, " (", enumerable.ToCommaList(), ")\n");
				}
			}
			Find.LetterStack.ReceiveLetter(Props.enoughPlantsLetterLabel, Props.enoughPlantsLetterText.Formatted(compSpawnSubplant.SubplantsForReading.Count, text.TrimEndNewlines()), LetterDefOf.NeutralEvent, new LookTargets(GetPawnsThatCanPsylink()));
		}
		pawnsThatCanPsylinkLastGrassGrow.Clear();
		pawnsThatCanPsylinkLastGrassGrow.AddRange(GetPawnsThatCanPsylink());
	}

	public int GetRequiredPlantCount(Pawn pawn)
	{
		int psylinkLevel = pawn.GetPsylinkLevel();
		if (parent.TryGetComp<CompSpawnSubplant>() == null)
		{
			Log.Warning("CompPsylinkable with requiredSubplantCountPerPsylinkLevel set on a Thing without CompSpawnSubplant!");
			return -1;
		}
		if (Props.requiredSubplantCountPerPsylinkLevel.Count <= psylinkLevel)
		{
			return Props.requiredSubplantCountPerPsylinkLevel.Last();
		}
		return Props.requiredSubplantCountPerPsylinkLevel[psylinkLevel];
	}

	public AcceptanceReport CanPsylink(Pawn pawn, LocalTargetInfo? knownSpot = null, bool checkSpot = true)
	{
		if (pawn.Dead || pawn.Faction != Faction.OfPlayer)
		{
			return false;
		}
		CompSpawnSubplant compSpawnSubplant = parent.TryGetComp<CompSpawnSubplant>();
		int requiredPlantCount = GetRequiredPlantCount(pawn);
		if (requiredPlantCount == -1)
		{
			return false;
		}
		if (!Props.requiredFocus.CanPawnUse(pawn))
		{
			return new AcceptanceReport("BeginLinkingRitualNeedFocus".Translate(Props.requiredFocus.label));
		}
		if (pawn.GetPsylinkLevel() >= pawn.GetMaxPsylinkLevel())
		{
			return new AcceptanceReport("BeginLinkingRitualMaxPsylinkLevel".Translate());
		}
		if (!pawn.Map.reservationManager.CanReserve(pawn, parent))
		{
			Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(parent, pawn);
			return new AcceptanceReport((pawn2 == null) ? "Reserved".Translate() : "ReservedBy".Translate(pawn.LabelShort, pawn2));
		}
		if (compSpawnSubplant.SubplantsForReading.Count < requiredPlantCount)
		{
			return new AcceptanceReport("BeginLinkingRitualNeedSubplants".Translate(requiredPlantCount.ToString(), compSpawnSubplant.Props.subplant.label, compSpawnSubplant.SubplantsForReading.Count.ToString()));
		}
		if (checkSpot)
		{
			LocalTargetInfo spot;
			if (knownSpot.HasValue)
			{
				if (!CanUseSpot(pawn, knownSpot.Value))
				{
					return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
				}
			}
			else if (!TryFindLinkSpot(pawn, out spot))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
			}
		}
		return AcceptanceReport.WasAccepted;
	}

	public bool TryFindLinkSpot(Pawn pawn, out LocalTargetInfo spot)
	{
		spot = MeditationUtility.FindMeditationSpot(pawn).spot;
		if (CanUseSpot(pawn, spot))
		{
			return true;
		}
		int num = GenRadial.NumCellsInRadius(2.9f);
		int num2 = GenRadial.NumCellsInRadius(3.9f);
		for (int i = num; i < num2; i++)
		{
			IntVec3 intVec = parent.Position + GenRadial.RadialPattern[i];
			if (CanUseSpot(pawn, intVec))
			{
				spot = intVec;
				return true;
			}
		}
		spot = IntVec3.Zero;
		return false;
	}

	private bool CanUseSpot(Pawn pawn, LocalTargetInfo spot)
	{
		IntVec3 cell = spot.Cell;
		if (cell.DistanceTo(parent.Position) > 3.9f)
		{
			return false;
		}
		if (!cell.Standable(parent.Map))
		{
			return false;
		}
		if (!GenSight.LineOfSight(cell, parent.Position, parent.Map))
		{
			return false;
		}
		if (!pawn.CanReach(spot, PathEndMode.OnCell, Danger.Deadly))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__13))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__13(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn
		};
	}

	public void FinishLinkingRitual(Pawn pawn, int plantsToKeep)
	{
		if (ModLister.CheckRoyalty("Psylinkable"))
		{
			FleckMaker.Static(parent.Position, pawn.Map, FleckDefOf.PsycastAreaEffect, 10f);
			SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(parent));
			CompSpawnSubplant compSpawnSubplant = parent.TryGetComp<CompSpawnSubplant>();
			int num = GetRequiredPlantCount(pawn) - plantsToKeep;
			List<Thing> list = compSpawnSubplant.SubplantsForReading.OrderByDescending((Thing p) => p.Position.DistanceTo(parent.Position)).ToList();
			for (int i = 0; i < num && i < list.Count; i++)
			{
				list[i].Destroy();
			}
			compSpawnSubplant.Cleanup();
			pawn.ChangePsylinkLevel(1);
			Find.History.Notify_PsylinkAvailable();
		}
	}

	public override void PostExposeData()
	{
		Scribe_Collections.Look(ref pawnsThatCanPsylinkLastGrassGrow, "pawnsThatCanPsylinkLastGrassGrow", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawnsThatCanPsylinkLastGrassGrow.RemoveAll((Pawn x) => x == null);
		}
	}
}
