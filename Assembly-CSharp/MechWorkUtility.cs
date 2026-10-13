using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

// Token: 0x0200003A RID: 58
public static class MechWorkUtility
{
	// Token: 0x06000137 RID: 311 RVA: 0x00008E0F File Offset: 0x0000700F
	public static IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
	{
		if (!parentDef.race.IsMechanoid || parentDef.race.mechEnabledWorkTypes.NullOrEmpty<WorkTypeDef>())
		{
			yield break;
		}
		TaggedString taggedString = "MechWorkActivitiesExplanation".Translate() + ":\n";
		foreach (WorkTypeDef workTypeDef in from wt in parentDef.race.mechEnabledWorkTypes
		orderby wt.label
		select wt)
		{
			IEnumerable<WorkGiverDef> source = workTypeDef.workGiversByPriority.Where((WorkGiverDef wg) => wg.canBeDoneByMechs);
			if (source.Any<WorkGiverDef>())
			{
				taggedString += "\n - " + workTypeDef.gerundLabel.CapitalizeFirst();
				foreach (WorkGiverDef workGiverDef in source.OrderBy((WorkGiverDef wg) => wg.label))
				{
					taggedString += "\n  - " + workGiverDef.LabelCap;
				}
			}
		}
		yield return new StatDrawEntry(StatCategoryDefOf.PawnWork, "MechWorkActivities".Translate(), (from w in parentDef.race.mechEnabledWorkTypes
		select w.gerundLabel).ToCommaList(true, false).CapitalizeFirst(), taggedString, 502, null, null, false, false);
		yield return new StatDrawEntry(StatCategoryDefOf.PawnWork, "MechWorkSkill".Translate(), parentDef.race.mechFixedSkillLevel.ToString(), "MechWorkSkillDesc".Translate(), 501, null, null, false, false);
		yield break;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x00008E20 File Offset: 0x00007020
	public static bool AnyWorkMechCouldDo(RecipeDef recipe)
	{
		IEnumerable<WorkTypeDef> mechEnabledWorkTypes = (from p in DefDatabase<PawnKindDef>.AllDefs
		where p.RaceProps.IsWorkMech
		select p).SelectMany((PawnKindDef p) => p.RaceProps.mechEnabledWorkTypes).Distinct<WorkTypeDef>();
		if (recipe.requiredGiverWorkType != null && !mechEnabledWorkTypes.Contains(recipe.requiredGiverWorkType))
		{
			return false;
		}
		IEnumerable<ThingDef> recipeUsers = recipe.AllRecipeUsers;
		return (from wg in DefDatabase<WorkGiverDef>.AllDefs
		where !wg.fixedBillGiverDefs.NullOrEmpty<ThingDef>() && wg.fixedBillGiverDefs.Intersect(recipeUsers).Any<ThingDef>()
		select wg).Any((WorkGiverDef wg) => mechEnabledWorkTypes.Contains(wg.workType));
	}
}
