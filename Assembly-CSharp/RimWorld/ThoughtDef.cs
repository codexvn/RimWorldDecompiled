using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ThoughtDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__69 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ThoughtDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003CConfigErrors_003Ed__69(int _003C_003E1__state)
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
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ThoughtDef thoughtDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)thoughtDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c9;
				case 3:
					_003C_003E1__state = -1;
					goto IL_00ff;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0135;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0173;
				case 6:
					{
						_003C_003E1__state = -4;
						goto IL_01e5;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (thoughtDef.stages.NullOrEmpty())
					{
						_003C_003E2__current = "no stages";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c9;
					IL_0135:
					if (!thoughtDef.IsMemory && thoughtDef.workerClass == null && thoughtDef.IsSituational)
					{
						_003C_003E2__current = "is a situational thought but has no workerClass. Situational thoughts require workerClasses to analyze the situation";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0173;
					IL_01ff:
					_003Ci_003E5__3++;
					goto IL_0211;
					IL_00c9:
					if (thoughtDef.workerClass != null && thoughtDef.nextThought != null)
					{
						_003C_003E2__current = "has a nextThought but also has a workerClass. nextThought only works for memories";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00ff;
					IL_01e5:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 6;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_01ff;
					IL_0173:
					_003Ci_003E5__3 = 0;
					goto IL_0211;
					IL_00ff:
					if (thoughtDef.IsMemory && thoughtDef.workerClass != null)
					{
						_003C_003E2__current = "has a workerClass but is a memory. workerClass only works for situational thoughts, not memories";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0135;
					IL_0211:
					if (_003Ci_003E5__3 < thoughtDef.stages.Count)
					{
						if (thoughtDef.stages[_003Ci_003E5__3] != null)
						{
							_003C_003E7__wrap1 = thoughtDef.stages[_003Ci_003E5__3].ConfigErrors().GetEnumerator();
							_003C_003E1__state = -4;
							goto IL_01e5;
						}
						goto IL_01ff;
					}
					return false;
				}
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__69 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__69(0)
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

	public Type thoughtClass;

	public Type workerClass;

	public List<ThoughtStage> stages = new List<ThoughtStage>();

	public int stackLimit = 1;

	public float stackedEffectMultiplier = 0.75f;

	public bool stagesStack;

	public float durationDays;

	public bool invert;

	public bool validWhileDespawned;

	public ThoughtDef nextThought;

	public ThoughtDef producesMemoryThought;

	public List<TraitDef> nullifyingTraits;

	public List<TraitDef> neverNullifyIfAnyTrait;

	[XmlInheritanceAllowDuplicateNodes]
	public List<TraitRequirement> nullifyingTraitDegrees;

	public List<TaleDef> nullifyingOwnTales;

	public List<PreceptDef> nullifyingPrecepts;

	public List<HediffDef> nullifyingHediffs;

	public List<GeneDef> nullifyingGenes;

	public List<TraitDef> requiredTraits;

	public List<GeneDef> requiredGenes;

	public int requiredTraitsDegree = int.MinValue;

	public List<HediffDef> requiredHediffs;

	public StatDef effectMultiplyingStat;

	public SimpleCurve effectMultiplyingStatCurve;

	public HediffDef hediff;

	public MentalStateDef mentalState;

	public GameConditionDef gameCondition;

	public bool nullifiedIfNotColonist;

	public ThoughtDef thoughtToMake;

	[NoTranslate]
	private string icon;

	public bool showBubble;

	public ExpectationDef minExpectation;

	public ExpectationDef minExpectationForNegativeThought;

	public List<ThoughtDef> replaceThoughts;

	public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

	public ChemicalDef chemicalDef;

	public bool lerpMoodToZero;

	public int stackLimitForSameOtherPawn = -1;

	public float lerpOpinionToZeroAfterDurationPct = 0.7f;

	public DevelopmentalStage socialTargetDevelopmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;

	public bool ignoreSubhumans = true;

	public bool doNotApplyToQuestLodgers;

	public float maxCumulatedOpinionOffset = float.MaxValue;

	public TaleDef taleDef;

	public Gender gender;

	[MustTranslate]
	public string labelInBracketsExtraForHediff;

	[Unsaved(false)]
	private ThoughtWorker workerInt;

	[Unsaved(false)]
	private BoolUnknown isMemoryCached = BoolUnknown.Unknown;

	private Texture2D iconInt;

	public string Label
	{
		get
		{
			if (!label.NullOrEmpty())
			{
				return label;
			}
			if (!stages.NullOrEmpty())
			{
				if (!stages[0].label.NullOrEmpty())
				{
					return stages[0].label;
				}
				if (!stages[0].labelSocial.NullOrEmpty())
				{
					return stages[0].labelSocial;
				}
			}
			Log.Error("Cannot get good label for ThoughtDef " + defName);
			return defName;
		}
	}

	public override TaggedString LabelCap
	{
		get
		{
			if (Label.NullOrEmpty())
			{
				return null;
			}
			if (cachedLabelCap.NullOrEmpty())
			{
				cachedLabelCap = Label.CapitalizeFirst();
			}
			return cachedLabelCap;
		}
	}

	public int DurationTicks => (int)(durationDays * 60000f);

	public bool IsMemory
	{
		get
		{
			if (isMemoryCached == BoolUnknown.Unknown)
			{
				isMemoryCached = ((!(durationDays > 0f) && !typeof(Thought_Memory).IsAssignableFrom(thoughtClass)) ? BoolUnknown.False : BoolUnknown.True);
			}
			return isMemoryCached == BoolUnknown.True;
		}
	}

	public bool IsSituational => Worker != null;

	public bool IsSocial => typeof(ISocialThought).IsAssignableFrom(ThoughtClass);

	public bool RequiresSpecificTraitsDegree => requiredTraitsDegree != int.MinValue;

	public ThoughtWorker Worker
	{
		get
		{
			if (workerInt == null && workerClass != null)
			{
				workerInt = (ThoughtWorker)Activator.CreateInstance(workerClass);
				workerInt.def = this;
			}
			return workerInt;
		}
	}

	public Type ThoughtClass
	{
		get
		{
			if (thoughtClass != null)
			{
				return thoughtClass;
			}
			if (IsMemory)
			{
				return typeof(Thought_Memory);
			}
			return typeof(Thought_Situational);
		}
	}

	public Texture2D Icon
	{
		get
		{
			if (iconInt == null)
			{
				if (icon == null)
				{
					return null;
				}
				iconInt = ContentFinder<Texture2D>.Get(icon);
			}
			return iconInt;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__69))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__69(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static ThoughtDef Named(string defName)
	{
		return DefDatabase<ThoughtDef>.GetNamed(defName);
	}
}
