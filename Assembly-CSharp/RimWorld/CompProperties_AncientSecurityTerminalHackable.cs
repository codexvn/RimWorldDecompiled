using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BCE RID: 11214
	public class CompProperties_AncientSecurityTerminalHackable : CompProperties_Hackable
	{
		// Token: 0x0600FAE4 RID: 64228 RVA: 0x0048E5CD File Offset: 0x0048C7CD
		public CompProperties_AncientSecurityTerminalHackable()
		{
			this.compClass = typeof(CompAncientSecurityTerminal);
		}

		// Token: 0x0400A4BF RID: 42175
		[MustTranslate]
		public string letterTurretDisabledLabel;

		// Token: 0x0400A4C0 RID: 42176
		[MustTranslate]
		public string letterTurretDisabledText;

		// Token: 0x0400A4C1 RID: 42177
		[MustTranslate]
		public string letterTurretHackedLabel;

		// Token: 0x0400A4C2 RID: 42178
		[MustTranslate]
		public string letterTurretHackedText;

		// Token: 0x0400A4C3 RID: 42179
		[MustTranslate]
		public string letterDoorOpenedLabel;

		// Token: 0x0400A4C4 RID: 42180
		[MustTranslate]
		public string letterDoorOpenedText;

		// Token: 0x0400A4C5 RID: 42181
		[MustTranslate]
		public string messageNoValidTarget;
	}
}
