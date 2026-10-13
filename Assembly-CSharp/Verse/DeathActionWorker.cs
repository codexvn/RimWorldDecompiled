using System;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200018F RID: 399
	public abstract class DeathActionWorker
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00035D63 File Offset: 0x00033F63
		public virtual RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_Died;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool DangerousInMelee
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B56 RID: 2902
		public abstract void PawnDied(Corpse corpse, Lord prevLord);

		// Token: 0x0400067F RID: 1663
		public DeathActionProperties props;
	}
}
