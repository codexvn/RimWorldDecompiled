using System;

namespace RimWorld
{
	// Token: 0x020035B8 RID: 13752
	[DefOf]
	public static class PawnRelationDefOf
	{
		// Token: 0x060133A6 RID: 78758 RVA: 0x005B5C91 File Offset: 0x005B3E91
		static PawnRelationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnRelationDefOf));
		}

		// Token: 0x0400D165 RID: 53605
		public static PawnRelationDef Kin;

		// Token: 0x0400D166 RID: 53606
		public static PawnRelationDef UncleOrAunt;

		// Token: 0x0400D167 RID: 53607
		public static PawnRelationDef Child;

		// Token: 0x0400D168 RID: 53608
		public static PawnRelationDef Grandchild;

		// Token: 0x0400D169 RID: 53609
		public static PawnRelationDef Grandparent;

		// Token: 0x0400D16A RID: 53610
		public static PawnRelationDef Sibling;

		// Token: 0x0400D16B RID: 53611
		public static PawnRelationDef HalfSibling;

		// Token: 0x0400D16C RID: 53612
		public static PawnRelationDef Cousin;

		// Token: 0x0400D16D RID: 53613
		public static PawnRelationDef NephewOrNiece;

		// Token: 0x0400D16E RID: 53614
		public static PawnRelationDef Spouse;

		// Token: 0x0400D16F RID: 53615
		public static PawnRelationDef Parent;

		// Token: 0x0400D170 RID: 53616
		public static PawnRelationDef GreatGrandchild;

		// Token: 0x0400D171 RID: 53617
		public static PawnRelationDef GreatGrandparent;

		// Token: 0x0400D172 RID: 53618
		public static PawnRelationDef GranduncleOrGrandaunt;

		// Token: 0x0400D173 RID: 53619
		public static PawnRelationDef Lover;

		// Token: 0x0400D174 RID: 53620
		public static PawnRelationDef ExLover;

		// Token: 0x0400D175 RID: 53621
		public static PawnRelationDef ExSpouse;

		// Token: 0x0400D176 RID: 53622
		public static PawnRelationDef Fiance;

		// Token: 0x0400D177 RID: 53623
		public static PawnRelationDef Bond;

		// Token: 0x0400D178 RID: 53624
		[MayRequireBiotech]
		public static PawnRelationDef Overseer;

		// Token: 0x0400D179 RID: 53625
		[MayRequireBiotech]
		public static PawnRelationDef ParentBirth;
	}
}
