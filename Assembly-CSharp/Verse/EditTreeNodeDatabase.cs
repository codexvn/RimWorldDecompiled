using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BB0 RID: 2992
	public static class EditTreeNodeDatabase
	{
		// Token: 0x06004C7B RID: 19579 RVA: 0x001848F4 File Offset: 0x00182AF4
		public static TreeNode_Editor RootOf(object obj)
		{
			for (int i = 0; i < EditTreeNodeDatabase.roots.Count; i++)
			{
				if (EditTreeNodeDatabase.roots[i].obj == obj)
				{
					return EditTreeNodeDatabase.roots[i];
				}
			}
			TreeNode_Editor treeNode_Editor = TreeNode_Editor.NewRootNode(obj);
			EditTreeNodeDatabase.roots.Add(treeNode_Editor);
			return treeNode_Editor;
		}

		// Token: 0x04003681 RID: 13953
		private static List<TreeNode_Editor> roots = new List<TreeNode_Editor>();
	}
}
