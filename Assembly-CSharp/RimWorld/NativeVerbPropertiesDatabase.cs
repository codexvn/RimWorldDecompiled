using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003673 RID: 13939
	public static class NativeVerbPropertiesDatabase
	{
		// Token: 0x0601358D RID: 79245 RVA: 0x005C18B0 File Offset: 0x005BFAB0
		public static VerbProperties VerbWithCategory(VerbCategory id)
		{
			VerbProperties verbProperties = (from v in NativeVerbPropertiesDatabase.allVerbDefs
			where v.category == id
			select v).FirstOrDefault<VerbProperties>();
			if (verbProperties == null)
			{
				Log.Error("Failed to find Verb with id " + id.ToString());
			}
			return verbProperties;
		}

		// Token: 0x0400D6A8 RID: 54952
		public static List<VerbProperties> allVerbDefs = VerbDefsHardcodedNative.AllVerbDefs().ToList<VerbProperties>();
	}
}
