using System;
using System.Collections.Generic;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C3E RID: 15422
	public class WorldFeature : IExposable, ILoadReferenceable
	{
		// Token: 0x06014EC0 RID: 85696 RVA: 0x00643DFE File Offset: 0x00641FFE
		protected static void FeatureSizePoint10_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06014EC1 RID: 85697 RVA: 0x00643DFE File Offset: 0x00641FFE
		protected static void FeatureSizePoint25_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06014EC2 RID: 85698 RVA: 0x00643DFE File Offset: 0x00641FFE
		protected static void FeatureSizePoint50_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06014EC3 RID: 85699 RVA: 0x00643DFE File Offset: 0x00641FFE
		protected static void FeatureSizePoint100_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06014EC4 RID: 85700 RVA: 0x00643DFE File Offset: 0x00641FFE
		protected static void FeatureSizePoint200_Changed()
		{
			WorldFeature.TweakChanged();
		}

		// Token: 0x06014EC5 RID: 85701 RVA: 0x00643E08 File Offset: 0x00642008
		private static void TweakChanged()
		{
			Find.WorldFeatures.textsCreated = false;
			WorldFeature.EffectiveDrawSizeCurve[0] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[0].x, WorldFeature.FeatureSizePoint10);
			WorldFeature.EffectiveDrawSizeCurve[1] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[1].x, WorldFeature.FeatureSizePoint25);
			WorldFeature.EffectiveDrawSizeCurve[2] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[2].x, WorldFeature.FeatureSizePoint50);
			WorldFeature.EffectiveDrawSizeCurve[3] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[3].x, WorldFeature.FeatureSizePoint100);
			WorldFeature.EffectiveDrawSizeCurve[4] = new CurvePoint(WorldFeature.EffectiveDrawSizeCurve[4].x, WorldFeature.FeatureSizePoint200);
		}

		// Token: 0x170033BC RID: 13244
		// (get) Token: 0x06014EC6 RID: 85702 RVA: 0x00643EE8 File Offset: 0x006420E8
		public float EffectiveDrawSize
		{
			get
			{
				return WorldFeature.EffectiveDrawSizeCurve.Evaluate(this.maxDrawSizeInTiles);
			}
		}

		// Token: 0x06014EC7 RID: 85703 RVA: 0x000024DF File Offset: 0x000006DF
		public WorldFeature()
		{
		}

		// Token: 0x06014EC8 RID: 85704 RVA: 0x00643EFA File Offset: 0x006420FA
		public WorldFeature(FeatureDef def, PlanetLayer layer)
		{
			this.def = def;
			this.layer = layer;
			this.uniqueID = Find.UniqueIDsManager.GetNextWorldFeatureID();
		}

		// Token: 0x06014EC9 RID: 85705 RVA: 0x00643F20 File Offset: 0x00642120
		public void ExposeData()
		{
			Scribe_Defs.Look<FeatureDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.uniqueID, "uniqueID", 0, false);
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<Vector3>(ref this.drawCenter, "drawCenter", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.drawAngle, "drawAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.maxDrawSizeInTiles, "maxDrawSizeInTiles", 0f, false);
			Scribe_References.Look<PlanetLayer>(ref this.layer, "layer", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.layer == null)
			{
				this.layer = Find.WorldGrid.Surface;
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06014ECA RID: 85706 RVA: 0x00643FDE File Offset: 0x006421DE
		public string GetUniqueLoadID()
		{
			return "WorldFeature_" + this.uniqueID.ToString();
		}

		// Token: 0x170033BD RID: 13245
		// (get) Token: 0x06014ECB RID: 85707 RVA: 0x00643FF5 File Offset: 0x006421F5
		public IEnumerable<int> Tiles
		{
			get
			{
				WorldGrid worldGrid = Find.WorldGrid;
				int tilesCount = worldGrid.TilesCount;
				int num;
				for (int i = 0; i < tilesCount; i = num + 1)
				{
					if (worldGrid[i].feature == this)
					{
						yield return i;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0400E941 RID: 59713
		public int uniqueID;

		// Token: 0x0400E942 RID: 59714
		public FeatureDef def;

		// Token: 0x0400E943 RID: 59715
		public PlanetLayer layer;

		// Token: 0x0400E944 RID: 59716
		public string name;

		// Token: 0x0400E945 RID: 59717
		public Vector3 drawCenter;

		// Token: 0x0400E946 RID: 59718
		public float drawAngle;

		// Token: 0x0400E947 RID: 59719
		public float maxDrawSizeInTiles;

		// Token: 0x0400E948 RID: 59720
		public float alpha;

		// Token: 0x0400E949 RID: 59721
		protected static SimpleCurve EffectiveDrawSizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 15f),
				true
			},
			{
				new CurvePoint(25f, 40f),
				true
			},
			{
				new CurvePoint(50f, 90f),
				true
			},
			{
				new CurvePoint(100f, 150f),
				true
			},
			{
				new CurvePoint(200f, 200f),
				true
			}
		};

		// Token: 0x0400E94A RID: 59722
		[TweakValue("Interface.World", 0f, 40f)]
		protected static float FeatureSizePoint10 = 15f;

		// Token: 0x0400E94B RID: 59723
		[TweakValue("Interface.World", 0f, 100f)]
		protected static float FeatureSizePoint25 = 40f;

		// Token: 0x0400E94C RID: 59724
		[TweakValue("Interface.World", 0f, 200f)]
		protected static float FeatureSizePoint50 = 90f;

		// Token: 0x0400E94D RID: 59725
		[TweakValue("Interface.World", 0f, 400f)]
		protected static float FeatureSizePoint100 = 150f;

		// Token: 0x0400E94E RID: 59726
		[TweakValue("Interface.World", 0f, 800f)]
		protected static float FeatureSizePoint200 = 200f;
	}
}
