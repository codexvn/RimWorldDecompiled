using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B06 RID: 15110
	public class QuestNode_Root_Gravcore_AbandonedSettlement : QuestNode_Root_Gravcore
	{
		// Token: 0x06014915 RID: 84245 RVA: 0x006227C8 File Offset: 0x006209C8
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				return;
			}
			Faction hostileFaction;
			Faction faction;
			if (!this.TryGetFactions(out faction, out hostileFaction))
			{
				return;
			}
			slate.Set<Faction>("settlementFaction", faction, false);
			slate.Set<Faction>("hostileFaction", hostileFaction, false);
			float points = slate.Get<float>("points", 0f, false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapGenerated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string inSignal3 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapSettled");
			string inSignalEnable = QuestGenUtility.HardcodedSignalWithQuestID("gravcore.Unfogged");
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.AbandonedSettlement, new SitePartParams
				{
					points = 3000f,
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, faction, false, null, WorldObjectDefOf.ClaimableSite);
			site.desiredThreatPoints = 100f;
			site.doorsAlwaysOpenForPlayerPawns = true;
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			slate.Set<Thing>("gravcore", site.parts[0].things.FirstOrDefault((Thing t) => t.def == ThingDefOf.Gravcore), false);
			quest.Letter(LetterDefOf.NeutralEvent, inSignal, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[letterTextMapGenerated]", null, "[letterLabelMapGenerated]", null, null);
			if (Find.Storyteller.difficulty.allowViolentQuests)
			{
				quest.Delay(Rand.RangeInclusive(60, 120), delegate
				{
					int num = 0;
					int raidsCount = QuestNode_Root_Gravcore_AbandonedSettlement.RaidsCountRange.RandomInRange;
					int i;
					Action <>9__2;
					int j;
					for (i = 0; i < raidsCount; i = j + 1)
					{
						int num2 = (i == 0) ? 0 : QuestNode_Root_Gravcore_AbandonedSettlement.TicksBetweenRaidsRange.RandomInRange;
						Quest quest = quest;
						int delayTicks = num + num2;
						Action inner;
						if ((inner = <>9__2) == null)
						{
							inner = (<>9__2 = delegate()
							{
								Faction hostileFaction;
								string text = "GravcoreAbandonedSettlementRaid".Translate(hostileFaction.Named("FACTION")).Resolve();
								if (i < raidsCount - 1)
								{
									text = text + "\n\n" + "GravcoreAbandonedSettlementRaid_Reinforcements".Translate().Resolve();
								}
								Quest quest2 = quest;
								MapParent site = site;
								FloatRange pointsRange = new FloatRange(points);
								hostileFaction = hostileFaction;
								string inSignal4 = null;
								RaidStrategyDef immediateAttack = RaidStrategyDefOf.ImmediateAttack;
								quest2.RandomRaid(site, pointsRange, hostileFaction, inSignal4, PawnsArrivalModeDefOf.EdgeWalkInGroups, immediateAttack, "LetterLabelAmbushInExistingMap".Translate().CapitalizeFirst() + ": " + hostileFaction.Name, text, false);
							});
						}
						quest.Delay(delayTicks, inner, null, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false).debugLabel = "Raid delay";
						num += num2;
						j = i;
					}
				}, inSignalEnable, null, null, false, null, null, false, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, false);
			}
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.Gravcore));
			quest.RewardChoice(null, null).choices.Add(choice);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal3, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x06014916 RID: 84246 RVA: 0x00622A0C File Offset: 0x00620C0C
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			Faction faction2;
			return base.TestRunInt(slate) && this.TryGetFactions(out faction, out faction2);
		}

		// Token: 0x06014917 RID: 84247 RVA: 0x00622A34 File Offset: 0x00620C34
		private bool TryGetFactions(out Faction settlementFaction, out Faction hostileFaction)
		{
			IEnumerable<Faction> source = from x in Find.FactionManager.AllFactionsVisible
			where x.def.humanlikeFaction && !x.IsPlayer && !x.temporary
			select x;
			Faction setFac;
			if (!source.TryRandomElement(out setFac))
			{
				hostileFaction = null;
				settlementFaction = null;
				return false;
			}
			settlementFaction = setFac;
			if (!(from x in source
			where x.HostileTo(Faction.OfPlayer) && x != setFac
			select x).TryRandomElement(out hostileFaction))
			{
				hostileFaction = null;
				return false;
			}
			return true;
		}

		// Token: 0x0400E468 RID: 58472
		private static readonly IntRange TicksBetweenRaidsRange = new IntRange(30000, 60000);

		// Token: 0x0400E469 RID: 58473
		private static readonly IntRange RaidsCountRange = new IntRange(3, 4);

		// Token: 0x0400E46A RID: 58474
		private const int SettlementPoints = 3000;

		// Token: 0x0400E46B RID: 58475
		private const string GravcoreAlias = "gravcore";
	}
}
