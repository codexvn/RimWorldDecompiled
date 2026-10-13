using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000078 RID: 120
	public abstract class DebugTabMenu
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000E251 File Offset: 0x0000C451
		public int Count
		{
			get
			{
				return this.VisibleActions.Count;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000E260 File Offset: 0x0000C460
		protected List<DebugActionNode> VisibleActions
		{
			get
			{
				if (this.cachedVisibleActions == null)
				{
					this.cachedVisibleActions = new List<DebugActionNode>();
					List<DebugActionNode> children = this.dialog.CurrentNode.children;
					for (int i = 0; i < children.Count; i++)
					{
						DebugActionNode debugActionNode = children[i];
						if (debugActionNode.VisibleNow)
						{
							this.cachedVisibleActions.Add(debugActionNode);
						}
					}
				}
				return this.cachedVisibleActions;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000E2C4 File Offset: 0x0000C4C4
		public DebugTabMenu(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode rootNode)
		{
			this.def = def;
			this.dialog = dialog;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000E2DA File Offset: 0x0000C4DA
		public void Enter(DebugActionNode root)
		{
			this.myRoot = root;
			this.myRoot.Enter(this.dialog);
		}

		// Token: 0x0600029E RID: 670
		public abstract DebugActionNode InitActions(DebugActionNode root);

		// Token: 0x0600029F RID: 671 RVA: 0x0000E2F4 File Offset: 0x0000C4F4
		public int HighlightedIndex(int currentHighlightIndex, int prioritizedHighlightedIndex)
		{
			List<DebugActionNode> visibleActions = this.VisibleActions;
			if (visibleActions.NullOrEmpty<DebugActionNode>() || prioritizedHighlightedIndex >= visibleActions.Count)
			{
				return -1;
			}
			if (this.dialog.FilterAllows(visibleActions[prioritizedHighlightedIndex].LabelNow))
			{
				return prioritizedHighlightedIndex;
			}
			if (this.dialog.Filter.NullOrEmpty())
			{
				return 0;
			}
			for (int i = 0; i < visibleActions.Count; i++)
			{
				if (this.dialog.FilterAllows(visibleActions[i].LabelNow))
				{
					currentHighlightIndex = i;
					break;
				}
			}
			return currentHighlightIndex;
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000E37A File Offset: 0x0000C57A
		public string LabelAtIndex(int index)
		{
			return this.VisibleActions[index].LabelNow;
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000E390 File Offset: 0x0000C590
		public void ListOptions(int highlightedIndex, float columnWidth)
		{
			string b = null;
			List<DebugActionNode> visibleActions = this.VisibleActions;
			for (int i = 0; i < visibleActions.Count; i++)
			{
				DebugActionNode debugActionNode = visibleActions[i];
				if (this.dialog.FilterAllows(debugActionNode.LabelNow))
				{
					if (debugActionNode.category != b)
					{
						this.dialog.DoGap(7f);
						this.dialog.DoLabel(debugActionNode.category, columnWidth);
						b = debugActionNode.category;
					}
					Log.openOnMessage = true;
					try
					{
						this.dialog.DrawNode(debugActionNode, columnWidth, highlightedIndex == i);
					}
					finally
					{
						Log.openOnMessage = false;
					}
				}
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000E43C File Offset: 0x0000C63C
		public void Recache()
		{
			this.cachedVisibleActions = null;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000E445 File Offset: 0x0000C645
		public void OnAcceptKeyPressed(int index)
		{
			if (index < 0)
			{
				return;
			}
			this.VisibleActions[index].Enter(this.dialog);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000E463 File Offset: 0x0000C663
		public static DebugTabMenu CreateMenu(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root)
		{
			return (DebugTabMenu)Activator.CreateInstance(def.menuClass, new object[]
			{
				def,
				dialog,
				root
			});
		}

		// Token: 0x04000176 RID: 374
		protected DebugActionNode myRoot;

		// Token: 0x04000177 RID: 375
		public readonly DebugTabMenuDef def;

		// Token: 0x04000178 RID: 376
		protected readonly Dialog_Debug dialog;

		// Token: 0x04000179 RID: 377
		private List<DebugActionNode> cachedVisibleActions;
	}
}
