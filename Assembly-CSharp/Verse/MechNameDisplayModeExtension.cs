using System;

namespace Verse
{
	// Token: 0x02000D51 RID: 3409
	public static class MechNameDisplayModeExtension
	{
		// Token: 0x060056E1 RID: 22241 RVA: 0x001BEECC File Offset: 0x001BD0CC
		public static string ToStringHuman(this MechNameDisplayMode mode)
		{
			switch (mode)
			{
			case MechNameDisplayMode.None:
				return "Never".Translate().CapitalizeFirst();
			case MechNameDisplayMode.WhileDrafted:
				return "MechNameDisplayMode_WhileDrafted".Translate().CapitalizeFirst();
			case MechNameDisplayMode.Always:
				return "MechNameDisplayMode_Always".Translate().CapitalizeFirst();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x001BEF3C File Offset: 0x001BD13C
		public static bool ShouldDisplayMechName(this MechNameDisplayMode mode, Pawn mech)
		{
			switch (mode)
			{
			case MechNameDisplayMode.None:
				return false;
			case MechNameDisplayMode.WhileDrafted:
				return mech.Name != null && mech.Drafted;
			case MechNameDisplayMode.Always:
				return mech.Name != null;
			default:
				throw new NotImplementedException(Prefs.MechNameMode.ToStringSafe<MechNameDisplayMode>());
			}
		}
	}
}
