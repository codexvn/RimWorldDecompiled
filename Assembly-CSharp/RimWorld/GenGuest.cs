using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002445 RID: 9285
	public static class GenGuest
	{
		// Token: 0x0600C8FC RID: 51452 RVA: 0x003AB78C File Offset: 0x003A998C
		public static void PrisonerRelease(Pawn p)
		{
			if ((p.Faction == Faction.OfPlayer || p.IsWildMan()) && p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.WasImprisoned, null, null);
			}
			if (p.SlaveFaction != null)
			{
				Faction hostFaction = p.HostFaction;
				p.SetFaction(p.SlaveFaction, null);
				p.guest.SetGuestStatus(hostFaction, GuestStatus.Prisoner);
			}
			GenGuest.GuestRelease(p);
		}

		// Token: 0x0600C8FD RID: 51453 RVA: 0x003AB80C File Offset: 0x003A9A0C
		public static void SlaveRelease(Pawn p)
		{
			if ((p.Faction == Faction.OfPlayer || p.IsWildMan()) && p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.WasEnslaved, null, null);
			}
			GenGuest.GuestRelease(p);
		}

		// Token: 0x0600C8FE RID: 51454 RVA: 0x003AB862 File Offset: 0x003A9A62
		private static bool ShouldStayOnMapOnRelease(Pawn pawn)
		{
			return pawn.IsWildMan() || (pawn.HomeFaction != null && pawn.HomeFaction.IsPlayer);
		}

		// Token: 0x0600C8FF RID: 51455 RVA: 0x003AB884 File Offset: 0x003A9A84
		private static void GuestRelease(Pawn p)
		{
			Pawn_Ownership ownership = p.ownership;
			if (ownership != null)
			{
				ownership.UnclaimAll();
			}
			if (p.Drafted)
			{
				p.drafter.Drafted = false;
			}
			p.guest.Released = true;
			p.guest.SetNoInteraction();
			IntVec3 c;
			if (GenGuest.ShouldStayOnMapOnRelease(p))
			{
				int interactionsToday = p.mindState.interactionsToday;
				int lastAssignedInteractTime = p.mindState.lastAssignedInteractTime;
				if (p.HomeFaction != null)
				{
					p.guest.SetGuestStatus(null, GuestStatus.Guest);
				}
				p.mindState.interactionsToday = interactionsToday;
				p.mindState.lastAssignedInteractTime = lastAssignedInteractTime;
				if (p.IsWildMan())
				{
					p.mindState.WildManEverReachedOutside = false;
				}
				if (ModsConfig.IdeologyActive && p.Ideo != null && p.IsColonist)
				{
					p.Ideo.RecacheColonistBelieverCount();
				}
			}
			else if (RCellFinder.TryFindBestExitSpot(p, out c, TraverseMode.ByPawn, true))
			{
				Job job = JobMaker.MakeJob(JobDefOf.Goto, c);
				job.exitMapOnArrival = true;
				p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
			p.Notify_Released();
		}

		// Token: 0x0600C900 RID: 51456 RVA: 0x003AB9A8 File Offset: 0x003A9BA8
		public static void AddHealthyPrisonerReleasedThoughts(Pawn prisoner)
		{
			if (!prisoner.IsColonist)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonistsAndPrisoners)
				{
					if (pawn.needs.mood != null && pawn != prisoner)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ReleasedHealthyPrisoner, prisoner, null);
					}
				}
			}
		}

		// Token: 0x0600C901 RID: 51457 RVA: 0x003ABA30 File Offset: 0x003A9C30
		public static void RemoveHealthyPrisonerReleasedThoughts(Pawn prisoner)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists)
			{
				if (pawn.needs.mood != null && pawn != prisoner)
				{
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.ReleasedHealthyPrisoner, prisoner);
				}
			}
		}

		// Token: 0x0600C902 RID: 51458 RVA: 0x003ABAAC File Offset: 0x003A9CAC
		public static void EmancipateSlave(Pawn warden, Pawn slave)
		{
			if (!slave.IsSlave)
			{
				return;
			}
			GenGuest.SlaveRelease(slave);
			if (slave.IsWildMan())
			{
				slave.mindState.WildManEverReachedOutside = false;
			}
			Messages.Message("MessageSlaveEmancipated".Translate(slave, warden), new LookTargets(new TargetInfo[]
			{
				slave,
				warden
			}), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600C903 RID: 51459 RVA: 0x003ABB27 File Offset: 0x003A9D27
		public static void EnslavePrisoner(Pawn warden, Pawn prisoner)
		{
			GenGuest.TryEnslavePrisoner(warden, prisoner);
		}

		// Token: 0x0600C904 RID: 51460 RVA: 0x003ABB34 File Offset: 0x003A9D34
		public static bool TryEnslavePrisoner(Pawn warden, Pawn prisoner)
		{
			if (prisoner.IsSlave)
			{
				return false;
			}
			if (prisoner.IsCreepJoiner && prisoner.creepjoiner.CanTriggerAggressive)
			{
				prisoner.creepjoiner.DoAggressive();
				return false;
			}
			if (prisoner.Faction != null && prisoner.Faction.def.hidden)
			{
				prisoner.SetFactionDirect(null);
			}
			bool everEnslaved = prisoner.guest.EverEnslaved;
			prisoner.guest.SetGuestStatus(warden.Faction, GuestStatus.Slave);
			Messages.Message("MessagePrisonerEnslaved".Translate(prisoner, warden), new LookTargets(new TargetInfo[]
			{
				prisoner,
				warden
			}), MessageTypeDefOf.NeutralEvent, true);
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.EnslavedPrisoner, warden.Named(HistoryEventArgsNames.Doer)), true);
			if (!everEnslaved)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.EnslavedPrisonerNotPreviouslyEnslaved, warden.Named(HistoryEventArgsNames.Doer)), true);
			}
			prisoner.apparel.UnlockAll();
			return true;
		}
	}
}
