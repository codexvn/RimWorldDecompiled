using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026CC RID: 9932
	public class RitualBehaviorWorker_Speech : RitualBehaviorWorker
	{
		// Token: 0x0600D9BF RID: 55743 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_Speech()
		{
		}

		// Token: 0x0600D9C0 RID: 55744 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_Speech(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9C1 RID: 55745 RVA: 0x004071F8 File Offset: 0x004053F8
		protected override LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Pawn organizer2 = assignments.AssignedPawns("speaker").First<Pawn>();
			return new LordJob_Joinable_Speech(target, organizer2, ritual, this.def.stages, assignments, false);
		}

		// Token: 0x0600D9C2 RID: 55746 RVA: 0x00407230 File Offset: 0x00405430
		protected override void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Pawn arg = assignments.AssignedPawns("speaker").First<Pawn>();
			Find.LetterStack.ReceiveLetter(this.def.letterTitle.Formatted(ritual.Named("RITUAL")), this.def.letterText.Formatted(arg.Named("SPEAKER"), ritual.Named("RITUAL"), ritual.ideo.MemberNamePlural.Named("IDEOMEMBERS")) + "\n\n" + ritual.outcomeEffect.ExtraAlertParagraph(ritual), LetterDefOf.PositiveEvent, target, null, null, null, null, 0, true);
		}

		// Token: 0x0600D9C3 RID: 55747 RVA: 0x004072DC File Offset: 0x004054DC
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			Precept_Role precept_Role = ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == PreceptDefOf.IdeoRole_Leader);
			if (precept_Role == null)
			{
				return null;
			}
			if (precept_Role.ChosenPawnSingle() == null)
			{
				return "CantStartRitualRoleNotAssigned".Translate(precept_Role.LabelCap);
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}
	}
}
