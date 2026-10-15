using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class HediffDef : Def, IRenderNodePropertiesParent
{
	private class ReportStringOverride
	{
		public JobDef jobDef;

		[MustTranslate]
		public string reportString;
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__105 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HediffDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		private HediffInfectionPathway _003Cvector_003E5__4;

		private int _003Cj_003E5__5;

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
		public _003CConfigErrors_003Ed__105(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 10:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 12:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 14:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003Cvector_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				HediffDef hediffDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)hediffDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00a2;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00a2;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ea;
				case 3:
					_003C_003E1__state = -1;
					goto IL_012e;
				case 4:
					_003C_003E1__state = -1;
					goto IL_015c;
				case 5:
					_003C_003E1__state = -1;
					goto IL_018a;
				case 6:
					_003C_003E1__state = -1;
					goto IL_01c4;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0201;
				case 8:
					_003C_003E1__state = -1;
					goto IL_02aa;
				case 9:
					_003C_003E1__state = -1;
					goto IL_02f2;
				case 10:
					_003C_003E1__state = -4;
					goto IL_03cd;
				case 11:
					_003C_003E1__state = -1;
					goto IL_0497;
				case 12:
					_003C_003E1__state = -5;
					goto IL_0556;
				case 13:
					_003C_003E1__state = -1;
					goto IL_0618;
				case 14:
					{
						_003C_003E1__state = -6;
						goto IL_06ab;
					}
					IL_00a2:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (hediffDef.hediffClass == null)
					{
						_003C_003E2__current = "hediffClass is null";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ea;
					IL_0640:
					if (hediffDef.hediffGivers == null)
					{
						break;
					}
					_003Ci_003E5__3 = 0;
					goto IL_06d7;
					IL_0582:
					if (_003Cj_003E5__5 < hediffDef.stages[_003Ci_003E5__3].hediffGivers.Count)
					{
						_003C_003E7__wrap1 = hediffDef.stages[_003Ci_003E5__3].hediffGivers[_003Cj_003E5__5].ConfigErrors().GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_0556;
					}
					goto IL_05a8;
					IL_00ea:
					if (!hediffDef.comps.NullOrEmpty() && !typeof(HediffWithComps).IsAssignableFrom(hediffDef.hediffClass))
					{
						_003C_003E2__current = "has comps but hediffClass is not HediffWithComps or subclass thereof";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_012e;
					IL_03cd:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = $"{hediffDef.comps[_003Ci_003E5__3]}: {current2}";
						_003C_003E1__state = 10;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3++;
					goto IL_03f9;
					IL_040f:
					if (hediffDef.stages != null)
					{
						if (!typeof(Hediff_Addiction).IsAssignableFrom(hediffDef.hediffClass))
						{
							_003Ci_003E5__3 = 0;
							goto IL_04a9;
						}
						goto IL_04bc;
					}
					goto IL_0640;
					IL_012e:
					if (hediffDef.minSeverity > hediffDef.initialSeverity)
					{
						_003C_003E2__current = "minSeverity is greater than initialSeverity";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_015c;
					IL_0556:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 12;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					_003Cj_003E5__5++;
					goto IL_0582;
					IL_015c:
					if (hediffDef.maxSeverity < hediffDef.initialSeverity)
					{
						_003C_003E2__current = "maxSeverity is lower than initialSeverity";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_018a;
					IL_06d7:
					if (_003Ci_003E5__3 >= hediffDef.hediffGivers.Count)
					{
						break;
					}
					_003C_003E7__wrap1 = hediffDef.hediffGivers[_003Ci_003E5__3].ConfigErrors().GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_06ab;
					IL_018a:
					if (!hediffDef.tendable && hediffDef.HasComp(typeof(HediffComp_TendDuration)))
					{
						_003C_003E2__current = "has HediffComp_TendDuration but tendable = false";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_01c4;
					IL_04a9:
					if (_003Ci_003E5__3 < hediffDef.stages.Count)
					{
						if (_003Ci_003E5__3 >= 1 && hediffDef.stages[_003Ci_003E5__3].minSeverity <= hediffDef.stages[_003Ci_003E5__3 - 1].minSeverity)
						{
							_003C_003E2__current = "stages are not in order of minSeverity";
							_003C_003E1__state = 11;
							return true;
						}
						goto IL_0497;
					}
					goto IL_04bc;
					IL_01c4:
					if (string.IsNullOrEmpty(hediffDef.description))
					{
						_003C_003E2__current = "Hediff with defName " + hediffDef.defName + " has no description!";
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0201;
					IL_05a8:
					if (hediffDef.stages[_003Ci_003E5__3].minSeverity > hediffDef.maxSeverity)
					{
						_003C_003E2__current = $"minSeverity of stage {_003Ci_003E5__3} is greater than maxSeverity.";
						_003C_003E1__state = 13;
						return true;
					}
					goto IL_0618;
					IL_0201:
					if (hediffDef.possiblePathways != null)
					{
						_003Ci_003E5__3 = 0;
						goto IL_030b;
					}
					goto IL_0323;
					IL_06ab:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 14;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3++;
					goto IL_06d7;
					IL_030b:
					if (_003Ci_003E5__3 < hediffDef.possiblePathways.Count - 1)
					{
						_003Cvector_003E5__4 = hediffDef.possiblePathways[_003Ci_003E5__3];
						for (int i = _003Ci_003E5__3 + 1; i < hediffDef.possiblePathways.Count; i++)
						{
							HediffInfectionPathway hediffInfectionPathway = hediffDef.possiblePathways[i];
							if (_003Cvector_003E5__4.PathwayDef == hediffInfectionPathway.PathwayDef)
							{
								_003C_003E2__current = "Multiple possible infection vectors of type: " + _003Cvector_003E5__4.PathwayDef.defName;
								_003C_003E1__state = 8;
								return true;
							}
						}
						goto IL_02aa;
					}
					goto IL_0323;
					IL_0497:
					_003Ci_003E5__3++;
					goto IL_04a9;
					IL_04bc:
					_003Ci_003E5__3 = 0;
					goto IL_062a;
					IL_02aa:
					if (string.IsNullOrEmpty(_003Cvector_003E5__4.Explanation))
					{
						_003C_003E2__current = "Missing explanation for possible infection vector: " + _003Cvector_003E5__4.PathwayDef.defName;
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_02f2;
					IL_062a:
					if (_003Ci_003E5__3 < hediffDef.stages.Count)
					{
						if (hediffDef.stages[_003Ci_003E5__3].hediffGivers != null)
						{
							_003Cj_003E5__5 = 0;
							goto IL_0582;
						}
						goto IL_05a8;
					}
					goto IL_0640;
					IL_02f2:
					_003Cvector_003E5__4 = null;
					_003Ci_003E5__3++;
					goto IL_030b;
					IL_0323:
					if (hediffDef.comps != null)
					{
						_003Ci_003E5__3 = 0;
						goto IL_03f9;
					}
					goto IL_040f;
					IL_0618:
					_003Ci_003E5__3++;
					goto IL_062a;
					IL_03f9:
					if (_003Ci_003E5__3 < hediffDef.comps.Count)
					{
						_003C_003E7__wrap1 = hediffDef.comps[_003Ci_003E5__3].ConfigErrors(hediffDef).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_03cd;
					}
					goto IL_040f;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
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
			_003CConfigErrors_003Ed__105 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__105(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__106 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HediffDef _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__106(int _003C_003E1__state)
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
				HediffDef hediffDef = _003C_003E4__this;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
				}
				else
				{
					_003C_003E1__state = -1;
					if (hediffDef.stages == null || hediffDef.stages.Count != 1)
					{
						goto IL_009f;
					}
					_003C_003E7__wrap1 = hediffDef.stages[0].SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					StatDrawEntry current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				goto IL_009f;
				IL_009f:
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
			_003CSpecialDisplayStats_003Ed__106 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__106(0)
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

	public Type hediffClass = typeof(Hediff);

	public List<HediffCompProperties> comps;

	[MustTranslate]
	public string descriptionShort;

	[NoTranslate]
	public string debugLabelExtra;

	public float initialSeverity = 0.5f;

	public float lethalSeverity = -1f;

	public List<HediffStage> stages;

	public bool tendable;

	public bool isBad = true;

	public ThingDef spawnThingOnRemoved;

	public float chanceToCauseNoPain;

	public bool canApplyDodChanceForCapacityChanges;

	public bool makesSickThought;

	public bool makesAlert = true;

	public NeedDef chemicalNeed;

	public float minSeverity;

	public float maxSeverity = float.MaxValue;

	public bool scenarioCanAdd;

	public List<HediffGiver> hediffGivers;

	public bool cureAllAtOnceIfCuredByItem;

	public TaleDef taleOnVisible;

	public bool recordDownedTale = true;

	public bool everCurableByItem = true;

	public List<string> tags;

	public bool priceImpact;

	public float priceOffset;

	public bool chronic;

	public bool keepOnBodyPartRestoration;

	public bool countsAsAddedPartOrImplant;

	public bool blocksSocialInteraction;

	public bool blocksSleeping;

	[MustTranslate]
	public string overrideTooltip;

	[MustTranslate]
	public string extraTooltip;

	[MustTranslate]
	public string inspectString;

	public bool levelIsQuantity;

	public bool removeOnDeathrestStart;

	public bool preventsCrawling;

	public bool preventsPregnancy;

	public bool preventsLungRot;

	public bool pregnant;

	public bool allowMothballIfLowPriorityWorldPawn;

	public List<string> removeWithTags;

	public List<BodyPartDef> onlyLifeThreateningTo;

	public bool canAffectBionicOrImplant = true;

	public bool alwaysShowSeverity;

	public bool showGizmosOnCorpse;

	public BodyPartDef defaultInstallPart;

	public Color? hairColorOverride;

	public List<HediffInfectionPathway> possiblePathways;

	public List<InfectionPathwayDef> givesInfectionPathways;

	public bool duplicationAllowed = true;

	public bool preventsDeath;

	public List<MeditationFocusDef> allowedMeditationFocusTypes;

	public List<AbilityDef> abilities;

	public bool isInfection;

	public bool forceRemoveOnResurrection;

	public bool organicAddedBodypart;

	public bool deprioritizeHealing;

	public bool clearsEgo;

	public List<Aptitude> aptitudes;

	public SimpleCurve removeOnRedressChanceByDaysCurve = new SimpleCurve
	{
		new CurvePoint(0f, 0f),
		new CurvePoint(1f, 0f)
	};

	public bool removeOnQuestLodgers;

	public List<PawnKindDef> removeOnRedressIfNotOfKind;

	public bool displayWound;

	public float? woundAnchorRange;

	public Color defaultLabelColor = Color.white;

	private List<PawnRenderNodeProperties> renderNodeProperties;

	public Color? skinColorOverride;

	public Color? skinColorTint;

	public float skinColorTintStrength = 0.5f;

	public ShaderTypeDef skinShader;

	public bool forceRenderTreeRecache;

	public InjuryProps injuryProps;

	public AddedBodyPartProps addedPartProps;

	private List<ReportStringOverride> reportStringOverrides;

	[MustTranslate]
	public string labelNoun;

	[MustTranslate]
	public string battleStateLabel;

	[MustTranslate]
	public string labelNounPretty;

	[MustTranslate]
	public string targetPrefix;

	private bool alwaysAllowMothballCached;

	private bool alwaysAllowMothball;

	private string descriptionCached;

	private Dictionary<JobDef, string> reportStringOverridesDict;

	private Hediff concreteExampleInt;

	public bool HasDefinedGraphicProperties
	{
		get
		{
			if (renderNodeProperties.NullOrEmpty())
			{
				return skinShader != null;
			}
			return true;
		}
	}

	public List<PawnRenderNodeProperties> RenderNodeProperties => renderNodeProperties ?? PawnRenderUtility.EmptyRenderNodeProperties;

	public bool IsAddiction => typeof(Hediff_Addiction).IsAssignableFrom(hediffClass);

	public bool AlwaysAllowMothball
	{
		get
		{
			if (!alwaysAllowMothballCached)
			{
				alwaysAllowMothball = true;
				if (comps != null && comps.Count > 0)
				{
					alwaysAllowMothball = false;
				}
				if (stages != null)
				{
					for (int i = 0; i < stages.Count; i++)
					{
						HediffStage hediffStage = stages[i];
						if (hediffStage.deathMtbDays > 0f || (hediffStage.hediffGivers != null && hediffStage.hediffGivers.Count > 0))
						{
							alwaysAllowMothball = false;
						}
					}
				}
				alwaysAllowMothballCached = true;
			}
			return alwaysAllowMothball;
		}
	}

	public Hediff ConcreteExample => concreteExampleInt ?? (concreteExampleInt = HediffMaker.Debug_MakeConcreteExampleHediff(this));

	public string Description
	{
		get
		{
			if (descriptionCached == null)
			{
				if (!descriptionShort.NullOrEmpty())
				{
					descriptionCached = descriptionShort;
				}
				else
				{
					descriptionCached = description;
				}
			}
			return descriptionCached;
		}
	}

	public bool HasComp(Type compClass)
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i].compClass == compClass)
				{
					return true;
				}
			}
		}
		return false;
	}

	public HediffCompProperties CompPropsFor(Type compClass)
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i].compClass == compClass)
				{
					return comps[i];
				}
			}
		}
		return null;
	}

	public T CompProps<T>() where T : HediffCompProperties
	{
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				if (comps[i] is T result)
				{
					return result;
				}
			}
		}
		return null;
	}

	public bool PossibleToDevelopImmunityNaturally()
	{
		HediffCompProperties_Immunizable hediffCompProperties_Immunizable = CompProps<HediffCompProperties_Immunizable>();
		if (hediffCompProperties_Immunizable != null && (hediffCompProperties_Immunizable.immunityPerDayNotSick > 0f || hediffCompProperties_Immunizable.immunityPerDaySick > 0f))
		{
			return true;
		}
		return false;
	}

	public string PrettyTextForPart(BodyPartRecord bodyPart)
	{
		if (labelNounPretty.NullOrEmpty())
		{
			return null;
		}
		return labelNounPretty.Formatted(label, bodyPart.Label);
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		if (comps != null)
		{
			for (int i = 0; i < comps.Count; i++)
			{
				comps[i].ResolveReferences(this);
			}
		}
		if (renderNodeProperties != null)
		{
			for (int j = 0; j < renderNodeProperties.Count; j++)
			{
				renderNodeProperties[j].ResolveReferencesRecursive();
			}
		}
	}

	public int StageAtSeverity(float severity)
	{
		if (stages == null)
		{
			return 0;
		}
		for (int num = stages.Count - 1; num >= 0; num--)
		{
			if (severity >= stages[num].minSeverity)
			{
				return num;
			}
		}
		return 0;
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

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__105))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__105(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__106))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__106(-2)
		{
			_003C_003E4__this = this
		};
	}

	public bool TryGetReportStringOverrideFor(JobDef def, out string str)
	{
		if (reportStringOverrides.NullOrEmpty())
		{
			str = null;
			return false;
		}
		if (reportStringOverridesDict == null)
		{
			reportStringOverridesDict = new Dictionary<JobDef, string>();
			if (reportStringOverrides != null)
			{
				foreach (ReportStringOverride reportStringOverride in reportStringOverrides)
				{
					reportStringOverridesDict[reportStringOverride.jobDef] = reportStringOverride.reportString;
				}
			}
		}
		return reportStringOverridesDict.TryGetValue(def, out str);
	}

	public static HediffDef Named(string defName)
	{
		return DefDatabase<HediffDef>.GetNamed(defName);
	}
}
