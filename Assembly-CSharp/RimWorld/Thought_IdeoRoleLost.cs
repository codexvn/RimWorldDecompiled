using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200253C RID: 9532
	public class Thought_IdeoRoleLost : Thought_Memory
	{
		// Token: 0x17002009 RID: 8201
		// (get) Token: 0x0600D08C RID: 53388 RVA: 0x003D5602 File Offset: 0x003D3802
		public Precept_Role Role
		{
			get
			{
				return (Precept_Role)this.sourcePrecept;
			}
		}

		// Token: 0x1700200A RID: 8202
		// (get) Token: 0x0600D08D RID: 53389 RVA: 0x003D560F File Offset: 0x003D380F
		public override bool ShouldDiscard
		{
			get
			{
				return this.Role == null || this.pawn.Ideo != this.Role.ideo || this.Role.IsAssigned(this.pawn) || base.ShouldDiscard;
			}
		}

		// Token: 0x1700200B RID: 8203
		// (get) Token: 0x0600D08E RID: 53390 RVA: 0x003D564C File Offset: 0x003D384C
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.Role.Named("ROLE"));
			}
		}

		// Token: 0x1700200C RID: 8204
		// (get) Token: 0x0600D08F RID: 53391 RVA: 0x003D5673 File Offset: 0x003D3873
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.Role.Named("ROLE"));
			}
		}
	}
}
