using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B7D RID: 11133
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		// Token: 0x0600F8FB RID: 63739 RVA: 0x00486438 File Offset: 0x00484638
		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && !x.building.mineablePreventMeteorite && !x.IsSmoothed
			select x);
		}

		// Token: 0x0600F8FC RID: 63740 RVA: 0x00486478 File Offset: 0x00484678
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			int randomInRange = (parms.countRange ?? ThingSetMaker_Meteorite.MineablesCountRange).RandomInRange;
			ThingDef def = this.FindRandomMineableDef();
			for (int i = 0; i < randomInRange; i++)
			{
				Building building = (Building)ThingMaker.MakeThing(def, null);
				building.canChangeTerrainOnDestroyed = false;
				outThings.Add(building);
			}
		}

		// Token: 0x0600F8FD RID: 63741 RVA: 0x004864E4 File Offset: 0x004846E4
		private ThingDef FindRandomMineableDef()
		{
			float value = Rand.Value;
			if (value < 0.4f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where !x.building.isResourceRock
				select x).RandomElement<ThingDef>();
			}
			if (value < 0.75f)
			{
				return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5f
				select x).RandomElement<ThingDef>();
			}
			return (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
			where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5f
			select x).RandomElement<ThingDef>();
		}

		// Token: 0x0600F8FE RID: 63742 RVA: 0x00486593 File Offset: 0x00484793
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}

		// Token: 0x0400A3B7 RID: 41911
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		// Token: 0x0400A3B8 RID: 41912
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		// Token: 0x0400A3B9 RID: 41913
		private const float PreciousMineableMarketValue = 5f;
	}
}
