using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033A2 RID: 13218
	public class ITab_Pawn_Character : ITab
	{
		// Token: 0x1700309D RID: 12445
		// (get) Token: 0x06012943 RID: 76099 RVA: 0x0057CB78 File Offset: 0x0057AD78
		private Pawn PawnToShowInfoAbout
		{
			get
			{
				Pawn pawn = null;
				if (this.SelPawn != null)
				{
					pawn = this.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse != null)
					{
						pawn = corpse.InnerPawn;
					}
				}
				if (pawn == null)
				{
					Log.Error("Character tab found no selected pawn to display.");
					return null;
				}
				return pawn;
			}
		}

		// Token: 0x1700309E RID: 12446
		// (get) Token: 0x06012944 RID: 76100 RVA: 0x0057CBBE File Offset: 0x0057ADBE
		public override bool IsVisible
		{
			get
			{
				return this.PawnToShowInfoAbout.story != null;
			}
		}

		// Token: 0x06012945 RID: 76101 RVA: 0x0057CBCE File Offset: 0x0057ADCE
		public ITab_Pawn_Character()
		{
			this.labelKey = "TabCharacter";
			this.tutorTag = "Character";
		}

		// Token: 0x06012946 RID: 76102 RVA: 0x0057CBEC File Offset: 0x0057ADEC
		protected override void UpdateSize()
		{
			base.UpdateSize();
			this.size = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout) + new Vector2(17f, 17f) * 2f;
		}

		// Token: 0x06012947 RID: 76103 RVA: 0x0057CC24 File Offset: 0x0057AE24
		protected override void FillTab()
		{
			this.UpdateSize();
			Vector2 vector = CharacterCardUtility.PawnCardSize(this.PawnToShowInfoAbout);
			CharacterCardUtility.DrawCharacterCard(new Rect(17f, 17f, vector.x, vector.y), this.PawnToShowInfoAbout, null, default(Rect), true);
		}
	}
}
