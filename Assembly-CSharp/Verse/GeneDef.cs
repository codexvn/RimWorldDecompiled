using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse;

public class GeneDef : Def, IRenderNodePropertiesParent
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__110 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GeneDef _003C_003E4__this;

		private List<PawnRenderNodeProperties>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<string> _003C_003E7__wrap2;

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
		public _003CConfigErrors_003Ed__110(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 4)
			{
				try
				{
					if (num == -4 || num == 4)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				GeneDef geneDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (geneDef.geneClass == null)
					{
						_003C_003E2__current = "geneClass is null";
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0064;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0064;
				case 2:
					_003C_003E1__state = -1;
					goto IL_009b;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00d0;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_0145;
					}
					IL_00d0:
					if (!geneDef.HasDefinedGraphicProperties)
					{
						break;
					}
					_003C_003E7__wrap1 = geneDef.renderNodeProperties.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_015f;
					IL_0064:
					if (!typeof(Gene).IsAssignableFrom(geneDef.geneClass))
					{
						_003C_003E2__current = "geneClass is not Gene or subclass thereof";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_009b;
					IL_009b:
					if (geneDef.mentalBreakMtbDays > 0f && geneDef.mentalBreakDef == null)
					{
						_003C_003E2__current = "mentalBreakMtbDays is >0 with null mentalBreakDef";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00d0;
					IL_015f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						PawnRenderNodeProperties current = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0145;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
					break;
					IL_0145:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
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
			_003CConfigErrors_003Ed__110 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__110(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__109 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GeneDef _003C_003E4__this;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		private PawnCapacityModifier _003CcapMod_003E5__4;

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
		public _003CSpecialDisplayStats_003Ed__109(int _003C_003E1__state)
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
			_003CcapMod_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				GeneDef geneDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)geneDef).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00a8;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00a8;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0130;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01ad;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0230;
				case 5:
					_003C_003E1__state = -1;
					goto IL_02cd;
				case 6:
					_003C_003E1__state = -1;
					goto IL_0373;
				case 7:
					_003C_003E1__state = -1;
					goto IL_03fd;
				case 8:
					_003C_003E1__state = -1;
					goto IL_04af;
				case 9:
					_003C_003E1__state = -1;
					goto IL_0594;
				case 10:
					_003C_003E1__state = -1;
					goto IL_0614;
				case 11:
					_003C_003E1__state = -1;
					goto IL_06ca;
				case 12:
					_003C_003E1__state = -1;
					goto IL_0762;
				case 13:
					_003C_003E1__state = -1;
					goto IL_07c8;
				case 14:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00a8:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (geneDef.painFactor != 1f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), "x" + geneDef.painFactor.ToStringPercent(), "Stat_Hediff_Pain_Desc".Translate(), 4050);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0130;
					IL_03fd:
					_003Ci_003E5__3++;
					goto IL_040f;
					IL_0614:
					if (_003CcapMod_003E5__4.SetMaxDefined && req.Pawn != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, _003CcapMod_003E5__4.capacity.GetLabelFor().CapitalizeFirst(), "max".Translate().CapitalizeFirst() + " " + _003CcapMod_003E5__4.EvaluateSetMax(req.Pawn).ToStringPercent(), _003CcapMod_003E5__4.capacity.description, 4060);
						_003C_003E1__state = 11;
						return true;
					}
					goto IL_06ca;
					IL_0130:
					if (geneDef.painOffset != 0f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), (geneDef.painOffset * 100f).ToString("+###0;-###0") + "%", "Stat_Hediff_Pain_Desc".Translate(), 4050);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01ad;
					IL_04c1:
					if (_003Ci_003E5__3 < geneDef.statFactors.Count)
					{
						StatModifier statModifier = geneDef.statFactors[_003Ci_003E5__3];
						if (statModifier.stat.CanShowWithLoadedMods())
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier.stat.LabelCap, statModifier.ToStringAsFactor, statModifier.stat.description, 4070);
							_003C_003E1__state = 8;
							return true;
						}
						goto IL_04af;
					}
					goto IL_04d7;
					IL_01ad:
					if (geneDef.missingGeneRomanceChanceFactor != 1f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "MissingGeneRomanceChance".Translate(geneDef.LabelCap.Named("GENE")), "x" + geneDef.missingGeneRomanceChanceFactor.ToStringPercent(), "StatsReport_MissingGeneRomanceChance".Translate(), 4050);
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0230;
					IL_0425:
					if (geneDef.statFactors != null)
					{
						_003Ci_003E5__3 = 0;
						goto IL_04c1;
					}
					goto IL_04d7;
					IL_0230:
					if (geneDef.forcedTraits != null)
					{
						string text = geneDef.forcedTraits.Select((GeneticTraitData x) => x.def.DataAtDegree(x.degree).label).ToLineList(null, capitalizeItems: true);
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "ForcedTraits".Translate(), text, "ForcedTraitsDesc".Translate() + "\n\n" + text, 4080);
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_02cd;
					IL_07c8:
					if (geneDef.biostatArc != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Genetics, "ArchitesRequired".Translate().CapitalizeFirst(), geneDef.biostatArc.ToString(), "ArchitesRequiredDesc".Translate(), 995);
						_003C_003E1__state = 14;
						return true;
					}
					break;
					IL_04d7:
					if (geneDef.capMods != null)
					{
						_003Ci_003E5__3 = 0;
						goto IL_06e3;
					}
					goto IL_06f9;
					IL_04af:
					_003Ci_003E5__3++;
					goto IL_04c1;
					IL_02cd:
					if (geneDef.aptitudes != null)
					{
						string text2 = geneDef.aptitudes.Select((Aptitude x) => x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList(null, capitalizeItems: true);
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Aptitudes".Translate().CapitalizeFirst(), text2, "AptitudesDesc".Translate() + "\n\n" + text2, 4090);
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_0373;
					IL_06ca:
					_003CcapMod_003E5__4 = null;
					_003Ci_003E5__3++;
					goto IL_06e3;
					IL_06f9:
					if (geneDef.biostatCpx != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Genetics, "Complexity".Translate().CapitalizeFirst(), geneDef.biostatCpx.ToString(), "ComplexityDesc".Translate(), 998);
						_003C_003E1__state = 12;
						return true;
					}
					goto IL_0762;
					IL_06e3:
					if (_003Ci_003E5__3 < geneDef.capMods.Count)
					{
						_003CcapMod_003E5__4 = geneDef.capMods[_003Ci_003E5__3];
						if (_003CcapMod_003E5__4.offset != 0f)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, _003CcapMod_003E5__4.capacity.GetLabelFor().CapitalizeFirst(), (_003CcapMod_003E5__4.offset * 100f).ToString("+#;-#") + "%", _003CcapMod_003E5__4.capacity.description, 4060);
							_003C_003E1__state = 9;
							return true;
						}
						goto IL_0594;
					}
					goto IL_06f9;
					IL_0373:
					if (geneDef.statOffsets != null)
					{
						_003Ci_003E5__3 = 0;
						goto IL_040f;
					}
					goto IL_0425;
					IL_040f:
					if (_003Ci_003E5__3 < geneDef.statOffsets.Count)
					{
						StatModifier statModifier2 = geneDef.statOffsets[_003Ci_003E5__3];
						if (statModifier2.stat.CanShowWithLoadedMods())
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier2.stat.LabelCap, statModifier2.ValueToStringAsOffset, statModifier2.stat.description, 4070);
							_003C_003E1__state = 7;
							return true;
						}
						goto IL_03fd;
					}
					goto IL_0425;
					IL_0594:
					if (_003CcapMod_003E5__4.postFactor != 1f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, _003CcapMod_003E5__4.capacity.GetLabelFor().CapitalizeFirst(), "x" + _003CcapMod_003E5__4.postFactor.ToStringPercent(), _003CcapMod_003E5__4.capacity.description, 4060);
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_0614;
					IL_0762:
					if (geneDef.biostatMet != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Genetics, "Metabolism".Translate().CapitalizeFirst(), geneDef.biostatMet.ToString(), "MetabolismDesc".Translate(), 997);
						_003C_003E1__state = 13;
						return true;
					}
					goto IL_07c8;
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
			_003CSpecialDisplayStats_003Ed__109 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__109(0)
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

	public Type geneClass = typeof(Gene);

	[MustTranslate]
	public string labelShortAdj;

	[MustTranslate]
	public List<string> customEffectDescriptions;

	[NoTranslate]
	public string iconPath;

	private Color? iconColor;

	public GeneCategoryDef displayCategory;

	public float displayOrderInCategory;

	public List<PawnRenderNodeProperties> renderNodeProperties;

	public bool neverGrayHair;

	public bool skinIsHairColor;

	public bool tattoosVisible = true;

	public FurDef fur;

	public SoundDef soundCall;

	public SoundDef soundDeath;

	public SoundDef soundWounded;

	public Type resourceGizmoType = typeof(GeneGizmo_Resource);

	public float resourceLossPerDay;

	[MustTranslate]
	public string resourceLabel;

	[MustTranslate]
	public string resourceDescription;

	public List<float> resourceGizmoThresholds;

	public bool showGizmoOnWorldView;

	public bool showGizmoWhenDrafted;

	public bool showGizmoOnMultiSelect;

	public List<AbilityDef> abilities;

	public List<GeneticTraitData> forcedTraits;

	public List<GeneticTraitData> suppressedTraits;

	public List<NeedDef> enablesNeeds;

	public List<NeedDef> disablesNeeds;

	public WorkTags disabledWorkTags;

	public bool ignoreDarkness;

	public EndogeneCategory endogeneCategory;

	public bool dislikesSunlight;

	public float minAgeActive;

	public float lovinMTBFactor = 1f;

	public bool immuneToToxGasExposure;

	public bool immuneToVacuumBurns;

	public bool randomChosen;

	public int? waterCellCost;

	public HistoryEventDef deathHistoryEvent;

	public List<Aptitude> aptitudes;

	public PassionMod passionMod;

	public List<StatModifier> statOffsets;

	public List<StatModifier> statFactors;

	public List<ConditionalStatAffecter> conditionalStatAffecters;

	public float painOffset;

	public float painFactor = 1f;

	public float foodPoisoningChanceFactor = 1f;

	public List<DamageFactor> damageFactors;

	public SimpleCurve biologicalAgeTickFactorFromAgeCurve;

	public List<HediffDef> makeImmuneTo;

	public List<HediffDef> hediffGiversCannotGive;

	public ChemicalDef chemical;

	public float addictionChanceFactor = 1f;

	public float overdoseChanceFactor = 1f;

	public float toleranceBuildupFactor = 1f;

	public bool sterilize;

	public List<PawnCapacityModifier> capMods;

	public bool preventPermanentWounds;

	public bool dontMindRawFood;

	public Color? hairColorOverride;

	public Color? skinColorBase;

	public Color? skinColorOverride;

	public float randomBrightnessFactor;

	public TagFilter hairTagFilter;

	public TagFilter beardTagFilter;

	public GeneticBodyType? bodyType;

	public List<HeadTypeDef> forcedHeadTypes;

	public float minMelanin = -1f;

	public HairDef forcedHair;

	public bool womenCanHaveBeards;

	public float socialFightChanceFactor = 1f;

	public float aggroMentalBreakSelectionChanceFactor = 1f;

	public float mentalBreakMtbDays;

	public MentalBreakDef mentalBreakDef;

	public float missingGeneRomanceChanceFactor = 1f;

	public float prisonBreakMTBFactor = 1f;

	public int biostatCpx = 1;

	public int biostatMet;

	public int biostatArc;

	public List<string> exclusionTags;

	public GeneDef prerequisite;

	public float selectionWeight = 1f;

	public bool canGenerateInGeneSet = true;

	public GeneSymbolPack symbolPack;

	public float marketValueFactor = 1f;

	public bool removeOnRedress;

	public bool passOnDirectly = true;

	public float selectionWeightFactorDarkSkin = 1f;

	public float selectionWeightCultist = 1f;

	[Unsaved(false)]
	private string cachedDescription;

	[Unsaved(false)]
	private Texture2D cachedIcon;

	public Texture2D Icon
	{
		get
		{
			if (cachedIcon == null)
			{
				if (iconPath.NullOrEmpty())
				{
					cachedIcon = BaseContent.BadTex;
				}
				else
				{
					cachedIcon = ContentFinder<Texture2D>.Get(iconPath) ?? BaseContent.BadTex;
				}
			}
			return cachedIcon;
		}
	}

	public Color IconColor
	{
		get
		{
			if (iconColor.HasValue)
			{
				return iconColor.Value;
			}
			if (skinColorBase.HasValue)
			{
				return skinColorBase.Value;
			}
			if (skinColorOverride.HasValue)
			{
				return skinColorOverride.Value;
			}
			if (hairColorOverride.HasValue)
			{
				return hairColorOverride.Value;
			}
			return Color.white;
		}
	}

	public string LabelShortAdj
	{
		get
		{
			if (!labelShortAdj.NullOrEmpty())
			{
				return labelShortAdj;
			}
			return label;
		}
	}

	public string DescriptionFull => cachedDescription ?? (cachedDescription = GetDescriptionFull());

	public bool HasDefinedGraphicProperties => !renderNodeProperties.NullOrEmpty();

	public List<PawnRenderNodeProperties> RenderNodeProperties => renderNodeProperties;

	public bool RandomChosen
	{
		get
		{
			if (randomChosen)
			{
				return true;
			}
			if (biostatArc > 0 || biostatCpx != 0 || biostatMet != 0)
			{
				return false;
			}
			if (!hairColorOverride.HasValue && !skinColorBase.HasValue && !skinColorOverride.HasValue && !bodyType.HasValue && forcedHeadTypes.NullOrEmpty() && hairTagFilter == null && beardTagFilter == null)
			{
				return HasDefinedGraphicProperties;
			}
			return true;
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		if (displayCategory == null)
		{
			displayCategory = GeneCategoryDefOf.Miscellaneous;
		}
		if (renderNodeProperties != null)
		{
			for (int i = 0; i < renderNodeProperties.Count; i++)
			{
				renderNodeProperties[i].ResolveReferencesRecursive();
			}
		}
	}

	public int AptitudeFor(SkillDef skill)
	{
		int num = 0;
		if (aptitudes.NullOrEmpty())
		{
			return num;
		}
		for (int i = 0; i < aptitudes.Count; i++)
		{
			if (aptitudes[i].skill == skill)
			{
				num += aptitudes[i].level;
			}
		}
		return num;
	}

	private string GetDescriptionFull()
	{
		StringBuilder sb = new StringBuilder();
		if (!description.NullOrEmpty())
		{
			sb.Append(description).AppendLine().AppendLine();
		}
		bool flag = false;
		if (prerequisite != null)
		{
			sb.AppendLine("Requires".Translate() + ": " + prerequisite.LabelCap);
			flag = true;
		}
		if (minAgeActive > 0f)
		{
			sb.AppendLine(string.Concat("TakesEffectAfterAge".Translate() + ": ", minAgeActive.ToString()));
			flag = true;
		}
		if (flag)
		{
			sb.AppendLine();
		}
		bool flag2 = false;
		if (biostatCpx != 0)
		{
			sb.AppendLineTagged("Complexity".Translate().Colorize(GeneUtility.GCXColor) + ": " + biostatCpx.ToStringWithSign());
			flag2 = true;
		}
		if (biostatMet != 0)
		{
			sb.AppendLineTagged("Metabolism".Translate().CapitalizeFirst().Colorize(GeneUtility.METColor) + ": " + biostatMet.ToStringWithSign());
			flag2 = true;
		}
		if (biostatArc != 0)
		{
			sb.AppendLineTagged("ArchitesRequired".Translate().Colorize(GeneUtility.ARCColor) + ": " + biostatArc.ToStringWithSign());
			flag2 = true;
		}
		if (flag2)
		{
			sb.AppendLine();
		}
		if (forcedTraits != null)
		{
			sb.AppendLineTagged(("ForcedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
			sb.Append(forcedTraits.Select((GeneticTraitData x) => x.def.DataAtDegree(x.degree).label).ToLineList("  - ", capitalizeItems: true)).AppendLine().AppendLine();
		}
		if (suppressedTraits != null)
		{
			sb.AppendLineTagged(("SuppressedTraits".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
			sb.Append(suppressedTraits.Select((GeneticTraitData x) => x.def.DataAtDegree(x.degree).label).ToLineList("  - ", capitalizeItems: true)).AppendLine().AppendLine();
		}
		if (aptitudes != null)
		{
			sb.AppendLineTagged(("Aptitudes".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
			sb.Append(aptitudes.Select((Aptitude x) => x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", capitalizeItems: true)).AppendLine().AppendLine();
		}
		bool effectsTitleWritten = false;
		if (passionMod != null)
		{
			switch (passionMod.modType)
			{
			case PassionMod.PassionModType.AddOneLevel:
				AppendEffectLine("PassionModAdd".Translate(passionMod.skill));
				break;
			case PassionMod.PassionModType.DropAll:
				AppendEffectLine("PassionModDrop".Translate(passionMod.skill));
				break;
			}
		}
		if (!statFactors.NullOrEmpty())
		{
			for (int i = 0; i < statFactors.Count; i++)
			{
				StatModifier statModifier = statFactors[i];
				if (statModifier.stat.CanShowWithLoadedMods())
				{
					AppendEffectLine(statModifier.stat.LabelCap + " " + statModifier.ToStringAsFactor);
				}
			}
		}
		if (!conditionalStatAffecters.NullOrEmpty())
		{
			for (int j = 0; j < conditionalStatAffecters.Count; j++)
			{
				if (conditionalStatAffecters[j].statFactors.NullOrEmpty())
				{
					continue;
				}
				for (int k = 0; k < conditionalStatAffecters[j].statFactors.Count; k++)
				{
					StatModifier statModifier2 = conditionalStatAffecters[j].statFactors[k];
					if (statModifier2.stat.CanShowWithLoadedMods())
					{
						AppendEffectLine(statModifier2.stat.LabelCap + " " + statModifier2.ToStringAsFactor + " (" + conditionalStatAffecters[j].Label + ")");
					}
				}
			}
		}
		if (!statOffsets.NullOrEmpty())
		{
			for (int l = 0; l < statOffsets.Count; l++)
			{
				StatModifier statModifier3 = statOffsets[l];
				if (statModifier3.stat.CanShowWithLoadedMods())
				{
					AppendEffectLine(statModifier3.stat.LabelCap + " " + statModifier3.ValueToStringAsOffset);
				}
			}
		}
		if (!conditionalStatAffecters.NullOrEmpty())
		{
			for (int m = 0; m < conditionalStatAffecters.Count; m++)
			{
				if (conditionalStatAffecters[m].statOffsets.NullOrEmpty())
				{
					continue;
				}
				for (int n = 0; n < conditionalStatAffecters[m].statOffsets.Count; n++)
				{
					StatModifier statModifier4 = conditionalStatAffecters[m].statOffsets[n];
					if (statModifier4.stat.CanShowWithLoadedMods())
					{
						AppendEffectLine(statModifier4.stat.LabelCap + " " + statModifier4.ValueToStringAsOffset + " (" + conditionalStatAffecters[m].Label.UncapitalizeFirst() + ")");
					}
				}
			}
		}
		if (!capMods.NullOrEmpty())
		{
			for (int num = 0; num < capMods.Count; num++)
			{
				PawnCapacityModifier pawnCapacityModifier = capMods[num];
				if (pawnCapacityModifier.offset != 0f)
				{
					AppendEffectLine(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " " + (pawnCapacityModifier.offset * 100f).ToString("+#;-#") + "%");
				}
				if (pawnCapacityModifier.postFactor != 1f)
				{
					AppendEffectLine(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " x" + pawnCapacityModifier.postFactor.ToStringPercent());
				}
				if (pawnCapacityModifier.setMax != 999f)
				{
					AppendEffectLine(pawnCapacityModifier.capacity.GetLabelFor().CapitalizeFirst() + " " + "max".Translate().CapitalizeFirst() + ": " + pawnCapacityModifier.setMax.ToStringPercent());
				}
			}
		}
		if (!customEffectDescriptions.NullOrEmpty())
		{
			foreach (string customEffectDescription in customEffectDescriptions)
			{
				AppendEffectLine(customEffectDescription.ResolveTags());
			}
		}
		if (!damageFactors.NullOrEmpty())
		{
			for (int num2 = 0; num2 < damageFactors.Count; num2++)
			{
				AppendEffectLine("DamageType".Translate(damageFactors[num2].damageDef.label).CapitalizeFirst() + " x" + damageFactors[num2].factor.ToStringPercent());
			}
		}
		if (resourceLossPerDay != 0f && !resourceLabel.NullOrEmpty())
		{
			AppendEffectLine("ResourceLossPerDay".Translate(resourceLabel.Named("RESOURCE"), (-Mathf.RoundToInt(resourceLossPerDay * 100f)).ToStringWithSign().Named("OFFSET")).CapitalizeFirst());
		}
		if (!Mathf.Approximately(painFactor, 1f))
		{
			AppendEffectLine("Pain".Translate() + " x" + painFactor.ToStringPercent());
		}
		if (painOffset != 0f)
		{
			AppendEffectLine("Pain".Translate() + " " + (painOffset * 100f).ToString("+###0;-###0") + "%");
		}
		if (chemical != null)
		{
			if (!Mathf.Approximately(addictionChanceFactor, 1f))
			{
				if (addictionChanceFactor <= 0f)
				{
					AppendEffectLine("AddictionImmune".Translate(chemical).CapitalizeFirst());
				}
				else
				{
					AppendEffectLine("AddictionChanceFactor".Translate(chemical).CapitalizeFirst() + " x" + addictionChanceFactor.ToStringPercent());
				}
			}
			if (overdoseChanceFactor != 1f)
			{
				AppendEffectLine("OverdoseChanceFactor".Translate(chemical).CapitalizeFirst() + " x" + overdoseChanceFactor.ToStringPercent());
			}
			if (toleranceBuildupFactor != 1f)
			{
				AppendEffectLine("ToleranceBuildupFactor".Translate(chemical).CapitalizeFirst() + " x" + toleranceBuildupFactor.ToStringPercent());
			}
		}
		if (!enablesNeeds.NullOrEmpty())
		{
			if (enablesNeeds.Count == 1)
			{
				AppendEffectLine($"{"AddsNeed".Translate()}: {enablesNeeds[0].LabelCap}");
			}
			else
			{
				AppendEffectLine($"{"AddsNeeds".Translate()}: {enablesNeeds.Select((NeedDef x) => x.label).ToCommaList().CapitalizeFirst()}");
			}
		}
		if (!disablesNeeds.NullOrEmpty())
		{
			if (disablesNeeds.Count == 1)
			{
				AppendEffectLine($"{"DisablesNeed".Translate()}: {disablesNeeds[0].LabelCap}");
			}
			else
			{
				AppendEffectLine($"{"DisablesNeeds".Translate()}: {disablesNeeds.Select((NeedDef x) => x.label).ToCommaList().CapitalizeFirst()}");
			}
		}
		if (missingGeneRomanceChanceFactor != 1f)
		{
			AppendEffectLine("MissingGeneRomanceChance".Translate(label.Named("GENE")) + " x" + missingGeneRomanceChanceFactor.ToStringPercent());
		}
		if (ignoreDarkness)
		{
			AppendEffectLine("UnaffectedByDarkness".Translate());
		}
		if (foodPoisoningChanceFactor != 1f)
		{
			if (foodPoisoningChanceFactor <= 0f)
			{
				AppendEffectLine("FoodPoisoningImmune".Translate());
			}
			else
			{
				AppendEffectLine("Stat_Hediff_FoodPoisoningChanceFactor_Name".Translate() + " x" + foodPoisoningChanceFactor.ToStringPercent());
			}
		}
		if (socialFightChanceFactor != 1f)
		{
			if (socialFightChanceFactor <= 0f)
			{
				AppendEffectLine("WillNeverSocialFight".Translate());
			}
			else
			{
				AppendEffectLine("SocialFightChanceFactor".Translate() + " x" + socialFightChanceFactor.ToStringPercent());
			}
		}
		if (aggroMentalBreakSelectionChanceFactor != 1f)
		{
			if (aggroMentalBreakSelectionChanceFactor >= 999f)
			{
				AppendEffectLine("AlwaysAggroMentalBreak".Translate());
			}
			else if (aggroMentalBreakSelectionChanceFactor <= 0f)
			{
				AppendEffectLine("NeverAggroMentalBreak".Translate());
			}
			else
			{
				AppendEffectLine("AggroMentalBreakSelectionChanceFactor".Translate() + " x" + aggroMentalBreakSelectionChanceFactor.ToStringPercent());
			}
		}
		if (prisonBreakMTBFactor != 1f)
		{
			if (prisonBreakMTBFactor < 0f)
			{
				AppendEffectLine("WillNeverPrisonBreak".Translate());
			}
			else
			{
				AppendEffectLine("PrisonBreakIntervalFactor".Translate() + " x" + prisonBreakMTBFactor.ToStringPercent());
			}
		}
		bool flag3 = effectsTitleWritten;
		if (!makeImmuneTo.NullOrEmpty())
		{
			if (flag3)
			{
				sb.AppendLine();
			}
			sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
			sb.AppendLine(makeImmuneTo.Select((HediffDef x) => x.label).ToLineList("  - ", capitalizeItems: true));
			flag3 = true;
		}
		if (!hediffGiversCannotGive.NullOrEmpty())
		{
			if (flag3)
			{
				sb.AppendLine();
			}
			sb.AppendLineTagged(("ImmuneTo".Translate() + ":").Colorize(ColoredText.TipSectionTitleColor));
			sb.AppendLine(hediffGiversCannotGive.Select((HediffDef x) => x.label).ToLineList("  - ", capitalizeItems: true));
			flag3 = true;
		}
		if (biologicalAgeTickFactorFromAgeCurve != null)
		{
			if (flag3)
			{
				sb.AppendLine();
			}
			sb.AppendLineTagged(("AgeFactors".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
			sb.AppendLine(biologicalAgeTickFactorFromAgeCurve.Select((CurvePoint p) => "PeriodYears".Translate(p.x).ToString() + ": x" + p.y.ToStringPercent()).ToLineList("  - ", capitalizeItems: true));
			flag3 = true;
		}
		if (disabledWorkTags != 0)
		{
			if (flag3)
			{
				sb.AppendLine();
			}
			IEnumerable<WorkTypeDef> source = DefDatabase<WorkTypeDef>.AllDefsListForReading.Where((WorkTypeDef x) => (disabledWorkTags & x.workTags) != 0);
			sb.AppendLineTagged(("DisabledWorkLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
			sb.AppendLine("  - " + source.Select((WorkTypeDef x) => x.labelShort).ToCommaList().CapitalizeFirst());
			if (disabledWorkTags.ExactlyOneWorkTagSet())
			{
				sb.AppendLine("  - " + disabledWorkTags.LabelTranslated().CapitalizeFirst());
			}
			flag3 = true;
		}
		if (!abilities.NullOrEmpty())
		{
			if (flag3)
			{
				sb.AppendLine();
			}
			if (abilities.Count == 1)
			{
				sb.AppendLineTagged(("GivesAbility".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
			}
			else
			{
				sb.AppendLineTagged(("GivesAbilities".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
			}
			sb.AppendLine(abilities.Select((AbilityDef x) => x.label).ToLineList("  - ", capitalizeItems: true));
			flag3 = true;
		}
		IEnumerable<ThoughtDef> enumerable = DefDatabase<ThoughtDef>.AllDefs.Where((ThoughtDef x) => (x.requiredGenes.NotNullAndContains(this) || x.nullifyingGenes.NotNullAndContains(this)) && x.stages != null && x.stages.Any((ThoughtStage y) => y.baseMoodEffect != 0f));
		if (enumerable.Any())
		{
			if (flag3)
			{
				sb.AppendLine();
			}
			sb.AppendLineTagged(("Mood".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
			foreach (ThoughtDef item in enumerable)
			{
				ThoughtStage thoughtStage = item.stages.FirstOrDefault((ThoughtStage x) => x.baseMoodEffect != 0f);
				if (thoughtStage != null)
				{
					string text2 = thoughtStage.LabelCap + ": " + thoughtStage.baseMoodEffect.ToStringWithSign();
					if (item.requiredGenes.NotNullAndContains(this))
					{
						sb.AppendLine("  - " + text2);
					}
					else if (item.nullifyingGenes.NotNullAndContains(this))
					{
						sb.AppendLine("  - " + "Removes".Translate() + ": " + text2);
					}
				}
			}
		}
		return sb.ToString().TrimEndNewlines();
		void AppendEffectLine(string text)
		{
			if (!effectsTitleWritten)
			{
				sb.AppendLineTagged(("Effects".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
				effectsTitleWritten = true;
			}
			sb.AppendLine("  - " + text);
		}
	}

	public bool ConflictsWith(GeneDef other)
	{
		if (this == other)
		{
			return true;
		}
		if (exclusionTags != null && other.exclusionTags != null)
		{
			for (int i = 0; i < exclusionTags.Count; i++)
			{
				if (other.exclusionTags.Contains(exclusionTags[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__109))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__109(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__110))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__110(-2)
		{
			_003C_003E4__this = this
		};
	}
}
