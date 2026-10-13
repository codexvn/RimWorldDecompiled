using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200285A RID: 10330
	public abstract class SitePartWorker_WorkSite : SitePartWorker
	{
		// Token: 0x1700226F RID: 8815
		// (get) Token: 0x0600E1DD RID: 57821
		public abstract IEnumerable<PreceptDef> DisallowedPrecepts { get; }

		// Token: 0x17002270 RID: 8816
		// (get) Token: 0x0600E1DE RID: 57822
		public abstract PawnGroupKindDef WorkerGroupKind { get; }

		// Token: 0x0600E1DF RID: 57823 RVA: 0x00422D01 File Offset: 0x00420F01
		public virtual bool CanSpawnOn(PlanetTile tile)
		{
			return this.LootThings(tile).Any<SitePartWorker_WorkSite.CampLootThingStruct>();
		}

		// Token: 0x0600E1E0 RID: 57824 RVA: 0x00422D10 File Offset: 0x00420F10
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			SitePartWorker_WorkSite.CampLootThingStruct campLootThingStruct = this.LootThings(site.Tile).RandomElementByWeight((SitePartWorker_WorkSite.CampLootThingStruct t) => t.weight);
			this.OnLootChosen(site, sitePart, campLootThingStruct);
			float x = SitePartWorker_WorkSite.PointsMarketValue.Evaluate(sitePart.parms.threatPoints);
			List<ThingDefCount> list = new List<ThingDefCount>();
			sitePart.things = new ThingOwner<Thing>(sitePart);
			sitePart.things.dontTickContents = true;
			List<ThingDefCount> list2 = new List<ThingDefCount>();
			float num = SitePartWorker_WorkSite.PointsMarketValue.Evaluate(x);
			if (campLootThingStruct.thing2 == null)
			{
				list2.Add(new ThingDefCount(campLootThingStruct.thing, Mathf.CeilToInt(num / campLootThingStruct.thing.BaseMarketValue)));
			}
			else
			{
				list2.Add(new ThingDefCount(campLootThingStruct.thing, Mathf.CeilToInt(num / 2f / campLootThingStruct.thing.BaseMarketValue)));
				list2.Add(new ThingDefCount(campLootThingStruct.thing2, Mathf.CeilToInt(num / 2f / campLootThingStruct.thing2.BaseMarketValue)));
			}
			foreach (ThingDefCount thingDefCount in list2)
			{
				int i = thingDefCount.Count;
				ThingDef thingDef = thingDefCount.ThingDef;
				while (i > 0)
				{
					Thing thing = ThingMaker.MakeThing(thingDef, null);
					thing.stackCount = Mathf.Min(i, thing.def.stackLimit);
					list.Add(new ThingDefCount(thingDef, thing.stackCount));
					i -= thing.stackCount;
					sitePart.things.TryAdd(thing, true);
				}
			}
			sitePart.lootThings = list;
			sitePart.expectedEnemyCount = this.GetEnemiesCount(site, sitePart.parms);
		}

		// Token: 0x0600E1E1 RID: 57825 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void OnLootChosen(Site site, SitePart sitePart, SitePartWorker_WorkSite.CampLootThingStruct loot)
		{
		}

		// Token: 0x0600E1E2 RID: 57826 RVA: 0x00422EEC File Offset: 0x004210EC
		public virtual IEnumerable<SitePartWorker_WorkSite.CampLootThingStruct> LootThings(PlanetTile tile)
		{
			foreach (SitePartDef.WorkSiteLootThing workSiteLootThing in this.def.lootTable)
			{
				yield return new SitePartWorker_WorkSite.CampLootThingStruct
				{
					thing = workSiteLootThing.thing,
					weight = workSiteLootThing.weight
				};
			}
			List<SitePartDef.WorkSiteLootThing>.Enumerator enumerator = default(List<SitePartDef.WorkSiteLootThing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600E1E3 RID: 57827 RVA: 0x004218AA File Offset: 0x0041FAAA
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600E1E4 RID: 57828 RVA: 0x00422EFC File Offset: 0x004210FC
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = this.GetEnemiesCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("enemiesLabel", this.GetEnemiesLabel(part.site, enemiesCount)));
		}

		// Token: 0x0600E1E5 RID: 57829 RVA: 0x00422F5C File Offset: 0x0042115C
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return (site.Label + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetEnemiesCount(site, sitePart.parms), "People".Translate())).TrimEndNewlines() + ("\n" + "Contains".Translate() + ": " + string.Join(", ", (from t in sitePart.lootThings
			select t.ThingDef).Distinct<ThingDef>().Select(delegate(ThingDef t)
			{
				int num = 0;
				foreach (ThingDefCount thingDefCount in sitePart.lootThings)
				{
					if (thingDefCount.ThingDef == t)
					{
						num += thingDefCount.Count;
					}
				}
				return t.LabelCap + " x" + num.ToString();
			})));
		}

		// Token: 0x0600E1E6 RID: 57830 RVA: 0x00423041 File Offset: 0x00421241
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement, null));
			return sitePartParams;
		}

		// Token: 0x0600E1E7 RID: 57831 RVA: 0x00423070 File Offset: 0x00421270
		public override bool FactionCanOwn(Faction faction)
		{
			if (faction.Hidden || faction.temporary || !faction.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == this.WorkerGroupKind))
			{
				return false;
			}
			if (faction.ideos == null || faction.ideos.PrimaryIdeo == null)
			{
				return true;
			}
			Ideo ideology = faction.ideos.PrimaryIdeo;
			return !this.DisallowedPrecepts.Any((PreceptDef pDef) => ideology.PreceptsListForReading.Any((Precept p) => p.def == pDef));
		}

		// Token: 0x0600E1E8 RID: 57832 RVA: 0x004230FC File Offset: 0x004212FC
		protected int GetEnemiesCount(Site site, SitePartParams parms)
		{
			return GenStep_WorkSitePawns.GetEnemiesCount(site, parms, this.WorkerGroupKind);
		}

		// Token: 0x0600E1E9 RID: 57833 RVA: 0x0042310C File Offset: 0x0042130C
		protected string GetEnemiesLabel(Site site, int enemiesCount)
		{
			if (site.Faction == null)
			{
				return (enemiesCount == 1) ? "Enemy".Translate() : "Enemies".Translate();
			}
			if (enemiesCount != 1)
			{
				return site.Faction.def.pawnsPlural;
			}
			return site.Faction.def.pawnSingular;
		}

		// Token: 0x040097A7 RID: 38823
		public static readonly SimpleCurve PointsMarketValue = new SimpleCurve
		{
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(250f, 450f),
				true
			},
			{
				new CurvePoint(800f, 2000f),
				true
			},
			{
				new CurvePoint(10000f, 5000f),
				true
			}
		};

		// Token: 0x0200285B RID: 10331
		public struct CampLootThingStruct
		{
			// Token: 0x040097A8 RID: 38824
			public ThingDef thing;

			// Token: 0x040097A9 RID: 38825
			public ThingDef thing2;

			// Token: 0x040097AA RID: 38826
			public float weight;
		}
	}
}
