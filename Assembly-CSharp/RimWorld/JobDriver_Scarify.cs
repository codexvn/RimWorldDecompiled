using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011B6 RID: 4534
	public class JobDriver_Scarify : JobDriver
	{
		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x06006EDF RID: 28383 RVA: 0x00228BC8 File Offset: 0x00226DC8
		protected Pawn Target
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06006EE0 RID: 28384 RVA: 0x00228BF0 File Offset: 0x00226DF0
		public static bool AvailableOnNow(Pawn pawn, BodyPartRecord part = null)
		{
			return pawn.RaceProps.Humanlike && (Faction.OfPlayerSilentFail == null || Faction.OfPlayer.ideos.AnyPreceptWithRequiredScars()) && (part == null || (!pawn.health.WouldDieAfterAddingHediff(HediffDefOf.Scarification, part, 1f) && !pawn.health.WouldLosePartAfterAddingHediff(HediffDefOf.Scarification, part, 1f)));
		}

		// Token: 0x06006EE1 RID: 28385 RVA: 0x00228C5A File Offset: 0x00226E5A
		public static IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn)
		{
			foreach (BodyPartRecord bodyPartRecord in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				if (bodyPartRecord.def.canScarify)
				{
					yield return bodyPartRecord;
				}
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006EE2 RID: 28386 RVA: 0x00228C6C File Offset: 0x00226E6C
		public static void Scarify(Pawn pawn, BodyPartRecord part)
		{
			if (!ModLister.CheckIdeology("Scarification"))
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation = lord.LordJob as LordJob_Ritual_Mutilation;
				if (lordJob_Ritual_Mutilation != null)
				{
					lordJob_Ritual_Mutilation.mutilatedPawns.Add(pawn);
				}
			}
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Scarification, pawn, part);
			HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff.TryGetComp<HediffComp_GetsPermanent>();
			hediffComp_GetsPermanent.IsPermanent = true;
			hediffComp_GetsPermanent.SetPainCategory(JobDriver_Scarify.InjuryPainCategories.RandomElementByWeight((HealthTuning.PainCategoryWeighted e) => e.weight).category);
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x06006EE3 RID: 28387 RVA: 0x00228D0F File Offset: 0x00226F0F
		public static void CreateHistoryEventDef(Pawn pawn)
		{
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotScarified, pawn.Named(HistoryEventArgsNames.Doer)), true);
		}

		// Token: 0x06006EE4 RID: 28388 RVA: 0x00228D31 File Offset: 0x00226F31
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006EE5 RID: 28389 RVA: 0x00228D54 File Offset: 0x00226F54
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Scarify"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Pawn target = this.Target;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false);
			yield return Toils_General.Wait(35, TargetIndex.None);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			Func<BodyPartRecord, bool> <>9__1;
			toil.initAction = delegate()
			{
				IEnumerable<BodyPartRecord> partsToApplyOn = JobDriver_Scarify.GetPartsToApplyOn(target);
				Func<BodyPartRecord, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((BodyPartRecord part) => JobDriver_Scarify.AvailableOnNow(target, part)));
				}
				BodyPartRecord part2;
				if (!partsToApplyOn.Where(predicate).TryRandomElement(out part2) && !JobDriver_Scarify.GetPartsToApplyOn(target).TryRandomElement(out part2))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Errored, true, true);
					Log.Error("Failed to find body part to scarify");
				}
				JobDriver_Scarify.Scarify(target, part2);
				JobDriver_Scarify.CreateHistoryEventDef(target);
				SoundDefOf.Execute_Cut.PlayOneShot(target);
				if (target.RaceProps.BloodDef != null)
				{
					CellRect cellRect = new CellRect(target.PositionHeld.x - 1, target.PositionHeld.z - 1, 3, 3);
					for (int i = 0; i < 3; i++)
					{
						IntVec3 randomCell = cellRect.RandomCell;
						if (randomCell.InBounds(this.Map) && GenSight.LineOfSight(randomCell, target.PositionHeld, this.Map))
						{
							FilthMaker.TryMakeFilth(randomCell, target.MapHeld, target.RaceProps.BloodDef, target.LabelIndefinite(), 1, FilthSourceFlags.None);
						}
					}
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil;
			yield break;
		}

		// Token: 0x04004D57 RID: 19799
		private const TargetIndex TargetPawnIndex = TargetIndex.A;

		// Token: 0x04004D58 RID: 19800
		public static readonly HealthTuning.PainCategoryWeighted[] InjuryPainCategories = new HealthTuning.PainCategoryWeighted[]
		{
			new HealthTuning.PainCategoryWeighted(PainCategory.LowPain, 0.8f),
			new HealthTuning.PainCategoryWeighted(PainCategory.MediumPain, 0.2f)
		};
	}
}
