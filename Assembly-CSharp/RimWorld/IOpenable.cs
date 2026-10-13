using System;

namespace RimWorld
{
	// Token: 0x02002960 RID: 10592
	public interface IOpenable
	{
		// Token: 0x17002370 RID: 9072
		// (get) Token: 0x0600E883 RID: 59523
		bool CanOpen { get; }

		// Token: 0x17002371 RID: 9073
		// (get) Token: 0x0600E884 RID: 59524
		int OpenTicks { get; }

		// Token: 0x0600E885 RID: 59525
		void Open();
	}
}
