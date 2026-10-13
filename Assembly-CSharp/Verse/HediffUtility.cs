using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200072C RID: 1836
	public static class HediffUtility
	{
		// Token: 0x06002CA1 RID: 11425 RVA: 0x000E6FBC File Offset: 0x000E51BC
		public static T TryGetComp<T>(this Hediff hd) where T : HediffComp
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return default(T);
			}
			if (hediffWithComps.comps != null)
			{
				for (int i = 0; i < hediffWithComps.comps.Count; i++)
				{
					T t = hediffWithComps.comps[i] as T;
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000E7025 File Offset: 0x000E5225
		public static bool TryGetComp<T>(this Hediff hd, out T comp) where T : HediffComp
		{
			comp = hd.TryGetComp<T>();
			return comp != null;
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000E7044 File Offset: 0x000E5244
		public static bool IsTended(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_TendDuration hediffComp_TendDuration = hediffWithComps.TryGetComp<HediffComp_TendDuration>();
			return hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended;
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000E7070 File Offset: 0x000E5270
		public static bool IsPermanent(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_GetsPermanent hediffComp_GetsPermanent = hediffWithComps.TryGetComp<HediffComp_GetsPermanent>();
			return hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent;
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000E709C File Offset: 0x000E529C
		public static bool FullyImmune(this Hediff hd)
		{
			HediffWithComps hediffWithComps = hd as HediffWithComps;
			if (hediffWithComps == null)
			{
				return false;
			}
			HediffComp_Immunizable hediffComp_Immunizable = hediffWithComps.TryGetComp<HediffComp_Immunizable>();
			return hediffComp_Immunizable != null && hediffComp_Immunizable.FullyImmune;
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000E70C7 File Offset: 0x000E52C7
		public static bool CanHealFromTending(this Hediff_Injury hd)
		{
			return hd.IsTended() && !hd.IsPermanent();
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000E70DC File Offset: 0x000E52DC
		public static bool CanHealNaturally(this Hediff_Injury hd)
		{
			return !hd.IsPermanent();
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000E70E8 File Offset: 0x000E52E8
		public static int CountAddedAndImplantedParts(this HediffSet hs)
		{
			int num = 0;
			List<Hediff> hediffs = hs.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.countsAsAddedPartOrImplant)
				{
					num++;
				}
			}
			return num;
		}
	}
}
