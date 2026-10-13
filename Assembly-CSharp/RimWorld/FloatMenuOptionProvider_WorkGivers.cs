using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003348 RID: 13128
	public class FloatMenuOptionProvider_WorkGivers : FloatMenuOptionProvider
	{
		// Token: 0x17003014 RID: 12308
		// (get) Token: 0x06012733 RID: 75571 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003015 RID: 12309
		// (get) Token: 0x06012734 RID: 75572 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003016 RID: 12310
		// (get) Token: 0x06012735 RID: 75573 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003017 RID: 12311
		// (get) Token: 0x06012736 RID: 75574 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool MechanoidCanDo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003018 RID: 12312
		// (get) Token: 0x06012737 RID: 75575 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanSelfTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012738 RID: 75576 RVA: 0x0056FF76 File Offset: 0x0056E176
		public override IEnumerable<FloatMenuOption> GetOptions(FloatMenuContext context)
		{
			FloatMenuOptionProvider_WorkGivers.shouldProcessEquivalenceGroups = false;
			if (FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage == null || FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage.Length != DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount)
			{
				FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage = new FloatMenuOption[DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount];
			}
			FloatMenuOptionProvider_WorkGivers.tmpUsedLabels.Clear();
			foreach (FloatMenuOption floatMenuOption in this.GetWorkGiversOptionsFor(context.FirstSelectedPawn, context.ClickedCell, context))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06012739 RID: 75577 RVA: 0x0056FF8D File Offset: 0x0056E18D
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_WorkGivers.shouldProcessEquivalenceGroups = false;
			foreach (FloatMenuOption floatMenuOption in this.GetWorkGiversOptionsFor(context.FirstSelectedPawn, clickedThing, context))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (FloatMenuOptionProvider_WorkGivers.shouldProcessEquivalenceGroups)
			{
				int num;
				for (int i = 0; i < FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage.Length; i = num)
				{
					if (FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[i] != null)
					{
						FloatMenuOptionProvider_WorkGivers.tmpUsedLabels.Add(FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[i].Label);
						yield return FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[i];
						FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[i] = null;
					}
					num = i + 1;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0601273A RID: 75578 RVA: 0x0056FFAB File Offset: 0x0056E1AB
		private IEnumerable<FloatMenuOption> GetWorkGiversOptionsFor(Pawn pawn, LocalTargetInfo target, FloatMenuContext context)
		{
			if (pawn.thinker.TryGetMainTreeThinkNode<JobGiver_Work>() == null)
			{
				yield break;
			}
			FloatMenuOptionProvider_WorkGivers.tmpFloatMenuOptions.Clear();
			foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefsListForReading)
			{
				foreach (WorkGiverDef workGiverDef in workTypeDef.workGiversByPriority)
				{
					try
					{
						FloatMenuOption workGiverOption = this.GetWorkGiverOption(pawn, workGiverDef, target, context);
						if (workGiverOption != null)
						{
							FloatMenuOptionProvider_WorkGivers.tmpUsedLabels.Add(workGiverOption.Label);
							FloatMenuOptionProvider_WorkGivers.tmpFloatMenuOptions.Add(workGiverOption);
						}
					}
					catch (Exception ex)
					{
						string str = "Exception in WorkGiverOptionProvider_WorkGivers.GetWorkGiversOptionsFor for ";
						string defName = workGiverDef.defName;
						string str2 = ": ";
						Exception ex2 = ex;
						Log.Error(str + defName + str2 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			foreach (FloatMenuOption floatMenuOption in FloatMenuOptionProvider_WorkGivers.tmpFloatMenuOptions)
			{
				yield return floatMenuOption;
			}
			List<FloatMenuOption>.Enumerator enumerator3 = default(List<FloatMenuOption>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0601273B RID: 75579 RVA: 0x0056FFD0 File Offset: 0x0056E1D0
		private FloatMenuOption GetWorkGiverOption(Pawn pawn, WorkGiverDef workGiver, LocalTargetInfo target, FloatMenuContext context)
		{
			if (pawn.Drafted && !workGiver.canBeDoneWhileDrafted)
			{
				return null;
			}
			WorkGiver_Scanner workGiver_Scanner = workGiver.Worker as WorkGiver_Scanner;
			if (workGiver_Scanner == null || !workGiver_Scanner.def.directOrderable)
			{
				return null;
			}
			JobFailReason.Clear();
			if (target.HasThing)
			{
				if (FloatMenuOptionProvider_WorkGivers.ScannerShouldSkip(pawn, workGiver_Scanner, target.Thing))
				{
					return null;
				}
			}
			else if (!workGiver_Scanner.PotentialWorkCellsGlobal(pawn).Contains(target.Cell) || workGiver_Scanner.ShouldSkip(pawn, true))
			{
				return null;
			}
			Action action = null;
			Job job;
			if (target.HasThing)
			{
				if (!workGiver_Scanner.HasJobOnThing(pawn, target.Thing, true))
				{
					job = null;
				}
				else
				{
					job = workGiver_Scanner.JobOnThing(pawn, target.Thing, true);
				}
			}
			else if (!workGiver_Scanner.HasJobOnCell(pawn, target.Cell, true))
			{
				job = null;
			}
			else
			{
				job = workGiver_Scanner.JobOnCell(pawn, target.Cell, true);
			}
			string text;
			if (!JobFailReason.CustomJobString.NullOrEmpty())
			{
				text = "CannotGenericWorkCustom".Translate(JobFailReason.CustomJobString).CapitalizeFirst();
			}
			else if (target.HasThing)
			{
				text = "CannotGenericWork".Translate(workGiver.verb, target.Thing.Label);
			}
			else
			{
				text = "CannotGenericWork".Translate(workGiver.verb, "AreaLower".Translate());
			}
			if (job == null)
			{
				if (!JobFailReason.HaveReason)
				{
					return null;
				}
				text = text + ": " + JobFailReason.Reason.CapitalizeFirst();
			}
			else
			{
				WorkTypeDef workType = workGiver_Scanner.def.workType;
				PawnCapacityDef pawnCapacityDef = workGiver_Scanner.MissingRequiredCapacity(pawn);
				if (pawnCapacityDef != null)
				{
					text += ": " + "CannotMissingHealthActivities".Translate(pawnCapacityDef.label);
				}
				else if (pawn.WorkTagIsDisabled(workGiver_Scanner.def.workTags))
				{
					text += ": " + "CannotPrioritizeWorkGiverDisabled".Translate(workGiver_Scanner.def.label);
				}
				else if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(pawn, job))
				{
					if (target.HasThing)
					{
						text = "CannotGenericAlreadyAm".Translate(workGiver_Scanner.PostProcessedGerund(job), target.Thing.LabelShort, target.Thing);
					}
					else
					{
						text = "CannotGenericAlreadyAmCustom".Translate(workGiver_Scanner.PostProcessedGerund(job));
					}
				}
				else if (pawn.workSettings.GetPriority(workType) == 0)
				{
					if (pawn.WorkTypeIsDisabled(workType))
					{
						text += ": " + "CannotPrioritizeWorkTypeDisabled".Translate(workType.gerundLabel);
					}
					else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
					{
						text += ": " + "CannotPrioritizeNotAssignedToWorkType".Translate(workType.gerundLabel);
					}
					else
					{
						text += ": " + "CannotPrioritizeWorkTypeDisabled".Translate(workType.pawnLabel);
					}
				}
				else if (job.def == JobDefOf.Research && target.Thing is Building_ResearchBench)
				{
					text += ": " + "CannotPrioritizeResearch".Translate();
				}
				else if (target.HasThing && target.Thing.IsForbidden(pawn))
				{
					if (!target.Thing.Position.InAllowedArea(pawn))
					{
						text += ": " + "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + (" (" + pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")");
					}
					else
					{
						text += ": " + "CannotPrioritizeForbidden".Translate(target.Thing.Label, target.Thing);
					}
				}
				else if (!target.HasThing && target.Cell.IsForbidden(pawn))
				{
					if (!target.Cell.InAllowedArea(pawn))
					{
						text += ": " + "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + (" (" + pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")");
					}
					else
					{
						text += ": " + "CannotPrioritizeCellForbidden".Translate();
					}
				}
				else if ((target.HasThing && !pawn.CanReach(target.Thing, workGiver_Scanner.PathEndMode, Danger.Deadly, false, false, TraverseMode.ByPawn)) || (!target.HasThing && !pawn.CanReach(target.Cell, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
				{
					text += ": " + "NoPath".Translate().CapitalizeFirst();
				}
				else
				{
					if (target.HasThing)
					{
						text = "PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), target.Thing.Label).CapitalizeFirst();
					}
					else if (job.def.displayAsAreaInFloatMenu)
					{
						text = "PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), "AreaLower".Translate()).CapitalizeFirst();
					}
					else
					{
						text = "PrioritizeGenericSimple".Translate(workGiver_Scanner.PostProcessedGerund(job)).CapitalizeFirst();
					}
					Job localJob = job;
					WorkGiver_Scanner localScanner = workGiver_Scanner;
					job.workGiverDef = workGiver_Scanner.def;
					WorkGiverDef giver = workGiver;
					action = delegate()
					{
						if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, context.ClickedCell))
						{
							if (giver.forceMote != null)
							{
								MoteMaker.MakeStaticMote(context.ClickedCell, pawn.Map, giver.forceMote, 1f);
							}
							if (giver.forceFleck != null)
							{
								FleckMaker.Static(context.ClickedCell, pawn.Map, giver.forceFleck, 1f);
							}
						}
					};
				}
			}
			if (DebugViewSettings.showFloatMenuWorkGivers)
			{
				text += string.Format(" (from {0})", workGiver.defName);
			}
			FloatMenuOption menuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), pawn, target, "ReservedBy", null);
			if (FloatMenuOptionProvider_WorkGivers.tmpUsedLabels.Any((string used) => used == menuOption.Label))
			{
				return null;
			}
			if (pawn.Drafted && workGiver.autoTakeablePriorityDrafted != -1)
			{
				menuOption.autoTakeable = true;
				menuOption.autoTakeablePriority = (float)workGiver.autoTakeablePriorityDrafted;
			}
			if (target.HasThing && workGiver.equivalenceGroup != null)
			{
				if (FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] == null || (FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index].Disabled && !menuOption.Disabled))
				{
					FloatMenuOptionProvider_WorkGivers.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] = menuOption;
					FloatMenuOptionProvider_WorkGivers.shouldProcessEquivalenceGroups = true;
				}
				return null;
			}
			return menuOption;
		}

		// Token: 0x0601273C RID: 75580 RVA: 0x005707C8 File Offset: 0x0056E9C8
		private static bool ScannerShouldSkip(Pawn pawn, WorkGiver_Scanner scanner, Thing t)
		{
			return (!scanner.PotentialWorkThingRequest.Accepts(t) && (scanner.PotentialWorkThingsGlobal(pawn) == null || !scanner.PotentialWorkThingsGlobal(pawn).Contains(t))) || scanner.ShouldSkip(pawn, true);
		}

		// Token: 0x0400BF9F RID: 49055
		private static bool shouldProcessEquivalenceGroups = false;

		// Token: 0x0400BFA0 RID: 49056
		private static FloatMenuOption[] equivalenceGroupTempStorage;

		// Token: 0x0400BFA1 RID: 49057
		private static HashSet<string> tmpUsedLabels = new HashSet<string>();

		// Token: 0x0400BFA2 RID: 49058
		private static List<FloatMenuOption> tmpFloatMenuOptions = new List<FloatMenuOption>();
	}
}
