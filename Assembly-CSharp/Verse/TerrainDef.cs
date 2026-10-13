using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002EE RID: 750
	public class TerrainDef : BuildableDef
	{
		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x00055525 File Offset: 0x00053725
		public bool Removable
		{
			get
			{
				return this.layerable;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001253 RID: 4691 RVA: 0x0005552D File Offset: 0x0005372D
		public bool IsCarpet
		{
			get
			{
				return this.researchPrerequisites.NotNullAndContains(ResearchProjectDefOf.CarpetMaking);
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001254 RID: 4692 RVA: 0x0005553F File Offset: 0x0005373F
		public Color DrawColor
		{
			get
			{
				if (this.colorDef != null)
				{
					return this.colorDef.color;
				}
				return this.color;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001255 RID: 4693 RVA: 0x0005555B File Offset: 0x0005375B
		public bool IsRiver
		{
			get
			{
				return this.HasTag("River");
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001256 RID: 4694 RVA: 0x00055568 File Offset: 0x00053768
		public bool IsOcean
		{
			get
			{
				return this.HasTag("Ocean");
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06001257 RID: 4695 RVA: 0x00055575 File Offset: 0x00053775
		public bool IsWater
		{
			get
			{
				return this.HasTag("Water");
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001258 RID: 4696 RVA: 0x00055582 File Offset: 0x00053782
		public bool IsIce
		{
			get
			{
				return this.HasTag("Ice");
			}
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001259 RID: 4697 RVA: 0x0005558F File Offset: 0x0005378F
		public bool IsFlood
		{
			get
			{
				return this.HasTag("Flood");
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x0600125A RID: 4698 RVA: 0x0005559C File Offset: 0x0005379C
		public bool IsFine
		{
			get
			{
				return this.HasTag("FineFloor");
			}
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x0600125B RID: 4699 RVA: 0x000555A9 File Offset: 0x000537A9
		public bool IsSoil
		{
			get
			{
				return this.HasTag("Soil");
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x000555B6 File Offset: 0x000537B6
		public bool IsRoad
		{
			get
			{
				return this.HasTag("Road");
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x0600125D RID: 4701 RVA: 0x000555C3 File Offset: 0x000537C3
		public bool IsFloor
		{
			get
			{
				return this.HasTag("Floor");
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x000555D0 File Offset: 0x000537D0
		public bool IsRock
		{
			get
			{
				return this.HasTag("NaturalRock");
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x000555DD File Offset: 0x000537DD
		public bool IsSubstructure
		{
			get
			{
				return ModsConfig.OdysseyActive && this.HasTag("Substructure");
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x000555F4 File Offset: 0x000537F4
		public Shader Shader
		{
			get
			{
				if (this.customShader != null)
				{
					return this.customShader.Shader;
				}
				Shader result = null;
				switch (this.edgeType)
				{
				case TerrainDef.TerrainEdgeType.Hard:
					result = ShaderDatabase.TerrainHard;
					break;
				case TerrainDef.TerrainEdgeType.Fade:
					result = ShaderDatabase.TerrainFade;
					break;
				case TerrainDef.TerrainEdgeType.FadeRough:
					result = ShaderDatabase.TerrainFadeRough;
					break;
				case TerrainDef.TerrainEdgeType.Water:
					result = ShaderDatabase.TerrainWater;
					break;
				}
				return result;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06001261 RID: 4705 RVA: 0x00055658 File Offset: 0x00053858
		private Shader ShaderPolluted
		{
			get
			{
				if (this.pollutionShaderType != null)
				{
					return this.pollutionShaderType.Shader;
				}
				if (this.customShader != null)
				{
					return this.customShader.Shader;
				}
				Shader result = null;
				switch (this.edgeType)
				{
				case TerrainDef.TerrainEdgeType.Hard:
					result = ShaderDatabase.TerrainHardPolluted;
					break;
				case TerrainDef.TerrainEdgeType.Fade:
					result = ShaderDatabase.TerrainFadePolluted;
					break;
				case TerrainDef.TerrainEdgeType.FadeRough:
					result = ShaderDatabase.TerrainFadeRoughPolluted;
					break;
				}
				return result;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x000556C1 File Offset: 0x000538C1
		public Material DrawMatPolluted
		{
			get
			{
				if (this.graphicPolluted == BaseContent.BadGraphic)
				{
					return this.graphic.MatSingle;
				}
				return this.graphicPolluted.MatSingle;
			}
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x000556E8 File Offset: 0x000538E8
		public override void PostLoad()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.drawStyleCategory == null)
				{
					this.drawStyleCategory = DrawStyleCategoryDefOf.Floors;
				}
				if (this.dontRender)
				{
					return;
				}
				if (this.graphic == BaseContent.BadGraphic)
				{
					this.graphic = GraphicDatabase.Get<Graphic_Terrain>(this.texturePath, this.Shader, Vector2.one, this.DrawColor, 2000 + this.renderPrecedence);
					if (this.edgeType == TerrainDef.TerrainEdgeType.FadeRough || this.edgeType == TerrainDef.TerrainEdgeType.Water)
					{
						this.graphic.MatSingle.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					}
					if (this.customShader != null && this.customShaderParameters != null)
					{
						for (int j = 0; j < this.customShaderParameters.Count; j++)
						{
							this.customShaderParameters[j].Apply(this.graphic.MatSingle);
						}
					}
				}
				if (!this.waterDepthShader.NullOrEmpty())
				{
					this.waterDepthMaterial = MaterialAllocator.Create(ShaderDatabase.LoadShader(this.waterDepthShader));
					this.waterDepthMaterial.renderQueue = 2000 + this.renderPrecedence;
					this.waterDepthMaterial.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					if (this.waterDepthShaderParameters != null)
					{
						for (int k = 0; k < this.waterDepthShaderParameters.Count; k++)
						{
							this.waterDepthShaderParameters[k].Apply(this.waterDepthMaterial);
						}
					}
				}
				if (ModsConfig.BiotechActive && this.graphicPolluted == BaseContent.BadGraphic && (!this.pollutionOverlayTexturePath.NullOrEmpty() || !this.pollutedTexturePath.NullOrEmpty()))
				{
					Texture2D texture2D = null;
					if (!this.pollutionOverlayTexturePath.NullOrEmpty())
					{
						texture2D = ContentFinder<Texture2D>.Get(this.pollutionOverlayTexturePath, true);
					}
					this.graphicPolluted = GraphicDatabase.Get<Graphic_Terrain>(this.pollutedTexturePath ?? this.texturePath, this.ShaderPolluted, Vector2.one, this.DrawColor, 2000 + this.renderPrecedence);
					Material matSingle = this.graphicPolluted.MatSingle;
					if (texture2D != null)
					{
						matSingle.SetTexture(ShaderPropertyIDs.BurnTex, texture2D);
					}
					matSingle.SetColor(ShaderPropertyIDs.BurnColor, this.pollutionColor);
					matSingle.SetVector(ShaderPropertyIDs.BurnScale, this.pollutionOverlayScale);
					matSingle.SetVector(ShaderPropertyIDs.ScrollSpeed, this.pollutionOverlayScrollSpeed);
					matSingle.SetColor(ShaderPropertyIDs.PollutionTintColor, this.pollutionTintColor);
					if (this.edgeType == TerrainDef.TerrainEdgeType.FadeRough)
					{
						matSingle.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					}
					if (matSingle != this.graphic.MatSingle)
					{
						matSingle.SetFloat(ShaderPropertyIDs.IsPolluted, 1f);
					}
					if ((this.pollutionShaderType != null || this.customShader != null) && this.customShaderParameters != null)
					{
						for (int l = 0; l < this.customShaderParameters.Count; l++)
						{
							this.customShaderParameters[l].Apply(matSingle);
						}
					}
				}
				if (this.bridgePropsPath != null && this.bridgePropsLoopGraphic == BaseContent.BadGraphic)
				{
					this.bridgePropsLoopGraphic = GraphicDatabase.Get<Graphic_Terrain>(this.bridgePropsPath + "_Loop", ShaderDatabase.Transparent);
					this.bridgePropsRightGraphic = GraphicDatabase.Get<Graphic_Terrain>(this.bridgePropsPath + "_Right", ShaderDatabase.Transparent);
				}
			});
			if (this.tools != null)
			{
				for (int i = 0; i < this.tools.Count; i++)
				{
					this.tools[i].id = i.ToString();
				}
			}
			base.PostLoad();
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x00055742 File Offset: 0x00053942
		protected override void ResolveIcon()
		{
			base.ResolveIcon();
			this.uiIconColor = this.DrawColor;
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00055756 File Offset: 0x00053956
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.texturePath.NullOrEmpty() && !this.dontRender)
			{
				yield return "missing texturePath";
			}
			if (this.fertility < 0f)
			{
				yield return "Terrain Def " + ((this != null) ? this.ToString() : null) + " has no fertility value set.";
			}
			if (this.renderPrecedence > 400)
			{
				yield return "Render order " + this.renderPrecedence.ToString() + " is out of range (must be < 400)";
			}
			if (this.generatedFilth != null && (this.filthAcceptanceMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None)
			{
				yield return this.defName + " makes terrain filth and also accepts it.";
			}
			if (this.Flammable() && this.burnedDef == null && !this.layerable)
			{
				yield return "flammable but burnedDef is null and not layerable";
			}
			if (this.burnedDef != null && this.burnedDef.Flammable())
			{
				yield return "burnedDef is flammable";
			}
			if (this.throwFleckChance > 0f && this.fleckData.fleck == null)
			{
				yield return "throwFleckChance is > 0 but fleckData.fleck is null";
			}
			yield break;
			yield break;
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00055766 File Offset: 0x00053966
		public static TerrainDef Named(string defName)
		{
			return DefDatabase<TerrainDef>.GetNamed(defName, true);
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0005576F File Offset: 0x0005396F
		public bool HasTag(string tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x00055787 File Offset: 0x00053987
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			string[] array = (from ta in this.affordances.Distinct<TerrainAffordanceDef>()
			orderby ta.order
			select ta.label).ToArray<string>();
			if (array.Length != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Terrain, "Supports".Translate(), array.ToCommaList(false, false).CapitalizeFirst(), "Stat_Thing_Terrain_Supports_Desc".Translate(), 2000, null, null, false, false);
			}
			if (this.IsFine && ModsConfig.RoyaltyActive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Terrain, "Stat_Thing_Terrain_Fine_Name".Translate(), "Stat_Thing_Terrain_Fine_Value".Translate(), "Stat_Thing_Terrain_Fine_Desc".Translate(), 2000, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x040010A8 RID: 4264
		[NoTranslate]
		public string texturePath;

		// Token: 0x040010A9 RID: 4265
		public TerrainDef.TerrainEdgeType edgeType;

		// Token: 0x040010AA RID: 4266
		public ShaderTypeDef customShader;

		// Token: 0x040010AB RID: 4267
		public List<ShaderParameter> customShaderParameters;

		// Token: 0x040010AC RID: 4268
		[NoTranslate]
		public string waterDepthShader;

		// Token: 0x040010AD RID: 4269
		public List<ShaderParameter> waterDepthShaderParameters;

		// Token: 0x040010AE RID: 4270
		public int renderPrecedence;

		// Token: 0x040010AF RID: 4271
		public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

		// Token: 0x040010B0 RID: 4272
		public bool layerable;

		// Token: 0x040010B1 RID: 4273
		[NoTranslate]
		public string scatterType;

		// Token: 0x040010B2 RID: 4274
		public bool takeFootprints;

		// Token: 0x040010B3 RID: 4275
		public bool natural;

		// Token: 0x040010B4 RID: 4276
		public bool takeSplashes;

		// Token: 0x040010B5 RID: 4277
		public bool avoidWander;

		// Token: 0x040010B6 RID: 4278
		public bool changeable = true;

		// Token: 0x040010B7 RID: 4279
		public TerrainDef smoothedTerrain;

		// Token: 0x040010B8 RID: 4280
		public TerrainDef gravshipReplacementTerrain;

		// Token: 0x040010B9 RID: 4281
		public bool holdSnowOrSand = true;

		// Token: 0x040010BA RID: 4282
		public bool isPaintable;

		// Token: 0x040010BB RID: 4283
		public bool extinguishesFire;

		// Token: 0x040010BC RID: 4284
		public Color color = Color.white;

		// Token: 0x040010BD RID: 4285
		public ColorDef colorDef;

		// Token: 0x040010BE RID: 4286
		public TerrainDef driesTo;

		// Token: 0x040010BF RID: 4287
		[NoTranslate]
		public List<string> tags;

		// Token: 0x040010C0 RID: 4288
		public TerrainDef burnedDef;

		// Token: 0x040010C1 RID: 4289
		public List<Tool> tools;

		// Token: 0x040010C2 RID: 4290
		public float extraDeteriorationFactor;

		// Token: 0x040010C3 RID: 4291
		public float destroyOnBombDamageThreshold = -1f;

		// Token: 0x040010C4 RID: 4292
		public bool destroyBuildingsOnDestroyed;

		// Token: 0x040010C5 RID: 4293
		public ThoughtDef traversedThought;

		// Token: 0x040010C6 RID: 4294
		public int extraDraftedPerceivedPathCost;

		// Token: 0x040010C7 RID: 4295
		public int extraNonDraftedPerceivedPathCost;

		// Token: 0x040010C8 RID: 4296
		public EffecterDef destroyEffect;

		// Token: 0x040010C9 RID: 4297
		public EffecterDef destroyEffectWater;

		// Token: 0x040010CA RID: 4298
		public bool autoRebuildable;

		// Token: 0x040010CB RID: 4299
		public TerrainDef.TerrainCategoryType categoryType;

		// Token: 0x040010CC RID: 4300
		public float meltSnowRadius;

		// Token: 0x040010CD RID: 4301
		public float heatPerTick;

		// Token: 0x040010CE RID: 4302
		public float igniteRadius;

		// Token: 0x040010CF RID: 4303
		public float ignitePawnsIntervalTicks;

		// Token: 0x040010D0 RID: 4304
		public int burnDamage;

		// Token: 0x040010D1 RID: 4305
		public float burnIntervalTicks;

		// Token: 0x040010D2 RID: 4306
		public bool supportsRock = true;

		// Token: 0x040010D3 RID: 4307
		public float toxicBuildupFactor;

		// Token: 0x040010D4 RID: 4308
		public bool dontRender;

		// Token: 0x040010D5 RID: 4309
		public bool exposesToVacuum;

		// Token: 0x040010D6 RID: 4310
		public bool canFreeze;

		// Token: 0x040010D7 RID: 4311
		public bool dangerous;

		// Token: 0x040010D8 RID: 4312
		public bool preventCraters;

		// Token: 0x040010D9 RID: 4313
		public bool canEverTerraform = true;

		// Token: 0x040010DA RID: 4314
		public WaterBodyType waterBodyType;

		// Token: 0x040010DB RID: 4315
		public bool cropIcon = true;

		// Token: 0x040010DC RID: 4316
		public float glowRadius;

		// Token: 0x040010DD RID: 4317
		public ColorInt glowColor;

		// Token: 0x040010DE RID: 4318
		public float throwFleckChance;

		// Token: 0x040010DF RID: 4319
		public TerrainFleckData fleckData;

		// Token: 0x040010E0 RID: 4320
		public bool isFoundation;

		// Token: 0x040010E1 RID: 4321
		public bool bridge;

		// Token: 0x040010E2 RID: 4322
		[NoTranslate]
		public string bridgePropsPath;

		// Token: 0x040010E3 RID: 4323
		[Unsaved(false)]
		public Graphic bridgePropsLoopGraphic = BaseContent.BadGraphic;

		// Token: 0x040010E4 RID: 4324
		[Unsaved(false)]
		public Graphic bridgePropsRightGraphic = BaseContent.BadGraphic;

		// Token: 0x040010E5 RID: 4325
		public bool temporary;

		// Token: 0x040010E6 RID: 4326
		public TempTerrainProps tempTerrain;

		// Token: 0x040010E7 RID: 4327
		public TerrainDef floodTerrain;

		// Token: 0x040010E8 RID: 4328
		public bool canBePolluted = true;

		// Token: 0x040010E9 RID: 4329
		[NoTranslate]
		public string pollutedTexturePath;

		// Token: 0x040010EA RID: 4330
		[NoTranslate]
		public string pollutionOverlayTexturePath;

		// Token: 0x040010EB RID: 4331
		public ShaderTypeDef pollutionShaderType;

		// Token: 0x040010EC RID: 4332
		public Color pollutionColor = Color.white;

		// Token: 0x040010ED RID: 4333
		public Vector2 pollutionOverlayScrollSpeed = Vector2.zero;

		// Token: 0x040010EE RID: 4334
		public Vector2 pollutionOverlayScale = Vector2.one;

		// Token: 0x040010EF RID: 4335
		public Color pollutionCloudColor = Color.white;

		// Token: 0x040010F0 RID: 4336
		public Color pollutionTintColor = Color.white;

		// Token: 0x040010F1 RID: 4337
		public ThingDef generatedFilth;

		// Token: 0x040010F2 RID: 4338
		public FilthSourceFlags filthAcceptanceMask = FilthSourceFlags.Any;

		// Token: 0x040010F3 RID: 4339
		public TerrainDef.SpaceEdgeGraphicData spaceEdgeGraphicData;

		// Token: 0x040010F4 RID: 4340
		[Unsaved(false)]
		public Material waterDepthMaterial;

		// Token: 0x040010F5 RID: 4341
		[Unsaved(false)]
		public Graphic graphicPolluted = BaseContent.BadGraphic;

		// Token: 0x020002EF RID: 751
		public enum TerrainEdgeType : byte
		{
			// Token: 0x040010F7 RID: 4343
			Hard,
			// Token: 0x040010F8 RID: 4344
			Fade,
			// Token: 0x040010F9 RID: 4345
			FadeRough,
			// Token: 0x040010FA RID: 4346
			Water
		}

		// Token: 0x020002F0 RID: 752
		public enum TerrainCategoryType : byte
		{
			// Token: 0x040010FC RID: 4348
			Misc,
			// Token: 0x040010FD RID: 4349
			Soil,
			// Token: 0x040010FE RID: 4350
			Stone,
			// Token: 0x040010FF RID: 4351
			Sand
		}

		// Token: 0x020002F1 RID: 753
		public class SpaceEdgeGraphicData
		{
			// Token: 0x17000351 RID: 849
			// (get) Token: 0x0600126D RID: 4717 RVA: 0x00055B9A File Offset: 0x00053D9A
			public Shader LoopShader
			{
				get
				{
					ShaderTypeDef shaderTypeDef = this.loopShader;
					return ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.TerrainEdge;
				}
			}

			// Token: 0x17000352 RID: 850
			// (get) Token: 0x0600126E RID: 4718 RVA: 0x00055BB7 File Offset: 0x00053DB7
			public Shader CornerShader
			{
				get
				{
					ShaderTypeDef shaderTypeDef = this.cornerShader;
					return ((shaderTypeDef != null) ? shaderTypeDef.Shader : null) ?? ShaderDatabase.Transparent;
				}
			}

			// Token: 0x0600126F RID: 4719 RVA: 0x00055BD4 File Offset: 0x00053DD4
			public void EnsureInitialized(TerrainDef def)
			{
				if (this.initialized)
				{
					return;
				}
				this.initialized = true;
				Shader shader = this.LoopShader;
				Shader shader2 = this.CornerShader;
				this.oShape = new CachedMaterial(this.OShapeTexPath, shader2, this.overrideColor ?? def.color);
				this.uShape = new CachedMaterial(this.UShapeTexPath, shader2, this.overrideColor ?? def.color);
				this.cornerInner = new CachedMaterial(this.CornerInnerTexPath, shader2, this.overrideColor ?? def.color);
				this.cornerOuter = new CachedMaterial(this.CornerOuterTexPath, shader2, this.overrideColor ?? def.color);
				this.flat = new CachedMaterial(this.FlatTexPath, shader2, this.overrideColor ?? def.color);
				this.loopLeft = new CachedMaterial(this.LoopLeftTexPath, shader, this.overrideColor ?? def.color);
				this.loopRight = new CachedMaterial(this.LoopRightTexPath, shader, this.overrideColor ?? def.color);
				this.loopSingle = new CachedMaterial(this.LoopSingleTexPath, shader, this.overrideColor ?? def.color);
				foreach (string texPath in this.LoopTexPaths)
				{
					this.loops.Add(new CachedMaterial(texPath, shader, this.overrideColor ?? def.color));
				}
			}

			// Token: 0x06001270 RID: 4720 RVA: 0x00055DFC File Offset: 0x00053FFC
			public Material GetMaterial(TerrainDef terrain, SectionLayer_TerrainEdges.EdgeType edgeType, int listIndexOffset = 0)
			{
				this.EnsureInitialized(terrain);
				switch (edgeType)
				{
				case SectionLayer_TerrainEdges.EdgeType.OShape:
					return this.oShape.Material;
				case SectionLayer_TerrainEdges.EdgeType.UShape:
					return this.uShape.Material;
				case SectionLayer_TerrainEdges.EdgeType.CornerInner:
					return this.cornerInner.Material;
				case SectionLayer_TerrainEdges.EdgeType.CornerOuter:
					return this.cornerOuter.Material;
				case SectionLayer_TerrainEdges.EdgeType.Flat:
					return this.flat.Material;
				case SectionLayer_TerrainEdges.EdgeType.LoopLeft:
					return this.loopLeft.Material;
				case SectionLayer_TerrainEdges.EdgeType.LoopRight:
					return this.loopRight.Material;
				case SectionLayer_TerrainEdges.EdgeType.LoopSingle:
					return this.loopSingle.Material;
				case SectionLayer_TerrainEdges.EdgeType.Loop:
					return this.loops[Mathf.Abs(listIndexOffset) % this.loops.Count].Material;
				default:
					throw new ArgumentOutOfRangeException("edgeType", edgeType, null);
				}
			}

			// Token: 0x04001100 RID: 4352
			[NoTranslate]
			public string OShapeTexPath;

			// Token: 0x04001101 RID: 4353
			[NoTranslate]
			public string UShapeTexPath;

			// Token: 0x04001102 RID: 4354
			[NoTranslate]
			public string CornerInnerTexPath;

			// Token: 0x04001103 RID: 4355
			[NoTranslate]
			public string CornerOuterTexPath;

			// Token: 0x04001104 RID: 4356
			[NoTranslate]
			public string FlatTexPath;

			// Token: 0x04001105 RID: 4357
			[NoTranslate]
			public string LoopLeftTexPath;

			// Token: 0x04001106 RID: 4358
			[NoTranslate]
			public string LoopRightTexPath;

			// Token: 0x04001107 RID: 4359
			[NoTranslate]
			public string LoopSingleTexPath;

			// Token: 0x04001108 RID: 4360
			[NoTranslate]
			public List<string> LoopTexPaths;

			// Token: 0x04001109 RID: 4361
			public Color? overrideColor;

			// Token: 0x0400110A RID: 4362
			private CachedMaterial oShape;

			// Token: 0x0400110B RID: 4363
			private CachedMaterial uShape;

			// Token: 0x0400110C RID: 4364
			private CachedMaterial cornerInner;

			// Token: 0x0400110D RID: 4365
			private CachedMaterial cornerOuter;

			// Token: 0x0400110E RID: 4366
			private CachedMaterial flat;

			// Token: 0x0400110F RID: 4367
			private CachedMaterial loopLeft;

			// Token: 0x04001110 RID: 4368
			private CachedMaterial loopRight;

			// Token: 0x04001111 RID: 4369
			private CachedMaterial loopSingle;

			// Token: 0x04001112 RID: 4370
			private ShaderTypeDef loopShader;

			// Token: 0x04001113 RID: 4371
			private ShaderTypeDef cornerShader;

			// Token: 0x04001114 RID: 4372
			private readonly List<CachedMaterial> loops = new List<CachedMaterial>();

			// Token: 0x04001115 RID: 4373
			private bool initialized;
		}
	}
}
