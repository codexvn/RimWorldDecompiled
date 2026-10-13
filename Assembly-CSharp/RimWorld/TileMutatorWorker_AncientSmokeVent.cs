using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020CF RID: 8399
	public class TileMutatorWorker_AncientSmokeVent : TileMutatorWorker_AncientVent
	{
		// Token: 0x0600B79B RID: 47003 RVA: 0x0034E0FF File Offset: 0x0034C2FF
		public TileMutatorWorker_AncientSmokeVent(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x17001BE6 RID: 7142
		// (get) Token: 0x0600B79C RID: 47004 RVA: 0x0034E116 File Offset: 0x0034C316
		protected override ThingDef AncientVentDef
		{
			get
			{
				return ThingDefOf.AncientSmokeVent;
			}
		}
	}
}
