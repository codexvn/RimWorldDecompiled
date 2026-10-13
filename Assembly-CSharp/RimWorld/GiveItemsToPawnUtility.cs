using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001735 RID: 5941
	public static class GiveItemsToPawnUtility
	{
		// Token: 0x06008BBE RID: 35774 RVA: 0x00299B70 File Offset: 0x00297D70
		public static Thing FindItemToGive(Pawn hauler, ThingDef thingDef)
		{
			return GenClosest.ClosestThingReachable(hauler.Position, hauler.Map, ThingRequest.ForDef(thingDef), PathEndMode.Touch, TraverseParms.For(hauler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing x) => !x.IsForbidden(hauler) && hauler.CanReserve(x, 1, -1, null, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
		}

		// Token: 0x06008BBF RID: 35775 RVA: 0x00299BD8 File Offset: 0x00297DD8
		public static bool IsWaitingForItems(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.CurLordToil is IWaitForItemsLordToil;
		}

		// Token: 0x06008BC0 RID: 35776 RVA: 0x00299BFF File Offset: 0x00297DFF
		public static int ItemCountWaitingFor(Pawn pawn)
		{
			if (!GiveItemsToPawnUtility.IsWaitingForItems(pawn))
			{
				return -1;
			}
			return ((IWaitForItemsLordToil)pawn.GetLord().CurLordToil).CountRemaining;
		}

		// Token: 0x06008BC1 RID: 35777 RVA: 0x00299C20 File Offset: 0x00297E20
		public static int ItemCountBeingHauled(Pawn requester)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = requester.Map.mapPawns.AllPawnsSpawned;
			Lord lord = requester.GetLord();
			int num = 0;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if (pawn.CurJob != null)
				{
					JobDriver_GiveToPawn jobDriver_GiveToPawn = pawn.jobs.curDriver as JobDriver_GiveToPawn;
					if (jobDriver_GiveToPawn != null && jobDriver_GiveToPawn.job.lord == lord)
					{
						num += jobDriver_GiveToPawn.CountBeingHauled;
					}
				}
			}
			return num;
		}

		// Token: 0x06008BC2 RID: 35778 RVA: 0x00299C9B File Offset: 0x00297E9B
		public static int ItemCountLeftToCollect(Pawn requester)
		{
			return GiveItemsToPawnUtility.ItemCountWaitingFor(requester) - GiveItemsToPawnUtility.ItemCountBeingHauled(requester);
		}

		// Token: 0x06008BC3 RID: 35779 RVA: 0x00299CAA File Offset: 0x00297EAA
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptionsForPawn(Pawn requester, Pawn current, ThingDef requestedThingDef, int countRequired)
		{
			Thing reachableThing = GiveItemsToPawnUtility.FindItemToGive(current, requestedThingDef);
			string value = string.Format("{0}x {1}", GiveItemsToPawnUtility.GetCountRemaining(requester, requestedThingDef, countRequired), requestedThingDef.label);
			if (reachableThing != null)
			{
				TaggedString taggedString = "GiveItemsTo".Translate(value, requester);
				yield return new FloatMenuOption(taggedString, delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.GiveToPawn, reachableThing, requester);
					job.haulMode = HaulMode.ToContainer;
					job.lord = requester.GetLord();
					current.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}, MenuOptionPriority.High, null, requester, 0f, null, null, true, 0);
			}
			else
			{
				TaggedString taggedString2 = "CannotGiveItemsTo".Translate(value, requester) + ": " + "NoItemFound".Translate();
				yield return new FloatMenuOption(taggedString2, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x06008BC4 RID: 35780 RVA: 0x00299CD0 File Offset: 0x00297ED0
		public static int GetCountRemaining(Pawn pawn, ThingDef requestedThingDef, int requestedThingCount)
		{
			int num = 0;
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i].def == requestedThingDef)
				{
					num += innerContainer[i].stackCount;
				}
			}
			return requestedThingCount - num;
		}
	}
}
