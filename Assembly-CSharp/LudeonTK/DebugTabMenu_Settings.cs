using System;
using System.Reflection;
using RimWorld;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000080 RID: 128
	public class DebugTabMenu_Settings : DebugTabMenu
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x0000E6BE File Offset: 0x0000C8BE
		public DebugTabMenu_Settings(DebugTabMenuDef def, Dialog_Debug dialog, DebugActionNode root) : base(def, dialog, root)
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000EB58 File Offset: 0x0000CD58
		public override DebugActionNode InitActions(DebugActionNode absRoot)
		{
			this.myRoot = new DebugActionNode("Settings", DebugActionType.Action, null, null);
			absRoot.AddChild(this.myRoot);
			foreach (FieldInfo fi in typeof(DebugSettings).GetFields())
			{
				this.AddNode(fi, "General");
			}
			foreach (FieldInfo fi2 in typeof(DebugViewSettings).GetFields())
			{
				this.AddNode(fi2, "View");
			}
			foreach (FieldInfo fi3 in typeof(DebugGenerationSettings).GetFields())
			{
				this.AddNode(fi3, "Generation");
			}
			return this.myRoot;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000EC18 File Offset: 0x0000CE18
		private void AddNode(FieldInfo fi, string categoryLabel)
		{
			if (fi.IsLiteral)
			{
				return;
			}
			DebugActionNode debugActionNode = new DebugActionNode(this.LegibleFieldName(fi), DebugActionType.Action, delegate()
			{
				bool flag = (bool)fi.GetValue(null);
				fi.SetValue(null, !flag);
				MethodInfo method = fi.DeclaringType.GetMethod(fi.Name + "Toggled", BindingFlags.Static | BindingFlags.Public);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}, null);
			debugActionNode.category = categoryLabel;
			debugActionNode.settingsField = fi;
			this.myRoot.AddChild(debugActionNode);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000EC7F File Offset: 0x0000CE7F
		private string LegibleFieldName(FieldInfo fi)
		{
			return GenText.SplitCamelCase(fi.Name).CapitalizeFirst();
		}
	}
}
