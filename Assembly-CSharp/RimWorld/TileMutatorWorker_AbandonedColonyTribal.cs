using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020BB RID: 8379
	public class TileMutatorWorker_AbandonedColonyTribal : TileMutatorWorker_AbandonedColony
	{
		// Token: 0x0600B764 RID: 46948 RVA: 0x0034CE80 File Offset: 0x0034B080
		public TileMutatorWorker_AbandonedColonyTribal(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B765 RID: 46949 RVA: 0x0034CEB0 File Offset: 0x0034B0B0
		protected override Faction GetFaction()
		{
			Faction result;
			if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out result, false, true, TechLevel.Animal, TechLevel.Neolithic, false, false))
			{
				return result;
			}
			return null;
		}
	}
}
