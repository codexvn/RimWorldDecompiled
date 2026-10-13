using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E12 RID: 11794
	public static class ShipLandingBeaconUtility
	{
		// Token: 0x060109A3 RID: 68003 RVA: 0x004C8F98 File Offset: 0x004C7198
		public static List<ShipLandingArea> GetLandingZones(Map map)
		{
			ShipLandingBeaconUtility.tmpShipLandingAreas.Clear();
			if (!ModsConfig.RoyaltyActive)
			{
				return ShipLandingBeaconUtility.tmpShipLandingAreas;
			}
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null && thing.Faction == Faction.OfPlayer)
				{
					foreach (ShipLandingArea shipLandingArea in compShipLandingBeacon.LandingAreas)
					{
						if (shipLandingArea.Active && !ShipLandingBeaconUtility.tmpShipLandingAreas.Contains(shipLandingArea))
						{
							shipLandingArea.RecalculateBlockingThing();
							ShipLandingBeaconUtility.tmpShipLandingAreas.Add(shipLandingArea);
						}
					}
				}
			}
			return ShipLandingBeaconUtility.tmpShipLandingAreas;
		}

		// Token: 0x060109A4 RID: 68004 RVA: 0x004C908C File Offset: 0x004C728C
		public static void DrawLinesToNearbyBeacons(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map, Thing thing = null)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing thing2 in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				if ((thing == null || thing != thing2) && thing2.Faction == Faction.OfPlayer)
				{
					CompShipLandingBeacon compShipLandingBeacon = thing2.TryGetComp<CompShipLandingBeacon>();
					if (compShipLandingBeacon != null && ShipLandingBeaconUtility.CanLinkTo(myPos, compShipLandingBeacon) && !GenThing.CloserThingBetween(myDef, myPos, thing2.Position, map, null))
					{
						GenDraw.DrawLineBetween(a, thing2.TrueCenter(), SimpleColor.White, 0.2f);
					}
				}
			}
			float minEdgeDistance = myDef.GetCompProperties<CompProperties_ShipLandingBeacon>().edgeLengthRange.min - 1f;
			float maxEdgeDistance = myDef.GetCompProperties<CompProperties_ShipLandingBeacon>().edgeLengthRange.max - 1f;
			foreach (Thing thing3 in map.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint))
			{
				if ((thing == null || thing != thing3) && thing3.def.entityDefToBuild == myDef && (myPos.x == thing3.Position.x || myPos.z == thing3.Position.z) && !ShipLandingBeaconUtility.AlignedDistanceTooShort(myPos, thing3.Position, minEdgeDistance) && !ShipLandingBeaconUtility.AlignedDistanceTooLong(myPos, thing3.Position, maxEdgeDistance) && !GenThing.CloserThingBetween(myDef, myPos, thing3.Position, map, null))
				{
					GenDraw.DrawLineBetween(a, thing3.TrueCenter(), SimpleColor.White, 0.2f);
				}
			}
		}

		// Token: 0x060109A5 RID: 68005 RVA: 0x004C9248 File Offset: 0x004C7448
		public static bool AlignedDistanceTooShort(IntVec3 position, IntVec3 otherPos, float minEdgeDistance)
		{
			if (position.x == otherPos.x)
			{
				return (float)Mathf.Abs(position.z - otherPos.z) < minEdgeDistance;
			}
			return position.z == otherPos.z && (float)Mathf.Abs(position.x - otherPos.x) < minEdgeDistance;
		}

		// Token: 0x060109A6 RID: 68006 RVA: 0x004C92A0 File Offset: 0x004C74A0
		private static bool AlignedDistanceTooLong(IntVec3 position, IntVec3 otherPos, float maxEdgeDistance)
		{
			if (position.x == otherPos.x)
			{
				return (float)Mathf.Abs(position.z - otherPos.z) >= maxEdgeDistance;
			}
			return position.z == otherPos.z && (float)Mathf.Abs(position.x - otherPos.x) >= maxEdgeDistance;
		}

		// Token: 0x060109A7 RID: 68007 RVA: 0x004C9300 File Offset: 0x004C7500
		public static bool CanLinkTo(IntVec3 position, CompShipLandingBeacon other)
		{
			if (position.x == other.parent.Position.x)
			{
				return other.parent.def.displayNumbersBetweenSameDefDistRange.Includes((float)(Mathf.Abs(position.z - other.parent.Position.z) + 1));
			}
			return position.z == other.parent.Position.z && other.parent.def.displayNumbersBetweenSameDefDistRange.Includes((float)(Mathf.Abs(position.x - other.parent.Position.x) + 1));
		}

		// Token: 0x0400ACFC RID: 44284
		public static List<ShipLandingArea> tmpShipLandingAreas = new List<ShipLandingArea>();
	}
}
