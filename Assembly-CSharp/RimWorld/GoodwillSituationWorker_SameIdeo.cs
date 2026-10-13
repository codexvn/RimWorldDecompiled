using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200259F RID: 9631
	public class GoodwillSituationWorker_SameIdeo : GoodwillSituationWorker
	{
		// Token: 0x0600D313 RID: 54035 RVA: 0x003E2B06 File Offset: 0x003E0D06
		public override string GetPostProcessedLabel(Faction other)
		{
			return this.def.label.Formatted(Faction.OfPlayer.ideos.PrimaryIdeo);
		}

		// Token: 0x0600D314 RID: 54036 RVA: 0x003E2B34 File Offset: 0x003E0D34
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			if (Find.IdeoManager.classicMode)
			{
				return 0;
			}
			Ideo primaryIdeo = Faction.OfPlayer.ideos.PrimaryIdeo;
			if (primaryIdeo == null || primaryIdeo != other.ideos.PrimaryIdeo)
			{
				return 0;
			}
			return this.def.naturalGoodwillOffset;
		}
	}
}
