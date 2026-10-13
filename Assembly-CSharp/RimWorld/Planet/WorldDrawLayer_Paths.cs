using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C86 RID: 15494
	public abstract class WorldDrawLayer_Paths : WorldDrawLayer
	{
		// Token: 0x17003464 RID: 13412
		// (get) Token: 0x060150D5 RID: 86229 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003465 RID: 13413
		// (get) Token: 0x060150D6 RID: 86230 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060150D7 RID: 86231 RVA: 0x0064CC84 File Offset: 0x0064AE84
		public void GeneratePaths(LayerSubMesh subMesh, PlanetTile tile, List<WorldDrawLayer_Paths.OutputDirection> nodes, Color32 color, bool allowSmoothTransition)
		{
			this.planetLayer.GetTileVertices(tile, this.tmpVerts);
			this.planetLayer.GetTileNeighbors(tile, this.tmpNeighbors);
			if (nodes.Count == 1 && this.pointyEnds)
			{
				int count = subMesh.verts.Count;
				this.AddPathEndpoint(subMesh, this.tmpVerts, this.tmpNeighbors.IndexOf(nodes[0].neighbor), color, tile, nodes[0]);
				subMesh.verts.Add(this.FinalizePoint(this.planetLayer.GetTileCenter(tile), nodes[0].distortionFrequency, nodes[0].distortionIntensity));
				subMesh.colors.Add(color.MutateAlpha(0));
				subMesh.tris.Add(count);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 1);
				subMesh.tris.Add(count + 3);
				subMesh.tris.Add(count + 2);
				return;
			}
			if (nodes.Count == 2)
			{
				int count2 = subMesh.verts.Count;
				int num = this.tmpNeighbors.IndexOf(nodes[0].neighbor);
				int num2 = this.tmpNeighbors.IndexOf(nodes[1].neighbor);
				if (allowSmoothTransition && Mathf.Abs(num - num2) > 1 && Mathf.Abs((num - num2 + this.tmpVerts.Count) % this.tmpVerts.Count) > 1)
				{
					this.AddPathEndpoint(subMesh, this.tmpVerts, num, color, tile, nodes[0]);
					this.AddPathEndpoint(subMesh, this.tmpVerts, num2, color, tile, nodes[1]);
					subMesh.tris.Add(count2);
					subMesh.tris.Add(count2 + 5);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 5);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 1);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 2);
					subMesh.tris.Add(count2 + 4);
					subMesh.tris.Add(count2 + 3);
					subMesh.tris.Add(count2 + 2);
					return;
				}
			}
			float num3 = 0f;
			for (int i = 0; i < nodes.Count; i++)
			{
				num3 = Mathf.Max(num3, nodes[i].width);
			}
			Vector3 vector = this.planetLayer.GetTileCenter(tile);
			this.tmpHexVerts.Clear();
			for (int j = 0; j < this.tmpVerts.Count; j++)
			{
				this.tmpHexVerts.Add(this.FinalizePoint(Vector3.LerpUnclamped(vector, this.tmpVerts[j], num3 * 0.5f * 2f), 0f, 0f));
			}
			vector = this.FinalizePoint(vector, 0f, 0f);
			int count3 = subMesh.verts.Count;
			subMesh.verts.Add(vector);
			subMesh.colors.Add(color);
			int count4 = subMesh.verts.Count;
			for (int k = 0; k < this.tmpHexVerts.Count; k++)
			{
				subMesh.verts.Add(this.tmpHexVerts[k]);
				subMesh.colors.Add(color.MutateAlpha(0));
				subMesh.tris.Add(count3);
				subMesh.tris.Add(count4 + (k + 1) % this.tmpHexVerts.Count);
				subMesh.tris.Add(count4 + k);
			}
			for (int l = 0; l < nodes.Count; l++)
			{
				if (nodes[l].width != 0f)
				{
					int count5 = subMesh.verts.Count;
					int num4 = this.tmpNeighbors.IndexOf(nodes[l].neighbor);
					this.AddPathEndpoint(subMesh, this.tmpVerts, num4, color, tile, nodes[l]);
					subMesh.tris.Add(count5);
					subMesh.tris.Add(count4 + (num4 + this.tmpHexVerts.Count - 1) % this.tmpHexVerts.Count);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5 + 1);
					subMesh.tris.Add(count5 + 1);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count5 + 2);
					subMesh.tris.Add(count3);
					subMesh.tris.Add(count4 + (num4 + 2) % this.tmpHexVerts.Count);
					subMesh.tris.Add(count5 + 2);
				}
			}
		}

		// Token: 0x060150D8 RID: 86232 RVA: 0x0064D1B4 File Offset: 0x0064B3B4
		private void AddPathEndpoint(LayerSubMesh subMesh, List<Vector3> verts, int index, Color32 color, PlanetTile tileID, WorldDrawLayer_Paths.OutputDirection data)
		{
			int index2 = (index + 1) % verts.Count;
			Find.WorldGrid.GetTileNeighbors(tileID, WorldDrawLayer_Paths.lhsID);
			Find.WorldGrid.GetTileNeighbors(data.neighbor, WorldDrawLayer_Paths.rhsID);
			float num = WorldDrawLayer_Paths.lhsID.Intersect(WorldDrawLayer_Paths.rhsID).Any((PlanetTile id) => Find.WorldGrid[id].WaterCovered) ? 0.5f : 1f;
			Vector3 a = this.FinalizePoint(verts[index], data.distortionFrequency, data.distortionIntensity * num);
			Vector3 b = this.FinalizePoint(verts[index2], data.distortionFrequency, data.distortionIntensity * num);
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f - data.width));
			subMesh.colors.Add(color.MutateAlpha(0));
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f));
			subMesh.colors.Add(color);
			subMesh.verts.Add(Vector3.LerpUnclamped(a, b, 0.5f + data.width));
			subMesh.colors.Add(color.MutateAlpha(0));
		}

		// Token: 0x060150D9 RID: 86233
		public abstract Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity);

		// Token: 0x0400EAD2 RID: 60114
		protected bool pointyEnds;

		// Token: 0x0400EAD3 RID: 60115
		private readonly List<Vector3> tmpVerts = new List<Vector3>();

		// Token: 0x0400EAD4 RID: 60116
		private readonly List<Vector3> tmpHexVerts = new List<Vector3>();

		// Token: 0x0400EAD5 RID: 60117
		private readonly List<PlanetTile> tmpNeighbors = new List<PlanetTile>();

		// Token: 0x0400EAD6 RID: 60118
		private static readonly List<PlanetTile> lhsID = new List<PlanetTile>();

		// Token: 0x0400EAD7 RID: 60119
		private static readonly List<PlanetTile> rhsID = new List<PlanetTile>();

		// Token: 0x02003C87 RID: 15495
		public struct OutputDirection
		{
			// Token: 0x0400EAD8 RID: 60120
			public PlanetTile neighbor;

			// Token: 0x0400EAD9 RID: 60121
			public float width;

			// Token: 0x0400EADA RID: 60122
			public float distortionFrequency;

			// Token: 0x0400EADB RID: 60123
			public float distortionIntensity;
		}
	}
}
