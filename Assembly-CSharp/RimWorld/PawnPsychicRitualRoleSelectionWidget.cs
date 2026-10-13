using System;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02003497 RID: 13463
	[StaticConstructorOnStartup]
	public class PawnPsychicRitualRoleSelectionWidget : PawnRoleSelectionWidgetBase<PsychicRitualRoleDef>
	{
		// Token: 0x06012EEC RID: 77548 RVA: 0x0059B430 File Offset: 0x00599630
		public PawnPsychicRitualRoleSelectionWidget(PsychicRitualDef ritualDef, PsychicRitualCandidatePool candidatePool, PsychicRitualRoleAssignments assignments) : base(candidatePool, assignments)
		{
			this.ritualDef = ritualDef;
			this.psychicRitualAssignments = assignments;
		}

		// Token: 0x06012EED RID: 77549 RVA: 0x0059B454 File Offset: 0x00599654
		public override bool ShouldDrawHighlight(PsychicRitualRoleDef highlightedRole, Pawn pawn)
		{
			PsychicRitualRoleDef.Reason reason;
			return highlightedRole != null && !DragAndDropWidget.Dragging && !this.assignments.AssignedPawns(highlightedRole).Contains(pawn) && highlightedRole.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, this.psychicRitualAssignments.Target, out reason);
		}

		// Token: 0x06012EEE RID: 77550 RVA: 0x0059B498 File Offset: 0x00599698
		protected override string ExtraTipContents(Pawn pawn)
		{
			this.sb.Clear();
			this.sb.AppendLine("  - " + StatDefOf.PsychicSensitivity.LabelCap + ": " + StatDefOf.PsychicSensitivity.Worker.ValueToStringFor(pawn));
			if (ModsConfig.IdeologyActive && !Find.IdeoManager.classicMode && pawn.Ideo != null)
			{
				this.sb.AppendLineTagged("  - " + "Ideo".Translate().CapitalizeFirst() + ": " + pawn.Ideo.name.Colorize(pawn.Ideo.Color));
			}
			foreach (string str in this.ritualDef.GetPawnTooltipExtras(pawn))
			{
				this.sb.AppendLine("  - " + str);
			}
			return this.sb.ToString().TrimEndNewlines();
		}

		// Token: 0x0400C41D RID: 50205
		protected PsychicRitualRoleAssignments psychicRitualAssignments;

		// Token: 0x0400C41E RID: 50206
		protected PsychicRitualDef ritualDef;

		// Token: 0x0400C41F RID: 50207
		private StringBuilder sb = new StringBuilder();
	}
}
