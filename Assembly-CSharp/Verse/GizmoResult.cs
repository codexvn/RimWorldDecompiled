using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B8E RID: 2958
	public struct GizmoResult
	{
		// Token: 0x17000B83 RID: 2947
		// (get) Token: 0x06004BAC RID: 19372 RVA: 0x001817F3 File Offset: 0x0017F9F3
		public GizmoState State
		{
			get
			{
				return this.stateInt;
			}
		}

		// Token: 0x17000B84 RID: 2948
		// (get) Token: 0x06004BAD RID: 19373 RVA: 0x001817FB File Offset: 0x0017F9FB
		public Event InteractEvent
		{
			get
			{
				return this.interactEventInt;
			}
		}

		// Token: 0x06004BAE RID: 19374 RVA: 0x00181803 File Offset: 0x0017FA03
		public GizmoResult(GizmoState state)
		{
			this.stateInt = state;
			this.interactEventInt = null;
		}

		// Token: 0x06004BAF RID: 19375 RVA: 0x00181813 File Offset: 0x0017FA13
		public GizmoResult(GizmoState state, Event interactEvent)
		{
			this.stateInt = state;
			this.interactEventInt = interactEvent;
		}

		// Token: 0x040035F2 RID: 13810
		private GizmoState stateInt;

		// Token: 0x040035F3 RID: 13811
		private Event interactEventInt;
	}
}
