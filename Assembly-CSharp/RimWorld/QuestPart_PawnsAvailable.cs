using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BD7 RID: 7127
	public class QuestPart_PawnsAvailable : QuestPartActivable
	{
		// Token: 0x06009EB5 RID: 40629 RVA: 0x002DE2BC File Offset: 0x002DC4BC
		public override void QuestPartTick()
		{
			if (this.mapParent == null || this.mapParent.Destroyed)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
			}
			if (this.requiredCount > 0 && Find.TickManager.TicksAbs % 500 == 0)
			{
				int num = 0;
				IReadOnlyList<Pawn> allPawnsSpawned = this.mapParent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].def == this.race && allPawnsSpawned[i].Faction == null)
					{
						num++;
					}
				}
				if (num < this.requiredCount)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalPawnsNotAvailable, false));
				}
			}
		}

		// Token: 0x06009EB6 RID: 40630 RVA: 0x002DE381 File Offset: 0x002DC581
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalDecrement)
			{
				this.requiredCount--;
			}
		}

		// Token: 0x06009EB7 RID: 40631 RVA: 0x002DE3AC File Offset: 0x002DC5AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.requiredCount, "requiredCount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalDecrement, "inSignalChangeCount", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnsNotAvailable, "outSignalPawnsNotAvailable", null, false);
		}

		// Token: 0x04006DD3 RID: 28115
		public ThingDef race;

		// Token: 0x04006DD4 RID: 28116
		public int requiredCount;

		// Token: 0x04006DD5 RID: 28117
		public MapParent mapParent;

		// Token: 0x04006DD6 RID: 28118
		public string inSignalDecrement;

		// Token: 0x04006DD7 RID: 28119
		public string outSignalPawnsNotAvailable;

		// Token: 0x04006DD8 RID: 28120
		private const int CheckInterval = 500;
	}
}
