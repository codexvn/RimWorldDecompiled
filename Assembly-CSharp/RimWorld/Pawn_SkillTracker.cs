using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002511 RID: 9489
	public class Pawn_SkillTracker : IExposable
	{
		// Token: 0x17001FD2 RID: 8146
		// (get) Token: 0x0600CF51 RID: 53073 RVA: 0x003CD5C4 File Offset: 0x003CB7C4
		public int PassionCount
		{
			get
			{
				int num = 0;
				foreach (SkillRecord skillRecord in this.skills)
				{
					if (!skillRecord.TotallyDisabled)
					{
						if (skillRecord.passion == Passion.Major)
						{
							num += 2;
						}
						else if (skillRecord.passion == Passion.Minor)
						{
							num++;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x0600CF52 RID: 53074 RVA: 0x003CD638 File Offset: 0x003CB838
		public Pawn_SkillTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			foreach (SkillDef def in DefDatabase<SkillDef>.AllDefs)
			{
				this.skills.Add(new SkillRecord(this.pawn, def));
			}
		}

		// Token: 0x0600CF53 RID: 53075 RVA: 0x003CD6B4 File Offset: 0x003CB8B4
		public void ExposeData()
		{
			Scribe_Collections.Look<SkillRecord>(ref this.skills, "skills", LookMode.Deep, new object[]
			{
				this.pawn
			});
			Scribe_Values.Look<int>(ref this.lastXpSinceMidnightResetTimestamp, "lastXpSinceMidnightResetTimestamp", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.skills.RemoveAll((SkillRecord x) => x == null) != 0)
				{
					Log.Error("Some skills were null after loading for " + this.pawn.ToStringSafe<Pawn>());
				}
				if (this.skills.RemoveAll((SkillRecord x) => x.def == null) != 0)
				{
					Log.Error("Some skills had null def after loading for " + this.pawn.ToStringSafe<Pawn>());
				}
				List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					bool flag = false;
					for (int j = 0; j < this.skills.Count; j++)
					{
						if (this.skills[j].def == allDefsListForReading[i])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						Log.Warning(this.pawn.ToStringSafe<Pawn>() + " had no " + allDefsListForReading[i].ToStringSafe<SkillDef>() + " skill. Adding.");
						this.skills.Add(new SkillRecord(this.pawn, allDefsListForReading[i]));
					}
				}
			}
		}

		// Token: 0x0600CF54 RID: 53076 RVA: 0x003CD828 File Offset: 0x003CBA28
		public SkillRecord GetSkill(SkillDef skillDef)
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				if (this.skills[i].def == skillDef)
				{
					return this.skills[i];
				}
			}
			string str = "Did not find skill of def ";
			string str2 = (skillDef != null) ? skillDef.ToString() : null;
			string str3 = ", returning ";
			SkillRecord skillRecord = this.skills[0];
			Log.Error(str + str2 + str3 + ((skillRecord != null) ? skillRecord.ToString() : null));
			return this.skills[0];
		}

		// Token: 0x0600CF55 RID: 53077 RVA: 0x003CD8B4 File Offset: 0x003CBAB4
		public void SkillsTickInterval(int delta)
		{
			if (!this.pawn.IsHashIntervalTick(200, delta))
			{
				return;
			}
			if (!this.CanGainXP())
			{
				return;
			}
			if (GenLocalDate.HourInteger(this.pawn) == 0 && (this.lastXpSinceMidnightResetTimestamp < 0 || Find.TickManager.TicksGame - this.lastXpSinceMidnightResetTimestamp >= 30000))
			{
				for (int i = 0; i < this.skills.Count; i++)
				{
					this.skills[i].xpSinceMidnight = 0f;
				}
				this.lastXpSinceMidnightResetTimestamp = Find.TickManager.TicksGame;
			}
			for (int j = 0; j < this.skills.Count; j++)
			{
				this.skills[j].Interval();
			}
		}

		// Token: 0x0600CF56 RID: 53078 RVA: 0x003CD96F File Offset: 0x003CBB6F
		private bool CanGainXP()
		{
			return !ModsConfig.AnomalyActive || !this.pawn.IsMutant || this.pawn.mutant.Def.canGainXP;
		}

		// Token: 0x0600CF57 RID: 53079 RVA: 0x003CD99F File Offset: 0x003CBB9F
		public void Learn(SkillDef sDef, float xp, bool direct = false, bool ignoreLearnRate = false)
		{
			if (!this.CanGainXP())
			{
				return;
			}
			this.GetSkill(sDef).Learn(xp, direct, ignoreLearnRate);
		}

		// Token: 0x0600CF58 RID: 53080 RVA: 0x003CD9BC File Offset: 0x003CBBBC
		public float AverageOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			if (workDef.relevantSkills.Count == 0)
			{
				return 3f;
			}
			float num = 0f;
			for (int i = 0; i < workDef.relevantSkills.Count; i++)
			{
				num += (float)this.GetSkill(workDef.relevantSkills[i]).Level;
			}
			return num / (float)workDef.relevantSkills.Count;
		}

		// Token: 0x0600CF59 RID: 53081 RVA: 0x003CDA24 File Offset: 0x003CBC24
		public Passion MaxPassionOfRelevantSkillsFor(WorkTypeDef workDef)
		{
			if (workDef.relevantSkills.Count == 0)
			{
				return Passion.None;
			}
			Passion passion = Passion.None;
			for (int i = 0; i < workDef.relevantSkills.Count; i++)
			{
				Passion passion2 = this.GetSkill(workDef.relevantSkills[i]).passion;
				if (passion2 > passion)
				{
					passion = passion2;
				}
			}
			return passion;
		}

		// Token: 0x0600CF5A RID: 53082 RVA: 0x003CDA78 File Offset: 0x003CBC78
		public void Notify_SkillDisablesChanged()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].Notify_SkillDisablesChanged();
			}
		}

		// Token: 0x0600CF5B RID: 53083 RVA: 0x003CDAAC File Offset: 0x003CBCAC
		public void DirtyAptitudes()
		{
			for (int i = 0; i < this.skills.Count; i++)
			{
				this.skills[i].DirtyAptitudes();
			}
		}

		// Token: 0x04008CFE RID: 36094
		private const int MinorPassionWeight = 1;

		// Token: 0x04008CFF RID: 36095
		private const int MajorPassionWeight = 2;

		// Token: 0x04008D00 RID: 36096
		private Pawn pawn;

		// Token: 0x04008D01 RID: 36097
		public List<SkillRecord> skills = new List<SkillRecord>();

		// Token: 0x04008D02 RID: 36098
		private int lastXpSinceMidnightResetTimestamp = -1;
	}
}
