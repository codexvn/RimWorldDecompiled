using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000BBF RID: 3007
	public class Listing_Tree : Listing_Lines
	{
		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06004CE4 RID: 19684 RVA: 0x00187336 File Offset: 0x00185536
		protected virtual float LabelWidth
		{
			get
			{
				return base.ColumnWidth - 26f;
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06004CE5 RID: 19685 RVA: 0x00187344 File Offset: 0x00185544
		protected float EditAreaWidth
		{
			get
			{
				return base.ColumnWidth - this.LabelWidth;
			}
		}

		// Token: 0x06004CE6 RID: 19686 RVA: 0x00187353 File Offset: 0x00185553
		public override void Begin(Rect rect)
		{
			base.Begin(rect);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
		}

		// Token: 0x06004CE7 RID: 19687 RVA: 0x00187368 File Offset: 0x00185568
		public override void End()
		{
			base.End();
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06004CE8 RID: 19688 RVA: 0x0018737C File Offset: 0x0018557C
		protected float XAtIndentLevel(int indentLevel)
		{
			return (float)indentLevel * this.nestIndentWidth;
		}

		// Token: 0x06004CE9 RID: 19689 RVA: 0x00187388 File Offset: 0x00185588
		protected void LabelLeft(string label, string tipText, int indentLevel, float widthOffset = 0f, Color? textColor = null, float leftOffset = 0f)
		{
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, this.lineHeight)
			{
				xMin = this.XAtIndentLevel(indentLevel) + 18f + leftOffset
			};
			Widgets.DrawHighlightIfMouseover(rect);
			if (!tipText.NullOrEmpty())
			{
				if (Mouse.IsOver(rect))
				{
					GUI.DrawTexture(rect, TexUI.HighlightTex);
				}
				TooltipHandler.TipRegion(rect, tipText);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = (textColor ?? Color.white);
			rect.width = this.LabelWidth - rect.xMin + widthOffset;
			rect.yMax += 5f;
			rect.yMin -= 5f;
			Widgets.Label(rect, label.Truncate(rect.width, null));
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x06004CEA RID: 19690 RVA: 0x00187480 File Offset: 0x00185680
		protected bool OpenCloseWidget(TreeNode node, int indentLevel, int openMask)
		{
			if (!node.Openable)
			{
				return false;
			}
			float x = this.XAtIndentLevel(indentLevel);
			float y = this.curY + this.lineHeight / 2f - 9f;
			Rect butRect = new Rect(x, y, 18f, 18f);
			bool flag = this.IsOpen(node, openMask);
			Texture2D tex = flag ? TexButton.Collapse : TexButton.Reveal;
			if (Widgets.ButtonImage(butRect, tex, true, null))
			{
				if (flag)
				{
					SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				}
				node.SetOpen(openMask, !flag);
				return true;
			}
			return false;
		}

		// Token: 0x06004CEB RID: 19691 RVA: 0x00187516 File Offset: 0x00185716
		public virtual bool IsOpen(TreeNode node, int openMask)
		{
			return node.IsOpen(openMask);
		}

		// Token: 0x06004CEC RID: 19692 RVA: 0x00187524 File Offset: 0x00185724
		public void InfoText(string text, int indentLevel)
		{
			Text.WordWrap = true;
			Rect rect = new Rect(0f, this.curY, base.ColumnWidth, 50f);
			rect.xMin = this.LabelWidth;
			rect.height = Text.CalcHeight(text, rect.width);
			Widgets.Label(rect, text);
			this.curY += rect.height;
			Text.WordWrap = false;
		}

		// Token: 0x06004CED RID: 19693 RVA: 0x00187598 File Offset: 0x00185798
		public bool ButtonText(string label)
		{
			Text.WordWrap = true;
			float num = Text.CalcHeight(label, base.ColumnWidth);
			bool result = Widgets.ButtonText(new Rect(0f, this.curY, base.ColumnWidth, num), label, true, true, true, null);
			this.curY += num + 0f;
			Text.WordWrap = false;
			return result;
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x001875FB File Offset: 0x001857FB
		public WidgetRow StartWidgetsRow(int indentLevel)
		{
			WidgetRow result = new WidgetRow(this.LabelWidth, this.curY, UIDirection.RightThenUp, 99999f, 4f);
			this.curY += 24f;
			return result;
		}

		// Token: 0x040036B3 RID: 14003
		public float nestIndentWidth = 11f;

		// Token: 0x040036B4 RID: 14004
		protected const float OpenCloseWidgetSize = 18f;
	}
}
