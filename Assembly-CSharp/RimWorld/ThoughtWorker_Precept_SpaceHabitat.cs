using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001842 RID: 6210
	public class ThoughtWorker_Precept_SpaceHabitat : ThoughtWorker_Precept
	{
		// Token: 0x06008F65 RID: 36709 RVA: 0x002A5AC0 File Offset: 0x002A3CC0
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.OdysseyActive || !p.SpawnedOrAnyParentSpawned)
			{
				return ThoughtState.Inactive;
			}
			PlanetTile tile = p.Tile;
			if (tile.Valid && tile.LayerDef.isSpace)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
