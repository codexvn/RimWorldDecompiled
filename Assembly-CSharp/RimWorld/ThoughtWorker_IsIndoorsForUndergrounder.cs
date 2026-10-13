using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188F RID: 6287
	public class ThoughtWorker_IsIndoorsForUndergrounder : ThoughtWorker
	{
		// Token: 0x0600900E RID: 36878 RVA: 0x002A7F8C File Offset: 0x002A618C
		public static bool IsAwakeAndIndoors(Pawn p, out bool isNaturalRoof)
		{
			isNaturalRoof = false;
			if (!p.Awake())
			{
				return false;
			}
			if (p.Position.UsesOutdoorTemperature(p.Map))
			{
				return false;
			}
			RoofDef roofDef = p.Map.roofGrid.RoofAt(p.Position);
			if (roofDef == null)
			{
				return false;
			}
			isNaturalRoof = roofDef.isNatural;
			return true;
		}

		// Token: 0x0600900F RID: 36879 RVA: 0x002A7FE0 File Offset: 0x002A61E0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && !flag;
		}
	}
}
