using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034F1 RID: 13553
	public static class TutorSystem
	{
		// Token: 0x1700319A RID: 12698
		// (get) Token: 0x060130A8 RID: 77992 RVA: 0x005A495B File Offset: 0x005A2B5B
		public static bool TutorialMode
		{
			get
			{
				return Find.Storyteller != null && Find.Storyteller.def != null && Find.Storyteller.def.tutorialMode;
			}
		}

		// Token: 0x1700319B RID: 12699
		// (get) Token: 0x060130A9 RID: 77993 RVA: 0x005A4981 File Offset: 0x005A2B81
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.AdaptiveTrainingEnabled && (Find.Storyteller == null || Find.Storyteller.def == null || !Find.Storyteller.def.disableAdaptiveTraining);
			}
		}

		// Token: 0x060130AA RID: 77994 RVA: 0x005A49B3 File Offset: 0x005A2BB3
		public static void Notify_Event(string eventTag, IntVec3 cell)
		{
			TutorSystem.Notify_Event(new EventPack(eventTag, cell));
		}

		// Token: 0x060130AB RID: 77995 RVA: 0x005A49C4 File Offset: 0x005A2BC4
		public static void Notify_Event(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return;
			}
			if (DebugViewSettings.logTutor)
			{
				string str = "Notify_Event: ";
				EventPack eventPack = ep;
				Log.Message(str + eventPack.ToString());
			}
			if (Current.Game == null)
			{
				return;
			}
			Lesson lesson = Find.ActiveLesson.Current;
			if (Find.ActiveLesson.Current != null)
			{
				Find.ActiveLesson.Current.Notify_Event(ep);
			}
			foreach (InstructionDef instructionDef in DefDatabase<InstructionDef>.AllDefs)
			{
				if (instructionDef.eventTagInitiate == ep.Tag && (instructionDef.eventTagInitiateSource == null || (lesson != null && instructionDef.eventTagInitiateSource == lesson.Instruction)) && (TutorSystem.TutorialMode || !instructionDef.tutorialModeOnly))
				{
					Find.ActiveLesson.Activate(instructionDef);
					break;
				}
			}
		}

		// Token: 0x060130AC RID: 77996 RVA: 0x005A4AB0 File Offset: 0x005A2CB0
		public static bool AllowAction(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return true;
			}
			if (DebugViewSettings.logTutor)
			{
				string str = "AllowAction: ";
				EventPack eventPack = ep;
				Log.Message(str + eventPack.ToString());
			}
			if (ep.Cells != null && ep.Cells.Count<IntVec3>() == 1)
			{
				return TutorSystem.AllowAction(new EventPack(ep.Tag, ep.Cells.First<IntVec3>()));
			}
			if (Find.ActiveLesson.Current != null)
			{
				AcceptanceReport acceptanceReport = Find.ActiveLesson.Current.AllowAction(ep);
				if (!acceptanceReport.Accepted)
				{
					Messages.Message((!acceptanceReport.Reason.NullOrEmpty()) ? acceptanceReport.Reason : Find.ActiveLesson.Current.DefaultRejectInputMessage, MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return true;
		}
	}
}
