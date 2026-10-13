using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200251C RID: 9500
	public class VirtualPawnRelation : IExposable
	{
		// Token: 0x0600CF9B RID: 53147 RVA: 0x000024DF File Offset: 0x000006DF
		public VirtualPawnRelation()
		{
		}

		// Token: 0x0600CF9C RID: 53148 RVA: 0x003CF68F File Offset: 0x003CD88F
		public VirtualPawnRelation(PawnRelationDef def, RelationshipRecord record, int startTicks)
		{
			this.def = def;
			this.record = record;
			this.startTicks = startTicks;
		}

		// Token: 0x0600CF9D RID: 53149 RVA: 0x003CF6AC File Offset: 0x003CD8AC
		public void ExposeData()
		{
			Scribe_Defs.Look<PawnRelationDef>(ref this.def, "def");
			Scribe_References.Look<RelationshipRecord>(ref this.record, "record", false);
			Scribe_Values.Look<int>(ref this.startTicks, "startTicks", 0, false);
		}

		// Token: 0x04008D59 RID: 36185
		public PawnRelationDef def;

		// Token: 0x04008D5A RID: 36186
		public RelationshipRecord record;

		// Token: 0x04008D5B RID: 36187
		public int startTicks;
	}
}
