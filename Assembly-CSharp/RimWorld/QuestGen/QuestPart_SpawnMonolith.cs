using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B71 RID: 15217
	public class QuestPart_SpawnMonolith : QuestPart
	{
		// Token: 0x06014ADF RID: 84703 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_SpawnMonolith()
		{
		}

		// Token: 0x06014AE0 RID: 84704 RVA: 0x0062EECA File Offset: 0x0062D0CA
		public QuestPart_SpawnMonolith(string inSignal)
		{
			this.inSignal = inSignal;
		}

		// Token: 0x06014AE1 RID: 84705 RVA: 0x0062EEDC File Offset: 0x0062D0DC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.inSignal)
			{
				return;
			}
			Map randomSurfacePlayerHomeMap = Find.RandomSurfacePlayerHomeMap;
			if (randomSurfacePlayerHomeMap == null)
			{
				Log.Error("Tried to spawn monolith but no map found");
				return;
			}
			IntVec3 loc;
			Find.Anomaly.TryGetCellForMonolithSpawn(randomSurfacePlayerHomeMap, out loc, false);
			GenSpawn.Spawn(ThingDefOf.MonolithIncoming, loc, randomSurfacePlayerHomeMap, WipeMode.Vanish);
		}

		// Token: 0x06014AE2 RID: 84706 RVA: 0x0062EF2E File Offset: 0x0062D12E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x0400E5CE RID: 58830
		private string inSignal;
	}
}
