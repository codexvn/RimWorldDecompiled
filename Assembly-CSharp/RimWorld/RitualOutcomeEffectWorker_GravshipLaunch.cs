using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002781 RID: 10113
	public class RitualOutcomeEffectWorker_GravshipLaunch : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x0600DDB6 RID: 56758 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_GravshipLaunch()
		{
		}

		// Token: 0x0600DDB7 RID: 56759 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_GravshipLaunch(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DDB8 RID: 56760 RVA: 0x00414190 File Offset: 0x00412390
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (progress < 1f)
			{
				Messages.Message("GravshipLaunchInterrupted".Translate(), MessageTypeDefOf.NegativeEvent, true);
				return;
			}
			try
			{
				Thing thing = jobRitual.selectedTarget.Thing;
				CompPilotConsole compPilotConsole = (thing != null) ? thing.TryGetComp<CompPilotConsole>() : null;
				float quality = base.GetQuality(jobRitual, progress);
				compPilotConsole.engine.launchInfo = new LaunchInfo
				{
					pilot = jobRitual.PawnWithRole("pilot"),
					copilot = jobRitual.PawnWithRole("copilot"),
					quality = quality,
					doNegativeOutcome = Rand.Chance(GravshipUtility.NegativeLandingOutcomeFromQuality(quality))
				};
				if (jobRitual.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
				{
					compPilotConsole.StartChoosingDestination();
				}
				else
				{
					GravshipUtility.PreLaunchConfirmation(compPilotConsole.engine, new Action(compPilotConsole.StartChoosingDestination));
				}
			}
			catch (Exception ex)
			{
				string str = "Error launching gravship: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}
	}
}
