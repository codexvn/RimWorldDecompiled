using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6C RID: 7020
	public class HistoryAutoRecorderWorker_FreeColonists : HistoryAutoRecorderWorker
	{
		// Token: 0x06009CB2 RID: 40114 RVA: 0x002D736A File Offset: 0x002D556A
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoLodgers.Count<Pawn>();
		}
	}
}
