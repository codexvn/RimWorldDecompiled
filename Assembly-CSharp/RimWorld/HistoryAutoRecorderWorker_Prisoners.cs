using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6D RID: 7021
	public class HistoryAutoRecorderWorker_Prisoners : HistoryAutoRecorderWorker
	{
		// Token: 0x06009CB4 RID: 40116 RVA: 0x002D7377 File Offset: 0x002D5577
		public override float PullRecord()
		{
			return (float)PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_PrisonersOfColony.Count<Pawn>();
		}
	}
}
