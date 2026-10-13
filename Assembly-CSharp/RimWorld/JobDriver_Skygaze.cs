using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011EE RID: 4590
	public class JobDriver_Skygaze : JobDriver
	{
		// Token: 0x0600701F RID: 28703 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06007020 RID: 28704 RVA: 0x0022D3E7 File Offset: 0x0022B5E7
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.pawn.jobs.posture = PawnPosture.LayingOnGroundFaceUp;
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				float extraJoyGainFactor = this.pawn.Map.gameConditionManager.AggregateSkyGazeJoyGainFactor(this.pawn.Map);
				JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, null);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.FailOn(() => this.pawn.Position.Roofed(this.pawn.Map));
			toil.FailOn(() => this.pawn.Map.weatherManager.curWeather.preventSkygaze);
			toil.FailOn(() => !JoyUtility.EnjoyableOutsideNow(this.pawn, null));
			yield return toil;
			yield break;
		}

		// Token: 0x06007021 RID: 28705 RVA: 0x0022D3F8 File Offset: 0x0022B5F8
		public override string GetReport()
		{
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse))
			{
				return "WatchingEclipse".Translate();
			}
			if (base.Map.gameConditionManager.ConditionIsActive(GameConditionDefOf.Aurora))
			{
				return "WatchingAurora".Translate();
			}
			float num = GenCelestial.CurCelestialSunGlow(base.Map);
			if (num < 0.1f)
			{
				return "Stargazing".Translate();
			}
			if (num >= 0.65f)
			{
				return "CloudWatching".Translate();
			}
			if (GenLocalDate.DayPercent(this.pawn) < 0.5f)
			{
				return "WatchingSunrise".Translate();
			}
			return "WatchingSunset".Translate();
		}
	}
}
