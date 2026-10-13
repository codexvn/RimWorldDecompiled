using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CF5 RID: 11509
	public class CompInspectStringEmergence : CompInspectString
	{
		// Token: 0x060102A6 RID: 66214 RVA: 0x004AD4D0 File Offset: 0x004AB6D0
		public override string CompInspectStringExtra()
		{
			if (this.sourcePawn != null)
			{
				return base.Props.inspectString.Formatted(this.sourcePawn.Named("SOURCEPAWN")).Resolve();
			}
			return null;
		}

		// Token: 0x060102A7 RID: 66215 RVA: 0x004AD50F File Offset: 0x004AB70F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Pawn>(ref this.sourcePawn, "sourcePawn", false);
		}

		// Token: 0x0400A8D4 RID: 43220
		public Pawn sourcePawn;
	}
}
