using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020014B9 RID: 5305
	public class JobGiver_PrisonerEscape : ThinkNode_JobGiver
	{
		// Token: 0x06007F1F RID: 32543 RVA: 0x00264824 File Offset: 0x00262A24
		protected override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (this.ShouldStartEscaping(pawn) && RCellFinder.TryFindBestExitSpot(pawn, out c, TraverseMode.ByPawn, true))
			{
				if (!pawn.guest.Released)
				{
					Messages.Message("MessagePrisonerIsEscaping".Translate(pawn.LabelShort, pawn), pawn, MessageTypeDefOf.ThreatSmall, true);
					Find.TickManager.slower.SignalForceNormalSpeed();
				}
				Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
				job.exitMapOnArrival = true;
				return job;
			}
			return null;
		}

		// Token: 0x06007F20 RID: 32544 RVA: 0x002648AC File Offset: 0x00262AAC
		private bool ShouldStartEscaping(Pawn pawn)
		{
			if (!pawn.guest.IsPrisoner || pawn.guest.HostFaction != Faction.OfPlayer || !pawn.guest.PrisonerIsSecure)
			{
				return false;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.PrisonerSecure(pawn))
			{
				return false;
			}
			District district = pawn.GetDistrict(RegionType.Set_Passable);
			if (district.TouchesMapEdge)
			{
				return true;
			}
			bool found = false;
			RegionTraverser.BreadthFirstTraverse(district.Regions[0], (Region from, Region reg) => reg.door == null || reg.door.FreePassage, delegate(Region reg)
			{
				if (reg.District.TouchesMapEdge)
				{
					found = true;
					return true;
				}
				return false;
			}, 25, RegionType.Set_Passable);
			return found;
		}

		// Token: 0x0400552A RID: 21802
		private const int MaxRegionsToCheckWhenEscapingThroughOpenDoors = 25;
	}
}
