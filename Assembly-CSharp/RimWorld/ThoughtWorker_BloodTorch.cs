using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020018E6 RID: 6374
	public class ThoughtWorker_BloodTorch : ThoughtWorker
	{
		// Token: 0x06009113 RID: 37139 RVA: 0x002AADEC File Offset: 0x002A8FEC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (p.Suspended)
			{
				return false;
			}
			Room pawnRoom = p.GetRoom(RegionType.Set_All);
			if (GenClosest.ClosestThingReachable(p.Position, p.Map, ThingRequest.ForDef(ThingDefOf.SanguphageMeetingTorch), PathEndMode.ClosestTouch, TraverseParms.For(p, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 8f, delegate(Thing b)
			{
				Room room = b.GetRoom(RegionType.Set_All);
				if (room != null && room != pawnRoom)
				{
					return false;
				}
				CompGlower compGlower = b.TryGetComp<CompGlower>();
				return compGlower != null && compGlower.Glows;
			}, null, 0, -1, false, RegionType.Set_Passable, false, false) == null)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(0);
		}

		// Token: 0x04005C6E RID: 23662
		private const float MaxDist = 8f;
	}
}
