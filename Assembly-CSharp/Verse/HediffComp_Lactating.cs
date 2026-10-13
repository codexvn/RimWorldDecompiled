using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006D9 RID: 1753
	[StaticConstructorOnStartup]
	public class HediffComp_Lactating : HediffComp_Chargeable
	{
		// Token: 0x06002B78 RID: 11128 RVA: 0x000E3320 File Offset: 0x000E1520
		public override void TryCharge(float desiredChargeAmount)
		{
			Pawn pawn = base.Pawn;
			bool flag;
			if (pawn == null)
			{
				flag = (null != null);
			}
			else
			{
				Pawn_NeedsTracker needs = pawn.needs;
				flag = (((needs != null) ? needs.food : null) != null);
			}
			if (!flag)
			{
				return;
			}
			desiredChargeAmount *= PawnUtility.BodyResourceGrowthSpeed(base.Pawn);
			float num = Mathf.Min(desiredChargeAmount, base.Pawn.needs.food.CurLevel);
			base.Pawn.needs.food.CurLevel -= num;
			base.TryCharge(num);
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x000E339D File Offset: 0x000E159D
		public float AddedNutritionPerDay()
		{
			return base.Props.fullChargeAmount * 60000f / (float)base.Props.ticksToFullCharge;
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002B7A RID: 11130 RVA: 0x000E33C0 File Offset: 0x000E15C0
		public override string CompTipStringExtra
		{
			get
			{
				if (base.Charge >= base.Props.fullChargeAmount)
				{
					return "LactatingStoppedBecauseFull".Translate();
				}
				float num = PawnUtility.BodyResourceGrowthSpeed(base.Pawn);
				if (num == 0f)
				{
					return "LactatingStoppedBecauseHungry".Translate().Colorize(ColorLibrary.RedReadable);
				}
				float f = this.AddedNutritionPerDay() * num;
				return "LactatingAddedNutritionPerDay".Translate(f.ToStringByStyle(ToStringStyle.FloatMaxTwo, ToStringNumberSense.Absolute), num);
			}
		}
	}
}
