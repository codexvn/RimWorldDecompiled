using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB3 RID: 7091
	public class QuestPart_Filter_ArgNotEqual : QuestPart_Filter
	{
		// Token: 0x06009DF6 RID: 40438 RVA: 0x002DBB20 File Offset: 0x002D9D20
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return !args.TryGetArg(this.name, out namedArgument) || !object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06009DF7 RID: 40439 RVA: 0x002DBB54 File Offset: 0x002D9D54
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06009DF8 RID: 40440 RVA: 0x002DBB8B File Offset: 0x002D9D8B
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x04006D65 RID: 28005
		public string name;

		// Token: 0x04006D66 RID: 28006
		public object obj;

		// Token: 0x04006D67 RID: 28007
		private Type objType;

		// Token: 0x04006D68 RID: 28008
		private LookMode objLookMode;
	}
}
