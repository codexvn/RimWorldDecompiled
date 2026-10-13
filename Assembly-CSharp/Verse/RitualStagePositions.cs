using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200016F RID: 367
	public class RitualStagePositions : IExposable
	{
		// Token: 0x06000AA6 RID: 2726 RVA: 0x00032274 File Offset: 0x00030474
		public void ExposeData()
		{
			Scribe_Collections.Look<PawnRitualReference, PawnStagePosition>(ref this.referencePositions, "referencePosition", LookMode.Deep, LookMode.Deep, ref this.pawnRefListTmp, ref this.positionListTmp, true, false, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				Scribe_Collections.Look<Pawn, PawnStagePosition>(ref this.positionsTmp, "positions", LookMode.Reference, LookMode.Deep, ref this.pawnListTmp, ref this.positionListTmp, true, false, false);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Scribe_Collections.Look<Pawn, PawnStagePosition>(ref this.positionsTmp, "positions", LookMode.Reference, LookMode.Deep, ref this.pawnListTmp, ref this.positionListTmp, true, false, false);
				if (this.positionsTmp != null && this.positionsTmp.Count > 0)
				{
					if (this.referencePositions == null)
					{
						this.referencePositions = new Dictionary<PawnRitualReference, PawnStagePosition>();
					}
					using (Dictionary<Pawn, PawnStagePosition>.Enumerator enumerator = this.positionsTmp.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Pawn, PawnStagePosition> pawnPos = enumerator.Current;
							if (this.referencePositions.All((KeyValuePair<PawnRitualReference, PawnStagePosition> p) => p.Key.pawn != pawnPos.Key))
							{
								this.referencePositions.Add(new PawnRitualReference(pawnPos.Key), pawnPos.Value);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400059A RID: 1434
		public Dictionary<PawnRitualReference, PawnStagePosition> referencePositions = new Dictionary<PawnRitualReference, PawnStagePosition>();

		// Token: 0x0400059B RID: 1435
		private Dictionary<Pawn, PawnStagePosition> positionsTmp = new Dictionary<Pawn, PawnStagePosition>();

		// Token: 0x0400059C RID: 1436
		private List<Pawn> pawnListTmp;

		// Token: 0x0400059D RID: 1437
		private List<PawnRitualReference> pawnRefListTmp;

		// Token: 0x0400059E RID: 1438
		private List<PawnStagePosition> positionListTmp;
	}
}
