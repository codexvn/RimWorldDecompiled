using System;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DE9 RID: 15849
	public class SitePartWorker_AbandonedSettlement : SitePartWorker
	{
		// Token: 0x06015A0F RID: 88591 RVA: 0x00679504 File Offset: 0x00677704
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.things = new ThingOwner<Thing>(sitePart);
			sitePart.things.TryAdd(ThingMaker.MakeThing(ThingDefOf.Gravcore, null), true);
			Thing thing = ThingMaker.MakeThing(ThingDefOf.GravlitePanel, null);
			int i = SitePartWorker_AbandonedSettlement.GravlitePanelsCountRange.RandomInRange;
			while (i > 0)
			{
				int num = Mathf.Min(i, thing.def.stackLimit);
				i -= num;
				Thing thing2 = ThingMaker.MakeThing(ThingDefOf.GravlitePanel, null);
				thing2.stackCount = num;
				sitePart.things.TryAdd(thing2, true);
			}
		}

		// Token: 0x06015A10 RID: 88592 RVA: 0x00679598 File Offset: 0x00677798
		public override void PostMapGenerate(Map map)
		{
			float points = SymbolResolver_Settlement.DefaultPawnsPoints.RandomInRange * 0.2f;
			Faction faction;
			Find.FactionManager.GetFactions(false, false, true, TechLevel.Undefined, false).TryRandomElement(out faction);
			MapGenUtility.ScatterCorpses(map, faction, points, new FloatRange(20f, 60f));
			MapGenUtility.DestroyTurrets(map);
			MapGenUtility.ForbidAllItems(map);
		}

		// Token: 0x0400F05C RID: 61532
		private const float CorpsePointFactor = 0.2f;

		// Token: 0x0400F05D RID: 61533
		public static readonly IntRange GravlitePanelsCountRange = new IntRange(150, 200);
	}
}
