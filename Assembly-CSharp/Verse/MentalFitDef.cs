using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002A8 RID: 680
	public class MentalFitDef : Def
	{
		// Token: 0x060010C8 RID: 4296 RVA: 0x0004F2F7 File Offset: 0x0004D4F7
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.mentalState == null)
			{
				yield return "mentalState not set.";
			}
			if (this.mtbDaysMoodCurve == null)
			{
				yield return "mtbDaysMoodCurve not set.";
			}
			yield break;
			yield break;
		}

		// Token: 0x060010C9 RID: 4297 RVA: 0x0004F308 File Offset: 0x0004D508
		public float CalculateMTBDays(Pawn pawn)
		{
			if (!this.developmentalStageFilter.Has(pawn.DevelopmentalStage))
			{
				return float.PositiveInfinity;
			}
			if (pawn.needs.mood == null)
			{
				return float.PositiveInfinity;
			}
			SimpleCurve simpleCurve = this.mtbDaysMoodCurve;
			if (simpleCurve == null)
			{
				return float.PositiveInfinity;
			}
			return simpleCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
		}

		// Token: 0x04000E20 RID: 3616
		public MentalStateDef mentalState;

		// Token: 0x04000E21 RID: 3617
		public SimpleCurve mtbDaysMoodCurve;

		// Token: 0x04000E22 RID: 3618
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;
	}
}
