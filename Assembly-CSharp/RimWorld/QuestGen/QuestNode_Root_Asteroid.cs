using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B32 RID: 15154
	public class QuestNode_Root_Asteroid : QuestNode
	{
		// Token: 0x060149B3 RID: 84403 RVA: 0x00625F14 File Offset: 0x00624114
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return this.TryFindSiteTile(out planetTile);
		}

		// Token: 0x060149B4 RID: 84404 RVA: 0x00625F2C File Offset: 0x0062412C
		protected override void RunInt()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			PlanetTile tile;
			this.TryFindSiteTile(out tile);
			SpaceMapParent spaceMapParent = (SpaceMapParent)WorldObjectMaker.MakeWorldObject(this.worldObjectDef);
			spaceMapParent.Tile = tile;
			if (this.mineables != null)
			{
				ThingDef thingDef = this.mineables.RandomElement<ThingDef>();
				slate.Set<string>("resource", thingDef.label, false);
				spaceMapParent.nameInt = "AsteroidName".Translate(thingDef.label.Named("RESOURCE"));
				spaceMapParent.preciousResource = thingDef;
			}
			slate.Set<SpaceMapParent>("worldObject", spaceMapParent, false);
			quest.SpawnWorldObject(spaceMapParent, null, null);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("worldObject.MapRemoved");
			int delayTicks = QuestNode_Root_Asteroid.TimeoutDays.RandomInRange * 60000;
			quest.WorldObjectTimeout(spaceMapParent, delayTicks, null, null, false, null, true);
			quest.Delay(delayTicks, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x060149B5 RID: 84405 RVA: 0x0062605C File Offset: 0x0062425C
		private bool TryFindSiteTile(out PlanetTile tile)
		{
			tile = PlanetTile.Invalid;
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			PlanetTile planetTile;
			PlanetTile origin;
			if (!TileFinder.TryFindRandomPlayerTile(out planetTile, false, null, true, null))
			{
				origin = new PlanetTile(0, Find.WorldGrid.Surface);
			}
			else
			{
				origin = planetTile;
			}
			PlanetLayer planetLayer;
			if (!Find.WorldGrid.TryGetFirstAdjacentLayerOfDef(origin, this.layerDef, out planetLayer))
			{
				return false;
			}
			FastTileFinder.TileQueryParams query = new FastTileFinder.TileQueryParams(origin, 1f, 3f, FastTileFinder.LandmarkMode.Any, true, Hilliness.Undefined, Hilliness.Undefined, true, true, true);
			return planetLayer.FastTileFinder.Query(query, null, null, default(FastTileFinder.TileQueryParams)).TryRandomElement(out tile);
		}

		// Token: 0x0400E4C3 RID: 58563
		public PlanetLayerDef layerDef;

		// Token: 0x0400E4C4 RID: 58564
		public WorldObjectDef worldObjectDef;

		// Token: 0x0400E4C5 RID: 58565
		public List<ThingDef> mineables;

		// Token: 0x0400E4C6 RID: 58566
		private const int MaxDistanceFromColony = 3;

		// Token: 0x0400E4C7 RID: 58567
		private const int MinDistanceFromColony = 1;

		// Token: 0x0400E4C8 RID: 58568
		private static readonly IntRange TimeoutDays = new IntRange(45, 60);
	}
}
