using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031B8 RID: 12728
	public class Dialog_SelectXenotypeIcon : Window
	{
		// Token: 0x17002E85 RID: 11909
		// (get) Token: 0x06011E85 RID: 73349 RVA: 0x00535264 File Offset: 0x00533464
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(334f + this.Margin * 2f + 16f, 400f);
			}
		}

		// Token: 0x06011E86 RID: 73350 RVA: 0x00535288 File Offset: 0x00533488
		public Dialog_SelectXenotypeIcon(XenotypeIconDef selected, Action<XenotypeIconDef> iconSelector) : base(null)
		{
			this.selected = selected;
			this.iconSelector = iconSelector;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x06011E87 RID: 73351 RVA: 0x005352A6 File Offset: 0x005334A6
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("xenotype icon"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x06011E88 RID: 73352 RVA: 0x005352C4 File Offset: 0x005334C4
		public override void DoWindowContents(Rect rect)
		{
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, "SelectIcon".Translate());
			Text.Font = GameFont.Small;
			rect2.yMin += 39f;
			rect2.yMax -= Window.CloseButSize.y + 4f;
			Rect outRect = rect2;
			outRect.yMax -= 4f;
			Rect rect3 = new Rect(outRect.x, outRect.y, outRect.width - 16f, this.scrollHeight);
			Widgets.DrawLightHighlight(rect3);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect3, true);
			float num = outRect.x + 6f;
			float num2 = outRect.y + 6f;
			foreach (XenotypeIconDef xenotypeIconDef in DefDatabase<XenotypeIconDef>.AllDefs)
			{
				if (num + 35f + 6f > rect3.width)
				{
					num = outRect.x + 6f;
					num2 += 41f;
					this.scrollHeight = Mathf.Max(this.scrollHeight, num2);
				}
				Rect rect4 = new Rect(num, num2, 35f, 35f);
				Widgets.DrawHighlight(rect4);
				if (this.selected == xenotypeIconDef)
				{
					GUI.color = Dialog_SelectXenotypeIcon.OutlineColorSelected;
					Widgets.DrawHighlight(rect4);
					Widgets.DrawBox(rect4.ExpandedBy(2f), 2, null);
				}
				else
				{
					GUI.color = Dialog_SelectXenotypeIcon.OutlineColorUnselected;
					Widgets.DrawBox(rect4, 1, null);
				}
				GUI.color = Color.white;
				if (Widgets.ButtonImage(rect4, xenotypeIconDef.Icon, XenotypeDef.IconColor, true, null))
				{
					this.selected = xenotypeIconDef;
				}
				num += 41f;
			}
			Widgets.EndScrollView();
			if (Widgets.ButtonText(new Rect((rect.width - Window.CloseButSize.x) / 2f, rect2.yMax, Window.CloseButSize.x, Window.CloseButSize.y), "Accept".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x06011E89 RID: 73353 RVA: 0x0053550C File Offset: 0x0053370C
		public override void PreClose()
		{
			this.iconSelector(this.selected);
		}

		// Token: 0x0400B948 RID: 47432
		private Vector2 scrollPosition;

		// Token: 0x0400B949 RID: 47433
		private float scrollHeight;

		// Token: 0x0400B94A RID: 47434
		private XenotypeIconDef selected;

		// Token: 0x0400B94B RID: 47435
		private Action<XenotypeIconDef> iconSelector;

		// Token: 0x0400B94C RID: 47436
		private const float HeaderHeight = 35f;

		// Token: 0x0400B94D RID: 47437
		private const float IconSize = 35f;

		// Token: 0x0400B94E RID: 47438
		private const float IconGap = 6f;

		// Token: 0x0400B94F RID: 47439
		private const int IconsPerRow = 8;

		// Token: 0x0400B950 RID: 47440
		private static readonly Color OutlineColorSelected = new Color(1f, 1f, 0.7f, 1f);

		// Token: 0x0400B951 RID: 47441
		private static readonly Color OutlineColorUnselected = new Color(1f, 1f, 1f, 0.1f);
	}
}
