using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003018 RID: 12312
	public class Alert_NeedJoySources : Alert
	{
		// Token: 0x06011525 RID: 70949 RVA: 0x004F7A3A File Offset: 0x004F5C3A
		public Alert_NeedJoySources()
		{
			this.defaultLabel = "NeedJoySource".Translate();
		}

		// Token: 0x06011526 RID: 70950 RVA: 0x004F7A58 File Offset: 0x004F5C58
		public override TaggedString GetExplanation()
		{
			Map map = this.badMap;
			int value = JoyUtility.JoyKindsOnMapCount(map);
			string label = map.info.parent.Label;
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			int joyKindsNeeded = expectationDef.joyKindsNeeded;
			string value2 = "AvailableRecreationTypes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + JoyUtility.JoyKindsOnMapString(map);
			string value3 = "MissingRecreationTypes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + JoyUtility.JoyKindsNotOnMapString(map);
			return "NeedJoySourceDesc".Translate(value, label, expectationDef.label, joyKindsNeeded, value2, value3);
		}

		// Token: 0x06011527 RID: 70951 RVA: 0x004F7B15 File Offset: 0x004F5D15
		public override AlertReport GetReport()
		{
			return this.BadMap() != null;
		}

		// Token: 0x06011528 RID: 70952 RVA: 0x004F7B28 File Offset: 0x004F5D28
		private Map BadMap()
		{
			this.badMap = null;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedJoySource(maps[i]))
				{
					this.badMap = maps[i];
					break;
				}
			}
			return this.badMap;
		}

		// Token: 0x06011529 RID: 70953 RVA: 0x004F7B78 File Offset: 0x004F5D78
		private bool NeedJoySource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			int num = JoyUtility.JoyKindsOnMapCount(map);
			int joyKindsNeeded = ExpectationsUtility.CurrentExpectationFor(map).joyKindsNeeded;
			return num < joyKindsNeeded;
		}

		// Token: 0x0400B37C RID: 45948
		private Map badMap;
	}
}
