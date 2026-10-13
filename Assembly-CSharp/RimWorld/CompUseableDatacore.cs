using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002EC4 RID: 11972
	public class CompUseableDatacore : CompUsable
	{
		// Token: 0x06010E62 RID: 69218 RVA: 0x004DDA60 File Offset: 0x004DBC60
		public override LocalTargetInfo GetExtraTarget(Pawn pawn)
		{
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.ResearchBench), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing thing) => pawn.CanReserve(thing, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06010E63 RID: 69219 RVA: 0x004DDACC File Offset: 0x004DBCCC
		public override AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
		{
			AcceptanceReport result = base.CanBeUsedBy(p, forced, ignoreReserveAndReachable);
			if (!result.Accepted)
			{
				return result;
			}
			if (!this.GetExtraTarget(p).HasThing)
			{
				return "NoResearchBench".Translate();
			}
			return true;
		}

		// Token: 0x06010E64 RID: 69220 RVA: 0x004DDB15 File Offset: 0x004DBD15
		public override void UsedBy(Pawn p)
		{
			base.UsedBy(p);
			Find.History.Notify_MechanoidDatacoreReadOrLost();
		}

		// Token: 0x06010E65 RID: 69221 RVA: 0x004DDB28 File Offset: 0x004DBD28
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			Find.History.Notify_MechanoidDatacoreReadOrLost();
		}

		// Token: 0x06010E66 RID: 69222 RVA: 0x004DDB3C File Offset: 0x004DBD3C
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			base.Notify_AbandonedAtTile(tile);
			Find.History.Notify_MechanoidDatacoreReadOrLost();
		}
	}
}
