using System;

namespace RimWorld
{
	// Token: 0x020033B4 RID: 13236
	public class ITab_Pawn_Guest : ITab_Pawn_Visitor
	{
		// Token: 0x170030A8 RID: 12456
		// (get) Token: 0x060129B4 RID: 76212 RVA: 0x00581742 File Offset: 0x0057F942
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.HostFaction == Faction.OfPlayer && !this.SelPawn.IsPrisoner && !this.SelPawn.IsSlaveOfColony;
			}
		}

		// Token: 0x060129B5 RID: 76213 RVA: 0x00581773 File Offset: 0x0057F973
		public ITab_Pawn_Guest()
		{
			this.labelKey = "TabGuest";
			this.tutorTag = "Guest";
		}
	}
}
