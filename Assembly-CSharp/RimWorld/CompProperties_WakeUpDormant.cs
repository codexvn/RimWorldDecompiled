using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ECE RID: 11982
	public class CompProperties_WakeUpDormant : CompProperties
	{
		// Token: 0x06010EB2 RID: 69298 RVA: 0x004DE87C File Offset: 0x004DCA7C
		public CompProperties_WakeUpDormant()
		{
			this.compClass = typeof(CompWakeUpDormant);
		}

		// Token: 0x0400AFF7 RID: 45047
		[NoTranslate]
		public string wakeUpSignalTag = "CompCanBeDormant.WakeUp";

		// Token: 0x0400AFF8 RID: 45048
		public float wakeUpCheckRadius = -1f;

		// Token: 0x0400AFF9 RID: 45049
		public float wakeUpOnThingConstructedRadius = 3f;

		// Token: 0x0400AFFA RID: 45050
		public bool wakeUpOnDamage = true;

		// Token: 0x0400AFFB RID: 45051
		public bool onlyWakeUpSameFaction = true;

		// Token: 0x0400AFFC RID: 45052
		public bool onlySendSignalOnce = true;

		// Token: 0x0400AFFD RID: 45053
		public SoundDef wakeUpSound;

		// Token: 0x0400AFFE RID: 45054
		public bool wakeUpIfAnyTargetClose;

		// Token: 0x0400AFFF RID: 45055
		public TargetingParameters wakeUpTargetingParams = TargetingParameters.ForColonist();

		// Token: 0x0400B000 RID: 45056
		public bool wakeUpWithDelay;

		// Token: 0x0400B001 RID: 45057
		[NoTranslate]
		public string radiusCheckInspectPaneKey;

		// Token: 0x0400B002 RID: 45058
		[NoTranslate]
		public string activateMessageKey;

		// Token: 0x0400B003 RID: 45059
		[NoTranslate]
		public string activatePluralMessageKey;

		// Token: 0x0400B004 RID: 45060
		public MessageTypeDef activateMessageType;
	}
}
