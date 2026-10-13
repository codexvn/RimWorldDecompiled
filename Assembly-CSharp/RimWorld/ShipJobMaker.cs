using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001777 RID: 6007
	public static class ShipJobMaker
	{
		// Token: 0x06008CFF RID: 36095 RVA: 0x0029EFA9 File Offset: 0x0029D1A9
		public static ShipJob MakeShipJob(ShipJobDef def)
		{
			ShipJob shipJob = (ShipJob)Activator.CreateInstance(def.jobClass);
			shipJob.def = def;
			shipJob.loadID = Find.UniqueIDsManager.GetNextShipJobID();
			return shipJob;
		}
	}
}
