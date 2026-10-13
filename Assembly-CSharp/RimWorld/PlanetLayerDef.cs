using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB2 RID: 6834
	public class PlanetLayerDef : Def
	{
		// Token: 0x17001775 RID: 6005
		// (get) Token: 0x06009910 RID: 39184 RVA: 0x002C99ED File Offset: 0x002C7BED
		public bool SurfaceTiles
		{
			get
			{
				return this.usesSurfaceTiles;
			}
		}

		// Token: 0x17001776 RID: 6006
		// (get) Token: 0x06009911 RID: 39185 RVA: 0x002C99F5 File Offset: 0x002C7BF5
		public BiomeDef DefaultBiome
		{
			get
			{
				return this.defaultBiome ?? BiomeDefOf.TemperateForest;
			}
		}

		// Token: 0x17001777 RID: 6007
		// (get) Token: 0x06009912 RID: 39186 RVA: 0x002C9A08 File Offset: 0x002C7C08
		public Texture2D ViewGizmoTexture
		{
			get
			{
				if (!this.cachedGizmoTexture)
				{
					return this.cachedGizmoTexture = ContentFinder<Texture2D>.Get(this.viewGizmoTexPath, true);
				}
				return this.cachedGizmoTexture;
			}
		}

		// Token: 0x17001778 RID: 6008
		// (get) Token: 0x06009913 RID: 39187 RVA: 0x002C9A3E File Offset: 0x002C7C3E
		public WorldObjectDef DefaultWorldObject
		{
			get
			{
				return this.defaultMapWorldObject ?? WorldObjectDefOf.Settlement;
			}
		}

		// Token: 0x17001779 RID: 6009
		// (get) Token: 0x06009914 RID: 39188 RVA: 0x002C9A4F File Offset: 0x002C7C4F
		public WorldObjectDef SettlementWorldObjectDef
		{
			get
			{
				return this.settlementWorldObjectDef ?? WorldObjectDefOf.Settlement;
			}
		}

		// Token: 0x1700177A RID: 6010
		// (get) Token: 0x06009915 RID: 39189 RVA: 0x002C9A60 File Offset: 0x002C7C60
		public WorldObjectDef DestroyedSettlementWorldObjectDef
		{
			get
			{
				return this.destroyedSettlementWorldObjectDef ?? WorldObjectDefOf.DestroyedSettlement;
			}
		}

		// Token: 0x1700177B RID: 6011
		// (get) Token: 0x06009916 RID: 39190 RVA: 0x002C9A71 File Offset: 0x002C7C71
		public Material WorldLineMaterial
		{
			get
			{
				return MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, this.lineColor, 3590);
			}
		}

		// Token: 0x1700177C RID: 6012
		// (get) Token: 0x06009917 RID: 39191 RVA: 0x002C9A8D File Offset: 0x002C7C8D
		public Material WorldLineMaterialHighVis
		{
			get
			{
				return MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, this.lineColor, 3590);
			}
		}

		// Token: 0x1700177D RID: 6013
		// (get) Token: 0x06009918 RID: 39192 RVA: 0x002C9AAC File Offset: 0x002C7CAC
		public List<WorldGenStepDef> GenStepsInOrder
		{
			get
			{
				List<WorldGenStepDef> result;
				if ((result = this.cachedGenSteps) == null)
				{
					result = (this.cachedGenSteps = (from x in this.worldGenSteps
					orderby x.order, x.index
					select x).ToList<WorldGenStepDef>());
				}
				return result;
			}
		}

		// Token: 0x1700177E RID: 6014
		// (get) Token: 0x06009919 RID: 39193 RVA: 0x002C9B1F File Offset: 0x002C7D1F
		public List<WITab> Tabs
		{
			get
			{
				return this.cachedTabs;
			}
		}

		// Token: 0x0600991A RID: 39194 RVA: 0x002C9B27 File Offset: 0x002C7D27
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.tileType == null || !this.tileType.SameOrSubclassOf<Tile>())
			{
				yield return "tileType type is not the same or subclass of Tile";
			}
			if (this.layerType == null || !this.layerType.SameOrSubclassOf<PlanetLayer>())
			{
				yield return "layerType type is not the same or subclass of PlanetLayer";
			}
			foreach (Type type in this.worldDrawLayers)
			{
				if (!type.IsSubclassOf(typeof(WorldDrawLayer)))
				{
					yield return string.Format("{0} layer {1} is type that is not a subclass of {2}", "worldDrawLayers", type, "WorldDrawLayer");
				}
			}
			List<Type>.Enumerator enumerator2 = default(List<Type>.Enumerator);
			foreach (Type type2 in this.worldTabs)
			{
				if (!type2.IsSubclassOf(typeof(WITab)))
				{
					yield return string.Format("{0} layer {1} is type that is not a subclass of {2}", "worldTabs", type2, "WITab");
				}
			}
			enumerator2 = default(List<Type>.Enumerator);
			if (string.IsNullOrEmpty(this.viewGizmoTexPath))
			{
				yield return "Must provide a texture to viewGizmoTexPath, use PlaceholderImage if required.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600991B RID: 39195 RVA: 0x002C9B38 File Offset: 0x002C7D38
		public override void PostLoad()
		{
			base.PostLoad();
			this.usesSurfaceTiles = typeof(SurfaceTile).IsAssignableFrom(this.tileType);
			foreach (Type type in this.worldTabs)
			{
				this.cachedTabs.Add((WITab)Activator.CreateInstance(type));
			}
		}

		// Token: 0x040067F1 RID: 26609
		private BiomeDef defaultBiome;

		// Token: 0x040067F2 RID: 26610
		private WorldObjectDef defaultMapWorldObject;

		// Token: 0x040067F3 RID: 26611
		private WorldObjectDef settlementWorldObjectDef;

		// Token: 0x040067F4 RID: 26612
		private WorldObjectDef destroyedSettlementWorldObjectDef;

		// Token: 0x040067F5 RID: 26613
		public bool onlyAllowWhitelistedIncidents;

		// Token: 0x040067F6 RID: 26614
		public bool onlyAllowWhitelistedBiomes;

		// Token: 0x040067F7 RID: 26615
		public bool onlyAllowWhitelistedGameConditions;

		// Token: 0x040067F8 RID: 26616
		public bool onlyAllowWhitelistedQuests;

		// Token: 0x040067F9 RID: 26617
		public bool onlyAllowWhitelistedArrivals;

		// Token: 0x040067FA RID: 26618
		public bool onlyAllowWhitelistedArrivalModes;

		// Token: 0x040067FB RID: 26619
		public bool alwaysRaycastable = true;

		// Token: 0x040067FC RID: 26620
		public bool obstructsExpandingIcons = true;

		// Token: 0x040067FD RID: 26621
		public bool canFormCaravans = true;

		// Token: 0x040067FE RID: 26622
		public bool isSpace;

		// Token: 0x040067FF RID: 26623
		public float rangeDistanceFactor = 1f;

		// Token: 0x04006800 RID: 26624
		public float generatedLocationFactor = 1f;

		// Token: 0x04006801 RID: 26625
		public float raidPointsFactor = 1f;

		// Token: 0x04006802 RID: 26626
		public BiomeDef backgroundBiome;

		// Token: 0x04006803 RID: 26627
		public bool ignoreNoBuildArea;

		// Token: 0x04006804 RID: 26628
		public Color lineColor = Color.white;

		// Token: 0x04006805 RID: 26629
		public float lineWidthFactor = 1f;

		// Token: 0x04006806 RID: 26630
		private List<WorldGenStepDef> worldGenSteps = new List<WorldGenStepDef>();

		// Token: 0x04006807 RID: 26631
		public List<Type> worldDrawLayers = new List<Type>();

		// Token: 0x04006808 RID: 26632
		private List<Type> worldTabs = new List<Type>();

		// Token: 0x04006809 RID: 26633
		public Type layerType = typeof(PlanetLayer);

		// Token: 0x0400680A RID: 26634
		public Type tileType = typeof(Tile);

		// Token: 0x0400680B RID: 26635
		[MustTranslate]
		public string elevationString = "{0}m";

		// Token: 0x0400680C RID: 26636
		[MustTranslate]
		public string viewGizmoTooltip;

		// Token: 0x0400680D RID: 26637
		[MustTranslate]
		public string gerundLabel;

		// Token: 0x0400680E RID: 26638
		public bool viewGizmoOnlyVisibleWithDirectConnection = true;

		// Token: 0x0400680F RID: 26639
		public FloatRange settlementsPer100kTiles = new FloatRange(75f, 85f);

		// Token: 0x04006810 RID: 26640
		public SimpleCurve viewAngleSettlementsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x04006811 RID: 26641
		public string viewGizmoTexPath;

		// Token: 0x04006812 RID: 26642
		[Unsaved(false)]
		private List<WorldGenStepDef> cachedGenSteps;

		// Token: 0x04006813 RID: 26643
		[Unsaved(false)]
		private readonly List<WITab> cachedTabs = new List<WITab>();

		// Token: 0x04006814 RID: 26644
		[Unsaved(false)]
		private Texture2D cachedGizmoTexture;

		// Token: 0x04006815 RID: 26645
		private bool usesSurfaceTiles;
	}
}
