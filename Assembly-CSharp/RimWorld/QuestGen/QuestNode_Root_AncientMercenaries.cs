using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B20 RID: 15136
	public class QuestNode_Root_AncientMercenaries : QuestNode
	{
		// Token: 0x06014973 RID: 84339 RVA: 0x00624C29 File Offset: 0x00622E29
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive;
		}

		// Token: 0x06014974 RID: 84340 RVA: 0x00624C38 File Offset: 0x00622E38
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			float num = slate.Get<float>("points", 0f, false);
			PlanetTile tile = slate.Get<PlanetTile>("siteTile", default(PlanetTile), false);
			ThingSetMakerParams parms = new ThingSetMakerParams
			{
				makingFaction = Faction.OfAncientsHostile,
				countRange = new IntRange?(new IntRange(1, 1)),
				totalMarketValueRange = new FloatRange?(new FloatRange(0.7f, 1.3f) * QuestTuning.PointsToRewardMarketValueCurve.Evaluate(num))
			};
			List<Thing> list = ThingSetMakerDefOf.Reward_UniqueWeapon.root.Generate(parms);
			if (list.Count != 1)
			{
				Log.Error("Expected 1 unique weapon, got " + list.Count.ToString());
			}
			ThingWithComps thingWithComps = list.First<Thing>() as ThingWithComps;
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.AncientSoldier_Leader, Faction.OfAncientsHostile, null);
			if (thingWithComps != null)
			{
				pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				pawn.equipment.AddEquipment(thingWithComps);
			}
			IEnumerable<Pawn> collection = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Combat,
				faction = Faction.OfAncientsHostile,
				points = Mathf.Max(num / 2f, Faction.OfAncientsHostile.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null) * 1.05f)
			}, true);
			List<Pawn> list2 = new List<Pawn>();
			list2.Add(pawn);
			list2.AddRange(collection);
			List<TileMutatorDef> source = (from x in DefDatabase<TileMutatorDef>.AllDefs
			where x.categories.Contains("AncientStructure")
			select x).ToList<TileMutatorDef>();
			tile.Tile.AddMutator(source.RandomElement<TileMutatorDef>());
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.BanditGang, new SitePartParams())
			}, tile, Faction.OfAncientsHostile, false, null, WorldObjectDefOf.ClaimableSite);
			quest.SpawnWorldObject(site, null, null);
			slate.Set<Site>("site", site, false);
			slate.Set<Pawn>("LEADER", pawn, false);
			slate.Set<ThingWithComps>("WEAPON", thingWithComps, false);
			slate.Set<string>("mercenaryList", PawnUtility.PawnKindsToLineList(from p in list2
			select p.kindDef, "  - ", ColoredText.ThreatColor), false);
			slate.Set<bool>("atLandmark", site.Tile.Tile.Landmark != null, false);
			Slate slate2 = slate;
			string name = "landmarkName";
			Landmark landmark = site.Tile.Tile.Landmark;
			slate2.Set<string>(name, ((landmark != null) ? landmark.name : null) ?? "", false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.AllEnemiesDefeated");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			quest.WorldObjectTimeout(site, 1800000, null, null, false, null, true);
			quest.Delay(1800000, delegate
			{
				quest.End(QuestEndOutcome.Fail, 0, null, null, QuestPart.SignalListenMode.OngoingOnly, false, false);
			}, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			quest.AddPart(new QuestPart_SpawnPawnsInStructure(list2, inSignal));
			quest.End(QuestEndOutcome.Success, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x0400E49C RID: 58524
		private const int TimeoutTicks = 1800000;
	}
}
