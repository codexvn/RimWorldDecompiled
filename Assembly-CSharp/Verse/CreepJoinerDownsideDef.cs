using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000224 RID: 548
	public class CreepJoinerDownsideDef : CreepJoinerBaseDef
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x00043D15 File Offset: 0x00041F15
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.triggerMtbDays != 0f && this.triggersAfterDays != FloatRange.Zero)
			{
				yield return "cannot use both triggerMtbDays and triggersAfterDays fields, use one or neither.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040008F6 RID: 2294
		public bool repeats;

		// Token: 0x040008F7 RID: 2295
		public bool hasLetter;

		// Token: 0x040008F8 RID: 2296
		[MustTranslate]
		public string letterLabel;

		// Token: 0x040008F9 RID: 2297
		[MustTranslate]
		public string letterDesc;

		// Token: 0x040008FA RID: 2298
		public LetterDef letterDef;

		// Token: 0x040008FB RID: 2299
		public List<BackstoryTrait> traits = new List<BackstoryTrait>();

		// Token: 0x040008FC RID: 2300
		public List<HediffDef> hediffs = new List<HediffDef>();

		// Token: 0x040008FD RID: 2301
		public FloatRange triggersAfterDays = FloatRange.Zero;

		// Token: 0x040008FE RID: 2302
		public List<AbilityDef> abilities = new List<AbilityDef>();

		// Token: 0x040008FF RID: 2303
		public float triggerMtbDays;

		// Token: 0x04000900 RID: 2304
		public FloatRange triggerMinDays = FloatRange.Zero;

		// Token: 0x04000901 RID: 2305
		public bool canOccurWhenImprisoned;

		// Token: 0x04000902 RID: 2306
		public bool canOccurWhileDowned = true;

		// Token: 0x04000903 RID: 2307
		public bool mustBeConscious;

		// Token: 0x04000904 RID: 2308
		public Type workerType;
	}
}
