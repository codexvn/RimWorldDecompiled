using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E54 RID: 11860
	public class CompProperties_StatOffsetBase : CompProperties
	{
		// Token: 0x06010B16 RID: 68374 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x06010B17 RID: 68375 RVA: 0x004CFED8 File Offset: 0x004CE0D8
		public virtual float GetMaxOffset(Thing parent = null)
		{
			float num = 0f;
			for (int i = 0; i < this.offsets.Count; i++)
			{
				num += this.offsets[i].MaxOffset(parent);
			}
			return num;
		}

		// Token: 0x0400AE04 RID: 44548
		public StatDef statDef;

		// Token: 0x0400AE05 RID: 44549
		public List<FocusStrengthOffset> offsets = new List<FocusStrengthOffset>();
	}
}
