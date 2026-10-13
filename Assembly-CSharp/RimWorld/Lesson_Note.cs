using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020034E9 RID: 13545
	public class Lesson_Note : Lesson
	{
		// Token: 0x17003197 RID: 12695
		// (get) Token: 0x06013086 RID: 77958 RVA: 0x005A407A File Offset: 0x005A227A
		public bool Expiring
		{
			get
			{
				return this.expiryTime < float.MaxValue;
			}
		}

		// Token: 0x17003198 RID: 12696
		// (get) Token: 0x06013087 RID: 77959 RVA: 0x005A408C File Offset: 0x005A228C
		public Rect MainRect
		{
			get
			{
				float height = Text.CalcHeight(this.def.HelpTextAdjusted, 432f) + 20f;
				return new Rect(Messages.MessagesTopLeftStandard.x, 0f, 500f, height);
			}
		}

		// Token: 0x17003199 RID: 12697
		// (get) Token: 0x06013088 RID: 77960 RVA: 0x005A40D0 File Offset: 0x005A22D0
		public override float MessagesYOffset
		{
			get
			{
				return this.MainRect.height;
			}
		}

		// Token: 0x06013089 RID: 77961 RVA: 0x005A40EB File Offset: 0x005A22EB
		public Lesson_Note()
		{
		}

		// Token: 0x0601308A RID: 77962 RVA: 0x005A4105 File Offset: 0x005A2305
		public Lesson_Note(ConceptDef concept)
		{
			this.def = concept;
		}

		// Token: 0x0601308B RID: 77963 RVA: 0x005A4126 File Offset: 0x005A2326
		public override void ExposeData()
		{
			Scribe_Defs.Look<ConceptDef>(ref this.def, "def");
		}

		// Token: 0x0601308C RID: 77964 RVA: 0x005A4138 File Offset: 0x005A2338
		public override void OnActivated()
		{
			base.OnActivated();
			SoundDefOf.TutorMessageAppear.PlayOneShotOnCamera(null);
		}

		// Token: 0x0601308D RID: 77965 RVA: 0x005A414C File Offset: 0x005A234C
		public override void LessonOnGUI()
		{
			Rect mainRect = this.MainRect;
			float alpha = 1f;
			if (this.doFadeIn)
			{
				alpha = Mathf.Clamp01(base.AgeSeconds / 0.4f);
			}
			if (this.Expiring)
			{
				float num = this.expiryTime - Time.timeSinceLevelLoad;
				if (num < 1.1f)
				{
					alpha = num / 1.1f;
				}
			}
			Find.WindowStack.ImmediateWindow(134706, mainRect, WindowLayer.Super, delegate
			{
				Rect rect = mainRect.AtZero();
				Text.Font = GameFont.Small;
				if (!this.Expiring)
				{
					this.def.HighlightAllTags();
				}
				if (this.doFadeIn || this.Expiring)
				{
					GUI.color = new Color(1f, 1f, 1f, alpha);
				}
				Widgets.DrawWindowBackgroundTutor(rect);
				Rect rect2 = rect.ContractedBy(10f);
				rect2.width = 432f;
				Widgets.Label(rect2, this.def.HelpTextAdjusted);
				Rect butRect = new Rect(rect.xMax - 32f - 8f, rect.y + 8f, 32f, 32f);
				Texture2D tex;
				if (this.Expiring)
				{
					tex = Widgets.CheckboxOnTex;
				}
				else
				{
					tex = TexButton.CloseXBig;
				}
				if (Widgets.ButtonImage(butRect, tex, new Color(0.95f, 0.95f, 0.95f), new Color(0.8352941f, 0.6666667f, 0.27450982f), true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.CloseButtonClicked();
				}
				if (Time.timeSinceLevelLoad > this.expiryTime)
				{
					this.CloseButtonClicked();
				}
				GUI.color = Color.white;
			}, false, false, alpha, null, false);
		}

		// Token: 0x0601308E RID: 77966 RVA: 0x005A41F4 File Offset: 0x005A23F4
		private void CloseButtonClicked()
		{
			KnowledgeAmount know = this.def.noteTeaches ? KnowledgeAmount.NoteTaught : KnowledgeAmount.NoteClosed;
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(this.def, know);
			Find.ActiveLesson.Deactivate();
		}

		// Token: 0x0601308F RID: 77967 RVA: 0x005A4229 File Offset: 0x005A2429
		public override void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (this.def == conc && PlayerKnowledgeDatabase.GetKnowledge(conc) > 0.2f && !this.Expiring)
			{
				this.expiryTime = Time.timeSinceLevelLoad + 2.1f;
			}
		}

		// Token: 0x0400C531 RID: 50481
		public ConceptDef def;

		// Token: 0x0400C532 RID: 50482
		public bool doFadeIn = true;

		// Token: 0x0400C533 RID: 50483
		private float expiryTime = float.MaxValue;

		// Token: 0x0400C534 RID: 50484
		private const float RectWidth = 500f;

		// Token: 0x0400C535 RID: 50485
		private const float TextWidth = 432f;

		// Token: 0x0400C536 RID: 50486
		private const float FadeInDuration = 0.4f;

		// Token: 0x0400C537 RID: 50487
		private const float DoneButPad = 8f;

		// Token: 0x0400C538 RID: 50488
		private const float DoneButSize = 32f;

		// Token: 0x0400C539 RID: 50489
		private const float ExpiryDuration = 2.1f;

		// Token: 0x0400C53A RID: 50490
		private const float ExpiryFadeTime = 1.1f;
	}
}
