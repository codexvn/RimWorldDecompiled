using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003622 RID: 13858
	[DefOf]
	public static class OrbitalDebrisDefOf
	{
		// Token: 0x0601340D RID: 78861 RVA: 0x005B6368 File Offset: 0x005B4568
		static OrbitalDebrisDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(OrbitalDebrisDefOf));
		}

		// Token: 0x0400D592 RID: 54674
		[MayRequireOdyssey]
		public static OrbitalDebrisDef Asteroid;

		// Token: 0x0400D593 RID: 54675
		[MayRequireOdyssey]
		public static OrbitalDebrisDef Manmade;

		// Token: 0x0400D594 RID: 54676
		[MayRequireOdyssey]
		public static OrbitalDebrisDef Mechanoid;
	}
}
