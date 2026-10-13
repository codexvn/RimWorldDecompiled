using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020002B6 RID: 694
	public class TraitRequirement
	{
		// Token: 0x06001102 RID: 4354 RVA: 0x0004FCBE File Offset: 0x0004DEBE
		public bool Matches(Trait trait)
		{
			return trait.def == this.def && (this.degree == null || trait.Degree == this.degree.Value);
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x0004FCF4 File Offset: 0x0004DEF4
		public bool HasTrait(Pawn p)
		{
			if (p.story == null)
			{
				return false;
			}
			if (this.degree == null)
			{
				return p.story.traits.HasTrait(this.def);
			}
			return p.story.traits.HasTrait(this.def, this.degree.Value);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x0004FD50 File Offset: 0x0004DF50
		public Trait GetTrait(Pawn p)
		{
			if (p.story == null)
			{
				return null;
			}
			if (this.degree == null)
			{
				return p.story.traits.GetTrait(this.def);
			}
			return p.story.traits.GetTrait(this.def, this.degree.Value);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x0004FDAC File Offset: 0x0004DFAC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "degree");
		}

		// Token: 0x04000E7C RID: 3708
		public TraitDef def;

		// Token: 0x04000E7D RID: 3709
		public int? degree;
	}
}
