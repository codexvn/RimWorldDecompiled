using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002537 RID: 9527
	[StaticConstructorOnStartup]
	public abstract class Thought : IExposable
	{
		// Token: 0x17001FF7 RID: 8183
		// (get) Token: 0x0600D067 RID: 53351
		public abstract int CurStageIndex { get; }

		// Token: 0x17001FF8 RID: 8184
		// (get) Token: 0x0600D068 RID: 53352 RVA: 0x003D4D82 File Offset: 0x003D2F82
		public virtual int DurationTicks
		{
			get
			{
				return this.def.DurationTicks;
			}
		}

		// Token: 0x17001FF9 RID: 8185
		// (get) Token: 0x0600D069 RID: 53353 RVA: 0x003D4D8F File Offset: 0x003D2F8F
		public ThoughtStage CurStage
		{
			get
			{
				return this.def.stages[this.CurStageIndex];
			}
		}

		// Token: 0x17001FFA RID: 8186
		// (get) Token: 0x0600D06A RID: 53354 RVA: 0x003D4DA7 File Offset: 0x003D2FA7
		public virtual bool VisibleInNeedsTab
		{
			get
			{
				return this.CurStage.visible;
			}
		}

		// Token: 0x17001FFB RID: 8187
		// (get) Token: 0x0600D06B RID: 53355 RVA: 0x003D4DB4 File Offset: 0x003D2FB4
		public virtual string LabelCap
		{
			get
			{
				if (this.def.Worker == null)
				{
					return this.CurStage.LabelCap.Formatted(this.pawn.Named("PAWN")).CapitalizeFirst();
				}
				return this.def.Worker.PostProcessLabel(this.pawn, this.CurStage.LabelCap).CapitalizeFirst();
			}
		}

		// Token: 0x17001FFC RID: 8188
		// (get) Token: 0x0600D06C RID: 53356 RVA: 0x003D4E22 File Offset: 0x003D3022
		protected virtual float BaseMoodOffset
		{
			get
			{
				return this.CurStage.baseMoodEffect;
			}
		}

		// Token: 0x17001FFD RID: 8189
		// (get) Token: 0x0600D06D RID: 53357 RVA: 0x003D4E2F File Offset: 0x003D302F
		public virtual string LabelCapSocial
		{
			get
			{
				if (this.CurStage.labelSocial != null)
				{
					return this.CurStage.LabelSocialCap.Formatted(this.pawn.Named("PAWN"));
				}
				return this.LabelCap;
			}
		}

		// Token: 0x17001FFE RID: 8190
		// (get) Token: 0x0600D06E RID: 53358 RVA: 0x003D4E6C File Offset: 0x003D306C
		public virtual string Description
		{
			get
			{
				string text = this.CurStage.description;
				if (text == null)
				{
					text = this.def.description;
				}
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessDescription(this.pawn, text);
				}
				else
				{
					Thought_Memory thought_Memory = this as Thought_Memory;
					if (thought_Memory != null && thought_Memory.otherPawn != null)
					{
						text = text.Formatted(this.pawn.Named("PAWN"), thought_Memory.otherPawn.Named("OTHERPAWN")).Resolve();
					}
					else
					{
						ISocialThought socialThought = this as ISocialThought;
						if (socialThought != null && socialThought.OtherPawn() != null)
						{
							text = text.Formatted(this.pawn.Named("PAWN"), socialThought.OtherPawn().Named("OTHERPAWN")).Resolve();
						}
						else
						{
							text = text.Formatted(this.pawn.Named("PAWN")).Resolve();
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.sourcePrecept != null && !Find.IdeoManager.classicMode)
				{
					text += this.CausedByBeliefInPrecept;
				}
				string text2 = ThoughtUtility.ThoughtNullifiedMessage(this.pawn, this.def);
				if (!string.IsNullOrEmpty(text2))
				{
					text = text + "\n\n(" + text2 + ")";
				}
				return text;
			}
		}

		// Token: 0x17001FFF RID: 8191
		// (get) Token: 0x0600D06F RID: 53359 RVA: 0x003D4FBC File Offset: 0x003D31BC
		public virtual string BabyTalk
		{
			get
			{
				string result;
				if ((result = this.cachedBabyText) == null)
				{
					result = (this.cachedBabyText = ThoughtUtility.GenerateBabyTalk(this.Description.Split('\n', StringSplitOptions.None)[0]));
				}
				return result;
			}
		}

		// Token: 0x17002000 RID: 8192
		// (get) Token: 0x0600D070 RID: 53360 RVA: 0x003D4FF4 File Offset: 0x003D31F4
		public string CausedByBeliefInPrecept
		{
			get
			{
				return "\n\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + this.sourcePrecept.ideo.name.Colorize(this.sourcePrecept.ideo.TextColor) + (this.sourcePrecept.def.label.NullOrEmpty() ? string.Empty : string.Concat(new string[]
				{
					" (",
					this.sourcePrecept.def.issue.LabelCap.ToString(),
					": ",
					this.sourcePrecept.def.LabelCap.ToString(),
					")"
				}));
			}
		}

		// Token: 0x17002001 RID: 8193
		// (get) Token: 0x0600D071 RID: 53361 RVA: 0x003D50F4 File Offset: 0x003D32F4
		public Texture2D Icon
		{
			get
			{
				if (this.def.Icon != null)
				{
					return this.def.Icon;
				}
				if (this.MoodOffset() > 0f)
				{
					return Thought.DefaultGoodIcon;
				}
				return Thought.DefaultBadIcon;
			}
		}

		// Token: 0x0600D072 RID: 53362 RVA: 0x003D5130 File Offset: 0x003D3330
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				Precept precept = this.sourcePrecept;
				if (((precept != null) ? precept.ideo : null) != null && !Find.IdeoManager.IdeosListForReading.Contains(this.sourcePrecept.ideo))
				{
					this.sourcePrecept = null;
				}
			}
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Precept>(ref this.sourcePrecept, "sourcePrecept", false);
		}

		// Token: 0x0600D073 RID: 53363 RVA: 0x003D51A0 File Offset: 0x003D33A0
		public virtual float MoodOffset()
		{
			if (this.CurStage == null)
			{
				string str = "CurStage is null while ShouldDiscard is false on ";
				string defName = this.def.defName;
				string str2 = " for ";
				Pawn pawn = this.pawn;
				Log.Error(str + defName + str2 + ((pawn != null) ? pawn.ToString() : null));
				return 0f;
			}
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			float num = this.BaseMoodOffset;
			if (this.def.effectMultiplyingStat != null)
			{
				if (this.def.effectMultiplyingStatCurve != null)
				{
					num *= this.def.effectMultiplyingStatCurve.Evaluate(this.pawn.GetStatValue(this.def.effectMultiplyingStat, true, -1));
				}
				else
				{
					num *= this.pawn.GetStatValue(this.def.effectMultiplyingStat, true, -1);
				}
			}
			if (this.def.Worker != null)
			{
				num *= this.def.Worker.MoodMultiplier(this.pawn);
			}
			return num;
		}

		// Token: 0x0600D074 RID: 53364 RVA: 0x003D5295 File Offset: 0x003D3495
		public virtual bool GroupsWith(Thought other)
		{
			return this.def == other.def && (this.CurStageIndex == other.CurStageIndex || this.def.stagesStack);
		}

		// Token: 0x0600D075 RID: 53365 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Init()
		{
		}

		// Token: 0x0600D076 RID: 53366 RVA: 0x003D52C2 File Offset: 0x003D34C2
		public override string ToString()
		{
			return "(" + this.def.defName + ")";
		}

		// Token: 0x04008DC9 RID: 36297
		public Pawn pawn;

		// Token: 0x04008DCA RID: 36298
		public ThoughtDef def;

		// Token: 0x04008DCB RID: 36299
		public Precept sourcePrecept;

		// Token: 0x04008DCC RID: 36300
		[Unsaved(false)]
		public float cachedMoodOffsetOfGroup;

		// Token: 0x04008DCD RID: 36301
		[Unsaved(false)]
		private string cachedBabyText;

		// Token: 0x04008DCE RID: 36302
		private static readonly Texture2D DefaultGoodIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericGood", true);

		// Token: 0x04008DCF RID: 36303
		private static readonly Texture2D DefaultBadIcon = ContentFinder<Texture2D>.Get("Things/Mote/ThoughtSymbol/GenericBad", true);
	}
}
