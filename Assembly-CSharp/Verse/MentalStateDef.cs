using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020002AB RID: 683
	public class MentalStateDef : Def
	{
		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x0004F537 File Offset: 0x0004D737
		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x0004F577 File Offset: 0x0004D777
		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060010D7 RID: 4311 RVA: 0x0004F584 File Offset: 0x0004D784
		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[i].mentalState == this)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x0004F5C9 File Offset: 0x0004D7C9
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		// Token: 0x04000E2D RID: 3629
		public Type stateClass = typeof(MentalState);

		// Token: 0x04000E2E RID: 3630
		public Type workerClass = typeof(MentalStateWorker);

		// Token: 0x04000E2F RID: 3631
		public MentalStateCategory category;

		// Token: 0x04000E30 RID: 3632
		public bool prisonersCanDo = true;

		// Token: 0x04000E31 RID: 3633
		public bool slavesCanDo = true;

		// Token: 0x04000E32 RID: 3634
		public bool inCaravanCanDo;

		// Token: 0x04000E33 RID: 3635
		public bool colonistsOnly;

		// Token: 0x04000E34 RID: 3636
		public bool slavesOnly;

		// Token: 0x04000E35 RID: 3637
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x04000E36 RID: 3638
		public bool downedCanDo;

		// Token: 0x04000E37 RID: 3639
		public bool unspawnedNotInCaravanCanDo;

		// Token: 0x04000E38 RID: 3640
		public bool blockNormalThoughts;

		// Token: 0x04000E39 RID: 3641
		public bool stopsJobs = true;

		// Token: 0x04000E3A RID: 3642
		public List<InteractionDef> blockInteractionInitiationExcept;

		// Token: 0x04000E3B RID: 3643
		public List<InteractionDef> blockInteractionRecipientExcept;

		// Token: 0x04000E3C RID: 3644
		public bool blockRandomInteraction;

		// Token: 0x04000E3D RID: 3645
		public EffecterDef stateEffecter;

		// Token: 0x04000E3E RID: 3646
		public TaleDef tale;

		// Token: 0x04000E3F RID: 3647
		public bool allowBeatfire;

		// Token: 0x04000E40 RID: 3648
		public DrugCategory drugCategory = DrugCategory.Any;

		// Token: 0x04000E41 RID: 3649
		public bool ignoreDrugPolicy;

		// Token: 0x04000E42 RID: 3650
		public IntRange ticksBetweenRamblingRange;

		// Token: 0x04000E43 RID: 3651
		public float recoveryMtbDays = 1f;

		// Token: 0x04000E44 RID: 3652
		public int minTicksBeforeRecovery = 500;

		// Token: 0x04000E45 RID: 3653
		public int maxTicksBeforeRecovery = 99999999;

		// Token: 0x04000E46 RID: 3654
		public bool recoverFromSleep;

		// Token: 0x04000E47 RID: 3655
		public bool recoverFromDowned = true;

		// Token: 0x04000E48 RID: 3656
		public bool recoverFromCaptured = true;

		// Token: 0x04000E49 RID: 3657
		public bool recoverFromCollapsingExhausted = true;

		// Token: 0x04000E4A RID: 3658
		public ThoughtDef moodRecoveryThought;

		// Token: 0x04000E4B RID: 3659
		public bool allowGuilty = true;

		// Token: 0x04000E4C RID: 3660
		[MustTranslate]
		public string beginLetter;

		// Token: 0x04000E4D RID: 3661
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x04000E4E RID: 3662
		public LetterDef beginLetterDef;

		// Token: 0x04000E4F RID: 3663
		public Color nameColor = Color.green;

		// Token: 0x04000E50 RID: 3664
		[MustTranslate]
		public string recoveryMessage;

		// Token: 0x04000E51 RID: 3665
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x04000E52 RID: 3666
		public bool escapingPrisonersIgnore;

		// Token: 0x04000E53 RID: 3667
		public bool blocksDefendAndExpandHive;

		// Token: 0x04000E54 RID: 3668
		private MentalStateWorker workerInt;
	}
}
