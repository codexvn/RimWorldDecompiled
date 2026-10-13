using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E29 RID: 15913
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		// Token: 0x06015BC8 RID: 89032 RVA: 0x0067FFC0 File Offset: 0x0067E1C0
		public override void PostMapGenerate()
		{
			Building_ShipReactor building_ShipReactor = ((MapParent)this.parent).Map.listerBuildings.AllBuildingsColonistOfClass<Building_ShipReactor>().FirstOrDefault<Building_ShipReactor>();
			if (building_ShipReactor != null)
			{
				building_ShipReactor.charlonsReactor = true;
			}
		}

		// Token: 0x06015BC9 RID: 89033 RVA: 0x0067FFF7 File Offset: 0x0067E1F7
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in CaravanArrivalAction_VisitEscapeShip.GetFloatMenuOptions(caravan, (MapParent)this.parent))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}
	}
}
