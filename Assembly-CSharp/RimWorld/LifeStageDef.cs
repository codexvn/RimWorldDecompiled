using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A84 RID: 6788
	public class LifeStageDef : Def
	{
		// Token: 0x17001749 RID: 5961
		// (get) Token: 0x0600983C RID: 38972 RVA: 0x002C691B File Offset: 0x002C4B1B
		public string Adjective
		{
			get
			{
				return this.adjective ?? this.label;
			}
		}

		// Token: 0x1700174A RID: 5962
		// (get) Token: 0x0600983D RID: 38973 RVA: 0x002C692D File Offset: 0x002C4B2D
		public LifeStageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LifeStageWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0600983E RID: 38974 RVA: 0x002C695F File Offset: 0x002C4B5F
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (!this.icon.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				});
			}
		}

		// Token: 0x04006613 RID: 26131
		[MustTranslate]
		private string adjective;

		// Token: 0x04006614 RID: 26132
		public bool visible = true;

		// Token: 0x04006615 RID: 26133
		public bool reproductive;

		// Token: 0x04006616 RID: 26134
		public bool milkable;

		// Token: 0x04006617 RID: 26135
		public bool shearable;

		// Token: 0x04006618 RID: 26136
		public bool caravanRideable;

		// Token: 0x04006619 RID: 26137
		public bool alwaysDowned;

		// Token: 0x0400661A RID: 26138
		public bool claimable;

		// Token: 0x0400661B RID: 26139
		public DevelopmentalStage developmentalStage = DevelopmentalStage.Adult;

		// Token: 0x0400661C RID: 26140
		public Type workerClass = typeof(LifeStageWorker);

		// Token: 0x0400661D RID: 26141
		public bool involuntarySleepIsNegativeEvent = true;

		// Token: 0x0400661E RID: 26142
		public ThinkTreeDef thinkTreeMainOverride;

		// Token: 0x0400661F RID: 26143
		public ThinkTreeDef thinkTreeConstantOverride;

		// Token: 0x04006620 RID: 26144
		public bool canDoRandomMentalBreaks = true;

		// Token: 0x04006621 RID: 26145
		[MustTranslate]
		public string customMoodTipString;

		// Token: 0x04006622 RID: 26146
		public bool canSleepWhileHeld;

		// Token: 0x04006623 RID: 26147
		public bool canVoluntarilySleep = true;

		// Token: 0x04006624 RID: 26148
		public bool canSleepWhenStarving = true;

		// Token: 0x04006625 RID: 26149
		public bool canInitiateSocialInteraction = true;

		// Token: 0x04006626 RID: 26150
		public float equipmentDrawDistanceFactor = 1f;

		// Token: 0x04006627 RID: 26151
		public float soundAttackChance = 0.25f;

		// Token: 0x04006628 RID: 26152
		public float voxPitch = 1f;

		// Token: 0x04006629 RID: 26153
		public float voxVolume = 1f;

		// Token: 0x0400662A RID: 26154
		[NoTranslate]
		public string icon;

		// Token: 0x0400662B RID: 26155
		[Unsaved(false)]
		public Texture2D iconTex;

		// Token: 0x0400662C RID: 26156
		public GraphicData silhouetteGraphicData;

		// Token: 0x0400662D RID: 26157
		public List<StatModifier> statOffsets = new List<StatModifier>();

		// Token: 0x0400662E RID: 26158
		public List<StatModifier> statFactors = new List<StatModifier>();

		// Token: 0x0400662F RID: 26159
		public float bodySizeFactor = 1f;

		// Token: 0x04006630 RID: 26160
		public float healthScaleFactor = 1f;

		// Token: 0x04006631 RID: 26161
		public float hungerRateFactor = 1f;

		// Token: 0x04006632 RID: 26162
		public float marketValueFactor = 1f;

		// Token: 0x04006633 RID: 26163
		public float foodMaxFactor = 1f;

		// Token: 0x04006634 RID: 26164
		public float attachPointScaleFactor = 1f;

		// Token: 0x04006635 RID: 26165
		public float meleeDamageFactor = 1f;

		// Token: 0x04006636 RID: 26166
		public SimpleCurve involuntarySleepMTBDaysFromRest;

		// Token: 0x04006637 RID: 26167
		public float? fallAsleepMaxThresholdOverride;

		// Token: 0x04006638 RID: 26168
		public float? naturalWakeThresholdOverride;

		// Token: 0x04006639 RID: 26169
		public float? bodyWidth;

		// Token: 0x0400663A RID: 26170
		public Vector3 bodyDrawOffset = Vector3.zero;

		// Token: 0x0400663B RID: 26171
		public float? headSizeFactor;

		// Token: 0x0400663C RID: 26172
		public float? eyeSizeFactor;

		// Token: 0x0400663D RID: 26173
		public float? sittingOffset;

		// Token: 0x0400663E RID: 26174
		public TagFilter hairStyleFilter;

		// Token: 0x0400663F RID: 26175
		[Unsaved(false)]
		private LifeStageWorker workerInt;
	}
}
