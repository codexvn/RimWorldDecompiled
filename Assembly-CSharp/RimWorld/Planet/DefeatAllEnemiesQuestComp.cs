using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E24 RID: 15908
	public class DefeatAllEnemiesQuestComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x1700363A RID: 13882
		// (get) Token: 0x06015B9D RID: 88989 RVA: 0x0067F8EE File Offset: 0x0067DAEE
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x06015B9E RID: 88990 RVA: 0x0067F8F6 File Offset: 0x0067DAF6
		public DefeatAllEnemiesQuestComp()
		{
			this.rewards = new ThingOwner<Thing>(this);
		}

		// Token: 0x06015B9F RID: 88991 RVA: 0x0067F90A File Offset: 0x0067DB0A
		public void StartQuest(Faction requestingFaction, int relationsImprovement, List<Thing> rewards)
		{
			this.StopQuest();
			this.active = true;
			this.requestingFaction = requestingFaction;
			this.relationsImprovement = relationsImprovement;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
			this.rewards.TryAddRangeOrTransfer(rewards, true, false);
		}

		// Token: 0x06015BA0 RID: 88992 RVA: 0x0067F941 File Offset: 0x0067DB41
		public void StopQuest()
		{
			this.active = false;
			this.requestingFaction = null;
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06015BA1 RID: 88993 RVA: 0x0067F960 File Offset: 0x0067DB60
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			if (this.active)
			{
				MapParent mapParent = this.parent as MapParent;
				if (mapParent != null)
				{
					this.CheckAllEnemiesDefeated(mapParent);
				}
			}
		}

		// Token: 0x06015BA2 RID: 88994 RVA: 0x0067F992 File Offset: 0x0067DB92
		private void CheckAllEnemiesDefeated(MapParent mapParent)
		{
			if (!mapParent.HasMap)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(mapParent.Map, true, false))
			{
				return;
			}
			this.GiveRewardsAndSendLetter();
			this.StopQuest();
		}

		// Token: 0x06015BA3 RID: 88995 RVA: 0x0067F9BC File Offset: 0x0067DBBC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Values.Look<int>(ref this.relationsImprovement, "relationsImprovement", 0, false);
			Scribe_References.Look<Faction>(ref this.requestingFaction, "requestingFaction", false);
			Scribe_Deep.Look<ThingOwner>(ref this.rewards, "rewards", new object[]
			{
				this
			});
		}

		// Token: 0x06015BA4 RID: 88996 RVA: 0x0067FA20 File Offset: 0x0067DC20
		private void GiveRewardsAndSendLetter()
		{
			Map map = Find.AnyPlayerHomeMap ?? ((MapParent)this.parent).Map;
			DefeatAllEnemiesQuestComp.tmpRewards.AddRange(this.rewards);
			this.rewards.Clear();
			IntVec3 intVec = DropCellFinder.TradeDropSpot(map);
			DropPodUtility.DropThingsNear(intVec, map, DefeatAllEnemiesQuestComp.tmpRewards, 110, false, false, false, true, true, null);
			DefeatAllEnemiesQuestComp.tmpRewards.Clear();
			FactionRelationKind playerRelationKind = this.requestingFaction.PlayerRelationKind;
			TaggedString text = "LetterDefeatAllEnemiesQuestCompleted".Translate(this.requestingFaction.Name, this.relationsImprovement.ToString());
			Faction.OfPlayer.TryAffectGoodwillWith(this.requestingFaction, this.relationsImprovement, false, false, HistoryEventDefOf.QuestGoodwillReward, null);
			this.requestingFaction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, this.requestingFaction.PlayerRelationKind, null);
			Find.LetterStack.ReceiveLetter("LetterLabelDefeatAllEnemiesQuestCompleted".Translate(), text, LetterDefOf.PositiveEvent, new GlobalTargetInfo(intVec, map, false), this.requestingFaction, null, null, null, 0, true);
		}

		// Token: 0x06015BA5 RID: 88997 RVA: 0x0067FB32 File Offset: 0x0067DD32
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06015BA6 RID: 88998 RVA: 0x0067FB40 File Offset: 0x0067DD40
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.rewards;
		}

		// Token: 0x06015BA7 RID: 88999 RVA: 0x0067FB48 File Offset: 0x0067DD48
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.rewards.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x06015BA8 RID: 89000 RVA: 0x0067FB5C File Offset: 0x0067DD5C
		public override string CompInspectStringExtra()
		{
			if (this.active)
			{
				string value = GenThing.ThingsToCommaList(this.rewards, true, true, 5).CapitalizeFirst();
				return "QuestTargetDestroyInspectString".Translate(this.requestingFaction.Name, value, GenThing.GetMarketValue(this.rewards).ToStringMoney(null)).CapitalizeFirst();
			}
			return null;
		}

		// Token: 0x0400F135 RID: 61749
		private bool active;

		// Token: 0x0400F136 RID: 61750
		public Faction requestingFaction;

		// Token: 0x0400F137 RID: 61751
		public int relationsImprovement;

		// Token: 0x0400F138 RID: 61752
		public ThingOwner rewards;

		// Token: 0x0400F139 RID: 61753
		private static readonly List<Thing> tmpRewards = new List<Thing>();
	}
}
