using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200351E RID: 13598
	public static class BuildRelatedCommandUtility
	{
		// Token: 0x0601316B RID: 78187 RVA: 0x005A975B File Offset: 0x005A795B
		public static IEnumerable<Command> RelatedBuildCommands(BuildableDef building)
		{
			foreach (Command command in BuildFacilityCommandUtility.BuildFacilityCommands(building))
			{
				yield return command;
			}
			IEnumerator<Command> enumerator = null;
			ThingDef buildingDef = building as ThingDef;
			if (buildingDef == null)
			{
				yield break;
			}
			BuildingProperties building2 = buildingDef.building;
			List<ThingDef> list = (building2 != null) ? building2.relatedBuildCommands : null;
			if (list != null)
			{
				Func<Ideo, bool> <>9__1;
				foreach (ThingDef thingDef in list)
				{
					if (ModsConfig.IdeologyActive && building.ideoBuilding && thingDef.ideoBuilding)
					{
						IEnumerable<Ideo> allIdeos = Find.FactionManager.OfPlayer.ideos.AllIdeos;
						Func<Ideo, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((Ideo i) => BuildRelatedCommandUtility.<RelatedBuildCommands>g__IdeoHasBuilding|0_0(i, (ThingDef)building)));
						}
						Ideo ideo = allIdeos.FirstOrDefault(predicate);
						if (ideo != null)
						{
							bool flag = false;
							foreach (Ideo ideo2 in Find.FactionManager.OfPlayer.ideos.AllIdeos)
							{
								if (ideo2 == ideo && BuildRelatedCommandUtility.<RelatedBuildCommands>g__IdeoHasBuilding|0_0(ideo2, thingDef))
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								continue;
							}
						}
					}
					Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(thingDef, true);
					if (designator_Build != null)
					{
						yield return designator_Build;
					}
				}
				List<ThingDef>.Enumerator enumerator2 = default(List<ThingDef>.Enumerator);
			}
			BuildingProperties building3 = buildingDef.building;
			List<TerrainDef> list2 = (building3 != null) ? building3.relatedTerrain : null;
			if (list2 != null)
			{
				foreach (TerrainDef buildable in list2)
				{
					Designator_Build designator_Build2 = BuildCopyCommandUtility.FindAllowedDesignator(buildable, true);
					if (designator_Build2 != null)
					{
						yield return designator_Build2;
					}
				}
				List<TerrainDef>.Enumerator enumerator4 = default(List<TerrainDef>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0601316C RID: 78188 RVA: 0x005A976C File Offset: 0x005A796C
		[CompilerGenerated]
		internal static bool <RelatedBuildCommands>g__IdeoHasBuilding|0_0(Ideo ideo, ThingDef td)
		{
			return ideo.HasPreceptForBuilding(td) || ideo.PreceptsListForReading.Any(delegate(Precept p)
			{
				Precept_RitualSeat precept_RitualSeat = p as Precept_RitualSeat;
				return precept_RitualSeat != null && precept_RitualSeat.ThingDef == td;
			});
		}
	}
}
