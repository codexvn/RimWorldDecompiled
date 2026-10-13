using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C27 RID: 3111
	public class DiaNodeList
	{
		// Token: 0x06004FDA RID: 20442 RVA: 0x0019ABF4 File Offset: 0x00198DF4
		public DiaNodeMold RandomNodeFromList()
		{
			List<DiaNodeMold> list = this.Nodes.ListFullCopy<DiaNodeMold>();
			foreach (string nodeName in this.NodeNames)
			{
				list.Add(DialogDatabase.GetNodeNamed(nodeName));
			}
			foreach (DiaNodeMold diaNodeMold in list)
			{
				if (diaNodeMold.unique && diaNodeMold.used)
				{
					list.Remove(diaNodeMold);
				}
			}
			return list.RandomElement<DiaNodeMold>();
		}

		// Token: 0x04003967 RID: 14695
		public string Name = "NeedsName";

		// Token: 0x04003968 RID: 14696
		public List<DiaNodeMold> Nodes = new List<DiaNodeMold>();

		// Token: 0x04003969 RID: 14697
		public List<string> NodeNames = new List<string>();
	}
}
