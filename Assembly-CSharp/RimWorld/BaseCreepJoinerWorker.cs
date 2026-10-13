using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020022D1 RID: 8913
	public abstract class BaseCreepJoinerWorker
	{
		// Token: 0x17001DAB RID: 7595
		// (get) Token: 0x0600C1E8 RID: 49640 RVA: 0x00383046 File Offset: 0x00381246
		// (set) Token: 0x0600C1E9 RID: 49641 RVA: 0x0038304E File Offset: 0x0038124E
		public Pawn_CreepJoinerTracker Tracker { get; set; }

		// Token: 0x17001DAC RID: 7596
		// (get) Token: 0x0600C1EA RID: 49642 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CanOccurOnDeath
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001DAD RID: 7597
		// (get) Token: 0x0600C1EB RID: 49643 RVA: 0x00383057 File Offset: 0x00381257
		public Pawn Pawn
		{
			get
			{
				return this.Tracker.Pawn;
			}
		}

		// Token: 0x0600C1EC RID: 49644 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnCreated()
		{
		}

		// Token: 0x0600C1ED RID: 49645 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanDoResponse()
		{
			return true;
		}

		// Token: 0x0600C1EE RID: 49646
		public abstract void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs);
	}
}
