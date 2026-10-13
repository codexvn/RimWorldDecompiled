using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E55 RID: 11861
	public abstract class CompStatOffsetBase : ThingComp
	{
		// Token: 0x17002AC9 RID: 10953
		// (get) Token: 0x06010B19 RID: 68377 RVA: 0x004CFF2A File Offset: 0x004CE12A
		public CompProperties_StatOffsetBase Props
		{
			get
			{
				return (CompProperties_StatOffsetBase)this.props;
			}
		}

		// Token: 0x17002ACA RID: 10954
		// (get) Token: 0x06010B1A RID: 68378 RVA: 0x004CFF37 File Offset: 0x004CE137
		public virtual Pawn LastUser
		{
			get
			{
				return this.lastUser;
			}
		}

		// Token: 0x06010B1B RID: 68379
		public abstract float GetStatOffset(Pawn pawn = null);

		// Token: 0x06010B1C RID: 68380
		public abstract IEnumerable<string> GetExplanation();

		// Token: 0x06010B1D RID: 68381 RVA: 0x004CFF3F File Offset: 0x004CE13F
		public virtual void Used(Pawn pawn)
		{
			this.lastUser = pawn;
		}

		// Token: 0x06010B1E RID: 68382 RVA: 0x004CFF48 File Offset: 0x004CE148
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.lastUser, "lastUser", false);
		}

		// Token: 0x0400AE06 RID: 44550
		protected Pawn lastUser;
	}
}
