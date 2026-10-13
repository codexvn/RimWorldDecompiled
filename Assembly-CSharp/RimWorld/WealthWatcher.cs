using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Unity.Collections;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200219E RID: 8606
	public class WealthWatcher
	{
		// Token: 0x17001C8C RID: 7308
		// (get) Token: 0x0600BBFE RID: 48126 RVA: 0x003676FA File Offset: 0x003658FA
		public int HealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.totalHealth;
			}
		}

		// Token: 0x17001C8D RID: 7309
		// (get) Token: 0x0600BBFF RID: 48127 RVA: 0x00367708 File Offset: 0x00365908
		public float WealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems + this.wealthBuildings + this.wealthPawns;
			}
		}

		// Token: 0x17001C8E RID: 7310
		// (get) Token: 0x0600BC00 RID: 48128 RVA: 0x00367724 File Offset: 0x00365924
		public float WealthItems
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems;
			}
		}

		// Token: 0x17001C8F RID: 7311
		// (get) Token: 0x0600BC01 RID: 48129 RVA: 0x00367732 File Offset: 0x00365932
		public float WealthBuildings
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthBuildings;
			}
		}

		// Token: 0x17001C90 RID: 7312
		// (get) Token: 0x0600BC02 RID: 48130 RVA: 0x00367740 File Offset: 0x00365940
		public float WealthFloorsOnly
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthFloorsOnly;
			}
		}

		// Token: 0x17001C91 RID: 7313
		// (get) Token: 0x0600BC03 RID: 48131 RVA: 0x0036774E File Offset: 0x0036594E
		public float WealthPawns
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthPawns;
			}
		}

		// Token: 0x0600BC04 RID: 48132 RVA: 0x0036775C File Offset: 0x0036595C
		public static void ResetStaticData()
		{
			int num = -1;
			List<TerrainDef> allDefsListForReading = DefDatabase<TerrainDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				num = Mathf.Max(num, (int)allDefsListForReading[i].index);
			}
			WealthWatcher.cachedTerrainMarketValue = new float[num + 1];
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				WealthWatcher.cachedTerrainMarketValue[(int)allDefsListForReading[j].index] = allDefsListForReading[j].GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
		}

		// Token: 0x0600BC05 RID: 48133 RVA: 0x003677D7 File Offset: 0x003659D7
		public WealthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BC06 RID: 48134 RVA: 0x003677FC File Offset: 0x003659FC
		private void RecountIfNeeded()
		{
			if ((float)Find.TickManager.TicksGame - this.lastCountTick > 5000f)
			{
				this.ForceRecount(false);
			}
		}

		// Token: 0x0600BC07 RID: 48135 RVA: 0x00367820 File Offset: 0x00365A20
		public void ForceRecount(bool allowDuringInit = false)
		{
			if (!allowDuringInit && Current.ProgramState != ProgramState.Playing)
			{
				Log.Error("WealthWatcher recount in game mode " + Current.ProgramState.ToString());
				return;
			}
			this.wealthItems = this.CalculateWealthItems();
			this.wealthBuildings = 0f;
			this.wealthPawns = 0f;
			this.wealthFloorsOnly = 0f;
			this.totalHealth = 0;
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.Faction == Faction.OfPlayer)
				{
					this.wealthBuildings += thing.GetStatValue(StatDefOf.MarketValueIgnoreHp, true, -1);
					this.totalHealth += thing.HitPoints;
				}
			}
			this.wealthFloorsOnly = this.CalculateWealthFloors();
			this.wealthBuildings += this.wealthFloorsOnly;
			foreach (Pawn pawn in this.map.mapPawns.PawnsInFaction(Faction.OfPlayer))
			{
				if (!pawn.IsQuestLodger())
				{
					float num = pawn.MarketValue;
					if (pawn.IsSlave)
					{
						num *= 0.75f;
					}
					this.wealthPawns += num;
					if (pawn.IsFreeColonist)
					{
						this.totalHealth += Mathf.RoundToInt(pawn.health.summaryHealth.SummaryHealthPercent * 100f);
					}
				}
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps)
			{
				if (pocketMapParent.HasMap && pocketMapParent.sourceMap == this.map)
				{
					if (Current.ProgramState == ProgramState.Playing)
					{
						pocketMapParent.Map.wealthWatcher.ForceRecount(false);
					}
					this.wealthItems += pocketMapParent.Map.wealthWatcher.wealthItems;
					this.wealthBuildings += pocketMapParent.Map.wealthWatcher.wealthBuildings;
					this.wealthPawns += pocketMapParent.Map.wealthWatcher.wealthPawns;
					this.wealthFloorsOnly += pocketMapParent.Map.wealthWatcher.wealthFloorsOnly;
					this.totalHealth += pocketMapParent.Map.wealthWatcher.totalHealth;
				}
			}
			this.lastCountTick = (float)Find.TickManager.TicksGame;
		}

		// Token: 0x0600BC08 RID: 48136 RVA: 0x00367AF0 File Offset: 0x00365CF0
		public static float GetEquipmentApparelAndInventoryWealth(Pawn p)
		{
			float num = 0f;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					num += allEquipmentListForReading[i].MarketValue * (float)allEquipmentListForReading[i].stackCount;
				}
			}
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					num += wornApparel[j].MarketValue * (float)wornApparel[j].stackCount;
				}
			}
			if (p.inventory != null)
			{
				ThingOwner<Thing> innerContainer = p.inventory.innerContainer;
				for (int k = 0; k < innerContainer.Count; k++)
				{
					num += innerContainer[k].MarketValue * (float)innerContainer[k].stackCount;
				}
			}
			return num;
		}

		// Token: 0x0600BC09 RID: 48137 RVA: 0x00367BDC File Offset: 0x00365DDC
		private float CalculateWealthItems()
		{
			this.tmpThings.Clear();
			ThingOwnerUtility.GetAllThingsRecursively<Thing>(this.map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), this.tmpThings, false, new Predicate<IThingHolder>(WealthWatcher.WealthItemsFilter), true);
			float num = 0f;
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.tmpThings[i].SpawnedOrAnyParentSpawned && !this.tmpThings[i].PositionHeld.Fogged(this.map))
				{
					num += this.tmpThings[i].MarketValue * (float)this.tmpThings[i].stackCount;
				}
			}
			this.tmpThings.Clear();
			return num;
		}

		// Token: 0x0600BC0A RID: 48138 RVA: 0x00367C9C File Offset: 0x00365E9C
		public static bool WealthItemsFilter(IThingHolder x)
		{
			if (x is PassingShip || x is MapComponent)
			{
				return false;
			}
			Pawn pawn = x as Pawn;
			return (pawn == null || pawn.Faction == Faction.OfPlayer) && (pawn == null || !pawn.IsQuestLodger());
		}

		// Token: 0x0600BC0B RID: 48139 RVA: 0x00367CE4 File Offset: 0x00365EE4
		private float CalculateWealthFloors()
		{
			TerrainDef[] topGrid = this.map.terrainGrid.topGrid;
			TerrainDef[] foundationGrid = this.map.terrainGrid.foundationGrid;
			NativeBitArray fogGrid_Unsafe = this.map.fogGrid.FogGrid_Unsafe;
			IntVec3 size = this.map.Size;
			float num = 0f;
			if (!fogGrid_Unsafe.IsCreated)
			{
				return 0f;
			}
			int i = 0;
			int num2 = size.x * size.z;
			while (i < num2)
			{
				if (!fogGrid_Unsafe.IsSet(i) && topGrid[i] != null)
				{
					num += WealthWatcher.cachedTerrainMarketValue[(int)topGrid[i].index];
				}
				if (foundationGrid[i] != null)
				{
					num += WealthWatcher.cachedTerrainMarketValue[(int)foundationGrid[i].index];
				}
				i++;
			}
			return num;
		}

		// Token: 0x04008171 RID: 33137
		private Map map;

		// Token: 0x04008172 RID: 33138
		private float wealthItems;

		// Token: 0x04008173 RID: 33139
		private float wealthBuildings;

		// Token: 0x04008174 RID: 33140
		private float wealthPawns;

		// Token: 0x04008175 RID: 33141
		private float wealthFloorsOnly;

		// Token: 0x04008176 RID: 33142
		private int totalHealth;

		// Token: 0x04008177 RID: 33143
		private float lastCountTick = -99999f;

		// Token: 0x04008178 RID: 33144
		private static float[] cachedTerrainMarketValue;

		// Token: 0x04008179 RID: 33145
		private const int MinCountInterval = 5000;

		// Token: 0x0400817A RID: 33146
		public const float WealthFactor_Slave = 0.75f;

		// Token: 0x0400817B RID: 33147
		private List<Thing> tmpThings = new List<Thing>();
	}
}
