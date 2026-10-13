using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EA0 RID: 11936
	public class CompProperties_Transporter : CompProperties
	{
		// Token: 0x06010D03 RID: 68867 RVA: 0x004D6C0F File Offset: 0x004D4E0F
		public CompProperties_Transporter()
		{
			this.compClass = typeof(CompTransporter);
		}

		// Token: 0x0400AF02 RID: 44802
		public float massCapacity = 150f;

		// Token: 0x0400AF03 RID: 44803
		public float restEffectiveness;

		// Token: 0x0400AF04 RID: 44804
		public bool max1PerGroup;

		// Token: 0x0400AF05 RID: 44805
		public bool canChangeAssignedThingsAfterStarting;

		// Token: 0x0400AF06 RID: 44806
		public bool showOverallStats = true;

		// Token: 0x0400AF07 RID: 44807
		public SoundDef pawnLoadedSound;

		// Token: 0x0400AF08 RID: 44808
		public SoundDef pawnExitSound;

		// Token: 0x0400AF09 RID: 44809
		public bool shouldTickContents = true;

		// Token: 0x0400AF0A RID: 44810
		public bool showMassInInspectString;
	}
}
