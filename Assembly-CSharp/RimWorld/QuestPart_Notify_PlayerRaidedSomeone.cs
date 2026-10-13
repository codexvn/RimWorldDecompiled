using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C71 RID: 7281
	public class QuestPart_Notify_PlayerRaidedSomeone : QuestPart
	{
		// Token: 0x0600A242 RID: 41538 RVA: 0x002EA538 File Offset: 0x002E8738
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				IEnumerable<Pawn> enumerable = null;
				if (this.getRaidersFromMap != null)
				{
					enumerable = this.getRaidersFromMap.mapPawns.FreeColonistsSpawned;
				}
				if (this.getRaidersFromMapParent != null)
				{
					Map map = this.getRaidersFromMapParent.Map;
					if (map == null)
					{
						Log.Error("Cannot resolve map for QuestPart_NotifyPlayerRaidedSomeone.getRaidersFromWorldObjectMap!");
						return;
					}
					enumerable = map.mapPawns.FreeColonistsSpawned;
				}
				if (enumerable == null)
				{
					Log.Error("No raiders could be determined to notify ideoligons!");
					return;
				}
				IdeoUtility.Notify_PlayerRaidedSomeone(enumerable);
			}
		}

		// Token: 0x0600A243 RID: 41539 RVA: 0x002EA5B5 File Offset: 0x002E87B5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Map>(ref this.getRaidersFromMap, "getRaidersFromMap", false);
			Scribe_References.Look<MapParent>(ref this.getRaidersFromMapParent, "getRaidersFromMapParent", false);
		}

		// Token: 0x0400701A RID: 28698
		public string inSignal;

		// Token: 0x0400701B RID: 28699
		public Map getRaidersFromMap;

		// Token: 0x0400701C RID: 28700
		public MapParent getRaidersFromMapParent;
	}
}
