using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003668 RID: 13928
	public static class MiscDebugDrawer
	{
		// Token: 0x06013568 RID: 79208 RVA: 0x005C0CF0 File Offset: 0x005BEEF0
		public static void DebugDrawInteractionCells()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (DebugViewSettings.drawInteractionCells)
			{
				foreach (object obj in Find.Selector.SelectedObjects)
				{
					Thing thing = obj as Thing;
					if (thing != null)
					{
						CellRenderer.RenderCell(thing.InteractionCell, 0.5f);
					}
				}
			}
		}
	}
}
