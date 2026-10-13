using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002835 RID: 10293
	public class UniqueIDsManager : IExposable
	{
		// Token: 0x0600E10B RID: 57611 RVA: 0x00420612 File Offset: 0x0041E812
		public int GetNextThingID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextThingID);
		}

		// Token: 0x0600E10C RID: 57612 RVA: 0x0042061F File Offset: 0x0041E81F
		public int GetNextBillID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBillID);
		}

		// Token: 0x0600E10D RID: 57613 RVA: 0x0042062C File Offset: 0x0041E82C
		public int GetNextFactionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextFactionID);
		}

		// Token: 0x0600E10E RID: 57614 RVA: 0x00420639 File Offset: 0x0041E839
		public int GetNextLordID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLordID);
		}

		// Token: 0x0600E10F RID: 57615 RVA: 0x00420646 File Offset: 0x0041E846
		public int GetNextTaleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTaleID);
		}

		// Token: 0x0600E110 RID: 57616 RVA: 0x00420653 File Offset: 0x0041E853
		public int GetNextPassingShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPassingShipID);
		}

		// Token: 0x0600E111 RID: 57617 RVA: 0x00420660 File Offset: 0x0041E860
		public int GetNextWorldObjectID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldObjectID);
		}

		// Token: 0x0600E112 RID: 57618 RVA: 0x0042066D File Offset: 0x0041E86D
		public int GetNextMapID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMapID);
		}

		// Token: 0x0600E113 RID: 57619 RVA: 0x0042067A File Offset: 0x0041E87A
		public int GetNextCaravanID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextCaravanID);
		}

		// Token: 0x0600E114 RID: 57620 RVA: 0x00420687 File Offset: 0x0041E887
		public int GetNextAreaID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAreaID);
		}

		// Token: 0x0600E115 RID: 57621 RVA: 0x00420694 File Offset: 0x0041E894
		public int GetNextAnimalPenID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAnimalPenID);
		}

		// Token: 0x0600E116 RID: 57622 RVA: 0x004206A1 File Offset: 0x0041E8A1
		public int GetNextTransporterGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransporterGroupID);
		}

		// Token: 0x0600E117 RID: 57623 RVA: 0x004206AE File Offset: 0x0041E8AE
		public int GetNextAncientCryptosleepCasketGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAncientCryptosleepCasketGroupID);
		}

		// Token: 0x0600E118 RID: 57624 RVA: 0x004206BB File Offset: 0x0041E8BB
		public int GetNextJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextJobID);
		}

		// Token: 0x0600E119 RID: 57625 RVA: 0x004206C8 File Offset: 0x0041E8C8
		public int GetNextSignalTagID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextSignalTagID);
		}

		// Token: 0x0600E11A RID: 57626 RVA: 0x004206D5 File Offset: 0x0041E8D5
		public int GetNextWorldFeatureID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextWorldFeatureID);
		}

		// Token: 0x0600E11B RID: 57627 RVA: 0x004206E2 File Offset: 0x0041E8E2
		public int GetNextHediffID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextHediffID);
		}

		// Token: 0x0600E11C RID: 57628 RVA: 0x004206EF File Offset: 0x0041E8EF
		public int GetNextBattleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextBattleID);
		}

		// Token: 0x0600E11D RID: 57629 RVA: 0x004206FC File Offset: 0x0041E8FC
		public int GetNextLogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLogID);
		}

		// Token: 0x0600E11E RID: 57630 RVA: 0x00420709 File Offset: 0x0041E909
		public int GetNextLetterID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextLetterID);
		}

		// Token: 0x0600E11F RID: 57631 RVA: 0x00420716 File Offset: 0x0041E916
		public int GetNextArchivedDialogID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextArchivedDialogID);
		}

		// Token: 0x0600E120 RID: 57632 RVA: 0x00420723 File Offset: 0x0041E923
		public int GetNextMessageID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextMessageID);
		}

		// Token: 0x0600E121 RID: 57633 RVA: 0x00420730 File Offset: 0x0041E930
		public int GetNextZoneID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextZoneID);
		}

		// Token: 0x0600E122 RID: 57634 RVA: 0x0042073D File Offset: 0x0041E93D
		public int GetNextPlanID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPlanID);
		}

		// Token: 0x0600E123 RID: 57635 RVA: 0x0042074A File Offset: 0x0041E94A
		public int GetNextQuestID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextQuestID);
		}

		// Token: 0x0600E124 RID: 57636 RVA: 0x00420757 File Offset: 0x0041E957
		public int GetNextGameConditionID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextGameConditionID);
		}

		// Token: 0x0600E125 RID: 57637 RVA: 0x00420764 File Offset: 0x0041E964
		public int GetNextIdeoID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextIdeoID);
		}

		// Token: 0x0600E126 RID: 57638 RVA: 0x00420771 File Offset: 0x0041E971
		public int GetNextPreceptID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPreceptID);
		}

		// Token: 0x0600E127 RID: 57639 RVA: 0x0042077E File Offset: 0x0041E97E
		public int GetNextRitualObligationID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextRitualObligationID);
		}

		// Token: 0x0600E128 RID: 57640 RVA: 0x0042078B File Offset: 0x0041E98B
		public int GetNextPresenceDemandID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPresenceDemandID);
		}

		// Token: 0x0600E129 RID: 57641 RVA: 0x00420798 File Offset: 0x0041E998
		public int GetNextTransportShipID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextTransportShipID);
		}

		// Token: 0x0600E12A RID: 57642 RVA: 0x004207A5 File Offset: 0x0041E9A5
		public int GetNextShipJobID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextShipJobID);
		}

		// Token: 0x0600E12B RID: 57643 RVA: 0x004207B2 File Offset: 0x0041E9B2
		public int GetNextRitualRoleID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextRitualRoleID);
		}

		// Token: 0x0600E12C RID: 57644 RVA: 0x004207BF File Offset: 0x0041E9BF
		public int GetNextAbilityID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextAbilityID);
		}

		// Token: 0x0600E12D RID: 57645 RVA: 0x004207CC File Offset: 0x0041E9CC
		public int GetNextGeneID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextGeneID);
		}

		// Token: 0x0600E12E RID: 57646 RVA: 0x004207D9 File Offset: 0x0041E9D9
		public int GetNextCocoonGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextCocoonGroupID);
		}

		// Token: 0x0600E12F RID: 57647 RVA: 0x004207E6 File Offset: 0x0041E9E6
		public int GetNextStorageGroupID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextStorageGroupID);
		}

		// Token: 0x0600E130 RID: 57648 RVA: 0x004207F3 File Offset: 0x0041E9F3
		public int GetNextPitGateIncidentID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPitGateIncidentID);
		}

		// Token: 0x0600E131 RID: 57649 RVA: 0x00420800 File Offset: 0x0041EA00
		public int GetNextPsychicRitualID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPsychicRitualID);
		}

		// Token: 0x0600E132 RID: 57650 RVA: 0x0042080D File Offset: 0x0041EA0D
		public int GetNextPsychicRitualToilID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextPsychicRitualToilID);
		}

		// Token: 0x0600E133 RID: 57651 RVA: 0x0042081A File Offset: 0x0041EA1A
		public int GetNextStructureSketchID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextStructureSketchID);
		}

		// Token: 0x0600E134 RID: 57652 RVA: 0x00420827 File Offset: 0x0041EA27
		public int GetNextRoomID()
		{
			return UniqueIDsManager.GetNextID(ref this.nextRoomID);
		}

		// Token: 0x0600E135 RID: 57653 RVA: 0x00420834 File Offset: 0x0041EA34
		public UniqueIDsManager()
		{
			this.nextThingID = Rand.Range(0, 1000);
		}

		// Token: 0x0600E136 RID: 57654 RVA: 0x00420850 File Offset: 0x0041EA50
		private static int GetNextID(ref int nextID)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars && !Find.UniqueIDsManager.wasLoaded)
			{
				Log.Warning("Getting next unique ID during LoadingVars before UniqueIDsManager was loaded. Assigning a random value.");
				return Rand.Int;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Log.Warning("Getting next unique ID during saving This may cause bugs.");
			}
			int result = nextID;
			nextID++;
			if (nextID == 2147483647)
			{
				Log.Warning("Next ID is at max value. Resetting to 0. This may cause bugs.");
				nextID = 0;
			}
			return result;
		}

		// Token: 0x0600E137 RID: 57655 RVA: 0x004208B4 File Offset: 0x0041EAB4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextThingID, "nextThingID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBillID, "nextBillID", 0, false);
			Scribe_Values.Look<int>(ref this.nextFactionID, "nextFactionID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLordID, "nextLordID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTaleID, "nextTaleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPassingShipID, "nextPassingShipID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldObjectID, "nextWorldObjectID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMapID, "nextMapID", 0, false);
			Scribe_Values.Look<int>(ref this.nextCaravanID, "nextCaravanID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAreaID, "nextAreaID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAnimalPenID, "nextAnimalPenID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTransporterGroupID, "nextTransporterGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAncientCryptosleepCasketGroupID, "nextAncientCryptosleepCasketGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextJobID, "nextJobID", 0, false);
			Scribe_Values.Look<int>(ref this.nextSignalTagID, "nextSignalTagID", 0, false);
			Scribe_Values.Look<int>(ref this.nextWorldFeatureID, "nextWorldFeatureID", 0, false);
			Scribe_Values.Look<int>(ref this.nextHediffID, "nextHediffID", 0, false);
			Scribe_Values.Look<int>(ref this.nextBattleID, "nextBattleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLogID, "nextLogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextLetterID, "nextLetterID", 0, false);
			Scribe_Values.Look<int>(ref this.nextArchivedDialogID, "nextArchivedDialogID", 0, false);
			Scribe_Values.Look<int>(ref this.nextMessageID, "nextMessageID", 0, false);
			Scribe_Values.Look<int>(ref this.nextZoneID, "nextZoneID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPlanID, "nextPlanID", 0, false);
			Scribe_Values.Look<int>(ref this.nextQuestID, "nextQuestID", 0, false);
			Scribe_Values.Look<int>(ref this.nextGameConditionID, "nextGameConditionID", 0, false);
			Scribe_Values.Look<int>(ref this.nextIdeoID, "nextIdeoID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPreceptID, "nextPreceptID", 0, false);
			Scribe_Values.Look<int>(ref this.nextRitualObligationID, "nextRitualObligationID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPresenceDemandID, "nextPresenceDemandID", 0, false);
			Scribe_Values.Look<int>(ref this.nextTransportShipID, "nextTransportShipID", 0, false);
			Scribe_Values.Look<int>(ref this.nextShipJobID, "nextShipJobID", 0, false);
			Scribe_Values.Look<int>(ref this.nextRitualRoleID, "nextRitualRoleID", 0, false);
			Scribe_Values.Look<int>(ref this.nextAbilityID, "nextAbilityID", 0, false);
			Scribe_Values.Look<int>(ref this.nextGeneID, "nextGeneID", 0, false);
			Scribe_Values.Look<int>(ref this.nextCocoonGroupID, "nextCocoonGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextStorageGroupID, "nextStorageGroupID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPitGateIncidentID, "nextPitGateIncidentID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPsychicRitualToilID, "nextPsychicRitualToilID", 0, false);
			Scribe_Values.Look<int>(ref this.nextPsychicRitualID, "nextPsychicRitualID", 0, false);
			Scribe_Values.Look<int>(ref this.nextStructureSketchID, "nextStructureSketchID", 0, false);
			Scribe_Values.Look<int>(ref this.nextRoomID, "nextRoomID", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.wasLoaded = true;
			}
		}

		// Token: 0x04009741 RID: 38721
		private int nextThingID;

		// Token: 0x04009742 RID: 38722
		private int nextBillID;

		// Token: 0x04009743 RID: 38723
		private int nextFactionID;

		// Token: 0x04009744 RID: 38724
		private int nextLordID;

		// Token: 0x04009745 RID: 38725
		private int nextTaleID;

		// Token: 0x04009746 RID: 38726
		private int nextPassingShipID;

		// Token: 0x04009747 RID: 38727
		private int nextWorldObjectID;

		// Token: 0x04009748 RID: 38728
		private int nextMapID;

		// Token: 0x04009749 RID: 38729
		private int nextCaravanID;

		// Token: 0x0400974A RID: 38730
		private int nextAreaID;

		// Token: 0x0400974B RID: 38731
		private int nextAnimalPenID;

		// Token: 0x0400974C RID: 38732
		private int nextTransporterGroupID;

		// Token: 0x0400974D RID: 38733
		private int nextAncientCryptosleepCasketGroupID;

		// Token: 0x0400974E RID: 38734
		private int nextJobID;

		// Token: 0x0400974F RID: 38735
		private int nextSignalTagID;

		// Token: 0x04009750 RID: 38736
		private int nextWorldFeatureID;

		// Token: 0x04009751 RID: 38737
		private int nextHediffID;

		// Token: 0x04009752 RID: 38738
		private int nextBattleID;

		// Token: 0x04009753 RID: 38739
		private int nextLogID;

		// Token: 0x04009754 RID: 38740
		private int nextLetterID;

		// Token: 0x04009755 RID: 38741
		private int nextArchivedDialogID;

		// Token: 0x04009756 RID: 38742
		private int nextMessageID;

		// Token: 0x04009757 RID: 38743
		private int nextZoneID;

		// Token: 0x04009758 RID: 38744
		private int nextPlanID;

		// Token: 0x04009759 RID: 38745
		private int nextQuestID;

		// Token: 0x0400975A RID: 38746
		private int nextGameConditionID;

		// Token: 0x0400975B RID: 38747
		private int nextIdeoID;

		// Token: 0x0400975C RID: 38748
		private int nextPreceptID;

		// Token: 0x0400975D RID: 38749
		private int nextRitualObligationID;

		// Token: 0x0400975E RID: 38750
		private int nextPresenceDemandID;

		// Token: 0x0400975F RID: 38751
		private int nextTransportShipID;

		// Token: 0x04009760 RID: 38752
		private int nextShipJobID;

		// Token: 0x04009761 RID: 38753
		private int nextRitualRoleID;

		// Token: 0x04009762 RID: 38754
		private int nextAbilityID;

		// Token: 0x04009763 RID: 38755
		private int nextGeneID;

		// Token: 0x04009764 RID: 38756
		private int nextCocoonGroupID;

		// Token: 0x04009765 RID: 38757
		private int nextStorageGroupID;

		// Token: 0x04009766 RID: 38758
		private int nextPitGateIncidentID;

		// Token: 0x04009767 RID: 38759
		private int nextPsychicRitualToilID;

		// Token: 0x04009768 RID: 38760
		private int nextPsychicRitualID;

		// Token: 0x04009769 RID: 38761
		private int nextStructureSketchID;

		// Token: 0x0400976A RID: 38762
		private int nextRoomID;

		// Token: 0x0400976B RID: 38763
		private bool wasLoaded;
	}
}
