using System;
using System.Collections.Generic;
using System.Reflection;
using RimWorld;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200007C RID: 124
	public class DebugTabMenu_Actions : DebugTabMenu
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000E6BE File Offset: 0x0000C8BE
		public DebugTabMenu_Actions(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root) : base(def, dialog, root)
		{
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000E6CC File Offset: 0x0000C8CC
		public override DebugActionNode InitActions(DebugActionNode absRoot)
		{
			this.myRoot = new DebugActionNode("Actions", DebugActionType.Action, null, null);
			absRoot.AddChild(this.myRoot);
			this.moreActionsNode = new DebugActionNode("Show more actions", DebugActionType.Action, null, null)
			{
				category = "More debug actions",
				displayPriority = -999999
			};
			this.myRoot.AddChild(this.moreActionsNode);
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugActionAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
					DebugActionYielderAttribute debugActionYielderAttribute;
					if (methodInfo.TryGetAttribute(out debugActionYielderAttribute))
					{
						foreach (DebugActionNode child in ((IEnumerable<DebugActionNode>)methodInfo.Invoke(null, null)))
						{
							this.myRoot.AddChild(child);
						}
					}
				}
			}
			this.myRoot.TrySort();
			return this.myRoot;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000E80C File Offset: 0x0000CA0C
		private void GenerateCacheForMethod(MethodInfo method, DebugActionAttribute attribute)
		{
			string text = string.IsNullOrEmpty(attribute.name) ? GenText.SplitCamelCase(method.Name) : attribute.name;
			if (attribute.actionType == DebugActionType.ToolMap || attribute.actionType == DebugActionType.ToolMapForPawns || attribute.actionType == DebugActionType.ToolWorld)
			{
				text = "T: " + text;
			}
			Type returnType = method.ReturnType;
			DebugActionNode debugActionNode;
			if (returnType == typeof(List<DebugActionNode>))
			{
				debugActionNode = new DebugActionNode();
				debugActionNode.childGetter = (Delegate.CreateDelegate(typeof(Func<List<DebugActionNode>>), method) as Func<List<DebugActionNode>>);
				if (!text.EndsWith("..."))
				{
					text += "...";
				}
			}
			else if (returnType == typeof(DebugActionNode))
			{
				debugActionNode = (DebugActionNode)method.Invoke(null, null);
				if (debugActionNode.children.Any<DebugActionNode>() && !text.EndsWith("..."))
				{
					text += "...";
				}
			}
			else
			{
				debugActionNode = new DebugActionNode();
				if (attribute.actionType == DebugActionType.ToolMapForPawns)
				{
					debugActionNode.pawnAction = (Delegate.CreateDelegate(typeof(Action<Pawn>), method) as Action<Pawn>);
				}
				else
				{
					debugActionNode.action = (Delegate.CreateDelegate(typeof(Action), method) as Action);
				}
			}
			if (debugActionNode.label.NullOrEmpty())
			{
				debugActionNode.label = text;
			}
			debugActionNode.label = debugActionNode.label.Replace("\\", "/");
			debugActionNode.category = (attribute.category ?? "General");
			debugActionNode.actionType = attribute.actionType;
			debugActionNode.displayPriority = attribute.displayPriority;
			debugActionNode.sourceAttribute = attribute;
			if (attribute.hideInSubMenu)
			{
				this.moreActionsNode.AddChild(debugActionNode);
				return;
			}
			this.myRoot.AddChild(debugActionNode);
		}

		// Token: 0x04000180 RID: 384
		private DebugActionNode moreActionsNode;
	}
}
