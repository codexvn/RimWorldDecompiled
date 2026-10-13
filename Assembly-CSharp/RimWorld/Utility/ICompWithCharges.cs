using System;

namespace RimWorld.Utility
{
	// Token: 0x02003894 RID: 14484
	public interface ICompWithCharges
	{
		// Token: 0x1700330C RID: 13068
		// (get) Token: 0x0601403C RID: 81980
		int RemainingCharges { get; }

		// Token: 0x0601403D RID: 81981
		bool CanBeUsed(out string reason);
	}
}
