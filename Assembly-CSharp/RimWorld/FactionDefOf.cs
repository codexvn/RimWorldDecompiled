using System;

namespace RimWorld
{
	// Token: 0x0200358C RID: 13708
	[DefOf]
	public static class FactionDefOf
	{
		// Token: 0x0601337A RID: 78714 RVA: 0x005B59A5 File Offset: 0x005B3BA5
		static FactionDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(FactionDefOf));
		}

		// Token: 0x0400CBBE RID: 52158
		public static FactionDef PlayerColony;

		// Token: 0x0400CBBF RID: 52159
		public static FactionDef PlayerTribe;

		// Token: 0x0400CBC0 RID: 52160
		public static FactionDef Ancients;

		// Token: 0x0400CBC1 RID: 52161
		public static FactionDef AncientsHostile;

		// Token: 0x0400CBC2 RID: 52162
		public static FactionDef Mechanoid;

		// Token: 0x0400CBC3 RID: 52163
		public static FactionDef Insect;

		// Token: 0x0400CBC4 RID: 52164
		public static FactionDef Pirate;

		// Token: 0x0400CBC5 RID: 52165
		public static FactionDef OutlanderCivil;

		// Token: 0x0400CBC6 RID: 52166
		public static FactionDef TribeCivil;

		// Token: 0x0400CBC7 RID: 52167
		public static FactionDef OutlanderRough;

		// Token: 0x0400CBC8 RID: 52168
		public static FactionDef TribeRough;

		// Token: 0x0400CBC9 RID: 52169
		[MayRequireRoyalty]
		public static FactionDef Empire;

		// Token: 0x0400CBCA RID: 52170
		[MayRequireRoyalty]
		public static FactionDef OutlanderRefugee;

		// Token: 0x0400CBCB RID: 52171
		[MayRequireIdeology]
		public static FactionDef Beggars;

		// Token: 0x0400CBCC RID: 52172
		[MayRequireIdeology]
		public static FactionDef Pilgrims;

		// Token: 0x0400CBCD RID: 52173
		[MayRequireBiotech]
		public static FactionDef PirateWaster;

		// Token: 0x0400CBCE RID: 52174
		[MayRequireBiotech]
		public static FactionDef Sanguophages;

		// Token: 0x0400CBCF RID: 52175
		[MayRequireAnomaly]
		public static FactionDef HoraxCult;

		// Token: 0x0400CBD0 RID: 52176
		[MayRequireAnomaly]
		public static FactionDef Entities;

		// Token: 0x0400CBD1 RID: 52177
		[MayRequireOdyssey]
		public static FactionDef TradersGuild;

		// Token: 0x0400CBD2 RID: 52178
		[MayRequireOdyssey]
		public static FactionDef Salvagers;
	}
}
