using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A42 RID: 14914
	public class QuestNode_GetFaction : QuestNode
	{
		// Token: 0x06014681 RID: 83585 RVA: 0x00618870 File Offset: 0x00616A70
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			if (slate.TryGet<Faction>(this.storeAs.GetValue(slate), out faction, false) && this.IsGoodFaction(faction, slate))
			{
				return true;
			}
			if (this.TryFindFaction(out faction, slate))
			{
				slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
				return true;
			}
			return false;
		}

		// Token: 0x06014682 RID: 83586 RVA: 0x006188C4 File Offset: 0x00616AC4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Faction faction;
			if (QuestGen.slate.TryGet<Faction>(this.storeAs.GetValue(slate), out faction, false) && this.IsGoodFaction(faction, QuestGen.slate))
			{
				return;
			}
			if (!this.TryFindFaction(out faction, QuestGen.slate))
			{
				return;
			}
			QuestGen.slate.Set<Faction>(this.storeAs.GetValue(slate), faction, false);
			if (!faction.Hidden)
			{
				QuestPart_InvolvedFactions questPart_InvolvedFactions = new QuestPart_InvolvedFactions();
				questPart_InvolvedFactions.factions.Add(faction);
				QuestGen.quest.AddPart(questPart_InvolvedFactions);
			}
		}

		// Token: 0x06014683 RID: 83587 RVA: 0x00618950 File Offset: 0x00616B50
		private bool TryFindFaction(out Faction faction, Slate slate)
		{
			return (from x in Find.FactionManager.GetFactions(true, false, true, TechLevel.Undefined, false)
			where this.IsGoodFaction(x, slate)
			select x).TryRandomElement(out faction);
		}

		// Token: 0x06014684 RID: 83588 RVA: 0x00618998 File Offset: 0x00616B98
		private bool IsGoodFaction(Faction faction, Slate slate)
		{
			if (faction.Hidden && (this.allowedHiddenFactions.GetValue(slate) == null || !this.allowedHiddenFactions.GetValue(slate).Contains(faction)))
			{
				return false;
			}
			if (this.ofPawn.GetValue(slate) != null && faction != this.ofPawn.GetValue(slate).Faction)
			{
				return false;
			}
			if (this.exclude.GetValue(slate) != null && this.exclude.GetValue(slate).Contains(faction))
			{
				return false;
			}
			if (this.mustBePermanentEnemy.GetValue(slate) && !faction.def.permanentEnemy)
			{
				return false;
			}
			if (!this.allowEnemy.GetValue(slate) && faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (!this.allowNeutral.GetValue(slate) && faction.PlayerRelationKind == FactionRelationKind.Neutral)
			{
				return false;
			}
			if (!this.allowAlly.GetValue(slate) && faction.PlayerRelationKind == FactionRelationKind.Ally)
			{
				return false;
			}
			if (!(this.allowPermanentEnemy.GetValue(slate) ?? true) && faction.def.permanentEnemy)
			{
				return false;
			}
			if (this.playerCantBeAttackingCurrently.GetValue(slate) && SettlementUtility.IsPlayerAttackingAnySettlementOf(faction))
			{
				return false;
			}
			if (this.mustHaveGoodwillRewardsEnabled.GetValue(slate) && !faction.allowGoodwillRewards)
			{
				return false;
			}
			if (this.peaceTalksCantExist.GetValue(slate))
			{
				if (this.PeaceTalksExist(faction))
				{
					return false;
				}
				string tag = QuestNode_QuestUnique.GetProcessedTag("PeaceTalks", faction);
				if (Find.QuestManager.questsInDisplayOrder.Any((Quest q) => q.tags.Contains(tag)))
				{
					return false;
				}
			}
			if (this.leaderMustBeSafe.GetValue(slate) && (faction.leader == null || faction.leader.Spawned || faction.leader.IsPrisoner))
			{
				return false;
			}
			Thing value = this.mustBeHostileToFactionOf.GetValue(slate);
			return value == null || value.Faction == null || (value.Faction != faction && faction.HostileTo(value.Faction));
		}

		// Token: 0x06014685 RID: 83589 RVA: 0x00618B9C File Offset: 0x00616D9C
		private bool PeaceTalksExist(Faction faction)
		{
			List<PeaceTalks> peaceTalks = Find.WorldObjects.PeaceTalks;
			for (int i = 0; i < peaceTalks.Count; i++)
			{
				if (peaceTalks[i].Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400E191 RID: 57745
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E192 RID: 57746
		public SlateRef<bool> allowEnemy;

		// Token: 0x0400E193 RID: 57747
		public SlateRef<bool> allowNeutral;

		// Token: 0x0400E194 RID: 57748
		public SlateRef<bool> allowAlly;

		// Token: 0x0400E195 RID: 57749
		public SlateRef<bool> allowAskerFaction;

		// Token: 0x0400E196 RID: 57750
		public SlateRef<bool?> allowPermanentEnemy;

		// Token: 0x0400E197 RID: 57751
		public SlateRef<bool> mustBePermanentEnemy;

		// Token: 0x0400E198 RID: 57752
		public SlateRef<bool> playerCantBeAttackingCurrently;

		// Token: 0x0400E199 RID: 57753
		public SlateRef<bool> peaceTalksCantExist;

		// Token: 0x0400E19A RID: 57754
		public SlateRef<bool> leaderMustBeSafe;

		// Token: 0x0400E19B RID: 57755
		public SlateRef<bool> mustHaveGoodwillRewardsEnabled;

		// Token: 0x0400E19C RID: 57756
		public SlateRef<Pawn> ofPawn;

		// Token: 0x0400E19D RID: 57757
		public SlateRef<Thing> mustBeHostileToFactionOf;

		// Token: 0x0400E19E RID: 57758
		public SlateRef<IEnumerable<Faction>> exclude;

		// Token: 0x0400E19F RID: 57759
		public SlateRef<IEnumerable<Faction>> allowedHiddenFactions;
	}
}
