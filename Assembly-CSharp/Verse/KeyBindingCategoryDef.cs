using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200029B RID: 667
	public class KeyBindingCategoryDef : Def
	{
		// Token: 0x0600109B RID: 4251 RVA: 0x0004EA93 File Offset: 0x0004CC93
		public static KeyBindingCategoryDef Named(string defName)
		{
			return DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, true);
		}

		// Token: 0x04000DCA RID: 3530
		public bool isGameUniversal;

		// Token: 0x04000DCB RID: 3531
		public List<KeyBindingCategoryDef> checkForConflicts = new List<KeyBindingCategoryDef>();

		// Token: 0x04000DCC RID: 3532
		public bool selfConflicting = true;
	}
}
