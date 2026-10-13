using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000244 RID: 580
	public class HediffCompProperties
	{
		// Token: 0x06000EE6 RID: 3814 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostLoad()
		{
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences(HediffDef parent)
		{
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x00045D9B File Offset: 0x00043F9B
		public virtual IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return "compClass is null";
			}
			int num;
			for (int i = 0; i < parentDef.comps.Count; i = num + 1)
			{
				if (parentDef.comps[i] != this && parentDef.comps[i].compClass == this.compClass)
				{
					string str = "two comps with same compClass: ";
					Type type = this.compClass;
					yield return str + ((type != null) ? type.ToString() : null);
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04000A06 RID: 2566
		[TranslationHandle]
		public Type compClass;
	}
}
