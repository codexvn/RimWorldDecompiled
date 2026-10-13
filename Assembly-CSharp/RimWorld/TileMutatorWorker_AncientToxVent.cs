using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020CE RID: 8398
	public class TileMutatorWorker_AncientToxVent : TileMutatorWorker_AncientVent
	{
		// Token: 0x0600B799 RID: 47001 RVA: 0x0034E0FF File Offset: 0x0034C2FF
		public TileMutatorWorker_AncientToxVent(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x17001BE5 RID: 7141
		// (get) Token: 0x0600B79A RID: 47002 RVA: 0x0034E10F File Offset: 0x0034C30F
		protected override ThingDef AncientVentDef
		{
			get
			{
				return ThingDefOf.AncientToxVent;
			}
		}
	}
}
