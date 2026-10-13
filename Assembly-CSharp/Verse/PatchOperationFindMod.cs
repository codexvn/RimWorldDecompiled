using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	// Token: 0x020005A2 RID: 1442
	public class PatchOperationFindMod : PatchOperation
	{
		// Token: 0x0600256B RID: 9579 RVA: 0x000C124C File Offset: 0x000BF44C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool flag = false;
			for (int i = 0; i < this.mods.Count; i++)
			{
				if (ModLister.HasActiveModWithName(this.mods[i]))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return true;
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000C12B6 File Offset: 0x000BF4B6
		public override string ToString()
		{
			return string.Format("{0}({1})", base.ToString(), this.mods.ToCommaList(false, false));
		}

		// Token: 0x04001EB4 RID: 7860
		private List<string> mods;

		// Token: 0x04001EB5 RID: 7861
		private PatchOperation match;

		// Token: 0x04001EB6 RID: 7862
		private PatchOperation nomatch;
	}
}
