using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A3B RID: 10811
	public class Building_OrbitalTradeBeacon : Building
	{
		// Token: 0x1700255A RID: 9562
		// (get) Token: 0x0600F001 RID: 61441 RVA: 0x0045A413 File Offset: 0x00458613
		public IEnumerable<IntVec3> TradeableCells
		{
			get
			{
				return Building_OrbitalTradeBeacon.TradeableCellsAround(base.Position, base.Map);
			}
		}

		// Token: 0x0600F002 RID: 61442 RVA: 0x0045A426 File Offset: 0x00458626
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>() != null)
			{
				yield return new Command_Action
				{
					action = new Action(this.MakeMatchingStockpile),
					hotKey = KeyBindingDefOf.Misc1,
					defaultDesc = "CommandMakeBeaconStockpileDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Stockpile", true),
					defaultLabel = "CommandMakeBeaconStockpileLabel".Translate()
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F003 RID: 61443 RVA: 0x0045A438 File Offset: 0x00458638
		private void MakeMatchingStockpile()
		{
			Designator_ZoneAddStockpile_Resources des = DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Resources>();
			des.DesignateMultiCell(from c in this.TradeableCells
			where des.CanDesignateCell(c).Accepted
			select c);
		}

		// Token: 0x0600F004 RID: 61444 RVA: 0x0045A478 File Offset: 0x00458678
		public static List<IntVec3> TradeableCellsAround(IntVec3 pos, Map map)
		{
			Building_OrbitalTradeBeacon.tradeableCells.Clear();
			if (!pos.InBounds(map))
			{
				return Building_OrbitalTradeBeacon.tradeableCells;
			}
			Region region = pos.GetRegion(map, RegionType.Set_Passable);
			if (region == null)
			{
				return Building_OrbitalTradeBeacon.tradeableCells;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null, delegate(Region r)
			{
				foreach (IntVec3 item in r.Cells)
				{
					if (item.InHorDistOf(pos, 7.9f))
					{
						Building_OrbitalTradeBeacon.tradeableCells.Add(item);
					}
				}
				return false;
			}, 16, RegionType.Set_Passable);
			return Building_OrbitalTradeBeacon.tradeableCells;
		}

		// Token: 0x0600F005 RID: 61445 RVA: 0x0045A502 File Offset: 0x00458702
		public static IEnumerable<Building_OrbitalTradeBeacon> AllPowered(Map map)
		{
			foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in map.listerBuildings.AllBuildingsColonistOfClass<Building_OrbitalTradeBeacon>())
			{
				CompPowerTrader comp = building_OrbitalTradeBeacon.GetComp<CompPowerTrader>();
				if (comp == null || comp.PowerOn)
				{
					yield return building_OrbitalTradeBeacon;
				}
			}
			IEnumerator<Building_OrbitalTradeBeacon> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04009E7E RID: 40574
		private const float TradeRadius = 7.9f;

		// Token: 0x04009E7F RID: 40575
		private static List<IntVec3> tradeableCells = new List<IntVec3>();
	}
}
