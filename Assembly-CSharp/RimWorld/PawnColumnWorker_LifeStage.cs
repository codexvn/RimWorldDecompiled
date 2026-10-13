using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003442 RID: 13378
	public class PawnColumnWorker_LifeStage : PawnColumnWorker_Icon
	{
		// Token: 0x06012D22 RID: 77090 RVA: 0x00595310 File Offset: 0x00593510
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			return pawn.ageTracker.CurLifeStageRace.GetIcon(pawn);
		}

		// Token: 0x06012D23 RID: 77091 RVA: 0x00595324 File Offset: 0x00593524
		protected override string GetIconTip(Pawn pawn)
		{
			int num = Mathf.FloorToInt(pawn.ageTracker.AgeBiologicalYearsFloat);
			int num2 = Mathf.FloorToInt(pawn.ageTracker.AgeBiologicalYearsFloat * 60f);
			num2 -= num * 60;
			string text = "";
			if (num > 0)
			{
				if (num == 1)
				{
					text += "Period1Year".Translate();
				}
				else
				{
					text += "PeriodYears".Translate(num);
				}
				text += ", ";
			}
			if (num2 > 0)
			{
				if (num2 == 1)
				{
					text += "Period1Day".Translate();
				}
				else
				{
					text += "PeriodDays".Translate(num2);
				}
			}
			else if (num <= 0)
			{
				text += "PeriodHours".Translate(((float)pawn.ageTracker.AgeBiologicalTicks / 2500f).ToString("0.0"));
			}
			return pawn.ageTracker.CurLifeStage.LabelCap + " (" + text.TrimEnd(new char[]
			{
				',',
				' '
			}) + ")";
		}

		// Token: 0x06012D24 RID: 77092 RVA: 0x00595470 File Offset: 0x00593670
		public override int Compare(Pawn a, Pawn b)
		{
			return a.ageTracker.AgeBiologicalTicks.CompareTo(b.ageTracker.AgeBiologicalTicks);
		}
	}
}
