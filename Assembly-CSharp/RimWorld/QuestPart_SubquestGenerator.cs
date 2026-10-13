using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB7 RID: 7351
	public abstract class QuestPart_SubquestGenerator : QuestPartActivable
	{
		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x0600A3CA RID: 41930 RVA: 0x002EF98B File Offset: 0x002EDB8B
		protected virtual bool CanGenerateSubquest
		{
			get
			{
				return this.PendingSubquestCount < this.maxActiveSubquests && (this.maxSuccessfulSubquests < 0 || this.SuccessfulSubquestCount + this.PendingSubquestCount < this.maxSuccessfulSubquests);
			}
		}

		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x0600A3CB RID: 41931 RVA: 0x002EF9BD File Offset: 0x002EDBBD
		protected int SuccessfulSubquestCount
		{
			get
			{
				return this.quest.GetSubquests(new QuestState?(QuestState.EndedSuccess)).Count<Quest>();
			}
		}

		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x0600A3CC RID: 41932 RVA: 0x002EF9D8 File Offset: 0x002EDBD8
		private int PendingSubquestCount
		{
			get
			{
				return this.quest.GetSubquests(null).Count((Quest q) => q.State == QuestState.Ongoing || q.State == QuestState.NotYetAccepted);
			}
		}

		// Token: 0x17001937 RID: 6455
		// (get) Token: 0x0600A3CD RID: 41933 RVA: 0x002EFA1D File Offset: 0x002EDC1D
		protected bool Paused
		{
			get
			{
				return Find.AnyPlayerHomeMap == null;
			}
		}

		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x0600A3CE RID: 41934 RVA: 0x002EFA28 File Offset: 0x002EDC28
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.expiryInfoPartKey.NullOrEmpty())
				{
					return null;
				}
				return string.Format("{0} {1} / {2}", this.expiryInfoPartKey.Translate(), this.SuccessfulSubquestCount, this.maxSuccessfulSubquests);
			}
		}

		// Token: 0x0600A3CF RID: 41935 RVA: 0x002EFA74 File Offset: 0x002EDC74
		public override void QuestPartTick()
		{
			if (this.subquestDefs.Count == 0)
			{
				return;
			}
			if (this.maxSuccessfulSubquests > 0 && this.SuccessfulSubquestCount >= this.maxSuccessfulSubquests)
			{
				base.Complete();
				return;
			}
			if (this.Paused)
			{
				return;
			}
			if (this.currentInterval != null)
			{
				this.currentInterval--;
				int? num = this.currentInterval;
				int num2 = 0;
				if (num.GetValueOrDefault() < num2 & num != null)
				{
					if (!this.TryGenerateSubquest())
					{
						Log.Warning("Failed to generate subquest for quest " + this.quest.root.defName);
					}
					this.currentInterval = null;
				}
			}
			if (this.currentInterval == null && this.CanGenerateSubquest)
			{
				this.currentInterval = new int?(this.interval.RandomInRange);
			}
		}

		// Token: 0x0600A3D0 RID: 41936
		protected abstract QuestScriptDef GetNextSubquestDef();

		// Token: 0x0600A3D1 RID: 41937
		protected abstract Slate InitSlate();

		// Token: 0x0600A3D2 RID: 41938 RVA: 0x002EFB70 File Offset: 0x002EDD70
		protected virtual bool TryGenerateSubquest()
		{
			QuestScriptDef nextSubquestDef = this.GetNextSubquestDef();
			if (nextSubquestDef == null)
			{
				return false;
			}
			Slate vars = this.InitSlate();
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(nextSubquestDef, vars);
			quest.parent = this.quest;
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest, null);
			}
			return true;
		}

		// Token: 0x0600A3D3 RID: 41939 RVA: 0x002EFBC4 File Offset: 0x002EDDC4
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Generate random subquest", true, true, true, null) && this.CanGenerateSubquest)
			{
				this.TryGenerateSubquest();
			}
			curY += rect.height + 4f;
			Rect rect2 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect2, "Generate specific subquest", true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (List<QuestScriptDef>.Enumerator enumerator = this.subquestDefs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						QuestScriptDef questDef = enumerator.Current;
						list.Add(new FloatMenuOption(questDef.defName, delegate()
						{
							Slate vars = this.InitSlate();
							Quest quest2 = QuestUtility.GenerateQuestAndMakeAvailable(questDef, vars);
							quest2.parent = this.quest;
							if (!quest2.hidden && quest2.root.sendAvailableLetter)
							{
								QuestUtility.SendLetterQuestAvailable(quest2, null);
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				if (list.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			curY += rect2.height + 4f;
			Rect rect3 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect3, "Remove active subquests", true, true, true, null))
			{
				foreach (Quest quest in this.quest.GetSubquests(null))
				{
					Find.QuestManager.Remove(quest);
				}
			}
			curY += rect3.height + 4f;
			Rect rect4 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect4, "Complete quest", true, true, true, null))
			{
				base.Complete();
			}
			curY += rect4.height + 4f;
		}

		// Token: 0x0600A3D4 RID: 41940 RVA: 0x002EFE04 File Offset: 0x002EE004
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int?>(ref this.currentInterval, "currentInterval", null, false);
			Scribe_Collections.Look<QuestScriptDef>(ref this.subquestDefs, "subquestDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<IntRange>(ref this.interval, "interval", default(IntRange), false);
			Scribe_Values.Look<int>(ref this.maxActiveSubquests, "maxActiveSubquests", 0, false);
			Scribe_Values.Look<int>(ref this.maxSuccessfulSubquests, "maxSuccessfulSubquests", -1, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartKey, "expiryInfoPartKey", null, false);
		}

		// Token: 0x04007124 RID: 28964
		public List<QuestScriptDef> subquestDefs = new List<QuestScriptDef>();

		// Token: 0x04007125 RID: 28965
		public IntRange interval;

		// Token: 0x04007126 RID: 28966
		public int maxActiveSubquests = 2;

		// Token: 0x04007127 RID: 28967
		public string expiryInfoPartKey;

		// Token: 0x04007128 RID: 28968
		public int maxSuccessfulSubquests = -1;

		// Token: 0x04007129 RID: 28969
		private int? currentInterval;
	}
}
