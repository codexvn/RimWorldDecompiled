using System;

namespace Verse
{
	// Token: 0x020007C6 RID: 1990
	public interface IStrippable
	{
		// Token: 0x0600325B RID: 12891
		bool AnythingToStrip();

		// Token: 0x0600325C RID: 12892
		void Strip(bool notifyFaction = true);
	}
}
