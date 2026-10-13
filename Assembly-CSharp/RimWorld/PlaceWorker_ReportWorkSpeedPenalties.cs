using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E3 RID: 14307
	public class PlaceWorker_ReportWorkSpeedPenalties : PlaceWorker
	{
		// Token: 0x06013D44 RID: 81220 RVA: 0x005E3590 File Offset: 0x005E1790
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef == null)
			{
				return;
			}
			bool flag = StatPart_WorkTableOutdoors.Applies(thingDef, map, loc);
			bool flag2 = StatPart_WorkTableTemperature.Applies(thingDef, map, loc);
			bool flag3 = StatPart_WorkTableRoomRole.WouldApplyToBuildingIfPlaced(thingDef, map, loc);
			if (flag || flag2 || flag3)
			{
				string str = "WillGetWorkSpeedPenalty".Translate(def.label).CapitalizeFirst() + ": ";
				string text = "";
				if (flag)
				{
					text += "Outdoors".Translate().CapitalizeFirst();
				}
				if (flag2)
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += "BadTemperature".Translate();
				}
				if (flag3)
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += "NotInRoomRole".Translate(thingDef.building.workTableRoomRole.label);
				}
				Messages.Message(str + text.CapitalizeFirst() + ".", new TargetInfo(loc, map, false), MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
