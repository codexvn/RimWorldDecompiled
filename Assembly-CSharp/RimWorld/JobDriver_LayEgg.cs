using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001113 RID: 4371
	public class JobDriver_LayEgg : JobDriver
	{
		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x06006A94 RID: 27284 RVA: 0x0021A58C File Offset: 0x0021878C
		public CompEggContainer EggBoxComp
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEggContainer>();
			}
		}

		// Token: 0x06006A95 RID: 27285 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006A96 RID: 27286 RVA: 0x0021A5B2 File Offset: 0x002187B2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			yield return Toils_General.Wait(500, TargetIndex.None);
			yield return Toils_General.Do(delegate
			{
				Thing thing = this.pawn.GetComp<CompEggLayer>().ProduceEgg();
				if (this.job.GetTarget(TargetIndex.A).HasThing && this.EggBoxComp.Accepts(thing.def))
				{
					this.EggBoxComp.innerContainer.TryAdd(thing, true);
					return;
				}
				GenPlace.TryPlaceThing(thing, this.pawn.Position, this.pawn.Map, ThingPlaceMode.Near, delegate(Thing t, int i)
				{
					if (this.pawn.Faction != Faction.OfPlayer)
					{
						t.SetForbidden(true, true);
					}
				}, null, null, 1);
			});
			yield break;
		}

		// Token: 0x04004B3B RID: 19259
		private const int LayEgg = 500;

		// Token: 0x04004B3C RID: 19260
		private const TargetIndex LaySpotOrEggBoxInd = TargetIndex.A;
	}
}
