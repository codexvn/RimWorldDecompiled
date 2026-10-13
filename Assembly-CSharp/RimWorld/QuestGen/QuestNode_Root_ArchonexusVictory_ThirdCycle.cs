using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B30 RID: 15152
	public class QuestNode_Root_ArchonexusVictory_ThirdCycle : QuestNode_Root_ArchonexusVictory_Cycle
	{
		// Token: 0x1700335F RID: 13151
		// (get) Token: 0x060149AA RID: 84394 RVA: 0x00075162 File Offset: 0x00073362
		protected override int ArchonexusCycle
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x060149AB RID: 84395 RVA: 0x00625CEC File Offset: 0x00623EEC
		protected override void RunInt()
		{
			base.RunInt();
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			float num = slate.Get<float>("points", 0f, false);
			Faction faction = slate.Get<Faction>("roughTribe", null, false);
			PlanetTile tile;
			this.TryFindSiteTile(out tile, false);
			if (faction != null)
			{
				quest.RequirementsToAcceptFactionRelation(faction, FactionRelationKind.Ally, true);
			}
			base.TryAddStudyRequirement(quest, slate, ThingDefOf.GrandArchotechStructure);
			quest.DialogWithCloseBehavior("[questDescriptionBeforeAccepted]", null, quest.AddedSignal, null, null, QuestPart.SignalListenMode.NotYetAcceptedOnly, QuestPartDialogCloseAction.CloseActionKey.ArchonexusVictorySound3rd);
			quest.DescriptionPart("[questDescriptionBeforeAccepted]", quest.AddedSignal, quest.InitiateSignal, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, null);
			quest.DescriptionPart("[questDescriptionAfterAccepted]", quest.InitiateSignal, null, QuestPart.SignalListenMode.OngoingOrNotYetAccepted, null);
			quest.Letter(LetterDefOf.PositiveEvent, null, null, null, null, false, QuestPart.SignalListenMode.OngoingOnly, null, false, "[questAcceptedLetterText]", null, "[questAcceptedLetterLabel]", null, null);
			float threatPoints = Find.Storyteller.difficulty.allowViolentQuests ? (num * QuestNode_Root_ArchonexusVictory_ThirdCycle.ThreatPointsFactor) : 0f;
			SitePartParams parms = new SitePartParams
			{
				threatPoints = threatPoints
			};
			Site site = QuestGen_Sites.GenerateSite(Gen.YieldSingle<SitePartDefWithParams>(new SitePartDefWithParams(SitePartDefOf.Archonexus, parms)), tile, Faction.OfAncients, false, null, null);
			if (num <= 0f && Find.Storyteller.difficulty.allowViolentQuests)
			{
				quest.SetSitePartThreatPointsToCurrent(site, SitePartDefOf.Archonexus, this.map.Parent, null, QuestNode_Root_ArchonexusVictory_ThirdCycle.ThreatPointsFactor);
			}
			quest.SpawnWorldObject(site, null, null);
			slate.Set<bool>("factionless", faction == null, false);
			slate.Set<bool>("threatsEnabled", Find.Storyteller.difficulty.allowViolentQuests, false);
		}

		// Token: 0x060149AC RID: 84396 RVA: 0x00625E78 File Offset: 0x00624078
		private bool TryFindSiteTile(out PlanetTile tile, bool exitOnFirstTileFound = false)
		{
			return TileFinder.TryFindNewSiteTile(out tile, 10, 40, false, null, 0f, true, TileFinderMode.Near, exitOnFirstTileFound, false, null, (PlanetTile p) => !ModsConfig.OdysseyActive || Find.World.landmarks[p] == null);
		}

		// Token: 0x060149AD RID: 84397 RVA: 0x00625EBC File Offset: 0x006240BC
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile planetTile;
			return base.TestRunInt(slate) && this.TryFindSiteTile(out planetTile, true);
		}

		// Token: 0x0400E4BE RID: 58558
		private const int MinDistanceFromColony = 10;

		// Token: 0x0400E4BF RID: 58559
		private const int MaxDistanceFromColony = 40;

		// Token: 0x0400E4C0 RID: 58560
		private static float ThreatPointsFactor = 0.6f;
	}
}
