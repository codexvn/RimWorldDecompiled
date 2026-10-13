using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020034F5 RID: 13557
	[StaticConstructorOnStartup]
	public static class WidgetsWork
	{
		// Token: 0x1700319C RID: 12700
		// (get) Token: 0x060130C1 RID: 78017 RVA: 0x005A52AF File Offset: 0x005A34AF
		private static Texture2D WorkBoxBGTex_AgeDisabled
		{
			get
			{
				if (WidgetsWork.workBoxBGTex_AgeDisabled == null)
				{
					WidgetsWork.workBoxBGTex_AgeDisabled = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_AgeDisabled", true);
				}
				return WidgetsWork.workBoxBGTex_AgeDisabled;
			}
		}

		// Token: 0x060130C2 RID: 78018 RVA: 0x005A52D4 File Offset: 0x005A34D4
		public static Color ColorOfPriority(int prio)
		{
			switch (prio)
			{
			case 1:
				return new Color(0f, 1f, 0f);
			case 2:
				return new Color(1f, 0.9f, 0.5f);
			case 3:
				return new Color(0.8f, 0.7f, 0.5f);
			case 4:
				return new Color(0.74f, 0.74f, 0.74f);
			default:
				return Color.grey;
			}
		}

		// Token: 0x060130C3 RID: 78019 RVA: 0x005A5354 File Offset: 0x005A3554
		public static void DrawWorkBoxFor(float x, float y, Pawn p, WorkTypeDef wType, bool incapableBecauseOfCapacities)
		{
			if (p.WorkTypeIsDisabled(wType))
			{
				int value;
				if (p.IsWorkTypeDisabledByAge(wType, out value))
				{
					Rect rect = new Rect(x, y, 25f, 25f);
					if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
					{
						Messages.Message("MessageWorkTypeDisabledAge".Translate(p, p.ageTracker.AgeBiologicalYears, wType.labelShort, value), p, MessageTypeDefOf.RejectInput, false);
						SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
					}
					GUI.DrawTexture(rect, WidgetsWork.WorkBoxBGTex_AgeDisabled);
				}
				return;
			}
			Rect rect2 = new Rect(x, y, 25f, 25f);
			if (incapableBecauseOfCapacities)
			{
				GUI.color = new Color(1f, 0.3f, 0.3f);
			}
			WidgetsWork.DrawWorkBoxBackground(rect2, p, wType);
			GUI.color = Color.white;
			if (Find.PlaySettings.useWorkPriorities)
			{
				int priority = p.workSettings.GetPriority(wType);
				if (priority > 0)
				{
					Text.Anchor = TextAnchor.MiddleCenter;
					GUI.color = WidgetsWork.ColorOfPriority(priority);
					Widgets.Label(rect2.ContractedBy(-3f), priority.ToStringCached());
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
				}
				if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect2))
				{
					bool flag = p.workSettings.WorkIsActive(wType);
					if (Event.current.button == 0)
					{
						int num = p.workSettings.GetPriority(wType) - 1;
						if (num < 0)
						{
							num = 4;
						}
						p.workSettings.SetPriority(wType, num);
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					if (Event.current.button == 1)
					{
						int num2 = p.workSettings.GetPriority(wType) + 1;
						if (num2 > 4)
						{
							num2 = 0;
						}
						p.workSettings.SetPriority(wType, num2);
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					if (!flag && p.workSettings.WorkIsActive(wType) && wType.relevantSkills.Any<SkillDef>() && p.skills.AverageOfRelevantSkillsFor(wType) <= 2f)
					{
						SoundDefOf.Crunch.PlayOneShotOnCamera(null);
					}
					if (!flag && p.workSettings.WorkIsActive(wType) && p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wType))
					{
						Messages.Message("MessageIdeoOpposedWorkTypeSelected".Translate(p, wType.gerundLabel), p, MessageTypeDefOf.CautionInput, false);
						SoundDefOf.DislikedWorkTypeActivated.PlayOneShotOnCamera(null);
					}
					Event.current.Use();
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorkTab, KnowledgeAmount.SpecificInteraction);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ManualWorkPriorities, KnowledgeAmount.SmallInteraction);
					return;
				}
			}
			else
			{
				if (p.workSettings.GetPriority(wType) > 0)
				{
					GUI.DrawTexture(rect2, WidgetsWork.WorkBoxCheckTex);
				}
				if (Widgets.ButtonInvisible(rect2, true))
				{
					if (p.workSettings.GetPriority(wType) > 0)
					{
						p.workSettings.SetPriority(wType, 0);
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
					else
					{
						p.workSettings.SetPriority(wType, 3);
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
						if (wType.relevantSkills.Any<SkillDef>() && p.skills.AverageOfRelevantSkillsFor(wType) <= 2f)
						{
							SoundDefOf.Crunch.PlayOneShotOnCamera(null);
						}
						if (p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wType))
						{
							Messages.Message("MessageIdeoOpposedWorkTypeSelected".Translate(p, wType.gerundLabel), p, MessageTypeDefOf.CautionInput, false);
							SoundDefOf.DislikedWorkTypeActivated.PlayOneShotOnCamera(null);
						}
					}
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorkTab, KnowledgeAmount.SpecificInteraction);
				}
			}
		}

		// Token: 0x060130C4 RID: 78020 RVA: 0x005A56EC File Offset: 0x005A38EC
		public static string TipForPawnWorker(Pawn p, WorkTypeDef wDef, bool incapableBecauseOfCapacities)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = wDef.gerundLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor);
			int priority = p.workSettings.GetPriority(wDef);
			text = text + ": " + ("Priority" + priority.ToString()).Translate().Colorize(WidgetsWork.ColorOfPriority(priority));
			stringBuilder.AppendLine(text);
			if (p.WorkTypeIsDisabled(wDef))
			{
				string text2 = "CannotDoThisWork".Translate(p.LabelShort, p);
				List<string> reasonsForDisabledWorkType = p.GetReasonsForDisabledWorkType(wDef);
				if (!reasonsForDisabledWorkType.NullOrEmpty<string>())
				{
					string text3 = "\n\n" + string.Join(". ", reasonsForDisabledWorkType);
					if (reasonsForDisabledWorkType.Count == 1)
					{
						text3 += ".";
					}
					text2 += text3.Colorize(ColorLibrary.RedReadable);
				}
				stringBuilder.Append(text2);
			}
			else
			{
				float num = p.skills.AverageOfRelevantSkillsFor(wDef);
				if (wDef.relevantSkills.Any<SkillDef>())
				{
					string text4 = "";
					foreach (SkillDef skillDef in wDef.relevantSkills)
					{
						text4 = text4 + skillDef.skillLabel.CapitalizeFirst() + ", ";
					}
					text4 = text4.Substring(0, text4.Length - 2);
					stringBuilder.AppendLine("RelevantSkills".Translate(text4, num.ToString("0.#"), 20));
				}
				if (p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(wDef))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("SelectedWorkTypeOpposedByIdeo".Translate(p).Colorize(WidgetsWork.ColorOfPriority(2)));
				}
				if (wDef.relevantSkills.Any<SkillDef>() && num <= 2f && p.workSettings.WorkIsActive(wDef))
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("SelectedWorkTypeWithVeryBadSkill".Translate().Colorize(WidgetsWork.ColorOfPriority(2)));
				}
				stringBuilder.AppendLine();
				stringBuilder.Append(wDef.description);
				if (incapableBecauseOfCapacities)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
					stringBuilder.Append("IncapableOfWorkTypeBecauseOfCapacities".Translate());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060130C5 RID: 78021 RVA: 0x005A5984 File Offset: 0x005A3B84
		private static void DrawWorkBoxBackground(Rect rect, Pawn p, WorkTypeDef workDef)
		{
			float num = p.skills.AverageOfRelevantSkillsFor(workDef);
			Texture2D image;
			Texture2D image2;
			float a;
			if (num < 4f)
			{
				image = WidgetsWork.WorkBoxBGTex_Awful;
				image2 = WidgetsWork.WorkBoxBGTex_Bad;
				a = num / 4f;
			}
			else if (num <= 14f)
			{
				image = WidgetsWork.WorkBoxBGTex_Bad;
				image2 = WidgetsWork.WorkBoxBGTex_Mid;
				a = (num - 4f) / 10f;
			}
			else
			{
				image = WidgetsWork.WorkBoxBGTex_Mid;
				image2 = WidgetsWork.WorkBoxBGTex_Excellent;
				a = (num - 14f) / 6f;
			}
			GUI.DrawTexture(rect, image);
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, a);
			GUI.DrawTexture(rect, image2);
			if (p.Ideo != null && p.Ideo.IsWorkTypeConsideredDangerous(workDef))
			{
				GUI.color = Color.white;
				GUI.DrawTexture(rect, WidgetsWork.WorkBoxOverlay_PreceptWarning);
			}
			if (workDef.relevantSkills.Any<SkillDef>() && num <= 2f && p.workSettings.WorkIsActive(workDef))
			{
				GUI.color = Color.white;
				GUI.DrawTexture(rect.ContractedBy(-2f), WidgetsWork.WorkBoxOverlay_Warning);
			}
			Passion passion = p.skills.MaxPassionOfRelevantSkillsFor(workDef);
			if (passion > Passion.None)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.4f);
				Rect position = rect;
				position.xMin = rect.center.x;
				position.yMin = rect.center.y;
				if (passion == Passion.Minor)
				{
					GUI.DrawTexture(position, WidgetsWork.PassionWorkboxMinorIcon);
				}
				else if (passion == Passion.Major)
				{
					GUI.DrawTexture(position, WidgetsWork.PassionWorkboxMajorIcon);
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x0400C559 RID: 50521
		public const float WorkBoxSize = 25f;

		// Token: 0x0400C55A RID: 50522
		public static readonly Texture2D WorkBoxBGTex_Awful = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_Awful", true);

		// Token: 0x0400C55B RID: 50523
		public static readonly Texture2D WorkBoxBGTex_Bad = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_Bad", true);

		// Token: 0x0400C55C RID: 50524
		private const int AwfulBGMax = 4;

		// Token: 0x0400C55D RID: 50525
		public static readonly Texture2D WorkBoxBGTex_Mid = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_Mid", true);

		// Token: 0x0400C55E RID: 50526
		private const int BadBGMax = 14;

		// Token: 0x0400C55F RID: 50527
		public static readonly Texture2D WorkBoxBGTex_Excellent = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxBG_Excellent", true);

		// Token: 0x0400C560 RID: 50528
		public static readonly Texture2D WorkBoxCheckTex = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxCheck", true);

		// Token: 0x0400C561 RID: 50529
		public static readonly Texture2D PassionWorkboxMinorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMinorGray", true);

		// Token: 0x0400C562 RID: 50530
		public static readonly Texture2D PassionWorkboxMajorIcon = ContentFinder<Texture2D>.Get("UI/Icons/PassionMajorGray", true);

		// Token: 0x0400C563 RID: 50531
		public static readonly Texture2D WorkBoxOverlay_Warning = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxOverlay_Warning", true);

		// Token: 0x0400C564 RID: 50532
		public static readonly Texture2D WorkBoxOverlay_PreceptWarning = ContentFinder<Texture2D>.Get("UI/Widgets/WorkBoxOverlay_PreceptWarning", true);

		// Token: 0x0400C565 RID: 50533
		private const int WarnIfSelectedMax = 2;

		// Token: 0x0400C566 RID: 50534
		private static Texture2D workBoxBGTex_AgeDisabled;

		// Token: 0x0400C567 RID: 50535
		private const float PassionOpacity = 0.4f;
	}
}
