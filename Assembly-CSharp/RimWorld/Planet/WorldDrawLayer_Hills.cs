using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C81 RID: 15489
	public class WorldDrawLayer_Hills : WorldDrawLayer
	{
		// Token: 0x060150B2 RID: 86194 RVA: 0x0064C4A7 File Offset: 0x0064A6A7
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			for (int i = 0; i < this.planetLayer.TilesCount; i++)
			{
				Tile tile = this.planetLayer[i];
				if (tile.Landmark == null)
				{
					Material material;
					FloatRange floatRange;
					switch (tile.hilliness)
					{
					case Hilliness.SmallHills:
						material = WorldMaterials.SmallHills;
						floatRange = WorldDrawLayer_Hills.BasePosOffsetRange_SmallHills;
						break;
					case Hilliness.LargeHills:
						material = WorldMaterials.LargeHills;
						floatRange = WorldDrawLayer_Hills.BasePosOffsetRange_LargeHills;
						break;
					case Hilliness.Mountainous:
						material = WorldMaterials.Mountains;
						floatRange = WorldDrawLayer_Hills.BasePosOffsetRange_Mountains;
						break;
					case Hilliness.Impassable:
						material = WorldMaterials.ImpassableMountains;
						floatRange = WorldDrawLayer_Hills.BasePosOffsetRange_ImpassableMountains;
						break;
					default:
						goto IL_206;
					}
					LayerSubMesh subMesh = base.GetSubMesh(material);
					Vector3 vector = this.planetLayer.GetTileCenter(new PlanetTile(i, this.planetLayer));
					Vector3 posForTangents = vector;
					float magnitude = vector.magnitude;
					vector = (vector + Rand.UnitVector3 * (floatRange.RandomInRange * this.planetLayer.AverageTileSize)).normalized * magnitude;
					WorldRendererUtility.PrintQuadTangentialToPlanet(vector, posForTangents, WorldDrawLayer_Hills.BaseSizeRange.RandomInRange * this.planetLayer.GetTileSize(tile.tile), 0.005f, subMesh, false, Rand.Range(0f, 360f), false);
					WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, WorldDrawLayer_Hills.TexturesInAtlas.x), Rand.Range(0, WorldDrawLayer_Hills.TexturesInAtlas.z), WorldDrawLayer_Hills.TexturesInAtlas.x, WorldDrawLayer_Hills.TexturesInAtlas.z, subMesh);
				}
				IL_206:;
			}
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x0400EAC1 RID: 60097
		private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x0400EAC2 RID: 60098
		private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

		// Token: 0x0400EAC3 RID: 60099
		private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

		// Token: 0x0400EAC4 RID: 60100
		private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

		// Token: 0x0400EAC5 RID: 60101
		private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

		// Token: 0x0400EAC6 RID: 60102
		private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);
	}
}
