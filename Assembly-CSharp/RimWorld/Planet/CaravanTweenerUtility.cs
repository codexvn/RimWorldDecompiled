using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D73 RID: 15731
	public static class CaravanTweenerUtility
	{
		// Token: 0x060156EC RID: 87788 RVA: 0x0066A164 File Offset: 0x00668364
		public static Vector3 PatherTweenedPosRoot(Caravan caravan)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			if (!caravan.Spawned)
			{
				return worldGrid.GetTileCenter(caravan.Tile);
			}
			if (caravan.pather.Moving)
			{
				float num;
				if (!caravan.pather.IsNextTilePassable())
				{
					num = 0f;
				}
				else
				{
					num = 1f - caravan.pather.nextTileCostLeft / caravan.pather.nextTileCostTotal;
				}
				PlanetTile tile;
				if (caravan.pather.nextTile == caravan.Tile && caravan.pather.previousTileForDrawingIfInDoubt.Valid)
				{
					tile = caravan.pather.previousTileForDrawingIfInDoubt;
				}
				else
				{
					tile = caravan.Tile;
				}
				return worldGrid.GetTileCenter(caravan.pather.nextTile) * num + worldGrid.GetTileCenter(tile) * (1f - num);
			}
			return worldGrid.GetTileCenter(caravan.Tile);
		}

		// Token: 0x060156ED RID: 87789 RVA: 0x0066A24C File Offset: 0x0066844C
		public static Vector3 CaravanCollisionPosOffsetFor(Caravan caravan)
		{
			if (!caravan.Spawned)
			{
				return Vector3.zero;
			}
			bool flag = caravan.Spawned && caravan.pather.Moving;
			float d = 0.15f * Find.WorldGrid.AverageTileSize;
			if (!flag || caravan.pather.nextTile == caravan.pather.Destination)
			{
				PlanetTile tile;
				if (flag)
				{
					tile = caravan.pather.nextTile;
				}
				else
				{
					tile = caravan.Tile;
				}
				int num;
				int vertexIndex;
				CaravanTweenerUtility.GetCaravansStandingAtOrAboutToStandAt(tile, out num, out vertexIndex, caravan);
				if (num == 0)
				{
					return Vector3.zero;
				}
				return WorldRendererUtility.ProjectOnQuadTangentialToPlanet(Find.WorldGrid.GetTileCenter(tile), GenGeo.RegularPolygonVertexPosition(num, vertexIndex, 0f) * d);
			}
			else
			{
				if (CaravanTweenerUtility.DrawPosCollides(caravan))
				{
					Rand.PushState();
					Rand.Seed = caravan.ID;
					float f = Rand.Range(0f, 360f);
					Rand.PopState();
					Vector2 point = new Vector2(Mathf.Cos(f), Mathf.Sin(f)) * d;
					return WorldRendererUtility.ProjectOnQuadTangentialToPlanet(CaravanTweenerUtility.PatherTweenedPosRoot(caravan), point);
				}
				return Vector3.zero;
			}
		}

		// Token: 0x060156EE RID: 87790 RVA: 0x0066A35C File Offset: 0x0066855C
		private static void GetCaravansStandingAtOrAboutToStandAt(PlanetTile tile, out int caravansCount, out int caravansWithLowerIdCount, Caravan forCaravan)
		{
			caravansCount = 0;
			caravansWithLowerIdCount = 0;
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			int i = 0;
			while (i < caravans.Count)
			{
				Caravan caravan = caravans[i];
				if (caravan.Tile != tile)
				{
					if (caravan.pather.Moving && !(caravan.pather.nextTile != caravan.pather.Destination))
					{
						if (!(caravan.pather.Destination != tile))
						{
							goto IL_7A;
						}
					}
				}
				else if (!caravan.pather.Moving)
				{
					goto IL_7A;
				}
				IL_94:
				i++;
				continue;
				IL_7A:
				caravansCount++;
				if (caravan.ID < forCaravan.ID)
				{
					caravansWithLowerIdCount++;
					goto IL_94;
				}
				goto IL_94;
			}
		}

		// Token: 0x060156EF RID: 87791 RVA: 0x0066A410 File Offset: 0x00668610
		private static bool DrawPosCollides(Caravan caravan)
		{
			Vector3 a = CaravanTweenerUtility.PatherTweenedPosRoot(caravan);
			float num = Find.WorldGrid.AverageTileSize * 0.2f;
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan2 = caravans[i];
				if (caravan2 != caravan && Vector3.Distance(a, CaravanTweenerUtility.PatherTweenedPosRoot(caravan2)) < num)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400EE89 RID: 61065
		private const float BaseRadius = 0.15f;

		// Token: 0x0400EE8A RID: 61066
		private const float BaseDistToCollide = 0.2f;
	}
}
