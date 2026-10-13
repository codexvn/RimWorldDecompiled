using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C8 RID: 712
	public class SkillRequirement
	{
		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06001156 RID: 4438 RVA: 0x000512EC File Offset: 0x0004F4EC
		public string Summary
		{
			get
			{
				if (this.skill == null)
				{
					return "";
				}
				return string.Format("{0} ({1})", this.skill.LabelCap, this.minLevel);
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00051324 File Offset: 0x0004F524
		public bool PawnSatisfies(Pawn pawn)
		{
			return (pawn.IsColonyMechPlayerControlled && pawn.RaceProps.mechFixedSkillLevel >= this.minLevel && pawn.RaceProps.mechEnabledWorkTypes.Any((WorkTypeDef w) => w.relevantSkills.NotNullAndContains(this.skill))) || (pawn.skills != null && pawn.skills.GetSkill(this.skill).Level >= this.minLevel);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00051397 File Offset: 0x0004F597
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
			this.minLevel = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x000513C3 File Offset: 0x0004F5C3
		public override string ToString()
		{
			if (this.skill == null)
			{
				return "null-skill-requirement";
			}
			return this.skill.defName + "-" + this.minLevel.ToString();
		}

		// Token: 0x04000F7D RID: 3965
		public SkillDef skill;

		// Token: 0x04000F7E RID: 3966
		public int minLevel;
	}
}
