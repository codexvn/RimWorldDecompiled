using System;

namespace RimWorld
{
	// Token: 0x020033B5 RID: 13237
	public class ITab_Pawn_Prisoner : ITab_Pawn_Visitor
	{
		// Token: 0x170030A9 RID: 12457
		// (get) Token: 0x060129B6 RID: 76214 RVA: 0x00581791 File Offset: 0x0057F991
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.IsPrisonerOfColony;
			}
		}

		// Token: 0x060129B7 RID: 76215 RVA: 0x0058179E File Offset: 0x0057F99E
		public ITab_Pawn_Prisoner()
		{
			this.labelKey = "TabPrisoner";
			this.tutorTag = "Prisoner";
		}
	}
}
