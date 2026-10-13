using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200358D RID: 13709
	[DefOf]
	public static class WorkTypeDefOf
	{
		// Token: 0x0601337B RID: 78715 RVA: 0x005B59B6 File Offset: 0x005B3BB6
		static WorkTypeDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkTypeDefOf));
		}

		// Token: 0x0400CBD3 RID: 52179
		public static WorkTypeDef Mining;

		// Token: 0x0400CBD4 RID: 52180
		public static WorkTypeDef Growing;

		// Token: 0x0400CBD5 RID: 52181
		public static WorkTypeDef Construction;

		// Token: 0x0400CBD6 RID: 52182
		public static WorkTypeDef Warden;

		// Token: 0x0400CBD7 RID: 52183
		public static WorkTypeDef Doctor;

		// Token: 0x0400CBD8 RID: 52184
		public static WorkTypeDef Firefighter;

		// Token: 0x0400CBD9 RID: 52185
		public static WorkTypeDef Hunting;

		// Token: 0x0400CBDA RID: 52186
		public static WorkTypeDef Handling;

		// Token: 0x0400CBDB RID: 52187
		public static WorkTypeDef Crafting;

		// Token: 0x0400CBDC RID: 52188
		public static WorkTypeDef Hauling;

		// Token: 0x0400CBDD RID: 52189
		public static WorkTypeDef Cleaning;

		// Token: 0x0400CBDE RID: 52190
		public static WorkTypeDef Research;

		// Token: 0x0400CBDF RID: 52191
		public static WorkTypeDef PlantCutting;

		// Token: 0x0400CBE0 RID: 52192
		public static WorkTypeDef Smithing;

		// Token: 0x0400CBE1 RID: 52193
		[MayRequireBiotech]
		public static WorkTypeDef Childcare;

		// Token: 0x0400CBE2 RID: 52194
		[MayRequireAnomaly]
		public static WorkTypeDef DarkStudy;
	}
}
