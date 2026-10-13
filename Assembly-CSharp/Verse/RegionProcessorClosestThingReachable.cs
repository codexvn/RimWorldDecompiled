using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000CEE RID: 3310
	public class RegionProcessorClosestThingReachable : RegionProcessorDelegateCache
	{
		// Token: 0x0600547D RID: 21629 RVA: 0x001B32E4 File Offset: 0x001B14E4
		public void SetParameters(TraverseParms traverseParams, float maxDistance, IntVec3 root, bool ignoreEntirelyForbiddenRegions, ThingRequest req, PathEndMode peMode, Func<Thing, float> priorityGetter, Predicate<Thing> validator, int minRegions, float closestDistSquared = 9999999f, int regionsSeenScan = 0, float bestPrio = -3.4028235E+38f, Thing closestThing = null, bool lookInHaulSources = false)
		{
			this.traverseParams = traverseParams;
			this.maxDistance = maxDistance;
			this.root = root;
			this.regionsSeenScan = regionsSeenScan;
			this.ignoreEntirelyForbiddenRegions = ignoreEntirelyForbiddenRegions;
			this.req = req;
			this.peMode = peMode;
			this.priorityGetter = priorityGetter;
			this.validator = validator;
			this.bestPrio = bestPrio;
			this.closestDistSquared = closestDistSquared;
			this.closestThing = closestThing;
			this.minRegions = minRegions;
			this.lookInHaulSources = lookInHaulSources;
			this.maxDistSquared = maxDistance * maxDistance;
		}

		// Token: 0x0600547E RID: 21630 RVA: 0x001B3368 File Offset: 0x001B1568
		public void Clear()
		{
			this.SetParameters(default(TraverseParms), 0f, default(IntVec3), false, default(ThingRequest), PathEndMode.None, null, null, 0, 0f, 0, 0f, null, false);
		}

		// Token: 0x0600547F RID: 21631 RVA: 0x001B33AD File Offset: 0x001B15AD
		protected override bool RegionEntryPredicate(Region from, Region to)
		{
			return to.Allows(this.traverseParams, false) && (this.maxDistance > 5000f || to.extentsClose.ClosestDistSquaredTo(this.root) < this.maxDistSquared);
		}

		// Token: 0x06005480 RID: 21632 RVA: 0x001B33E8 File Offset: 0x001B15E8
		protected override bool RegionProcessor(Region reg)
		{
			if (RegionTraverser.ShouldCountRegion(reg))
			{
				this.regionsSeenScan++;
			}
			if (!reg.IsDoorway && !reg.Allows(this.traverseParams, true))
			{
				return false;
			}
			if (!this.ignoreEntirelyForbiddenRegions || !reg.IsForbiddenEntirely(this.traverseParams.pawn))
			{
				List<Thing> list = reg.ListerThings.ThingsMatching(this.req);
				for (int i = 0; i < list.Count; i++)
				{
					this.ProcessThing(reg, list[i]);
				}
				Pawn pawn = this.traverseParams.pawn;
				bool flag = pawn != null && pawn.IsColonist;
				if (this.lookInHaulSources)
				{
					foreach (Building building in reg.ListerThings.GetThingsOfType<Building>())
					{
						IHaulSource haulSource = building as IHaulSource;
						if (haulSource != null && (!flag || haulSource.HaulSourceEnabled))
						{
							foreach (Thing t in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
							{
								if (this.req.Accepts(t))
								{
									this.ProcessThing(reg, t);
								}
							}
						}
					}
				}
			}
			return this.regionsSeenScan >= this.minRegions && this.closestThing != null;
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x001B3558 File Offset: 0x001B1758
		private void ProcessThing(Region reg, Thing t)
		{
			if (!ReachabilityWithinRegion.ThingFromRegionListerReachable(t.SpawnedParentOrMe, reg, this.peMode, this.traverseParams.pawn))
			{
				return;
			}
			Func<Thing, float> func = this.priorityGetter;
			float num = (func != null) ? func(t) : 0f;
			if (num < this.bestPrio)
			{
				return;
			}
			float num2 = (float)(t.PositionHeld - this.root).LengthHorizontalSquared;
			if ((num > this.bestPrio || num2 < this.closestDistSquared) && num2 < this.maxDistSquared && (this.validator == null || this.validator(t)))
			{
				this.closestThing = t;
				this.closestDistSquared = num2;
				this.bestPrio = num;
			}
		}

		// Token: 0x04003CF1 RID: 15601
		private TraverseParms traverseParams;

		// Token: 0x04003CF2 RID: 15602
		private float maxDistance;

		// Token: 0x04003CF3 RID: 15603
		private IntVec3 root;

		// Token: 0x04003CF4 RID: 15604
		public Thing closestThing;

		// Token: 0x04003CF5 RID: 15605
		public int regionsSeenScan;

		// Token: 0x04003CF6 RID: 15606
		private bool ignoreEntirelyForbiddenRegions;

		// Token: 0x04003CF7 RID: 15607
		private ThingRequest req;

		// Token: 0x04003CF8 RID: 15608
		private PathEndMode peMode;

		// Token: 0x04003CF9 RID: 15609
		private Func<Thing, float> priorityGetter;

		// Token: 0x04003CFA RID: 15610
		private Predicate<Thing> validator;

		// Token: 0x04003CFB RID: 15611
		private float bestPrio;

		// Token: 0x04003CFC RID: 15612
		private float closestDistSquared;

		// Token: 0x04003CFD RID: 15613
		private int minRegions;

		// Token: 0x04003CFE RID: 15614
		private bool lookInHaulSources;

		// Token: 0x04003CFF RID: 15615
		private float maxDistSquared;
	}
}
