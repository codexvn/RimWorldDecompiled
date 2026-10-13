using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033C2 RID: 13250
	public class ITab_Pawn_Social : ITab
	{
		// Token: 0x170030AE RID: 12462
		// (get) Token: 0x060129E6 RID: 76262 RVA: 0x005826B3 File Offset: 0x005808B3
		public override bool IsVisible
		{
			get
			{
				return this.SelPawnForSocialInfo.RaceProps.IsFlesh && (!ModsConfig.AnomalyActive || !this.SelPawnForSocialInfo.RaceProps.IsAnomalyEntity);
			}
		}

		// Token: 0x170030AF RID: 12463
		// (get) Token: 0x060129E7 RID: 76263 RVA: 0x005826E8 File Offset: 0x005808E8
		private Pawn SelPawnForSocialInfo
		{
			get
			{
				if (this.SelPawn != null)
				{
					return this.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				string str = "Social tab on non-pawn non-corpse ";
				Thing selThing = base.SelThing;
				throw new InvalidOperationException(str + ((selThing != null) ? selThing.ToString() : null));
			}
		}

		// Token: 0x060129E8 RID: 76264 RVA: 0x0058273B File Offset: 0x0058093B
		public ITab_Pawn_Social()
		{
			this.size = new Vector2(540f, 510f);
			this.labelKey = "TabSocial";
			this.tutorTag = "Social";
		}

		// Token: 0x060129E9 RID: 76265 RVA: 0x0058276E File Offset: 0x0058096E
		protected override void FillTab()
		{
			SocialCardUtility.DrawSocialCard(new Rect(0f, 0f, this.size.x, this.size.y), this.SelPawnForSocialInfo);
		}

		// Token: 0x0400C165 RID: 49509
		public const float Width = 540f;
	}
}
