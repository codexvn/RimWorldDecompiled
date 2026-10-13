using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E66 RID: 7782
	public class StorytellerComp_RefiringUniqueQuest : StorytellerComp
	{
		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x0600ACE7 RID: 44263 RVA: 0x0031376A File Offset: 0x0031196A
		private int IntervalsPassed
		{
			get
			{
				return Find.TickManager.TicksGame / 1000;
			}
		}

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x0600ACE8 RID: 44264 RVA: 0x00315A48 File Offset: 0x00313C48
		private StorytellerCompProperties_RefiringUniqueQuest Props
		{
			get
			{
				return (StorytellerCompProperties_RefiringUniqueQuest)this.props;
			}
		}

		// Token: 0x0600ACE9 RID: 44265 RVA: 0x00315A55 File Offset: 0x00313C55
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (!this.Props.incident.TargetAllowed(target))
			{
				yield break;
			}
			Quest quest = null;
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				if (questsListForReading[i].root == this.Props.incident.questScriptDef && (quest == null || questsListForReading[i].appearanceTick > quest.appearanceTick))
				{
					quest = questsListForReading[i];
					break;
				}
			}
			bool flag;
			if (quest == null)
			{
				if (this.Props.minColonyWealth > 0 && WealthUtility.PlayerWealth < (float)this.Props.minColonyWealth)
				{
					flag = false;
				}
				else if (this.generateSkipped)
				{
					flag = ((float)GenTicks.TicksGame >= this.Props.minDaysPassed * 60000f);
				}
				else
				{
					flag = (this.IntervalsPassed == (int)(this.Props.minDaysPassed * 60f) + 1);
				}
			}
			else
			{
				flag = (this.Props.refireEveryDays >= 0f && (quest.State != QuestState.EndedSuccess && quest.State != QuestState.Ongoing && quest.State != QuestState.NotYetAccepted && quest.cleanupTick >= 0 && this.IntervalsPassed == (int)((float)quest.cleanupTick + this.Props.refireEveryDays * 60000f) / 1000));
			}
			if (flag)
			{
				IncidentParms parms = this.GenerateParms(this.Props.incident.category, target);
				if (this.Props.incident.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(this.Props.incident, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x0600ACEA RID: 44266 RVA: 0x00315A6C File Offset: 0x00313C6C
		public override void Initialize()
		{
			base.Initialize();
			if ((float)GenTicks.TicksGame >= this.Props.minDaysPassed * 60000f)
			{
				this.generateSkipped = true;
			}
		}

		// Token: 0x0600ACEB RID: 44267 RVA: 0x00315A94 File Offset: 0x00313C94
		public override string ToString()
		{
			string str = base.ToString();
			string str2 = " ";
			IncidentDef incident = this.Props.incident;
			return str + str2 + ((incident != null) ? incident.ToString() : null);
		}

		// Token: 0x040076EF RID: 30447
		private bool generateSkipped;
	}
}
