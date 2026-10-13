using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002543 RID: 9539
	public class Thought_MemoryObservationTerror : Thought_MemoryObservation
	{
		// Token: 0x1700201F RID: 8223
		// (get) Token: 0x0600D0BC RID: 53436 RVA: 0x003D60D0 File Offset: 0x003D42D0
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.target.Named("THING"), this.pawn.Named("PAWN"));
			}
		}

		// Token: 0x17002020 RID: 8224
		// (get) Token: 0x0600D0BD RID: 53437 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Save
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002021 RID: 8225
		// (set) Token: 0x0600D0BE RID: 53438 RVA: 0x003D6107 File Offset: 0x003D4307
		public override Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
				this.target = value;
			}
		}

		// Token: 0x04008DE4 RID: 36324
		public int intensity;

		// Token: 0x04008DE5 RID: 36325
		private Thing target;
	}
}
