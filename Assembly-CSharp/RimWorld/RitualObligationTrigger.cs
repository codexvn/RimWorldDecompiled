using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002720 RID: 10016
	public abstract class RitualObligationTrigger : IExposable
	{
		// Token: 0x170021B2 RID: 8626
		// (get) Token: 0x0600DBE1 RID: 56289 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string TriggerExtraDesc
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600DBE2 RID: 56290 RVA: 0x0040CEC4 File Offset: 0x0040B0C4
		public virtual void Init(RitualObligationTriggerProperties props)
		{
			this.mustBePlayerIdeo = props.mustBePlayerIdeo;
		}

		// Token: 0x0600DBE3 RID: 56291 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick()
		{
		}

		// Token: 0x0600DBE4 RID: 56292 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
		}

		// Token: 0x0600DBE5 RID: 56293 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberGenerated(Pawn pawn)
		{
		}

		// Token: 0x0600DBE6 RID: 56294 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberDied(Pawn p)
		{
		}

		// Token: 0x0600DBE7 RID: 56295 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberCorpseDestroyed(Pawn p)
		{
		}

		// Token: 0x0600DBE8 RID: 56296 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberLost(Pawn p)
		{
		}

		// Token: 0x0600DBE9 RID: 56297 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberGained(Pawn p)
		{
		}

		// Token: 0x0600DBEA RID: 56298 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_GameStarted()
		{
		}

		// Token: 0x0600DBEB RID: 56299 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_IdeoReformed()
		{
		}

		// Token: 0x0600DBEC RID: 56300 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_RitualExecuted(LordJob_Ritual ritual)
		{
		}

		// Token: 0x0600DBED RID: 56301 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MemberGuestStatusChanged(Pawn pawn)
		{
		}

		// Token: 0x0600DBEE RID: 56302 RVA: 0x0040CED2 File Offset: 0x0040B0D2
		public virtual void CopyTo(RitualObligationTrigger other)
		{
			other.ritual = this.ritual;
			other.mustBePlayerIdeo = this.mustBePlayerIdeo;
		}

		// Token: 0x0600DBEF RID: 56303 RVA: 0x0040CEEC File Offset: 0x0040B0EC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.mustBePlayerIdeo, "mustBePlayerIdeo", false, false);
		}

		// Token: 0x0400949E RID: 38046
		public Precept_Ritual ritual;

		// Token: 0x0400949F RID: 38047
		protected bool mustBePlayerIdeo;
	}
}
