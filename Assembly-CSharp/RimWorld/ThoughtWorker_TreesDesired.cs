using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001828 RID: 6184
	public class ThoughtWorker_TreesDesired : ThoughtWorker_Precept
	{
		// Token: 0x06008F14 RID: 36628 RVA: 0x002A4D2E File Offset: 0x002A2F2E
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.surroundings == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageIndex(p));
		}

		// Token: 0x06008F15 RID: 36629 RVA: 0x002A4D4C File Offset: 0x002A2F4C
		private int ThoughtStageIndex(Pawn p)
		{
			ThoughtWorker_TreesDesired.<>c__DisplayClass1_0 CS$<>8__locals1;
			CS$<>8__locals1.p = p;
			int num = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Super, 2500, ref CS$<>8__locals1);
			if (num > 1)
			{
				return 0;
			}
			if (num == 1)
			{
				return 1;
			}
			int num2 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Super, 15000, ref CS$<>8__locals1);
			int num3 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Full, 15000, ref CS$<>8__locals1);
			if (num2 >= 1 && num3 >= 3)
			{
				return 2;
			}
			if (num2 >= 1)
			{
				return 3;
			}
			if (num3 >= 5)
			{
				return 4;
			}
			if (num3 >= 3)
			{
				return 5;
			}
			int num4 = ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Mini, 30000, ref CS$<>8__locals1);
			if (num3 >= 1 && num4 >= 1)
			{
				return 6;
			}
			if (num3 == 1)
			{
				return 7;
			}
			if (num4 >= 5)
			{
				return 8;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Mini, 60000, ref CS$<>8__locals1) >= 1)
			{
				return 9;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|1_1(120000, ref CS$<>8__locals1))
			{
				return 10;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|1_1(180000, ref CS$<>8__locals1))
			{
				return 11;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|1_1(240000, ref CS$<>8__locals1))
			{
				return 12;
			}
			if (ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__AnySightingsInRange|1_1(300000, ref CS$<>8__locals1))
			{
				return 13;
			}
			return 14;
		}

		// Token: 0x06008F17 RID: 36631 RVA: 0x002A4E33 File Offset: 0x002A3033
		[CompilerGenerated]
		internal static int <ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory category, int ticks, ref ThoughtWorker_TreesDesired.<>c__DisplayClass1_0 A_2)
		{
			return A_2.p.surroundings.NumSightingsInRange(category, ticks);
		}

		// Token: 0x06008F18 RID: 36632 RVA: 0x002A4E47 File Offset: 0x002A3047
		[CompilerGenerated]
		internal static bool <ThoughtStageIndex>g__AnySightingsInRange|1_1(int ticks, ref ThoughtWorker_TreesDesired.<>c__DisplayClass1_0 A_1)
		{
			return ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Super, ticks, ref A_1) > 0 || ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Full, ticks, ref A_1) > 0 || ThoughtWorker_TreesDesired.<ThoughtStageIndex>g__SightingsInRange|1_0(TreeCategory.Mini, ticks, ref A_1) > 0;
		}
	}
}
