using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200030D RID: 781
	public static class ThinkTreeKeyAssigner
	{
		// Token: 0x06001393 RID: 5011 RVA: 0x0005CE38 File Offset: 0x0005B038
		public static void Reset()
		{
			ThinkTreeKeyAssigner.assignedKeys.Clear();
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x0005CE44 File Offset: 0x0005B044
		public static void AssignKeys(ThinkNode rootNode, int startHash)
		{
			Rand.PushState(startHash);
			foreach (ThinkNode thinkNode in rootNode.ThisAndChildrenRecursive)
			{
				thinkNode.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(thinkNode));
			}
			Rand.PopState();
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x0005CEA0 File Offset: 0x0005B0A0
		public static void AssignSingleKey(ThinkNode node, int startHash)
		{
			Rand.PushState(startHash);
			node.SetUniqueSaveKey(ThinkTreeKeyAssigner.NextUnusedKeyFor(node));
			Rand.PopState();
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x0005CEBC File Offset: 0x0005B0BC
		private static int NextUnusedKeyFor(ThinkNode node)
		{
			int num = 0;
			while (node != null)
			{
				num = Gen.HashCombineInt(num, GenText.StableStringHash(node.GetType().Name));
				node = node.parent;
			}
			while (ThinkTreeKeyAssigner.assignedKeys.Contains(num))
			{
				num ^= Rand.Int;
			}
			ThinkTreeKeyAssigner.assignedKeys.Add(num);
			return num;
		}

		// Token: 0x04001276 RID: 4726
		private static HashSet<int> assignedKeys = new HashSet<int>();
	}
}
