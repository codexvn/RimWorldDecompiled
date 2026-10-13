using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001877 RID: 6263
	public class ThoughtWorker_Expectations : ThoughtWorker
	{
		// Token: 0x06008FD5 RID: 36821 RVA: 0x002A74D0 File Offset: 0x002A56D0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p);
			if (expectationDef == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(expectationDef.thoughtStage);
		}

		// Token: 0x06008FD6 RID: 36822 RVA: 0x002A7508 File Offset: 0x002A5708
		public override string PostProcessDescription(Pawn p, string description)
		{
			string text = base.PostProcessDescription(p, description);
			if (ModsConfig.IdeologyActive)
			{
				Ideo ideo = p.Ideo;
				Precept_Role precept_Role = (ideo != null) ? ideo.GetRole(p) : null;
				if (precept_Role != null && ExpectationsUtility.OffsetByRole(p))
				{
					if (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive)
					{
						return text + "\n\n" + "RoleRaisedExpectationGracePeriod".Translate(precept_Role.LabelCap, MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
					}
					return text + "\n\n" + "RoleRaisedExpectation".Translate(precept_Role.LabelCap);
				}
			}
			return text;
		}
	}
}
