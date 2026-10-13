using System;

namespace LudeonTK
{
	// Token: 0x02000087 RID: 135
	public struct DebugMenuOption
	{
		// Token: 0x060002EA RID: 746 RVA: 0x0000FC45 File Offset: 0x0000DE45
		public DebugMenuOption(string label, DebugMenuOptionMode mode, Action method)
		{
			this.label = label;
			this.method = method;
			this.mode = mode;
		}

		// Token: 0x040001AF RID: 431
		public DebugMenuOptionMode mode;

		// Token: 0x040001B0 RID: 432
		public string label;

		// Token: 0x040001B1 RID: 433
		public Action method;
	}
}
