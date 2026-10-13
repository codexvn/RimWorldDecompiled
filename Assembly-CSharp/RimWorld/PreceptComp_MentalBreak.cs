using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200263D RID: 9789
	public class PreceptComp_MentalBreak : PreceptComp
	{
		// Token: 0x0600D67B RID: 54907 RVA: 0x003FA3CC File Offset: 0x003F85CC
		public override IEnumerable<string> GetDescriptions()
		{
			yield return this.mentalBreakDef.LabelCap;
			yield break;
		}

		// Token: 0x040091CB RID: 37323
		public MentalBreakDef mentalBreakDef;
	}
}
