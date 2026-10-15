using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class IngestibleProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__44 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public IngestibleProperties _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__44(int _003C_003E1__state)
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
			IngestibleProperties ingestibleProperties = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (ingestibleProperties.preferability == FoodPreferability.Undefined)
				{
					_003C_003E2__current = "undefined preferability";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_005c;
			case 1:
				_003C_003E1__state = -1;
				goto IL_005c;
			case 2:
				_003C_003E1__state = -1;
				goto IL_007f;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00e5;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0130;
			case 5:
				_003C_003E1__state = -1;
				goto IL_0160;
			case 6:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00e5:
				if (!ingestibleProperties.parent.IsCorpse && (int)ingestibleProperties.preferability > 3 && !ingestibleProperties.parent.socialPropernessMatters && ingestibleProperties.parent.EverHaulable)
				{
					_003C_003E2__current = "ingestible preferability > DesperateOnlyForHumanlikes but socialPropernessMatters=false. This will cause bugs wherein wardens will look in prison cells for food to give to prisoners and so will repeatedly pick up and drop food inside the cell.";
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0130;
				IL_005c:
				if (ingestibleProperties.foodType == FoodTypeFlags.None)
				{
					_003C_003E2__current = "no foodType";
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_007f;
				IL_007f:
				if (ingestibleProperties.parent.GetStatValueAbstract(StatDefOf.Nutrition) == 0f && ingestibleProperties.preferability != FoodPreferability.NeverForNutrition)
				{
					_003C_003E2__current = "Nutrition == 0 but preferability is " + ingestibleProperties.preferability.ToString() + " instead of " + FoodPreferability.NeverForNutrition;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00e5;
				IL_0130:
				if (ingestibleProperties.joy > 0f && ingestibleProperties.joyKind == null)
				{
					_003C_003E2__current = "joy > 0 with no joy kind";
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_0160;
				IL_0160:
				if (ingestibleProperties.joy == 0f && ingestibleProperties.joyKind != null)
				{
					_003C_003E2__current = "joy is 0 but joyKind is " + ingestibleProperties.joyKind;
					_003C_003E1__state = 6;
					return true;
				}
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
			_003CConfigErrors_003Ed__44 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__44(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__46 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public IngestibleProperties _003C_003E4__this;

		private int _003Ci_003E5__2;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap2;

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
		public _003CSpecialDisplayStats_003Ed__46(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 4)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				IngestibleProperties ingestibleProperties = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (ingestibleProperties.joy > 0f)
					{
						StatCategoryDef category = ((ingestibleProperties.drugCategory != 0) ? StatCategoryDefOf.Drug : StatCategoryDefOf.Basics);
						_003C_003E2__current = new StatDrawEntry(category, "Joy".Translate(), ingestibleProperties.joy.ToStringPercent("F0") + " (" + ingestibleProperties.JoyKind.label + ")", "Stat_Thing_Ingestible_Joy_Desc".Translate(), 4751);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00cd;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00cd;
				case 2:
					_003C_003E1__state = -1;
					goto IL_014c;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01b9;
				case 4:
					{
						_003C_003E1__state = -3;
						goto IL_0229;
					}
					IL_0229:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003Ci_003E5__2++;
					goto IL_0255;
					IL_014c:
					if (ingestibleProperties.drugCategory != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "DrugCategory".Translate().CapitalizeFirst(), ingestibleProperties.drugCategory.GetLabel().CapitalizeFirst(), "Stat_Thing_Drug_Category_Desc".Translate(), 2485);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01b9;
					IL_0255:
					if (_003Ci_003E5__2 >= ingestibleProperties.outcomeDoers.Count)
					{
						break;
					}
					_003C_003E7__wrap2 = ingestibleProperties.outcomeDoers[_003Ci_003E5__2].SpecialDisplayStats(ingestibleProperties.parent).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0229;
					IL_00cd:
					if (ingestibleProperties.HumanEdible && ingestibleProperties.parent.GetStatValueAbstract(StatDefOf.Nutrition) > 0f)
					{
						RoyalTitleDef royalTitleDef = ingestibleProperties.MaxSatisfiedTitle();
						if (royalTitleDef != null)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Thing_Ingestible_MaxSatisfiedTitle".Translate(), royalTitleDef.GetLabelCapForBothGenders(), "Stat_Thing_Ingestible_MaxSatisfiedTitle_Desc".Translate(), 4752);
							_003C_003E1__state = 2;
							return true;
						}
					}
					goto IL_014c;
					IL_01b9:
					if (ingestibleProperties.outcomeDoers == null)
					{
						break;
					}
					_003Ci_003E5__2 = 0;
					goto IL_0255;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__46 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__46(0)
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

	[Unsaved(false)]
	public ThingDef parent;

	public int maxNumToIngestAtOnce;

	public int defaultNumToIngestAtOnce = 20;

	public List<IngestionOutcomeDoer> outcomeDoers;

	public int baseIngestTicks = 500;

	public float chairSearchRadius = 32f;

	public bool useEatingSpeedStat = true;

	public bool babiesCanIngest;

	public bool humanlikeOnly;

	public bool nonDrugIngestibleWithoutFoodNeed;

	public ThoughtDef tasteThought;

	public ThoughtDef specialThoughtDirect;

	public ThoughtDef specialThoughtAsIngredient;

	public HistoryEventDef ateEvent;

	public EffecterDef ingestEffect;

	public EffecterDef ingestEffectEat;

	public SoundDef ingestSound;

	[MustTranslate]
	public string ingestCommandString;

	[MustTranslate]
	public string ingestReportString;

	[MustTranslate]
	public string ingestReportStringEat;

	public HoldOffsetSet ingestHoldOffsetStanding;

	public bool ingestHoldUsesTable = true;

	public bool tableDesired = true;

	public bool showIngestFloatOption = true;

	public FoodTypeFlags foodType;

	public float joy;

	public JoyKindDef joyKind;

	public ThingDef sourceDef;

	public FoodPreferability preferability;

	public bool nurseable;

	public float optimalityOffsetHumanlikes;

	public float optimalityOffsetFeedingAnimals;

	public DrugCategory drugCategory;

	public bool canAutoSelectAsFoodForCaravan = true;

	public bool lowPriorityCaravanFood;

	[Unsaved(false)]
	private float cachedNutrition = -1f;

	public JoyKindDef JoyKind => joyKind ?? JoyKindDefOf.Gluttonous;

	public bool HumanEdible => (FoodTypeFlags.OmnivoreHuman & foodType) != 0;

	public bool IsMeal
	{
		get
		{
			if ((int)preferability >= 7)
			{
				return (int)preferability <= 10;
			}
			return false;
		}
	}

	public float CachedNutrition
	{
		get
		{
			if (cachedNutrition == -1f)
			{
				cachedNutrition = parent.GetStatValueAbstract(StatDefOf.Nutrition);
			}
			return cachedNutrition;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__44))]
	public IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__44(-2)
		{
			_003C_003E4__this = this
		};
	}

	public RoyalTitleDef MaxSatisfiedTitle()
	{
		return (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.foodRequirement.Defined && t.foodRequirement.Acceptable(parent)
			orderby t.seniority descending
			select t).FirstOrDefault();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__46))]
	internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__46(-2)
		{
			_003C_003E4__this = this
		};
	}

	private bool TryGetOutcomeDoer<T>(out T doer) where T : IngestionOutcomeDoer
	{
		doer = null;
		if (outcomeDoers == null)
		{
			return false;
		}
		foreach (IngestionOutcomeDoer outcomeDoer in outcomeDoers)
		{
			if (outcomeDoer is T val)
			{
				doer = val;
				return true;
			}
		}
		return false;
	}
}
