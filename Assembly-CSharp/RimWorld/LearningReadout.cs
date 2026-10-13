using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003357 RID: 13143
	[StaticConstructorOnStartup]
	public class LearningReadout : IExposable
	{
		// Token: 0x17003024 RID: 12324
		// (get) Token: 0x06012788 RID: 75656 RVA: 0x0057307C File Offset: 0x0057127C
		public int ActiveConceptsCount
		{
			get
			{
				return this.activeConcepts.Count;
			}
		}

		// Token: 0x17003025 RID: 12325
		// (get) Token: 0x06012789 RID: 75657 RVA: 0x00573089 File Offset: 0x00571289
		public bool ShowAllMode
		{
			get
			{
				return this.showAllMode;
			}
		}

		// Token: 0x0601278A RID: 75658 RVA: 0x00573094 File Offset: 0x00571294
		public LearningReadout()
		{
			this.windowOnGUICached = new Action(this.WindowOnGUI);
		}

		// Token: 0x0601278B RID: 75659 RVA: 0x005730E8 File Offset: 0x005712E8
		public void ExposeData()
		{
			Scribe_Collections.Look<ConceptDef>(ref this.activeConcepts, "activeConcepts", LookMode.Undefined, Array.Empty<object>());
			Scribe_Defs.Look<ConceptDef>(ref this.selectedConcept, "selectedConcept");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.activeConcepts.RemoveAll((ConceptDef c) => PlayerKnowledgeDatabase.IsComplete(c));
			}
		}

		// Token: 0x0601278C RID: 75660 RVA: 0x0057314E File Offset: 0x0057134E
		public bool TryActivateConcept(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc))
			{
				return false;
			}
			this.activeConcepts.Add(conc);
			SoundDefOf.Lesson_Activated.PlayOneShotOnCamera(null);
			this.lastConceptActivateRealTime = RealTime.LastRealTime;
			return true;
		}

		// Token: 0x0601278D RID: 75661 RVA: 0x00573183 File Offset: 0x00571383
		public bool IsActive(ConceptDef conc)
		{
			return this.activeConcepts.Contains(conc);
		}

		// Token: 0x0601278E RID: 75662 RVA: 0x000026EA File Offset: 0x000008EA
		public void LearningReadoutUpdate()
		{
		}

		// Token: 0x0601278F RID: 75663 RVA: 0x00573194 File Offset: 0x00571394
		public void Notify_ConceptNewlyLearned(ConceptDef conc)
		{
			if (this.activeConcepts.Contains(conc) || this.selectedConcept == conc)
			{
				SoundDefOf.Lesson_Deactivated.PlayOneShotOnCamera(null);
				SoundDefOf.CommsWindow_Close.PlayOneShotOnCamera(null);
			}
			if (this.activeConcepts.Contains(conc))
			{
				this.activeConcepts.Remove(conc);
			}
			if (this.selectedConcept == conc)
			{
				this.selectedConcept = null;
			}
		}

		// Token: 0x06012790 RID: 75664 RVA: 0x005731F9 File Offset: 0x005713F9
		private string FilterSearchStringInput(string input)
		{
			if (input == this.searchString)
			{
				return input;
			}
			if (input.Length > 20)
			{
				input = input.Substring(0, 20);
			}
			return input;
		}

		// Token: 0x06012791 RID: 75665 RVA: 0x00573224 File Offset: 0x00571424
		public void LearningReadoutOnGUI()
		{
			if (TutorSystem.TutorialMode || !TutorSystem.AdaptiveTrainingEnabled)
			{
				return;
			}
			if (!Find.PlaySettings.showLearningHelper && this.activeConcepts.Count == 0)
			{
				return;
			}
			if (Find.WindowStack.IsOpen<Screen_Credits>())
			{
				return;
			}
			float b = (float)UI.screenHeight / 2f;
			float a = this.contentHeight + 14f;
			this.windowRect = new Rect((float)UI.screenWidth - 8f - 200f, 8f, 200f, Mathf.Min(a, b));
			Rect rect = this.windowRect;
			Find.WindowStack.ImmediateWindow(76136312, this.windowRect, WindowLayer.Super, this.windowOnGUICached, false, false, 1f, null, false);
			float num = Time.realtimeSinceStartup - this.lastConceptActivateRealTime;
			if (num < 1f && num > 0f)
			{
				GenUI.DrawFlash(rect.x, rect.center.y, (float)UI.screenWidth * 0.6f, Pulser.PulseBrightness(1f, 1f, num) * 0.85f, new Color(0.8f, 0.77f, 0.53f));
			}
			ConceptDef conceptDef = (this.selectedConcept != null) ? this.selectedConcept : this.mouseoverConcept;
			if (conceptDef != null)
			{
				this.DrawInfoPane(conceptDef);
				conceptDef.HighlightAllTags();
			}
			this.mouseoverConcept = null;
		}

		// Token: 0x06012792 RID: 75666 RVA: 0x0057337C File Offset: 0x0057157C
		private void WindowOnGUI()
		{
			Rect rect = this.windowRect.AtZero().ContractedBy(7f);
			bool flag = this.contentHeight > rect.height;
			Widgets.DrawWindowBackgroundTutor(this.windowRect.AtZero());
			float num = rect.y;
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(rect.x, num, rect.width - 24f, 24f);
			Widgets.Label(rect2, "LearningHelper".Translate());
			num += rect2.height;
			if (Widgets.ButtonImage(new Rect(rect2.xMax, rect2.y, 24f, 24f), (!this.showAllMode) ? TexButton.Plus : TexButton.Minus, true, null))
			{
				this.showAllMode = !this.showAllMode;
				if (this.showAllMode)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			if (this.showAllMode)
			{
				Rect rect3 = new Rect(rect.x, num, rect.width - 20f - 2f, 28f);
				this.searchString = this.FilterSearchStringInput(Widgets.TextField(rect3, this.searchString));
				if (this.searchString == "")
				{
					GUI.color = new Color(0.6f, 0.6f, 0.6f, 1f);
					Text.Anchor = TextAnchor.MiddleLeft;
					Rect rect4 = rect3;
					rect4.xMin += 7f;
					Widgets.Label(rect4, "Filter".Translate() + "...");
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
				}
				if (Widgets.ButtonImage(new Rect(rect3.xMax + 4f, num + 14f - 10f, 20f, 20f), TexButton.CloseXSmall, true, null))
				{
					this.searchString = "";
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				}
				num += 32f;
			}
			LearningReadout.tmpConceptsToShow.Clear();
			if (this.showAllMode)
			{
				LearningReadout.tmpConceptsToShow.AddRange(DefDatabase<ConceptDef>.AllDefsListForReading);
			}
			else
			{
				LearningReadout.tmpConceptsToShow.AddRange(this.activeConcepts);
			}
			if (LearningReadout.tmpConceptsToShow.Any<ConceptDef>())
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Widgets.DrawLineHorizontal(rect.x, num, rect.width);
				GUI.color = Color.white;
				num += 4f;
			}
			float num2 = num - rect.y;
			rect.yMin = num;
			Rect viewRect = rect.AtZero();
			if (flag)
			{
				viewRect.height = this.contentHeight - num2;
				viewRect.width -= 20f;
				Widgets.BeginScrollView(rect, ref this.scrollPosition, viewRect, true);
			}
			else
			{
				Widgets.BeginGroup(rect);
			}
			num = 0f;
			if (this.showAllMode)
			{
				LearningReadout.tmpConceptsToShow.SortBy((ConceptDef x) => -this.DisplayPriority(x), (ConceptDef x) => x.label);
			}
			for (int i = 0; i < LearningReadout.tmpConceptsToShow.Count; i++)
			{
				if (!LearningReadout.tmpConceptsToShow[i].TriggeredDirect)
				{
					num = this.DrawConceptListRow(0f, num, viewRect.width, LearningReadout.tmpConceptsToShow[i]).yMax;
				}
			}
			LearningReadout.tmpConceptsToShow.Clear();
			this.contentHeight = num2 + num;
			if (flag)
			{
				Widgets.EndScrollView();
				return;
			}
			Widgets.EndGroup();
		}

		// Token: 0x06012793 RID: 75667 RVA: 0x00573720 File Offset: 0x00571920
		private int DisplayPriority(ConceptDef conc)
		{
			int num = 1;
			if (this.MatchesSearchString(conc))
			{
				num += 10000;
			}
			return num;
		}

		// Token: 0x06012794 RID: 75668 RVA: 0x00573741 File Offset: 0x00571941
		private bool MatchesSearchString(ConceptDef conc)
		{
			return this.searchString != "" && conc.label.IndexOf(this.searchString, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06012795 RID: 75669 RVA: 0x00573770 File Offset: 0x00571970
		private Rect DrawConceptListRow(float x, float y, float width, ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool flag = PlayerKnowledgeDatabase.IsComplete(conc);
			object obj = !flag && knowledge > 0f;
			float num = Text.CalcHeight(conc.LabelCap, width);
			object obj2 = obj;
			if (obj2 != null)
			{
				num += 0f;
			}
			Rect rect = new Rect(x, y, width, num);
			if (obj2 != null)
			{
				Rect rect2 = new Rect(rect);
				rect2.yMin += 1f;
				rect2.yMax -= 1f;
				Widgets.FillableBar(rect2, PlayerKnowledgeDatabase.GetKnowledge(conc), LearningReadout.ProgressBarFillTex, LearningReadout.ProgressBarBGTex, false);
			}
			if (flag)
			{
				GUI.DrawTexture(rect, BaseContent.GreyTex);
			}
			if (this.selectedConcept == conc)
			{
				GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
			}
			Widgets.DrawHighlightIfMouseover(rect);
			if (this.MatchesSearchString(conc))
			{
				Widgets.DrawHighlight(rect);
			}
			Widgets.Label(rect, conc.LabelCap);
			if (Mouse.IsOver(rect) && this.selectedConcept == null)
			{
				this.mouseoverConcept = conc;
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.selectedConcept == conc)
				{
					this.selectedConcept = null;
				}
				else
				{
					this.selectedConcept = conc;
				}
				SoundDefOf.PageChange.PlayOneShotOnCamera(null);
			}
			return rect;
		}

		// Token: 0x06012796 RID: 75670 RVA: 0x0057389C File Offset: 0x00571A9C
		private Rect DrawInfoPane(ConceptDef conc)
		{
			float knowledge = PlayerKnowledgeDatabase.GetKnowledge(conc);
			bool complete = PlayerKnowledgeDatabase.IsComplete(conc);
			bool drawProgressBar = !complete && knowledge > 0f;
			Text.Font = GameFont.Medium;
			float titleHeight = Text.CalcHeight(conc.LabelCap, 276f);
			Text.Font = GameFont.Small;
			float textHeight = Text.CalcHeight(conc.HelpTextAdjusted, 296f);
			float num = titleHeight + textHeight + 14f + 5f;
			if (this.selectedConcept == conc)
			{
				num += 40f;
			}
			if (drawProgressBar)
			{
				num += 30f;
			}
			Rect outRect = new Rect((float)UI.screenWidth - 8f - 200f - 8f - 310f, 8f, 310f, num);
			Rect outRect2 = outRect;
			Find.WindowStack.ImmediateWindow(987612111, outRect, WindowLayer.Super, delegate
			{
				outRect = outRect.AtZero();
				Rect rect = outRect.ContractedBy(7f);
				Widgets.DrawShadowAround(outRect);
				Widgets.DrawWindowBackgroundTutor(outRect);
				Rect rect2 = rect;
				rect2.width -= 20f;
				rect2.height = titleHeight + 5f;
				Text.Font = GameFont.Medium;
				Widgets.Label(rect2, conc.LabelCap);
				Text.Font = GameFont.Small;
				Rect rect3 = rect;
				rect3.yMin = rect2.yMax;
				rect3.height = textHeight;
				Widgets.Label(rect3, conc.HelpTextAdjusted);
				if (drawProgressBar)
				{
					Rect rect4 = rect;
					rect4.yMin = rect3.yMax;
					rect4.height = 30f;
					Widgets.FillableBar(rect4, PlayerKnowledgeDatabase.GetKnowledge(conc), LearningReadout.ProgressBarFillTex);
				}
				if (this.selectedConcept == conc)
				{
					if (Widgets.CloseButtonFor(outRect))
					{
						this.selectedConcept = null;
						SoundDefOf.PageChange.PlayOneShotOnCamera(null);
					}
					Rect rect5 = new Rect(rect.center.x - 70f, rect.yMax - 30f, 140f, 30f);
					if (!complete)
					{
						if (Widgets.ButtonText(rect5, "MarkLearned".Translate(), true, true, true, null))
						{
							this.selectedConcept = null;
							SoundDefOf.PageChange.PlayOneShotOnCamera(null);
							PlayerKnowledgeDatabase.SetKnowledge(conc, 1f);
							return;
						}
					}
					else
					{
						GUI.color = new Color(1f, 1f, 1f, 0.5f);
						Text.Anchor = TextAnchor.MiddleCenter;
						Widgets.Label(rect5, "AlreadyLearned".Translate());
						Text.Anchor = TextAnchor.UpperLeft;
						GUI.color = Color.white;
					}
				}
			}, false, false, 1f, null, false);
			return outRect2;
		}

		// Token: 0x0400BFE1 RID: 49121
		private List<ConceptDef> activeConcepts = new List<ConceptDef>();

		// Token: 0x0400BFE2 RID: 49122
		private ConceptDef selectedConcept;

		// Token: 0x0400BFE3 RID: 49123
		private bool showAllMode;

		// Token: 0x0400BFE4 RID: 49124
		private float contentHeight;

		// Token: 0x0400BFE5 RID: 49125
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x0400BFE6 RID: 49126
		private string searchString = "";

		// Token: 0x0400BFE7 RID: 49127
		private float lastConceptActivateRealTime = -999f;

		// Token: 0x0400BFE8 RID: 49128
		private ConceptDef mouseoverConcept;

		// Token: 0x0400BFE9 RID: 49129
		private Rect windowRect;

		// Token: 0x0400BFEA RID: 49130
		private Action windowOnGUICached;

		// Token: 0x0400BFEB RID: 49131
		private const float OuterMargin = 8f;

		// Token: 0x0400BFEC RID: 49132
		private const float InnerMargin = 7f;

		// Token: 0x0400BFED RID: 49133
		private const float ReadoutWidth = 200f;

		// Token: 0x0400BFEE RID: 49134
		private const float InfoPaneWidth = 310f;

		// Token: 0x0400BFEF RID: 49135
		private const float OpenButtonSize = 24f;

		// Token: 0x0400BFF0 RID: 49136
		public static readonly Texture2D ProgressBarFillTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.74509805f, 0.6039216f, 0.2f));

		// Token: 0x0400BFF1 RID: 49137
		public static readonly Texture2D ProgressBarBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.50980395f, 0.40784314f, 0.13333334f));

		// Token: 0x0400BFF2 RID: 49138
		private static List<ConceptDef> tmpConceptsToShow = new List<ConceptDef>();
	}
}
