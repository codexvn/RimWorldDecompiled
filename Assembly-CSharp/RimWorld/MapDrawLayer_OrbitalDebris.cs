using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F7E RID: 8062
	public class MapDrawLayer_OrbitalDebris : MapDrawLayer
	{
		// Token: 0x17001B17 RID: 6935
		// (get) Token: 0x0600B229 RID: 45609 RVA: 0x0032EC20 File Offset: 0x0032CE20
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive && base.Map.OrbitalDebris != null;
			}
		}

		// Token: 0x0600B22A RID: 45610 RVA: 0x0032EC39 File Offset: 0x0032CE39
		public MapDrawLayer_OrbitalDebris(Map map) : base(map)
		{
			if (base.Map.OrbitalDebris == null)
			{
				return;
			}
			this.parallaxPer10CellLookup = new Dictionary<LayerSubMesh, float>();
			this.offsets = new Dictionary<OrbitalDebrisDef.DebrisGraphic, float>();
			this.CacheData();
		}

		// Token: 0x0600B22B RID: 45611 RVA: 0x0032EC6C File Offset: 0x0032CE6C
		private void CacheData()
		{
			this.parallaxPer10CellLookup.Clear();
			this.offsets.Clear();
			List<OrbitalDebrisDef.DebrisGraphic> list = (from debris in base.Map.OrbitalDebris.graphics
			orderby debris.order
			select debris).ToList<OrbitalDebrisDef.DebrisGraphic>();
			float num = 0.03658537f;
			for (int i = 0; i < list.Count; i++)
			{
				this.offsets[list[i]] = num;
				num += 0.03658537f;
			}
		}

		// Token: 0x0600B22C RID: 45612 RVA: 0x0032ECFC File Offset: 0x0032CEFC
		public override void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			Vector3 a = Find.Camera.transform.position.Yto0() - base.Map.Center.ToVector3().Yto0();
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Matrix4x4 matrix = Matrix4x4.Translate(a / 10f * this.parallaxPer10CellLookup[layerSubMesh]);
					Graphics.DrawMesh(layerSubMesh.mesh, matrix, layerSubMesh.material, layerSubMesh.renderLayer);
				}
			}
		}

		// Token: 0x0600B22D RID: 45613 RVA: 0x0032EDB4 File Offset: 0x0032CFB4
		public override void Regenerate()
		{
			if (!ModLister.CheckOdyssey("Orbital Debris"))
			{
				return;
			}
			base.ClearSubMeshes(MeshParts.All);
			this.parallaxPer10CellLookup.Clear();
			OrbitalDebrisDef orbitalDebris = base.Map.OrbitalDebris;
			Rand.PushState(base.Map.uniqueID);
			CellRect cellRect = base.Map.BoundsRect(0).ExpandedBy(30);
			int num = Mathf.RoundToInt((float)(cellRect.Size.x * cellRect.Size.z) / 10000f * MapDrawLayer_OrbitalDebris.GroupsPer100SqrTiles.RandomInRange);
			for (int i = 0; i < num; i++)
			{
				Vector3 vector;
				if (this.TryGetGroupCenter(out vector))
				{
					int randomInRange = MapDrawLayer_OrbitalDebris.PiecesPerCluster.RandomInRange;
					for (int j = 0; j < randomInRange; j++)
					{
						OrbitalDebrisDef.DebrisGraphic graphic = orbitalDebris.graphics.RandomElementByWeight((OrbitalDebrisDef.DebrisGraphic w) => w.weighting);
						Vector3 vect = vector + new Vector3((float)MapDrawLayer_OrbitalDebris.ClusterSize.RandomInRange, 0f, (float)MapDrawLayer_OrbitalDebris.ClusterSize.RandomInRange).RotatedBy(Rand.Range(0f, 360f));
						this.AddQuad(graphic, vect.ToIntVec3());
					}
					MapDrawLayer_OrbitalDebris.usedPoints.Add(vector);
				}
			}
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			MapDrawLayer_OrbitalDebris.usedPoints.Clear();
		}

		// Token: 0x0600B22E RID: 45614 RVA: 0x0032EF38 File Offset: 0x0032D138
		private bool TryGetGroupCenter(out Vector3 position)
		{
			position = Vector3.zero;
			Vector3 a = base.Map.Center.ToVector3();
			for (int i = 0; i < 20; i++)
			{
				float x = Rand.ByCurve(MapDrawLayer_OrbitalDebris.DistanceFromCenterWeightingCurve) * (float)base.Map.Size.x / 2f;
				int num = Rand.Range(0, 360);
				bool flag = true;
				for (int j = 0; j < 360; j += 20)
				{
					position = a + new Vector3(x, 0f, 0f).RotatedBy((float)((j + num) % 360));
					using (List<Vector3>.Enumerator enumerator = MapDrawLayer_OrbitalDebris.usedPoints.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (Vector3.Distance(enumerator.Current, position) <= (float)(MapDrawLayer_OrbitalDebris.ClusterSize.max * 2))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B22F RID: 45615 RVA: 0x0032F058 File Offset: 0x0032D258
		private void AddQuad(OrbitalDebrisDef.DebrisGraphic graphic, IntVec3 c)
		{
			if (!this.offsets.ContainsKey(graphic))
			{
				this.CacheData();
			}
			Material matSingle = graphic.graphicData.Graphic.MatSingle;
			LayerSubMesh subMesh = base.GetSubMesh(matSingle);
			subMesh.Clear(MeshParts.Verts | MeshParts.Tris | MeshParts.UVs1 | MeshParts.Normals);
			this.parallaxPer10CellLookup[subMesh] = graphic.parallaxPer10Cells;
			int count = subMesh.verts.Count;
			float num = Rand.RangeSeeded(0f, 0.03658537f, base.Map.cellIndices[c]);
			int num2 = Rand.Range(0, 360);
			for (int i = 0; i < 4; i++)
			{
				float randomInRange = MapDrawLayer_OrbitalDebris.SizeFactorRange.RandomInRange;
				Vector3 vector = new Vector3((MapDrawLayer_OrbitalDebris.UVs[i].x - 0.5f) * graphic.graphicData.drawSize.x * randomInRange, 0f, (MapDrawLayer_OrbitalDebris.UVs[i].y - 0.5f) * graphic.graphicData.drawSize.y * randomInRange);
				vector = vector.RotatedBy((float)num2);
				vector += new Vector3((float)c.x, AltitudeLayer.BelowTerrain.AltitudeFor() - this.offsets[graphic] + num, (float)c.z);
				subMesh.verts.Add(vector);
				subMesh.normals.Add(Vector3.up);
				subMesh.uvs.Add(MapDrawLayer_OrbitalDebris.UVs[i]);
			}
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 1);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count);
			subMesh.tris.Add(count + 2);
			subMesh.tris.Add(count + 3);
		}

		// Token: 0x040079D4 RID: 31188
		private readonly Dictionary<LayerSubMesh, float> parallaxPer10CellLookup;

		// Token: 0x040079D5 RID: 31189
		private readonly Dictionary<OrbitalDebrisDef.DebrisGraphic, float> offsets;

		// Token: 0x040079D6 RID: 31190
		private static readonly Vector2[] UVs = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};

		// Token: 0x040079D7 RID: 31191
		private static readonly FloatRange GroupsPer100SqrTiles = new FloatRange(2f, 2.5f);

		// Token: 0x040079D8 RID: 31192
		private static readonly FloatRange SizeFactorRange = new FloatRange(0.75f, 1.25f);

		// Token: 0x040079D9 RID: 31193
		private static readonly IntRange PiecesPerCluster = new IntRange(5, 20);

		// Token: 0x040079DA RID: 31194
		private static readonly IntRange ClusterSize = new IntRange(20, 40);

		// Token: 0x040079DB RID: 31195
		private static readonly SimpleCurve DistanceFromCenterWeightingCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 0.2f),
				true
			},
			{
				new CurvePoint(0.4f, 0.6f),
				true
			},
			{
				new CurvePoint(0.6f, 0.4f),
				true
			},
			{
				new CurvePoint(0.9f, 0.2f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};

		// Token: 0x040079DC RID: 31196
		private static readonly List<Vector3> usedPoints = new List<Vector3>();
	}
}
