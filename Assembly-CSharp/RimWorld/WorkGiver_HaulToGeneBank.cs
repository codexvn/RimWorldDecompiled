using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020015E0 RID: 5600
	public class WorkGiver_HaulToGeneBank : WorkGiver_Scanner
	{
		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06008432 RID: 33842 RVA: 0x0027A383 File Offset: 0x00278583
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForDef(ThingDefOf.Genepack);
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06008433 RID: 33843 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008434 RID: 33844 RVA: 0x0027511F File Offset: 0x0027331F
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return !ModsConfig.BiotechActive;
		}

		// Token: 0x06008435 RID: 33845 RVA: 0x0027A38F File Offset: 0x0027858F
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return ModLister.CheckBiotech("Genepack") && pawn.CanReserve(t, 1, -1, null, forced) && this.FindGeneBank(pawn, t) != null;
		}

		// Token: 0x06008436 RID: 33846 RVA: 0x0027A3C0 File Offset: 0x002785C0
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Thing thing = this.FindGeneBank(pawn, t);
			if (thing != null)
			{
				Job job = JobMaker.MakeJob(JobDefOf.CarryGenepackToContainer, t, thing, thing.InteractionCell);
				job.count = t.stackCount;
				return job;
			}
			return null;
		}

		// Token: 0x06008437 RID: 33847 RVA: 0x0027A408 File Offset: 0x00278608
		private Thing FindGeneBank(Pawn pawn, Thing genepackThing)
		{
			Genepack genepack = genepackThing as Genepack;
			if (!genepack.AutoLoad)
			{
				return null;
			}
			if (genepack.targetContainer != null)
			{
				if (genepack.targetContainer.Map == genepack.Map)
				{
					CompGenepackContainer compGenepackContainer = genepack.targetContainer.TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null && !compGenepackContainer.Full)
					{
						return genepack.targetContainer;
					}
				}
				return null;
			}
			return GenClosest.ClosestThingReachable(genepack.Position, genepack.Map, ThingRequest.ForGroup(ThingRequestGroup.GenepackHolder), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing x)
			{
				if (x.IsForbidden(pawn) || !pawn.CanReserve(x, 1, -1, null, false))
				{
					return false;
				}
				CompGenepackContainer compGenepackContainer2 = x.TryGetComp<CompGenepackContainer>();
				if (compGenepackContainer2 == null || compGenepackContainer2.Full || !compGenepackContainer2.autoLoad)
				{
					return false;
				}
				Thing targetContainer = genepack.targetContainer;
				return targetContainer == null || targetContainer == compGenepackContainer2.parent;
			}, null, 0, -1, false, RegionType.Set_Passable, false, false);
		}
	}
}
