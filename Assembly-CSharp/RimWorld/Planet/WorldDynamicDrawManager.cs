using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C75 RID: 15477
	public class WorldDynamicDrawManager
	{
		// Token: 0x0601505F RID: 86111 RVA: 0x0064B3B8 File Offset: 0x006495B8
		public void RegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot register drawable " + ((o != null) ? o.ToString() : null) + " while drawing is in progress. WorldObjects shouldn't be spawned in Draw methods.");
				}
				this.drawObjects.Add(o);
			}
		}

		// Token: 0x06015060 RID: 86112 RVA: 0x0064B408 File Offset: 0x00649608
		public void DeRegisterDrawable(WorldObject o)
		{
			if (o.def.useDynamicDrawer)
			{
				if (this.drawingNow)
				{
					Log.Warning("Cannot deregister drawable " + ((o != null) ? o.ToString() : null) + " while drawing is in progress. WorldObjects shouldn't be despawned in Draw methods.");
				}
				this.drawObjects.Remove(o);
			}
		}

		// Token: 0x06015061 RID: 86113 RVA: 0x0064B458 File Offset: 0x00649658
		public void DrawDynamicWorldObjects()
		{
			if (WorldRendererUtility.WorldBackgroundNow)
			{
				return;
			}
			if (!DebugViewSettings.drawWorldObjects)
			{
				return;
			}
			this.drawingNow = true;
			try
			{
				foreach (WorldObject worldObject in this.drawObjects)
				{
					try
					{
						if (!worldObject.def.expandingIcon || ExpandableWorldObjectsUtility.TransitionPct(worldObject) < 1f)
						{
							if (!WorldRendererUtility.WorldBackgroundNow || worldObject.VisibleInBackground)
							{
								worldObject.Draw();
							}
						}
					}
					catch (Exception ex)
					{
						string str = "Exception drawing ";
						WorldObject worldObject2 = worldObject;
						string str2 = (worldObject2 != null) ? worldObject2.ToString() : null;
						string str3 = ": ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			catch (Exception ex3)
			{
				string str4 = "Exception drawing dynamic world objects: ";
				Exception ex4 = ex3;
				Log.Error(str4 + ((ex4 != null) ? ex4.ToString() : null));
			}
			this.drawingNow = false;
		}

		// Token: 0x0400EA92 RID: 60050
		private readonly HashSet<WorldObject> drawObjects = new HashSet<WorldObject>();

		// Token: 0x0400EA93 RID: 60051
		private bool drawingNow;
	}
}
