using System;
using System.Reflection;
using RimWorld;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200007E RID: 126
	public class DebugTabMenu_Output : DebugTabMenu
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x0000E6BE File Offset: 0x0000C8BE
		public DebugTabMenu_Output(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root) : base(def, dialog, root)
		{
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000EA0C File Offset: 0x0000CC0C
		public override DebugActionNode InitActions(DebugActionNode absRoot)
		{
			this.myRoot = new DebugActionNode("Outputs", DebugActionType.Action, null, null);
			absRoot.AddChild(this.myRoot);
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					DebugOutputAttribute attribute;
					if (methodInfo.TryGetAttribute(out attribute))
					{
						this.GenerateCacheForMethod(methodInfo, attribute);
					}
				}
			}
			this.myRoot.TrySort();
			return this.myRoot;
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000EAB4 File Offset: 0x0000CCB4
		private void GenerateCacheForMethod(MethodInfo method, DebugOutputAttribute attribute)
		{
			string label = attribute.name ?? GenText.SplitCamelCase(method.Name);
			Action action = Delegate.CreateDelegate(typeof(Action), method) as Action;
			DebugActionNode debugActionNode = new DebugActionNode(label, DebugActionType.Action, action, null);
			debugActionNode.category = (attribute.category ?? "General");
			debugActionNode.visibilityGetter = (() => !attribute.onlyWhenPlaying || Current.ProgramState == ProgramState.Playing);
			this.myRoot.AddChild(debugActionNode);
		}

		// Token: 0x04000184 RID: 388
		public const string DefaultCategory = "General";
	}
}
