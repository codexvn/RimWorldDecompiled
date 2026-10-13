using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020036DA RID: 14042
	public abstract class StatPart
	{
		// Token: 0x060137BA RID: 79802
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x060137BB RID: 79803
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x060137BC RID: 79804 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x060137BD RID: 79805 RVA: 0x002F117C File Offset: 0x002EF37C
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			return Enumerable.Empty<Dialog_InfoCard.Hyperlink>();
		}

		// Token: 0x060137BE RID: 79806 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ForceShow(StatRequest req)
		{
			return false;
		}

		// Token: 0x0400D87B RID: 55419
		public float priority;

		// Token: 0x0400D87C RID: 55420
		[Unsaved(false)]
		public StatDef parentStat;
	}
}
