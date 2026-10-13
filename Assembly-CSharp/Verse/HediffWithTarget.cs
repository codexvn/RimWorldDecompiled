using System;

namespace Verse
{
	// Token: 0x02000733 RID: 1843
	public class HediffWithTarget : HediffWithComps
	{
		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x000E8400 File Offset: 0x000E6600
		public override bool ShouldRemove
		{
			get
			{
				if (this.target != null)
				{
					Pawn pawn = this.target as Pawn;
					if (pawn == null || !pawn.Dead)
					{
						return base.ShouldRemove;
					}
				}
				return true;
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x000E8434 File Offset: 0x000E6634
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x040023C9 RID: 9161
		public Thing target;
	}
}
