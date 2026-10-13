using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001C58 RID: 7256
	public static class LeaveQuestPartUtility
	{
		// Token: 0x0600A1A1 RID: 41377 RVA: 0x002E723C File Offset: 0x002E543C
		public static void MakePawnLeave(Pawn pawn, Quest quest)
		{
			Caravan caravan = pawn.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(pawn, caravan.PawnsListForReading, null);
				caravan.RemovePawn(pawn);
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				Rand.PushState(quest.id ^ 960512692);
				Faction faction;
				if (pawn.HasExtraHomeFaction(quest) && pawn.GetExtraHomeFaction(quest) != Faction.OfPlayer)
				{
					faction = pawn.GetExtraHomeFaction(quest);
				}
				else if (pawn.HasExtraMiniFaction(quest) && pawn.GetExtraMiniFaction(quest) != Faction.OfPlayer)
				{
					faction = pawn.GetExtraMiniFaction(quest);
				}
				else if (pawn.IsSlave && pawn.SlaveFaction != Faction.OfPlayer)
				{
					faction = pawn.SlaveFaction;
				}
				else if (!(from x in Find.FactionManager.GetFactions(false, false, false, TechLevel.Undefined, false)
				where !x.HostileTo(Faction.OfPlayer)
				select x).TryRandomElement(out faction) && !Find.FactionManager.GetFactions(false, false, false, TechLevel.Undefined, false).TryRandomElement(out faction))
				{
					faction = null;
				}
				Rand.PopState();
				if (pawn.Faction != faction)
				{
					pawn.SetFaction(faction, null);
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
			{
				if (pawn2.playerSettings.Master == pawn)
				{
					pawn2.playerSettings.Master = null;
				}
			}
			if (pawn.guest != null)
			{
				if (pawn.InBed() && pawn.CurrentBed().Faction == Faction.OfPlayer && (pawn.Faction == null || !pawn.Faction.HostileTo(Faction.OfPlayer)))
				{
					pawn.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Guest);
				}
				else
				{
					pawn.guest.SetGuestStatus(null, GuestStatus.Guest);
				}
			}
			Pawn_CarryTracker carryTracker = pawn.carryTracker;
			if (((carryTracker != null) ? carryTracker.CarriedThing : null) != null)
			{
				Thing thing;
				pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out thing, null);
			}
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedByQuest, null);
			}
		}

		// Token: 0x0600A1A2 RID: 41378 RVA: 0x002E7458 File Offset: 0x002E5658
		public static void MakePawnsLeave(IEnumerable<Pawn> pawns, bool sendLetter, Quest quest, bool wakeUp = false)
		{
			bool flag = pawns.Any((Pawn x) => x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer);
			List<Pawn> list = (from x in pawns
			where x.Spawned || x.IsCaravanMember()
			select x).ToList<Pawn>();
			if (sendLetter && list.Any<Pawn>())
			{
				Pawn pawn;
				string value = GenLabel.BestGroupLabel(list, false, out pawn);
				string value2 = GenLabel.BestGroupLabel(list, true, out pawn);
				if (flag)
				{
					if (pawn != null)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelPawnLeaving".Translate(value), "LetterPawnLeaving".Translate(value2), LetterDefOf.NeutralEvent, pawn, null, quest, null, null, 0, true);
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelPawnsLeaving".Translate(value), "LetterPawnsLeaving".Translate(value2), LetterDefOf.NeutralEvent, list[0], null, quest, null, null, 0, true);
					}
				}
				else if (pawn != null)
				{
					Messages.Message("MessagePawnLeaving".Translate(value2), pawn, MessageTypeDefOf.NeutralEvent, true);
				}
				else
				{
					Messages.Message("MessagePawnsLeaving".Translate(value2), list[0], MessageTypeDefOf.NeutralEvent, true);
				}
			}
			foreach (Pawn pawn2 in pawns)
			{
				LeaveQuestPartUtility.MakePawnLeave(pawn2, quest);
			}
			IEnumerable<Pawn> enumerable = from p in pawns
			where p.Spawned && !p.Downed
			select p;
			if (enumerable.Any<Pawn>())
			{
				Pawn pawn3 = enumerable.First<Pawn>();
				LordJob_ExitMapBest lordJob = new LordJob_ExitMapBest(LocomotionUrgency.Walk, true, true);
				LordMaker.MakeNewLord(pawn3.Faction, lordJob, pawn3.MapHeld, enumerable);
			}
			if (wakeUp)
			{
				foreach (Pawn p2 in pawns)
				{
					if (!p2.Awake())
					{
						RestUtility.WakeUp(p2, true);
					}
				}
			}
		}
	}
}
