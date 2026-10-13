using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200126F RID: 4719
	public class JobDriver_ExtractBioferrite : JobDriver
	{
		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x0600736E RID: 29550 RVA: 0x00229653 File Offset: 0x00227853
		private Thing Platform
		{
			get
			{
				return base.TargetThingA;
			}
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x0600736F RID: 29551 RVA: 0x00237823 File Offset: 0x00235A23
		private Pawn InnerPawn
		{
			get
			{
				Building_HoldingPlatform building_HoldingPlatform = this.Platform as Building_HoldingPlatform;
				if (building_HoldingPlatform == null)
				{
					return null;
				}
				return building_HoldingPlatform.HeldPawn;
			}
		}

		// Token: 0x06007370 RID: 29552 RVA: 0x0023783B File Offset: 0x00235A3B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Platform, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007371 RID: 29553 RVA: 0x00237863 File Offset: 0x00235A63
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate()
			{
				Pawn innerPawn = this.InnerPawn;
				if (innerPawn == null || innerPawn.Destroyed)
				{
					return true;
				}
				CompHoldingPlatformTarget compHoldingPlatformTarget = innerPawn.TryGetComp<CompHoldingPlatformTarget>();
				return compHoldingPlatformTarget != null && (compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release || !compHoldingPlatformTarget.extractBioferrite);
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			int ticks = (int)(1f / this.pawn.GetStatValue(StatDefOf.MedicalTendSpeed, true, -1) * 2000f);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, true, false, false, TargetIndex.A, PathEndMode.Touch);
			toil.activeSkill = (() => SkillDefOf.Medicine);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.ClosestTouch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Recipe_Surgery, 1f);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				Pawn innerPawn = this.InnerPawn;
				int num = Mathf.FloorToInt(CompProducesBioferrite.BioferritePerDay(innerPawn) * 4f);
				if (num > 0)
				{
					Thing thing = ThingMaker.MakeThing(ThingDefOf.Bioferrite, null);
					thing.stackCount = num;
					GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
				innerPawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 4f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				innerPawn.health.AddHediff(HediffDefOf.BioferriteExtracted, null, null, null);
			});
			yield break;
		}

		// Token: 0x06007372 RID: 29554 RVA: 0x00237873 File Offset: 0x00235A73
		public override string GetReport()
		{
			return JobUtility.GetResolvedJobReport(this.job.def.reportString, this.InnerPawn, this.job.targetB, this.job.targetC);
		}

		// Token: 0x04004F91 RID: 20369
		private const TargetIndex PlatformIndex = TargetIndex.A;

		// Token: 0x04004F92 RID: 20370
		private const int ExtractDuration = 2000;

		// Token: 0x04004F93 RID: 20371
		private const int CutDamage = 4;

		// Token: 0x04004F94 RID: 20372
		private const int BioferriteModifier = 4;
	}
}
