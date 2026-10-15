using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;

namespace Verse;

public static class HediffStatsUtility
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__0 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Hediff instance;

		public Hediff _003C_003E3__instance;

		private HediffStage stage;

		public HediffStage _003C_003E3__stage;

		private List<PawnCapacityModifier> _003CcapModsToDisplay_003E5__2;

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
		public _003CSpecialDisplayStats_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CcapModsToDisplay_003E5__2 = null;
			_003CcapMod_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			DamageFactor damageFactor;
			float num3;
			float num4;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (instance != null && instance.Bleeding)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "BleedingRate".Translate(), instance.BleedRateScaled.ToStringPercent() + "/" + "LetterDay".Translate(), "Stat_Hediff_BleedingRate_Desc".Translate(), 4040);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0103;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0103;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0215;
			case 3:
				_003C_003E1__state = -1;
				goto IL_02a4;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0317;
			case 5:
				_003C_003E1__state = -1;
				goto IL_03af;
			case 6:
				_003C_003E1__state = -1;
				goto IL_04fe;
			case 7:
				_003C_003E1__state = -1;
				goto IL_0578;
			case 8:
				_003C_003E1__state = -1;
				goto IL_0623;
			case 9:
				_003C_003E1__state = -1;
				goto IL_0769;
			case 10:
				_003C_003E1__state = -1;
				goto IL_07dd;
			case 11:
				_003C_003E1__state = -1;
				goto IL_085c;
			case 12:
				_003C_003E1__state = -1;
				goto IL_08d0;
			case 13:
				_003C_003E1__state = -1;
				goto IL_094f;
			case 14:
				_003C_003E1__state = -1;
				goto IL_09e2;
			case 15:
				_003C_003E1__state = -1;
				goto IL_0a4c;
			case 16:
				_003C_003E1__state = -1;
				goto IL_0ab8;
			case 17:
				_003C_003E1__state = -1;
				goto IL_0b6a;
			case 18:
				_003C_003E1__state = -1;
				goto IL_0bd6;
			case 19:
				_003C_003E1__state = -1;
				goto IL_0caa;
			case 20:
				_003C_003E1__state = -1;
				goto IL_0d99;
			case 21:
				_003C_003E1__state = -1;
				goto IL_0e9e;
			case 22:
				_003C_003E1__state = -1;
				goto IL_0f8d;
			case 23:
				{
					_003C_003E1__state = -1;
					goto IL_1108;
				}
				IL_0eb0:
				if (_003Ci_003E5__3 < stage.statFactors.Count)
				{
					StatModifier statModifier = stage.statFactors[_003Ci_003E5__3];
					if (statModifier.stat.CanShowWithLoadedMods())
					{
						float num = statModifier.value;
						if (instance != null && instance.CurStage.multiplyStatChangesBySeverity)
						{
							num = StatWorker.ScaleFactor(num, instance.Severity);
						}
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier.stat.LabelCap, statModifier.stat.Worker.ValueToString(num, finalized: false, ToStringNumberSense.Factor), statModifier.stat.description, 4070);
						_003C_003E1__state = 21;
						return true;
					}
					goto IL_0e9e;
				}
				goto IL_0ecb;
				IL_08d0:
				if (stage.restFallFactorOffset != 0f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_TirednessOffset_Name".Translate(), stage.restFallFactorOffset.ToStringSign() + stage.restFallFactorOffset.ToStringPercent(), "Stat_Hediff_TirednessOffset_Desc".Translate(), 4050);
					_003C_003E1__state = 13;
					return true;
				}
				goto IL_094f;
				IL_0103:
				if ((instance != null && instance.pawn.RaceProps.IsFlesh) || (stage != null && instance == null))
				{
					float num2 = 0f;
					if (instance != null)
					{
						num2 = instance.PainOffset;
					}
					else if (stage != null)
					{
						num2 = stage.painOffset;
					}
					if (num2 != 0f)
					{
						if (num2 > 0f && num2 < 0.01f)
						{
							num2 = 0.01f;
						}
						if (num2 < 0f && num2 > -0.01f)
						{
							num2 = -0.01f;
						}
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), $"{num2 * 100f:+###0;-###0}%", "Stat_Hediff_Pain_Desc".Translate(), 4050);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0215;
				}
				goto IL_02a4;
				IL_0f8d:
				_003Ci_003E5__3++;
				goto IL_0f9f;
				IL_063c:
				if (_003Ci_003E5__3 < _003CcapModsToDisplay_003E5__2.Count)
				{
					_003CcapMod_003E5__4 = _003CcapModsToDisplay_003E5__2[_003Ci_003E5__3];
					if (instance?.pawn?.health == null || instance.pawn.health.capacities.CapableOf(_003CcapMod_003E5__4.capacity))
					{
						if (_003CcapMod_003E5__4.offset != 0f)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, _003CcapMod_003E5__4.capacity.GetLabelFor().CapitalizeFirst(), $"{_003CcapMod_003E5__4.offset * 100f:+#;-#}%", _003CcapMod_003E5__4.capacity.description, 4060);
							_003C_003E1__state = 6;
							return true;
						}
						goto IL_04fe;
					}
					goto IL_062a;
				}
				goto IL_0652;
				IL_0fba:
				if (stage.damageFactors.NullOrEmpty())
				{
					break;
				}
				_003Ci_003E5__3 = 0;
				goto IL_111a;
				IL_111a:
				if (_003Ci_003E5__3 >= stage.damageFactors.Count)
				{
					break;
				}
				damageFactor = stage.damageFactors[_003Ci_003E5__3];
				num3 = damageFactor.factor;
				if (instance != null && instance.CurStage.multiplyStatChangesBySeverity)
				{
					num3 = StatWorker.ScaleFactor(num3, instance.Severity);
				}
				if (!(Math.Abs(num3 - 1f) < 0.001f))
				{
					_003C_003E2__current = new StatDrawEntry(label: (!(num3 >= 1f)) ? ((string)"DamageFactorResistance".Translate(damageFactor.damageDef.Named("DAMAGE")).CapitalizeFirst()) : ((string)"DamageFactorWeakness".Translate(damageFactor.damageDef.Named("DAMAGE")).CapitalizeFirst()), category: StatCategoryDefOf.CapacityEffects, valueString: "x" + num3.ToStringPercent(), reportText: "DamageFactor".Translate(damageFactor.damageDef.Named("DAMAGE")).CapitalizeFirst(), displayPriorityWithinCategory: 4075);
					_003C_003E1__state = 23;
					return true;
				}
				goto IL_1108;
				IL_0ab8:
				if (ModsConfig.AnomalyActive && stage.regeneration != -1f && stage.showRegenerationStat)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_Regeneration_Name".Translate(), "Stat_Hediff_Regeneration_Stat".Translate($"{stage.regeneration:0}"), "Stat_Hediff_Regeneration_Desc".Translate(), 4025);
					_003C_003E1__state = 17;
					return true;
				}
				goto IL_0b6a;
				IL_09e2:
				if (stage.totalBleedFactor != 1f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_TotalBleedFactor_Name".Translate(), stage.totalBleedFactor.ToStringPercent(), "Stat_Hediff_TotalBleedFactor_Desc".Translate(), 4041);
					_003C_003E1__state = 15;
					return true;
				}
				goto IL_0a4c;
				IL_0a4c:
				if (stage.naturalHealingFactor != -1f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_NaturalHealingFactor_Name".Translate(), stage.naturalHealingFactor.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Factor), "Stat_Hediff_NaturalHealingFactor_Desc".Translate(), 4020);
					_003C_003E1__state = 16;
					return true;
				}
				goto IL_0ab8;
				IL_0e9e:
				_003Ci_003E5__3++;
				goto IL_0eb0;
				IL_0ecb:
				if (stage.statFactorsBySeverity != null && instance != null)
				{
					_003Ci_003E5__3 = 0;
					goto IL_0f9f;
				}
				goto IL_0fba;
				IL_04fe:
				if (_003CcapMod_003E5__4.postFactor != 1f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, _003CcapMod_003E5__4.capacity.GetLabelFor().CapitalizeFirst(), "x" + _003CcapMod_003E5__4.postFactor.ToStringPercent(), _003CcapMod_003E5__4.capacity.description, 4060);
					_003C_003E1__state = 7;
					return true;
				}
				goto IL_0578;
				IL_0cd7:
				if (stage.statOffsetsBySeverity != null && instance != null)
				{
					_003Ci_003E5__3 = 0;
					goto IL_0dab;
				}
				goto IL_0dc6;
				IL_0578:
				if (_003CcapMod_003E5__4.SetMaxDefined && instance != null)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, _003CcapMod_003E5__4.capacity.GetLabelFor().CapitalizeFirst(), "max".Translate().CapitalizeFirst() + " " + _003CcapMod_003E5__4.EvaluateSetMax(instance.pawn).ToStringPercent(), _003CcapMod_003E5__4.capacity.description, 4060);
					_003C_003E1__state = 8;
					return true;
				}
				goto IL_0623;
				IL_1108:
				_003Ci_003E5__3++;
				goto IL_111a;
				IL_0b6a:
				if (stage.foodPoisoningChanceFactor != 1f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Stat_Hediff_FoodPoisoningChanceFactor_Name".Translate(), stage.foodPoisoningChanceFactor.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Factor), "Stat_Hediff_FoodPoisoningChanceFactor_Desc".Translate(), 4030);
					_003C_003E1__state = 18;
					return true;
				}
				goto IL_0bd6;
				IL_0215:
				num4 = 1f;
				if (instance != null)
				{
					num4 = instance.PainFactor;
				}
				else if (stage != null)
				{
					num4 = stage.painFactor;
				}
				if (num4 != 1f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Pain".Translate(), "x" + num4.ToStringPercent(), "Stat_Hediff_Pain_Desc".Translate(), 4050);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_02a4;
				IL_0623:
				_003CcapMod_003E5__4 = null;
				goto IL_062a;
				IL_062a:
				_003Ci_003E5__3++;
				goto IL_063c;
				IL_0652:
				_003CcapModsToDisplay_003E5__2 = null;
				if (stage == null)
				{
					break;
				}
				if (stage.AffectsMemory || stage.AffectsSocialInteractions)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (stage.AffectsMemory)
					{
						string text = "MemoryLower".Translate();
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							text = text.CapitalizeFirst();
						}
						stringBuilder.Append(text);
					}
					if (stage.AffectsSocialInteractions)
					{
						string text2 = "SocialInteractionsLower".Translate();
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						else
						{
							text2 = text2.CapitalizeFirst();
						}
						stringBuilder.Append(text2);
					}
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Affects".Translate(), stringBuilder.ToString(), "Stat_Hediff_Affects_Desc".Translate(), 4080);
					_003C_003E1__state = 9;
					return true;
				}
				goto IL_0769;
				IL_0bd6:
				if (stage.statOffsets != null)
				{
					_003Ci_003E5__3 = 0;
					goto IL_0cbc;
				}
				goto IL_0cd7;
				IL_0cbc:
				if (_003Ci_003E5__3 < stage.statOffsets.Count)
				{
					StatModifier statModifier2 = stage.statOffsets[_003Ci_003E5__3];
					if (statModifier2.stat.CanShowWithLoadedMods())
					{
						float num5 = statModifier2.value;
						if (instance != null && instance.CurStage.multiplyStatChangesBySeverity)
						{
							num5 *= instance.Severity;
						}
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifier2.stat.LabelCap, statModifier2.stat.Worker.ValueToString(num5, finalized: false, ToStringNumberSense.Offset), statModifier2.stat.description, 4070);
						_003C_003E1__state = 19;
						return true;
					}
					goto IL_0caa;
				}
				goto IL_0cd7;
				IL_02a4:
				if (stage != null && stage.partEfficiencyOffset != 0f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), stage.partEfficiencyOffset.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Offset), "Stat_Hediff_PartEfficiency_Desc".Translate(), 4050);
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0317;
				IL_0f9f:
				if (_003Ci_003E5__3 < stage.statFactorsBySeverity.Count)
				{
					StatModifierBySeverity statModifierBySeverity = stage.statFactorsBySeverity[_003Ci_003E5__3];
					if (statModifierBySeverity.stat.CanShowWithLoadedMods())
					{
						float val = statModifierBySeverity.valueBySeverity.Evaluate(instance.Severity);
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifierBySeverity.stat.LabelCap, statModifierBySeverity.stat.Worker.ValueToString(val, finalized: false, ToStringNumberSense.Factor), statModifierBySeverity.stat.description, 4070);
						_003C_003E1__state = 22;
						return true;
					}
					goto IL_0f8d;
				}
				goto IL_0fba;
				IL_0769:
				if (stage.hungerRateFactor != 1f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "HungerRate".Translate(), "x" + stage.hungerRateFactor.ToStringPercent(), "Stat_Hediff_HungerRateFactor_Desc".Translate(), 4051);
					_003C_003E1__state = 10;
					return true;
				}
				goto IL_07dd;
				IL_0317:
				if (instance?.def.addedPartProps != null && instance.def.addedPartProps.partEfficiency != 1f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), instance.def.addedPartProps.partEfficiency.ToStringByStyle(ToStringStyle.PercentZero), "Stat_Hediff_PartEfficiencyAbsolute_Desc".Translate(), 5000);
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_03af;
				IL_07dd:
				if (stage.hungerRateFactorOffset != 0f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Stat_Hediff_HungerRateOffset_Name".Translate(), stage.hungerRateFactorOffset.ToStringSign() + stage.hungerRateFactorOffset.ToStringPercent(), "Stat_Hediff_HungerRateOffset_Desc".Translate(), 4051);
					_003C_003E1__state = 11;
					return true;
				}
				goto IL_085c;
				IL_0d99:
				_003Ci_003E5__3++;
				goto IL_0dab;
				IL_0caa:
				_003Ci_003E5__3++;
				goto IL_0cbc;
				IL_085c:
				if (stage.restFallFactor != 1f)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "Tiredness".Translate(), "x" + stage.restFallFactor.ToStringPercent(), "Stat_Hediff_TirednessFactor_Desc".Translate(), 4050);
					_003C_003E1__state = 12;
					return true;
				}
				goto IL_08d0;
				IL_0dc6:
				if (stage.statFactors != null)
				{
					_003Ci_003E5__3 = 0;
					goto IL_0eb0;
				}
				goto IL_0ecb;
				IL_03af:
				_003CcapModsToDisplay_003E5__2 = null;
				if (instance != null)
				{
					_003CcapModsToDisplay_003E5__2 = instance.CapMods;
				}
				else if (stage != null)
				{
					_003CcapModsToDisplay_003E5__2 = stage.capMods;
				}
				if (_003CcapModsToDisplay_003E5__2 != null)
				{
					_003Ci_003E5__3 = 0;
					goto IL_063c;
				}
				goto IL_0652;
				IL_0dab:
				if (_003Ci_003E5__3 < stage.statOffsetsBySeverity.Count)
				{
					StatModifierBySeverity statModifierBySeverity2 = stage.statOffsetsBySeverity[_003Ci_003E5__3];
					if (statModifierBySeverity2.stat.CanShowWithLoadedMods())
					{
						float val2 = statModifierBySeverity2.valueBySeverity.Evaluate(instance.Severity);
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, statModifierBySeverity2.stat.LabelCap, statModifierBySeverity2.stat.Worker.ValueToString(val2, finalized: false, ToStringNumberSense.Offset), statModifierBySeverity2.stat.description, 4070);
						_003C_003E1__state = 20;
						return true;
					}
					goto IL_0d99;
				}
				goto IL_0dc6;
				IL_094f:
				if (stage.makeImmuneTo != null)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.CapacityEffects, "PreventsInfection".Translate(), stage.makeImmuneTo.Select((HediffDef im) => im.label).ToCommaList().CapitalizeFirst(), "Stat_Hediff_PreventsInfection_Desc".Translate(), 4050);
					_003C_003E1__state = 14;
					return true;
				}
				goto IL_09e2;
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
			_003CSpecialDisplayStats_003Ed__0 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__0(0);
			}
			_003CSpecialDisplayStats_003Ed__.stage = _003C_003E3__stage;
			_003CSpecialDisplayStats_003Ed__.instance = _003C_003E3__instance;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__0))]
	public static IEnumerable<StatDrawEntry> SpecialDisplayStats(HediffStage stage, Hediff instance)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__0(-2)
		{
			_003C_003E3__stage = stage,
			_003C_003E3__instance = instance
		};
	}

	public static float GetStatOffsetForSeverity(StatDef stat, HediffStage stage, Pawn pawn, float severity)
	{
		float num = stage.statOffsets.GetStatOffsetFromList(stat);
		if (num != 0f)
		{
			if (stage.statOffsetEffectMultiplier != null)
			{
				num *= pawn.GetStatValue(stage.statOffsetEffectMultiplier);
			}
			if (stage.multiplyStatChangesBySeverity)
			{
				num *= severity;
			}
			return num;
		}
		if (stage.statOffsetsBySeverity != null)
		{
			for (int i = 0; i < stage.statOffsetsBySeverity.Count; i++)
			{
				if (stage.statOffsetsBySeverity[i].stat == stat)
				{
					return stage.statOffsetsBySeverity[i].valueBySeverity.Evaluate(severity);
				}
			}
		}
		return 0f;
	}

	public static float GetStatFactorForSeverity(StatDef stat, HediffStage stage, Pawn pawn, float severity)
	{
		float num = stage.statFactors.GetStatFactorFromList(stat);
		if (Math.Abs(num - 1f) > float.Epsilon)
		{
			if (stage.statFactorEffectMultiplier != null)
			{
				num = StatWorker.ScaleFactor(num, pawn.GetStatValue(stage.statFactorEffectMultiplier));
			}
			if (stage.multiplyStatChangesBySeverity)
			{
				num = StatWorker.ScaleFactor(num, severity);
			}
			return num;
		}
		if (stage.statFactorsBySeverity != null)
		{
			for (int i = 0; i < stage.statFactorsBySeverity.Count; i++)
			{
				if (stage.statFactorsBySeverity[i].stat == stat)
				{
					return stage.statFactorsBySeverity[i].valueBySeverity.Evaluate(severity);
				}
			}
		}
		return 1f;
	}
}
