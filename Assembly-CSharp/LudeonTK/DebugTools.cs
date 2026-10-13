using System;

namespace LudeonTK
{
	// Token: 0x02000079 RID: 121
	public static class DebugTools
	{
		// Token: 0x060002A5 RID: 677 RVA: 0x0000E487 File Offset: 0x0000C687
		public static void DebugToolsOnGUI()
		{
			if (DebugTools.curTool != null)
			{
				DebugTools.curTool.DebugToolOnGUI();
			}
			if (DebugTools.curMeasureTool != null)
			{
				DebugTools.curMeasureTool.DebugToolOnGUI();
			}
		}

		// Token: 0x0400017A RID: 378
		public static DebugTool curTool;

		// Token: 0x0400017B RID: 379
		public static MeasureTool curMeasureTool;
	}
}
