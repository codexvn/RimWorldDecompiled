using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033C3 RID: 13251
	public class ITab_Pawn_Training : ITab
	{
		// Token: 0x170030B0 RID: 12464
		// (get) Token: 0x060129EA RID: 76266 RVA: 0x005827A0 File Offset: 0x005809A0
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.training != null && this.SelPawn.Faction == Faction.OfPlayer && !this.SelPawn.RaceProps.hideTrainingTab && (!this.SelPawn.IsMutant || this.SelPawn.mutant.Def.tameable);
			}
		}

		// Token: 0x060129EB RID: 76267 RVA: 0x00582804 File Offset: 0x00580A04
		public ITab_Pawn_Training()
		{
			this.labelKey = "TabTraining";
			this.tutorTag = "Training";
		}

		// Token: 0x060129EC RID: 76268 RVA: 0x00582824 File Offset: 0x00580A24
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(17f);
			rect.yMin += 10f;
			TrainingCardUtility.DrawTrainingCard(rect, this.SelPawn);
		}

		// Token: 0x060129ED RID: 76269 RVA: 0x00582880 File Offset: 0x00580A80
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = new Vector2(300f, TrainingCardUtility.TotalHeightForPawn(this.SelPawn));
		}
	}
}
