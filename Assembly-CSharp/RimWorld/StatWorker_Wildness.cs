using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200375F RID: 14175
	public class StatWorker_Wildness : StatWorker
	{
		// Token: 0x06013A56 RID: 80470 RVA: 0x005D5BE4 File Offset: 0x005D3DE4
		public override bool ShouldShowFor(StatRequest req)
		{
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			if (pawn != null && (pawn.IsWildMan() || pawn.IsAnimal))
			{
				return true;
			}
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef != null)
			{
				RaceProperties race = thingDef.race;
				if (race != null && race.Animal)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013A57 RID: 80471 RVA: 0x005D5C44 File Offset: 0x005D3E44
		public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
			stringBuilder.AppendLine();
			ThingDef thingDef = req.Def as ThingDef;
			if (thingDef == null)
			{
				return stringBuilder.ToString();
			}
			Pawn pawn = req.Pawn ?? (req.Thing as Pawn);
			if (pawn != null)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", "TrainingDecayInterval".Translate(), TrainableUtility.DegradationPeriodTicks(pawn).ToStringTicksToDays("F1")));
				stringBuilder.AppendLine();
			}
			else
			{
				RaceProperties race = thingDef.race;
				if (race != null && !race.Humanlike)
				{
					stringBuilder.AppendLine(string.Format("{0}: {1}", "TrainingDecayInterval".Translate(), TrainableUtility.DegradationPeriodTicks(thingDef).ToStringTicksToDays("F1")));
					stringBuilder.AppendLine();
				}
			}
			if (pawn != null && !TrainableUtility.TamenessCanDecay(pawn))
			{
				string key = pawn.RaceProps.FenceBlocked ? "TamenessWillNotDecayFenceBlocked" : "TamenessWillNotDecay";
				stringBuilder.AppendLine(key.Translate());
			}
			else if (!TrainableUtility.TamenessCanDecay(thingDef))
			{
				string key2 = thingDef.race.FenceBlocked ? "TamenessWillNotDecayFenceBlocked" : "TamenessWillNotDecay";
				stringBuilder.AppendLine(key2.Translate());
			}
			return stringBuilder.ToString();
		}
	}
}
