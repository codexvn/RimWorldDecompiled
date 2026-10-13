using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ADE RID: 10974
	public static class FireUtility
	{
		// Token: 0x0600F498 RID: 62616 RVA: 0x004718E2 File Offset: 0x0046FAE2
		public static bool CanEverAttachFire(this Thing t)
		{
			return !t.Destroyed && t.FlammableNow && t.def.category == ThingCategory.Pawn && t.TryGetComp<CompAttachBase>() != null;
		}

		// Token: 0x0600F499 RID: 62617 RVA: 0x00471914 File Offset: 0x0046FB14
		public static float ChanceToStartFireIn(IntVec3 c, Map map, SimpleCurve flammabilityChanceCurve = null)
		{
			List<Thing> thingList = c.GetThingList(map);
			float num = c.TerrainFlammability(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing is Fire)
				{
					return 0f;
				}
				if (thing.def.category != ThingCategory.Pawn && thingList[i].FlammableNow)
				{
					num = Mathf.Max(num, thing.GetStatValue(StatDefOf.Flammability, true, -1));
				}
			}
			if (flammabilityChanceCurve != null)
			{
				num = flammabilityChanceCurve.Evaluate(num);
			}
			if (num > 0f)
			{
				Building edifice = c.GetEdifice(map);
				if (edifice != null && edifice.def.passability == Traversability.Impassable && edifice.OccupiedRect().ContractedBy(1).Contains(c))
				{
					return 0f;
				}
				List<Thing> thingList2 = c.GetThingList(map);
				for (int j = 0; j < thingList2.Count; j++)
				{
					if (thingList2[j].def.category == ThingCategory.Filth && !thingList2[j].def.filth.allowsFire)
					{
						return 0f;
					}
				}
			}
			return num;
		}

		// Token: 0x0600F49A RID: 62618 RVA: 0x00471A34 File Offset: 0x0046FC34
		public static bool TryStartFireIn(IntVec3 c, Map map, float fireSize, Thing instigator, SimpleCurve flammabilityChanceCurve = null)
		{
			if (FireUtility.ChanceToStartFireIn(c, map, flammabilityChanceCurve) <= 0f)
			{
				return false;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
			fire.fireSize = fireSize;
			fire.instigator = instigator;
			GenSpawn.Spawn(fire, c, map, Rot4.North, WipeMode.Vanish, false, false);
			return true;
		}

		// Token: 0x0600F49B RID: 62619 RVA: 0x00471A82 File Offset: 0x0046FC82
		public static float ChanceToAttachFireFromEvent(Thing t)
		{
			return FireUtility.ChanceToAttachFireCumulative(t, 60f);
		}

		// Token: 0x0600F49C RID: 62620 RVA: 0x00471A90 File Offset: 0x0046FC90
		public static float ChanceToAttachFireCumulative(Thing t, float freqInTicks)
		{
			if (!t.CanEverAttachFire())
			{
				return 0f;
			}
			if (t.HasAttachment(ThingDefOf.Fire))
			{
				return 0f;
			}
			float num = FireUtility.ChanceToCatchFirePerSecondForPawnFromFlammability.Evaluate(t.GetStatValue(StatDefOf.Flammability, true, -1));
			return 1f - Mathf.Pow(1f - num, freqInTicks / 60f);
		}

		// Token: 0x0600F49D RID: 62621 RVA: 0x00471AF0 File Offset: 0x0046FCF0
		public static void TryAttachFire(this Thing t, float fireSize, Thing instigator)
		{
			if (!t.CanEverAttachFire())
			{
				return;
			}
			if (t.HasAttachment(ThingDefOf.Fire))
			{
				return;
			}
			Fire fire = (Fire)ThingMaker.MakeThing(ThingDefOf.Fire, null);
			fire.fireSize = fireSize;
			fire.instigator = instigator;
			fire.AttachTo(t);
			GenSpawn.Spawn(fire, t.Position, t.Map, Rot4.North, WipeMode.Vanish, false, false);
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.jobs.StopAll(false, true);
				pawn.records.Increment(RecordDefOf.TimesOnFire);
			}
		}

		// Token: 0x0600F49E RID: 62622 RVA: 0x00471B7A File Offset: 0x0046FD7A
		public static bool IsBurning(this TargetInfo t)
		{
			if (t.HasThing)
			{
				return t.Thing.IsBurning();
			}
			return t.Cell.ContainsStaticFire(t.Map);
		}

		// Token: 0x0600F49F RID: 62623 RVA: 0x00471BA8 File Offset: 0x0046FDA8
		public static bool IsBurning(this Thing t)
		{
			if (t.Destroyed || !t.Spawned)
			{
				return false;
			}
			if (!(t.def.size == IntVec2.One))
			{
				using (CellRect.Enumerator enumerator = t.OccupiedRect().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ContainsStaticFire(t.Map))
						{
							return true;
						}
					}
				}
				return false;
			}
			if (t is Pawn)
			{
				return t.HasAttachment(ThingDefOf.Fire);
			}
			return t.Position.ContainsStaticFire(t.Map);
		}

		// Token: 0x0600F4A0 RID: 62624 RVA: 0x00471C5C File Offset: 0x0046FE5C
		public static bool ContainsStaticFire(this IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				if (fire != null && fire.parent == null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600F4A1 RID: 62625 RVA: 0x00471CA4 File Offset: 0x0046FEA4
		public static int NumFiresAt(IntVec3 c, Map map)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsBurning())
				{
					num++;
				}
				if (list[i].def.CompDefForAssignableFrom<CompFireOverlayBase>() != null)
				{
					CompGlower compGlower = list[i].TryGetComp<CompGlower>();
					if (compGlower != null && compGlower.Glows)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600F4A2 RID: 62626 RVA: 0x00471D18 File Offset: 0x0046FF18
		public static bool ContainsTrap(this IntVec3 c, Map map)
		{
			Building edifice = c.GetEdifice(map);
			return edifice != null && edifice is Building_Trap;
		}

		// Token: 0x0600F4A3 RID: 62627 RVA: 0x00471D3B File Offset: 0x0046FF3B
		public static bool Flammable(this TerrainDef terrain)
		{
			return terrain.GetStatValueAbstract(StatDefOf.Flammability, null) > 0.01f;
		}

		// Token: 0x0600F4A4 RID: 62628 RVA: 0x00471D50 File Offset: 0x0046FF50
		public static bool TerrainFlammableNow(this IntVec3 c, Map map)
		{
			TerrainDef terrain = c.GetTerrain(map);
			TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
			if (!terrain.Flammable() && (terrainDef == null || !terrainDef.Flammable()))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].FireBulwark)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F4A5 RID: 62629 RVA: 0x00471DB0 File Offset: 0x0046FFB0
		public static float TerrainFlammability(this IntVec3 c, Map map)
		{
			if (!c.TerrainFlammableNow(map))
			{
				return 0f;
			}
			BuildableDef terrain = c.GetTerrain(map);
			TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
			float num = terrain.GetStatValueAbstract(StatDefOf.Flammability, null);
			if (terrainDef != null)
			{
				num = Mathf.Max(num, terrainDef.GetStatValueAbstract(StatDefOf.Flammability, null));
			}
			return num;
		}

		// Token: 0x0600F4A6 RID: 62630 RVA: 0x00471E04 File Offset: 0x00470004
		public static List<Fire> GetFiresNearCell(this IntVec3 cell, Map map)
		{
			FireUtility.fireList.Clear();
			Room room = RegionAndRoomQuery.RoomAt(cell, map, RegionType.Set_All);
			if (room == null || room.Dereferenced || room.Fogged || room.IsHuge || room.TouchesMapEdge)
			{
				Region region = cell.GetRegion(map, RegionType.Set_Passable);
				if (region == null)
				{
					List<Thing> list = map.thingGrid.ThingsListAt(cell);
					for (int i = 0; i < list.Count; i++)
					{
						Fire fire = list[i] as Fire;
						if (fire != null && fire.parent == null)
						{
							FireUtility.fireList.Add(fire);
						}
					}
				}
				else
				{
					region.ListerThings.GetThingsOfType<Fire>(FireUtility.fireList);
				}
			}
			else
			{
				List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
				for (int j = 0; j < containedAndAdjacentThings.Count; j++)
				{
					Fire fire2 = containedAndAdjacentThings[j] as Fire;
					if (fire2 != null)
					{
						FireUtility.fireList.Add(fire2);
					}
				}
			}
			FireUtility.fireList.Shuffle<Fire>();
			FireUtility.fireList.Swap(0, FireUtility.fireList.FindIndex(0, (Fire f) => f.Position == cell));
			return FireUtility.fireList;
		}

		// Token: 0x0400A13D RID: 41277
		private static readonly SimpleCurve ChanceToCatchFirePerSecondForPawnFromFlammability = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 0.07f),
				true
			},
			{
				new CurvePoint(0.3f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x0400A13E RID: 41278
		private static readonly List<Fire> fireList = new List<Fire>();
	}
}
