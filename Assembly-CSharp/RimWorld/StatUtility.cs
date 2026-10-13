using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003734 RID: 14132
	public static class StatUtility
	{
		// Token: 0x06013966 RID: 80230 RVA: 0x005CE5C4 File Offset: 0x005CC7C4
		public static void SetStatValueInList(ref List<StatModifier> statList, StatDef stat, float value)
		{
			if (statList == null)
			{
				statList = new List<StatModifier>();
			}
			for (int i = 0; i < statList.Count; i++)
			{
				if (statList[i].stat == stat)
				{
					statList[i].value = value;
					return;
				}
			}
			StatModifier statModifier = new StatModifier();
			statModifier.stat = stat;
			statModifier.value = value;
			statList.Add(statModifier);
		}

		// Token: 0x06013967 RID: 80231 RVA: 0x005CE629 File Offset: 0x005CC829
		public static float GetStatFactorFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 1f);
		}

		// Token: 0x06013968 RID: 80232 RVA: 0x005CE637 File Offset: 0x005CC837
		public static float GetStatFactorFromList(this List<StatModifierQuality> modList, StatDef stat, QualityCategory qc)
		{
			return modList.GetStatValueFromList(stat, qc, 1f);
		}

		// Token: 0x06013969 RID: 80233 RVA: 0x005CE646 File Offset: 0x005CC846
		public static bool TryGetStatFactorRangeFromList(this List<StatModifierQuality> modList, StatDef stat, out FloatRange range)
		{
			return modList.TryGetStatValueRangeFromList(stat, out range);
		}

		// Token: 0x0601396A RID: 80234 RVA: 0x005CE650 File Offset: 0x005CC850
		public static float GetStatOffsetFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 0f);
		}

		// Token: 0x0601396B RID: 80235 RVA: 0x005CE65E File Offset: 0x005CC85E
		public static float GetStatOffsetFromList(this List<StatModifierQuality> modList, StatDef stat, QualityCategory qc)
		{
			return modList.GetStatValueFromList(stat, qc, 0f);
		}

		// Token: 0x0601396C RID: 80236 RVA: 0x005CE646 File Offset: 0x005CC846
		public static bool TryGetStatOffsetRangeFromList(this List<StatModifierQuality> modList, StatDef stat, out FloatRange range)
		{
			return modList.TryGetStatValueRangeFromList(stat, out range);
		}

		// Token: 0x0601396D RID: 80237 RVA: 0x005CE670 File Offset: 0x005CC870
		public static float GetStatValueFromList(this List<StatModifier> modList, StatDef stat, float defaultValue)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
					{
						return modList[i].value;
					}
				}
			}
			return defaultValue;
		}

		// Token: 0x0601396E RID: 80238 RVA: 0x005CE6B0 File Offset: 0x005CC8B0
		public static float GetStatValueFromList(this List<StatModifierQuality> modList, StatDef stat, QualityCategory qc, float defaultValue)
		{
			if (modList != null)
			{
				foreach (StatModifierQuality statModifierQuality in modList)
				{
					if (statModifierQuality.stat == stat)
					{
						return statModifierQuality.GetValue(qc);
					}
				}
				return defaultValue;
			}
			return defaultValue;
		}

		// Token: 0x0601396F RID: 80239 RVA: 0x005CE710 File Offset: 0x005CC910
		public static bool TryGetStatValueRangeFromList(this List<StatModifierQuality> modList, StatDef stat, out FloatRange range)
		{
			if (modList != null)
			{
				foreach (StatModifierQuality statModifierQuality in modList)
				{
					if (statModifierQuality.stat == stat)
					{
						range = new FloatRange(statModifierQuality.GetValue(QualityCategory.Awful), statModifierQuality.GetValue(QualityCategory.Legendary));
						return true;
					}
				}
			}
			range = default(FloatRange);
			return false;
		}

		// Token: 0x06013970 RID: 80240 RVA: 0x005CE78C File Offset: 0x005CC98C
		public static bool StatListContains(this List<StatModifier> modList, StatDef stat)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06013971 RID: 80241 RVA: 0x005CE7C0 File Offset: 0x005CC9C0
		public static string GetOffsetsAndFactorsFor(StatDef stat, Thing thing)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StatRequest statRequest = StatRequest.For(thing);
			float baseValueFor = stat.Worker.GetBaseValueFor(statRequest);
			ToStringNumberSense toStringNumberSense = stat.toStringNumberSense;
			stat.Worker.GetOffsetsAndFactorsExplanation(statRequest, stringBuilder, baseValueFor, "    ");
			stat.Worker.GetAdditionalOffsetsAndFactorsExplanation(statRequest, toStringNumberSense, stringBuilder, "    ");
			return stringBuilder.ToString();
		}
	}
}
