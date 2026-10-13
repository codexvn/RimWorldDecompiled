using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BD8 RID: 15320
	public class QuestNode_EndGame_ShipEscape_FindShipTile : QuestNode
	{
		// Token: 0x06014CC7 RID: 85191 RVA: 0x0063B1E8 File Offset: 0x006393E8
		private bool TryFindRootTile(out PlanetTile tile)
		{
			return TileFinder.TryFindRandomPlayerTile(out tile, false, delegate(PlanetTile x)
			{
				PlanetTile planetTile;
				return this.TryFindDestinationTileActual(x, 180, out planetTile);
			}, false, null);
		}

		// Token: 0x06014CC8 RID: 85192 RVA: 0x0063B200 File Offset: 0x00639400
		private bool TryFindDestinationTile(PlanetTile rootTile, out PlanetTile tile)
		{
			int num = 800;
			for (int i = 0; i < 1000; i++)
			{
				num = (int)((float)num * Rand.Range(0.5f, 0.75f));
				if (num <= 180)
				{
					num = 180;
				}
				if (this.TryFindDestinationTileActual(rootTile, num, out tile))
				{
					return true;
				}
				if (num <= 180)
				{
					return false;
				}
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x06014CC9 RID: 85193 RVA: 0x0063B268 File Offset: 0x00639468
		private bool TryFindDestinationTileActual(PlanetTile rootTile, int minDist, out PlanetTile tile)
		{
			for (int i = 0; i < 2; i++)
			{
				bool canTraverseImpassable = i == 1;
				if (TileFinder.TryFindPassableTileWithTraversalDistance(rootTile, minDist, 800, out tile, (PlanetTile x) => !Find.WorldObjects.AnyWorldObjectAt(x) && Find.WorldGrid[x].PrimaryBiome.canBuildBase && Find.WorldGrid[x].PrimaryBiome.canAutoChoose, true, TileFinderMode.Near, canTraverseImpassable, false))
				{
					return true;
				}
			}
			tile = PlanetTile.Invalid;
			return false;
		}

		// Token: 0x06014CCA RID: 85194 RVA: 0x0063B2C8 File Offset: 0x006394C8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PlanetTile rootTile;
			this.TryFindRootTile(out rootTile);
			PlanetTile var;
			this.TryFindDestinationTile(rootTile, out var);
			slate.Set<PlanetTile>(this.storeAs.GetValue(slate), var, false);
		}

		// Token: 0x06014CCB RID: 85195 RVA: 0x0063B304 File Offset: 0x00639504
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile rootTile;
			PlanetTile planetTile;
			return this.TryFindRootTile(out rootTile) && this.TryFindDestinationTile(rootTile, out planetTile);
		}

		// Token: 0x0400E782 RID: 59266
		private const int MinTraversalDistance = 180;

		// Token: 0x0400E783 RID: 59267
		private const int MaxTraversalDistance = 800;

		// Token: 0x0400E784 RID: 59268
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
