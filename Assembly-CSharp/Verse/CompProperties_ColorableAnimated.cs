using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008FC RID: 2300
	public class CompProperties_ColorableAnimated : CompProperties
	{
		// Token: 0x06003B5F RID: 15199 RVA: 0x00139F62 File Offset: 0x00138162
		public CompProperties_ColorableAnimated()
		{
			this.compClass = typeof(CompColorable_Animated);
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x00139F8C File Offset: 0x0013818C
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.colors.Count == 0)
			{
				yield return "there should be at least one color specified in colors list";
			}
			yield break;
		}

		// Token: 0x04002AEA RID: 10986
		public int changeInterval = 1;

		// Token: 0x04002AEB RID: 10987
		public bool startWithRandom;

		// Token: 0x04002AEC RID: 10988
		public List<Color> colors = new List<Color>();
	}
}
