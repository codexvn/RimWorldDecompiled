using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class PlantProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__112 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PlantProperties _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__112(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			PlantProperties plantProperties = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (plantProperties.maxMeshCount > 25)
				{
					_003C_003E2__current = "maxMeshCount > MaxMaxMeshCount";
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__112 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__112(0)
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
	private sealed class _003CGetHarvestYieldHyperlinks_003Ed__113 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PlantProperties _003C_003E4__this;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003CGetHarvestYieldHyperlinks_003Ed__113(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			PlantProperties plantProperties = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Dialog_InfoCard.Hyperlink(plantProperties.harvestedThingDef);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003CGetHarvestYieldHyperlinks_003Ed__113 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetHarvestYieldHyperlinks_003Ed__113(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__114 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PlantProperties _003C_003E4__this;

		private string _003Cattributes_003E5__2;

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
		public _003CSpecialDisplayStats_003Ed__114(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cattributes_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			PlantProperties plantProperties = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (plantProperties.sowMinSkill > 0)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowingSkillToSow".Translate(), plantProperties.sowMinSkill.ToString(), "Stat_Thing_Plant_MinGrowingSkillToSow_Desc".Translate(), 4151);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00a4;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00a4;
			case 2:
				_003C_003E1__state = -1;
				if (!plantProperties.completelyIgnoreFertility)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "FertilityRequirement".Translate(), plantProperties.fertilityMin.ToStringPercent(), "Stat_Thing_Plant_FertilityRequirement_Desc".Translate(), 4156);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0286;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "FertilitySensitivity".Translate(), plantProperties.fertilitySensitivity.ToStringPercent(), "Stat_Thing_Plant_FertilitySensitivity_Desc".Translate(), 4155);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0286;
			case 5:
				_003C_003E1__state = -1;
				goto IL_02d8;
			case 6:
				_003C_003E1__state = -1;
				goto IL_0332;
			case 7:
				_003C_003E1__state = -1;
				goto IL_03b3;
			case 8:
				_003C_003E1__state = -1;
				goto IL_04a6;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "MaxGrowthTemperature".Translate(), plantProperties.maxGrowthTemperature.ToStringTemperature(), "Stat_Thing_Plant_MaxGrowthTemperature_Desc".Translate(), 4153);
				_003C_003E1__state = 10;
				return true;
			case 10:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_04a6:
				if (!plantProperties.isStump)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowthTemperature".Translate(), plantProperties.minGrowthTemperature.ToStringTemperature(), "Stat_Thing_Plant_MinGrowthTemperature_Desc".Translate(), 4152);
					_003C_003E1__state = 9;
					return true;
				}
				break;
				IL_0286:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "LightRequirement".Translate(), plantProperties.growMinGlow.ToStringPercent(), "Stat_Thing_Plant_LightRequirement_Desc".Translate(), 4154);
				_003C_003E1__state = 5;
				return true;
				IL_00a4:
				_003Cattributes_003E5__2 = "";
				if (plantProperties.Harvestable)
				{
					string text = "Harvestable".Translate();
					if (!_003Cattributes_003E5__2.NullOrEmpty())
					{
						_003Cattributes_003E5__2 += ", ";
						text = text.UncapitalizeFirst();
					}
					_003Cattributes_003E5__2 += text;
				}
				if (plantProperties.LimitedLifespan)
				{
					string text2 = "LimitedLifespan".Translate();
					if (!_003Cattributes_003E5__2.NullOrEmpty())
					{
						_003Cattributes_003E5__2 += ", ";
						text2 = text2.UncapitalizeFirst();
					}
					_003Cattributes_003E5__2 += text2;
				}
				if (!plantProperties.isStump)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "GrowingTime".Translate(), plantProperties.growDays.ToString("0.##") + " " + "Days".Translate(), "GrowingTimeDesc".Translate(), 4158);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_02d8;
				IL_0332:
				if (plantProperties.LimitedLifespan)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "LifeSpan".Translate(), plantProperties.LifespanDays.ToString("0.##") + " " + "Days".Translate(), "Stat_Thing_Plant_LifeSpan_Desc".Translate(), 4150);
					_003C_003E1__state = 7;
					return true;
				}
				goto IL_03b3;
				IL_03b3:
				if (plantProperties.harvestYield > 0f)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Stat_Thing_Plant_HarvestYield_Desc".Translate());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.cropYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "HarvestYield".Translate(), Mathf.CeilToInt(plantProperties.harvestYield * Find.Storyteller.difficulty.cropYieldFactor).ToString("F0"), stringBuilder.ToString(), 4150, null, plantProperties.GetHarvestYieldHyperlinks());
					_003C_003E1__state = 8;
					return true;
				}
				goto IL_04a6;
				IL_02d8:
				if (!_003Cattributes_003E5__2.NullOrEmpty())
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Attributes".Translate(), _003Cattributes_003E5__2, "Stat_Thing_Plant_Attributes_Desc".Translate(), 4157);
					_003C_003E1__state = 6;
					return true;
				}
				goto IL_0332;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__114 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__114(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public List<PlantBiomeRecord> wildBiomes;

	public int wildClusterRadius = -1;

	public float wildClusterWeight = 15f;

	public float wildOrder = 2f;

	public bool wildEqualLocalDistribution = true;

	public bool cavePlant;

	public float cavePlantWeight = 1f;

	public List<string> wildTerrainTags;

	public bool wildPlantUseDistanceToShore;

	public float plantRespawningCommonalityFactor = 1f;

	[NoTranslate]
	public List<string> sowTags = new List<string>();

	public float sowWork = 10f;

	public int sowMinSkill;

	public bool blockAdjacentSow;

	public List<ResearchProjectDef> sowResearchPrerequisites;

	public bool mustBeWildToSow;

	public bool mustBePermanentDarknessToSow;

	public float harvestWork = 10f;

	public float harvestYield;

	public ThingDef harvestedThingDef;

	[NoTranslate]
	public string harvestTag;

	public float harvestMinGrowth = 0.65f;

	public float harvestAfterGrowth;

	public bool harvestFailable = true;

	public bool harvestYieldAffectedByDifficulty = true;

	public SoundDef soundHarvesting;

	public SoundDef soundHarvestFinish;

	public bool autoHarvestable = true;

	public bool skipDeteriorationMessage;

	public float growDays = 2f;

	public float lifespanDaysPerGrowDays = 8f;

	public float growMinGlow = 0.51f;

	public float growOptimalGlow = 1f;

	public bool diesToLight;

	public bool vacuumResistant;

	public bool terraformable;

	public Pollution pollution;

	public float minGrowthTemperature;

	public float minOptimalGrowthTemperature = 6f;

	public float maxOptimalGrowthTemperature = 42f;

	public float maxGrowthTemperature = 58f;

	public float fertilityMin = 0.9f;

	public float fertilitySensitivity = 0.5f;

	public bool completelyIgnoreFertility;

	public List<TerrainDef> terrainBlacklist;

	public bool dieIfLeafless;

	public bool neverBlightable;

	public bool interferesWithRoof;

	public bool dieIfNoSunlight = true;

	public bool dieFromToxicFallout = true;

	public PlantPurpose purpose = PlantPurpose.Misc;

	public bool humanFoodPlant;

	public bool treeLoversCareIfChopped = true;

	public bool allowAutoCut = true;

	public bool drugForHarvestPurposes;

	public TreeCategory treeCategory;

	public ThingDef burnedThingDef;

	public ThingDef choppedThingDef;

	public ThingDef smashedThingDef;

	public bool canDeteriorate;

	public bool showGrowthInInspectPane = true;

	public float minSpacingBetweenSamePlant;

	public bool forceIsTree;

	public bool warnIfMarkedForCut;

	public bool isStump;

	public float topWindExposure = 0.25f;

	public int maxMeshCount = 1;

	public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

	public bool showInFrozenWater = true;

	public bool destroyedByFlooding = true;

	[NoTranslate]
	private string leaflessGraphicPath;

	[Unsaved(false)]
	public Graphic leaflessGraphic;

	[NoTranslate]
	private string immatureGraphicPath;

	[Unsaved(false)]
	public Graphic immatureGraphic;

	public bool dropLeaves;

	[NoTranslate]
	private string pollutedGraphicPath;

	[Unsaved(false)]
	public Graphic pollutedGraphic;

	[NoTranslate]
	private string leaflessImmatureGraphicPath;

	[Unsaved(false)]
	public Graphic leaflessImmatureGraphic;

	[NoTranslate]
	private string snowOverlayGraphicPath;

	[Unsaved(false)]
	public Graphic snowOverlayGraphic;

	[NoTranslate]
	private string leaflessSnowOverlayGraphicPath;

	[Unsaved(false)]
	public Graphic leaflessSnowOverlayGraphic;

	[NoTranslate]
	private string immatureSnowOverlayGraphicPath;

	[Unsaved(false)]
	public Graphic immatureSnowOverlayGraphic;

	public const int MaxMaxMeshCount = 25;

	private HashSet<string> wildTerrainTagsSet;

	public bool Sowable => !sowTags.NullOrEmpty();

	public bool Harvestable => harvestYield > 0.001f;

	public bool HarvestDestroys => harvestAfterGrowth <= 0f;

	public bool IsTree
	{
		get
		{
			if (!(harvestTag == "Wood"))
			{
				return forceIsTree;
			}
			return true;
		}
	}

	public float LifespanDays => growDays * lifespanDaysPerGrowDays;

	public int LifespanTicks => (int)(LifespanDays * 60000f);

	public bool LimitedLifespan => lifespanDaysPerGrowDays > 0f;

	public bool Blightable
	{
		get
		{
			if (Sowable && Harvestable)
			{
				return !neverBlightable;
			}
			return false;
		}
	}

	public bool GrowsInClusters => wildClusterRadius > 0;

	public bool RequiresPollution => pollution == Pollution.PollutedOnly;

	public bool RequiresNoPollution => pollution == Pollution.CleanOnly;

	public HashSet<string> WildTerrainTags
	{
		get
		{
			HashSet<string> hashSet = wildTerrainTagsSet;
			if (hashSet == null)
			{
				IEnumerable<string> enumerable = wildTerrainTags;
				hashSet = (wildTerrainTagsSet = new HashSet<string>(enumerable ?? Enumerable.Empty<string>()));
			}
			return hashSet;
		}
	}

	public void PostLoadSpecial(ThingDef parentDef)
	{
		if (!leaflessGraphicPath.NullOrEmpty())
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				leaflessGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, leaflessGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
			});
		}
		if (!immatureGraphicPath.NullOrEmpty())
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				immatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, immatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
			});
		}
		if (ModsConfig.BiotechActive && !pollutedGraphicPath.NullOrEmpty())
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				pollutedGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, pollutedGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
			});
		}
		if (!leaflessImmatureGraphicPath.NullOrEmpty())
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				leaflessImmatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, leaflessImmatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
			});
		}
		if (!snowOverlayGraphicPath.NullOrEmpty())
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				snowOverlayGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, snowOverlayGraphicPath, ShaderDatabase.TransparentPlant, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
			});
		}
		if (!leaflessSnowOverlayGraphicPath.NullOrEmpty())
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				leaflessSnowOverlayGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, leaflessSnowOverlayGraphicPath, ShaderDatabase.TransparentPlant, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
			});
		}
		if (!immatureSnowOverlayGraphicPath.NullOrEmpty())
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				immatureSnowOverlayGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, immatureSnowOverlayGraphicPath, ShaderDatabase.TransparentPlant, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
			});
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__112))]
	public IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__112(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetHarvestYieldHyperlinks_003Ed__113))]
	private IEnumerable<Dialog_InfoCard.Hyperlink> GetHarvestYieldHyperlinks()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetHarvestYieldHyperlinks_003Ed__113(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__114))]
	internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__114(-2)
		{
			_003C_003E4__this = this
		};
	}
}
