using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200322E RID: 12846
	public static class NeedsCardUtility
	{
		// Token: 0x060120EF RID: 73967 RVA: 0x00546D8C File Offset: 0x00544F8C
		public static Vector2 GetSize(Pawn pawn)
		{
			NeedsCardUtility.UpdateDisplayNeeds(pawn);
			if (pawn.needs.mood != null)
			{
				return NeedsCardUtility.FullSize;
			}
			return new Vector2(225f, (float)NeedsCardUtility.displayNeeds.Count * Mathf.Min(70f, NeedsCardUtility.FullSize.y / (float)NeedsCardUtility.displayNeeds.Count));
		}

		// Token: 0x060120F0 RID: 73968 RVA: 0x00546DE8 File Offset: 0x00544FE8
		public static void DoNeedsMoodAndThoughts(Rect rect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			Rect rect2 = new Rect(rect.x, rect.y, 225f, rect.height);
			NeedsCardUtility.DoNeeds(rect2, pawn);
			if (pawn.needs.mood != null)
			{
				NeedsCardUtility.DoMoodAndThoughts(new Rect(rect2.xMax, rect.y, rect.width - rect2.width, rect.height), pawn, ref thoughtScrollPosition);
			}
		}

		// Token: 0x060120F1 RID: 73969 RVA: 0x00546E5C File Offset: 0x0054505C
		public static void DoNeeds(Rect rect, Pawn pawn)
		{
			NeedsCardUtility.UpdateDisplayNeeds(pawn);
			float num = 0f;
			for (int i = 0; i < NeedsCardUtility.displayNeeds.Count; i++)
			{
				Need need = NeedsCardUtility.displayNeeds[i];
				Rect rect2 = new Rect(rect.x, rect.y + num, rect.width, Mathf.Min(70f, rect.height / (float)NeedsCardUtility.displayNeeds.Count));
				if (!need.def.major)
				{
					if (i > 0 && NeedsCardUtility.displayNeeds[i - 1].def.major)
					{
						rect2.y += 10f;
					}
					rect2.width *= 0.73f;
					rect2.height = Mathf.Max(rect2.height * 0.666f, 30f);
				}
				need.DrawOnGUI(rect2, int.MaxValue, -1f, true, true, null, true);
				num = rect2.yMax;
			}
		}

		// Token: 0x060120F2 RID: 73970 RVA: 0x00546F68 File Offset: 0x00545168
		private static void DoMoodAndThoughts(Rect rect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.width * 0.8f, 70f);
			pawn.needs.mood.DrawOnGUI(rect2, int.MaxValue, -1f, true, true, null, true);
			NeedsCardUtility.DrawThoughtListing(new Rect(0f, 80f, rect.width, rect.height - 70f - 10f).ContractedBy(10f), pawn, ref thoughtScrollPosition);
			Widgets.EndGroup();
		}

		// Token: 0x060120F3 RID: 73971 RVA: 0x00547004 File Offset: 0x00545204
		private static void UpdateDisplayNeeds(Pawn pawn)
		{
			NeedsCardUtility.displayNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (allNeeds[i].ShowOnNeedList)
				{
					NeedsCardUtility.displayNeeds.Add(allNeeds[i]);
				}
			}
			PawnNeedsUIUtility.SortInDisplayOrder(NeedsCardUtility.displayNeeds);
		}

		// Token: 0x060120F4 RID: 73972 RVA: 0x00547064 File Offset: 0x00545264
		private static void DrawThoughtListing(Rect listingRect, Pawn pawn, ref Vector2 thoughtScrollPosition)
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			Text.Font = GameFont.Small;
			IdeoUIUtility.DrawExtraThoughtInfoFromIdeo(pawn, ref listingRect);
			PawnNeedsUIUtility.GetThoughtGroupsInDisplayOrder(pawn.needs.mood, NeedsCardUtility.thoughtGroupsPresent);
			float height = (float)NeedsCardUtility.thoughtGroupsPresent.Count * 24f;
			Widgets.BeginScrollView(listingRect, ref thoughtScrollPosition, new Rect(0f, 0f, listingRect.width - 16f, height), true);
			Text.Anchor = TextAnchor.MiddleLeft;
			float num = 0f;
			for (int i = 0; i < NeedsCardUtility.thoughtGroupsPresent.Count; i++)
			{
				if (NeedsCardUtility.DrawThoughtGroup(new Rect(0f, num, listingRect.width - 16f, 20f), NeedsCardUtility.thoughtGroupsPresent[i], pawn))
				{
					num += 24f;
				}
			}
			Widgets.EndScrollView();
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060120F5 RID: 73973 RVA: 0x00547140 File Offset: 0x00545340
		private static bool DrawThoughtGroup(Rect rect, Thought group, Pawn pawn)
		{
			try
			{
				pawn.needs.mood.thoughts.GetMoodThoughts(group, NeedsCardUtility.thoughtGroup);
				if (NeedsCardUtility.thoughtGroup.Count == 0)
				{
					return false;
				}
				Thought leadingThought = PawnNeedsUIUtility.GetLeadingThoughtInGroup(NeedsCardUtility.thoughtGroup);
				if (!leadingThought.VisibleInNeedsTab)
				{
					NeedsCardUtility.thoughtGroup.Clear();
					return false;
				}
				if (leadingThought != NeedsCardUtility.thoughtGroup[0])
				{
					NeedsCardUtility.thoughtGroup.Remove(leadingThought);
					NeedsCardUtility.thoughtGroup.Insert(0, leadingThought);
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
				}
				if (Mouse.IsOver(rect))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(leadingThought.LabelCap.AsTipTitle()).AppendLine().AppendLine();
					if (pawn.DevelopmentalStage.Baby())
					{
						stringBuilder.AppendLine(leadingThought.BabyTalk);
						stringBuilder.AppendLine();
						stringBuilder.AppendTagged(("Translation".Translate() + ": " + leadingThought.Description).Colorize(ColoredText.SubtleGrayColor));
					}
					else
					{
						stringBuilder.Append(leadingThought.Description);
					}
					int durationTicks = group.DurationTicks;
					if (durationTicks > 5)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
						Thought_Memory thought_Memory = leadingThought as Thought_Memory;
						if (thought_Memory != null)
						{
							if (NeedsCardUtility.thoughtGroup.Count == 1)
							{
								stringBuilder.AppendTagged("ThoughtExpiresIn".Translate((durationTicks - thought_Memory.age).ToStringTicksToPeriod(true, false, true, true, false)));
							}
							else
							{
								int num = int.MaxValue;
								int num2 = int.MinValue;
								foreach (Thought thought in NeedsCardUtility.thoughtGroup)
								{
									Thought_Memory thought_Memory2 = (Thought_Memory)thought;
									num = Mathf.Min(num, thought_Memory2.age);
									num2 = Mathf.Max(num2, thought_Memory2.age);
								}
								stringBuilder.AppendTagged("ThoughtStartsExpiringIn".Translate((durationTicks - num2).ToStringTicksToPeriod(true, false, true, true, false)));
								stringBuilder.AppendLine();
								stringBuilder.AppendTagged("ThoughtFinishesExpiringIn".Translate((durationTicks - num).ToStringTicksToPeriod(true, false, true, true, false)));
							}
						}
					}
					if (NeedsCardUtility.thoughtGroup.Count > 1)
					{
						bool flag = false;
						for (int i = 1; i < NeedsCardUtility.thoughtGroup.Count; i++)
						{
							bool flag2 = false;
							for (int j = 0; j < i; j++)
							{
								if (NeedsCardUtility.thoughtGroup[i].LabelCap == NeedsCardUtility.thoughtGroup[j].LabelCap)
								{
									flag2 = true;
									break;
								}
							}
							if (!flag2)
							{
								if (!flag)
								{
									stringBuilder.AppendLine();
									stringBuilder.AppendLine();
									flag = true;
								}
								stringBuilder.AppendLine("+ " + NeedsCardUtility.thoughtGroup[i].LabelCap);
							}
						}
					}
					TooltipHandler.TipRegion(rect, new TipSignal(stringBuilder.ToString(), 7291));
				}
				Text.WordWrap = false;
				Text.Anchor = TextAnchor.MiddleLeft;
				Rect rect2 = new Rect(rect.x + 10f, rect.y, 225f, rect.height);
				rect2.yMin -= 3f;
				rect2.yMax += 3f;
				string text = leadingThought.LabelCap;
				if (NeedsCardUtility.thoughtGroup.Count > 1)
				{
					text = text + " x" + NeedsCardUtility.thoughtGroup.Count.ToString();
				}
				Widgets.Label(rect2, text);
				Text.Anchor = TextAnchor.MiddleCenter;
				float num3 = pawn.needs.mood.thoughts.MoodOffsetOfGroup(group);
				if (num3 == 0f)
				{
					GUI.color = NeedsCardUtility.NoEffectColor;
				}
				else if (num3 > 0f)
				{
					GUI.color = NeedsCardUtility.MoodColor;
				}
				else
				{
					GUI.color = NeedsCardUtility.MoodColorNegative;
				}
				Widgets.Label(new Rect(rect.x + 235f, rect.y, 32f, rect.height), num3.ToString("##0"));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				Text.WordWrap = true;
				if (ModsConfig.IdeologyActive && leadingThought.sourcePrecept != null && !Find.IdeoManager.classicMode)
				{
					IdeoUIUtility.DoIdeoIcon(new Rect(rect.x + 235f + 32f + 10f, rect.y, 20f, 20f), leadingThought.sourcePrecept.ideo, false, delegate
					{
						IdeoUIUtility.OpenIdeoInfo(leadingThought.sourcePrecept.ideo);
					});
				}
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception in DrawThoughtGroup for ";
				int num4 = 1;
				ThoughtDef def = group.def;
				array[num4] = ((def != null) ? def.ToString() : null);
				array[2] = " on ";
				array[3] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ": ";
				int num5 = 5;
				Exception ex2 = ex;
				array[num5] = ((ex2 != null) ? ex2.ToString() : null);
				Log.ErrorOnce(string.Concat(array), 3452698);
			}
			return true;
		}

		// Token: 0x0400BB6C RID: 47980
		private static List<Need> displayNeeds = new List<Need>();

		// Token: 0x0400BB6D RID: 47981
		public static readonly Color MoodColor = new Color(0.1f, 1f, 0.1f);

		// Token: 0x0400BB6E RID: 47982
		public static readonly Color MoodColorNegative = new Color(0.8f, 0.4f, 0.4f);

		// Token: 0x0400BB6F RID: 47983
		private static readonly Color NoEffectColor = new Color(0.5f, 0.5f, 0.5f, 0.75f);

		// Token: 0x0400BB70 RID: 47984
		private const float ThoughtHeight = 20f;

		// Token: 0x0400BB71 RID: 47985
		private const float ThoughtSpacing = 4f;

		// Token: 0x0400BB72 RID: 47986
		private const float ThoughtIntervalY = 24f;

		// Token: 0x0400BB73 RID: 47987
		private const float MoodX = 235f;

		// Token: 0x0400BB74 RID: 47988
		private const float MoodNumberWidth = 32f;

		// Token: 0x0400BB75 RID: 47989
		private const float NeedsColumnWidth = 225f;

		// Token: 0x0400BB76 RID: 47990
		public static readonly Vector2 FullSize = new Vector2(580f, 520f);

		// Token: 0x0400BB77 RID: 47991
		private static List<Thought> thoughtGroupsPresent = new List<Thought>();

		// Token: 0x0400BB78 RID: 47992
		private static List<Thought> thoughtGroup = new List<Thought>();
	}
}
