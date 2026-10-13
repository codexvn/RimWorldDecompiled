using System;

namespace RimWorld
{
	// Token: 0x02002BA3 RID: 11171
	public interface IActivity
	{
		// Token: 0x0600F9A5 RID: 63909
		void OnActivityActivated();

		// Token: 0x0600F9A6 RID: 63910
		void OnPassive();

		// Token: 0x0600F9A7 RID: 63911
		bool ShouldGoPassive();

		// Token: 0x0600F9A8 RID: 63912
		bool CanBeSuppressed();

		// Token: 0x0600F9A9 RID: 63913
		bool CanActivate();

		// Token: 0x0600F9AA RID: 63914
		string ActivityTooltipExtra();
	}
}
