using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DF0 RID: 11760
	public static class RottableUtility
	{
		// Token: 0x060108CD RID: 67789 RVA: 0x004C5860 File Offset: 0x004C3A60
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x060108CE RID: 67790 RVA: 0x004C5884 File Offset: 0x004C3A84
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x060108CF RID: 67791 RVA: 0x004C58A8 File Offset: 0x004C3AA8
		public static RotStage GetRotStage(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return RotStage.Fresh;
			}
			return compRottable.Stage;
		}

		// Token: 0x060108D0 RID: 67792 RVA: 0x004C58C7 File Offset: 0x004C3AC7
		public static RotDrawMode GetRotDrawMode(RotStage stage)
		{
			switch (stage)
			{
			case RotStage.Rotting:
				return RotDrawMode.Rotting;
			case RotStage.Dessicated:
				return RotDrawMode.Dessicated;
			}
			return RotDrawMode.Fresh;
		}
	}
}
