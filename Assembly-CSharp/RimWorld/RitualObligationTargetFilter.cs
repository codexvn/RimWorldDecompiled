using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020026E1 RID: 9953
	public abstract class RitualObligationTargetFilter : IExposable
	{
		// Token: 0x0600DA25 RID: 55845 RVA: 0x000024DF File Offset: 0x000006DF
		public RitualObligationTargetFilter()
		{
		}

		// Token: 0x0600DA26 RID: 55846 RVA: 0x0040888F File Offset: 0x00406A8F
		public RitualObligationTargetFilter(RitualObligationTargetFilterDef def)
		{
			this.def = def;
		}

		// Token: 0x0600DA27 RID: 55847 RVA: 0x0040889E File Offset: 0x00406A9E
		public RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
		{
			if ((target.Thing != null && !target.Thing.Spawned) || Find.IdeoManager.GetActiveRitualOn(target.Thing) != null)
			{
				return false;
			}
			return this.CanUseTargetInternal(target, obligation);
		}

		// Token: 0x0600DA28 RID: 55848
		public abstract IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map);

		// Token: 0x0600DA29 RID: 55849
		protected abstract RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation);

		// Token: 0x0600DA2A RID: 55850 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual List<string> MissingTargetBuilding(Ideo ideo)
		{
			return null;
		}

		// Token: 0x0600DA2B RID: 55851 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ObligationTargetsValid(RitualObligation obligation)
		{
			return true;
		}

		// Token: 0x0600DA2C RID: 55852
		public abstract IEnumerable<string> GetTargetInfos(RitualObligation obligation);

		// Token: 0x0600DA2D RID: 55853 RVA: 0x004088DE File Offset: 0x00406ADE
		public virtual IEnumerable<string> GetBlockingIssues(TargetInfo target, RitualRoleAssignments assignments)
		{
			foreach (RitualRole ritualRole in assignments.AllRolesForReading)
			{
				if (ritualRole.mustBeAbleToReachTarget)
				{
					Pawn pawn = assignments.FirstAssignedPawn(ritualRole);
					if (pawn != null && !pawn.CanReach((LocalTargetInfo)target, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
					{
						yield return "RitualTargetUnreachable".Translate(ritualRole.LabelCap);
					}
				}
			}
			List<RitualRole>.Enumerator enumerator = default(List<RitualRole>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600DA2E RID: 55854 RVA: 0x00035ADE File Offset: 0x00033CDE
		public virtual string LabelExtraPart(RitualObligation obligation)
		{
			return "";
		}

		// Token: 0x0600DA2F RID: 55855 RVA: 0x004088F5 File Offset: 0x00406AF5
		public virtual bool ShouldGrayOut(Pawn pawn, ILordJobAssignmentsManager<RitualRole> assignments, out TaggedString reason)
		{
			reason = TaggedString.Empty;
			return false;
		}

		// Token: 0x0600DA30 RID: 55856 RVA: 0x00408903 File Offset: 0x00406B03
		public virtual void CopyTo(RitualObligationTargetFilter other)
		{
			other.parent = this.parent;
			other.def = this.def;
		}

		// Token: 0x0600DA31 RID: 55857 RVA: 0x0040891D File Offset: 0x00406B1D
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualObligationTargetFilterDef>(ref this.def, "def");
			Scribe_References.Look<Precept_Ritual>(ref this.parent, "parent", false);
		}

		// Token: 0x040093B2 RID: 37810
		public Precept_Ritual parent;

		// Token: 0x040093B3 RID: 37811
		public RitualObligationTargetFilterDef def;
	}
}
