using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD9 RID: 7129
	public class QuestPart_ShuttleDelay : QuestPart_Delay
	{
		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x06009EBC RID: 40636 RVA: 0x002DE4AD File Offset: 0x002DC6AD
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.lodgers.Count; i = num + 1)
				{
					yield return this.lodgers[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x06009EBD RID: 40637 RVA: 0x002DE4BD File Offset: 0x002DC6BD
		public override AlertReport AlertReport
		{
			get
			{
				if (!this.alert || base.State != QuestPartState.Enabled)
				{
					return false;
				}
				return AlertReport.CulpritsAre(this.lodgers);
			}
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x06009EBE RID: 40638 RVA: 0x002DE4E2 File Offset: 0x002DC6E2
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x06009EBF RID: 40639 RVA: 0x002DE4F1 File Offset: 0x002DC6F1
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleArriveDelay".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose(true, true));
			}
		}

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x06009EC0 RID: 40640 RVA: 0x002DE514 File Offset: 0x002DC714
		public override string AlertExplanation
		{
			get
			{
				if (this.quest.hidden)
				{
					return "QuestPartShuttleArriveDelayDescHidden".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor));
				}
				return "QuestPartShuttleArriveDelayDesc".Translate(this.quest.name, base.TicksLeft.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor), (from p in this.lodgers
				select p.LabelShort).ToLineList("- ", false));
			}
		}

		// Token: 0x06009EC1 RID: 40641 RVA: 0x002DE5D0 File Offset: 0x002DC7D0
		public override string ExtraInspectString(ISelectable target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null && this.lodgers.Contains(pawn))
			{
				return "ShuttleDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x06009EC2 RID: 40642 RVA: 0x002DE61C File Offset: 0x002DC81C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "lodgers", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.alert, "alert", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06009EC3 RID: 40643 RVA: 0x002DE68A File Offset: 0x002DC88A
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.lodgers.AddRange(Find.RandomPlayerHomeMap.mapPawns.FreeColonists);
			}
		}

		// Token: 0x06009EC4 RID: 40644 RVA: 0x002DE6B3 File Offset: 0x002DC8B3
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.lodgers.Replace(replace, with);
		}

		// Token: 0x04006DDC RID: 28124
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x04006DDD RID: 28125
		public bool alert;
	}
}
