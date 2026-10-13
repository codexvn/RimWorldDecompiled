using System;
using RimWorld.Utility;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BB6 RID: 15286
	public class QuestPart_SetMonolithGleaming : QuestPart
	{
		// Token: 0x06014C27 RID: 85031 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_SetMonolithGleaming()
		{
		}

		// Token: 0x06014C28 RID: 85032 RVA: 0x0063845A File Offset: 0x0063665A
		public QuestPart_SetMonolithGleaming(Building_VoidMonolith monolith, string inSignal)
		{
			this.monolith = monolith;
			this.inSignal = inSignal;
		}

		// Token: 0x06014C29 RID: 85033 RVA: 0x00638470 File Offset: 0x00636670
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			Find.Anomaly.SetLevel(MonolithLevelDefOf.Gleaming, true);
			CellRect cellRect = this.monolith.OccupiedRect();
			CellRect value = new CellRect(cellRect.minX, cellRect.minZ - 3, 3, 3);
			VoidAwakeningUtility.SpawnVoidMetalAround(this.monolith, 300, QuestPart_SetMonolithGleaming.NumMassesRange.RandomInRange, true, new CellRect?(value));
			VoidAwakeningUtility.SpawnMetalhorrorDefenders(this.monolith);
		}

		// Token: 0x06014C2A RID: 85034 RVA: 0x006384F3 File Offset: 0x006366F3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x0400E71E RID: 59166
		private const int VoidMetalPatchSize = 300;

		// Token: 0x0400E71F RID: 59167
		private static readonly IntRange NumMassesRange = new IntRange(3, 4);

		// Token: 0x0400E720 RID: 59168
		private Building_VoidMonolith monolith;

		// Token: 0x0400E721 RID: 59169
		private string inSignal;
	}
}
