using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB2 RID: 7090
	public class QuestPart_Filter_ArgEqual : QuestPart_Filter
	{
		// Token: 0x06009DF2 RID: 40434 RVA: 0x002DBA98 File Offset: 0x002D9C98
		protected override bool Pass(SignalArgs args)
		{
			NamedArgument namedArgument;
			return args.TryGetArg(this.name, out namedArgument) && object.Equals(this.obj, namedArgument.arg);
		}

		// Token: 0x06009DF3 RID: 40435 RVA: 0x002DBAC9 File Offset: 0x002D9CC9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Universal.Look<object>(ref this.obj, "obj", ref this.objLookMode, ref this.objType, false);
		}

		// Token: 0x06009DF4 RID: 40436 RVA: 0x002DBB00 File Offset: 0x002D9D00
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.name = "test";
			this.obj = "value";
		}

		// Token: 0x04006D61 RID: 28001
		public string name;

		// Token: 0x04006D62 RID: 28002
		public object obj;

		// Token: 0x04006D63 RID: 28003
		public LookMode objLookMode;

		// Token: 0x04006D64 RID: 28004
		private Type objType;
	}
}
