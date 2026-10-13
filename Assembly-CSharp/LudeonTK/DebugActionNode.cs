using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200006F RID: 111
	public class DebugActionNode
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000D5E6 File Offset: 0x0000B7E6
		public bool IsRoot
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000D5F4 File Offset: 0x0000B7F4
		public bool On
		{
			get
			{
				if (this.cachedCheckOn == null)
				{
					if (this.settingsField == null)
					{
						this.cachedCheckOn = new bool?(false);
					}
					else
					{
						this.cachedCheckOn = new bool?((bool)this.settingsField.GetValue(null));
					}
				}
				return this.cachedCheckOn.Value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000D654 File Offset: 0x0000B854
		public string LabelNow
		{
			get
			{
				if (Time.frameCount >= this.lastLabelCacheFrame + 30)
				{
					this.DirtyLabelCache();
				}
				if (this.cachedLabelNow == null)
				{
					if (this.labelGetter != null)
					{
						try
						{
							this.cachedLabelNow = this.labelGetter();
							goto IL_72;
						}
						catch (Exception ex)
						{
							string str = "Exception getting label for DebugActionNode: ";
							Exception ex2 = ex;
							Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
							this.cachedLabelNow = null;
							return this.label;
						}
					}
					this.cachedLabelNow = this.label;
				}
				IL_72:
				return this.cachedLabelNow;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000D6EC File Offset: 0x0000B8EC
		public bool ActiveNow
		{
			get
			{
				DebugActionType debugActionType = this.actionType;
				if (debugActionType - DebugActionType.ToolMap > 1)
				{
					return debugActionType != DebugActionType.ToolWorld || WorldRendererUtility.WorldSelected;
				}
				return WorldRendererUtility.DrawingMap;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000D717 File Offset: 0x0000B917
		public bool VisibleNow
		{
			get
			{
				return (this.sourceAttribute == null || this.sourceAttribute.IsAllowedInCurrentGameState) && (this.visibilityGetter == null || this.visibilityGetter());
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000D748 File Offset: 0x0000B948
		public string Path
		{
			get
			{
				if (this.cachedPath == null)
				{
					if (this.parent != null && !this.parent.IsRoot)
					{
						this.cachedPath = this.parent.Path + "\\" + this.label;
					}
					else
					{
						this.cachedPath = this.label;
					}
				}
				return this.cachedPath;
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000D7A7 File Offset: 0x0000B9A7
		public DebugActionNode()
		{
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000D7C1 File Offset: 0x0000B9C1
		public DebugActionNode(string label, DebugActionType actionType = DebugActionType.Action, Action action = null, Action<Pawn> pawnAction = null)
		{
			this.label = label;
			this.actionType = actionType;
			this.action = action;
			this.pawnAction = pawnAction;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
		public void AddChild(DebugActionNode child)
		{
			child.SetParent(this);
			this.children.Add(child);
			this.sorted = false;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000D814 File Offset: 0x0000BA14
		public void SetParent(DebugActionNode newParent)
		{
			this.parent = newParent;
			this.DirtyPath();
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000D824 File Offset: 0x0000BA24
		private void DirtyPath()
		{
			this.cachedPath = null;
			foreach (DebugActionNode debugActionNode in this.children)
			{
				debugActionNode.DirtyPath();
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000D87C File Offset: 0x0000BA7C
		public void DirtyLabelCache()
		{
			this.cachedLabelNow = null;
			this.cachedCheckOn = null;
			this.lastLabelCacheFrame = Time.frameCount;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000D89C File Offset: 0x0000BA9C
		public void TrySort()
		{
			if (this.sorted)
			{
				return;
			}
			if (!this.children.NullOrEmpty<DebugActionNode>())
			{
				this.children = (from r in this.children
				orderby DebugActionCategories.GetOrderFor(r.category), r.category, -r.displayPriority
				select r).ToList<DebugActionNode>();
			}
			this.sorted = true;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000D944 File Offset: 0x0000BB44
		public void TrySetupChildren()
		{
			if (!this.childrenSetup && this.childGetter != null)
			{
				foreach (DebugActionNode child in this.childGetter())
				{
					this.AddChild(child);
				}
			}
			this.childrenSetup = true;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		private static IEnumerable<Pawn> PawnsInside(IThingHolder holder)
		{
			Pawn pawn = holder as Pawn;
			if (pawn != null)
			{
				yield return pawn;
			}
			ThingOwner directlyHeldThings = holder.GetDirectlyHeldThings();
			if (directlyHeldThings == null)
			{
				yield break;
			}
			foreach (Thing thing in ((IEnumerable<Thing>)directlyHeldThings))
			{
				IThingHolder thingHolder = thing as IThingHolder;
				if (thingHolder != null)
				{
					foreach (Pawn pawn2 in DebugActionNode.PawnsInside(thingHolder))
					{
						yield return pawn2;
					}
					IEnumerator<Pawn> enumerator2 = null;
				}
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000D9C4 File Offset: 0x0000BBC4
		private static IEnumerable<Pawn> PawnsAt(IntVec3 cell, Map map)
		{
			foreach (Thing thing in map.thingGrid.ThingsAt(cell))
			{
				IThingHolder thingHolder = thing as IThingHolder;
				if (thingHolder != null)
				{
					foreach (Pawn pawn in DebugActionNode.PawnsInside(thingHolder))
					{
						yield return pawn;
					}
					IEnumerator<Pawn> enumerator2 = null;
				}
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		public void Enter(Dialog_Debug dialog)
		{
			this.TrySetupChildren();
			if (this.children.Any<DebugActionNode>())
			{
				this.TrySort();
				if (dialog == null)
				{
					dialog = new Dialog_Debug();
					Find.WindowStack.Add(dialog);
				}
				dialog.SetCurrentNode(this);
				return;
			}
			if (dialog != null)
			{
				dialog.Close(true);
			}
			switch (this.actionType)
			{
			case DebugActionType.Action:
				this.action();
				break;
			case DebugActionType.ToolMap:
			case DebugActionType.ToolWorld:
				DebugTools.curTool = new DebugTool(this.LabelNow, this.action, null);
				break;
			case DebugActionType.ToolMapForPawns:
			{
				DebugActionNode instance = this;
				DebugTools.curTool = new DebugTool(this.LabelNow, delegate()
				{
					if (UI.MouseCell().InBounds(Find.CurrentMap))
					{
						foreach (Pawn obj in DebugActionNode.PawnsAt(UI.MouseCell(), Find.CurrentMap).ToList<Pawn>())
						{
							instance.pawnAction(obj);
						}
					}
				}, null);
				break;
			}
			}
			this.DirtyLabelCache();
		}

		// Token: 0x04000147 RID: 327
		public string label;

		// Token: 0x04000148 RID: 328
		public DebugActionType actionType;

		// Token: 0x04000149 RID: 329
		public Action action;

		// Token: 0x0400014A RID: 330
		public Action<Pawn> pawnAction;

		// Token: 0x0400014B RID: 331
		public Func<List<DebugActionNode>> childGetter;

		// Token: 0x0400014C RID: 332
		public DebugActionAttribute sourceAttribute;

		// Token: 0x0400014D RID: 333
		public Func<bool> visibilityGetter;

		// Token: 0x0400014E RID: 334
		public Func<string> labelGetter;

		// Token: 0x0400014F RID: 335
		public string category;

		// Token: 0x04000150 RID: 336
		public int displayPriority;

		// Token: 0x04000151 RID: 337
		public FieldInfo settingsField;

		// Token: 0x04000152 RID: 338
		private bool childrenSetup;

		// Token: 0x04000153 RID: 339
		public DebugActionNode parent;

		// Token: 0x04000154 RID: 340
		public List<DebugActionNode> children = new List<DebugActionNode>();

		// Token: 0x04000155 RID: 341
		private string cachedPath;

		// Token: 0x04000156 RID: 342
		private string cachedLabelNow;

		// Token: 0x04000157 RID: 343
		private bool sorted;

		// Token: 0x04000158 RID: 344
		private bool? cachedCheckOn;

		// Token: 0x04000159 RID: 345
		private int lastLabelCacheFrame = -1;

		// Token: 0x0400015A RID: 346
		private const int LabelRecacheFrameCount = 30;
	}
}
