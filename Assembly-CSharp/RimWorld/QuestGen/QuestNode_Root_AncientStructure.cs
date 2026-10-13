using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B29 RID: 15145
	public class QuestNode_Root_AncientStructure : QuestNode
	{
		// Token: 0x0601498E RID: 84366 RVA: 0x00624C29 File Offset: 0x00622E29
		protected override bool TestRunInt(Slate slate)
		{
			return ModsConfig.OdysseyActive;
		}

		// Token: 0x0601498F RID: 84367 RVA: 0x00625514 File Offset: 0x00623714
		protected override void RunInt()
		{
			Quest quest = QuestGen.quest;
			Slate slate = QuestGen.slate;
			slate.Get<float>("points", 0f, false);
			PlanetTile tile = slate.Get<PlanetTile>("siteTile", default(PlanetTile), false);
			tile.Tile.AddMutator(this.structureMutator.GetValue(slate));
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(this.sitePartDef.GetValue(slate), new SitePartParams())
			}, tile, Faction.OfAncientsHostile, false, null, WorldObjectDefOf.ClaimableSite);
			quest.SpawnWorldObject(site, null, null);
			slate.Set<Site>("site", site, false);
			slate.Set<bool>("atLandmark", site.Tile.Tile.Landmark != null, false);
			Slate slate2 = slate;
			string name = "landmarkName";
			Landmark landmark = site.Tile.Tile.Landmark;
			slate2.Set<string>(name, ((landmark != null) ? landmark.name : null) ?? "", false);
			string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("site.AllEnemiesDefeated");
			string inSignal2 = QuestGenUtility.HardcodedSignalWithQuestID("site.MapRemoved");
			quest.End(QuestEndOutcome.Success, 0, null, inSignal, QuestPart.SignalListenMode.OngoingOnly, false, false);
			quest.End(QuestEndOutcome.Unknown, 0, null, inSignal2, QuestPart.SignalListenMode.OngoingOnly, false, false);
		}

		// Token: 0x0400E4B0 RID: 58544
		public SlateRef<TileMutatorDef> structureMutator;

		// Token: 0x0400E4B1 RID: 58545
		public SlateRef<SitePartDef> sitePartDef;
	}
}
