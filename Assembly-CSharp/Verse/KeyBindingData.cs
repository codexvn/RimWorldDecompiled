using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D26 RID: 3366
	public class KeyBindingData
	{
		// Token: 0x060055D5 RID: 21973 RVA: 0x000024DF File Offset: 0x000006DF
		public KeyBindingData()
		{
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x001BB624 File Offset: 0x001B9824
		public KeyBindingData(KeyCode keyBindingA, KeyCode keyBindingB)
		{
			this.keyBindingA = keyBindingA;
			this.keyBindingB = keyBindingB;
		}

		// Token: 0x060055D7 RID: 21975 RVA: 0x001BB63C File Offset: 0x001B983C
		public override string ToString()
		{
			string str = "[";
			if (this.keyBindingA != KeyCode.None)
			{
				str += this.keyBindingA.ToString();
			}
			if (this.keyBindingB != KeyCode.None)
			{
				str = str + ", " + this.keyBindingB.ToString();
			}
			return str + "]";
		}

		// Token: 0x04003E11 RID: 15889
		public KeyCode keyBindingA;

		// Token: 0x04003E12 RID: 15890
		public KeyCode keyBindingB;
	}
}
