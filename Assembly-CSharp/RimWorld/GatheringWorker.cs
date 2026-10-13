using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001A3C RID: 6716
	public abstract class GatheringWorker
	{
		// Token: 0x06009744 RID: 38724 RVA: 0x002C312C File Offset: 0x002C132C
		public virtual bool CanExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			IntVec3 intVec;
			return organizer != null && this.TryFindGatherSpot(organizer, out intVec) && GatheringsUtility.PawnCanStartOrContinueGathering(organizer);
		}

		// Token: 0x06009745 RID: 38725 RVA: 0x002C3164 File Offset: 0x002C1364
		public virtual bool TryExecute(Map map, Pawn organizer = null)
		{
			if (organizer == null)
			{
				organizer = this.FindOrganizer(map);
			}
			if (organizer == null)
			{
				return false;
			}
			IntVec3 spot;
			if (!this.TryFindGatherSpot(organizer, out spot))
			{
				return false;
			}
			LordJob lordJob = this.CreateLordJob(spot, organizer);
			Faction faction = organizer.Faction;
			LordJob lordJob2 = lordJob;
			Map map2 = organizer.Map;
			object startingPawns;
			if (!lordJob.OrganizerIsStartingPawn)
			{
				startingPawns = null;
			}
			else
			{
				(startingPawns = new Pawn[1])[0] = organizer;
			}
			LordMaker.MakeNewLord(faction, lordJob2, map2, startingPawns);
			this.SendLetter(spot, organizer);
			return true;
		}

		// Token: 0x06009746 RID: 38726 RVA: 0x002C31CC File Offset: 0x002C13CC
		protected virtual void SendLetter(IntVec3 spot, Pawn organizer)
		{
			Find.LetterStack.ReceiveLetter(this.def.letterTitle, this.def.letterText.Formatted(organizer.Named("ORGANIZER")), LetterDefOf.PositiveEvent, new TargetInfo(spot, organizer.Map, false), null, null, null, null, 0, true);
		}

		// Token: 0x06009747 RID: 38727 RVA: 0x002C322B File Offset: 0x002C142B
		protected virtual Pawn FindOrganizer(Map map)
		{
			return GatheringsUtility.FindRandomGatheringOrganizer(Faction.OfPlayer, map, this.def);
		}

		// Token: 0x06009748 RID: 38728
		protected abstract bool TryFindGatherSpot(Pawn organizer, out IntVec3 spot);

		// Token: 0x06009749 RID: 38729
		protected abstract LordJob CreateLordJob(IntVec3 spot, Pawn organizer);

		// Token: 0x04006453 RID: 25683
		public GatheringDef def;
	}
}
