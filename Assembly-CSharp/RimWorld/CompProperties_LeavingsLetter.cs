using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D1A RID: 11546
	public class CompProperties_LeavingsLetter : CompProperties
	{
		// Token: 0x0601039D RID: 66461 RVA: 0x004B152C File Offset: 0x004AF72C
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			ThingFilter thingFilter = this.leavingsFilter;
			if (thingFilter == null)
			{
				return;
			}
			thingFilter.ResolveReferences();
		}

		// Token: 0x0601039E RID: 66462 RVA: 0x004B1545 File Offset: 0x004AF745
		public CompProperties_LeavingsLetter()
		{
			this.compClass = typeof(CompLeavingsLetter);
		}

		// Token: 0x0400A972 RID: 43378
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400A973 RID: 43379
		[MustTranslate]
		public string letterText;

		// Token: 0x0400A974 RID: 43380
		public LetterDef letterDef;

		// Token: 0x0400A975 RID: 43381
		public ThingFilter leavingsFilter;
	}
}
