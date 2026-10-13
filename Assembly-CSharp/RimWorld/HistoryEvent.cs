using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B60 RID: 7008
	public struct HistoryEvent
	{
		// Token: 0x06009C8A RID: 40074 RVA: 0x002D67D0 File Offset: 0x002D49D0
		public HistoryEvent(HistoryEventDef def)
		{
			this.def = def;
			this.args = default(SignalArgs);
		}

		// Token: 0x06009C8B RID: 40075 RVA: 0x002D67E5 File Offset: 0x002D49E5
		public HistoryEvent(HistoryEventDef def, SignalArgs args)
		{
			this.def = def;
			this.args = args;
		}

		// Token: 0x06009C8C RID: 40076 RVA: 0x002D67F5 File Offset: 0x002D49F5
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1)
		{
			this.def = def;
			this.args = new SignalArgs(arg1);
		}

		// Token: 0x06009C8D RID: 40077 RVA: 0x002D680A File Offset: 0x002D4A0A
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2);
		}

		// Token: 0x06009C8E RID: 40078 RVA: 0x002D6820 File Offset: 0x002D4A20
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2, arg3);
		}

		// Token: 0x06009C8F RID: 40079 RVA: 0x002D6838 File Offset: 0x002D4A38
		public HistoryEvent(HistoryEventDef def, NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.def = def;
			this.args = new SignalArgs(arg1, arg2, arg3, arg4);
		}

		// Token: 0x06009C90 RID: 40080 RVA: 0x002D6852 File Offset: 0x002D4A52
		public HistoryEvent(HistoryEventDef def, params NamedArgument[] args)
		{
			this.def = def;
			this.args = new SignalArgs(args);
		}

		// Token: 0x04006C8E RID: 27790
		public HistoryEventDef def;

		// Token: 0x04006C8F RID: 27791
		public SignalArgs args;
	}
}
