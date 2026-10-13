using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000219 RID: 537
	public abstract class BuildableDef : Def, IEquatable<BuildableDef>
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x00042F14 File Offset: 0x00041114
		public virtual IntVec2 Size
		{
			get
			{
				return IntVec2.One;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x00042F1B File Offset: 0x0004111B
		public bool MadeFromStuff
		{
			get
			{
				return !this.stuffCategories.NullOrEmpty<StuffCategoryDef>();
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000E38 RID: 3640 RVA: 0x00042F2B File Offset: 0x0004112B
		public bool BuildableByPlayer
		{
			get
			{
				return this.designationCategory != null;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x00042F36 File Offset: 0x00041136
		public Material DrawMatSingle
		{
			get
			{
				Graphic graphic = this.graphic;
				if (graphic == null)
				{
					return null;
				}
				return graphic.MatSingle;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000E3A RID: 3642 RVA: 0x00042F49 File Offset: 0x00041149
		public float Altitude
		{
			get
			{
				return this.altitudeLayer.AltitudeFor();
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00042F56 File Offset: 0x00041156
		public bool AffectsFertility
		{
			get
			{
				return this.fertility >= 0f;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000E3C RID: 3644 RVA: 0x00042F68 File Offset: 0x00041168
		public List<PlaceWorker> PlaceWorkers
		{
			get
			{
				if (this.placeWorkers == null)
				{
					return null;
				}
				if (this.placeWorkersInstantiatedInt == null)
				{
					this.placeWorkersInstantiatedInt = new List<PlaceWorker>();
					foreach (Type type in this.placeWorkers)
					{
						this.placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(type));
					}
				}
				return this.placeWorkersInstantiatedInt;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x00042FF0 File Offset: 0x000411F0
		public bool IsResearchFinished
		{
			get
			{
				if (this.researchPrerequisites != null)
				{
					for (int i = 0; i < this.researchPrerequisites.Count; i++)
					{
						if (!this.researchPrerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00043031 File Offset: 0x00041231
		public List<ThingDefCountClass> CostList
		{
			get
			{
				if (this.costListForDifficulty != null && this.costListForDifficulty.Applies)
				{
					return this.costListForDifficulty.costList;
				}
				return this.costList;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000E3F RID: 3647 RVA: 0x0004305A File Offset: 0x0004125A
		public int CostStuffCount
		{
			get
			{
				if (this.costListForDifficulty != null && this.costListForDifficulty.Applies)
				{
					return this.costListForDifficulty.costStuffCount;
				}
				return this.costStuffCount;
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00043084 File Offset: 0x00041284
		public bool ForceAllowPlaceOver(BuildableDef other)
		{
			if (this.PlaceWorkers == null)
			{
				return false;
			}
			for (int i = 0; i < this.PlaceWorkers.Count; i++)
			{
				if (this.PlaceWorkers[i].ForceAllowPlaceOver(other))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x000430C8 File Offset: 0x000412C8
		public override void PostLoad()
		{
			base.PostLoad();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.uiIconPath.NullOrEmpty())
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.uiIconPath, true);
				}
				else
				{
					this.ResolveIcon();
				}
				if (this.uiIconPathsStuff != null)
				{
					this.stuffUiIcons = new Dictionary<StuffAppearanceDef, Texture2D>();
					foreach (IconForStuffAppearance iconForStuffAppearance in this.uiIconPathsStuff)
					{
						this.stuffUiIcons.Add(iconForStuffAppearance.Appearance, ContentFinder<Texture2D>.Get(iconForStuffAppearance.IconPath, true));
					}
				}
			});
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x000430E4 File Offset: 0x000412E4
		protected virtual void ResolveIcon()
		{
			if (this.graphic == null || this.graphic == BaseContent.BadGraphic)
			{
				return;
			}
			ThingDef thingDef = this as ThingDef;
			if (thingDef != null && thingDef.mote != null)
			{
				return;
			}
			Graphic outerGraphic = this.graphic;
			if (this.uiIconForStackCount >= 1 && this is ThingDef)
			{
				Graphic_StackCount graphic_StackCount = this.graphic as Graphic_StackCount;
				if (graphic_StackCount != null)
				{
					outerGraphic = graphic_StackCount.SubGraphicForStackCount(this.uiIconForStackCount, (ThingDef)this);
				}
			}
			Material material = outerGraphic.ExtractInnerGraphicFor(null, null).MatAt(this.defaultPlacingRot, null);
			Shader shader;
			MaterialRequest materialRequest;
			if (ShaderDatabase.TryGetUIShader(material.shader, out shader) && MaterialPool.TryGetRequestForMat(material, out materialRequest))
			{
				materialRequest.shader = shader;
				if (materialRequest.colorTwo == Color.white)
				{
					materialRequest.colorTwo = this.uiIconColorTwo;
				}
				this.uiIconMaterial = MaterialPool.MatFrom(materialRequest);
			}
			this.uiIcon = (Texture2D)material.mainTexture;
			this.uiIconColor = material.color;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x000431E0 File Offset: 0x000413E0
		public Texture2D GetUIIconForStuff(ThingDef stuff)
		{
			Texture2D result;
			if (this.stuffUiIcons == null || ((stuff != null) ? stuff.stuffProps.appearance : null) == null || !this.stuffUiIcons.TryGetValue(stuff.stuffProps.appearance, out result))
			{
				return this.uiIcon;
			}
			return result;
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0004322C File Offset: 0x0004142C
		public Color GetColorForStuff(ThingDef stuff)
		{
			if (this.colorPerStuff.NullOrEmpty<ColorForStuff>())
			{
				return stuff.stuffProps.color;
			}
			for (int i = 0; i < this.colorPerStuff.Count; i++)
			{
				ColorForStuff colorForStuff = this.colorPerStuff[i];
				if (colorForStuff.Stuff == stuff)
				{
					return colorForStuff.Color;
				}
			}
			return stuff.stuffProps.color;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00043290 File Offset: 0x00041490
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.useStuffTerrainAffordance && !this.MadeFromStuff)
			{
				yield return "useStuffTerrainAffordance is true but it's not made from stuff";
			}
			if (this.costListForDifficulty != null && this.costListForDifficulty.difficultyVar.NullOrEmpty())
			{
				yield return "costListForDifficulty is not referencing a difficulty.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x000432A0 File Offset: 0x000414A0
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.BuildableByPlayer)
			{
				IEnumerable<TerrainAffordanceDef> enumerable = Enumerable.Empty<TerrainAffordanceDef>();
				if (this.PlaceWorkers != null)
				{
					enumerable = enumerable.Concat(this.PlaceWorkers.SelectMany((PlaceWorker pw) => pw.DisplayAffordances()));
				}
				TerrainAffordanceDef terrainAffordanceNeed = this.GetTerrainAffordanceNeed(req.StuffDef);
				if (terrainAffordanceNeed != null)
				{
					enumerable = enumerable.Concat(terrainAffordanceNeed);
				}
				string[] array = (from ta in enumerable.Distinct<TerrainAffordanceDef>()
				orderby ta.order
				select ta.label).ToArray<string>();
				if (array.Length != 0)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), array.ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_TerrainRequirement_Desc".Translate(), 1101, null, null, false, false);
				}
				BuildableDef.tmpCostList.Clear();
				BuildableDef.tmpHyperlinks.Clear();
				if (this.MadeFromStuff && this.costStuffCount > 0)
				{
					BuildableDef.tmpCostList.Add(this.costStuffCount.ToString() + "x " + (from x in this.stuffCategories
					select x.label).ToCommaListOr(false));
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
					{
						if (thingDef.IsStuff && !thingDef.stuffProps.categories.NullOrEmpty<StuffCategoryDef>() && thingDef.stuffProps.categories.Any((StuffCategoryDef x) => this.stuffCategories.Contains(x)))
						{
							BuildableDef.tmpHyperlinks.Add(new Dialog_InfoCard.Hyperlink(thingDef, -1));
						}
					}
				}
				List<ThingDefCountClass> list = this.CostList;
				if (!list.NullOrEmpty<ThingDefCountClass>())
				{
					using (List<ThingDefCountClass>.Enumerator enumerator3 = list.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							ThingDefCountClass c = enumerator3.Current;
							BuildableDef.tmpCostList.Add(c.Summary);
							if (!BuildableDef.tmpHyperlinks.Any((Dialog_InfoCard.Hyperlink x) => x.def == c.thingDef))
							{
								BuildableDef.tmpHyperlinks.Add(new Dialog_InfoCard.Hyperlink(c.thingDef, -1));
							}
						}
					}
				}
				if (BuildableDef.tmpCostList.Any<string>())
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Building, "Stat_Building_ResourcesToMake".Translate(), BuildableDef.tmpCostList.ToCommaList(false, false).CapitalizeFirst(), "Stat_Building_ResourcesToMakeDesc".Translate(), 4405, null, BuildableDef.tmpHyperlinks, false, false);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x00040A11 File Offset: 0x0003EC11
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x000432B7 File Offset: 0x000414B7
		public bool Equals(BuildableDef other)
		{
			return this.defName == other.defName;
		}

		// Token: 0x04000890 RID: 2192
		public List<StatModifier> statBases;

		// Token: 0x04000891 RID: 2193
		public Traversability passability;

		// Token: 0x04000892 RID: 2194
		public int pathCost;

		// Token: 0x04000893 RID: 2195
		public bool pathCostIgnoreRepeat = true;

		// Token: 0x04000894 RID: 2196
		public float fertility = -1f;

		// Token: 0x04000895 RID: 2197
		public List<ThingDefCountClass> costList;

		// Token: 0x04000896 RID: 2198
		public int costStuffCount;

		// Token: 0x04000897 RID: 2199
		public List<StuffCategoryDef> stuffCategories;

		// Token: 0x04000898 RID: 2200
		[MustTranslate]
		public string stuffCategorySummary;

		// Token: 0x04000899 RID: 2201
		public CostListForDifficulty costListForDifficulty;

		// Token: 0x0400089A RID: 2202
		public DrawStyleCategoryDef drawStyleCategory;

		// Token: 0x0400089B RID: 2203
		public bool clearBuildingArea = true;

		// Token: 0x0400089C RID: 2204
		public Rot4 defaultPlacingRot = Rot4.North;

		// Token: 0x0400089D RID: 2205
		public float resourcesFractionWhenDeconstructed = 0.5f;

		// Token: 0x0400089E RID: 2206
		public List<AltitudeLayer> blocksAltitudes;

		// Token: 0x0400089F RID: 2207
		public StyleCategoryDef dominantStyleCategory;

		// Token: 0x040008A0 RID: 2208
		public bool forcePassableByFlyingPawns;

		// Token: 0x040008A1 RID: 2209
		public bool forceMoveItemsBeforeConstruction;

		// Token: 0x040008A2 RID: 2210
		public bool isAltar;

		// Token: 0x040008A3 RID: 2211
		public bool useStuffTerrainAffordance;

		// Token: 0x040008A4 RID: 2212
		public TerrainAffordanceDef terrainAffordanceNeeded;

		// Token: 0x040008A5 RID: 2213
		public List<ThingDef> buildingPrerequisites;

		// Token: 0x040008A6 RID: 2214
		public List<ThingDef> discoveryPrerequisites;

		// Token: 0x040008A7 RID: 2215
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040008A8 RID: 2216
		public int minMonolithLevel;

		// Token: 0x040008A9 RID: 2217
		public int constructionSkillPrerequisite;

		// Token: 0x040008AA RID: 2218
		public int artisticSkillPrerequisite;

		// Token: 0x040008AB RID: 2219
		public TechLevel minTechLevelToBuild;

		// Token: 0x040008AC RID: 2220
		public TechLevel maxTechLevelToBuild;

		// Token: 0x040008AD RID: 2221
		public bool requireInspectedGravEngine;

		// Token: 0x040008AE RID: 2222
		public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

		// Token: 0x040008AF RID: 2223
		public EffecterDef repairEffect;

		// Token: 0x040008B0 RID: 2224
		public EffecterDef constructEffect;

		// Token: 0x040008B1 RID: 2225
		public List<ColorForStuff> colorPerStuff;

		// Token: 0x040008B2 RID: 2226
		public bool canGenerateDefaultDesignator = true;

		// Token: 0x040008B3 RID: 2227
		public bool ideoBuilding;

		// Token: 0x040008B4 RID: 2228
		public float specialDisplayRadius;

		// Token: 0x040008B5 RID: 2229
		public List<Type> placeWorkers;

		// Token: 0x040008B6 RID: 2230
		public DesignationCategoryDef designationCategory;

		// Token: 0x040008B7 RID: 2231
		public DesignatorDropdownGroupDef designatorDropdown;

		// Token: 0x040008B8 RID: 2232
		public KeyBindingDef designationHotKey;

		// Token: 0x040008B9 RID: 2233
		public float uiOrder = 2999f;

		// Token: 0x040008BA RID: 2234
		[NoTranslate]
		public string uiIconPath;

		// Token: 0x040008BB RID: 2235
		public List<IconForStuffAppearance> uiIconPathsStuff;

		// Token: 0x040008BC RID: 2236
		public Vector2 uiIconOffset;

		// Token: 0x040008BD RID: 2237
		public Color uiIconColor = Color.white;

		// Token: 0x040008BE RID: 2238
		public Color uiIconColorTwo = Color.white;

		// Token: 0x040008BF RID: 2239
		public int uiIconForStackCount = -1;

		// Token: 0x040008C0 RID: 2240
		[Unsaved(false)]
		public ThingDef blueprintDef;

		// Token: 0x040008C1 RID: 2241
		[Unsaved(false)]
		public ThingDef installBlueprintDef;

		// Token: 0x040008C2 RID: 2242
		[Unsaved(false)]
		public ThingDef frameDef;

		// Token: 0x040008C3 RID: 2243
		[Unsaved(false)]
		private List<PlaceWorker> placeWorkersInstantiatedInt;

		// Token: 0x040008C4 RID: 2244
		[Unsaved(false)]
		public Graphic graphic = BaseContent.BadGraphic;

		// Token: 0x040008C5 RID: 2245
		[Unsaved(false)]
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x040008C6 RID: 2246
		[Unsaved(false)]
		public Material uiIconMaterial;

		// Token: 0x040008C7 RID: 2247
		[Unsaved(false)]
		public Dictionary<StuffAppearanceDef, Texture2D> stuffUiIcons;

		// Token: 0x040008C8 RID: 2248
		[Unsaved(false)]
		public float uiIconAngle;

		// Token: 0x040008C9 RID: 2249
		protected static List<string> tmpCostList = new List<string>();

		// Token: 0x040008CA RID: 2250
		protected static List<Dialog_InfoCard.Hyperlink> tmpHyperlinks = new List<Dialog_InfoCard.Hyperlink>();
	}
}
