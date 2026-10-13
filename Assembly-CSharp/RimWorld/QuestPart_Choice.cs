using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C1C RID: 7196
	public class QuestPart_Choice : QuestPart
	{
		// Token: 0x170018A2 RID: 6306
		// (get) Token: 0x0600A043 RID: 41027 RVA: 0x002E2F8F File Offset: 0x002E118F
		public override bool PreventsAutoAccept
		{
			get
			{
				return this.choices.Count >= 2;
			}
		}

		// Token: 0x0600A044 RID: 41028 RVA: 0x002E2FA4 File Offset: 0x002E11A4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalChoiceUsed)
			{
				this.choiceUsed = true;
				for (int i = 0; i < this.choices.Count; i++)
				{
					for (int j = 0; j < this.choices[i].rewards.Count; j++)
					{
						this.choices[i].rewards[j].Notify_Used();
					}
				}
			}
		}

		// Token: 0x0600A045 RID: 41029 RVA: 0x002E3028 File Offset: 0x002E1228
		public override void Notify_PreCleanup()
		{
			base.Notify_PreCleanup();
			for (int i = 0; i < this.choices.Count; i++)
			{
				for (int j = 0; j < this.choices[i].rewards.Count; j++)
				{
					this.choices[i].rewards[j].Notify_PreCleanup();
				}
			}
		}

		// Token: 0x0600A046 RID: 41030 RVA: 0x002E3090 File Offset: 0x002E1290
		public override void Notify_PawnDiscarded(Pawn pawn)
		{
			foreach (QuestPart_Choice.Choice choice in this.choices)
			{
				for (int i = choice.rewards.Count - 1; i >= 0; i--)
				{
					Reward_Pawn reward_Pawn = choice.rewards[i] as Reward_Pawn;
					if (reward_Pawn != null && reward_Pawn.pawn == pawn)
					{
						choice.rewards.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x0600A047 RID: 41031 RVA: 0x002E3120 File Offset: 0x002E1320
		public void Choose(QuestPart_Choice.Choice choice)
		{
			for (int i = this.choices.Count - 1; i >= 0; i--)
			{
				if (this.choices[i] != choice)
				{
					for (int j = 0; j < this.choices[i].questParts.Count; j++)
					{
						if (!choice.questParts.Contains(this.choices[i].questParts[j]))
						{
							this.choices[i].questParts[j].Notify_PreCleanup();
							this.choices[i].questParts[j].Cleanup();
							this.quest.RemovePart(this.choices[i].questParts[j]);
						}
					}
					this.choices.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600A048 RID: 41032 RVA: 0x002E3210 File Offset: 0x002E1410
		public override void PreQuestAccept()
		{
			base.PreQuestAccept();
			if (this.choices.Count >= 2)
			{
				Log.Error("Tried to accept a quest but " + base.GetType().Name + " still has a choice unresolved. Auto-choosing the first option.");
				this.Choose(this.choices[0]);
			}
		}

		// Token: 0x0600A049 RID: 41033 RVA: 0x002E3264 File Offset: 0x002E1464
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			bool flag = false;
			for (int i = 0; i < this.choices.Count; i++)
			{
				for (int j = 0; j < this.choices[i].rewards.Count; j++)
				{
					Reward_Items reward_Items = this.choices[i].rewards[j] as Reward_Items;
					if (reward_Items != null)
					{
						for (int k = 0; k < reward_Items.items.Count; k++)
						{
							if (!flag && reward_Items.items[k].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								flag = true;
							}
							else if (ModsConfig.BiotechActive && !Find.History.mechlinkEverAvailable && reward_Items.items[k].def == ThingDefOf.Mechlink)
							{
								MechanitorUtility.Notify_MechlinkQuestRewardAvailable(this.quest, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600A04A RID: 41034 RVA: 0x002E3358 File Offset: 0x002E1558
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalChoiceUsed, "inSignalChoiceUsed", null, false);
			Scribe_Collections.Look<QuestPart_Choice.Choice>(ref this.choices, "choices", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.choiceUsed, "choiceUsed", false, false);
		}

		// Token: 0x04006ED2 RID: 28370
		public string inSignalChoiceUsed;

		// Token: 0x04006ED3 RID: 28371
		public List<QuestPart_Choice.Choice> choices = new List<QuestPart_Choice.Choice>();

		// Token: 0x04006ED4 RID: 28372
		public bool choiceUsed;

		// Token: 0x02001C1D RID: 7197
		public class Choice : IExposable
		{
			// Token: 0x0600A04C RID: 41036 RVA: 0x002E33B8 File Offset: 0x002E15B8
			public void ExposeData()
			{
				Scribe_Collections.Look<QuestPart>(ref this.questParts, "questParts", LookMode.Reference, Array.Empty<object>());
				Scribe_Collections.Look<Reward>(ref this.rewards, "rewards", LookMode.Deep, Array.Empty<object>());
			}

			// Token: 0x04006ED5 RID: 28373
			public List<QuestPart> questParts = new List<QuestPart>();

			// Token: 0x04006ED6 RID: 28374
			public List<Reward> rewards = new List<Reward>();
		}
	}
}
