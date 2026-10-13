using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002898 RID: 10392
	public class ScenPart_ConfigPage_ConfigureStartingPawns : ScenPart_ConfigPage_ConfigureStartingPawnsBase
	{
		// Token: 0x1700229B RID: 8859
		// (get) Token: 0x0600E381 RID: 58241 RVA: 0x00427D95 File Offset: 0x00425F95
		protected override int TotalPawnCount
		{
			get
			{
				return this.pawnCount;
			}
		}

		// Token: 0x0600E382 RID: 58242 RVA: 0x00427DA0 File Offset: 0x00425FA0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			base.DoEditInterface(listing);
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			scenPartRect.height = ScenPart.RowHeight;
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect = new Rect(scenPartRect.x - 200f, scenPartRect.y + ScenPart.RowHeight, 200f, ScenPart.RowHeight);
			rect.xMax -= 4f;
			Widgets.Label(rect, "ScenPart_StartWithPawns_OutOf".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnCount, ref this.pawnCountBuffer, 1f, 10f);
			scenPartRect.y += ScenPart.RowHeight;
			Widgets.TextFieldNumeric<int>(scenPartRect, ref this.pawnChoiceCount, ref this.pawnCountChoiceBuffer, (float)this.pawnCount, 10f);
		}

		// Token: 0x0600E383 RID: 58243 RVA: 0x00427E7A File Offset: 0x0042607A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pawnCount, "pawnCount", 0, false);
		}

		// Token: 0x0600E384 RID: 58244 RVA: 0x00427E94 File Offset: 0x00426094
		public override string Summary(Scenario scen)
		{
			if (this.pawnCount == 1)
			{
				return "ScenPart_StartWithPawn".Translate();
			}
			return "ScenPart_StartWithPawns".Translate(this.pawnCount);
		}

		// Token: 0x0600E385 RID: 58245 RVA: 0x00427EC9 File Offset: 0x004260C9
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.pawnCount;
		}

		// Token: 0x0600E386 RID: 58246 RVA: 0x00427ED8 File Offset: 0x004260D8
		public override void Randomize()
		{
			this.pawnCount = Rand.RangeInclusive(1, 6);
			this.pawnChoiceCount = 10;
		}

		// Token: 0x0600E387 RID: 58247 RVA: 0x00427EF0 File Offset: 0x004260F0
		protected override void GenerateStartingPawns()
		{
			int num = 0;
			do
			{
				StartingPawnUtility.ClearAllStartingPawns();
				for (int i = 0; i < this.pawnCount; i++)
				{
					StartingPawnUtility.AddNewPawn(-1);
				}
				num++;
			}
			while (num <= 20 && !StartingPawnUtility.WorkTypeRequirementsSatisfied());
		}

		// Token: 0x0600E388 RID: 58248 RVA: 0x00427F2B File Offset: 0x0042612B
		public override void PostIdeoChosen()
		{
			Find.GameInitData.allowedDevelopmentalStages = this.allowedDevelopmentalStages;
			base.PostIdeoChosen();
		}

		// Token: 0x0400985C RID: 39004
		public int pawnCount = 3;

		// Token: 0x0400985D RID: 39005
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x0400985E RID: 39006
		private string pawnCountBuffer;

		// Token: 0x0400985F RID: 39007
		private string pawnCountChoiceBuffer;

		// Token: 0x04009860 RID: 39008
		private const int MaxPawnChoiceCount = 10;
	}
}
