using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001286 RID: 4742
	public class JobDriver_Forage : JobDriver
	{
		// Token: 0x06007401 RID: 29697 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007402 RID: 29698 RVA: 0x0023947F File Offset: 0x0023767F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !this.job.GetTarget(TargetIndex.A).Cell.GetTerrain(this.pawn.Map).natural);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, 240, true, true, false, TargetIndex.None, PathEndMode.Touch);
			toil.WithEffect(EffecterDefOf.BuryPawn, () => this.job.GetTarget(TargetIndex.A), null);
			yield return toil;
			yield return this.CompleteForageToil();
			yield break;
		}

		// Token: 0x06007403 RID: 29699 RVA: 0x0023948F File Offset: 0x0023768F
		private Toil CompleteForageToil()
		{
			Toil toil = ToilMaker.MakeToil("CompleteForageToil");
			toil.initAction = delegate()
			{
				Thing thing = ThingMaker.MakeThing(this.pawn.Map.Biome.foragedFood, null);
				thing.stackCount = JobDriver_Forage.StackCountRange.RandomInRange;
				GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, null, null, null, 1);
				Messages.Message("MessageAnimalForagedFood".Translate(this.pawn.Named("ANIMAL"), thing.Named("FOOD")), thing, MessageTypeDefOf.PositiveEvent, true);
			};
			return toil;
		}

		// Token: 0x04004FD6 RID: 20438
		private const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x04004FD7 RID: 20439
		private const int ForageTicks = 240;

		// Token: 0x04004FD8 RID: 20440
		private static readonly IntRange StackCountRange = new IntRange(10, 25);
	}
}
