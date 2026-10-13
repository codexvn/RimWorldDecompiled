using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002DCA RID: 11722
	public class CompPsylinkable : ThingComp
	{
		// Token: 0x17002A11 RID: 10769
		// (get) Token: 0x060107C1 RID: 67521 RVA: 0x004C157F File Offset: 0x004BF77F
		public CompProperties_Psylinkable Props
		{
			get
			{
				return (CompProperties_Psylinkable)this.props;
			}
		}

		// Token: 0x17002A12 RID: 10770
		// (get) Token: 0x060107C2 RID: 67522 RVA: 0x004C158C File Offset: 0x004BF78C
		public CompSpawnSubplant CompSubplant
		{
			get
			{
				return this.parent.TryGetComp<CompSpawnSubplant>();
			}
		}

		// Token: 0x060107C3 RID: 67523 RVA: 0x004C159C File Offset: 0x004BF79C
		private IEnumerable<Pawn> GetPawnsThatCanPsylink(int level = -1)
		{
			return from p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists
			where this.Props.requiredFocus.CanPawnUse(p) && this.GetRequiredPlantCount(p) <= this.CompSubplant.SubplantsForReading.Count && (level == -1 || p.GetPsylinkLevel() == level)
			select p;
		}

		// Token: 0x060107C4 RID: 67524 RVA: 0x004C15D3 File Offset: 0x004BF7D3
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.CompSubplant.onGrassGrown = new Action(this.OnGrassGrown);
		}

		// Token: 0x060107C5 RID: 67525 RVA: 0x004C15EC File Offset: 0x004BF7EC
		private void OnGrassGrown()
		{
			bool flag = false;
			foreach (Pawn item in this.GetPawnsThatCanPsylink(-1))
			{
				if (!this.pawnsThatCanPsylinkLastGrassGrow.Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
				string text = "";
				for (int i = 0; i < this.Props.requiredSubplantCountPerPsylinkLevel.Count; i++)
				{
					IEnumerable<string> enumerable = from p in this.GetPawnsThatCanPsylink(i)
					select p.LabelShort;
					if (enumerable.Count<string>() > 0)
					{
						text = string.Concat(new string[]
						{
							text,
							"- " + "Level".Translate().CapitalizeFirst() + " ",
							(i + 1).ToString(),
							": ",
							this.Props.requiredSubplantCountPerPsylinkLevel[i].ToString(),
							" ",
							compSpawnSubplant.Props.subplant.label,
							" (",
							enumerable.ToCommaList(false, false),
							")\n"
						});
					}
				}
				Find.LetterStack.ReceiveLetter(this.Props.enoughPlantsLetterLabel, this.Props.enoughPlantsLetterText.Formatted(compSpawnSubplant.SubplantsForReading.Count, text.TrimEndNewlines()), LetterDefOf.NeutralEvent, new LookTargets(this.GetPawnsThatCanPsylink(-1)), null, null, null, null, 0, true);
			}
			this.pawnsThatCanPsylinkLastGrassGrow.Clear();
			this.pawnsThatCanPsylinkLastGrassGrow.AddRange(this.GetPawnsThatCanPsylink(-1));
		}

		// Token: 0x060107C6 RID: 67526 RVA: 0x004C17EC File Offset: 0x004BF9EC
		public int GetRequiredPlantCount(Pawn pawn)
		{
			int psylinkLevel = pawn.GetPsylinkLevel();
			if (this.parent.TryGetComp<CompSpawnSubplant>() == null)
			{
				Log.Warning("CompPsylinkable with requiredSubplantCountPerPsylinkLevel set on a Thing without CompSpawnSubplant!");
				return -1;
			}
			int result;
			if (this.Props.requiredSubplantCountPerPsylinkLevel.Count <= psylinkLevel)
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel.Last<int>();
			}
			else
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel[psylinkLevel];
			}
			return result;
		}

		// Token: 0x060107C7 RID: 67527 RVA: 0x004C1854 File Offset: 0x004BFA54
		public AcceptanceReport CanPsylink(Pawn pawn, LocalTargetInfo? knownSpot = null, bool checkSpot = true)
		{
			if (pawn.Dead || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int requiredPlantCount = this.GetRequiredPlantCount(pawn);
			if (requiredPlantCount == -1)
			{
				return false;
			}
			if (!this.Props.requiredFocus.CanPawnUse(pawn))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedFocus".Translate(this.Props.requiredFocus.label));
			}
			if (pawn.GetPsylinkLevel() >= pawn.GetMaxPsylinkLevel())
			{
				return new AcceptanceReport("BeginLinkingRitualMaxPsylinkLevel".Translate());
			}
			if (!pawn.Map.reservationManager.CanReserve(pawn, this.parent, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(this.parent, pawn, null);
				return new AcceptanceReport((pawn2 == null) ? "Reserved".Translate() : "ReservedBy".Translate(pawn.LabelShort, pawn2));
			}
			if (compSpawnSubplant.SubplantsForReading.Count < requiredPlantCount)
			{
				return new AcceptanceReport("BeginLinkingRitualNeedSubplants".Translate(requiredPlantCount.ToString(), compSpawnSubplant.Props.subplant.label, compSpawnSubplant.SubplantsForReading.Count.ToString()));
			}
			if (checkSpot)
			{
				LocalTargetInfo localTargetInfo;
				if (knownSpot != null)
				{
					if (!this.CanUseSpot(pawn, knownSpot.Value))
					{
						return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
					}
				}
				else if (!this.TryFindLinkSpot(pawn, out localTargetInfo))
				{
					return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060107C8 RID: 67528 RVA: 0x004C1A24 File Offset: 0x004BFC24
		public bool TryFindLinkSpot(Pawn pawn, out LocalTargetInfo spot)
		{
			spot = MeditationUtility.FindMeditationSpot(pawn).spot;
			if (this.CanUseSpot(pawn, spot))
			{
				return true;
			}
			int num = GenRadial.NumCellsInRadius(2.9f);
			int num2 = GenRadial.NumCellsInRadius(3.9f);
			for (int i = num; i < num2; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (this.CanUseSpot(pawn, c))
				{
					spot = c;
					return true;
				}
			}
			spot = IntVec3.Zero;
			return false;
		}

		// Token: 0x060107C9 RID: 67529 RVA: 0x004C1AC0 File Offset: 0x004BFCC0
		private bool CanUseSpot(Pawn pawn, LocalTargetInfo spot)
		{
			IntVec3 cell = spot.Cell;
			return cell.DistanceTo(this.parent.Position) <= 3.9f && cell.Standable(this.parent.Map) && GenSight.LineOfSight(cell, this.parent.Position, this.parent.Map) && pawn.CanReach(spot, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x060107CA RID: 67530 RVA: 0x004C1B35 File Offset: 0x004BFD35
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (pawn.Dead || pawn.Drafted)
			{
				yield break;
			}
			string text = "BeginLinkingRitualFloatMenu".Translate();
			AcceptanceReport acceptanceReport = this.CanPsylink(pawn, null, true);
			if (!acceptanceReport.Accepted && !string.IsNullOrWhiteSpace(acceptanceReport.Reason))
			{
				text = text + ": " + acceptanceReport.Reason;
			}
			yield return new FloatMenuOption(text, delegate()
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
					Find.WindowStack.Add(precept_Ritual.GetRitualBeginWindow(this.parent, null, null, null, null, pawn));
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			{
				Disabled = !acceptanceReport.Accepted
			};
			yield break;
		}

		// Token: 0x060107CB RID: 67531 RVA: 0x004C1B4C File Offset: 0x004BFD4C
		public void FinishLinkingRitual(Pawn pawn, int plantsToKeep)
		{
			if (!ModLister.CheckRoyalty("Psylinkable"))
			{
				return;
			}
			FleckMaker.Static(this.parent.Position, pawn.Map, FleckDefOf.PsycastAreaEffect, 10f);
			SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(this.parent));
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int num = this.GetRequiredPlantCount(pawn) - plantsToKeep;
			List<Thing> list = (from p in compSpawnSubplant.SubplantsForReading
			orderby p.Position.DistanceTo(this.parent.Position) descending
			select p).ToList<Thing>();
			int num2 = 0;
			while (num2 < num && num2 < list.Count)
			{
				list[num2].Destroy(DestroyMode.Vanish);
				num2++;
			}
			compSpawnSubplant.Cleanup();
			pawn.ChangePsylinkLevel(1, true);
			Find.History.Notify_PsylinkAvailable();
		}

		// Token: 0x060107CC RID: 67532 RVA: 0x004C1C10 File Offset: 0x004BFE10
		public override void PostExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsThatCanPsylinkLastGrassGrow, "pawnsThatCanPsylinkLastGrassGrow", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawnsThatCanPsylinkLastGrassGrow.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0400ABEF RID: 44015
		private List<Pawn> pawnsThatCanPsylinkLastGrassGrow = new List<Pawn>();

		// Token: 0x0400ABF0 RID: 44016
		public const float MaxDistance = 3.9f;
	}
}
