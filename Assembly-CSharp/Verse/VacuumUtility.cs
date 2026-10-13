using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200043F RID: 1087
	public static class VacuumUtility
	{
		// Token: 0x06001C18 RID: 7192 RVA: 0x0008D750 File Offset: 0x0008B950
		public static void PawnVacuumTickInterval(Pawn pawn, int delta)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (!pawn.IsHashIntervalTick(60, delta) || !pawn.Spawned || !pawn.Map.Biome.inVacuum)
			{
				return;
			}
			if (!pawn.HarmedByVacuum)
			{
				return;
			}
			float vacuum = pawn.Position.GetVacuum(pawn.Map);
			if (vacuum < 0.5f)
			{
				return;
			}
			float num = 0.02f * vacuum * Mathf.Max(1f - pawn.GetStatValue(StatDefOf.VacuumResistance, true, -1), 0f);
			if (num > 0f)
			{
				HealthUtility.AdjustSeverity(pawn, HediffDefOf.VacuumExposure, num);
			}
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x0008D7EC File Offset: 0x0008B9EC
		public static bool CanBeVacuumBurnt(Pawn pawn)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!pawn.def.race.canBeVacuumBurnt)
			{
				return false;
			}
			if (pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					if (gene.Active && gene.def.immuneToVacuumBurns)
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x0008D880 File Offset: 0x0008BA80
		public static bool TryGetVacuumBurnablePart(Pawn pawn, out BodyPartRecord p)
		{
			if (!VacuumUtility.CanBeVacuumBurnt(pawn))
			{
				p = null;
				return false;
			}
			VacuumUtility.tmpParts.AddRange(pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Outside, null, null));
			for (int i = VacuumUtility.tmpParts.Count - 1; i >= 0; i--)
			{
				BodyPartRecord bodyPartRecord = VacuumUtility.tmpParts[i];
				bool flag = false;
				if (!bodyPartRecord.def.canBeVacuumBurnt || !bodyPartRecord.def.IsSkinCovered(bodyPartRecord, pawn.health.hediffSet))
				{
					VacuumUtility.tmpParts.RemoveAt(i);
				}
				else
				{
					if (pawn.apparel != null)
					{
						for (int j = 0; j < bodyPartRecord.groups.Count; j++)
						{
							if (pawn.apparel.BodyPartGroupIsCovered(bodyPartRecord.groups[j], (Apparel ap) => VacuumUtility.IsProtectiveApparel(ap.def)))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							Apparel apparel;
							flag = pawn.apparel.TryGetFirstApparelOnBodyPart(bodyPartRecord, out apparel, (Apparel ap) => VacuumUtility.IsProtectiveApparel(ap.def));
						}
					}
					if (flag)
					{
						VacuumUtility.tmpParts.RemoveAt(i);
					}
				}
			}
			if (VacuumUtility.tmpParts.Empty<BodyPartRecord>())
			{
				p = null;
				return false;
			}
			VacuumUtility.tmpParts.TryRandomElementByWeight((BodyPartRecord w) => w.coverage, out p);
			VacuumUtility.tmpParts.Clear();
			return p != null;
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x0008DA00 File Offset: 0x0008BC00
		public static bool IsProtectiveApparel(ThingDef thing)
		{
			return thing.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.VacuumResistance) >= 0.1f;
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x0008DA1C File Offset: 0x0008BC1C
		private static bool EverInVacuum(IntVec3 cell, Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (map == null)
			{
				return false;
			}
			if (!cell.InBounds(map) || !map.Biome.inVacuum)
			{
				return false;
			}
			Building edifice = cell.GetEdifice(map);
			return edifice == null || ((edifice.def.passability != Traversability.Impassable || edifice.def.Fillage != FillCategory.Full) && (edifice.def.building.canExchangeVacuum || edifice.def.building.alwaysExchangeVacuum || edifice.ExchangeVacuum || edifice.def.Fillage != FillCategory.Full));
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x0008DABB File Offset: 0x0008BCBB
		public static bool VacuumConcernTo(this Thing thing, Pawn pawn)
		{
			return pawn.ConcernedByVacuum && thing.PositionHeld.GetVacuum(pawn.MapHeld) >= 0.5f;
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x0008DAE2 File Offset: 0x0008BCE2
		public static bool VacuumConcernTo(this IntVec3 cell, Pawn pawn)
		{
			return pawn.ConcernedByVacuum && cell.GetVacuum(pawn.MapHeld) >= 0.5f;
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0008DB04 File Offset: 0x0008BD04
		public static float GetVacuum(this IntVec3 cell, Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 0f;
			}
			if (!VacuumUtility.EverInVacuum(cell, map))
			{
				return 0f;
			}
			Room room = cell.GetRoom(map);
			if (room == null)
			{
				return 1f;
			}
			return room.Vacuum;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x0008DB44 File Offset: 0x0008BD44
		public static bool IsRoomAirtight(Room room)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			VacuumUtility.tmpRoomOpenToOutside.Clear();
			if (VacuumUtility.<IsRoomAirtight>g__IsRoomDirectlyOpenToOutside|13_0(room))
			{
				return false;
			}
			foreach (IntVec3 c in room.BorderCells)
			{
				Building edifice = c.GetEdifice(room.Map);
				if (edifice != null && (edifice == null || !edifice.IsAirtight || edifice.def.building.alwaysExchangeVacuum))
				{
					CellRect cellRect = edifice.OccupiedRect();
					foreach (IntVec3 a in cellRect)
					{
						for (int i = 0; i < 4; i++)
						{
							IntVec3 intVec = a + GenAdj.CardinalDirections[i];
							if (!cellRect.Contains(intVec))
							{
								Room room2 = intVec.GetRoom(room.Map);
								if (room2 != null && room2 != room && VacuumUtility.<IsRoomAirtight>g__IsRoomDirectlyOpenToOutside|13_0(room2))
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0008DC90 File Offset: 0x0008BE90
		[CompilerGenerated]
		internal static bool <IsRoomAirtight>g__IsRoomDirectlyOpenToOutside|13_0(Room room)
		{
			bool result;
			if (VacuumUtility.tmpRoomOpenToOutside.TryGetValue(room, out result))
			{
				return result;
			}
			if (room.TouchesMapEdge || room.OpenRoofCount > 0)
			{
				VacuumUtility.tmpRoomOpenToOutside[room] = true;
				return true;
			}
			foreach (IntVec3 c in room.Cells)
			{
				TerrainDef terrainDef = room.Map.terrainGrid.FoundationAt(c);
				if (terrainDef == null || !terrainDef.IsSubstructure)
				{
					VacuumUtility.tmpRoomOpenToOutside[room] = true;
					return true;
				}
			}
			VacuumUtility.tmpRoomOpenToOutside[room] = false;
			return false;
		}

		// Token: 0x04001824 RID: 6180
		private const float SeverityPerSecond = 0.02f;

		// Token: 0x04001825 RID: 6181
		public const float MinVacuumForDamage = 0.5f;

		// Token: 0x04001826 RID: 6182
		public const float ApparelMinResistAvoidDamage = 0.1f;

		// Token: 0x04001827 RID: 6183
		private static readonly List<BodyPartRecord> tmpParts = new List<BodyPartRecord>();

		// Token: 0x04001828 RID: 6184
		private static Dictionary<Room, bool> tmpRoomOpenToOutside = new Dictionary<Room, bool>();
	}
}
