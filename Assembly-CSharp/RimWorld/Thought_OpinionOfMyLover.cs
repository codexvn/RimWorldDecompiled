using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001852 RID: 6226
	public class Thought_OpinionOfMyLover : Thought_Situational
	{
		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x06008F85 RID: 36741 RVA: 0x002A6284 File Offset: 0x002A4484
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false);
				string text = base.CurStage.label.Formatted(directPawnRelation.def.GetGenderSpecificLabel(directPawnRelation.otherPawn), directPawnRelation.otherPawn.LabelShort, directPawnRelation.otherPawn).CapitalizeFirst();
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessLabel(this.pawn, text);
				}
				return text;
			}
		}

		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x06008F86 RID: 36742 RVA: 0x002A6314 File Offset: 0x002A4514
		protected override float BaseMoodOffset
		{
			get
			{
				float num = 0.1f * (float)this.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false).otherPawn);
				if (num < 0f)
				{
					return Mathf.Min(num, -1f);
				}
				return Mathf.Max(num, 1f);
			}
		}
	}
}
