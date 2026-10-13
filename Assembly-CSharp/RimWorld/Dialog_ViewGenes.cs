using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033A1 RID: 13217
	public class Dialog_ViewGenes : Window
	{
		// Token: 0x1700309C RID: 12444
		// (get) Token: 0x0601293F RID: 76095 RVA: 0x0057C9E1 File Offset: 0x0057ABE1
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(736f, 700f);
			}
		}

		// Token: 0x06012940 RID: 76096 RVA: 0x0057C9F2 File Offset: 0x0057ABF2
		public Dialog_ViewGenes(Pawn target) : base(null)
		{
			this.target = target;
			this.closeOnClickedOutside = true;
		}

		// Token: 0x06012941 RID: 76097 RVA: 0x0057CA09 File Offset: 0x0057AC09
		public override void PostOpen()
		{
			if (!ModLister.CheckBiotech("genes viewing"))
			{
				this.Close(false);
				return;
			}
			base.PostOpen();
		}

		// Token: 0x06012942 RID: 76098 RVA: 0x0057CA28 File Offset: 0x0057AC28
		public override void DoWindowContents(Rect inRect)
		{
			inRect.yMax -= Window.CloseButSize.y;
			Rect rect = inRect;
			rect.xMin += 34f;
			Text.Font = GameFont.Medium;
			Widgets.Label(rect, "ViewGenes".Translate() + ": " + this.target.genes.XenotypeLabelCap);
			Text.Font = GameFont.Small;
			GUI.color = XenotypeDef.IconColor;
			GUI.DrawTexture(new Rect(inRect.x, inRect.y, 30f, 30f), this.target.genes.XenotypeIcon);
			GUI.color = Color.white;
			inRect.yMin += 34f;
			Vector2 zero = Vector2.zero;
			GeneUIUtility.DrawGenesInfo(inRect, this.target, this.InitialSize.y, ref zero, ref this.scrollPosition, null);
			if (Widgets.ButtonText(new Rect(inRect.xMax - Window.CloseButSize.x, inRect.yMax, Window.CloseButSize.x, Window.CloseButSize.y), "Close".Translate(), true, true, true, null))
			{
				this.Close(true);
			}
		}

		// Token: 0x0400C0EC RID: 49388
		private Pawn target;

		// Token: 0x0400C0ED RID: 49389
		private Vector2 scrollPosition;

		// Token: 0x0400C0EE RID: 49390
		private const float HeaderHeight = 30f;
	}
}
