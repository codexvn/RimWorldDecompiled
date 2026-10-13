using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003388 RID: 13192
	public class ITab_Art : ITab
	{
		// Token: 0x17003077 RID: 12407
		// (get) Token: 0x060128C0 RID: 75968 RVA: 0x0057906C File Offset: 0x0057726C
		private CompArt SelectedCompArt
		{
			get
			{
				Thing thing = Find.Selector.SingleSelectedThing;
				MinifiedThing minifiedThing = thing as MinifiedThing;
				if (minifiedThing != null)
				{
					thing = minifiedThing.InnerThing;
				}
				if (thing == null)
				{
					return null;
				}
				return thing.TryGetComp<CompArt>();
			}
		}

		// Token: 0x17003078 RID: 12408
		// (get) Token: 0x060128C1 RID: 75969 RVA: 0x005790A0 File Offset: 0x005772A0
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompArt != null && this.SelectedCompArt.Active;
			}
		}

		// Token: 0x060128C2 RID: 75970 RVA: 0x005790B7 File Offset: 0x005772B7
		public ITab_Art()
		{
			this.size = ITab_Art.WinSize;
			this.labelKey = "TabArt";
			this.tutorTag = "Art";
		}

		// Token: 0x060128C3 RID: 75971 RVA: 0x005790E0 File Offset: 0x005772E0
		protected override void FillTab()
		{
			Rect rect2;
			Rect rect = rect2 = new Rect(0f, 0f, ITab_Art.WinSize.x, ITab_Art.WinSize.y).ContractedBy(10f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect2, this.SelectedCompArt.Title.Truncate(rect2.width, null));
			if (ITab_Art.cachedImageSource != this.SelectedCompArt || ITab_Art.cachedTaleRef != this.SelectedCompArt.TaleRef)
			{
				ITab_Art.cachedImageDescription = this.SelectedCompArt.GenerateImageDescription();
				ITab_Art.cachedImageSource = this.SelectedCompArt;
				ITab_Art.cachedTaleRef = this.SelectedCompArt.TaleRef;
			}
			Rect rect3 = rect;
			rect3.yMin += 35f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect3, ITab_Art.cachedImageDescription);
		}

		// Token: 0x0400C0A4 RID: 49316
		private static string cachedImageDescription;

		// Token: 0x0400C0A5 RID: 49317
		private static CompArt cachedImageSource;

		// Token: 0x0400C0A6 RID: 49318
		private static TaleReference cachedTaleRef;

		// Token: 0x0400C0A7 RID: 49319
		private static readonly Vector2 WinSize = new Vector2(400f, 300f);
	}
}
