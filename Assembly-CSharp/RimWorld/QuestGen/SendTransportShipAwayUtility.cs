using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A27 RID: 14887
	public static class SendTransportShipAwayUtility
	{
		// Token: 0x0601462D RID: 83501 RVA: 0x006175AC File Offset: 0x006157AC
		public static void SendTransportShipAway(TransportShip transportShip, bool unloadContents, TransportShipDropMode unsatisfiedDropMode = TransportShipDropMode.NonRequired)
		{
			if (transportShip == null || transportShip.Disposed)
			{
				return;
			}
			if (!transportShip.started)
			{
				transportShip.Dispose();
				return;
			}
			if (!transportShip.ShipExistsAndIsSpawned || transportShip.LeavingSoonAutomatically)
			{
				return;
			}
			ShipJob_FlyAway shipJob_FlyAway = (ShipJob_FlyAway)ShipJobMaker.MakeShipJob(ShipJobDefOf.FlyAway);
			shipJob_FlyAway.dropMode = unsatisfiedDropMode;
			if (unloadContents)
			{
				transportShip.ForceJob(ShipJobDefOf.Unload);
				transportShip.AddJob(shipJob_FlyAway);
				return;
			}
			transportShip.ForceJob(shipJob_FlyAway);
		}
	}
}
