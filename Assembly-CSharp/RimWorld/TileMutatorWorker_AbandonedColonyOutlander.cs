using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020BA RID: 8378
	public class TileMutatorWorker_AbandonedColonyOutlander : TileMutatorWorker_AbandonedColony
	{
		// Token: 0x0600B762 RID: 46946 RVA: 0x0034CE80 File Offset: 0x0034B080
		public TileMutatorWorker_AbandonedColonyOutlander(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B763 RID: 46947 RVA: 0x0034CE8C File Offset: 0x0034B08C
		protected override Faction GetFaction()
		{
			Faction result;
			if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, false, true, TechLevel.Medieval, TechLevel.Spacer, false, false))
			{
				return result;
			}
			return null;
		}
	}
}
