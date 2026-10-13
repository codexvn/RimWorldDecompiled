using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI.Group
{
	// Token: 0x02001058 RID: 4184
	public static class PsychicRitualGizmo
	{
		// Token: 0x06006615 RID: 26133 RVA: 0x00205596 File Offset: 0x00203796
		public static IEnumerable<Gizmo> GetGizmos(Thing target)
		{
			IOrderedEnumerable<PsychicRitualDef_InvocationCircle> orderedEnumerable = from ritualDef in PsychicRitualGizmo.VisibleRituals()
			orderby ritualDef.label
			select ritualDef;
			using (IEnumerator<PsychicRitualDef_InvocationCircle> enumerator = orderedEnumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PsychicRitualDef_InvocationCircle ritualDef = enumerator.Current;
					Command_PsychicRitual command_PsychicRitual = new Command_PsychicRitual
					{
						defaultLabel = ritualDef.LabelCap,
						defaultDesc = ritualDef.description + string.Format("\n\n{0}", ritualDef.TimeAndOfferingLabel()),
						action = delegate()
						{
							PsychicRitualGizmo.InitializePsychicRitual(ritualDef, target);
						},
						icon = ritualDef.uiIcon
					};
					AcceptanceReport report = Find.PsychicRitualManager.CanInvoke(ritualDef, target.Map);
					if (report)
					{
						command_PsychicRitual.Disabled = false;
						command_PsychicRitual.disabledReason = null;
					}
					else
					{
						command_PsychicRitual.Disable(report.Reason.CapitalizeFirst());
					}
					yield return command_PsychicRitual;
				}
			}
			IEnumerator<PsychicRitualDef_InvocationCircle> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006616 RID: 26134 RVA: 0x002055A8 File Offset: 0x002037A8
		public static Gizmo CancelGizmo(PsychicRitual psychicRitual)
		{
			Action <>9__1;
			return new Command_Action
			{
				defaultLabel = "CommandCancelPsychicRitual".Translate(psychicRitual.def.label).CapitalizeFirst(),
				defaultDesc = "CommandCancelPsychicRitualDesc".Translate(psychicRitual.def.label).CapitalizeFirst().EndWithPeriod(),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/CancelPsychicRitual_Gizmo", true),
				action = delegate()
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text = "CommandCancelPsychicRitualConfirm".Translate(psychicRitual.def.label);
					Action confirmedAct;
					if ((confirmedAct = <>9__1) == null)
					{
						confirmedAct = (<>9__1 = delegate()
						{
							psychicRitual.CancelPsychicRitual(TaggedString.Empty);
						});
					}
					windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				},
				hotKey = KeyBindingDefOf.Misc6
			};
		}

		// Token: 0x06006617 RID: 26135 RVA: 0x00205664 File Offset: 0x00203864
		public static Gizmo LeaveGizmo(PsychicRitual psychicRitual, Pawn pawn)
		{
			return new Command_Action
			{
				defaultLabel = "CommandLeavePsychicRitual".Translate(psychicRitual.def.label).CapitalizeFirst(),
				defaultDesc = "CommandLeavePsychicRitualDesc".Translate(psychicRitual.def.label).CapitalizeFirst().EndWithPeriod(),
				action = delegate()
				{
					psychicRitual.LeavePsychicRitual(pawn, TaggedString.Empty);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				},
				hotKey = KeyBindingDefOf.Misc5
			};
		}

		// Token: 0x06006618 RID: 26136 RVA: 0x00205714 File Offset: 0x00203914
		private static List<PsychicRitualDef_InvocationCircle> VisibleRituals()
		{
			PsychicRitualGizmo.tmpVisibleRituals.Clear();
			foreach (PsychicRitualDef psychicRitualDef in DefDatabase<PsychicRitualDef>.AllDefs)
			{
				if (psychicRitualDef.Visible)
				{
					PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = psychicRitualDef as PsychicRitualDef_InvocationCircle;
					if (psychicRitualDef_InvocationCircle != null)
					{
						PsychicRitualGizmo.tmpVisibleRituals.Add(psychicRitualDef_InvocationCircle);
					}
				}
			}
			return PsychicRitualGizmo.tmpVisibleRituals;
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x00205788 File Offset: 0x00203988
		private static void InitializePsychicRitual(PsychicRitualDef_InvocationCircle psychicRitualDef, Thing target)
		{
			TargetInfo target2 = new TargetInfo(target);
			PsychicRitualRoleAssignments assignments = psychicRitualDef.BuildRoleAssignments(target2);
			PsychicRitualCandidatePool candidatePool = psychicRitualDef.FindCandidatePool();
			Map currentMap = Find.CurrentMap;
			psychicRitualDef.InitializeCast(currentMap);
			Find.WindowStack.Add(new Dialog_BeginPsychicRitual(psychicRitualDef, candidatePool, assignments, currentMap));
		}

		// Token: 0x0400490C RID: 18700
		private static readonly List<PsychicRitualDef_InvocationCircle> tmpVisibleRituals = new List<PsychicRitualDef_InvocationCircle>(16);
	}
}
