using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CA5 RID: 11429
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x1700288B RID: 10379
		// (get) Token: 0x060100A5 RID: 65701 RVA: 0x004A4E7E File Offset: 0x004A307E
		public float PoisonPercent
		{
			get
			{
				return this.poisonPct;
			}
		}

		// Token: 0x1700288C RID: 10380
		// (get) Token: 0x060100A6 RID: 65702 RVA: 0x004A4E86 File Offset: 0x004A3086
		public FoodPoisonCause Cause
		{
			get
			{
				return this.cause;
			}
		}

		// Token: 0x060100A7 RID: 65703 RVA: 0x004A4E8E File Offset: 0x004A308E
		public void SetPoisoned(FoodPoisonCause newCause)
		{
			this.poisonPct = 1f;
			this.cause = newCause;
		}

		// Token: 0x060100A8 RID: 65704 RVA: 0x004A4EA2 File Offset: 0x004A30A2
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
			Scribe_Values.Look<FoodPoisonCause>(ref this.cause, "cause", FoodPoisonCause.Unknown, false);
		}

		// Token: 0x060100A9 RID: 65705 RVA: 0x004A4ED2 File Offset: 0x004A30D2
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
			compFoodPoisonable.cause = this.cause;
		}

		// Token: 0x060100AA RID: 65706 RVA: 0x004A4EF8 File Offset: 0x004A30F8
		public override void Notify_RecipeProduced(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (Rand.Chance((room != null) ? room.GetStat(RoomStatDefOf.FoodPoisonChance) : RoomStatDefOf.FoodPoisonChance.roomlessScore))
			{
				this.SetPoisoned(FoodPoisonCause.FilthyKitchen);
				return;
			}
			if (Rand.Chance(pawn.GetStatValue(StatDefOf.FoodPoisonChance, true, -1)))
			{
				this.SetPoisoned(FoodPoisonCause.IncompetentCook);
			}
		}

		// Token: 0x060100AB RID: 65707 RVA: 0x004A4F54 File Offset: 0x004A3154
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompFoodPoisonable compFoodPoisonable = otherStack.TryGetComp<CompFoodPoisonable>();
			if (this.cause == FoodPoisonCause.Unknown && compFoodPoisonable.cause != FoodPoisonCause.Unknown)
			{
				this.cause = compFoodPoisonable.cause;
			}
			else if (compFoodPoisonable.cause != FoodPoisonCause.Unknown && this.cause != FoodPoisonCause.Unknown)
			{
				float num = this.poisonPct * (float)this.parent.stackCount;
				float num2 = compFoodPoisonable.poisonPct * (float)count;
				this.cause = ((num > num2) ? this.cause : compFoodPoisonable.cause);
			}
			this.poisonPct = GenMath.WeightedAverage(this.poisonPct, (float)this.parent.stackCount, compFoodPoisonable.poisonPct, (float)count);
		}

		// Token: 0x060100AC RID: 65708 RVA: 0x004A4FF9 File Offset: 0x004A31F9
		public override void PostIngested(Pawn ingester)
		{
			if (Rand.Chance(this.poisonPct * FoodUtility.GetFoodPoisonChanceFactor(ingester)))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, this.cause);
			}
		}

		// Token: 0x0400A7BD RID: 42941
		private float poisonPct;

		// Token: 0x0400A7BE RID: 42942
		private FoodPoisonCause cause;
	}
}
