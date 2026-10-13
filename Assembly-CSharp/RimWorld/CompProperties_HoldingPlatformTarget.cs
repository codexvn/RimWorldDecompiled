using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CE5 RID: 11493
	public class CompProperties_HoldingPlatformTarget : CompProperties
	{
		// Token: 0x0601024A RID: 66122 RVA: 0x004ABA53 File Offset: 0x004A9C53
		public CompProperties_HoldingPlatformTarget()
		{
			this.compClass = typeof(CompHoldingPlatformTarget);
		}

		// Token: 0x0400A8A1 RID: 43169
		public PawnKindDef heldPawnKind;

		// Token: 0x0400A8A2 RID: 43170
		[MustTranslate]
		public string capturedLetterLabel;

		// Token: 0x0400A8A3 RID: 43171
		[MustTranslate]
		public string capturedLetterText;

		// Token: 0x0400A8A4 RID: 43172
		public float baseEscapeIntervalMtbDays = 60f;

		// Token: 0x0400A8A5 RID: 43173
		public bool lookForTargetOnEscape = true;

		// Token: 0x0400A8A6 RID: 43174
		public bool canBeExecuted = true;

		// Token: 0x0400A8A7 RID: 43175
		public bool getsColdContainmentBonus;

		// Token: 0x0400A8A8 RID: 43176
		public bool hasAnimation = true;
	}
}
