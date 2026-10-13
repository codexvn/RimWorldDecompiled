using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001833 RID: 6195
	public class ThoughtWorker_Precept_GrowthVatInColony : ThoughtWorker_Precept
	{
		// Token: 0x06008F35 RID: 36661 RVA: 0x002A5336 File Offset: 0x002A3536
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive || !p.IsColonist)
			{
				return ThoughtState.Inactive;
			}
			return p.MapHeld.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GrowthVat).Any<Building>();
		}
	}
}
