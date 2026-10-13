using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E84 RID: 7812
	public class StatsRecord : IExposable
	{
		// Token: 0x0600AD7B RID: 44411 RVA: 0x003185B0 File Offset: 0x003167B0
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.numRaidsEnemy, "numRaidsEnemy", 0, false);
			Scribe_Values.Look<int>(ref this.numThreatBigs, "numThreatsQueued", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsKilled, "colonistsKilled", 0, false);
			Scribe_Values.Look<int>(ref this.colonistsLaunched, "colonistsLaunched", 0, false);
			Scribe_Values.Look<int>(ref this.greatestPopulation, "greatestPopulation", 3, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.UpdateGreatestPopulation();
			}
		}

		// Token: 0x0600AD7C RID: 44412 RVA: 0x00318625 File Offset: 0x00316825
		public void Notify_ColonistKilled()
		{
			this.colonistsKilled++;
		}

		// Token: 0x0600AD7D RID: 44413 RVA: 0x00318638 File Offset: 0x00316838
		public void UpdateGreatestPopulation()
		{
			int a = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists.Count<Pawn>();
			this.greatestPopulation = Mathf.Max(a, this.greatestPopulation);
		}

		// Token: 0x0400775D RID: 30557
		public int numRaidsEnemy;

		// Token: 0x0400775E RID: 30558
		public int numThreatBigs;

		// Token: 0x0400775F RID: 30559
		public int colonistsKilled;

		// Token: 0x04007760 RID: 30560
		public int colonistsLaunched;

		// Token: 0x04007761 RID: 30561
		public int greatestPopulation;
	}
}
