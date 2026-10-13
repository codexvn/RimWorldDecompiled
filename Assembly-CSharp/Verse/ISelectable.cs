using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C06 RID: 3078
	public interface ISelectable
	{
		// Token: 0x06004E8C RID: 20108
		IEnumerable<Gizmo> GetGizmos();

		// Token: 0x06004E8D RID: 20109
		string GetInspectString();

		// Token: 0x06004E8E RID: 20110
		IEnumerable<InspectTabBase> GetInspectTabs();
	}
}
