using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200065C RID: 1628
	public class Gene_Bloodfeeder : Gene
	{
		// Token: 0x0600290F RID: 10511 RVA: 0x000D8384 File Offset: 0x000D6584
		public override void PostAdd()
		{
			base.PostAdd();
			if (this.pawn.IsPrisonerOfColony && this.pawn.guest != null)
			{
				if (this.pawn.guest.HasInteractionWith((PrisonerInteractionModeDef interaction) => interaction.hideIfNoBloodfeeders))
				{
					this.pawn.guest.SetNoInteraction();
				}
			}
		}
	}
}
