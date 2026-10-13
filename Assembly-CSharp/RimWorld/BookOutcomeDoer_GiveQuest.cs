using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001937 RID: 6455
	[StaticConstructorOnStartup]
	public class BookOutcomeDoer_GiveQuest : BookOutcomeDoer
	{
		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x060092C5 RID: 37573 RVA: 0x002B36F1 File Offset: 0x002B18F1
		public new BookOutcomeProperties_GiveQuest Props
		{
			get
			{
				return (BookOutcomeProperties_GiveQuest)this.props;
			}
		}

		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x060092C6 RID: 37574 RVA: 0x002B36FE File Offset: 0x002B18FE
		private bool QuestGiven
		{
			get
			{
				return this.quest != null;
			}
		}

		// Token: 0x060092C7 RID: 37575 RVA: 0x002B3709 File Offset: 0x002B1909
		public override bool BenefitDetailsCanChange(Pawn reader = null)
		{
			return !this.QuestGiven && this.hasQuest;
		}

		// Token: 0x060092C8 RID: 37576 RVA: 0x002B371C File Offset: 0x002B191C
		public override string GetBenefitsString(Pawn reader = null)
		{
			if (!this.hasQuest)
			{
				return null;
			}
			TaggedString taggedString = " - " + "BookHasQuest".Translate();
			if (this.QuestGiven)
			{
				taggedString += string.Format(" ({0})", "DiscoveredQuest".Translate());
				taggedString = taggedString.Colorize(Color.gray);
			}
			return taggedString.Resolve();
		}

		// Token: 0x060092C9 RID: 37577 RVA: 0x002B3788 File Offset: 0x002B1988
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			return this.hasQuest && !this.QuestGiven;
		}

		// Token: 0x060092CA RID: 37578 RVA: 0x002B37A0 File Offset: 0x002B19A0
		public override void OnBookGenerated(Pawn author = null)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.hasQuest = Rand.Chance(this.Props.questChance);
			if (this.hasQuest)
			{
				this.questDef = this.GetQuestDef();
			}
			if (this.questDef == null)
			{
				this.hasQuest = false;
			}
		}

		// Token: 0x060092CB RID: 37579 RVA: 0x002B37F0 File Offset: 0x002B19F0
		private QuestScriptDef GetQuestDef()
		{
			IEnumerable<QuestScriptDef> giverQuests = QuestUtility.GetGiverQuests(QuestGiverTag.Reading);
			if (giverQuests.EnumerableNullOrEmpty<QuestScriptDef>())
			{
				return null;
			}
			return giverQuests.RandomElementByWeight((QuestScriptDef q) => q.rootSelectionWeight);
		}

		// Token: 0x060092CC RID: 37580 RVA: 0x002B3833 File Offset: 0x002B1A33
		public override void OnReadingTick(Pawn reader, float factor)
		{
			if (!this.hasQuest || this.QuestGiven)
			{
				return;
			}
			if (Rand.MTBEventOccurs(12500f, 1f, 1f) || this.giveNext)
			{
				this.GenerateQuest(reader);
			}
		}

		// Token: 0x060092CD RID: 37581 RVA: 0x002B386C File Offset: 0x002B1A6C
		private void GenerateQuest(Pawn reader)
		{
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(reader.Map), false);
			slate.Set<TaggedString>("discoveryMethod", "QuestDiscoveredFromBook".Translate(base.Book.Named("BOOK"), reader.Named("READER")), false);
			if (this.questDef == null)
			{
				this.questDef = this.GetQuestDef();
			}
			if (this.questDef == null)
			{
				return;
			}
			this.quest = QuestUtility.GenerateQuestAndMakeAvailable(this.questDef, slate);
			Messages.Message("MessageBookGaveQuest".Translate(this.quest.name, base.Book.Named("BOOK"), reader.Named("READER")), MessageTypeDefOf.PositiveEvent, true);
			if (!this.quest.hidden && this.quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(this.quest, slate.Get<string>("discoveryMethod", null, false));
			}
		}

		// Token: 0x060092CE RID: 37582 RVA: 0x002B3972 File Offset: 0x002B1B72
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (this.QuestGiven)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandViewQuest".Translate(this.quest.name),
					defaultDesc = "CommandViewQuestDesc".Translate(),
					icon = BookOutcomeDoer_GiveQuest.ViewQuestCommandTex,
					action = delegate()
					{
						Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
						((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					}
				};
			}
			else if (DebugSettings.godMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Give quest",
					icon = TexUI.Placeholder,
					action = delegate()
					{
						this.giveNext = true;
					}
				};
			}
			yield break;
		}

		// Token: 0x060092CF RID: 37583 RVA: 0x002B3982 File Offset: 0x002B1B82
		public override IEnumerable<RulePack> GetTopicRulePacks()
		{
			if (!this.hasQuest)
			{
				yield break;
			}
			yield return this.questDef.questSubjectRules;
			yield break;
		}

		// Token: 0x060092D0 RID: 37584 RVA: 0x002B3992 File Offset: 0x002B1B92
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.hasQuest, "hasQuest", false, false);
			Scribe_Defs.Look<QuestScriptDef>(ref this.questDef, "questDef");
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
		}

		// Token: 0x04005DA0 RID: 23968
		private bool hasQuest;

		// Token: 0x04005DA1 RID: 23969
		private QuestScriptDef questDef;

		// Token: 0x04005DA2 RID: 23970
		private Quest quest;

		// Token: 0x04005DA3 RID: 23971
		private bool giveNext;

		// Token: 0x04005DA4 RID: 23972
		private const int ReceiveQuestMTBTicks = 12500;

		// Token: 0x04005DA5 RID: 23973
		private static readonly Texture2D ViewQuestCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewQuest", true);
	}
}
