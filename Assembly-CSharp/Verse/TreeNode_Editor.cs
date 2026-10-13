using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BB8 RID: 3000
	public class TreeNode_Editor : TreeNode
	{
		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06004C8F RID: 19599 RVA: 0x001852F0 File Offset: 0x001834F0
		public object ParentObj
		{
			get
			{
				return ((TreeNode_Editor)this.parentNode).obj;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06004C90 RID: 19600 RVA: 0x00185304 File Offset: 0x00183504
		public Type ObjectType
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.FieldType;
				}
				if (this.IsListItem)
				{
					return this.ListRootObject.GetType().GetGenericArguments()[0];
				}
				if (this.obj != null)
				{
					return this.obj.GetType();
				}
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06004C91 RID: 19601 RVA: 0x0018535F File Offset: 0x0018355F
		public bool HasValue
		{
			get
			{
				return this.owningField != null || this.IsListItem;
			}
		}

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x06004C92 RID: 19602 RVA: 0x0018537C File Offset: 0x0018357C
		// (set) Token: 0x06004C93 RID: 19603 RVA: 0x001853EC File Offset: 0x001835EC
		public object Value
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.GetValue(this.ParentObj);
				}
				if (this.IsListItem)
				{
					return this.ListRootObject.GetType().GetProperty("Item").GetValue(this.ListRootObject, new object[]
					{
						this.owningIndex
					});
				}
				throw new InvalidOperationException();
			}
			set
			{
				if (this.owningField != null)
				{
					this.owningField.SetValue(this.ParentObj, value);
				}
				if (this.IsListItem)
				{
					this.ListRootObject.GetType().GetProperty("Item").SetValue(this.ListRootObject, value, new object[]
					{
						this.owningIndex
					});
				}
			}
		}

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06004C94 RID: 19604 RVA: 0x00185456 File Offset: 0x00183656
		public bool IsListItem
		{
			get
			{
				return this.owningIndex >= 0;
			}
		}

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06004C95 RID: 19605 RVA: 0x00185464 File Offset: 0x00183664
		private object ListRootObject
		{
			get
			{
				return this.ParentObj;
			}
		}

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06004C96 RID: 19606 RVA: 0x0018546C File Offset: 0x0018366C
		public override bool Openable
		{
			get
			{
				return this.obj != null && this.nodeType != EditTreeNodeType.TerminalValue && (this.nodeType != EditTreeNodeType.ListRoot || (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null) != 0);
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06004C97 RID: 19607 RVA: 0x001854C2 File Offset: 0x001836C2
		public bool HasContentLines
		{
			get
			{
				return this.nodeType != EditTreeNodeType.TerminalValue;
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06004C98 RID: 19608 RVA: 0x001854D0 File Offset: 0x001836D0
		public bool HasNewButton
		{
			get
			{
				return (this.nodeType == EditTreeNodeType.ComplexObject && this.obj == null) || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorReplaceableAttribute>());
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06004C99 RID: 19609 RVA: 0x00185507 File Offset: 0x00183707
		public bool HasDeleteButton
		{
			get
			{
				return this.IsListItem || (this.owningField != null && this.owningField.FieldType.HasAttribute<EditorNullableAttribute>());
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06004C9A RID: 19610 RVA: 0x00185538 File Offset: 0x00183738
		public string ExtraInfoText
		{
			get
			{
				if (this.obj == null)
				{
					return "null";
				}
				if (this.obj.GetType().HasAttribute<EditorShowClassNameAttribute>())
				{
					return this.obj.GetType().Name;
				}
				if (this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
				{
					int num = (int)this.obj.GetType().GetProperty("Count").GetValue(this.obj, null);
					return string.Concat(new string[]
					{
						"(",
						num.ToString(),
						" ",
						(num == 1) ? "element" : "elements",
						")"
					});
				}
				return "";
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06004C9B RID: 19611 RVA: 0x00185619 File Offset: 0x00183819
		public string LabelText
		{
			get
			{
				if (this.owningField != null)
				{
					return this.owningField.Name;
				}
				if (this.IsListItem)
				{
					return this.owningIndex.ToString();
				}
				return this.ObjectType.Name;
			}
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x00185654 File Offset: 0x00183854
		private TreeNode_Editor()
		{
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x0018566A File Offset: 0x0018386A
		public static TreeNode_Editor NewRootNode(object rootObj)
		{
			if (rootObj.GetType().IsValueEditable())
			{
				throw new ArgumentException();
			}
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.owningField = null;
			treeNode_Editor.obj = rootObj;
			treeNode_Editor.nestDepth = 0;
			treeNode_Editor.RebuildChildNodes();
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x001856A8 File Offset: 0x001838A8
		public static TreeNode_Editor NewChildNodeFromField(TreeNode_Editor parent, FieldInfo fieldInfo)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningField = fieldInfo;
			if (!fieldInfo.FieldType.IsValueEditable())
			{
				treeNode_Editor.obj = fieldInfo.GetValue(parent.obj);
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06004C9F RID: 19615 RVA: 0x00185704 File Offset: 0x00183904
		private static TreeNode_Editor NewChildNodeFromListItem(TreeNode_Editor parent, int listIndex)
		{
			TreeNode_Editor treeNode_Editor = new TreeNode_Editor();
			treeNode_Editor.parentNode = parent;
			treeNode_Editor.nestDepth = parent.nestDepth + 1;
			treeNode_Editor.owningIndex = listIndex;
			object obj = parent.obj;
			Type type = obj.GetType();
			if (!type.GetGenericArguments()[0].IsValueEditable())
			{
				object value = type.GetProperty("Item").GetValue(obj, new object[]
				{
					listIndex
				});
				treeNode_Editor.obj = value;
				treeNode_Editor.RebuildChildNodes();
			}
			treeNode_Editor.InitiallyCacheData();
			return treeNode_Editor;
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x00185788 File Offset: 0x00183988
		private void InitiallyCacheData()
		{
			if (this.obj != null && this.obj.GetType().IsGenericType && this.obj.GetType().GetGenericTypeDefinition() == typeof(List<>))
			{
				this.nodeType = EditTreeNodeType.ListRoot;
			}
			else if (this.ObjectType.IsValueEditable())
			{
				this.nodeType = EditTreeNodeType.TerminalValue;
			}
			else
			{
				this.nodeType = EditTreeNodeType.ComplexObject;
			}
			if (this.obj != null)
			{
				this.editWidgetsMethod = this.obj.GetType().GetMethod("DoEditWidgets");
			}
		}

		// Token: 0x06004CA1 RID: 19617 RVA: 0x0018581C File Offset: 0x00183A1C
		public void RebuildChildNodes()
		{
			if (this.obj == null)
			{
				return;
			}
			this.children = new List<TreeNode>();
			Type objType = this.obj.GetType();
			if (objType.IsGenericType && objType.GetGenericTypeDefinition() == typeof(List<>))
			{
				int num = (int)objType.GetProperty("Count").GetValue(this.obj, null);
				for (int i = 0; i < num; i++)
				{
					TreeNode_Editor item = TreeNode_Editor.NewChildNodeFromListItem(this, i);
					this.children.Add(item);
				}
				return;
			}
			IEnumerable<FieldInfo> fields = this.obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			Func<FieldInfo, int> <>9__0;
			Func<FieldInfo, int> keySelector;
			if ((keySelector = <>9__0) == null)
			{
				keySelector = (<>9__0 = ((FieldInfo f) => this.InheritanceDistanceBetween(objType, f.DeclaringType)));
			}
			foreach (FieldInfo fieldInfo in fields.OrderByDescending(keySelector))
			{
				if (fieldInfo.GetCustomAttributes(typeof(UnsavedAttribute), true).Length == 0 && fieldInfo.GetCustomAttributes(typeof(EditorHiddenAttribute), true).Length == 0 && !fieldInfo.IsNotSerialized)
				{
					TreeNode_Editor item2 = TreeNode_Editor.NewChildNodeFromField(this, fieldInfo);
					this.children.Add(item2);
				}
			}
		}

		// Token: 0x06004CA2 RID: 19618 RVA: 0x00185984 File Offset: 0x00183B84
		private int InheritanceDistanceBetween(Type childType, Type parentType)
		{
			Type type = childType;
			int num = 0;
			while (!(type == parentType))
			{
				type = type.BaseType;
				num++;
				if (type == null)
				{
					Log.Error(((childType != null) ? childType.ToString() : null) + " is not a subclass of " + ((parentType != null) ? parentType.ToString() : null));
					return -1;
				}
			}
			return num;
		}

		// Token: 0x06004CA3 RID: 19619 RVA: 0x001859E0 File Offset: 0x00183BE0
		public void CheckLatentDelete()
		{
			if (this.indexToDelete >= 0)
			{
				this.obj.GetType().GetMethod("RemoveAt").Invoke(this.obj, new object[]
				{
					this.indexToDelete
				});
				this.RebuildChildNodes();
				this.indexToDelete = -1;
			}
		}

		// Token: 0x06004CA4 RID: 19620 RVA: 0x00185A38 File Offset: 0x00183C38
		public void Delete()
		{
			if (this.owningField != null)
			{
				this.owningField.SetValue(this.obj, null);
				return;
			}
			if (this.IsListItem)
			{
				((TreeNode_Editor)this.parentNode).indexToDelete = this.owningIndex;
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06004CA5 RID: 19621 RVA: 0x00185A8C File Offset: 0x00183C8C
		public void DoSpecialPreElements(Listing_TreeDefs listing)
		{
			if (this.obj == null)
			{
				return;
			}
			if (this.editWidgetsMethod != null)
			{
				WidgetRow widgetRow = listing.StartWidgetsRow(this.nestDepth);
				this.editWidgetsMethod.Invoke(this.obj, new object[]
				{
					widgetRow
				});
			}
			Editable editable = this.obj as Editable;
			if (editable != null)
			{
				GUI.color = new Color(1f, 0.5f, 0.5f, 1f);
				foreach (string text in editable.ConfigErrors())
				{
					listing.InfoText(text, this.nestDepth);
				}
				GUI.color = Color.white;
			}
		}

		// Token: 0x06004CA6 RID: 19622 RVA: 0x00185B58 File Offset: 0x00183D58
		public override string ToString()
		{
			string text = "EditTreeNode(";
			if (this.ParentObj != null)
			{
				string str = text;
				string str2 = " owningObj=";
				object parentObj = this.ParentObj;
				text = str + str2 + ((parentObj != null) ? parentObj.ToString() : null);
			}
			if (this.owningField != null)
			{
				string str3 = text;
				string str4 = " owningField=";
				FieldInfo fieldInfo = this.owningField;
				text = str3 + str4 + ((fieldInfo != null) ? fieldInfo.ToString() : null);
			}
			if (this.owningIndex >= 0)
			{
				text = text + " owningIndex=" + this.owningIndex.ToString();
			}
			return text + ")";
		}

		// Token: 0x0400368F RID: 13967
		public object obj;

		// Token: 0x04003690 RID: 13968
		public FieldInfo owningField;

		// Token: 0x04003691 RID: 13969
		public int owningIndex = -1;

		// Token: 0x04003692 RID: 13970
		private MethodInfo editWidgetsMethod;

		// Token: 0x04003693 RID: 13971
		public EditTreeNodeType nodeType;

		// Token: 0x04003694 RID: 13972
		private int indexToDelete = -1;
	}
}
