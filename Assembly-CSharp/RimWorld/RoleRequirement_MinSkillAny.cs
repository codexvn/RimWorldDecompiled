using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02002826 RID: 10278
	public class RoleRequirement_MinSkillAny : RoleRequirement
	{
		// Token: 0x0600E0BF RID: 57535 RVA: 0x0041F638 File Offset: 0x0041D838
		public override string GetLabel(Precept_Role role)
		{
			if (this.labelCached == null)
			{
				if (this.skills.Count == 1)
				{
					this.labelCached = "RoleRequirementSkill".Translate() + ": " + RoleRequirement_MinSkillAny.<GetLabel>g__GetSkillStr|2_0(this.skills[0]);
				}
				else
				{
					this.labelCached = "RoleRequirementSkillAny".Translate() + ": " + (from s in this.skills
					select RoleRequirement_MinSkillAny.<GetLabel>g__GetSkillStr|2_0(s)).ToCommaList(false, false);
				}
			}
			return this.labelCached;
		}

		// Token: 0x0600E0C0 RID: 57536 RVA: 0x0041F6F0 File Offset: 0x0041D8F0
		public override bool Met(Pawn p, Precept_Role role)
		{
			foreach (SkillRequirement skillRequirement in this.skills)
			{
				if (p.skills.GetSkill(skillRequirement.skill).Level >= skillRequirement.minLevel)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E0C2 RID: 57538 RVA: 0x0041F764 File Offset: 0x0041D964
		[CompilerGenerated]
		internal static string <GetLabel>g__GetSkillStr|2_0(SkillRequirement requirement)
		{
			return requirement.skill.LabelCap + " " + requirement.minLevel.ToString();
		}

		// Token: 0x04009704 RID: 38660
		public List<SkillRequirement> skills;

		// Token: 0x04009705 RID: 38661
		[NoTranslate]
		private string labelCached;
	}
}
