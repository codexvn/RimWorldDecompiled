using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002505 RID: 9477
	public class Pawn_DrugPolicyTracker : IExposable
	{
		// Token: 0x17001FC4 RID: 8132
		// (get) Token: 0x0600CF0B RID: 53003 RVA: 0x003CC72C File Offset: 0x003CA92C
		// (set) Token: 0x0600CF0C RID: 53004 RVA: 0x003CC782 File Offset: 0x003CA982
		public DrugPolicy CurrentPolicy
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.disablePolicies)
				{
					return null;
				}
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.drugPolicyDatabase.DefaultDrugPolicy();
				}
				return this.curPolicy;
			}
			set
			{
				if (this.curPolicy == value)
				{
					return;
				}
				this.curPolicy = value;
			}
		}

		// Token: 0x17001FC5 RID: 8133
		// (get) Token: 0x0600CF0D RID: 53005 RVA: 0x003CC798 File Offset: 0x003CA998
		private float DayPercentNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return Mathf.InverseLerp(6f, 22f, GenLocalDate.HourFloat(this.pawn));
				}
				if (this.pawn.timetable == null)
				{
					return GenLocalDate.DayPercent(this.pawn);
				}
				float hoursPerDayNotSleeping = this.HoursPerDayNotSleeping;
				if (hoursPerDayNotSleeping == 0f)
				{
					return 1f;
				}
				float num = 0f;
				int num2 = GenLocalDate.HourOfDay(this.pawn);
				for (int i = 0; i < num2; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num += 1f;
					}
				}
				if (this.pawn.timetable.CurrentAssignment != TimeAssignmentDefOf.Sleep)
				{
					float num3 = (float)(Find.TickManager.TicksAbs % 2500) / 2500f;
					num += num3;
				}
				return num / hoursPerDayNotSleeping;
			}
		}

		// Token: 0x17001FC6 RID: 8134
		// (get) Token: 0x0600CF0E RID: 53006 RVA: 0x003CC878 File Offset: 0x003CAA78
		private float HoursPerDayNotSleeping
		{
			get
			{
				if (this.pawn.IsCaravanMember())
				{
					return 16f;
				}
				int num = 0;
				for (int i = 0; i < 24; i++)
				{
					if (this.pawn.timetable.times[i] != TimeAssignmentDefOf.Sleep)
					{
						num++;
					}
				}
				return (float)num;
			}
		}

		// Token: 0x0600CF0F RID: 53007 RVA: 0x003CC8CA File Offset: 0x003CAACA
		public Pawn_DrugPolicyTracker()
		{
		}

		// Token: 0x0600CF10 RID: 53008 RVA: 0x003CC8DD File Offset: 0x003CAADD
		public Pawn_DrugPolicyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CF11 RID: 53009 RVA: 0x003CC8F8 File Offset: 0x003CAAF8
		public void ExposeData()
		{
			Scribe_References.Look<DrugPolicy>(ref this.curPolicy, "curAssignedDrugs", false);
			Scribe_Collections.Look<DrugTakeRecord>(ref this.drugTakeRecords, "drugTakeRecords", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.drugTakeRecords.RemoveAll((DrugTakeRecord x) => x.drug == null) != 0)
				{
					Log.ErrorOnce("Removed some null drugs from drug policy tracker", 816929737);
				}
			}
		}

		// Token: 0x0600CF12 RID: 53010 RVA: 0x003CC970 File Offset: 0x003CAB70
		public bool HasEverTaken(ThingDef drug)
		{
			if (!drug.IsDrug)
			{
				ThingDef drug2 = drug;
				Log.Warning(((drug2 != null) ? drug2.ToString() : null) + " is not a drug.");
				return false;
			}
			return this.drugTakeRecords.Any((DrugTakeRecord x) => x.drug == drug);
		}

		// Token: 0x0600CF13 RID: 53011 RVA: 0x003CC9D4 File Offset: 0x003CABD4
		public bool AllowedToTakeToInventory(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(((thingDef != null) ? thingDef.ToString() : null) + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + ((thingDef != null) ? thingDef.ToString() : null));
				return false;
			}
			if (thingDef.IsNonMedicalDrug && !this.pawn.CanTakeDrug(thingDef))
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			return !drugPolicyEntry.allowScheduled && drugPolicyEntry.takeToInventory > 0 && !this.pawn.inventory.innerContainer.Contains(thingDef);
		}

		// Token: 0x0600CF14 RID: 53012 RVA: 0x003CCA80 File Offset: 0x003CAC80
		public bool AllowedToTakeScheduledEver(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				Log.Error(((thingDef != null) ? thingDef.ToString() : null) + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				Log.Error("AllowedToTakeScheduledEver on non-drug " + ((thingDef != null) ? thingDef.ToString() : null));
				return false;
			}
			return this.CurrentPolicy[thingDef].allowScheduled && (!thingDef.IsNonMedicalDrug || this.pawn.CanTakeDrug(thingDef)) && (thingDef.ingestible.drugCategory != DrugCategory.Hard || new HistoryEvent(HistoryEventDefOf.IngestedHardDrug, this.pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo());
		}

		// Token: 0x0600CF15 RID: 53013 RVA: 0x003CCB38 File Offset: 0x003CAD38
		public bool AllowedToTakeScheduledNow(ThingDef thingDef)
		{
			if (!thingDef.IsIngestible)
			{
				ThingDef thingDef2 = thingDef;
				Log.Error(((thingDef2 != null) ? thingDef2.ToString() : null) + " is not ingestible.");
				return false;
			}
			if (!thingDef.IsDrug)
			{
				string str = "AllowedToTakeScheduledEver on non-drug ";
				ThingDef thingDef3 = thingDef;
				Log.Error(str + ((thingDef3 != null) ? thingDef3.ToString() : null));
				return false;
			}
			if (!this.AllowedToTakeScheduledEver(thingDef))
			{
				return false;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[thingDef];
			if (drugPolicyEntry.onlyIfMoodBelow < 1f && this.pawn.needs.mood != null && this.pawn.needs.mood.CurLevelPercentage >= drugPolicyEntry.onlyIfMoodBelow)
			{
				return false;
			}
			if (drugPolicyEntry.onlyIfJoyBelow < 1f && this.pawn.needs.joy != null && this.pawn.needs.joy.CurLevelPercentage >= drugPolicyEntry.onlyIfJoyBelow)
			{
				return false;
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == thingDef);
			if (drugTakeRecord != null)
			{
				if (drugPolicyEntry.daysFrequency < 1f)
				{
					int num = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
					if (drugTakeRecord.TimesTakenThisDay >= num)
					{
						return false;
					}
				}
				else
				{
					int num2 = Mathf.Abs(GenDate.DaysPassed - drugTakeRecord.LastTakenDays);
					int num3 = Mathf.RoundToInt(drugPolicyEntry.daysFrequency);
					if (num2 < num3)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600CF16 RID: 53014 RVA: 0x003CCCBC File Offset: 0x003CAEBC
		public bool ShouldTryToTakeScheduledNow(ThingDef ingestible)
		{
			if (!ingestible.IsDrug)
			{
				return false;
			}
			if (!this.AllowedToTakeScheduledNow(ingestible))
			{
				return false;
			}
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
			if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.5f && this.CanCauseOverdose(ingestible))
			{
				int num = this.LastTicksWhenTakenDrugWhichCanCauseOverdose();
				if (Find.TickManager.TicksGame - num < 1250)
				{
					return false;
				}
			}
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == ingestible);
			if (drugTakeRecord == null)
			{
				return true;
			}
			DrugPolicyEntry drugPolicyEntry = this.CurrentPolicy[ingestible];
			if (drugPolicyEntry.daysFrequency < 1f)
			{
				int num2 = Mathf.RoundToInt(1f / drugPolicyEntry.daysFrequency);
				float num3 = 1f / (float)(num2 + 1);
				int num4 = 0;
				float dayPercentNotSleeping = this.DayPercentNotSleeping;
				for (int i = 0; i < num2; i++)
				{
					if (dayPercentNotSleeping > (float)(i + 1) * num3 - num3 * 0.5f)
					{
						num4++;
					}
				}
				return drugTakeRecord.TimesTakenThisDay < num4 && (drugTakeRecord.TimesTakenThisDay == 0 || (float)(Find.TickManager.TicksGame - drugTakeRecord.lastTakenTicks) / (this.HoursPerDayNotSleeping * 2500f) >= 0.6f * num3);
			}
			float dayPercentNotSleeping2 = this.DayPercentNotSleeping;
			Rand.PushState();
			Rand.Seed = Gen.HashCombineInt(GenDate.DaysPassed, this.pawn.thingIDNumber);
			bool result = dayPercentNotSleeping2 >= Rand.Range(0.1f, 0.35f);
			Rand.PopState();
			return result;
		}

		// Token: 0x0600CF17 RID: 53015 RVA: 0x003CCE64 File Offset: 0x003CB064
		public void Notify_DrugIngested(Thing drug)
		{
			DrugTakeRecord drugTakeRecord = this.drugTakeRecords.Find((DrugTakeRecord x) => x.drug == drug.def);
			if (drugTakeRecord == null)
			{
				drugTakeRecord = new DrugTakeRecord();
				drugTakeRecord.drug = drug.def;
				this.drugTakeRecords.Add(drugTakeRecord);
			}
			drugTakeRecord.lastTakenTicks = Find.TickManager.TicksGame;
			DrugTakeRecord drugTakeRecord2 = drugTakeRecord;
			int timesTakenThisDay = drugTakeRecord2.TimesTakenThisDay;
			drugTakeRecord2.TimesTakenThisDay = timesTakenThisDay + 1;
		}

		// Token: 0x0600CF18 RID: 53016 RVA: 0x003CCEDC File Offset: 0x003CB0DC
		public void Notify_LeftSuspension(int suspendedTicks)
		{
			foreach (DrugTakeRecord drugTakeRecord in this.drugTakeRecords)
			{
				drugTakeRecord.Notify_LeftSuspension(suspendedTicks);
			}
		}

		// Token: 0x0600CF19 RID: 53017 RVA: 0x003CCF30 File Offset: 0x003CB130
		private int LastTicksWhenTakenDrugWhichCanCauseOverdose()
		{
			int num = -999999;
			for (int i = 0; i < this.drugTakeRecords.Count; i++)
			{
				if (this.CanCauseOverdose(this.drugTakeRecords[i].drug))
				{
					num = Mathf.Max(num, this.drugTakeRecords[i].lastTakenTicks);
				}
			}
			return num;
		}

		// Token: 0x0600CF1A RID: 53018 RVA: 0x003CCF8C File Offset: 0x003CB18C
		private bool CanCauseOverdose(ThingDef drug)
		{
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			return compProperties != null && compProperties.CanCauseOverdose;
		}

		// Token: 0x04008CE7 RID: 36071
		public Pawn pawn;

		// Token: 0x04008CE8 RID: 36072
		private DrugPolicy curPolicy;

		// Token: 0x04008CE9 RID: 36073
		private List<DrugTakeRecord> drugTakeRecords = new List<DrugTakeRecord>();

		// Token: 0x04008CEA RID: 36074
		private const float DangerousDrugOverdoseSeverity = 0.5f;
	}
}
