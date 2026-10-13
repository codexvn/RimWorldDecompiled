using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BB9 RID: 15289
	public class QuestPart_Alert_StructuresArriving : QuestPart_Alert
	{
		// Token: 0x1700337C RID: 13180
		// (get) Token: 0x06014C35 RID: 85045 RVA: 0x0063865E File Offset: 0x0063685E
		private int TicksRemaining
		{
			get
			{
				return this.arriveTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700337D RID: 13181
		// (get) Token: 0x06014C36 RID: 85046 RVA: 0x00638671 File Offset: 0x00636871
		public override string AlertLabel
		{
			get
			{
				return base.AlertLabel + ": " + this.TicksRemaining.ToStringTicksToPeriod(false, false, true, true, false);
			}
		}

		// Token: 0x06014C37 RID: 85047 RVA: 0x0062423D File Offset: 0x0062243D
		public QuestPart_Alert_StructuresArriving()
		{
		}

		// Token: 0x06014C38 RID: 85048 RVA: 0x00638694 File Offset: 0x00636894
		public QuestPart_Alert_StructuresArriving(string label, string explanation, string inSignalEnable, string inSignalDisable, int delay, LookTargets lookTargets = null)
		{
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				this.label = x;
			}, QuestGenUtility.MergeRules(null, label, "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				this.explanation = x;
			}, QuestGenUtility.MergeRules(null, explanation, "root"));
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
			this.delay = delay;
			this.lookTargets = lookTargets;
		}

		// Token: 0x06014C39 RID: 85049 RVA: 0x0063870A File Offset: 0x0063690A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.arriveTick, "arriveTick", 0, false);
			Scribe_Values.Look<int>(ref this.delay, "delay", 0, false);
		}

		// Token: 0x06014C3A RID: 85050 RVA: 0x00638736 File Offset: 0x00636936
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalEnable)
			{
				this.arriveTick = Find.TickManager.TicksGame + this.delay;
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x0400E725 RID: 59173
		private int arriveTick;

		// Token: 0x0400E726 RID: 59174
		private int delay;
	}
}
