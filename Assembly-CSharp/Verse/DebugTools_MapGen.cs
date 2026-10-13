using System;
using System.Collections.Generic;
using LudeonTK;

namespace Verse
{
	// Token: 0x02000B4B RID: 2891
	public static class DebugTools_MapGen
	{
		// Token: 0x06004A97 RID: 19095 RVA: 0x0017B8B8 File Offset: 0x00179AB8
		public static List<DebugActionNode> Options_Scatterers()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (Type localSt2 in typeof(GenStep_Scatterer).AllLeafSubclasses())
			{
				Type localSt = localSt2;
				list.Add(new DebugActionNode(localSt.ToString(), DebugActionType.ToolMap, null, null)
				{
					action = delegate()
					{
						((GenStep_Scatterer)Activator.CreateInstance(localSt)).ForceScatterAt(UI.MouseCell(), Find.CurrentMap);
					}
				});
			}
			return list;
		}
	}
}
