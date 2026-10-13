using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200367A RID: 13946
	public abstract class Reward : IExposable
	{
		// Token: 0x170031D7 RID: 12759
		// (get) Token: 0x060135A7 RID: 79271 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool MakesUseOfChosenPawnSignal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170031D8 RID: 12760
		// (get) Token: 0x060135A8 RID: 79272 RVA: 0x005C2B46 File Offset: 0x005C0D46
		public virtual IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				return Enumerable.Empty<GenUI.AnonymousStackElement>();
			}
		}

		// Token: 0x170031D9 RID: 12761
		// (get) Token: 0x060135A9 RID: 79273 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float TotalMarketValue
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060135AA RID: 79274
		public abstract void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed);

		// Token: 0x060135AB RID: 79275
		public abstract IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules);

		// Token: 0x060135AC RID: 79276
		public abstract string GetDescription(RewardsGeneratorParams parms);

		// Token: 0x060135AD RID: 79277 RVA: 0x005C2B4D File Offset: 0x005C0D4D
		public virtual void Notify_Used()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x060135AE RID: 79278 RVA: 0x005C2B4D File Offset: 0x005C0D4D
		public virtual void Notify_PreCleanup()
		{
			this.usedOrCleanedUp = true;
		}

		// Token: 0x060135AF RID: 79279 RVA: 0x005C2B56 File Offset: 0x005C0D56
		public virtual void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.usedOrCleanedUp, "usedOrCleanedUp", false, false);
		}

		// Token: 0x0400D6BC RID: 54972
		protected bool usedOrCleanedUp;
	}
}
