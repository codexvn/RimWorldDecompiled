using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036D9 RID: 14041
	public static class StatExtension
	{
		// Token: 0x060137B4 RID: 79796 RVA: 0x005C91DA File Offset: 0x005C73DA
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true, int cacheStaleAfterTicks = -1)
		{
			return stat.Worker.GetValue(thing, applyPostProcess, cacheStaleAfterTicks);
		}

		// Token: 0x060137B5 RID: 79797 RVA: 0x005C91EA File Offset: 0x005C73EA
		public static float GetStatValueForPawn(this Thing thing, StatDef stat, Pawn pawn, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, pawn, applyPostProcess);
		}

		// Token: 0x060137B6 RID: 79798 RVA: 0x005C91FA File Offset: 0x005C73FA
		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		// Token: 0x060137B7 RID: 79799 RVA: 0x005C9209 File Offset: 0x005C7409
		public static float GetStatValueAbstract(this AbilityDef def, StatDef stat, Pawn forPawn = null)
		{
			return stat.Worker.GetValueAbstract(def, forPawn);
		}

		// Token: 0x060137B8 RID: 79800 RVA: 0x005C9218 File Offset: 0x005C7418
		public static bool StatBaseDefined(this BuildableDef def, StatDef stat)
		{
			if (def.statBases == null)
			{
				return false;
			}
			for (int i = 0; i < def.statBases.Count; i++)
			{
				if (def.statBases[i].stat == stat)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060137B9 RID: 79801 RVA: 0x005C925C File Offset: 0x005C745C
		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}
