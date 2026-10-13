using System;
using System.Runtime.CompilerServices;

namespace Verse
{
	// Token: 0x02000906 RID: 2310
	public static class ThingCompUtility
	{
		// Token: 0x06003BE7 RID: 15335 RVA: 0x0013A7EC File Offset: 0x001389EC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T TryGetComp<T>(this Thing thing) where T : ThingComp
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return default(T);
			}
			return thingWithComps.GetComp<T>();
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0013A814 File Offset: 0x00138A14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetComp<T>(this Thing thing, out T comp) where T : ThingComp
		{
			comp = default(T);
			ThingWithComps thingWithComps = thing as ThingWithComps;
			return thingWithComps != null && thingWithComps.TryGetComp(out comp);
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0013A83B File Offset: 0x00138A3B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetComp<T>(this ThingWithComps thing, out T comp) where T : ThingComp
		{
			comp = thing.GetComp<T>();
			return comp != null;
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x0013A858 File Offset: 0x00138A58
		public static ThingComp TryGetComp(this Thing thing, CompProperties properties)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return null;
			}
			return thingWithComps.GetCompByDefType(properties);
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x0013A878 File Offset: 0x00138A78
		public static bool HasComp<T>(this Thing thing) where T : ThingComp
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			return ((thingWithComps != null) ? thingWithComps.GetComp<T>() : default(T)) != null;
		}
	}
}
