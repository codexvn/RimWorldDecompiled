using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C24 RID: 3108
	public class DiaNode
	{
		// Token: 0x06004FD5 RID: 20437 RVA: 0x0019A9E8 File Offset: 0x00198BE8
		public DiaNode(TaggedString text)
		{
			this.text = text;
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x0019AA04 File Offset: 0x00198C04
		public DiaNode(DiaNodeMold newDef)
		{
			this.def = newDef;
			this.def.used = true;
			this.text = this.def.texts.RandomElement<string>();
			if (this.def.optionList.Count > 0)
			{
				using (List<DiaOptionMold>.Enumerator enumerator = this.def.optionList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DiaOptionMold diaOptionMold = enumerator.Current;
						this.options.Add(new DiaOption(diaOptionMold));
					}
					return;
				}
			}
			this.options.Add(new DiaOption("OK".Translate()));
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x000026EA File Offset: 0x000008EA
		public void PreClose()
		{
		}

		// Token: 0x04003958 RID: 14680
		public TaggedString text;

		// Token: 0x04003959 RID: 14681
		public List<DiaOption> options = new List<DiaOption>();

		// Token: 0x0400395A RID: 14682
		protected DiaNodeMold def;
	}
}
