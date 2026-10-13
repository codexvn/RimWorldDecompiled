using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x020001F5 RID: 501
	public class Editable
	{
		// Token: 0x06000D76 RID: 3446 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences()
		{
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostLoad()
		{
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> ConfigErrors()
		{
			return Enumerable.Empty<string>();
		}
	}
}
