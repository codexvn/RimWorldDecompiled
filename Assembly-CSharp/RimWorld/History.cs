using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B59 RID: 7001
	public sealed class History : IExposable
	{
		// Token: 0x06009C66 RID: 40038 RVA: 0x002D5E6C File Offset: 0x002D406C
		public History()
		{
			this.autoRecorderGroups = new List<HistoryAutoRecorderGroup>();
			this.AddOrRemoveHistoryRecorderGroups();
			this.curveDrawerStyle = new SimpleCurveDrawerStyle();
			this.curveDrawerStyle.DrawMeasures = true;
			this.curveDrawerStyle.DrawPoints = false;
			this.curveDrawerStyle.DrawBackground = true;
			this.curveDrawerStyle.DrawBackgroundLines = false;
			this.curveDrawerStyle.DrawLegend = true;
			this.curveDrawerStyle.DrawCurveMousePoint = true;
			this.curveDrawerStyle.OnlyPositiveValues = true;
			this.curveDrawerStyle.UseFixedSection = true;
			this.curveDrawerStyle.UseAntiAliasedLines = true;
			this.curveDrawerStyle.PointsRemoveOptimization = true;
			this.curveDrawerStyle.MeasureLabelsXCount = 10;
			this.curveDrawerStyle.MeasureLabelsYCount = 5;
			this.curveDrawerStyle.XIntegersOnly = true;
			this.curveDrawerStyle.LabelX = "Day".Translate();
		}

		// Token: 0x06009C67 RID: 40039 RVA: 0x002D5F80 File Offset: 0x002D4180
		public void HistoryTick()
		{
			for (int i = 0; i < this.autoRecorderGroups.Count; i++)
			{
				this.autoRecorderGroups[i].Tick();
			}
			this.historyEventsManager.HistoryEventsManagerTick();
		}

		// Token: 0x06009C68 RID: 40040 RVA: 0x002D5FBF File Offset: 0x002D41BF
		public List<HistoryAutoRecorderGroup> Groups()
		{
			return this.autoRecorderGroups;
		}

		// Token: 0x06009C69 RID: 40041 RVA: 0x002D5FC8 File Offset: 0x002D41C8
		public void ExposeData()
		{
			Scribe_Deep.Look<Archive>(ref this.archive, "archive", Array.Empty<object>());
			Scribe_Collections.Look<HistoryAutoRecorderGroup>(ref this.autoRecorderGroups, "autoRecorderGroups", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.lastPsylinkAvailable, "lastPsylinkAvailable", -999999, false);
			Scribe_Values.Look<int>(ref this.lastTickPlayerRaidedSomeone, "lastTickPlayerRaidedSomeone", -9999999, false);
			Scribe_Deep.Look<HistoryEventsManager>(ref this.historyEventsManager, "historyEventsManager", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.mechanitorEverDied, "mechanitorEverDied", false, false);
			Scribe_Values.Look<bool>(ref this.mechlinkEverAvailable, "mechlinkEverAvailable", false, false);
			Scribe_Values.Look<bool>(ref this.mechanoidDatacoreReadOrLost, "mechanoidDatacoreReadOrLost", false, false);
			Scribe_Values.Look<bool>(ref this.mechanoidDatacoreOpportunityAvailable, "mechanoidDatacoreOpportunityAvailable", false, false);
			Scribe_Values.Look<bool>(ref this.everThirdTrimesterPregnancy, "everThirdTrimesterPregnancy", false, false);
			Scribe_Values.Look<bool>(ref this.everCapturedUnrecruitablePawn, "everCapturedUnrecruitablePawn", false, false);
			Scribe_Values.Look<bool>(ref this.duplicateSicknessDiscovered, "duplicateSicknessDiscovered", false, false);
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.AddOrRemoveHistoryRecorderGroups();
				if (this.lastPsylinkAvailable == -999999)
				{
					this.lastPsylinkAvailable = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06009C6A RID: 40042 RVA: 0x002D60F0 File Offset: 0x002D42F0
		public void Notify_PsylinkAvailable()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x06009C6B RID: 40043 RVA: 0x002D6102 File Offset: 0x002D4302
		public void Notify_PlayerRaidedSomeone()
		{
			this.lastTickPlayerRaidedSomeone = Find.TickManager.TicksGame;
		}

		// Token: 0x06009C6C RID: 40044 RVA: 0x002D6114 File Offset: 0x002D4314
		public void Notify_MechanitorDied()
		{
			this.mechanitorEverDied = true;
		}

		// Token: 0x06009C6D RID: 40045 RVA: 0x002D611D File Offset: 0x002D431D
		public void Notify_MechlinkAvailable()
		{
			this.mechlinkEverAvailable = true;
		}

		// Token: 0x06009C6E RID: 40046 RVA: 0x002D6126 File Offset: 0x002D4326
		public void Notify_MechanoidDatacoreOppurtunityAvailable()
		{
			this.mechanoidDatacoreOpportunityAvailable = true;
		}

		// Token: 0x06009C6F RID: 40047 RVA: 0x002D612F File Offset: 0x002D432F
		public void Notify_MechanoidDatacoreReadOrLost()
		{
			this.mechanoidDatacoreReadOrLost = true;
		}

		// Token: 0x06009C70 RID: 40048 RVA: 0x002D6138 File Offset: 0x002D4338
		public void Notify_DuplicateSicknessDiscovered()
		{
			this.duplicateSicknessDiscovered = true;
		}

		// Token: 0x06009C71 RID: 40049 RVA: 0x002D60F0 File Offset: 0x002D42F0
		public void FinalizeInit()
		{
			this.lastPsylinkAvailable = Find.TickManager.TicksGame;
		}

		// Token: 0x06009C72 RID: 40050 RVA: 0x002D6144 File Offset: 0x002D4344
		private void AddOrRemoveHistoryRecorderGroups()
		{
			if (this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x == null) != 0)
			{
				Log.Warning("Some history auto recorder groups were null.");
			}
			using (IEnumerator<HistoryAutoRecorderGroupDef> enumerator = DefDatabase<HistoryAutoRecorderGroupDef>.AllDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HistoryAutoRecorderGroupDef def = enumerator.Current;
					if (!this.autoRecorderGroups.Any((HistoryAutoRecorderGroup x) => x.def == def))
					{
						HistoryAutoRecorderGroup historyAutoRecorderGroup = new HistoryAutoRecorderGroup();
						historyAutoRecorderGroup.def = def;
						historyAutoRecorderGroup.AddOrRemoveHistoryRecorders();
						this.autoRecorderGroups.Add(historyAutoRecorderGroup);
					}
				}
			}
			this.autoRecorderGroups.RemoveAll((HistoryAutoRecorderGroup x) => x.def == null);
		}

		// Token: 0x04006C73 RID: 27763
		public Archive archive = new Archive();

		// Token: 0x04006C74 RID: 27764
		private List<HistoryAutoRecorderGroup> autoRecorderGroups;

		// Token: 0x04006C75 RID: 27765
		public SimpleCurveDrawerStyle curveDrawerStyle;

		// Token: 0x04006C76 RID: 27766
		public int lastPsylinkAvailable = -999999;

		// Token: 0x04006C77 RID: 27767
		public int lastTickPlayerRaidedSomeone = -9999999;

		// Token: 0x04006C78 RID: 27768
		public bool mechanitorEverDied;

		// Token: 0x04006C79 RID: 27769
		public bool mechlinkEverAvailable;

		// Token: 0x04006C7A RID: 27770
		public bool mechanoidDatacoreOpportunityAvailable;

		// Token: 0x04006C7B RID: 27771
		public bool mechanoidDatacoreReadOrLost;

		// Token: 0x04006C7C RID: 27772
		public bool everThirdTrimesterPregnancy;

		// Token: 0x04006C7D RID: 27773
		public bool everCapturedUnrecruitablePawn;

		// Token: 0x04006C7E RID: 27774
		public bool duplicateSicknessDiscovered;

		// Token: 0x04006C7F RID: 27775
		public HistoryEventsManager historyEventsManager = new HistoryEventsManager();
	}
}
