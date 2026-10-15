using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class TerrainDef : BuildableDef
{
	public enum TerrainEdgeType : byte
	{
		Hard,
		Fade,
		FadeRough,
		Water
	}

	public enum TerrainCategoryType : byte
	{
		Misc,
		Soil,
		Stone,
		Sand
	}

	public class SpaceEdgeGraphicData
	{
		[NoTranslate]
		public string OShapeTexPath;

		[NoTranslate]
		public string UShapeTexPath;

		[NoTranslate]
		public string CornerInnerTexPath;

		[NoTranslate]
		public string CornerOuterTexPath;

		[NoTranslate]
		public string FlatTexPath;

		[NoTranslate]
		public string LoopLeftTexPath;

		[NoTranslate]
		public string LoopRightTexPath;

		[NoTranslate]
		public string LoopSingleTexPath;

		[NoTranslate]
		public List<string> LoopTexPaths;

		public Color? overrideColor;

		private CachedMaterial oShape;

		private CachedMaterial uShape;

		private CachedMaterial cornerInner;

		private CachedMaterial cornerOuter;

		private CachedMaterial flat;

		private CachedMaterial loopLeft;

		private CachedMaterial loopRight;

		private CachedMaterial loopSingle;

		private ShaderTypeDef loopShader;

		private ShaderTypeDef cornerShader;

		private readonly List<CachedMaterial> loops = new List<CachedMaterial>();

		private bool initialized;

		public Shader LoopShader => loopShader?.Shader ?? ShaderDatabase.TerrainEdge;

		public Shader CornerShader => cornerShader?.Shader ?? ShaderDatabase.Transparent;

		public void EnsureInitialized(TerrainDef def)
		{
			if (initialized)
			{
				return;
			}
			initialized = true;
			Shader shader = LoopShader;
			Shader shader2 = CornerShader;
			oShape = new CachedMaterial(OShapeTexPath, shader2, overrideColor ?? def.color);
			uShape = new CachedMaterial(UShapeTexPath, shader2, overrideColor ?? def.color);
			cornerInner = new CachedMaterial(CornerInnerTexPath, shader2, overrideColor ?? def.color);
			cornerOuter = new CachedMaterial(CornerOuterTexPath, shader2, overrideColor ?? def.color);
			flat = new CachedMaterial(FlatTexPath, shader2, overrideColor ?? def.color);
			loopLeft = new CachedMaterial(LoopLeftTexPath, shader, overrideColor ?? def.color);
			loopRight = new CachedMaterial(LoopRightTexPath, shader, overrideColor ?? def.color);
			loopSingle = new CachedMaterial(LoopSingleTexPath, shader, overrideColor ?? def.color);
			foreach (string loopTexPath in LoopTexPaths)
			{
				loops.Add(new CachedMaterial(loopTexPath, shader, overrideColor ?? def.color));
			}
		}

		public Material GetMaterial(TerrainDef terrain, SectionLayer_TerrainEdges.EdgeType edgeType, int listIndexOffset = 0)
		{
			EnsureInitialized(terrain);
			return edgeType switch
			{
				SectionLayer_TerrainEdges.EdgeType.OShape => oShape.Material, 
				SectionLayer_TerrainEdges.EdgeType.UShape => uShape.Material, 
				SectionLayer_TerrainEdges.EdgeType.CornerInner => cornerInner.Material, 
				SectionLayer_TerrainEdges.EdgeType.CornerOuter => cornerOuter.Material, 
				SectionLayer_TerrainEdges.EdgeType.Flat => flat.Material, 
				SectionLayer_TerrainEdges.EdgeType.LoopLeft => loopLeft.Material, 
				SectionLayer_TerrainEdges.EdgeType.LoopRight => loopRight.Material, 
				SectionLayer_TerrainEdges.EdgeType.LoopSingle => loopSingle.Material, 
				SectionLayer_TerrainEdges.EdgeType.Loop => loops[Mathf.Abs(listIndexOffset) % loops.Count].Material, 
				_ => throw new ArgumentOutOfRangeException("edgeType", edgeType, null), 
			};
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__120 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TerrainDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CConfigErrors_003Ed__120(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				TerrainDef terrainDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((BuildableDef)terrainDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_008a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_008a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00d9;
				case 3:
					_003C_003E1__state = -1;
					goto IL_011d;
				case 4:
					_003C_003E1__state = -1;
					goto IL_015f;
				case 5:
					_003C_003E1__state = -1;
					goto IL_019d;
				case 6:
					_003C_003E1__state = -1;
					goto IL_01d2;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0204;
				case 8:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_008a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (terrainDef.texturePath.NullOrEmpty() && !terrainDef.dontRender)
					{
						_003C_003E2__current = "missing texturePath";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d9;
					IL_01d2:
					if (terrainDef.burnedDef != null && terrainDef.burnedDef.Flammable())
					{
						_003C_003E2__current = "burnedDef is flammable";
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0204;
					IL_015f:
					if (terrainDef.generatedFilth != null && (terrainDef.filthAcceptanceMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None)
					{
						_003C_003E2__current = terrainDef.defName + " makes terrain filth and also accepts it.";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_019d;
					IL_00d9:
					if (terrainDef.fertility < 0f)
					{
						_003C_003E2__current = "Terrain Def " + terrainDef?.ToString() + " has no fertility value set.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_011d;
					IL_0204:
					if (terrainDef.throwFleckChance > 0f && terrainDef.fleckData.fleck == null)
					{
						_003C_003E2__current = "throwFleckChance is > 0 but fleckData.fleck is null";
						_003C_003E1__state = 8;
						return true;
					}
					break;
					IL_019d:
					if (terrainDef.Flammable() && terrainDef.burnedDef == null && !terrainDef.layerable)
					{
						_003C_003E2__current = "flammable but burnedDef is null and not layerable";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_01d2;
					IL_011d:
					if (terrainDef.renderPrecedence > 400)
					{
						_003C_003E2__current = "Render order " + terrainDef.renderPrecedence + " is out of range (must be < 400)";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_015f;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__120 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__120(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__123 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TerrainDef _003C_003E4__this;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSpecialDisplayStats_003Ed__123(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				TerrainDef terrainDef = _003C_003E4__this;
				string[] array;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((BuildableDef)terrainDef).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_014c;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					array = (from ta in terrainDef.affordances.Distinct()
						orderby ta.order
						select ta.label).ToArray();
					if (array.Length != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Terrain, "Supports".Translate(), array.ToCommaList().CapitalizeFirst(), "Stat_Thing_Terrain_Supports_Desc".Translate(), 2000);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_014c;
					IL_014c:
					if (terrainDef.IsFine && ModsConfig.RoyaltyActive)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Terrain, "Stat_Thing_Terrain_Fine_Name".Translate(), "Stat_Thing_Terrain_Fine_Value".Translate(), "Stat_Thing_Terrain_Fine_Desc".Translate(), 2000);
						_003C_003E1__state = 3;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__123 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__123(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.req = _003C_003E3__req;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[NoTranslate]
	public string texturePath;

	public TerrainEdgeType edgeType;

	public ShaderTypeDef customShader;

	public List<ShaderParameter> customShaderParameters;

	[NoTranslate]
	public string waterDepthShader;

	public List<ShaderParameter> waterDepthShaderParameters;

	public int renderPrecedence;

	public List<TerrainAffordanceDef> affordances = new List<TerrainAffordanceDef>();

	public bool layerable;

	[NoTranslate]
	public string scatterType;

	public bool takeFootprints;

	public bool natural;

	public bool takeSplashes;

	public bool avoidWander;

	public bool changeable = true;

	public TerrainDef smoothedTerrain;

	public TerrainDef gravshipReplacementTerrain;

	public bool holdSnowOrSand = true;

	public bool isPaintable;

	public bool extinguishesFire;

	public Color color = Color.white;

	public ColorDef colorDef;

	public TerrainDef driesTo;

	[NoTranslate]
	public List<string> tags;

	public TerrainDef burnedDef;

	public List<Tool> tools;

	public float extraDeteriorationFactor;

	public float destroyOnBombDamageThreshold = -1f;

	public bool destroyBuildingsOnDestroyed;

	public ThoughtDef traversedThought;

	public int extraDraftedPerceivedPathCost;

	public int extraNonDraftedPerceivedPathCost;

	public EffecterDef destroyEffect;

	public EffecterDef destroyEffectWater;

	public bool autoRebuildable;

	public TerrainCategoryType categoryType;

	public float meltSnowRadius;

	public float heatPerTick;

	public float igniteRadius;

	public float ignitePawnsIntervalTicks;

	public int burnDamage;

	public float burnIntervalTicks;

	public bool supportsRock = true;

	public float toxicBuildupFactor;

	public bool dontRender;

	public bool exposesToVacuum;

	public bool canFreeze;

	public bool dangerous;

	public bool preventCraters;

	public bool canEverTerraform = true;

	public WaterBodyType waterBodyType;

	public bool cropIcon = true;

	public float glowRadius;

	public ColorInt glowColor;

	public float throwFleckChance;

	public TerrainFleckData fleckData;

	public bool isFoundation;

	public bool bridge;

	[NoTranslate]
	public string bridgePropsPath;

	[NoTranslate]
	public string spaceBridgePropsPath;

	[Unsaved(false)]
	public Graphic bridgePropsLoopGraphic = BaseContent.BadGraphic;

	[Unsaved(false)]
	public Graphic bridgePropsRightGraphic = BaseContent.BadGraphic;

	[Unsaved(false)]
	public Graphic spaceBridgePropsLoopGraphic = BaseContent.BadGraphic;

	[Unsaved(false)]
	public Graphic spaceBridgePropsRightGraphic = BaseContent.BadGraphic;

	public bool temporary;

	public TempTerrainProps tempTerrain;

	public TerrainDef floodTerrain;

	public bool canBePolluted = true;

	[NoTranslate]
	public string pollutedTexturePath;

	[NoTranslate]
	public string pollutionOverlayTexturePath;

	public ShaderTypeDef pollutionShaderType;

	public Color pollutionColor = Color.white;

	public Vector2 pollutionOverlayScrollSpeed = Vector2.zero;

	public Vector2 pollutionOverlayScale = Vector2.one;

	public Color pollutionCloudColor = Color.white;

	public Color pollutionTintColor = Color.white;

	public ThingDef generatedFilth;

	public FilthSourceFlags filthAcceptanceMask = FilthSourceFlags.Any;

	public SpaceEdgeGraphicData spaceEdgeGraphicData;

	[Unsaved(false)]
	public Material waterDepthMaterial;

	[Unsaved(false)]
	public Graphic graphicPolluted = BaseContent.BadGraphic;

	public bool Removable => layerable;

	public bool IsCarpet => researchPrerequisites.NotNullAndContains(ResearchProjectDefOf.CarpetMaking);

	public Color DrawColor
	{
		get
		{
			if (colorDef != null)
			{
				return colorDef.color;
			}
			return color;
		}
	}

	public bool IsRiver => HasTag("River");

	public bool IsOcean => HasTag("Ocean");

	public bool IsWater => HasTag("Water");

	public bool IsIce => HasTag("Ice");

	public bool IsFlood => HasTag("Flood");

	public bool IsFine => HasTag("FineFloor");

	public bool IsSoil => HasTag("Soil");

	public bool IsRoad => HasTag("Road");

	public bool IsFloor => HasTag("Floor");

	public bool IsRock => HasTag("NaturalRock");

	public bool IsSubstructure
	{
		get
		{
			if (ModsConfig.OdysseyActive)
			{
				return HasTag("Substructure");
			}
			return false;
		}
	}

	public Shader Shader
	{
		get
		{
			if (customShader != null)
			{
				return customShader.Shader;
			}
			Shader result = null;
			switch (edgeType)
			{
			case TerrainEdgeType.Hard:
				result = ShaderDatabase.TerrainHard;
				break;
			case TerrainEdgeType.Fade:
				result = ShaderDatabase.TerrainFade;
				break;
			case TerrainEdgeType.FadeRough:
				result = ShaderDatabase.TerrainFadeRough;
				break;
			case TerrainEdgeType.Water:
				result = ShaderDatabase.TerrainWater;
				break;
			}
			return result;
		}
	}

	private Shader ShaderPolluted
	{
		get
		{
			if (pollutionShaderType != null)
			{
				return pollutionShaderType.Shader;
			}
			if (customShader != null)
			{
				return customShader.Shader;
			}
			Shader result = null;
			switch (edgeType)
			{
			case TerrainEdgeType.Hard:
				result = ShaderDatabase.TerrainHardPolluted;
				break;
			case TerrainEdgeType.Fade:
				result = ShaderDatabase.TerrainFadePolluted;
				break;
			case TerrainEdgeType.FadeRough:
				result = ShaderDatabase.TerrainFadeRoughPolluted;
				break;
			}
			return result;
		}
	}

	public Material DrawMatPolluted
	{
		get
		{
			if (graphicPolluted == BaseContent.BadGraphic)
			{
				return graphic.MatSingle;
			}
			return graphicPolluted.MatSingle;
		}
	}

	public override void PostLoad()
	{
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			if (drawStyleCategory == null)
			{
				drawStyleCategory = DrawStyleCategoryDefOf.Floors;
			}
			if (!dontRender)
			{
				if (graphic == BaseContent.BadGraphic)
				{
					graphic = GraphicDatabase.Get<Graphic_Terrain>(texturePath, Shader, Vector2.one, DrawColor, 2000 + renderPrecedence);
					if (edgeType == TerrainEdgeType.FadeRough || edgeType == TerrainEdgeType.Water)
					{
						graphic.MatSingle.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					}
					if (customShader != null && customShaderParameters != null)
					{
						for (int i = 0; i < customShaderParameters.Count; i++)
						{
							customShaderParameters[i].Apply(graphic.MatSingle);
						}
					}
				}
				if (!waterDepthShader.NullOrEmpty())
				{
					waterDepthMaterial = MaterialAllocator.Create(ShaderDatabase.LoadShader(waterDepthShader));
					waterDepthMaterial.renderQueue = 2000 + renderPrecedence;
					waterDepthMaterial.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					if (waterDepthShaderParameters != null)
					{
						for (int j = 0; j < waterDepthShaderParameters.Count; j++)
						{
							waterDepthShaderParameters[j].Apply(waterDepthMaterial);
						}
					}
				}
				if (ModsConfig.BiotechActive && graphicPolluted == BaseContent.BadGraphic && (!pollutionOverlayTexturePath.NullOrEmpty() || !pollutedTexturePath.NullOrEmpty()))
				{
					Texture2D texture2D = null;
					if (!pollutionOverlayTexturePath.NullOrEmpty())
					{
						texture2D = ContentFinder<Texture2D>.Get(pollutionOverlayTexturePath);
					}
					graphicPolluted = GraphicDatabase.Get<Graphic_Terrain>(pollutedTexturePath ?? texturePath, ShaderPolluted, Vector2.one, DrawColor, 2000 + renderPrecedence);
					Material matSingle = graphicPolluted.MatSingle;
					if (texture2D != null)
					{
						matSingle.SetTexture(ShaderPropertyIDs.BurnTex, texture2D);
					}
					matSingle.SetColor(ShaderPropertyIDs.BurnColor, pollutionColor);
					matSingle.SetVector(ShaderPropertyIDs.BurnScale, pollutionOverlayScale);
					matSingle.SetVector(ShaderPropertyIDs.ScrollSpeed, pollutionOverlayScrollSpeed);
					matSingle.SetColor(ShaderPropertyIDs.PollutionTintColor, pollutionTintColor);
					if (edgeType == TerrainEdgeType.FadeRough)
					{
						matSingle.SetTexture(ShaderPropertyIDs.AlphaAddTex, TexGame.AlphaAddTex);
					}
					if (matSingle != graphic.MatSingle)
					{
						matSingle.SetFloat(ShaderPropertyIDs.IsPolluted, 1f);
					}
					if ((pollutionShaderType != null || customShader != null) && customShaderParameters != null)
					{
						for (int k = 0; k < customShaderParameters.Count; k++)
						{
							customShaderParameters[k].Apply(matSingle);
						}
					}
				}
				if (!bridgePropsPath.NullOrEmpty() && bridgePropsLoopGraphic == BaseContent.BadGraphic)
				{
					bridgePropsLoopGraphic = GraphicDatabase.Get<Graphic_Terrain>(bridgePropsPath + "_Loop", ShaderDatabase.Transparent);
					bridgePropsRightGraphic = GraphicDatabase.Get<Graphic_Terrain>(bridgePropsPath + "_Right", ShaderDatabase.Transparent);
				}
				if (ModsConfig.OdysseyActive && !spaceBridgePropsPath.NullOrEmpty() && spaceBridgePropsLoopGraphic == BaseContent.BadGraphic)
				{
					spaceBridgePropsLoopGraphic = GraphicDatabase.Get<Graphic_Terrain>(spaceBridgePropsPath + "_Loop", ShaderDatabase.Transparent);
					spaceBridgePropsRightGraphic = GraphicDatabase.Get<Graphic_Terrain>(spaceBridgePropsPath + "_Right", ShaderDatabase.Transparent);
				}
			}
		});
		if (tools != null)
		{
			for (int l = 0; l < tools.Count; l++)
			{
				tools[l].id = l.ToString();
			}
		}
		base.PostLoad();
	}

	protected override void ResolveIcon()
	{
		base.ResolveIcon();
		uiIconColor = DrawColor;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__120))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__120(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static TerrainDef Named(string defName)
	{
		return DefDatabase<TerrainDef>.GetNamed(defName);
	}

	public bool HasTag(string tag)
	{
		if (tags != null)
		{
			return tags.Contains(tag);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__123))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__123(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}
}
