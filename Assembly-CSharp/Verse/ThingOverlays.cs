using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BD9 RID: 3033
	public class ThingOverlays
	{
		// Token: 0x06004DAD RID: 19885 RVA: 0x0018AE40 File Offset: 0x00189040
		public void ThingOverlaysOnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			List<Thing> list = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.HasGUIOverlay);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (currentViewRect.Contains(thing.Position) && !Find.CurrentMap.fogGrid.IsFogged(thing.Position))
				{
					try
					{
						thing.DrawGUIOverlay();
					}
					catch (Exception ex)
					{
						string str = "Exception drawing ThingOverlay for ";
						Thing thing2 = thing;
						string str2 = (thing2 != null) ? thing2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}
	}
}
