using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002726 RID: 10022
	public abstract class RitualObligationTrigger_EveryMember : RitualObligationTrigger
	{
		// Token: 0x0600DBF9 RID: 56313
		protected abstract void Recache();

		// Token: 0x0600DBFA RID: 56314 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_MemberDied(Pawn p)
		{
			this.Recache();
		}

		// Token: 0x0600DBFB RID: 56315 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			this.Recache();
		}

		// Token: 0x0600DBFC RID: 56316 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_MemberGenerated(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x0600DBFD RID: 56317 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_GameStarted()
		{
			this.Recache();
		}

		// Token: 0x0600DBFE RID: 56318 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_RitualExecuted(LordJob_Ritual ritual)
		{
			this.Recache();
		}

		// Token: 0x0600DBFF RID: 56319 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_MemberGained(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x0600DC00 RID: 56320 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_MemberLost(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x0600DC01 RID: 56321 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_MemberGuestStatusChanged(Pawn pawn)
		{
			this.Recache();
		}

		// Token: 0x0600DC02 RID: 56322 RVA: 0x0040D11B File Offset: 0x0040B31B
		public override void Notify_IdeoReformed()
		{
			this.Recache();
		}
	}
}
