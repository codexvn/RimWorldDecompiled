using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002FBF RID: 12223
	[StaticConstructorOnStartup]
	public abstract class Alert
	{
		// Token: 0x17002CA0 RID: 11424
		// (get) Token: 0x06011399 RID: 70553 RVA: 0x004EF90F File Offset: 0x004EDB0F
		public virtual AlertPriority Priority
		{
			get
			{
				return this.defaultPriority;
			}
		}

		// Token: 0x17002CA1 RID: 11425
		// (get) Token: 0x0601139A RID: 70554 RVA: 0x004EF917 File Offset: 0x004EDB17
		protected virtual Color BGColor
		{
			get
			{
				return Color.clear;
			}
		}

		// Token: 0x17002CA2 RID: 11426
		// (get) Token: 0x0601139B RID: 70555 RVA: 0x004EF91E File Offset: 0x004EDB1E
		public bool Active
		{
			get
			{
				return this.cachedActive;
			}
		}

		// Token: 0x17002CA3 RID: 11427
		// (get) Token: 0x0601139C RID: 70556 RVA: 0x004EF926 File Offset: 0x004EDB26
		public string Label
		{
			get
			{
				if (!this.Active)
				{
					return string.Empty;
				}
				return this.cachedLabel;
			}
		}

		// Token: 0x17002CA4 RID: 11428
		// (get) Token: 0x0601139D RID: 70557 RVA: 0x004EF93C File Offset: 0x004EDB3C
		public float Height
		{
			get
			{
				Text.Font = GameFont.Small;
				return Text.CalcHeight(this.Label, 148f);
			}
		}

		// Token: 0x17002CA5 RID: 11429
		// (get) Token: 0x0601139E RID: 70558 RVA: 0x004EF954 File Offset: 0x004EDB54
		public bool EnabledWithActiveExpansions
		{
			get
			{
				return (!this.requireRoyalty || ModsConfig.RoyaltyActive) && (!this.requireIdeology || ModsConfig.IdeologyActive) && (!this.requireBiotech || ModsConfig.BiotechActive) && (!this.requireAnomaly || ModsConfig.AnomalyActive);
			}
		}

		// Token: 0x0601139F RID: 70559
		public abstract AlertReport GetReport();

		// Token: 0x060113A0 RID: 70560 RVA: 0x004EF9A6 File Offset: 0x004EDBA6
		public virtual TaggedString GetExplanation()
		{
			return this.defaultExplanation;
		}

		// Token: 0x060113A1 RID: 70561 RVA: 0x004EF9B3 File Offset: 0x004EDBB3
		public virtual string GetLabel()
		{
			return this.defaultLabel;
		}

		// Token: 0x17002CA6 RID: 11430
		// (get) Token: 0x060113A2 RID: 70562 RVA: 0x004EF9BB File Offset: 0x004EDBBB
		public virtual string GetJumpToTargetsText
		{
			get
			{
				return "ClickToJumpToProblem".Translate();
			}
		}

		// Token: 0x060113A3 RID: 70563 RVA: 0x004EF9CC File Offset: 0x004EDBCC
		public void Notify_Started()
		{
			if (this.Priority >= AlertPriority.High)
			{
				if (this.alertBounce == null)
				{
					this.alertBounce = new AlertBounce();
				}
				this.alertBounce.DoAlertStartEffect();
				if (Time.timeSinceLevelLoad > 1f && Time.realtimeSinceStartup > this.lastBellTime + 0.5f)
				{
					SoundDefOf.TinyBell.PlayOneShotOnCamera(null);
					this.lastBellTime = Time.realtimeSinceStartup;
				}
			}
		}

		// Token: 0x060113A4 RID: 70564 RVA: 0x004EFA38 File Offset: 0x004EDC38
		public void Recalculate()
		{
			AlertReport report = this.GetReport();
			this.cachedActive = report.active;
			if (report.active)
			{
				this.cachedLabel = this.GetLabel();
			}
		}

		// Token: 0x060113A5 RID: 70565 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void AlertActiveUpdate()
		{
		}

		// Token: 0x060113A6 RID: 70566 RVA: 0x004EFA6C File Offset: 0x004EDC6C
		public virtual Rect DrawAt(float topY, bool minimized)
		{
			Rect rect = new Rect((float)UI.screenWidth - 154f, topY, 154f, this.Height);
			if (this.alertBounce != null)
			{
				rect.x -= this.alertBounce.CalculateHorizontalOffset();
			}
			GUI.color = this.BGColor;
			GUI.DrawTexture(rect, Alert.AlertBGTex);
			GUI.color = Color.white;
			Widgets.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(new Rect(0f, 0f, 148f, this.Height), this.Label);
			Widgets.EndGroup();
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, Alert.AlertBGTexHighlight);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.OnClick();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return rect;
		}

		// Token: 0x060113A7 RID: 70567 RVA: 0x004EFB38 File Offset: 0x004EDD38
		protected virtual void OnClick()
		{
			IEnumerable<GlobalTargetInfo> allCulprits = this.GetReport().AllCulprits;
			if (allCulprits != null)
			{
				Alert.tmpTargets.Clear();
				foreach (GlobalTargetInfo item in allCulprits)
				{
					if (item.IsValid)
					{
						Alert.tmpTargets.Add(item);
					}
				}
				if (Alert.tmpTargets.Any<GlobalTargetInfo>())
				{
					if (Event.current.button == 1)
					{
						this.jumpToTargetCycleIndex--;
					}
					else
					{
						this.jumpToTargetCycleIndex++;
					}
					CameraJumper.TryJumpAndSelect(Alert.tmpTargets[GenMath.PositiveMod(this.jumpToTargetCycleIndex, Alert.tmpTargets.Count)], CameraJumper.MovementMode.Pan);
					Alert.tmpTargets.Clear();
				}
			}
		}

		// Token: 0x060113A8 RID: 70568 RVA: 0x004EFC14 File Offset: 0x004EDE14
		public void DrawInfoPane()
		{
			Alert.<>c__DisplayClass40_0 CS$<>8__locals1 = new Alert.<>c__DisplayClass40_0();
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			this.Recalculate();
			if (!this.Active)
			{
				return;
			}
			CS$<>8__locals1.expString = this.GetExplanation();
			if (CS$<>8__locals1.expString.NullOrEmpty())
			{
				return;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			if (this.GetReport().AnyCulpritValid)
			{
				CS$<>8__locals1.expString += "\n\n(" + this.GetJumpToTargetsText + ")";
			}
			float num = Text.CalcHeight(CS$<>8__locals1.expString, 310f);
			num += 20f;
			CS$<>8__locals1.infoRect = new Rect((float)UI.screenWidth - 154f - 330f - 8f, Mathf.Max(Mathf.Min(Event.current.mousePosition.y, (float)UI.screenHeight - num), 0f), 330f, num);
			if (CS$<>8__locals1.infoRect.yMax > (float)UI.screenHeight)
			{
				Alert.<>c__DisplayClass40_0 CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals2.infoRect.y = CS$<>8__locals2.infoRect.y - ((float)UI.screenHeight - CS$<>8__locals1.infoRect.yMax);
			}
			if (CS$<>8__locals1.infoRect.y < 0f)
			{
				CS$<>8__locals1.infoRect.y = 0f;
			}
			Find.WindowStack.ImmediateWindow(138956, CS$<>8__locals1.infoRect, WindowLayer.Super, delegate
			{
				Text.Font = GameFont.Small;
				Rect rect = CS$<>8__locals1.infoRect.AtZero();
				Widgets.DrawWindowBackground(rect);
				Rect rect2 = rect.ContractedBy(10f);
				Widgets.BeginGroup(rect2);
				Widgets.Label(new Rect(0f, 0f, rect2.width, rect2.height), CS$<>8__locals1.expString);
				Widgets.EndGroup();
			}, false, false, 1f, null, false);
		}

		// Token: 0x0400B2C4 RID: 45764
		protected AlertPriority defaultPriority;

		// Token: 0x0400B2C5 RID: 45765
		protected string defaultLabel;

		// Token: 0x0400B2C6 RID: 45766
		protected string defaultExplanation;

		// Token: 0x0400B2C7 RID: 45767
		protected bool requireRoyalty;

		// Token: 0x0400B2C8 RID: 45768
		protected bool requireIdeology;

		// Token: 0x0400B2C9 RID: 45769
		protected bool requireBiotech;

		// Token: 0x0400B2CA RID: 45770
		protected bool requireAnomaly;

		// Token: 0x0400B2CB RID: 45771
		protected float lastBellTime = -1000f;

		// Token: 0x0400B2CC RID: 45772
		private int jumpToTargetCycleIndex = -1;

		// Token: 0x0400B2CD RID: 45773
		private bool cachedActive;

		// Token: 0x0400B2CE RID: 45774
		private string cachedLabel;

		// Token: 0x0400B2CF RID: 45775
		private AlertBounce alertBounce;

		// Token: 0x0400B2D0 RID: 45776
		public const float Width = 154f;

		// Token: 0x0400B2D1 RID: 45777
		private const float TextWidth = 148f;

		// Token: 0x0400B2D2 RID: 45778
		public const float InfoRectWidth = 330f;

		// Token: 0x0400B2D3 RID: 45779
		private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x0400B2D4 RID: 45780
		private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

		// Token: 0x0400B2D5 RID: 45781
		private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();
	}
}
