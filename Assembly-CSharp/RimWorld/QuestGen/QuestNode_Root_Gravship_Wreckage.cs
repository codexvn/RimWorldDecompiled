using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B1C RID: 15132
	public class QuestNode_Root_Gravship_Wreckage : QuestNode_Root_Gravcore
	{
		// Token: 0x06014961 RID: 84321 RVA: 0x006246F0 File Offset: 0x006228F0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Quest quest = QuestGen.quest;
			PlanetTile tile;
			if (!this.TryFindSiteTile(out tile))
			{
				Log.Error("Could not find valid site tile for gravengine wreckage quest.");
				return;
			}
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapSettled");
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.GravshipWreckage, new SitePartParams
				{
					points = slate.Get<float>("points", 0f, false),
					threatPoints = slate.Get<float>("points", 0f, false)
				})
			}, tile, null, false, null, WorldObjectDefOf.ClaimableSite);
			site.questTags = new List<string>
			{
				QuestGen.slate.CurrentPrefix
			};
			slate.Set<Site>("site", site, false);
			quest.SpawnWorldObject(site, null, null);
			Pawn pawn = this.FindAsker();
			slate.Set<Pawn>("asker", pawn, false);
			slate.Set<bool>("askerIsNull", pawn == null, false);
			QuestPart_Choice.Choice choice = new QuestPart_Choice.Choice();
			choice.rewards.Add(new Reward_DefinedThingDef(ThingDefOf.GravEngine));
			quest.RewardChoice(null, null).choices.Add(choice);
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.AddPart<QuestPart_EngineClaimed>();
		}

		// Token: 0x06014962 RID: 84322 RVA: 0x0062483C File Offset: 0x00622A3C
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive && ResearchProjectDefOf.BasicGravtech.IsFinished && !GravshipUtility.PlayerHasGravEngine() && base.TestRunInt(slate);
		}

		// Token: 0x06014963 RID: 84323 RVA: 0x00624868 File Offset: 0x00622A68
		private Pawn FindAsker()
		{
			Faction faction;
			if ((from f in Find.FactionManager.AllFactionsVisible
			where f.def.humanlikeFaction && !f.IsPlayer && !f.HostileTo(Faction.OfPlayer) && f.def.techLevel > TechLevel.Neolithic && f.leader != null && !f.temporary && !f.Hidden
			select f).TryRandomElement(out faction))
			{
				return faction.leader;
			}
			return null;
		}
	}
}
