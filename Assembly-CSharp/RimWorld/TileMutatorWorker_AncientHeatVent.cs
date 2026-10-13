using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020CD RID: 8397
	public class TileMutatorWorker_AncientHeatVent : TileMutatorWorker_AncientVent
	{
		// Token: 0x0600B797 RID: 46999 RVA: 0x0034E0FF File Offset: 0x0034C2FF
		public TileMutatorWorker_AncientHeatVent(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x17001BE4 RID: 7140
		// (get) Token: 0x0600B798 RID: 47000 RVA: 0x0034E108 File Offset: 0x0034C308
		protected override ThingDef AncientVentDef
		{
			get
			{
				return ThingDefOf.AncientHeatVent;
			}
		}
	}
}
