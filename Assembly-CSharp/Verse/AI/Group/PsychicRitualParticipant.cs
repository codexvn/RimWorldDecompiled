using System;

namespace Verse.AI.Group
{
	// Token: 0x02001061 RID: 4193
	public class PsychicRitualParticipant : IExposable
	{
		// Token: 0x06006656 RID: 26198 RVA: 0x000024DF File Offset: 0x000006DF
		public PsychicRitualParticipant()
		{
		}

		// Token: 0x06006657 RID: 26199 RVA: 0x002063A4 File Offset: 0x002045A4
		public PsychicRitualParticipant(ValueTuple<Pawn, IntVec3> pair)
		{
			this.pawn = pair.Item1;
			this.location = pair.Item2;
		}

		// Token: 0x06006658 RID: 26200 RVA: 0x002063D1 File Offset: 0x002045D1
		public void Deconstruct(out Pawn pawn, out IntVec3 location)
		{
			pawn = this.pawn;
			location = this.location;
		}

		// Token: 0x06006659 RID: 26201 RVA: 0x002063E8 File Offset: 0x002045E8
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<IntVec3>(ref this.location, "location", default(IntVec3), false);
		}

		// Token: 0x04004925 RID: 18725
		public Pawn pawn;

		// Token: 0x04004926 RID: 18726
		public IntVec3 location;
	}
}
