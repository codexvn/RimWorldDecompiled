using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200030E RID: 782
	public class ThinkTreeDef : Def
	{
		// Token: 0x06001398 RID: 5016 RVA: 0x0005CF20 File Offset: 0x0005B120
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			this.thinkRoot.ResolveSubnodesAndRecur();
			foreach (ThinkNode thinkNode in this.thinkRoot.ThisAndChildrenRecursive)
			{
				thinkNode.ResolveReferences();
			}
			ThinkTreeKeyAssigner.AssignKeys(this.thinkRoot, GenText.StableStringHash(this.defName));
			this.ResolveParentNodes(this.thinkRoot);
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x0005CFA4 File Offset: 0x0005B1A4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			HashSet<int> usedKeys = new HashSet<int>();
			HashSet<ThinkNode> instances = new HashSet<ThinkNode>();
			foreach (ThinkNode node in this.thinkRoot.ThisAndChildrenRecursive)
			{
				int key = node.UniqueSaveKey;
				if (key == -1)
				{
					string str = "Thinknode ";
					Type type = node.GetType();
					yield return str + ((type != null) ? type.ToString() : null) + " has invalid save key " + key.ToString();
				}
				else if (instances.Contains(node))
				{
					string str2 = "There are two same ThinkNode instances in one think tree (their type is ";
					Type type2 = node.GetType();
					yield return str2 + ((type2 != null) ? type2.ToString() : null) + ")";
				}
				else if (usedKeys.Contains(key))
				{
					string[] array = new string[5];
					array[0] = "Two ThinkNodes have the same unique save key ";
					array[1] = key.ToString();
					array[2] = " (one of the nodes is ";
					int num = 3;
					Type type3 = node.GetType();
					array[num] = ((type3 != null) ? type3.ToString() : null);
					array[4] = ")";
					yield return string.Concat(array);
				}
				if (key != -1)
				{
					usedKeys.Add(key);
				}
				instances.Add(node);
				node = null;
			}
			IEnumerator<ThinkNode> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x0005CFB4 File Offset: 0x0005B1B4
		public bool TryGetThinkNodeWithSaveKey(int key, out ThinkNode outNode)
		{
			outNode = null;
			if (key == -1)
			{
				return false;
			}
			if (key == this.thinkRoot.UniqueSaveKey)
			{
				outNode = this.thinkRoot;
				return true;
			}
			foreach (ThinkNode thinkNode in this.thinkRoot.ChildrenRecursive)
			{
				if (thinkNode.UniqueSaveKey == key)
				{
					outNode = thinkNode;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x0005D034 File Offset: 0x0005B234
		private void ResolveParentNodes(ThinkNode node)
		{
			for (int i = 0; i < node.subNodes.Count; i++)
			{
				if (node.subNodes[i].parent != null)
				{
					string[] array = new string[7];
					array[0] = "Think node ";
					int num = 1;
					ThinkNode thinkNode = node.subNodes[i];
					array[num] = ((thinkNode != null) ? thinkNode.ToString() : null);
					array[2] = " from think tree ";
					array[3] = this.defName;
					array[4] = " already has a parent node (";
					int num2 = 5;
					ThinkNode parent = node.subNodes[i].parent;
					array[num2] = ((parent != null) ? parent.ToString() : null);
					array[6] = "). This means that it's referenced by more than one think tree (should have been copied instead).";
					Log.Warning(string.Concat(array));
				}
				else
				{
					node.subNodes[i].parent = node;
					this.ResolveParentNodes(node.subNodes[i]);
				}
			}
		}

		// Token: 0x04001277 RID: 4727
		public ThinkNode thinkRoot;

		// Token: 0x04001278 RID: 4728
		[NoTranslate]
		public string insertTag;

		// Token: 0x04001279 RID: 4729
		public float insertPriority;
	}
}
