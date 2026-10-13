using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C26 RID: 3110
	public class DiaNodeMold
	{
		// Token: 0x06004FD8 RID: 20440 RVA: 0x0019AAD8 File Offset: 0x00198CD8
		public void PostLoad()
		{
			int num = 0;
			foreach (string text in this.texts.ListFullCopy<string>())
			{
				this.texts[num] = text.Replace("\\n", Environment.NewLine);
				num++;
			}
			foreach (DiaOptionMold diaOptionMold in this.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.PostLoad();
				}
			}
		}

		// Token: 0x04003960 RID: 14688
		public string name = "Unnamed";

		// Token: 0x04003961 RID: 14689
		public bool unique;

		// Token: 0x04003962 RID: 14690
		public List<string> texts = new List<string>();

		// Token: 0x04003963 RID: 14691
		public List<DiaOptionMold> optionList = new List<DiaOptionMold>();

		// Token: 0x04003964 RID: 14692
		[Unsaved(false)]
		public bool isRoot = true;

		// Token: 0x04003965 RID: 14693
		[Unsaved(false)]
		public bool used;

		// Token: 0x04003966 RID: 14694
		[Unsaved(false)]
		public DiaNodeType nodeType;
	}
}
