using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C83 RID: 15491
	public class WorldDrawLayer_Landmarks : WorldDrawLayer
	{
		// Token: 0x1700345B RID: 13403
		// (get) Token: 0x060150BF RID: 86207 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700345C RID: 13404
		// (get) Token: 0x060150C0 RID: 86208 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700345D RID: 13405
		// (get) Token: 0x060150C1 RID: 86209 RVA: 0x0032F764 File Offset: 0x0032D964
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive;
			}
		}

		// Token: 0x060150C2 RID: 86210 RVA: 0x0064C867 File Offset: 0x0064AA67
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			if (Find.World.landmarks != null)
			{
				foreach (KeyValuePair<PlanetTile, Landmark> keyValuePair in Find.World.landmarks.landmarks)
				{
					PlanetTile planetTile;
					Landmark landmark;
					keyValuePair.Deconstruct(out planetTile, out landmark);
					PlanetTile tile = planetTile;
					Landmark landmark2 = landmark;
					if (this.planetLayer == tile.Layer && landmark2.def.drawType == LandmarkDef.LandmarkDrawType.Standard)
					{
						this.DrawStandard(landmark2, tile);
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x060150C3 RID: 86211 RVA: 0x0064C878 File Offset: 0x0064AA78
		private void DrawStandard(Landmark landmark, PlanetTile tile)
		{
			LayerSubMesh subMesh = base.GetSubMesh(landmark.def.Material);
			Vector3 tileCenter = this.planetLayer.GetTileCenter(tile);
			float rotation = Rand.Range(0f, 360f);
			if (landmark.def.coastRotateMode != LandmarkDef.CoastRotateMode.None)
			{
				this.planetLayer.GetTileNeighbors(tile, WorldDrawLayer_Landmarks.neighbours);
				PlanetTile toTile = PlanetTile.Invalid;
				for (int i = 0; i < WorldDrawLayer_Landmarks.neighbours.Count; i++)
				{
					if (LandmarkDef.IsValidRotatableNeighbour(this.planetLayer[WorldDrawLayer_Landmarks.neighbours[i]].PrimaryBiome, landmark.def.coastRotateMode))
					{
						toTile = WorldDrawLayer_Landmarks.neighbours[i];
						break;
					}
				}
				if (toTile.Valid)
				{
					rotation = this.planetLayer.GetHeadingFromTo(tile, toTile) - 90f;
				}
			}
			Rand.PushState(tile.tileId);
			WorldRendererUtility.PrintQuadTangentialToPlanet(tileCenter, this.planetLayer.GetTileSize(tile) * landmark.def.drawScale, landmark.def.drawAboveRoads ? 0.025f : 0.0075f, subMesh, false, rotation, false);
			WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, landmark.def.atlasSize.x), Rand.Range(0, landmark.def.atlasSize.z), landmark.def.atlasSize.x, landmark.def.atlasSize.z, subMesh);
			Rand.PopState();
		}

		// Token: 0x0400EACC RID: 60108
		private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);
	}
}
