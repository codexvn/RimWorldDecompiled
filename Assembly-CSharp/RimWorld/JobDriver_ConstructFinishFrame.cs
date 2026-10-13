using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200116A RID: 4458
	public class JobDriver_ConstructFinishFrame : JobDriver, IBuildableDriver
	{
		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x06006D12 RID: 27922 RVA: 0x0022354C File Offset: 0x0022174C
		private Frame Frame
		{
			get
			{
				return (Frame)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x06006D13 RID: 27923 RVA: 0x00223574 File Offset: 0x00221774
		private bool IsBuildingAttachment
		{
			get
			{
				ThingDef thingDef = GenConstruct.BuiltDefOf(this.Frame.def) as ThingDef;
				BuildingProperties buildingProperties = (thingDef != null) ? thingDef.building : null;
				return buildingProperties != null && buildingProperties.isAttachment;
			}
		}

		// Token: 0x06006D14 RID: 27924 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006D15 RID: 27925 RVA: 0x002235AE File Offset: 0x002217AE
		public bool TryGetBuildableRect(out CellRect rect)
		{
			rect = this.Frame.OccupiedRect();
			return true;
		}

		// Token: 0x06006D16 RID: 27926 RVA: 0x002235C2 File Offset: 0x002217C2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.IsBuildingAttachment)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			}
			else
			{
				yield return Toils_Goto.GotoBuild(TargetIndex.A).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			}
			Toil build = ToilMaker.MakeToil("MakeNewToils");
			build.initAction = delegate()
			{
				GenClamor.DoClamor(build.actor, 15f, ClamorDefOf.Construction);
			};
			build.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = build.actor;
				Frame frame = this.Frame;
				if (frame.resourceContainer.Count > 0 && actor.skills != null)
				{
					actor.skills.Learn(SkillDefOf.Construction, 0.25f * (float)delta, false, false);
				}
				actor.rotationTracker.FaceTarget(this.IsBuildingAttachment ? GenConstruct.GetWallAttachedTo(frame) : frame);
				float num = actor.GetStatValue(StatDefOf.ConstructionSpeed, true, -1) * 1.7f * (float)delta;
				if (frame.Stuff != null)
				{
					num *= frame.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor, null);
				}
				float workToBuild = frame.WorkToBuild;
				if (actor.Faction == Faction.OfPlayer)
				{
					float statValue = actor.GetStatValue(StatDefOf.ConstructSuccessChance, true, -1);
					if (!TutorSystem.TutorialMode && Rand.Value < 1f - Mathf.Pow(statValue, num / workToBuild))
					{
						frame.FailConstruction(actor);
						this.ReadyForNextToil();
						return;
					}
				}
				if (frame.def.entityDefToBuild is TerrainDef)
				{
					this.Map.snowGrid.SetDepth(frame.Position, 0f);
					SandGrid sandGrid = this.Map.sandGrid;
					if (sandGrid != null)
					{
						sandGrid.SetDepth(frame.Position, 0f);
					}
				}
				frame.workDone += num;
				if (frame.workDone >= workToBuild)
				{
					frame.CompleteConstruction(actor);
					this.ReadyForNextToil();
					return;
				}
			};
			build.WithEffect(() => ((Frame)build.actor.jobs.curJob.GetTarget(TargetIndex.A).Thing).ConstructionEffect, TargetIndex.A, null);
			build.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			build.FailOn(() => !GenConstruct.CanConstruct(this.Frame, this.pawn, true, false, null));
			build.defaultCompleteMode = ToilCompleteMode.Delay;
			build.defaultDuration = 5000;
			build.activeSkill = (() => SkillDefOf.Construction);
			build.handlingFacing = true;
			yield return build;
			yield break;
		}

		// Token: 0x04004C79 RID: 19577
		private const int JobEndInterval = 5000;

		// Token: 0x04004C7A RID: 19578
		private const TargetIndex BuildingInd = TargetIndex.A;
	}
}
