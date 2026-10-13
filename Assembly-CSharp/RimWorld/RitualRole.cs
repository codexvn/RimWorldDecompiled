using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027AD RID: 10157
	public abstract class RitualRole : ILordJobRole, IExposable, ILoadReferenceable
	{
		// Token: 0x1700220B RID: 8715
		// (get) Token: 0x0600DE99 RID: 56985 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool Animal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700220C RID: 8716
		// (get) Token: 0x0600DE9A RID: 56986 RVA: 0x00417787 File Offset: 0x00415987
		public int MaxCount
		{
			get
			{
				return this.maxCount;
			}
		}

		// Token: 0x1700220D RID: 8717
		// (get) Token: 0x0600DE9B RID: 56987 RVA: 0x0041778F File Offset: 0x0041598F
		public int MinCount
		{
			get
			{
				if (!this.required)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x1700220E RID: 8718
		// (get) Token: 0x0600DE9C RID: 56988 RVA: 0x0041779C File Offset: 0x0041599C
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x1700220F RID: 8719
		// (get) Token: 0x0600DE9D RID: 56989 RVA: 0x004177A9 File Offset: 0x004159A9
		public TaggedString LabelCap
		{
			get
			{
				return this.label.CapitalizeFirst();
			}
		}

		// Token: 0x17002210 RID: 8720
		// (get) Token: 0x0600DE9E RID: 56990 RVA: 0x004177BB File Offset: 0x004159BB
		public TaggedString CategoryLabel
		{
			get
			{
				return this.categoryLabel;
			}
		}

		// Token: 0x17002211 RID: 8721
		// (get) Token: 0x0600DE9F RID: 56991 RVA: 0x004177C8 File Offset: 0x004159C8
		public TaggedString CategoryLabelCap
		{
			get
			{
				return (this.categoryLabel ?? this.label).CapitalizeFirst();
			}
		}

		// Token: 0x0600DEA0 RID: 56992 RVA: 0x004177E4 File Offset: 0x004159E4
		public RitualRole()
		{
			if (Find.UniqueIDsManager != null && this.loadID == -1)
			{
				this.loadID = Find.UniqueIDsManager.GetNextRitualRoleID();
			}
		}

		// Token: 0x0600DEA1 RID: 56993 RVA: 0x00417844 File Offset: 0x00415A44
		public virtual bool AppliesToPawn(Pawn p, out string reason, TargetInfo selectedTarget, LordJob_Ritual ritual = null, RitualRoleAssignments assignments = null, Precept_Ritual precept = null, bool skipReason = false)
		{
			if (!this.AppliesIfChild(p, out reason, skipReason))
			{
				return false;
			}
			Ideo ideo = p.Ideo;
			Precept_Role role = (ideo != null) ? ideo.GetRole(p) : null;
			Precept_Ritual ritual2;
			if ((ritual2 = ((ritual != null) ? ritual.Ritual : null)) == null)
			{
				ritual2 = (((assignments != null) ? assignments.Ritual : null) ?? precept);
			}
			return this.AppliesToRole(role, out reason, ritual2, p, skipReason);
		}

		// Token: 0x0600DEA2 RID: 56994 RVA: 0x004178A4 File Offset: 0x00415AA4
		public Precept_Role FindInstance(Ideo ideo)
		{
			foreach (Precept_Role precept_Role in ideo.RolesListForReading)
			{
				if (precept_Role.def == this.precept)
				{
					return precept_Role;
				}
			}
			return null;
		}

		// Token: 0x0600DEA3 RID: 56995 RVA: 0x00417908 File Offset: 0x00415B08
		protected bool AppliesIfChild(Pawn p, out string reason, bool skipReason = false)
		{
			reason = null;
			if (ModsConfig.BiotechActive && !this.allowChild && p.DevelopmentalStage.Juvenile())
			{
				if (!skipReason)
				{
					reason = (this.customChildDisallowMessage ?? "MessageRitualRoleCannotBeChild".Translate(this.Label).CapitalizeFirst());
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600DEA4 RID: 56996
		public abstract bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn pawn = null, bool skipReason = false);

		// Token: 0x0600DEA5 RID: 56997 RVA: 0x00417968 File Offset: 0x00415B68
		protected virtual int PawnDesirability(Pawn pawn)
		{
			if (this.precept == null)
			{
				return 0;
			}
			Ideo ideo = pawn.Ideo;
			PreceptDef preceptDef;
			if (ideo == null)
			{
				preceptDef = null;
			}
			else
			{
				Precept_Role role = ideo.GetRole(pawn);
				preceptDef = ((role != null) ? role.def : null);
			}
			if (preceptDef == this.precept)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600DEA6 RID: 56998 RVA: 0x0041799E File Offset: 0x00415B9E
		public virtual void OrderByDesirability(List<Pawn> pawns)
		{
			pawns.Sort((Pawn lhs, Pawn rhs) => this.PawnDesirability(rhs).CompareTo(this.PawnDesirability(lhs)));
		}

		// Token: 0x0600DEA7 RID: 56999 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ExtraInfoForDialog(IEnumerable<Pawn> selected)
		{
			return null;
		}

		// Token: 0x0600DEA8 RID: 57000 RVA: 0x004179B4 File Offset: 0x00415BB4
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.missingDesc, "missingDesc", null, false);
			Scribe_Values.Look<string>(ref this.customChildDisallowMessage, "customChildDisallowMessage", null, false);
			Scribe_Values.Look<string>(ref this.id, "id", null, false);
			Scribe_Values.Look<int>(ref this.maxCount, "maxCount", 0, false);
			Scribe_Values.Look<bool>(ref this.required, "required", false, false);
			Scribe_Values.Look<bool>(ref this.substitutable, "substitutable", false, false);
			Scribe_Values.Look<bool>(ref this.countsAsParticipant, "countsAsParticipant", true, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", -1, false);
			Scribe_Values.Look<bool>(ref this.defaultForSelectedColonist, "defaultForSelectedColonist", false, false);
			Scribe_Defs.Look<PreceptDef>(ref this.precept, "precept");
			Scribe_Values.Look<string>(ref this.mergeId, "mergeId", null, false);
			Scribe_Values.Look<bool>(ref this.allowOtherIdeos, "allowOtherIdeos", false, false);
			Scribe_Values.Look<bool>(ref this.allowDowned, "allowDowned", false, false);
			Scribe_Values.Look<bool>(ref this.allowKeepLayingDown, "allowKeepLayingDown", false, false);
			Scribe_Values.Look<bool>(ref this.allowChild, "allowChild", false, false);
			Scribe_Values.Look<bool>(ref this.endJobOnRitualCleanup, "endJobOnRitualCleanup", false, false);
			Scribe_Values.Look<bool>(ref this.mustBeAbleToReachTarget, "mustBeAbleToReachTarget", false, false);
			Scribe_Values.Look<bool>(ref this.removeFromAssignmentsOnDeath, "removeFromAssignmentsOnDeath", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.loadID == -1)
			{
				this.loadID = Find.UniqueIDsManager.GetNextRitualRoleID();
			}
		}

		// Token: 0x0600DEA9 RID: 57001 RVA: 0x00417B36 File Offset: 0x00415D36
		public string GetUniqueLoadID()
		{
			return "RitualRole_" + this.loadID.ToString();
		}

		// Token: 0x040095F6 RID: 38390
		[MustTranslate]
		protected string label;

		// Token: 0x040095F7 RID: 38391
		[MustTranslate]
		protected string categoryLabel;

		// Token: 0x040095F8 RID: 38392
		[MustTranslate]
		public string missingDesc;

		// Token: 0x040095F9 RID: 38393
		[MustTranslate]
		public string noCandidatesGizmoDesc;

		// Token: 0x040095FA RID: 38394
		[MustTranslate]
		public string customChildDisallowMessage;

		// Token: 0x040095FB RID: 38395
		[NoTranslate]
		public string id;

		// Token: 0x040095FC RID: 38396
		public PreceptDef precept;

		// Token: 0x040095FD RID: 38397
		public int maxCount;

		// Token: 0x040095FE RID: 38398
		[NoTranslate]
		public string mergeId;

		// Token: 0x040095FF RID: 38399
		public bool required;

		// Token: 0x04009600 RID: 38400
		public bool substitutable;

		// Token: 0x04009601 RID: 38401
		public bool ignoreBleeding;

		// Token: 0x04009602 RID: 38402
		public bool countsAsParticipant = true;

		// Token: 0x04009603 RID: 38403
		public bool addToLord = true;

		// Token: 0x04009604 RID: 38404
		public bool allowNonAggroMentalState;

		// Token: 0x04009605 RID: 38405
		public bool defaultForSelectedColonist;

		// Token: 0x04009606 RID: 38406
		public bool allowOtherIdeos;

		// Token: 0x04009607 RID: 38407
		public bool allowDowned;

		// Token: 0x04009608 RID: 38408
		public bool allowKeepLayingDown;

		// Token: 0x04009609 RID: 38409
		public bool allowChild = true;

		// Token: 0x0400960A RID: 38410
		public bool allowBaby;

		// Token: 0x0400960B RID: 38411
		public bool removeFromAssignmentsOnDeath = true;

		// Token: 0x0400960C RID: 38412
		public bool endJobOnRitualCleanup = true;

		// Token: 0x0400960D RID: 38413
		public bool mustBeAbleToReachTarget;

		// Token: 0x0400960E RID: 38414
		public bool blocksSocialInteraction;

		// Token: 0x0400960F RID: 38415
		private int loadID = -1;
	}
}
