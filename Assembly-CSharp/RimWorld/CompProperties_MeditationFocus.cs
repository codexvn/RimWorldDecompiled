using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D48 RID: 11592
	public class CompProperties_MeditationFocus : CompProperties_StatOffsetBase
	{
		// Token: 0x0601048F RID: 66703 RVA: 0x004B4C5F File Offset: 0x004B2E5F
		public CompProperties_MeditationFocus()
		{
			this.compClass = typeof(CompMeditationFocus);
		}

		// Token: 0x06010490 RID: 66704 RVA: 0x004B4C82 File Offset: 0x004B2E82
		public override IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.offsets.Count; i = num + 1)
			{
				string explanationAbstract = this.offsets[i].GetExplanationAbstract(def);
				if (!explanationAbstract.NullOrEmpty())
				{
					yield return explanationAbstract;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06010491 RID: 66705 RVA: 0x004B4C9C File Offset: 0x004B2E9C
		public override void ResolveReferences(ThingDef parent)
		{
			base.PostLoadSpecial(parent);
			for (int i = 0; i < this.offsets.Count; i++)
			{
				this.offsets[i].ResolveReferences();
			}
		}

		// Token: 0x0400AA18 RID: 43544
		public List<MeditationFocusDef> focusTypes = new List<MeditationFocusDef>();
	}
}
