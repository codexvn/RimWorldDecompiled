using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001856 RID: 6230
	public class ThoughtWorker_NotBondedAnimalMaster : ThoughtWorker_BondedAnimalMaster
	{
		// Token: 0x06008F92 RID: 36754 RVA: 0x002A64F5 File Offset: 0x002A46F5
		protected override bool AnimalMasterCheck(Pawn p, Pawn animal)
		{
			return animal.playerSettings.RespectedMaster != p;
		}
	}
}
