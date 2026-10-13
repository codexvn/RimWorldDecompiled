using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E7E RID: 15998
	public static class WorldInspectPaneUtility
	{
		// Token: 0x06015E34 RID: 89652 RVA: 0x0068C258 File Offset: 0x0068A458
		public static string AdjustedLabelFor(List<WorldObject> worldObjects, Rect rect)
		{
			if (worldObjects.Count == 1)
			{
				return worldObjects[0].LabelCap;
			}
			if (WorldInspectPaneUtility.AllLabelsAreSame(worldObjects))
			{
				return worldObjects[0].LabelCap + " x" + worldObjects.Count.ToStringCached();
			}
			return "VariousLabel".Translate();
		}

		// Token: 0x06015E35 RID: 89653 RVA: 0x0068C2B4 File Offset: 0x0068A4B4
		private static bool AllLabelsAreSame(List<WorldObject> worldObjects)
		{
			if (worldObjects.Count <= 1)
			{
				return true;
			}
			string labelCap = worldObjects[0].LabelCap;
			for (int i = 1; i < worldObjects.Count; i++)
			{
				if (worldObjects[i].LabelCap != labelCap)
				{
					return false;
				}
			}
			return true;
		}
	}
}
