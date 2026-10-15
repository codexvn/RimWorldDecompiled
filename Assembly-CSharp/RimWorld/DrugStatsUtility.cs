using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class DrugStatsUtility
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__15 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private CompProperties_Drug _003CdrugComp_003E5__2;

		private IngestionOutcomeDoer_GiveHediff _003ChighGiver_003E5__3;

		private float _003ChighFall_003E5__4;

		private HediffDef _003CaddictionHediff_003E5__5;

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
		public _003CSpecialDisplayStats_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CdrugComp_003E5__2 = null;
			_003ChighGiver_003E5__3 = null;
			_003CaddictionHediff_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			float num;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003CdrugComp_003E5__2 = GetDrugComp(def);
				if (_003CdrugComp_003E5__2 == null)
				{
					return false;
				}
				_003ChighGiver_003E5__3 = GetDrugHighGiver(def);
				if (_003ChighGiver_003E5__3 != null)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "HighGain".Translate(), _003ChighGiver_003E5__3.severity.ToStringPercent(), "Stat_Thing_Drug_HighGainPerDose_Desc".Translate(), 2480);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_01f2;
			case 1:
				_003C_003E1__state = -1;
				_003ChighFall_003E5__4 = Mathf.Abs(GetHighOffsetPerDay(def));
				if (_003ChighFall_003E5__4 > 0f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "HighFallRate".Translate(), "PerDay".Translate(_003ChighFall_003E5__4.ToStringPercent()), "Stat_Thing_Drug_HighFallPerDay_Desc".Translate(), 2470);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_01f2;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "HighDuration".Translate(), "PeriodDays".Translate((_003ChighGiver_003E5__3.severity / _003ChighFall_003E5__4).ToString("F1")), "Stat_Thing_Drug_HighDurationPerDose_Desc".Translate(), 2460);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				goto IL_01f2;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0263;
			case 5:
				_003C_003E1__state = -1;
				goto IL_02e0;
			case 6:
				_003C_003E1__state = -1;
				goto IL_03aa;
			case 7:
				_003C_003E1__state = -1;
				goto IL_0401;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionCost".Translate(), "PerDay".Translate(GetAddictionNeedCostPerDay(def).ToStringMoney()), "Stat_Thing_Drug_AddictionCost_Desc".Translate(), 2390);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedDoseInterval".Translate(), "PeriodDays".Translate((_003CdrugComp_003E5__2.needLevelOffset / GetNeed(def).fallPerDay).ToString("F1")), "Stat_Thing_Drug_AddictionNeedDoseInterval_Desc".Translate(), 2400);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				if (_003CdrugComp_003E5__2.chemical != null)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "Chemical".Translate(), _003CdrugComp_003E5__2.chemical.LabelCap, "Stat_Thing_Drug_Chemical_Desc".Translate(), 2490);
					_003C_003E1__state = 11;
					return true;
				}
				goto IL_05cd;
			case 11:
				_003C_003E1__state = -1;
				goto IL_05cd;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedOffset".Translate(), _003CdrugComp_003E5__2.needLevelOffset.ToStringPercent(), "Stat_Thing_Drug_AddictionNeedOffset_Desc".Translate(), 2420);
				_003C_003E1__state = 13;
				return true;
			case 13:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "MinimumToleranceForAddiction".Translate(), _003CdrugComp_003E5__2.minToleranceToAddict.ToStringPercent(), "Stat_Thing_Drug_MinToleranceForAddiction_Desc".Translate(), 2437);
				_003C_003E1__state = 14;
				return true;
			case 14:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionSeverityPerDose".Translate(), _003CdrugComp_003E5__2.existingAddictionSeverityOffset.ToStringPercent(), "Stat_Thing_Drug_AddictionSeverityPerDose_Desc".Translate(), 2424);
				_003C_003E1__state = 15;
				return true;
			case 15:
				_003C_003E1__state = -1;
				_003CaddictionHediff_003E5__5 = null;
				goto IL_0734;
			case 16:
			{
				_003C_003E1__state = -1;
				string text = "Stat_Thing_Drug_SafeDoseInterval_Desc".Translate();
				if (ModsConfig.BiotechActive && GetSafeDoseInterval(def) > 0f)
				{
					IngestionOutcomeDoer_GiveHediff toleranceGiver = GetToleranceGiver(def);
					if (((toleranceGiver != null) ? (GetDrugComp(def).minToleranceToAddict / toleranceGiver.severity) : 0f) >= 1f)
					{
						for (int i = 0; i < ThingDefOf.Human.race.lifeStageAges.Count; i++)
						{
							LifeStageAge lifeStageAge = ThingDefOf.Human.race.lifeStageAges[i];
							string text2 = ((i < ThingDefOf.Human.race.lifeStageAges.Count - 1) ? "AgeXToY".Translate(lifeStageAge.minAge, ThingDefOf.Human.race.lifeStageAges[i + 1].minAge - 1f) : "AgePlus".Translate(lifeStageAge.minAge));
							text += "\n  - " + lifeStageAge.def.LabelCap + " (" + text2 + ")" + ": " + "PeriodDays".Translate(GetSafeDoseInterval(def, lifeStageAge.def.bodySizeFactor).ToString("F1"));
						}
					}
				}
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "SafeDoseInterval".Translate(), GetSafeDoseIntervalReadout(def), text, 2435);
				_003C_003E1__state = 17;
				return true;
			}
			case 17:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0263:
				num = Mathf.Abs(GetToleranceOffsetPerDay(def));
				if (num > 0f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "ToleranceFallRate".Translate(), "PerDay".Translate(num.ToStringPercent()), "Stat_Thing_Drug_ToleranceFallPerDay_Desc".Translate(), 2440);
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_02e0;
				IL_02e0:
				if (_003CdrugComp_003E5__2.Addictive)
				{
					_003CaddictionHediff_003E5__5 = GetChemical(def)?.addictionHediff;
					if (_003CaddictionHediff_003E5__5 != null)
					{
						float num2 = Mathf.Abs(GetAddictionOffsetPerDay(def));
						if (num2 > 0f)
						{
							float num3 = _003CaddictionHediff_003E5__5.initialSeverity / num2;
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionRecoveryTime".Translate(), "PeriodDays".Translate(num3.ToString("F1")), "Stat_Thing_Drug_AddictionRecoveryTime_Desc".Translate(), 2395);
							_003C_003E1__state = 6;
							return true;
						}
						goto IL_03aa;
					}
					goto IL_0401;
				}
				goto IL_0734;
				IL_0734:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "RandomODChance".Translate(), _003CdrugComp_003E5__2.largeOverdoseChance.ToStringPercent(), "Stat_Thing_Drug_RandomODChance_Desc".Translate(), 2380);
				_003C_003E1__state = 16;
				return true;
				IL_01f2:
				if (GetTolerance(def) != null)
				{
					float toleranceGain = GetToleranceGain(def);
					if (toleranceGain > 0f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Drug, "ToleranceGain".Translate(), toleranceGain.ToStringPercent(), "Stat_Thing_Drug_ToleranceGainPerDose_Desc".Translate(), 2450);
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0263;
				}
				goto IL_02e0;
				IL_03aa:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionSeverityInitial".Translate(), _003CaddictionHediff_003E5__5.initialSeverity.ToStringPercent(), "Stat_Thing_Drug_AddictionSeverityInitial_Desc".Translate(), 2427);
				_003C_003E1__state = 7;
				return true;
				IL_0401:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "AddictionNeedFallRate".Translate(), "PerDay".Translate(GetNeed(def).fallPerDay.ToStringPercent()), "Stat_Thing_Drug_AddictionNeedFallRate_Desc".Translate(), 2410);
				_003C_003E1__state = 8;
				return true;
				IL_05cd:
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.DrugAddiction, "Addictiveness".Translate(), _003CdrugComp_003E5__2.addictiveness.ToStringPercent(), "Stat_Thing_Drug_Addictiveness_Desc".Translate(), 2428);
				_003C_003E1__state = 12;
				return true;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__15 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__15(0);
			}
			_003CSpecialDisplayStats_003Ed__.def = _003C_003E3__def;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public static readonly SimpleCurve ToleranceToAddictivenessFactorCurve = new SimpleCurve
	{
		new CurvePoint(0f, 1f),
		new CurvePoint(0.5f, 5f),
		new CurvePoint(0.8f, 15f)
	};

	public static CompProperties_Drug GetDrugComp(ThingDef d)
	{
		return d.GetCompProperties<CompProperties_Drug>();
	}

	public static ChemicalDef GetChemical(ThingDef d)
	{
		return GetDrugComp(d)?.chemical;
	}

	public static NeedDef GetNeed(ThingDef d)
	{
		return GetChemical(d)?.addictionHediff?.chemicalNeed;
	}

	public static HediffDef GetTolerance(ThingDef d)
	{
		return GetChemical(d)?.toleranceHediff;
	}

	public static float GetAddictivenessAtTolerance(ThingDef d, float tolerance)
	{
		float num = GetDrugComp(d).addictiveness;
		if (GetTolerance(d) != null && tolerance > 0f)
		{
			num *= ToleranceToAddictivenessFactorCurve.Evaluate(tolerance);
		}
		return num;
	}

	public static IngestionOutcomeDoer_GiveHediff GetDrugHighGiver(ThingDef d)
	{
		if (d.ingestible == null || d.ingestible.outcomeDoers == null)
		{
			return null;
		}
		foreach (IngestionOutcomeDoer outcomeDoer in d.ingestible.outcomeDoers)
		{
			if (outcomeDoer is IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff && typeof(Hediff_High).IsAssignableFrom(ingestionOutcomeDoer_GiveHediff.hediffDef.hediffClass))
			{
				return ingestionOutcomeDoer_GiveHediff;
			}
		}
		return null;
	}

	public static IngestionOutcomeDoer_GiveHediff GetToleranceGiver(ThingDef d)
	{
		if (d.ingestible == null || d.ingestible.outcomeDoers == null)
		{
			return null;
		}
		foreach (IngestionOutcomeDoer outcomeDoer in d.ingestible.outcomeDoers)
		{
			if (outcomeDoer is IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff && ingestionOutcomeDoer_GiveHediff.hediffDef == GetTolerance(d))
			{
				return ingestionOutcomeDoer_GiveHediff;
			}
		}
		return null;
	}

	public static float GetHighOffsetPerDay(ThingDef d)
	{
		IngestionOutcomeDoer_GiveHediff drugHighGiver = GetDrugHighGiver(d);
		if (drugHighGiver == null)
		{
			return 0f;
		}
		return drugHighGiver.hediffDef.CompProps<HediffCompProperties_SeverityPerDay>()?.severityPerDay ?? 0f;
	}

	public static float GetToleranceGain(ThingDef d)
	{
		if (d.ingestible == null || d.ingestible.outcomeDoers == null)
		{
			return 0f;
		}
		HediffDef tolerance = GetTolerance(d);
		if (tolerance != null)
		{
			foreach (IngestionOutcomeDoer outcomeDoer in d.ingestible.outcomeDoers)
			{
				if (outcomeDoer is IngestionOutcomeDoer_GiveHediff ingestionOutcomeDoer_GiveHediff && ingestionOutcomeDoer_GiveHediff.hediffDef == tolerance)
				{
					return ingestionOutcomeDoer_GiveHediff.severity;
				}
			}
		}
		return 0f;
	}

	public static float GetToleranceOffsetPerDay(ThingDef d)
	{
		HediffDef tolerance = GetTolerance(d);
		if (tolerance == null)
		{
			return 0f;
		}
		return tolerance.CompProps<HediffCompProperties_SeverityPerDay>()?.severityPerDay ?? 0f;
	}

	public static float GetAddictionOffsetPerDay(ThingDef d)
	{
		HediffDef hediffDef = GetChemical(d)?.addictionHediff;
		if (hediffDef == null)
		{
			return 0f;
		}
		return hediffDef.CompProps<HediffCompProperties_SeverityPerDay>()?.severityPerDay ?? 0f;
	}

	public static float GetAddictionNeedCostPerDay(ThingDef d)
	{
		NeedDef need = GetNeed(d);
		if (need != null)
		{
			return d.BaseMarketValue * need.fallPerDay * (1f + (1f - GetDrugComp(d).needLevelOffset));
		}
		return 0f;
	}

	public static float GetSafeDoseInterval(ThingDef d, float bodySizeFactor = 1f)
	{
		CompProperties_Drug drugComp = GetDrugComp(d);
		if (drugComp == null || !drugComp.Addictive)
		{
			return 0f;
		}
		if (drugComp.addictiveness >= 1f || GetToleranceGiver(d) == null)
		{
			return -1f;
		}
		if (bodySizeFactor == 0f)
		{
			return -1f;
		}
		float num = Mathf.Abs(GetToleranceOffsetPerDay(d));
		return Mathf.Max(drugComp.overdoseSeverityOffset.TrueMax, (num > 0f) ? (GetToleranceGiver(d).severity / num) : (-1f)) / bodySizeFactor;
	}

	public static string GetSafeDoseIntervalReadout(ThingDef d)
	{
		IngestionOutcomeDoer_GiveHediff toleranceGiver = GetToleranceGiver(d);
		float safeDoseInterval = GetSafeDoseInterval(d);
		float num = ((toleranceGiver != null) ? (GetDrugComp(d).minToleranceToAddict / toleranceGiver.severity) : 0f);
		if (safeDoseInterval == 0f)
		{
			return "AlwaysSafe".Translate();
		}
		if (num < 1f)
		{
			return "NeverSafe".Translate();
		}
		return "PeriodDays".Translate(safeDoseInterval.ToString("F1"));
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__15))]
	public static IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef def)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__15(-2)
		{
			_003C_003E3__def = def
		};
	}
}
