using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003951 RID: 14673
	public class SymbolResolver_DesiccatedCorpses : SymbolResolver
	{
		// Token: 0x06014302 RID: 82690 RVA: 0x006053E0 File Offset: 0x006035E0
		public override void Resolve(ResolveParams rp)
		{
			if (rp.desiccatedCorpsePawnKind != null && !rp.desiccatedCorpsePawnKind.RaceProps.IsFlesh)
			{
				Log.Error("Cannot create desiccated corpses for non-flesh based pawns.");
				return;
			}
			Map map = BaseGen.globalSettings.map;
			IntRange? desiccatedCorpseRandomAgeRange = rp.desiccatedCorpseRandomAgeRange;
			if (desiccatedCorpseRandomAgeRange == null)
			{
				IntRange defaultCorpseAge = SymbolResolver_DesiccatedCorpses.DefaultCorpseAge;
			}
			else
			{
				desiccatedCorpseRandomAgeRange.GetValueOrDefault();
			}
			SymbolResolver_DesiccatedCorpses.tmpSeenRooms.Clear();
			foreach (IntVec3 loc in rp.rect)
			{
				Room room = loc.GetRoom(map);
				if (room != null && !SymbolResolver_DesiccatedCorpses.tmpSeenRooms.Contains(room) && !room.PsychologicallyOutdoors && room.CellCount < 1000 && Rand.Chance(0.04f))
				{
					int cellCount = room.CellCount;
					float chance = SymbolResolver_DesiccatedCorpses.CorpseSpawnChanceOverRoomSizeCurve.Evaluate((float)cellCount);
					foreach (IntVec3 intVec in room.Cells)
					{
						if (Rand.Chance(chance) && SymbolResolver_DesiccatedCorpses.CanSpawnAt(intVec, map))
						{
							PawnKindDef pawnKindDef = rp.desiccatedCorpsePawnKind ?? this.GetRandomPawnKindForCorpse();
							this.SpawnCorpse(pawnKindDef, intVec, SymbolResolver_DesiccatedCorpses.DefaultCorpseAge.RandomInRange, map);
						}
					}
				}
				SymbolResolver_DesiccatedCorpses.tmpSeenRooms.Add(room);
			}
			SymbolResolver_DesiccatedCorpses.tmpSeenRooms.Clear();
			int num = Mathf.Max(1, Mathf.RoundToInt((rp.dessicatedCorpseDensityRange ?? SymbolResolver_DesiccatedCorpses.DefaultCorpseDensity).RandomInRange * (float)rp.rect.Area));
			for (int i = 0; i < num; i++)
			{
				IntVec3 spawnPosition = SymbolResolver_DesiccatedCorpses.FindRandomCorpseSpawnPosition(rp.rect, map);
				if (spawnPosition.IsValid)
				{
					PawnKindDef pawnKindDef2 = rp.desiccatedCorpsePawnKind ?? this.GetRandomPawnKindForCorpse();
					this.SpawnCorpse(pawnKindDef2, spawnPosition, SymbolResolver_DesiccatedCorpses.DefaultCorpseAge.RandomInRange, map);
				}
			}
		}

		// Token: 0x06014303 RID: 82691 RVA: 0x00605610 File Offset: 0x00603810
		private void SpawnCorpse(PawnKindDef pawnKindDef, IntVec3 spawnPosition, int age, Map map)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKindDef, null, PawnGenerationContext.NonPlayer, null, false, true, false, false, false, 0f, false, true, false, false, true, false, false, false, false, 0f, 0f, null, 0f, null, null, null, null, null, null, null, null, null, null, null, null, true, true, true, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			if (!pawn.Dead)
			{
				pawn.Kill(null, null);
			}
			if (pawn.inventory != null)
			{
				pawn.inventory.DestroyAll(DestroyMode.Vanish);
			}
			if (pawn.apparel != null)
			{
				pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (pawn.equipment != null)
			{
				pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			}
			pawn.Corpse.Age = age + Rand.Range(0, 900000);
			pawn.relations.hidePawnRelations = true;
			GenSpawn.Spawn(pawn.Corpse, spawnPosition, map, WipeMode.Vanish);
			pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
		}

		// Token: 0x06014304 RID: 82692 RVA: 0x00605759 File Offset: 0x00603959
		private PawnKindDef GetRandomPawnKindForCorpse()
		{
			return (from pk in DefDatabase<PawnKindDef>.AllDefs
			where pk.RaceProps.IsFlesh && pk.canBeScattered
			select pk).RandomElement<PawnKindDef>();
		}

		// Token: 0x06014305 RID: 82693 RVA: 0x0060578C File Offset: 0x0060398C
		private static IntVec3 FindRandomCorpseSpawnPosition(CellRect rect, Map map)
		{
			foreach (IntVec3 intVec in rect.Cells.InRandomOrder(null))
			{
				if (SymbolResolver_DesiccatedCorpses.CanSpawnAt(intVec, map))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06014306 RID: 82694 RVA: 0x006057F0 File Offset: 0x006039F0
		private static bool CanSpawnAt(IntVec3 cell, Map map)
		{
			return !cell.Impassable(map) && cell.GetThingList(map).Count == 0;
		}

		// Token: 0x0400DF2B RID: 57131
		private static FloatRange DefaultCorpseDensity = new FloatRange(0.001f, 0.002f);

		// Token: 0x0400DF2C RID: 57132
		private static IntRange DefaultCorpseAge = new IntRange(180000000, 720000000);

		// Token: 0x0400DF2D RID: 57133
		private const float ChanceToFillRoom = 0.04f;

		// Token: 0x0400DF2E RID: 57134
		private const int MaxRoomSizeToFill = 1000;

		// Token: 0x0400DF2F RID: 57135
		private static readonly SimpleCurve CorpseSpawnChanceOverRoomSizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.25f),
				true
			},
			{
				new CurvePoint(100f, 0.25f),
				true
			},
			{
				new CurvePoint(400f, 0.15f),
				true
			}
		};

		// Token: 0x0400DF30 RID: 57136
		private static HashSet<Room> tmpSeenRooms = new HashSet<Room>();
	}
}
