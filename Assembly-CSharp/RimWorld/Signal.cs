using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D1E RID: 7454
	public struct Signal
	{
		// Token: 0x0600A6B1 RID: 42673 RVA: 0x002FBCE8 File Offset: 0x002F9EE8
		public Signal(string tag, bool global = false)
		{
			this.tag = tag;
			this.args = default(SignalArgs);
			this.global = global;
		}

		// Token: 0x0600A6B2 RID: 42674 RVA: 0x002FBD04 File Offset: 0x002F9F04
		public Signal(string tag, SignalArgs args, bool global = false)
		{
			this.tag = tag;
			this.args = args;
			this.global = global;
		}

		// Token: 0x0600A6B3 RID: 42675 RVA: 0x002FBD1B File Offset: 0x002F9F1B
		public Signal(string tag, NamedArgument arg1)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1);
			this.global = false;
		}

		// Token: 0x0600A6B4 RID: 42676 RVA: 0x002FBD37 File Offset: 0x002F9F37
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2);
			this.global = false;
		}

		// Token: 0x0600A6B5 RID: 42677 RVA: 0x002FBD54 File Offset: 0x002F9F54
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3);
			this.global = false;
		}

		// Token: 0x0600A6B6 RID: 42678 RVA: 0x002FBD73 File Offset: 0x002F9F73
		public Signal(string tag, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.tag = tag;
			this.args = new SignalArgs(arg1, arg2, arg3, arg4);
			this.global = false;
		}

		// Token: 0x0600A6B7 RID: 42679 RVA: 0x002FBD94 File Offset: 0x002F9F94
		public Signal(string tag, params NamedArgument[] args)
		{
			this.tag = tag;
			this.args = new SignalArgs(args);
			this.global = false;
		}

		// Token: 0x0600A6B8 RID: 42680 RVA: 0x002FBDB0 File Offset: 0x002F9FB0
		public override string ToString()
		{
			return this.tag;
		}

		// Token: 0x04007362 RID: 29538
		public string tag;

		// Token: 0x04007363 RID: 29539
		public SignalArgs args;

		// Token: 0x04007364 RID: 29540
		public bool global;
	}
}
