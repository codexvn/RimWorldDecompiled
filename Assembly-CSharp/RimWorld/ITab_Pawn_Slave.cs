using System;

namespace RimWorld
{
	// Token: 0x020033B6 RID: 13238
	public class ITab_Pawn_Slave : ITab_Pawn_Visitor
	{
		// Token: 0x170030AA RID: 12458
		// (get) Token: 0x060129B8 RID: 76216 RVA: 0x005817BC File Offset: 0x0057F9BC
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.IsSlaveOfColony;
			}
		}

		// Token: 0x060129B9 RID: 76217 RVA: 0x005817C9 File Offset: 0x0057F9C9
		public ITab_Pawn_Slave()
		{
			this.labelKey = "TabSlave";
			this.tutorTag = "Slave";
		}
	}
}
