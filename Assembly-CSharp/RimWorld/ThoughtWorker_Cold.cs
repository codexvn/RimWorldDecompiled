using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001875 RID: 6261
	public class ThoughtWorker_Cold : ThoughtWorker
	{
		// Token: 0x06008FD1 RID: 36817 RVA: 0x002A73B0 File Offset: 0x002A55B0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float statValue = p.GetStatValue(StatDefOf.ComfyTemperatureMin, true, 10);
			float ambientTemperature = p.AmbientTemperature;
			float num = statValue - ambientTemperature;
			if (num <= 0f)
			{
				return ThoughtState.Inactive;
			}
			int num2;
			if (num < 10f)
			{
				num2 = 0;
			}
			else if (num < 20f)
			{
				num2 = 1;
			}
			else if (num < 30f)
			{
				num2 = 2;
			}
			else
			{
				num2 = 3;
			}
			if (ModsConfig.IdeologyActive)
			{
				Ideo ideo = p.Ideo;
				if (ideo != null && ideo.HasPrecept(PreceptDefOf.Temperature_Tough))
				{
					num2 -= 2;
				}
			}
			if (num2 >= 0)
			{
				return ThoughtState.ActiveAtStage(num2);
			}
			return ThoughtState.Inactive;
		}
	}
}
