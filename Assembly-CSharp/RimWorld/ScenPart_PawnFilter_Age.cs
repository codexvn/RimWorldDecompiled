using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020028C4 RID: 10436
	public class ScenPart_PawnFilter_Age : ScenPart
	{
		// Token: 0x0600E440 RID: 58432 RVA: 0x0042ABFB File Offset: 0x00428DFB
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.IntRange(listing.GetScenPartRect(this, 31f), (int)listing.CurHeight, ref this.allowedAgeRange, 15, 120, null, 4);
		}

		// Token: 0x0600E441 RID: 58433 RVA: 0x0042AC24 File Offset: 0x00428E24
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntRange>(ref this.allowedAgeRange, "allowedAgeRange", default(IntRange), false);
		}

		// Token: 0x0600E442 RID: 58434 RVA: 0x0042AC54 File Offset: 0x00428E54
		public override string Summary(Scenario scen)
		{
			if (this.allowedAgeRange.min > 15)
			{
				if (this.allowedAgeRange.max < 10000)
				{
					return "ScenPart_StartingPawnAgeRange".Translate(this.allowedAgeRange.min, this.allowedAgeRange.max);
				}
				return "ScenPart_StartingPawnAgeMin".Translate(this.allowedAgeRange.min);
			}
			else
			{
				if (this.allowedAgeRange.max < 10000)
				{
					return "ScenPart_StartingPawnAgeMax".Translate(this.allowedAgeRange.max);
				}
				throw new Exception();
			}
		}

		// Token: 0x0600E443 RID: 58435 RVA: 0x0042AD09 File Offset: 0x00428F09
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			return this.allowedAgeRange.Includes(pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x0600E444 RID: 58436 RVA: 0x0042AD24 File Offset: 0x00428F24
		public override void Randomize()
		{
			this.allowedAgeRange = new IntRange(15, 120);
			switch (Rand.RangeInclusive(0, 2))
			{
			case 0:
				this.allowedAgeRange.min = Rand.Range(20, 60);
				break;
			case 1:
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			case 2:
				this.allowedAgeRange.min = Rand.Range(20, 60);
				this.allowedAgeRange.max = Rand.Range(20, 60);
				break;
			}
			this.MakeAllowedAgeRangeValid();
		}

		// Token: 0x0600E445 RID: 58437 RVA: 0x0042ADB8 File Offset: 0x00428FB8
		private void MakeAllowedAgeRangeValid()
		{
			if (this.allowedAgeRange.max < 19)
			{
				this.allowedAgeRange.max = 19;
			}
			if (this.allowedAgeRange.max - this.allowedAgeRange.min < 4)
			{
				this.allowedAgeRange.min = this.allowedAgeRange.max - 4;
			}
		}

		// Token: 0x0600E446 RID: 58438 RVA: 0x0042AE13 File Offset: 0x00429013
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.allowedAgeRange.GetHashCode();
		}

		// Token: 0x040098C6 RID: 39110
		public IntRange allowedAgeRange = new IntRange(0, 999999);

		// Token: 0x040098C7 RID: 39111
		private const int RangeMin = 15;

		// Token: 0x040098C8 RID: 39112
		private const int RangeMax = 120;

		// Token: 0x040098C9 RID: 39113
		private const int RangeMinMax = 19;

		// Token: 0x040098CA RID: 39114
		private const int RangeMinWidth = 4;
	}
}
