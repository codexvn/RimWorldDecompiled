using System;
using Verse;

namespace RimWorld.Utility
{
	// Token: 0x02003895 RID: 14485
	public interface IReloadableComp : ICompWithCharges
	{
		// Token: 0x1700330D RID: 13069
		// (get) Token: 0x0601403E RID: 81982
		Thing ReloadableThing { get; }

		// Token: 0x1700330E RID: 13070
		// (get) Token: 0x0601403F RID: 81983
		ThingDef AmmoDef { get; }

		// Token: 0x1700330F RID: 13071
		// (get) Token: 0x06014040 RID: 81984
		int BaseReloadTicks { get; }

		// Token: 0x17003310 RID: 13072
		// (get) Token: 0x06014041 RID: 81985
		int MaxCharges { get; }

		// Token: 0x17003311 RID: 13073
		// (get) Token: 0x06014042 RID: 81986
		string LabelRemaining { get; }

		// Token: 0x06014043 RID: 81987
		bool NeedsReload(bool allowForceReload);

		// Token: 0x06014044 RID: 81988
		int MinAmmoNeeded(bool allowForcedReload);

		// Token: 0x06014045 RID: 81989
		int MaxAmmoNeeded(bool allowForcedReload);

		// Token: 0x06014046 RID: 81990
		int MaxAmmoAmount();

		// Token: 0x06014047 RID: 81991
		void ReloadFrom(Thing ammo);

		// Token: 0x06014048 RID: 81992
		string DisabledReason(int minNeeded, int maxNeeded);
	}
}
