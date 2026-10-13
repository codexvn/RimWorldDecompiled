using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000B9E RID: 2974
	public abstract class ChoiceLetter : LetterWithTimeout
	{
		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06004C03 RID: 19459 RVA: 0x0018311A File Offset: 0x0018131A
		// (set) Token: 0x06004C04 RID: 19460 RVA: 0x00183122 File Offset: 0x00181322
		public TaggedString Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value.CapitalizeFirst();
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x06004C05 RID: 19461
		public abstract IEnumerable<DiaOption> Choices { get; }

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x00183131 File Offset: 0x00181331
		protected DiaOption Option_Close
		{
			get
			{
				return new DiaOption("Close".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x06004C07 RID: 19463 RVA: 0x00183160 File Offset: 0x00181360
		protected DiaOption Option_JumpToLocation
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target, CameraJumper.MovementMode.Pan);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x06004C08 RID: 19464 RVA: 0x001831D0 File Offset: 0x001813D0
		protected DiaOption Option_JumpToLocationAndPostpone
		{
			get
			{
				GlobalTargetInfo target = this.lookTargets.TryGetPrimaryTarget();
				DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
				diaOption.action = delegate()
				{
					CameraJumper.TryJumpAndSelect(target, CameraJumper.MovementMode.Pan);
				};
				diaOption.resolveTree = true;
				if (!CameraJumper.CanJump(target))
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x06004C09 RID: 19465 RVA: 0x00183237 File Offset: 0x00181437
		protected DiaOption Option_Reject
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate()
					{
						Find.LetterStack.RemoveLetter(this);
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x00183268 File Offset: 0x00181468
		protected DiaOption Option_Postpone
		{
			get
			{
				DiaOption diaOption = new DiaOption("PostponeLetter".Translate());
				diaOption.resolveTree = true;
				if (this.LastTickBeforeTimeout)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x06004C0B RID: 19467 RVA: 0x001832A4 File Offset: 0x001814A4
		protected DiaOption Option_ViewInQuestsTab(string labelKey = "ViewRelatedQuest", bool postpone = false)
		{
			TaggedString taggedString = labelKey.Translate();
			if (this.title != this.quest.name && !this.quest.hidden)
			{
				taggedString += ": " + this.quest.name;
			}
			DiaOption diaOption = new DiaOption(taggedString);
			diaOption.action = delegate()
			{
				if (this.quest != null)
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests, true);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(this.quest);
					if (!postpone)
					{
						Find.LetterStack.RemoveLetter(this);
					}
				}
			};
			diaOption.resolveTree = true;
			if (this.quest == null || this.quest.hidden)
			{
				diaOption.Disable(null);
			}
			return diaOption;
		}

		// Token: 0x06004C0C RID: 19468 RVA: 0x00183350 File Offset: 0x00181550
		protected DiaOption Option_ViewInfoCard(int index)
		{
			int num = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
			if (index >= num)
			{
				return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkHediffDefs[index - num], -1));
			}
			return new DiaOption(new Dialog_InfoCard.Hyperlink(this.hyperlinkThingDefs[index], -1));
		}

		// Token: 0x06004C0D RID: 19469 RVA: 0x001833AC File Offset: 0x001815AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Values.Look<TaggedString>(ref this.text, "text", default(TaggedString), false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Collections.Look<ThingDef>(ref this.hyperlinkThingDefs, "hyperlinkThingDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<HediffDef>(ref this.hyperlinkHediffDefs, "hyperlinkHediffDefs", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x06004C0E RID: 19470 RVA: 0x0018343A File Offset: 0x0018163A
		protected override string GetMouseoverText()
		{
			return this.text.Resolve();
		}

		// Token: 0x06004C0F RID: 19471 RVA: 0x00183448 File Offset: 0x00181648
		public override void OpenLetter()
		{
			DiaNode diaNode = new DiaNode(this.text);
			diaNode.options.AddRange(this.Choices);
			Dialog_NodeTreeWithFactionInfo window = new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, this.radioMode, this.title);
			Find.WindowStack.Add(window);
		}

		// Token: 0x04003647 RID: 13895
		public string title;

		// Token: 0x04003648 RID: 13896
		private TaggedString text;

		// Token: 0x04003649 RID: 13897
		public bool radioMode;

		// Token: 0x0400364A RID: 13898
		public Quest quest;

		// Token: 0x0400364B RID: 13899
		public List<ThingDef> hyperlinkThingDefs;

		// Token: 0x0400364C RID: 13900
		public List<HediffDef> hyperlinkHediffDefs;
	}
}
