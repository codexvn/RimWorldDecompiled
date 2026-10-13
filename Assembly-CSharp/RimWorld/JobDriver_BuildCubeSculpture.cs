using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011CF RID: 4559
	public class JobDriver_BuildCubeSculpture : JobDriver
	{
		// Token: 0x06006F6F RID: 28527 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006F70 RID: 28528 RVA: 0x0022B101 File Offset: 0x00229301
		public override string GetReport()
		{
			if (base.CurToilIndex == 0)
			{
				return "ReportBuildingCubeEnroute".Translate();
			}
			return this.job.def.reportString.Formatted(this.GetMaterialString());
		}

		// Token: 0x06006F71 RID: 28529 RVA: 0x0022B140 File Offset: 0x00229340
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = Toils_Goto.Goto(TargetIndex.A, PathEndMode.OnCell);
			toil.AddFinishAction(new Action(this.CacheGroundType));
			yield return toil;
			Toil toil2 = Toils_General.Wait(5000, TargetIndex.None);
			toil2.WithProgressBar(TargetIndex.A, () => this.workDone / 5000f, false, -0.5f, false);
			toil2.WithEffect(new Func<EffecterDef>(this.GetSculptureEffecter), TargetIndex.A, null);
			toil2.activeSkill = (() => SkillDefOf.Construction);
			toil2.handlingFacing = true;
			toil2.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
				float num = 1.7f;
				if (this.pawn.skills != null)
				{
					this.pawn.skills.Learn(SkillDefOf.Construction, 0.25f * (float)delta, false, false);
					if (!this.pawn.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled)
					{
						num *= this.pawn.GetStatValue(StatDefOf.ConstructionSpeed, true, -1);
					}
				}
				if (this.pawn.Stuff != null)
				{
					num *= this.pawn.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
				}
				this.workDone += num * (float)delta;
				if (this.workDone >= 5000f)
				{
					base.ReadyForNextToil();
				}
			};
			yield return toil2;
			yield return Toils_General.Do(new Action(this.PlaceAndFinish));
			yield break;
		}

		// Token: 0x06006F72 RID: 28530 RVA: 0x0022B150 File Offset: 0x00229350
		private void CacheGroundType()
		{
			TerrainDef terrainDef = this.pawn.MapHeld.terrainGrid.TerrainAt(base.TargetA.Cell);
			if (terrainDef == null)
			{
				this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Scrap;
				return;
			}
			if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Soil)
			{
				this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Dirt;
				return;
			}
			if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Stone)
			{
				this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Stone;
				return;
			}
			if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Sand)
			{
				this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Sand;
				return;
			}
			this.cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Scrap;
		}

		// Token: 0x06006F73 RID: 28531 RVA: 0x0022B1C8 File Offset: 0x002293C8
		private void PlaceAndFinish()
		{
			Thing thing = GenSpawn.Spawn(this.GetSculptureThing(), base.TargetA.Cell, this.pawn.MapHeld, WipeMode.Vanish);
			thing.SetFaction(Faction.OfPlayer, null);
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(QualityUtility.GenerateQualityCreatedByPawn(this.pawn, SkillDefOf.Construction, false), new ArtGenerationContext?(ArtGenerationContext.Colony));
				QualityUtility.SendCraftNotification(thing, this.pawn);
			}
			CompArt compArt = thing.TryGetComp<CompArt>();
			if (compArt != null)
			{
				if (compQuality == null)
				{
					compArt.InitializeArt(ArtGenerationContext.Colony);
				}
				compArt.JustCreatedBy(this.pawn);
			}
			Hediff_CubeInterest hediff_CubeInterest;
			if (this.pawn.health.hediffSet.TryGetHediff<Hediff_CubeInterest>(out hediff_CubeInterest))
			{
				hediff_CubeInterest.Notify_BuiltSculpture(thing);
			}
			if (this.pawn.InMentalState && this.pawn.MentalStateDef == MentalStateDefOf.CubeSculpting)
			{
				this.pawn.MentalState.RecoverFromState();
			}
		}

		// Token: 0x06006F74 RID: 28532 RVA: 0x0022B2AC File Offset: 0x002294AC
		private Thing GetSculptureThing()
		{
			ThingDef def = null;
			if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Dirt)
			{
				def = ThingDefOf.DirtCubeSculpture;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Stone)
			{
				def = ThingDefOf.StoneCubeSculpture;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Sand)
			{
				def = ThingDefOf.SandCubeSculpture;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Scrap)
			{
				def = ThingDefOf.ScrapCubeSculpture;
			}
			return ThingMaker.MakeThing(def, null);
		}

		// Token: 0x06006F75 RID: 28533 RVA: 0x0022B304 File Offset: 0x00229504
		private EffecterDef GetSculptureEffecter()
		{
			EffecterDef result = null;
			if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Dirt)
			{
				result = EffecterDefOf.ConstructDirt;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Stone)
			{
				result = EffecterDefOf.ConstructDirt;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Sand)
			{
				result = EffecterDefOf.ConstructDirt;
			}
			else if (this.cubeMaterial == JobDriver_BuildCubeSculpture.CubeMaterial.Scrap)
			{
				result = EffecterDefOf.ConstructMetal;
			}
			return result;
		}

		// Token: 0x06006F76 RID: 28534 RVA: 0x0022B358 File Offset: 0x00229558
		private string GetMaterialString()
		{
			switch (this.cubeMaterial)
			{
			case JobDriver_BuildCubeSculpture.CubeMaterial.Dirt:
				return "CubeMaterialDirt".Translate();
			case JobDriver_BuildCubeSculpture.CubeMaterial.Stone:
				return "CubeMaterialStone".Translate();
			case JobDriver_BuildCubeSculpture.CubeMaterial.Sand:
				return "CubeMaterialSand".Translate();
			case JobDriver_BuildCubeSculpture.CubeMaterial.Scrap:
				return "CubeMaterialScrap".Translate();
			default:
				return "CubeMaterialScrap".Translate();
			}
		}

		// Token: 0x06006F77 RID: 28535 RVA: 0x0022B3D3 File Offset: 0x002295D3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<JobDriver_BuildCubeSculpture.CubeMaterial>(ref this.cubeMaterial, "cubeMaterial", JobDriver_BuildCubeSculpture.CubeMaterial.Dirt, false);
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x04004DB2 RID: 19890
		private JobDriver_BuildCubeSculpture.CubeMaterial cubeMaterial = JobDriver_BuildCubeSculpture.CubeMaterial.Scrap;

		// Token: 0x04004DB3 RID: 19891
		private float workDone;

		// Token: 0x04004DB4 RID: 19892
		private const TargetIndex Target = TargetIndex.A;

		// Token: 0x04004DB5 RID: 19893
		private const int BuildTimeTicks = 5000;

		// Token: 0x020011D0 RID: 4560
		private enum CubeMaterial
		{
			// Token: 0x04004DB7 RID: 19895
			Dirt,
			// Token: 0x04004DB8 RID: 19896
			Stone,
			// Token: 0x04004DB9 RID: 19897
			Sand,
			// Token: 0x04004DBA RID: 19898
			Scrap
		}
	}
}
