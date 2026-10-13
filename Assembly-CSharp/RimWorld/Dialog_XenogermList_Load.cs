using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031C2 RID: 12738
	public class Dialog_XenogermList_Load : Window
	{
		// Token: 0x06011EE0 RID: 73440 RVA: 0x0053802B File Offset: 0x0053622B
		public Dialog_XenogermList_Load(Action<CustomXenogerm> loadAction) : base(null)
		{
			this.doCloseButton = true;
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
			this.closeOnAccept = false;
			this.loadAction = loadAction;
		}

		// Token: 0x06011EE1 RID: 73441 RVA: 0x0053806C File Offset: 0x0053626C
		public override void DoWindowContents(Rect inRect)
		{
			List<CustomXenogerm> customXenogermsForReading = Find.CustomXenogermDatabase.CustomXenogermsForReading;
			Vector2 vector = new Vector2(inRect.width - 16f, 40f);
			float y = vector.y;
			float height = (float)customXenogermsForReading.Count * y;
			Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, height);
			float height2 = inRect.height - Window.CloseButSize.y - this.bottomAreaHeight - 18f;
			Rect outRect = inRect.TopPartPixels(height2);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			for (int i = customXenogermsForReading.Count - 1; i >= 0; i--)
			{
				CustomXenogerm xenogerm = customXenogermsForReading[i];
				if (num + vector.y >= this.scrollPosition.y && num <= this.scrollPosition.y + outRect.height)
				{
					Rect rect = new Rect(0f, num, vector.x, vector.y);
					if (i % 2 == 0)
					{
						Widgets.DrawAltRect(rect);
					}
					Widgets.BeginGroup(rect);
					Rect rect2 = new Rect(rect.width - 36f, (rect.height - 36f) / 2f, 36f, 36f);
					if (Widgets.ButtonImage(rect2, TexButton.Delete, Color.white, GenUI.SubtleMouseoverColor, true, null))
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDelete".Translate(xenogerm.name), delegate()
						{
							Find.CustomXenogermDatabase.Remove(xenogerm);
						}, true, null, WindowLayer.Dialog));
					}
					TooltipHandler.TipRegionByKey(rect2, "DeleteThisXenogerm");
					Text.Font = GameFont.Small;
					if (Widgets.ButtonText(new Rect(rect2.x - 100f, (rect.height - 36f) / 2f, 100f, 36f), "LoadGameButton".Translate(), true, true, true, null))
					{
						this.loadAction(xenogerm);
						this.Close(true);
					}
					GUI.color = Color.white;
					Rect rect3 = new Rect(8f, 0f, rect.width - 4f, rect.height);
					Text.Anchor = TextAnchor.MiddleLeft;
					Text.Font = GameFont.Small;
					Widgets.Label(rect3, xenogerm.name.Truncate(rect3.width * 1.8f, null));
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.EndGroup();
				}
				num += vector.y;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0400B999 RID: 47513
		protected float bottomAreaHeight;

		// Token: 0x0400B99A RID: 47514
		protected Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400B99B RID: 47515
		private Action<CustomXenogerm> loadAction;

		// Token: 0x0400B99C RID: 47516
		protected const float EntryHeight = 40f;

		// Token: 0x0400B99D RID: 47517
		protected const float NameLeftMargin = 8f;

		// Token: 0x0400B99E RID: 47518
		protected const float NameRightMargin = 4f;

		// Token: 0x0400B99F RID: 47519
		protected const float InfoWidth = 94f;

		// Token: 0x0400B9A0 RID: 47520
		protected const float InteractButWidth = 100f;

		// Token: 0x0400B9A1 RID: 47521
		protected const float InteractButHeight = 36f;

		// Token: 0x0400B9A2 RID: 47522
		protected const float DeleteButSize = 36f;
	}
}
