using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002184 RID: 8580
	public class MechClusterSketch : IExposable
	{
		// Token: 0x0600BB1C RID: 47900 RVA: 0x000024DF File Offset: 0x000006DF
		public MechClusterSketch()
		{
		}

		// Token: 0x0600BB1D RID: 47901 RVA: 0x003610D0 File Offset: 0x0035F2D0
		public MechClusterSketch(Sketch buildingsSketch, List<MechClusterSketch.Mech> pawns, bool startDormant)
		{
			this.buildingsSketch = buildingsSketch;
			this.pawns = pawns;
			this.startDormant = startDormant;
		}

		// Token: 0x0600BB1E RID: 47902 RVA: 0x003610ED File Offset: 0x0035F2ED
		public void ExposeData()
		{
			Scribe_Deep.Look<Sketch>(ref this.buildingsSketch, "buildingsSketch", Array.Empty<object>());
			Scribe_Collections.Look<MechClusterSketch.Mech>(ref this.pawns, "pawns", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.startDormant, "startDormant", false, false);
		}

		// Token: 0x040080E8 RID: 33000
		public Sketch buildingsSketch;

		// Token: 0x040080E9 RID: 33001
		public List<MechClusterSketch.Mech> pawns;

		// Token: 0x040080EA RID: 33002
		public bool startDormant;

		// Token: 0x02002185 RID: 8581
		public struct Mech : IExposable
		{
			// Token: 0x0600BB1F RID: 47903 RVA: 0x0036112C File Offset: 0x0035F32C
			public Mech(PawnKindDef kindDef)
			{
				this.kindDef = kindDef;
				this.position = IntVec3.Invalid;
			}

			// Token: 0x0600BB20 RID: 47904 RVA: 0x00361140 File Offset: 0x0035F340
			public void ExposeData()
			{
				Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
				Scribe_Values.Look<IntVec3>(ref this.position, "position", default(IntVec3), false);
			}

			// Token: 0x040080EB RID: 33003
			public PawnKindDef kindDef;

			// Token: 0x040080EC RID: 33004
			public IntVec3 position;
		}
	}
}
