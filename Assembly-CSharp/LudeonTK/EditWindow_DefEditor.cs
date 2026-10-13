using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x020000A6 RID: 166
	internal class EditWindow_DefEditor : EditWindow
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00014CAB File Offset: 0x00012EAB
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 600f);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000386 RID: 902 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00014CBC File Offset: 0x00012EBC
		public EditWindow_DefEditor(Def def)
		{
			this.def = def;
			this.cleanCopy = EditTreeNodeDatabase.RootOf(EditWindow_DefEditor.DeepClone(def));
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.optionalTitle = def.ToString();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00014D0C File Offset: 0x00012F0C
		public void InstanceEffecter()
		{
			Effecter effecter = ((EffecterDef)this.def).Spawn();
			IntVec3 cell = UI.UIToMapPosition(Find.Camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f))).ToIntVec3();
			effecter.Trigger(new TargetInfo(cell, Find.CurrentMap, false), new TargetInfo(cell, Find.CurrentMap, false), -1);
			effecter.Cleanup();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00014D78 File Offset: 0x00012F78
		public override void DoWindowContents(Rect inRect)
		{
			if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Escape))
			{
				UI.UnfocusCurrentControl();
			}
			Rect rect = new Rect(0f, 0f, inRect.width, 16f);
			this.labelColumnWidth = Widgets.HorizontalSlider(rect, this.labelColumnWidth, 0f, inRect.width, false, null, null, null, -1f);
			Rect outRect = inRect.AtZero();
			outRect.yMin += 16f;
			if (this.def is EffecterDef)
			{
				float x = inRect.x;
				base.DoRowButton(ref x, outRect.yMin, "Spawn Effecter", "spawn the effecter in the world", new Action(this.InstanceEffecter));
				base.DoRowButton(ref x, outRect.yMin, "Log Diff", "Diff the in-memory Def with the original", new Action(this.PrintDirty));
				outRect.yMin += 24f;
			}
			Rect rect2 = new Rect(0f, 0f, outRect.width - 16f, this.viewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect2, true);
			Listing_TreeDefs listing_TreeDefs = new Listing_TreeDefs(this.labelColumnWidth);
			listing_TreeDefs.Begin(rect2);
			TreeNode_Editor node = EditTreeNodeDatabase.RootOf(this.def);
			listing_TreeDefs.ContentLines(node, 0);
			listing_TreeDefs.End();
			if (Event.current.type == EventType.Layout)
			{
				this.viewHeight = listing_TreeDefs.CurHeight + 200f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00014F1E File Offset: 0x0001311E
		public override void PreClose()
		{
			base.PreClose();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00014F28 File Offset: 0x00013128
		public void PrintDirty()
		{
			Dictionary<string, string> dictionary = this.Compare(this.cleanCopy, EditTreeNodeDatabase.RootOf(this.def));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("DirtyValues For " + this.def.defName);
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				stringBuilder.AppendLine(keyValuePair.Key + ":" + keyValuePair.Value);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00014FD4 File Offset: 0x000131D4
		private Dictionary<string, string> Compare(TreeNode_Editor fromTree, TreeNode_Editor toTree)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			List<TreeNode_Editor> list = this.Flatten(fromTree, new List<TreeNode>()).Cast<TreeNode_Editor>().ToList<TreeNode_Editor>();
			List<TreeNode_Editor> list2 = this.Flatten(toTree, new List<TreeNode>()).Cast<TreeNode_Editor>().ToList<TreeNode_Editor>();
			int num = (list.Count < list2.Count) ? list.Count : list2.Count;
			for (int i = 0; i < num; i++)
			{
				TreeNode_Editor treeNode_Editor = list[i];
				TreeNode_Editor treeNode_Editor2 = list2[i];
				if (treeNode_Editor.HasValue && treeNode_Editor2.HasValue && treeNode_Editor.Value != null && treeNode_Editor2.Value != null && treeNode_Editor.Value.ToString() != treeNode_Editor2.Value.ToString())
				{
					dictionary[string.Format("{0}_{1}", treeNode_Editor.LabelText, treeNode_Editor.GetHashCode())] = string.Format("{0} -> {1}", treeNode_Editor.Value, treeNode_Editor2.Value);
				}
			}
			return dictionary;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x000150DC File Offset: 0x000132DC
		private List<TreeNode> Flatten(TreeNode tree, List<TreeNode> treeNodes)
		{
			treeNodes.Add(tree);
			if (tree.children != null)
			{
				foreach (TreeNode tree2 in tree.children)
				{
					this.Flatten(tree2, treeNodes);
				}
			}
			return treeNodes;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00015144 File Offset: 0x00013344
		public static object DeepClone(object objSource)
		{
			Type type = objSource.GetType();
			object obj = Activator.CreateInstance(type);
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (propertyInfo.CanWrite)
				{
					if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.IsEnum || propertyInfo.PropertyType == typeof(string))
					{
						propertyInfo.SetValue(obj, propertyInfo.GetValue(objSource, null), null);
					}
					else
					{
						object value = propertyInfo.GetValue(objSource, null);
						if (value == null)
						{
							propertyInfo.SetValue(obj, null, null);
						}
						else
						{
							propertyInfo.SetValue(obj, EditWindow_DefEditor.DeepClone(value), null);
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x04000222 RID: 546
		public Def def;

		// Token: 0x04000223 RID: 547
		public TreeNode_Editor cleanCopy;

		// Token: 0x04000224 RID: 548
		private float viewHeight;

		// Token: 0x04000225 RID: 549
		private Vector2 scrollPosition;

		// Token: 0x04000226 RID: 550
		private float labelColumnWidth = 140f;

		// Token: 0x04000227 RID: 551
		private const float TopAreaHeight = 16f;

		// Token: 0x04000228 RID: 552
		private const float ExtraScrollHeight = 200f;
	}
}
