using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003116 RID: 12566
	public class Dialog_AssignCaravanDrugPolicies : Window
	{
		// Token: 0x17002DE7 RID: 11751
		// (get) Token: 0x06011A7B RID: 72315 RVA: 0x00512161 File Offset: 0x00510361
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(550f, 500f);
			}
		}

		// Token: 0x06011A7C RID: 72316 RVA: 0x00512172 File Offset: 0x00510372
		public Dialog_AssignCaravanDrugPolicies(Caravan caravan) : base(null)
		{
			this.caravan = caravan;
			this.doCloseButton = true;
		}

		// Token: 0x06011A7D RID: 72317 RVA: 0x0051218C File Offset: 0x0051038C
		public override void DoWindowContents(Rect rect)
		{
			rect.height -= Window.CloseButSize.y;
			float num = 0f;
			if (Widgets.ButtonText(new Rect(rect.width - 354f - 16f, num, 354f, 32f), "ManageDrugPolicies".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
			}
			num += 42f;
			Rect outRect = new Rect(0f, num, rect.width, rect.height - num);
			Rect viewRect = new Rect(0f, 0f, rect.width - 16f, this.lastHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPos, viewRect, true);
			float num2 = 0f;
			foreach (Pawn pawn in this.caravan.pawns)
			{
				if (pawn.drugs != null && !pawn.DevelopmentalStage.Baby())
				{
					if (num2 + 30f >= this.scrollPos.y && num2 <= this.scrollPos.y + outRect.height)
					{
						this.DoRow(new Rect(0f, num2, viewRect.width, 30f), pawn);
					}
					num2 += 30f;
				}
			}
			this.lastHeight = num2;
			Widgets.EndScrollView();
		}

		// Token: 0x06011A7E RID: 72318 RVA: 0x0051232C File Offset: 0x0051052C
		private void DoRow(Rect rect, Pawn pawn)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 354f, 30f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect2, pawn.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			DrugPolicyUIUtility.DoAssignDrugPolicyButtons(new Rect(rect.x + rect.width - 354f, rect.y, 354f, 30f), pawn);
		}

		// Token: 0x0400B5D2 RID: 46546
		private Caravan caravan;

		// Token: 0x0400B5D3 RID: 46547
		private Vector2 scrollPos;

		// Token: 0x0400B5D4 RID: 46548
		private float lastHeight;

		// Token: 0x0400B5D5 RID: 46549
		private const float RowHeight = 30f;

		// Token: 0x0400B5D6 RID: 46550
		private const float AssignDrugPolicyButtonsTotalWidth = 354f;

		// Token: 0x0400B5D7 RID: 46551
		private const int ManageDrugPoliciesButtonHeight = 32;
	}
}
