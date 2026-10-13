using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200214D RID: 8525
	public static class PowerConnectionMaker
	{
		// Token: 0x0600BA43 RID: 47683 RVA: 0x0035B928 File Offset: 0x00359B28
		public static void ConnectAllConnectorsToTransmitter(CompPower newTransmitter)
		{
			foreach (CompPower compPower in PowerConnectionMaker.PotentialConnectorsForTransmitter(newTransmitter))
			{
				if (compPower.connectParent == null)
				{
					compPower.ConnectToTransmitter(newTransmitter, false);
				}
			}
		}

		// Token: 0x0600BA44 RID: 47684 RVA: 0x0035B980 File Offset: 0x00359B80
		public static void DisconnectAllFromTransmitterAndSetWantConnect(CompPower deadPc, Map map)
		{
			if (deadPc.connectChildren == null)
			{
				return;
			}
			for (int i = 0; i < deadPc.connectChildren.Count; i++)
			{
				CompPower compPower = deadPc.connectChildren[i];
				compPower.connectParent = null;
				CompPowerTrader compPowerTrader = compPower as CompPowerTrader;
				if (compPowerTrader != null)
				{
					compPowerTrader.PowerOn = false;
				}
				map.powerNetManager.Notify_ConnectorWantsConnect(compPower);
			}
		}

		// Token: 0x0600BA45 RID: 47685 RVA: 0x0035B9E0 File Offset: 0x00359BE0
		public static void TryConnectToAnyPowerNet(CompPower pc, List<PowerNet> disallowedNets = null)
		{
			if (pc.connectParent != null)
			{
				return;
			}
			if (!pc.parent.Spawned)
			{
				return;
			}
			CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(pc.parent.def.building.isAttachment ? GenConstruct.GetWallAttachedTo(pc.parent).Position : pc.parent.Position, pc.parent.Map, disallowedNets);
			if (compPower != null)
			{
				pc.ConnectToTransmitter(compPower, false);
				return;
			}
			pc.connectParent = null;
		}

		// Token: 0x0600BA46 RID: 47686 RVA: 0x0035BA60 File Offset: 0x00359C60
		public static void DisconnectFromPowerNet(CompPower pc)
		{
			if (pc.connectParent == null)
			{
				return;
			}
			if (pc.PowerNet != null)
			{
				pc.PowerNet.DeregisterConnector(pc);
			}
			if (pc.connectParent.connectChildren != null)
			{
				pc.connectParent.connectChildren.Remove(pc);
				if (pc.connectParent.connectChildren.Count == 0)
				{
					pc.connectParent.connectChildren = null;
				}
			}
			pc.connectParent = null;
		}

		// Token: 0x0600BA47 RID: 47687 RVA: 0x0035BACE File Offset: 0x00359CCE
		private static IEnumerable<CompPower> PotentialConnectorsForTransmitter(CompPower b)
		{
			if (!b.parent.Spawned)
			{
				Log.Warning("Can't check potential connectors for " + ((b != null) ? b.ToString() : null) + " because it's unspawned.");
				yield break;
			}
			CellRect rect = b.parent.OccupiedRect().ExpandedBy(6).ClipInsideMap(b.parent.Map);
			int num;
			for (int z = rect.minZ; z <= rect.maxZ; z = num + 1)
			{
				for (int x = rect.minX; x <= rect.maxX; x = num + 1)
				{
					IntVec3 c = new IntVec3(x, 0, z);
					List<Thing> thingList = b.parent.Map.thingGrid.ThingsListAt(c);
					for (int i = 0; i < thingList.Count; i = num + 1)
					{
						if (thingList[i].def.ConnectToPower)
						{
							yield return ((Building)thingList[i]).PowerComp;
						}
						num = i;
					}
					thingList = null;
					num = x;
				}
				num = z;
			}
			yield break;
		}

		// Token: 0x0600BA48 RID: 47688 RVA: 0x0035BAE0 File Offset: 0x00359CE0
		public static CompPower BestTransmitterForConnector(IntVec3 connectorPos, Map map, List<PowerNet> disallowedNets = null)
		{
			CellRect cellRect = CellRect.SingleCell(connectorPos).ExpandedBy(6).ClipInsideMap(map);
			cellRect.ClipInsideMap(map);
			float num = 999999f;
			CompPower result = null;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					Building transmitter = new IntVec3(j, 0, i).GetTransmitter(map);
					if (transmitter != null && !transmitter.Destroyed)
					{
						CompPower powerComp = transmitter.PowerComp;
						if (powerComp != null && powerComp.TransmitsPowerNow && (transmitter.def.building == null || transmitter.def.building.allowWireConnection) && (disallowedNets == null || !disallowedNets.Contains(powerComp.transNet)))
						{
							float num2 = (float)(transmitter.Position - connectorPos).LengthHorizontalSquared;
							if (num2 < num)
							{
								num = num2;
								result = powerComp;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04008061 RID: 32865
		private const int ConnectMaxDist = 6;
	}
}
