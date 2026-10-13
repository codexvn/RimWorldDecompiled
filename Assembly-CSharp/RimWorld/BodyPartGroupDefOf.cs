using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020035A1 RID: 13729
	[DefOf]
	public static class BodyPartGroupDefOf
	{
		// Token: 0x0601338F RID: 78735 RVA: 0x005B5B0A File Offset: 0x005B3D0A
		static BodyPartGroupDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(BodyPartGroupDefOf));
		}

		// Token: 0x0400CED6 RID: 52950
		public static BodyPartGroupDef Torso;

		// Token: 0x0400CED7 RID: 52951
		public static BodyPartGroupDef Legs;

		// Token: 0x0400CED8 RID: 52952
		public static BodyPartGroupDef LeftHand;

		// Token: 0x0400CED9 RID: 52953
		public static BodyPartGroupDef RightHand;

		// Token: 0x0400CEDA RID: 52954
		public static BodyPartGroupDef FullHead;

		// Token: 0x0400CEDB RID: 52955
		public static BodyPartGroupDef UpperHead;

		// Token: 0x0400CEDC RID: 52956
		public static BodyPartGroupDef Eyes;

		// Token: 0x0400CEDD RID: 52957
		public static BodyPartGroupDef LeftBlade;

		// Token: 0x0400CEDE RID: 52958
		public static BodyPartGroupDef FrontLeftPaw;
	}
}
