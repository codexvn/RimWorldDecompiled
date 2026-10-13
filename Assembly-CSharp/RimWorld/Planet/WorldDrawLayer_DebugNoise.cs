using System;
using System.Collections;
using System.Collections.Generic;
using LudeonTK;
using Unity.Collections;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02003C7D RID: 15485
	public class WorldDrawLayer_DebugNoise : WorldDrawLayer
	{
		// Token: 0x17003452 RID: 13394
		// (get) Token: 0x06015096 RID: 86166 RVA: 0x0064BDD0 File Offset: 0x00649FD0
		public override bool Visible
		{
			get
			{
				Dialog_DevNoiseWorld dialog_DevNoiseWorld;
				return base.Visible && this.TryGetWindow(out dialog_DevNoiseWorld) && dialog_DevNoiseWorld.alpha > 0f;
			}
		}

		// Token: 0x17003453 RID: 13395
		// (get) Token: 0x06015097 RID: 86167 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003454 RID: 13396
		// (get) Token: 0x06015098 RID: 86168 RVA: 0x00002501 File Offset: 0x00000701
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06015099 RID: 86169 RVA: 0x0064BDFE File Offset: 0x00649FFE
		private bool TryGetWindow(out Dialog_DevNoiseWorld window)
		{
			return Find.WindowStack.TryGetWindow<Dialog_DevNoiseWorld>(out window);
		}

		// Token: 0x0601509A RID: 86170 RVA: 0x0064BE0B File Offset: 0x0064A00B
		public override IEnumerable Regenerate()
		{
			if (this.material == null)
			{
				this.material = new Material(WorldMaterials.VertexColorTransparent);
			}
			Dialog_DevNoiseWorld window;
			this.TryGetWindow(out window);
			Perlin perlin = window.noise;
			if (perlin == null)
			{
				perlin = new Perlin(0.017000000923871994, 2.0, 0.5, 6, true, false, Find.World.info.Seed, QualityMode.Medium);
			}
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			NativeArray<int> unsafeTileIDToVerts_offsets = this.planetLayer.UnsafeTileIDToVerts_offsets;
			NativeArray<Vector3> unsafeVerts = this.planetLayer.UnsafeVerts;
			byte a = (byte)Mathf.Clamp(Mathf.RoundToInt(window.alpha * 255.999f), 0, 255);
			this.triangleIndexToTileID.Clear();
			for (int i = 0; i < this.planetLayer.TilesCount; i++)
			{
				Vector3 tileCenter = Find.WorldGrid.GetTileCenter(new PlanetTile(i, this.planetLayer));
				float num = (float)perlin.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z);
				byte b = (byte)Mathf.Clamp(Mathf.RoundToInt(num * 255.999f), 0, 255);
				if (window.cutoff == 0f || num >= window.cutoff || !window.cutoffEnabled)
				{
					int j;
					LayerSubMesh subMesh = base.GetSubMesh(this.material, out j);
					while (j >= this.triangleIndexToTileID.Count)
					{
						this.triangleIndexToTileID.Add(new List<int>());
					}
					int count = subMesh.verts.Count;
					int num2 = 0;
					int num3 = (i + 1 < unsafeTileIDToVerts_offsets.Length) ? unsafeTileIDToVerts_offsets[i + 1] : unsafeVerts.Length;
					for (int k = unsafeTileIDToVerts_offsets[i]; k < num3; k++)
					{
						subMesh.verts.Add(unsafeVerts[k]);
						subMesh.colors.Add(new Color32(b, b, b, a));
						if (k < num3 - 2)
						{
							subMesh.tris.Add(count + num2 + 2);
							subMesh.tris.Add(count + num2 + 1);
							subMesh.tris.Add(count);
							this.triangleIndexToTileID[j].Add(i);
						}
						num2++;
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x0400EAB0 RID: 60080
		private readonly List<List<int>> triangleIndexToTileID = new List<List<int>>();

		// Token: 0x0400EAB1 RID: 60081
		private Material material;
	}
}
