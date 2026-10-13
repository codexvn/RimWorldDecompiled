using System;
using RimWorld;

namespace Verse.Grammar
{
	// Token: 0x02000DEF RID: 3567
	public class Rule_NamePerson : Rule
	{
		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06005AD0 RID: 23248 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public override float BaseSelectionWeight
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06005AD1 RID: 23249 RVA: 0x001D24A3 File Offset: 0x001D06A3
		public override Rule DeepCopy()
		{
			Rule_NamePerson rule_NamePerson = (Rule_NamePerson)base.DeepCopy();
			rule_NamePerson.gender = this.gender;
			return rule_NamePerson;
		}

		// Token: 0x06005AD2 RID: 23250 RVA: 0x001D24BC File Offset: 0x001D06BC
		public override string Generate()
		{
			NameBank nameBank = PawnNameDatabaseShuffled.BankOf(PawnNameCategory.HumanStandard);
			Gender gender = this.gender;
			if (gender == Gender.None)
			{
				gender = ((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			return nameBank.GetName(PawnNameSlot.First, gender, false);
		}

		// Token: 0x06005AD3 RID: 23251 RVA: 0x001D24F2 File Offset: 0x001D06F2
		public override string ToString()
		{
			return this.keyword + "->(personname)";
		}

		// Token: 0x040041BC RID: 16828
		public Gender gender;
	}
}
