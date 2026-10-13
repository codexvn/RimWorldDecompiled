using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002823 RID: 10275
	public class RoleRequirement_NotChild : RoleRequirement
	{
		// Token: 0x0600E0B4 RID: 57524 RVA: 0x0041F552 File Offset: 0x0041D752
		public override string GetLabel(Precept_Role role)
		{
			return "RoleRequirementCannotBe".Translate();
		}

		// Token: 0x0600E0B5 RID: 57525 RVA: 0x0041F563 File Offset: 0x0041D763
		public override bool Met(Pawn p, Precept_Role role)
		{
			return !p.DevelopmentalStage.Juvenile();
		}

		// Token: 0x04009703 RID: 38659
		public static readonly RoleRequirement_NotChild Requirement = new RoleRequirement_NotChild();
	}
}
