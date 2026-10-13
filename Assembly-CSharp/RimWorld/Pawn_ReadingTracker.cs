using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200250E RID: 9486
	public class Pawn_ReadingTracker : IExposable
	{
		// Token: 0x17001FCC RID: 8140
		// (get) Token: 0x0600CF3D RID: 53053 RVA: 0x003CD400 File Offset: 0x003CB600
		// (set) Token: 0x0600CF3E RID: 53054 RVA: 0x003CD456 File Offset: 0x003CB656
		public ReadingPolicy CurrentPolicy
		{
			get
			{
				if (this.pawn.IsMutant && this.pawn.mutant.Def.disablePolicies)
				{
					return null;
				}
				if (this.curPolicy == null)
				{
					this.curPolicy = Current.Game.readingPolicyDatabase.DefaultReadingPolicy();
				}
				return this.curPolicy;
			}
			set
			{
				this.curPolicy = value;
			}
		}

		// Token: 0x0600CF3F RID: 53055 RVA: 0x000024DF File Offset: 0x000006DF
		public Pawn_ReadingTracker()
		{
		}

		// Token: 0x0600CF40 RID: 53056 RVA: 0x003CD45F File Offset: 0x003CB65F
		public Pawn_ReadingTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CF41 RID: 53057 RVA: 0x003CD46E File Offset: 0x003CB66E
		public void ExposeData()
		{
			Scribe_References.Look<ReadingPolicy>(ref this.curPolicy, "curAssignment", false);
		}

		// Token: 0x04008CF8 RID: 36088
		public Pawn pawn;

		// Token: 0x04008CF9 RID: 36089
		private ReadingPolicy curPolicy;
	}
}
