using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D8C RID: 15756
	[StaticConstructorOnStartup]
	public static class SettleUtility
	{
		// Token: 0x17003561 RID: 13665
		// (get) Token: 0x06015759 RID: 87897 RVA: 0x0066E190 File Offset: 0x0066C390
		public static bool PlayerSettlementsCountLimitReached
		{
			get
			{
				int num = 0;
				foreach (Map map in Find.Maps)
				{
					if (map.IsPlayerHome && map.Parent is Settlement)
					{
						num++;
					}
					else if (map.wasSpawnedViaGravShipLanding)
					{
						num++;
					}
				}
				return num >= Prefs.MaxNumberOfPlayerSettlements;
			}
		}

		// Token: 0x0601575A RID: 87898 RVA: 0x0066E210 File Offset: 0x0066C410
		public static Settlement AddNewHome(PlanetTile tile, Faction faction)
		{
			AbandonedArchotechStructures abandonedArchotechStructures = Find.WorldObjects.WorldObjectAt<AbandonedArchotechStructures>(tile);
			Settlement settlement;
			if (abandonedArchotechStructures != null)
			{
				settlement = abandonedArchotechStructures.GenerateSettlementAndDestroy();
			}
			else
			{
				settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			}
			settlement.Tile = tile;
			settlement.SetFaction(faction);
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
			Find.WorldObjects.Add(settlement);
			if (faction == Faction.OfPlayer)
			{
				TaleRecorder.RecordTale(TaleDefOf.TileSettled, Array.Empty<object>()).customLabel = "NewSettlement".Translate();
				if (Find.IdeoManager != null)
				{
					Find.IdeoManager.lastResettledTick = GenTicks.TicksGame;
				}
			}
			return settlement;
		}

		// Token: 0x0400EEE6 RID: 61158
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);

		// Token: 0x0400EEE7 RID: 61159
		public static readonly Texture2D CreateCampCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/CreateCamp", true);
	}
}
