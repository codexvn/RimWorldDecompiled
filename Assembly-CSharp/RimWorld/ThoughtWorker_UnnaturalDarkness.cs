using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D0 RID: 6352
	public class ThoughtWorker_UnnaturalDarkness : ThoughtWorker_GameCondition
	{
		// Token: 0x060090C5 RID: 37061 RVA: 0x002AA248 File Offset: 0x002A8448
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.AnomalyActive || !base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			if (p.MapHeld.weatherManager.curWeather != WeatherDefOf.UnnaturalDarkness_Stage2)
			{
				return ThoughtState.Inactive;
			}
			if (p.Inhumanized())
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.ActiveAtStage(0);
		}
	}
}
