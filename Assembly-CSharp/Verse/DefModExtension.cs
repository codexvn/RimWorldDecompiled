using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000566 RID: 1382
	public abstract class DefModExtension
	{
		// Token: 0x060023E4 RID: 9188 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences(Def parentDef)
		{
		}
	}
}
