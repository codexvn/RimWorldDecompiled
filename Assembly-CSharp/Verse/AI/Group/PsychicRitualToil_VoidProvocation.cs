using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200108D RID: 4237
	public class PsychicRitualToil_VoidProvocation : PsychicRitualToil
	{
		// Token: 0x060067AA RID: 26538 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_VoidProvocation()
		{
		}

		// Token: 0x060067AB RID: 26539 RVA: 0x0020D651 File Offset: 0x0020B851
		public PsychicRitualToil_VoidProvocation(PsychicRitualRoleDef invokerRole, SimpleCurve psychicShockChanceFromQualityCurve)
		{
			this.invokerRole = invokerRole;
			this.psychicShockChanceFromQualityCurve = psychicShockChanceFromQualityCurve;
		}

		// Token: 0x060067AC RID: 26540 RVA: 0x0020D668 File Offset: 0x0020B868
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn);
			}
		}

		// Token: 0x060067AD RID: 26541 RVA: 0x0020D694 File Offset: 0x0020B894
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker)
		{
			PsychicRitualDef_VoidProvocation psychicRitualDef_VoidProvocation = (PsychicRitualDef_VoidProvocation)psychicRitual.def;
			Map map = psychicRitual.Map;
			List<IncidentDef> list = new List<IncidentDef>();
			bool flag = false;
			foreach (EntityCategoryDef entityCategoryDef in from x in DefDatabase<EntityCategoryDef>.AllDefs
			orderby x.listOrder
			select x)
			{
				foreach (EntityCodexEntryDef entityCodexEntryDef in DefDatabase<EntityCodexEntryDef>.AllDefs)
				{
					if (entityCodexEntryDef.category == entityCategoryDef && !entityCodexEntryDef.provocationIncidents.NullOrEmpty<IncidentDef>() && !entityCodexEntryDef.Discovered)
					{
						foreach (IncidentDef incidentDef in entityCodexEntryDef.provocationIncidents)
						{
							IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incidentDef.category, map);
							incidentParms.bypassStorytellerSettings = true;
							if (incidentDef.Worker.CanFireNow(incidentParms))
							{
								list.Add(incidentDef);
								flag = true;
							}
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!list.Any<IncidentDef>())
			{
				foreach (EntityCodexEntryDef entityCodexEntryDef2 in DefDatabase<EntityCodexEntryDef>.AllDefs)
				{
					if (!entityCodexEntryDef2.provocationIncidents.NullOrEmpty<IncidentDef>())
					{
						foreach (IncidentDef incidentDef2 in entityCodexEntryDef2.provocationIncidents)
						{
							IncidentParms incidentParms2 = StorytellerUtility.DefaultParmsNow(incidentDef2.category, map);
							incidentParms2.bypassStorytellerSettings = true;
							if (incidentDef2.Worker.CanFireNow(incidentParms2))
							{
								list.Add(incidentDef2);
							}
						}
					}
				}
			}
			IncidentDef incidentDef3;
			bool flag2;
			if (list.TryRandomElement(out incidentDef3))
			{
				flag2 = true;
				IncidentParms incidentParms3 = StorytellerUtility.DefaultParmsNow(incidentDef3.category, map);
				incidentParms3.bypassStorytellerSettings = true;
				Find.Storyteller.incidentQueue.Add(incidentDef3, Find.TickManager.TicksGame + Mathf.RoundToInt(psychicRitualDef_VoidProvocation.incidentDelayHoursRange.RandomInRange * 2500f), incidentParms3, 0);
			}
			else
			{
				flag2 = false;
			}
			TaggedString taggedString = "VoidProvocationCompletedText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL")) + "\n\n" + (flag2 ? "VoidProvocationSucceeded" : "VoidProvocationFailed").Translate();
			if (flag2 && Rand.Chance(psychicRitualDef_VoidProvocation.psychicShockChanceFromQualityCurve.Evaluate(psychicRitual.PowerPercent)))
			{
				taggedString += "\n\n" + "VoidProvocationDarkPsychicShock".Translate(invoker.Named("INVOKER"));
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.DarkPsychicShock, invoker, null);
				int duration = Mathf.RoundToInt(psychicRitualDef_VoidProvocation.darkPsychicShockDurarionHoursRange.RandomInRange * 2500f);
				HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
				if (hediffComp_Disappears != null)
				{
					hediffComp_Disappears.SetDuration(duration);
				}
				invoker.health.AddHediff(hediff, null, null, null);
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label).CapitalizeFirst(), taggedString, LetterDefOf.NeutralEvent, null, 0, true);
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (pawn.needs.mood.thoughts.memories.NumMemoriesOfDef(ThoughtDefOf.VoidCuriosity) > 0)
				{
					pawn.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.VoidCuriosity);
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.VoidCuriositySatisfied, null, null);
				}
			}
			Find.Anomaly.hasPerformedVoidProvocation = true;
		}

		// Token: 0x060067AE RID: 26542 RVA: 0x0020DAE4 File Offset: 0x0020BCE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Deep.Look<SimpleCurve>(ref this.psychicShockChanceFromQualityCurve, "psychicShockChanceFromQualityCurve", Array.Empty<object>());
		}

		// Token: 0x040049DC RID: 18908
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x040049DD RID: 18909
		private SimpleCurve psychicShockChanceFromQualityCurve;
	}
}
