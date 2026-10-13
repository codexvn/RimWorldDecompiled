using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001272 RID: 4722
	public class JobDriver_ExtractSkull : JobDriver
	{
		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06007381 RID: 29569 RVA: 0x00237B3F File Offset: 0x00235D3F
		protected Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007382 RID: 29570 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007383 RID: 29571 RVA: 0x00237B56 File Offset: 0x00235D56
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Corpse.Destroyed || !this.Corpse.Spawned);
			this.FailOn(() => base.Map.designationManager.DesignationOn(base.TargetA.Thing, DesignationDefOf.ExtractSkull) == null);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false);
			Toil toil = Toils_General.Wait(180, TargetIndex.None);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.PlaySustainerOrSound(SoundDefOf.Recipe_Surgery, 1f);
			Toil toil2 = toil;
			toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				if (this.Corpse.CurRotDrawMode != RotDrawMode.Dessicated && Rand.Chance(0.016666668f * (float)delta))
				{
					CellRect cellRect = new CellRect(this.Corpse.PositionHeld.x - 1, this.Corpse.PositionHeld.z - 1, 3, 3);
					IntVec3 randomCell = cellRect.RandomCell;
					if (randomCell.InBounds(base.Map) && GenSight.LineOfSight(randomCell, this.Corpse.PositionHeld, base.Map))
					{
						FilthMaker.TryMakeFilth(randomCell, this.Corpse.MapHeld, this.Corpse.InnerPawn.RaceProps.BloodDef, this.Corpse.InnerPawn.LabelIndefinite(), 1, FilthSourceFlags.None);
					}
				}
			}));
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				this.Corpse.InnerPawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.Corpse.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).First((BodyPartRecord p) => p.def == BodyPartDefOf.Head), null, null);
				this.Corpse.Map.designationManager.RemoveDesignation(this.Corpse.Map.designationManager.DesignationOn(this.Corpse, DesignationDefOf.ExtractSkull));
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Skull, null);
				CompHasSources compHasSources = thing.TryGetComp<CompHasSources>();
				if (compHasSources != null)
				{
					compHasSources.AddSource(this.Corpse.InnerPawn.LabelShort);
				}
				GenPlace.TryPlaceThing(thing, this.Corpse.PositionHeld, this.Corpse.Map, ThingPlaceMode.Near, null, null, null, 1);
			});
			yield break;
		}

		// Token: 0x04004F9B RID: 20379
		public const int ExtractionTimeTicks = 180;

		// Token: 0x04004F9C RID: 20380
		private const TargetIndex CorpseInd = TargetIndex.A;
	}
}
