using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002895 RID: 10389
	public class ScenPart_ConfigPage : ScenPart
	{
		// Token: 0x0600E370 RID: 58224 RVA: 0x00427AC3 File Offset: 0x00425CC3
		public override IEnumerable<Page> GetConfigPages()
		{
			yield return (Page)Activator.CreateInstance(this.def.pageClass);
			yield break;
		}

		// Token: 0x0600E371 RID: 58225 RVA: 0x000026EA File Offset: 0x000008EA
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
		}
	}
}
