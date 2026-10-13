using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD3 RID: 7123
	public class QuestPart_MoodBelow : QuestPartActivable
	{
		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x06009E99 RID: 40601 RVA: 0x002DDD58 File Offset: 0x002DBF58
		public override AlertReport AlertReport
		{
			get
			{
				if (!this.showAlert || this.minTicksBelowThreshold < 60)
				{
					return AlertReport.Inactive;
				}
				this.culpritsResult.Clear();
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.MoodBelowThreshold(this.pawns[i]))
					{
						this.culpritsResult.Add(this.pawns[i]);
					}
				}
				return AlertReport.CulpritsAre(this.culpritsResult);
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x06009E9A RID: 40602 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AlertCritical
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x06009E9B RID: 40603 RVA: 0x002DDDD4 File Offset: 0x002DBFD4
		public override string AlertLabel
		{
			get
			{
				return "QuestPartMoodBelowThreshold".Translate();
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x06009E9C RID: 40604 RVA: 0x002DDDE8 File Offset: 0x002DBFE8
		public override string AlertExplanation
		{
			get
			{
				return "QuestPartMoodBelowThresholdDesc".Translate(this.quest.name, GenLabel.ThingsLabel(this.pawns.Where(new Func<Pawn, bool>(this.MoodBelowThreshold)).Cast<Thing>(), "  - "));
			}
		}

		// Token: 0x06009E9D RID: 40605 RVA: 0x002DDE40 File Offset: 0x002DC040
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			while (this.moodBelowThresholdTicks.Count < this.pawns.Count)
			{
				this.moodBelowThresholdTicks.Add(0);
			}
			for (int i = 0; i < this.pawns.Count; i++)
			{
				if (this.MoodBelowThreshold(this.pawns[i]))
				{
					List<int> list = this.moodBelowThresholdTicks;
					int index = i;
					int num = list[index];
					list[index] = num + 1;
					if (this.moodBelowThresholdTicks[i] >= this.minTicksBelowThreshold)
					{
						base.Complete(this.pawns[i].Named("SUBJECT"));
						return;
					}
				}
				else
				{
					this.moodBelowThresholdTicks[i] = 0;
				}
			}
		}

		// Token: 0x06009E9E RID: 40606 RVA: 0x002DDEFC File Offset: 0x002DC0FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<float>(ref this.threshold, "threshold", 0f, false);
			Scribe_Values.Look<int>(ref this.minTicksBelowThreshold, "minTicksBelowThreshold", 0, false);
			Scribe_Values.Look<bool>(ref this.showAlert, "showAlert", true, false);
			Scribe_Collections.Look<int>(ref this.moodBelowThresholdTicks, "moodBelowThresholdTicks", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06009E9F RID: 40607 RVA: 0x002DDFA8 File Offset: 0x002DC1A8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				this.pawns.Add(randomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>());
				this.threshold = 0.5f;
				this.minTicksBelowThreshold = 2500;
			}
		}

		// Token: 0x06009EA0 RID: 40608 RVA: 0x002DDFF9 File Offset: 0x002DC1F9
		private bool MoodBelowThreshold(Pawn pawn)
		{
			return pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.CurLevelPercentage < this.threshold;
		}

		// Token: 0x06009EA1 RID: 40609 RVA: 0x002DE02A File Offset: 0x002DC22A
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04006DC5 RID: 28101
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006DC6 RID: 28102
		public float threshold;

		// Token: 0x04006DC7 RID: 28103
		public int minTicksBelowThreshold;

		// Token: 0x04006DC8 RID: 28104
		public bool showAlert = true;

		// Token: 0x04006DC9 RID: 28105
		private List<int> moodBelowThresholdTicks = new List<int>();

		// Token: 0x04006DCA RID: 28106
		private List<Pawn> culpritsResult = new List<Pawn>();
	}
}
