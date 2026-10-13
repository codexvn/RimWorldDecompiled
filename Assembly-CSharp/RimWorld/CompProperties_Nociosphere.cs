using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D6E RID: 11630
	public class CompProperties_Nociosphere : CompProperties_Interactable
	{
		// Token: 0x0601056E RID: 66926 RVA: 0x004B764B File Offset: 0x004B584B
		public CompProperties_Nociosphere()
		{
			this.compClass = typeof(CompNociosphere);
		}

		// Token: 0x0400AA73 RID: 43635
		public List<CompProperties_Nociosphere.AttackDetails> attacks = new List<CompProperties_Nociosphere.AttackDetails>();

		// Token: 0x0400AA74 RID: 43636
		public FloatRange sentOnslaughtDurationSeconds;

		// Token: 0x0400AA75 RID: 43637
		public int minOnslaughtTicks;

		// Token: 0x0400AA76 RID: 43638
		public float activityOnRoofCollapsed = 0.3f;

		// Token: 0x0400AA77 RID: 43639
		[MustTranslate]
		public string onslaughtInspectText;

		// Token: 0x0400AA78 RID: 43640
		[MustTranslate]
		public string becomingUnstableInspectText;

		// Token: 0x0400AA79 RID: 43641
		[MustTranslate]
		public string unstableInspectText;

		// Token: 0x0400AA7A RID: 43642
		[MustTranslate]
		public string unstableWarning;

		// Token: 0x0400AA7B RID: 43643
		[MustTranslate]
		public string leftLetterLabel;

		// Token: 0x0400AA7C RID: 43644
		[MustTranslate]
		public string leftLetterText;

		// Token: 0x0400AA7D RID: 43645
		[MustTranslate]
		public string onslaughtEndedMessage;

		// Token: 0x0400AA7E RID: 43646
		[MustTranslate]
		public string departingMessage;

		// Token: 0x02002D6F RID: 11631
		public class AttackDetails
		{
			// Token: 0x0400AA7F RID: 43647
			public AbilityDef ability;

			// Token: 0x0400AA80 RID: 43648
			public bool requiresPawn = true;

			// Token: 0x0400AA81 RID: 43649
			public bool aiPreferArtificial;

			// Token: 0x0400AA82 RID: 43650
			public float maxCooldownTicks = 500f;
		}
	}
}
