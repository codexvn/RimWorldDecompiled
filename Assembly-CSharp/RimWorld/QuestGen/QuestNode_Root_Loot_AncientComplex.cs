using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B59 RID: 15193
	public class QuestNode_Root_Loot_AncientComplex : QuestNode_Root_AncientComplex
	{
		// Token: 0x17003364 RID: 13156
		// (get) Token: 0x06014A58 RID: 84568 RVA: 0x0062BACC File Offset: 0x00629CCC
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex_Loot;
			}
		}

		// Token: 0x06014A59 RID: 84569 RVA: 0x0062BAD3 File Offset: 0x00629CD3
		protected virtual bool BeforeRunInt()
		{
			return ModLister.CheckIdeology("Ancient complex rescue");
		}

		// Token: 0x06014A5A RID: 84570 RVA: 0x0062BAE4 File Offset: 0x00629CE4
		protected override void RunInt()
		{
			if (!this.BeforeRunInt())
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			Map map = QuestGen_Get.GetMap(false, null, false);
			float num = slate.Get<float>("points", 0f, false);
			PlanetTile tile;
			if (!slate.TryGet<PlanetTile>("siteTile", out tile, false))
			{
				this.TryFindSiteTile(out tile, false);
			}
			Faction faction;
			this.TryFindEnemyFaction(out faction);
			QuestGen.GenerateNewSignal("RaidArrives", true);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			LayoutStructureSketch ancientLayoutStructureSketch = this.QuestSetupComplex(quest, num);
			SitePartParams parms = new SitePartParams
			{
				threatPoints = (Find.Storyteller.difficulty.allowViolentQuests ? QuestNode_Root_AncientComplex.ThreatPointsOverPointsCurve.Evaluate(num) : 0f),
				ancientLayoutStructureSketch = ancientLayoutStructureSketch,
				ancientComplexRewardMaker = ThingSetMakerDefOf.MapGen_AncientComplexRoomLoot_Better
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(this.SitePartDef, parms)), tile, Faction.OfAncients, false, null, null);
			quest.SpawnWorldObject(site, null, null);
			TimedDetectionRaids component = site.GetComponent<TimedDetectionRaids>();
			if (component != null)
			{
				component.alertRaidsArrivingIn = true;
			}
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			slate.Set<Map>("map", map, false);
			slate.Set<Site>("site", site, false);
		}

		// Token: 0x06014A5B RID: 84571 RVA: 0x0062BC1C File Offset: 0x00629E1C
		public override LayoutStructureSketch QuestSetupComplex(Quest quest, float points)
		{
			return this.GenerateStructureSketch(points, false);
		}

		// Token: 0x06014A5C RID: 84572 RVA: 0x0062BC28 File Offset: 0x00629E28
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 2, 10, false, QuestNode_Root_AncientComplex.AllowedLandmarks, 0.5f, true, TileFinderMode.Near, exitOnFirstTileFound, false, null, null);
		}

		// Token: 0x06014A5D RID: 84573 RVA: 0x0062BC4F File Offset: 0x00629E4F
		private bool TryFindEnemyFaction(out Faction enemyFaction)
		{
			enemyFaction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			return enemyFaction != null;
		}

		// Token: 0x06014A5E RID: 84574 RVA: 0x0062BC68 File Offset: 0x00629E68
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			Faction faction;
			return this.TryFindSiteTile(out planetTile, true) && this.TryFindEnemyFaction(out faction);
		}

		// Token: 0x0400E565 RID: 58725
		private const int MinDistanceFromColony = 2;

		// Token: 0x0400E566 RID: 58726
		private const int MaxDistanceFromColony = 10;
	}
}
