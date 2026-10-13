using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200246C RID: 9324
	[StaticConstructorOnStartup]
	public abstract class Need : IExposable
	{
		// Token: 0x17001E93 RID: 7827
		// (get) Token: 0x0600CA42 RID: 51778 RVA: 0x003B3AF5 File Offset: 0x003B1CF5
		public string LabelCap
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17001E94 RID: 7828
		// (get) Token: 0x0600CA43 RID: 51779 RVA: 0x003B3B07 File Offset: 0x003B1D07
		public float CurInstantLevelPercentage
		{
			get
			{
				return this.CurInstantLevel / this.MaxLevel;
			}
		}

		// Token: 0x17001E95 RID: 7829
		// (get) Token: 0x0600CA44 RID: 51780 RVA: 0x00002501 File Offset: 0x00000701
		public virtual int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001E96 RID: 7830
		// (get) Token: 0x0600CA45 RID: 51781 RVA: 0x000B695A File Offset: 0x000B4B5A
		public virtual float CurInstantLevel
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x17001E97 RID: 7831
		// (get) Token: 0x0600CA46 RID: 51782 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float MaxLevel
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001E98 RID: 7832
		// (get) Token: 0x0600CA47 RID: 51783 RVA: 0x003B3B16 File Offset: 0x003B1D16
		// (set) Token: 0x0600CA48 RID: 51784 RVA: 0x003B3B1E File Offset: 0x003B1D1E
		public virtual float CurLevel
		{
			get
			{
				return this.curLevelInt;
			}
			set
			{
				this.curLevelInt = Mathf.Clamp(value, 0f, this.MaxLevel);
			}
		}

		// Token: 0x17001E99 RID: 7833
		// (get) Token: 0x0600CA49 RID: 51785 RVA: 0x003B3B37 File Offset: 0x003B1D37
		// (set) Token: 0x0600CA4A RID: 51786 RVA: 0x003B3B46 File Offset: 0x003B1D46
		public float CurLevelPercentage
		{
			get
			{
				return this.CurLevel / this.MaxLevel;
			}
			set
			{
				this.CurLevel = value * this.MaxLevel;
			}
		}

		// Token: 0x17001E9A RID: 7834
		// (get) Token: 0x0600CA4B RID: 51787 RVA: 0x003B3B58 File Offset: 0x003B1D58
		protected virtual bool IsFrozen
		{
			get
			{
				return this.pawn.Suspended || (this.def.freezeWhileSleeping && !this.pawn.Awake()) || (this.def.freezeInMentalState && this.pawn.InMentalState) || this.NeedFrozenFromDormanancy() || !this.IsPawnInteractableOrVisible;
			}
		}

		// Token: 0x17001E9B RID: 7835
		// (get) Token: 0x0600CA4C RID: 51788 RVA: 0x003B3BBF File Offset: 0x003B1DBF
		private bool IsPawnInteractableOrVisible
		{
			get
			{
				return this.pawn.SpawnedOrAnyParentSpawned || this.pawn.IsCaravanMember() || PawnUtility.IsTravelingInTransportPodWorldObject(this.pawn);
			}
		}

		// Token: 0x0600CA4D RID: 51789 RVA: 0x003B3BEF File Offset: 0x003B1DEF
		private bool NeedFrozenFromDormanancy()
		{
			return this.intDormant != null && !this.intDormant.Awake && this.intDormant.Props.freezeNeeds.Contains(this.def);
		}

		// Token: 0x17001E9C RID: 7836
		// (get) Token: 0x0600CA4E RID: 51790 RVA: 0x003B3C25 File Offset: 0x003B1E25
		public virtual bool ShowOnNeedList
		{
			get
			{
				return this.def.showOnNeedList;
			}
		}

		// Token: 0x0600CA4F RID: 51791 RVA: 0x003B3C32 File Offset: 0x003B1E32
		public Need(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.SetInitialLevel();
			this.intDormant = this.pawn.TryGetComp<CompCanBeDormant>();
		}

		// Token: 0x0600CA50 RID: 51792 RVA: 0x003B3C58 File Offset: 0x003B1E58
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<NeedDef>(ref this.def, "def");
			Scribe_Values.Look<float>(ref this.curLevelInt, "curLevel", 0f, false);
		}

		// Token: 0x0600CA51 RID: 51793
		public abstract void NeedInterval();

		// Token: 0x0600CA52 RID: 51794 RVA: 0x003B3C80 File Offset: 0x003B1E80
		public virtual string GetTipString()
		{
			string text = (this.LabelCap + ": " + this.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor) + "\n" + this.def.description;
			Pawn_StoryTracker story = this.pawn.story;
			Trait trait;
			Gene gene;
			Hediff hediff;
			if (((story != null) ? story.traits : null) != null && this.pawn.story.traits.TryGetNeedEnablingTrait(this.def, out trait))
			{
				text += string.Format("\n\n{0}: {1}", "ComesFromTrait".Translate(), trait.LabelCap);
			}
			else if (this.pawn.genes != null && this.pawn.genes.TryGetNeedEnablingGene(this.def, out gene))
			{
				text += string.Format("\n\n{0}: {1}", "ComesFromGene".Translate(), gene.LabelCap);
			}
			else if (this.pawn.Ideo != null && this.pawn.Ideo.EnablesNeed(this.def))
			{
				text += string.Format("\n\n{0}: {1}", "ComesFromIdeo".Translate(), this.pawn.Ideo.name.CapitalizeFirst());
			}
			else if (this.pawn.health != null && this.pawn.health.hediffSet.TryGetNeedEnablingHediff(this.def, out hediff))
			{
				text += string.Format("\n\n{0}: {1}", "ComesFromHediff".Translate(), hediff.LabelCap);
			}
			return text;
		}

		// Token: 0x0600CA53 RID: 51795 RVA: 0x003B3E2C File Offset: 0x003B202C
		public virtual void SetInitialLevel()
		{
			this.CurLevelPercentage = 0.5f;
		}

		// Token: 0x0600CA54 RID: 51796 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnNeedRemoved()
		{
		}

		// Token: 0x0600CA55 RID: 51797 RVA: 0x003B3E39 File Offset: 0x003B2039
		protected virtual void OffsetDebugPercent(float offsetPercent)
		{
			this.CurLevelPercentage += offsetPercent;
		}

		// Token: 0x0600CA56 RID: 51798 RVA: 0x003B3E4C File Offset: 0x003B204C
		public virtual void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			if (rect.height > 70f)
			{
				float num = (rect.height - 70f) / 2f;
				rect.height = 70f;
				rect.y += num;
			}
			Rect rect2 = rectForTooltip ?? rect;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			if (doTooltip && Mouse.IsOver(rect2))
			{
				TooltipHandler.TipRegion(rect2, new TipSignal(() => this.GetTipString(), rect2.GetHashCode()));
			}
			float num2 = 14f;
			float num3 = (customMargin >= 0f) ? customMargin : (num2 + 15f);
			if (rect.height < 50f)
			{
				num2 *= Mathf.InverseLerp(0f, 50f, rect.height);
			}
			if (drawLabel)
			{
				Text.Font = ((rect.height > 55f) ? GameFont.Small : GameFont.Tiny);
				Text.Anchor = TextAnchor.LowerLeft;
				Widgets.Label(new Rect(rect.x + num3 + rect.width * 0.1f, rect.y, rect.width - num3 - rect.width * 0.1f, rect.height / 2f), this.LabelCap);
				Text.Anchor = TextAnchor.UpperLeft;
			}
			Rect rect3 = rect;
			if (drawLabel)
			{
				rect3.y += rect.height / 2f;
				rect3.height -= rect.height / 2f;
			}
			rect3 = new Rect(rect3.x + num3, rect3.y, rect3.width - num3 * 2f, rect3.height - num2);
			if (DebugSettings.ShowDevGizmos)
			{
				float lineHeight = Text.LineHeight;
				Rect rect4 = new Rect(rect3.xMax - lineHeight, rect3.y - lineHeight, lineHeight, lineHeight);
				if (Widgets.ButtonImage(rect4.ContractedBy(4f), TexButton.Plus, true, null))
				{
					this.OffsetDebugPercent(0.1f);
				}
				if (Mouse.IsOver(rect4))
				{
					TooltipHandler.TipRegion(rect4, "+ 10%");
				}
				Rect rect5 = new Rect(rect4.xMin - lineHeight, rect3.y - lineHeight, lineHeight, lineHeight);
				if (Widgets.ButtonImage(rect5.ContractedBy(4f), TexButton.Minus, true, null))
				{
					this.OffsetDebugPercent(-0.1f);
				}
				if (Mouse.IsOver(rect5))
				{
					TooltipHandler.TipRegion(rect5, "- 10%");
				}
			}
			Rect rect6 = rect3;
			float num4 = 1f;
			if (this.def.scaleBar && this.MaxLevel < 1f)
			{
				num4 = this.MaxLevel;
			}
			rect6.width *= num4;
			Rect barRect = Widgets.FillableBar(rect6, this.CurLevelPercentage);
			if (drawArrows)
			{
				Widgets.FillableBarChangeArrows(rect6, this.GUIChangeArrow);
			}
			if (this.threshPercents != null)
			{
				for (int i = 0; i < Mathf.Min(this.threshPercents.Count, maxThresholdMarkers); i++)
				{
					this.DrawBarThreshold(barRect, this.threshPercents[i] * num4);
				}
			}
			if (this.def.showUnitTicks)
			{
				int num5 = 1;
				while ((float)num5 < this.MaxLevel)
				{
					this.DrawBarDivision(barRect, (float)num5 / this.MaxLevel * num4);
					num5++;
				}
			}
			float curInstantLevelPercentage = this.CurInstantLevelPercentage;
			if (curInstantLevelPercentage >= 0f)
			{
				this.DrawBarInstantMarkerAt(rect3, curInstantLevelPercentage * num4);
			}
			if (!this.def.tutorHighlightTag.NullOrEmpty())
			{
				UIHighlighter.HighlightOpportunity(rect, this.def.tutorHighlightTag);
			}
			Text.Font = GameFont.Small;
		}

		// Token: 0x0600CA57 RID: 51799 RVA: 0x003B41FC File Offset: 0x003B23FC
		protected void DrawBarInstantMarkerAt(Rect barRect, float pct)
		{
			if (pct > 1f)
			{
				NeedDef needDef = this.def;
				Log.ErrorOnce(((needDef != null) ? needDef.ToString() : null) + " drawing bar percent > 1 : " + pct.ToString(), 6932178);
			}
			float num = 12f;
			if (barRect.width < 150f)
			{
				num /= 2f;
			}
			Vector2 vector = new Vector2(barRect.x + barRect.width * pct, barRect.y + barRect.height);
			GUI.DrawTexture(new Rect(vector.x - num / 2f, vector.y, num, num), Need.BarInstantMarkerTex);
		}

		// Token: 0x0600CA58 RID: 51800 RVA: 0x003B42A8 File Offset: 0x003B24A8
		protected void DrawBarThreshold(Rect barRect, float threshPct)
		{
			float num = (float)((barRect.width > 60f) ? 2 : 1);
			Rect position = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y + barRect.height / 2f, num, barRect.height / 2f);
			Texture2D image;
			if (threshPct < this.CurLevelPercentage)
			{
				image = BaseContent.BlackTex;
				GUI.color = new Color(1f, 1f, 1f, 0.9f);
			}
			else
			{
				image = BaseContent.GreyTex;
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
			}
			GUI.DrawTexture(position, image);
			GUI.color = Color.white;
		}

		// Token: 0x0600CA59 RID: 51801 RVA: 0x003B4370 File Offset: 0x003B2570
		private void DrawBarDivision(Rect barRect, float threshPct)
		{
			float num = 5f;
			Rect rect = new Rect(barRect.x + barRect.width * threshPct - (num - 1f), barRect.y, num, barRect.height);
			if (threshPct < this.CurLevelPercentage)
			{
				GUI.color = new Color(0f, 0f, 0f, 0.9f);
			}
			else
			{
				GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
			Rect position = rect;
			position.yMax = position.yMin + 4f;
			GUI.DrawTextureWithTexCoords(position, Need.NeedUnitDividerTex, new Rect(0f, 0.5f, 1f, 0.5f));
			Rect position2 = rect;
			position2.yMin = position2.yMax - 4f;
			GUI.DrawTextureWithTexCoords(position2, Need.NeedUnitDividerTex, new Rect(0f, 0f, 1f, 0.5f));
			Rect position3 = rect;
			position3.yMin = position.yMax;
			position3.yMax = position2.yMin;
			if (position3.height > 0f)
			{
				GUI.DrawTextureWithTexCoords(position3, Need.NeedUnitDividerTex, new Rect(0f, 0.4f, 1f, 0.2f));
			}
			GUI.color = Color.white;
		}

		// Token: 0x0400899F RID: 35231
		public NeedDef def;

		// Token: 0x040089A0 RID: 35232
		protected readonly Pawn pawn;

		// Token: 0x040089A1 RID: 35233
		protected float curLevelInt;

		// Token: 0x040089A2 RID: 35234
		protected List<float> threshPercents;

		// Token: 0x040089A3 RID: 35235
		private CompCanBeDormant intDormant;

		// Token: 0x040089A4 RID: 35236
		public const float MaxDrawHeight = 70f;

		// Token: 0x040089A5 RID: 35237
		private static readonly Texture2D BarInstantMarkerTex = ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarker", true);

		// Token: 0x040089A6 RID: 35238
		private static readonly Texture2D NeedUnitDividerTex = ContentFinder<Texture2D>.Get("UI/Misc/NeedUnitDivider", true);

		// Token: 0x040089A7 RID: 35239
		private const float BarInstantMarkerSize = 12f;
	}
}
