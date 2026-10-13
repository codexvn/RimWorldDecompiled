using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C98 RID: 15512
	public abstract class WorldDrawLayer_SingleTile : WorldDrawLayer
	{
		// Token: 0x17003482 RID: 13442
		// (get) Token: 0x06015141 RID: 86337
		protected abstract PlanetTile Tile { get; }

		// Token: 0x17003483 RID: 13443
		// (get) Token: 0x06015142 RID: 86338
		protected abstract Material Material { get; }

		// Token: 0x17003484 RID: 13444
		// (get) Token: 0x06015143 RID: 86339 RVA: 0x0064EE46 File Offset: 0x0064D046
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || this.Tile != this.lastDrawnPlanetTile;
			}
		}

		// Token: 0x06015144 RID: 86340 RVA: 0x0064EE63 File Offset: 0x0064D063
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			PlanetTile tile = this.Tile;
			if (tile.Valid && tile.Layer == this.planetLayer)
			{
				LayerSubMesh subMesh = base.GetSubMesh(this.Material);
				Find.WorldGrid.GetTileVertices(tile, this.verts);
				int count = subMesh.verts.Count;
				int i = 0;
				int count2 = this.verts.Count;
				while (i < count2)
				{
					subMesh.verts.Add(this.verts[i] + this.verts[i].normalized * 0.02f);
					subMesh.uvs.Add((GenGeo.RegularPolygonVertexPosition(count2, i, 0f) + Vector2.one) / 2f);
					if (i < count2 - 2)
					{
						subMesh.tris.Add(count + i + 2);
						subMesh.tris.Add(count + i + 1);
						subMesh.tris.Add(count);
					}
					i++;
				}
				base.FinalizeMesh(MeshParts.All);
			}
			this.lastDrawnPlanetTile = tile;
			yield break;
			yield break;
		}

		// Token: 0x0400EB25 RID: 60197
		private PlanetTile lastDrawnPlanetTile = new PlanetTile(-1);

		// Token: 0x0400EB26 RID: 60198
		private List<Vector3> verts = new List<Vector3>();
	}
}
