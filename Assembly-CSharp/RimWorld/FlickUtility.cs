using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA3 RID: 7843
	public static class FlickUtility
	{
		// Token: 0x0600AE0C RID: 44556 RVA: 0x0031A078 File Offset: 0x00318278
		public static void UpdateFlickDesignation(Thing t)
		{
			bool flag = false;
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps != null)
			{
				for (int i = 0; i < thingWithComps.AllComps.Count; i++)
				{
					CompFlickable compFlickable = thingWithComps.AllComps[i] as CompFlickable;
					if (compFlickable != null && compFlickable.WantsFlick())
					{
						flag = true;
						break;
					}
				}
			}
			Designation designation = t.Map.designationManager.DesignationOn(t, DesignationDefOf.Flick);
			if (flag && designation == null)
			{
				t.Map.designationManager.AddDesignation(new Designation(t, DesignationDefOf.Flick, null));
			}
			else if (!flag && designation != null)
			{
				designation.Delete();
			}
			TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.SwitchFlickingDesignation, Array.Empty<string>());
		}

		// Token: 0x0600AE0D RID: 44557 RVA: 0x0031A128 File Offset: 0x00318328
		public static bool WantsToBeOn(Thing t)
		{
			CompFlickable compFlickable = t.TryGetComp<CompFlickable>();
			if (compFlickable != null && !compFlickable.SwitchIsOn)
			{
				return false;
			}
			CompSchedule compSchedule = t.TryGetComp<CompSchedule>();
			if (compSchedule != null && !compSchedule.Allowed)
			{
				return false;
			}
			if (t.TryGetComp<CompLightball>() != null && !t.IsRitualTarget())
			{
				return false;
			}
			CompAutoPowered compAutoPowered = t.TryGetComp<CompAutoPowered>();
			return compAutoPowered == null || compAutoPowered.WantsToBeOn;
		}
	}
}
