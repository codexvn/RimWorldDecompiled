using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001B1 RID: 433
	public static class InspectTabManager
	{
		// Token: 0x06000C30 RID: 3120 RVA: 0x00039C88 File Offset: 0x00037E88
		public static InspectTabBase GetSharedInstance(Type tabType)
		{
			InspectTabBase inspectTabBase;
			if (InspectTabManager.sharedInstances.TryGetValue(tabType, out inspectTabBase))
			{
				return inspectTabBase;
			}
			inspectTabBase = (InspectTabBase)Activator.CreateInstance(tabType);
			InspectTabManager.sharedInstances.Add(tabType, inspectTabBase);
			return inspectTabBase;
		}

		// Token: 0x04000703 RID: 1795
		private static Dictionary<Type, InspectTabBase> sharedInstances = new Dictionary<Type, InspectTabBase>();
	}
}
