using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026BC RID: 9916
	public class RitualStage_InteractWithRole : RitualStage
	{
		// Token: 0x0600D963 RID: 55651 RVA: 0x00405B90 File Offset: 0x00403D90
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			if (this.targetId != null)
			{
				return ritual.PawnWithRole(this.targetId);
			}
			return base.GetSecondFocus(ritual);
		}

		// Token: 0x0600D964 RID: 55652 RVA: 0x00405BB3 File Offset: 0x00403DB3
		public override IEnumerable<RitualStagePawnSecondFocus> GetPawnSecondFoci(LordJob_Ritual ritual)
		{
			if (this.targets.NullOrEmpty<RitualStage_InteractWithRole.PawnTarget>())
			{
				yield break;
			}
			foreach (RitualStage_InteractWithRole.PawnTarget pawnTarget in this.targets)
			{
				Pawn pawn = ritual.assignments.FirstAssignedPawn(pawnTarget.pawnId);
				Pawn t = ritual.assignments.FirstAssignedPawn(pawnTarget.targetId);
				yield return new RitualStagePawnSecondFocus
				{
					pawn = pawn,
					target = t
				};
			}
			List<RitualStage_InteractWithRole.PawnTarget>.Enumerator enumerator = default(List<RitualStage_InteractWithRole.PawnTarget>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600D965 RID: 55653 RVA: 0x00405BCA File Offset: 0x00403DCA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.targetId, "targetId", null, false);
			Scribe_Collections.Look<RitualStage_InteractWithRole.PawnTarget>(ref this.targets, "targets", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04009352 RID: 37714
		[NoTranslate]
		public string targetId;

		// Token: 0x04009353 RID: 37715
		public List<RitualStage_InteractWithRole.PawnTarget> targets;

		// Token: 0x020026BD RID: 9917
		public class PawnTarget : IExposable
		{
			// Token: 0x0600D967 RID: 55655 RVA: 0x00405BFA File Offset: 0x00403DFA
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.pawnId, "pawnId", null, false);
				Scribe_Values.Look<string>(ref this.targetId, "targetId", null, false);
			}

			// Token: 0x04009354 RID: 37716
			[NoTranslate]
			public string pawnId;

			// Token: 0x04009355 RID: 37717
			[NoTranslate]
			public string targetId;
		}
	}
}
