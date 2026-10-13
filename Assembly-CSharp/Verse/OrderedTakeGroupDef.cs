using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002AF RID: 687
	public class OrderedTakeGroupDef : Def
	{
		// Token: 0x060010DD RID: 4317 RVA: 0x0004F6B7 File Offset: 0x0004D8B7
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.max <= 0)
			{
				yield return "Max should be greather than zero.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000E5B RID: 3675
		public int max = 3;
	}
}
