using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200253D RID: 9533
	public class Thought_IdeoRoleEmpty : Thought_Situational
	{
		// Token: 0x1700200D RID: 8205
		// (get) Token: 0x0600D091 RID: 53393 RVA: 0x003D5602 File Offset: 0x003D3802
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x1700200E RID: 8206
		// (get) Token: 0x0600D092 RID: 53394 RVA: 0x003D569A File Offset: 0x003D389A
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x1700200F RID: 8207
		// (get) Token: 0x0600D093 RID: 53395 RVA: 0x003D56C1 File Offset: 0x003D38C1
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.ideo.memberName, this.Role.Named("ROLE"));
			}
		}

		// Token: 0x0600D094 RID: 53396 RVA: 0x003D5700 File Offset: 0x003D3900
		protected override ThoughtState CurrentStateInternal()
		{
			if (this.pawn.IsSlave)
			{
				return false;
			}
			if (GenDate.DaysPassed < 10)
			{
				return false;
			}
			if (this.Role.def.leaderRole && !Faction.OfPlayer.ideos.IsPrimary(this.Role.ideo))
			{
				return false;
			}
			return this.Role.Active && this.pawn.Ideo == this.Role.ideo && this.Role.ChosenPawnSingle() == null;
		}

		// Token: 0x0600D095 RID: 53397 RVA: 0x003D57A4 File Offset: 0x003D39A4
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoRoleEmpty thought_IdeoRoleEmpty = other as Thought_IdeoRoleEmpty;
			return thought_IdeoRoleEmpty != null && this.Role == thought_IdeoRoleEmpty.Role;
		}
	}
}
