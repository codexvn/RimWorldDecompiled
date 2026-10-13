using System;

namespace RimWorld
{
	// Token: 0x020035F3 RID: 13811
	[DefOf]
	public static class ShipJobDefOf
	{
		// Token: 0x060133E0 RID: 78816 RVA: 0x005B606B File Offset: 0x005B426B
		static ShipJobDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ShipJobDefOf));
		}

		// Token: 0x0400D4AB RID: 54443
		[MayRequireAnyOf("Ludeon.RimWorld.Royalty,Ludeon.RimWorld.Biotech")]
		public static ShipJobDef Arrive;

		// Token: 0x0400D4AC RID: 54444
		[MayRequireRoyalty]
		public static ShipJobDef FlyAway;

		// Token: 0x0400D4AD RID: 54445
		[MayRequireAnyOf("Ludeon.RimWorld.Royalty,Ludeon.RimWorld.Biotech")]
		public static ShipJobDef WaitTime;

		// Token: 0x0400D4AE RID: 54446
		[MayRequireRoyalty]
		public static ShipJobDef WaitForever;

		// Token: 0x0400D4AF RID: 54447
		[MayRequireAnyOf("Ludeon.RimWorld.Royalty,Ludeon.RimWorld.Biotech")]
		public static ShipJobDef Unload;

		// Token: 0x0400D4B0 RID: 54448
		[MayRequireRoyalty]
		public static ShipJobDef Unload_Destination;

		// Token: 0x0400D4B1 RID: 54449
		[MayRequireRoyalty]
		public static ShipJobDef WaitSendable;
	}
}
