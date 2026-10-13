using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003498 RID: 13464
	[StaticConstructorOnStartup]
	public class PawnRitualRoleSelectionWidget : PawnRoleSelectionWidgetBase<RitualRole>
	{
		// Token: 0x06012EEF RID: 77551 RVA: 0x0059B5C8 File Offset: 0x005997C8
		public override string SpectatorsLabel()
		{
			Precept_Ritual precept_Ritual = this.ritual;
			string text;
			if (precept_Ritual == null)
			{
				text = null;
			}
			else
			{
				RitualBehaviorWorker behavior = precept_Ritual.behavior;
				text = ((behavior != null) ? behavior.def.spectatorsLabel : null);
			}
			return text ?? base.SpectatorsLabel();
		}

		// Token: 0x06012EF0 RID: 77552 RVA: 0x0059B5F8 File Offset: 0x005997F8
		public PawnRitualRoleSelectionWidget(RitualRoleAssignments assignments, Precept_Ritual ritual, TargetInfo target, RitualOutcomeEffectDef outcome) : base(assignments, assignments)
		{
			this.ritualAssignments = assignments;
			this.ritual = ritual;
			this.target = target;
			this.outcome = outcome;
			this.showIdeoIcon = (ritual != null && ritual.showIdeoIconsInDialog);
		}

		// Token: 0x06012EF1 RID: 77553 RVA: 0x0059B654 File Offset: 0x00599854
		public override bool ShouldDrawHighlight(RitualRole highlightedRole, Pawn pawn)
		{
			string text;
			return highlightedRole != null && !DragAndDropWidget.Dragging && !this.assignments.AssignedPawns(highlightedRole).Contains(pawn) && highlightedRole.AppliesToPawn(pawn, out text, this.target, null, this.ritualAssignments, this.ritual, true);
		}

		// Token: 0x06012EF2 RID: 77554 RVA: 0x0059B6A0 File Offset: 0x005998A0
		public override void Notify_AssignmentsChanged()
		{
			if (this.outcome != null)
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.outcome.comps)
				{
					RitualOutcomeComp ritualOutcomeComp2 = ritualOutcomeComp;
					RitualRoleAssignments assignments = this.ritualAssignments;
					Precept_Ritual precept_Ritual = this.ritual;
					RitualOutcomeComp_Data data;
					if (precept_Ritual == null)
					{
						data = null;
					}
					else
					{
						RitualOutcomeEffectWorker outcomeEffect = precept_Ritual.outcomeEffect;
						data = ((outcomeEffect != null) ? outcomeEffect.DataForComp(ritualOutcomeComp) : null);
					}
					ritualOutcomeComp2.Notify_AssignmentsChanged(assignments, data);
				}
			}
		}

		// Token: 0x06012EF3 RID: 77555 RVA: 0x0059B724 File Offset: 0x00599924
		public override string ExtraInfoForRole(RitualRole role, Pawn pawnToBeAssigned, IEnumerable<Pawn> currentlyAssigned)
		{
			string text = (pawnToBeAssigned == null) ? role.ExtraInfoForDialog(currentlyAssigned) : null;
			Pawn pawn = pawnToBeAssigned ?? currentlyAssigned.FirstOrDefault<Pawn>();
			PreceptDef preceptDef;
			if (pawn == null)
			{
				preceptDef = null;
			}
			else
			{
				Ideo ideo = pawn.Ideo;
				if (ideo == null)
				{
					preceptDef = null;
				}
				else
				{
					Precept_Role role2 = ideo.GetRole(pawn);
					preceptDef = ((role2 != null) ? role2.def : null);
				}
			}
			PreceptDef preceptDef2 = preceptDef;
			if (pawn != null && preceptDef2 != role.precept && role.substitutable && role.precept != null)
			{
				if (text != null)
				{
					text += "\n\n";
				}
				text += this.ExtraRoleInfo(role);
				if (!Find.IdeoManager.classicMode)
				{
					text = text + ": " + this.BonusExplanationInfo(role).CapitalizeFirst().Resolve();
				}
			}
			return text;
		}

		// Token: 0x06012EF4 RID: 77556 RVA: 0x0059B7DA File Offset: 0x005999DA
		public override bool ShouldGrayOut(Pawn pawn, out TaggedString reason)
		{
			if (base.ShouldGrayOut(pawn, out reason))
			{
				return true;
			}
			Precept_Ritual precept_Ritual = this.ritual;
			return ((precept_Ritual != null) ? precept_Ritual.obligationTargetFilter : null) != null && this.ritual.obligationTargetFilter.ShouldGrayOut(pawn, this.assignments, out reason);
		}

		// Token: 0x06012EF5 RID: 77557 RVA: 0x0059B818 File Offset: 0x00599A18
		protected override string ExtraTipContents(Pawn pawn)
		{
			this.sb.Clear();
			this.tmpRelevantStats.Clear();
			foreach (RitualRole ritualRole in this.ritualAssignments.AllRolesForReading)
			{
				RitualRoleColonist ritualRoleColonist = ritualRole as RitualRoleColonist;
				if (ritualRoleColonist != null && ritualRoleColonist.usedStat != null)
				{
					this.tmpRelevantStats.Add(ritualRoleColonist.usedStat);
				}
			}
			if (this.tmpRelevantStats.Count == 0)
			{
				return null;
			}
			foreach (StatDef statDef in this.tmpRelevantStats)
			{
				if (statDef.Worker.IsDisabledFor(pawn))
				{
					this.sb.AppendLine("  - " + (statDef.LabelCap + ": " + "Disabled".Translate()).Colorize(ColorLibrary.RedReadable));
				}
				else
				{
					this.sb.AppendLine("  - " + statDef.LabelCap + ": " + statDef.Worker.ValueToStringFor(pawn));
				}
			}
			return this.sb.ToString();
		}

		// Token: 0x06012EF6 RID: 77558 RVA: 0x0059B988 File Offset: 0x00599B88
		public string ExtraRoleInfo(RitualRole role)
		{
			Precept precept = this.ritual.ideo.PreceptsListForReading.FirstOrDefault((Precept p) => p.def == role.precept);
			if (precept == null)
			{
				return null;
			}
			return "RitualRoleRequiresSocialRole".Translate(precept.Label);
		}

		// Token: 0x06012EF7 RID: 77559 RVA: 0x0059B9E4 File Offset: 0x00599BE4
		public TaggedString BonusExplanationInfo(RitualRole role)
		{
			TaggedString taggedString = "";
			bool flag = false;
			if (this.ritual.outcomeEffect != null && !this.ritual.outcomeEffect.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.ritual.outcomeEffect.def.comps)
				{
					if (ritualOutcomeComp is RitualOutcomeComp_RolePresentNotSubstituted)
					{
						if (flag)
						{
							taggedString += ", ";
						}
						taggedString += ritualOutcomeComp.GetBonusDescShort();
						flag = true;
					}
				}
			}
			if (flag)
			{
				return taggedString;
			}
			return "None".Translate();
		}

		// Token: 0x06012EF8 RID: 77560 RVA: 0x0059BAB0 File Offset: 0x00599CB0
		public override string SpectatorFilterReason(Pawn pawn)
		{
			if (this.ritual != null && this.ritual.behavior.def.spectatorFilter != null && !this.ritual.behavior.def.spectatorFilter.Allowed(pawn))
			{
				return this.ritual.behavior.def.spectatorFilter.description;
			}
			return null;
		}

		// Token: 0x0400C420 RID: 50208
		protected RitualRoleAssignments ritualAssignments;

		// Token: 0x0400C421 RID: 50209
		protected Precept_Ritual ritual;

		// Token: 0x0400C422 RID: 50210
		protected TargetInfo target;

		// Token: 0x0400C423 RID: 50211
		protected RitualOutcomeEffectDef outcome;

		// Token: 0x0400C424 RID: 50212
		private StringBuilder sb = new StringBuilder();

		// Token: 0x0400C425 RID: 50213
		private HashSet<StatDef> tmpRelevantStats = new HashSet<StatDef>();
	}
}
