using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E6F RID: 11887
	public static class ThingStyleHelper
	{
		// Token: 0x06010BD5 RID: 68565 RVA: 0x004D345B File Offset: 0x004D165B
		public static bool CanBeStyled(this ThingDef thingDef)
		{
			return thingDef.CompDefFor<CompStyleable>() != null;
		}

		// Token: 0x06010BD6 RID: 68566 RVA: 0x004D3468 File Offset: 0x004D1668
		public static bool GetEverSeenByPlayer(this Thing thing)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			bool? flag;
			if (thingWithComps == null)
			{
				flag = null;
			}
			else
			{
				CompStyleable compStyleable = thingWithComps.compStyleable;
				flag = ((compStyleable != null) ? new bool?(compStyleable.everSeenByPlayer) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x06010BD7 RID: 68567 RVA: 0x004D34B0 File Offset: 0x004D16B0
		public static ThingStyleDef GetStyleDef(this Thing thing)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return null;
			}
			CompStyleable compStyleable = thingWithComps.compStyleable;
			if (compStyleable == null)
			{
				return null;
			}
			return compStyleable.styleDef;
		}

		// Token: 0x06010BD8 RID: 68568 RVA: 0x004D34CE File Offset: 0x004D16CE
		public static Precept_ThingStyle GetStyleSourcePrecept(this Thing thing)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return null;
			}
			CompStyleable compStyleable = thingWithComps.compStyleable;
			if (compStyleable == null)
			{
				return null;
			}
			return compStyleable.SourcePrecept;
		}

		// Token: 0x06010BD9 RID: 68569 RVA: 0x004D34EC File Offset: 0x004D16EC
		public static void SetEverSeenByPlayer(this Thing thing, bool everSeenByPlayer)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
			if (compStyleable != null)
			{
				compStyleable.everSeenByPlayer = everSeenByPlayer;
			}
			if (everSeenByPlayer && thing.StyleSourcePrecept is Precept_Relic)
			{
				thing.StyleSourcePrecept.ideo.Notify_RelicSeenByPlayer(thing);
			}
		}

		// Token: 0x06010BDA RID: 68570 RVA: 0x004D3538 File Offset: 0x004D1738
		public static void SetStyleDef(this Thing thing, ThingStyleDef styleDef)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
			if (compStyleable == null)
			{
				if (styleDef != null)
				{
					Log.Warning("Tried setting ThingStyleDef to a thing without CompStyleable (" + thing.def.defName + ")!");
				}
				return;
			}
			compStyleable.styleDef = styleDef;
			compStyleable.cachedStyleCategoryDef = null;
		}

		// Token: 0x06010BDB RID: 68571 RVA: 0x004D358C File Offset: 0x004D178C
		public static void SetStyleSourcePrecept(this Thing thing, Precept_ThingStyle precept)
		{
			ThingWithComps thingWithComps = thing as ThingWithComps;
			CompStyleable compStyleable = (thingWithComps != null) ? thingWithComps.compStyleable : null;
			if (compStyleable == null)
			{
				if (precept != null)
				{
					Log.Warning("Tried setting StyleSourcePrecept to a thing without CompStyleable (" + thing.def.defName + ")!");
				}
				return;
			}
			compStyleable.SourcePrecept = precept;
		}
	}
}
