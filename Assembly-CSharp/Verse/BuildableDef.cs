using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public abstract class BuildableDef : Def, IEquatable<BuildableDef>
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass85_0
	{
		public ThingDefCountClass c;

		internal bool _003CSpecialDisplayStats_003Eb__5(Dialog_InfoCard.Hyperlink x)
		{
			return x.def == c.thingDef;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__82 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BuildableDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__82(int _003C_003E1__state)
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
				BuildableDef buildableDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)buildableDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00bd;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (buildableDef.useStuffTerrainAffordance && !buildableDef.MadeFromStuff)
					{
						_003C_003E2__current = "useStuffTerrainAffordance is true but it's not made from stuff";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00bd;
					IL_00bd:
					if (buildableDef.costListForDifficulty != null && buildableDef.costListForDifficulty.difficultyVar.NullOrEmpty())
					{
						_003C_003E2__current = "costListForDifficulty is not referencing a difficulty.";
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__82 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__82(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__85 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BuildableDef _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__85(int _003C_003E1__state)
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
				BuildableDef CS_0024_003C_003E8__locals11 = _003C_003E4__this;
				IEnumerable<TerrainAffordanceDef> enumerable;
				TerrainAffordanceDef terrainAffordanceNeed;
				string[] array;
				List<ThingDefCountClass> costList;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)CS_0024_003C_003E8__locals11).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_01ba;
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
					if (!CS_0024_003C_003E8__locals11.BuildableByPlayer)
					{
						break;
					}
					enumerable = Enumerable.Empty<TerrainAffordanceDef>();
					if (CS_0024_003C_003E8__locals11.PlaceWorkers != null)
					{
						enumerable = enumerable.Concat(CS_0024_003C_003E8__locals11.PlaceWorkers.SelectMany((PlaceWorker pw) => pw.DisplayAffordances()));
					}
					terrainAffordanceNeed = CS_0024_003C_003E8__locals11.GetTerrainAffordanceNeed(req.StuffDef);
					if (terrainAffordanceNeed != null)
					{
						enumerable = enumerable.Concat(terrainAffordanceNeed);
					}
					array = (from ta in enumerable.Distinct()
						orderby ta.order
						select ta.label).ToArray();
					if (array.Length != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "TerrainRequirement".Translate(), array.ToCommaList().CapitalizeFirst(), "Stat_Thing_TerrainRequirement_Desc".Translate(), 1101);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_01ba;
					IL_01ba:
					tmpCostList.Clear();
					tmpHyperlinks.Clear();
					if (CS_0024_003C_003E8__locals11.MadeFromStuff && CS_0024_003C_003E8__locals11.costStuffCount > 0)
					{
						tmpCostList.Add(CS_0024_003C_003E8__locals11.costStuffCount + "x " + CS_0024_003C_003E8__locals11.stuffCategories.Select((StuffCategoryDef x) => x.label).ToCommaListOr());
						foreach (ThingDef allDef in DefDatabase<ThingDef>.AllDefs)
						{
							if (allDef.IsStuff && !allDef.stuffProps.categories.NullOrEmpty() && allDef.stuffProps.categories.Any((StuffCategoryDef x) => CS_0024_003C_003E8__locals11.stuffCategories.Contains(x)))
							{
								tmpHyperlinks.Add(new Dialog_InfoCard.Hyperlink(allDef));
							}
						}
					}
					costList = CS_0024_003C_003E8__locals11.CostList;
					if (!costList.NullOrEmpty())
					{
						using List<ThingDefCountClass>.Enumerator enumerator2 = costList.GetEnumerator();
						while (enumerator2.MoveNext())
						{
							_003C_003Ec__DisplayClass85_0 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass85_0
							{
								c = enumerator2.Current
							};
							tmpCostList.Add(CS_0024_003C_003E8__locals15.c.Summary);
							if (!tmpHyperlinks.Any((Dialog_InfoCard.Hyperlink x) => x.def == CS_0024_003C_003E8__locals15.c.thingDef))
							{
								tmpHyperlinks.Add(new Dialog_InfoCard.Hyperlink(CS_0024_003C_003E8__locals15.c.thingDef));
							}
						}
					}
					if (tmpCostList.Any())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "Stat_Building_ResourcesToMake".Translate(), tmpCostList.ToCommaList().CapitalizeFirst(), "Stat_Building_ResourcesToMakeDesc".Translate(), 4405, null, tmpHyperlinks);
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
			_003CSpecialDisplayStats_003Ed__85 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__85(0)
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

	public List<StatModifier> statBases;

	public Traversability passability;

	public int pathCost;

	public bool pathCostIgnoreRepeat = true;

	public float fertility = -1f;

	public List<ThingDefCountClass> costList;

	public int costStuffCount;

	public List<StuffCategoryDef> stuffCategories;

	[MustTranslate]
	public string stuffCategorySummary;

	public CostListForDifficulty costListForDifficulty;

	public DrawStyleCategoryDef drawStyleCategory;

	public bool clearBuildingArea = true;

	public Rot4 defaultPlacingRot = Rot4.North;

	public float resourcesFractionWhenDeconstructed = 0.5f;

	public List<AltitudeLayer> blocksAltitudes;

	public StyleCategoryDef dominantStyleCategory;

	public bool forcePassableByFlyingPawns;

	public bool forceMoveItemsBeforeConstruction;

	public bool isAltar;

	public bool useStuffTerrainAffordance;

	public TerrainAffordanceDef terrainAffordanceNeeded;

	public List<ThingDef> buildingPrerequisites;

	public List<ThingDef> discoveryPrerequisites;

	public List<ResearchProjectDef> researchPrerequisites;

	public int minMonolithLevel;

	public int constructionSkillPrerequisite;

	public int artisticSkillPrerequisite;

	public TechLevel minTechLevelToBuild;

	public TechLevel maxTechLevelToBuild;

	public bool requireInspectedGravEngine;

	public AltitudeLayer altitudeLayer = AltitudeLayer.Item;

	public EffecterDef repairEffect;

	public EffecterDef constructEffect;

	public List<ColorForStuff> colorPerStuff;

	public bool canGenerateDefaultDesignator = true;

	public bool ideoBuilding;

	public float specialDisplayRadius;

	public List<Type> placeWorkers;

	public DesignationCategoryDef designationCategory;

	public DesignatorDropdownGroupDef designatorDropdown;

	public KeyBindingDef designationHotKey;

	public float uiOrder = 2999f;

	[NoTranslate]
	public string uiIconPath;

	public List<IconForStuffAppearance> uiIconPathsStuff;

	public Vector2 uiIconOffset;

	public Color uiIconColor = Color.white;

	public Color uiIconColorTwo = Color.white;

	public int uiIconForStackCount = -1;

	[Unsaved(false)]
	public ThingDef blueprintDef;

	[Unsaved(false)]
	public ThingDef installBlueprintDef;

	[Unsaved(false)]
	public ThingDef frameDef;

	[Unsaved(false)]
	private List<PlaceWorker> placeWorkersInstantiatedInt;

	[Unsaved(false)]
	public Graphic graphic = BaseContent.BadGraphic;

	[Unsaved(false)]
	public Texture2D uiIcon = BaseContent.BadTex;

	[Unsaved(false)]
	public Material uiIconMaterial;

	[Unsaved(false)]
	public Dictionary<StuffAppearanceDef, Texture2D> stuffUiIcons;

	[Unsaved(false)]
	public float uiIconAngle;

	protected static List<string> tmpCostList = new List<string>();

	protected static List<Dialog_InfoCard.Hyperlink> tmpHyperlinks = new List<Dialog_InfoCard.Hyperlink>();

	public virtual IntVec2 Size => IntVec2.One;

	public bool MadeFromStuff => !stuffCategories.NullOrEmpty();

	public bool BuildableByPlayer => designationCategory != null;

	public Material DrawMatSingle => graphic?.MatSingle;

	public float Altitude => altitudeLayer.AltitudeFor();

	public bool AffectsFertility => fertility >= 0f;

	public List<PlaceWorker> PlaceWorkers
	{
		get
		{
			if (placeWorkers == null)
			{
				return null;
			}
			if (placeWorkersInstantiatedInt == null)
			{
				placeWorkersInstantiatedInt = new List<PlaceWorker>();
				foreach (Type placeWorker in placeWorkers)
				{
					placeWorkersInstantiatedInt.Add((PlaceWorker)Activator.CreateInstance(placeWorker));
				}
			}
			return placeWorkersInstantiatedInt;
		}
	}

	public bool IsResearchFinished
	{
		get
		{
			if (researchPrerequisites != null)
			{
				for (int i = 0; i < researchPrerequisites.Count; i++)
				{
					if (!researchPrerequisites[i].IsFinished)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public List<ThingDefCountClass> CostList
	{
		get
		{
			if (costListForDifficulty != null && costListForDifficulty.Applies)
			{
				return costListForDifficulty.costList;
			}
			return costList;
		}
	}

	public int CostStuffCount
	{
		get
		{
			if (costListForDifficulty != null && costListForDifficulty.Applies)
			{
				return costListForDifficulty.costStuffCount;
			}
			return costStuffCount;
		}
	}

	public bool ForceAllowPlaceOver(BuildableDef other)
	{
		if (PlaceWorkers == null)
		{
			return false;
		}
		for (int i = 0; i < PlaceWorkers.Count; i++)
		{
			if (PlaceWorkers[i].ForceAllowPlaceOver(other))
			{
				return true;
			}
		}
		return false;
	}

	public override void PostLoad()
	{
		base.PostLoad();
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			if (!uiIconPath.NullOrEmpty())
			{
				uiIcon = ContentFinder<Texture2D>.Get(uiIconPath);
			}
			else
			{
				ResolveIcon();
			}
			if (uiIconPathsStuff != null)
			{
				stuffUiIcons = new Dictionary<StuffAppearanceDef, Texture2D>();
				foreach (IconForStuffAppearance item in uiIconPathsStuff)
				{
					stuffUiIcons.Add(item.Appearance, ContentFinder<Texture2D>.Get(item.IconPath));
				}
			}
		});
	}

	protected virtual void ResolveIcon()
	{
		if (graphic == null || graphic == BaseContent.BadGraphic || this is ThingDef { mote: not null })
		{
			return;
		}
		Graphic outerGraphic = graphic;
		if (uiIconForStackCount >= 1 && this is ThingDef && graphic is Graphic_StackCount graphic_StackCount)
		{
			outerGraphic = graphic_StackCount.SubGraphicForStackCount(uiIconForStackCount, (ThingDef)this);
		}
		Material material = outerGraphic.ExtractInnerGraphicFor(null, null).MatAt(defaultPlacingRot);
		if (ShaderDatabase.TryGetUIShader(material.shader, out var uiShader) && MaterialPool.TryGetRequestForMat(material, out var request))
		{
			request.shader = uiShader;
			if (request.colorTwo == Color.white)
			{
				request.colorTwo = uiIconColorTwo;
			}
			uiIconMaterial = MaterialPool.MatFrom(request);
		}
		uiIcon = (Texture2D)material.mainTexture;
		uiIconColor = material.color;
	}

	public Texture2D GetUIIconForStuff(ThingDef stuff)
	{
		if (stuffUiIcons == null || stuff?.stuffProps.appearance == null || !stuffUiIcons.TryGetValue(stuff.stuffProps.appearance, out var value))
		{
			return uiIcon;
		}
		return value;
	}

	public Color GetColorForStuff(ThingDef stuff)
	{
		if (colorPerStuff.NullOrEmpty())
		{
			return stuff.stuffProps.color;
		}
		for (int i = 0; i < colorPerStuff.Count; i++)
		{
			ColorForStuff colorForStuff = colorPerStuff[i];
			if (colorForStuff.Stuff == stuff)
			{
				return colorForStuff.Color;
			}
		}
		return stuff.stuffProps.color;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__82))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__82(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__85))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__85(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}

	public override string ToString()
	{
		return defName;
	}

	public bool Equals(BuildableDef other)
	{
		return defName == other.defName;
	}
}
