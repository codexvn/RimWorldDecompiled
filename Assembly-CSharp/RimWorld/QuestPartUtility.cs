using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCF RID: 7375
	public static class QuestPartUtility
	{
		// Token: 0x0600A470 RID: 42096 RVA: 0x002F17EC File Offset: 0x002EF9EC
		public static T MakeAndAddEndCondition<T>(Quest quest, string inSignalActivate, QuestEndOutcome outcome, Letter letter = null) where T : QuestPartActivable, new()
		{
			T t = Activator.CreateInstance<T>();
			t.inSignalEnable = inSignalActivate;
			quest.AddPart(t);
			if (letter != null)
			{
				quest.AddPart(new QuestPart_Letter
				{
					letter = letter,
					inSignal = t.OutSignalCompleted
				});
			}
			quest.AddPart(new QuestPart_QuestEnd
			{
				inSignal = t.OutSignalCompleted,
				outcome = new QuestEndOutcome?(outcome)
			});
			return t;
		}

		// Token: 0x0600A471 RID: 42097 RVA: 0x002F186C File Offset: 0x002EFA6C
		public static QuestPart_QuestEnd MakeAndAddEndNodeWithLetter(Quest quest, string inSignalActivate, QuestEndOutcome outcome, Letter letter)
		{
			quest.AddPart(new QuestPart_Letter
			{
				letter = letter,
				inSignal = inSignalActivate
			});
			QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
			questPart_QuestEnd.inSignal = inSignalActivate;
			questPart_QuestEnd.outcome = new QuestEndOutcome?(outcome);
			quest.AddPart(questPart_QuestEnd);
			return questPart_QuestEnd;
		}

		// Token: 0x0600A472 RID: 42098 RVA: 0x002F18B8 File Offset: 0x002EFAB8
		public static QuestPart_Delay MakeAndAddQuestTimeoutDelay(Quest quest, int delayTicks, WorldObject worldObject)
		{
			QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = new QuestPart_WorldObjectTimeout();
			questPart_WorldObjectTimeout.delayTicks = delayTicks;
			questPart_WorldObjectTimeout.expiryInfoPart = "QuestExpiresIn".Translate();
			questPart_WorldObjectTimeout.expiryInfoPartTip = "QuestExpiresOn".Translate();
			questPart_WorldObjectTimeout.isBad = true;
			questPart_WorldObjectTimeout.outcomeCompletedSignalArg = QuestEndOutcome.Fail;
			questPart_WorldObjectTimeout.inSignalEnable = quest.InitiateSignal;
			quest.AddPart(questPart_WorldObjectTimeout);
			string text = "Quest" + quest.id.ToString() + ".DelayingWorldObject";
			QuestUtility.AddQuestTag(ref worldObject.questTags, text);
			questPart_WorldObjectTimeout.inSignalDisable = text + ".MapGenerated";
			quest.AddPart(new QuestPart_QuestEnd
			{
				inSignal = questPart_WorldObjectTimeout.OutSignalCompleted
			});
			return questPart_WorldObjectTimeout;
		}

		// Token: 0x0600A473 RID: 42099 RVA: 0x002F196F File Offset: 0x002EFB6F
		public static IEnumerable<GenUI.AnonymousStackElement> GetRewardStackElementsForThings(IEnumerable<Thing> things, bool detailsHidden = false)
		{
			QuestPartUtility.tmpThings.Clear();
			foreach (Thing thing2 in things)
			{
				bool flag = false;
				for (int j = 0; j < QuestPartUtility.tmpThings.Count; j++)
				{
					if (QuestPartUtility.tmpThings[j].First.CanStackWith(thing2))
					{
						QuestPartUtility.tmpThings[j] = new Pair<Thing, int>(QuestPartUtility.tmpThings[j].First, QuestPartUtility.tmpThings[j].Second + thing2.stackCount);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestPartUtility.tmpThings.Add(new Pair<Thing, int>(thing2, thing2.stackCount));
				}
			}
			int num;
			for (int i = 0; i < QuestPartUtility.tmpThings.Count; i = num + 1)
			{
				Thing thing = QuestPartUtility.tmpThings[i].First.GetInnerIfMinified();
				int second = QuestPartUtility.tmpThings[i].Second;
				Pawn pawn = thing as Pawn;
				string label;
				if (pawn != null)
				{
					label = "PawnQuestReward".Translate(pawn);
				}
				else
				{
					label = thing.LabelCapNoCount + ((second > 1) ? (" x" + second.ToString()) : "");
				}
				yield return new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect rect)
					{
						Widgets.DrawHighlight(rect);
						Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
						if (Mouse.IsOver(rect))
						{
							Widgets.DrawHighlight(rect);
							TaggedString taggedString = detailsHidden ? "NoMoreInfoAvailable".Translate() : "ClickForMoreInfo".Translate();
							taggedString = taggedString.Colorize(ColoredText.SubtleGrayColor);
							Pawn pawn2 = thing as Pawn;
							if (pawn2 != null && pawn2.RaceProps.Humanlike)
							{
								string text = pawn2.LabelCap;
								if (ModsConfig.BiotechActive && pawn2.genes != null)
								{
									text += (" (" + pawn2.genes.XenotypeLabel + ")").Colorize(ColoredText.SubtleGrayColor);
									text = text + "\n\n" + pawn2.genes.XenotypeDescShort;
								}
								SkillRecord skillRecord = pawn2.skills.skills.MaxBy(delegate(SkillRecord s)
								{
									if (!s.TotallyDisabled)
									{
										return s.Level;
									}
									return -1;
								});
								string arg = new StringBuilder().Append("BestSkillLabel".Translate().Colorize(ColoredText.TipSectionTitleColor)).Append(": ").Append(skillRecord.def.LabelCap.ToString()).Append(" (").Append("BestSkillInfoLevel".Translate(skillRecord.Level).ToString()).Append(')').ToString();
								TooltipHandler.TipRegion(rect, string.Format("{0}\n\n{1}\n\n{2}", text, arg, taggedString));
							}
							else
							{
								TooltipHandler.TipRegion(rect, thing.DescriptionDetailed + "\n\n" + taggedString);
							}
						}
						Widgets.ThingIcon(new Rect(rect2.x, rect2.y, 22f, 22f), thing, 1f, null, false, 1f, false);
						Rect rect3 = rect2;
						rect3.xMin += 24f;
						Widgets.Label(rect3, label);
						if (Widgets.ButtonInvisible(rect, true))
						{
							if (detailsHidden)
							{
								Messages.Message("NoMoreInfoAvailable".Translate(), MessageTypeDefOf.RejectInput, false);
								return;
							}
							Find.WindowStack.Add(new Dialog_InfoCard(thing, null));
						}
					},
					width = Text.CalcSize(label).x + 12f + 22f + 2f
				};
				num = i;
			}
			yield break;
		}

		// Token: 0x0600A474 RID: 42100 RVA: 0x002F1986 File Offset: 0x002EFB86
		public static IEnumerable<GenUI.AnonymousStackElement> GetRewardStackElementsForThings(IEnumerable<Reward_Items.RememberedItem> thingDefs)
		{
			QuestPartUtility.tmpThingDefs.Clear();
			foreach (Reward_Items.RememberedItem rememberedItem in thingDefs)
			{
				bool flag = false;
				for (int j = 0; j < QuestPartUtility.tmpThingDefs.Count; j++)
				{
					if (QuestPartUtility.tmpThingDefs[j].thing == rememberedItem.thing && QuestPartUtility.tmpThingDefs[j].label == rememberedItem.label)
					{
						QuestPartUtility.tmpThingDefs[j] = new Reward_Items.RememberedItem(QuestPartUtility.tmpThingDefs[j].thing, QuestPartUtility.tmpThingDefs[j].stackCount + rememberedItem.stackCount, QuestPartUtility.tmpThingDefs[j].label);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestPartUtility.tmpThingDefs.Add(rememberedItem);
				}
			}
			int num;
			for (int i = 0; i < QuestPartUtility.tmpThingDefs.Count; i = num + 1)
			{
				ThingStuffPairWithQuality thing = QuestPartUtility.tmpThingDefs[i].thing;
				int stackCount = QuestPartUtility.tmpThingDefs[i].stackCount;
				string label = QuestPartUtility.tmpThingDefs[i].label.CapitalizeFirst() + ((stackCount > 1) ? (" x" + stackCount.ToString()) : "");
				yield return new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect rect)
					{
						Widgets.DrawHighlight(rect);
						Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
						if (Mouse.IsOver(rect))
						{
							Widgets.DrawHighlight(rect);
							TaggedString str = thing.thing.DescriptionDetailed + "\n\n" + "ClickForMoreInfo".Translate().Colorize(ColoredText.SubtleGrayColor);
							TooltipHandler.TipRegion(rect, str);
						}
						Widgets.ThingIcon(new Rect(rect2.x, rect2.y, 22f, 22f), thing.thing, thing.stuff, null, 1f, null, null, 1f);
						Rect rect3 = rect2;
						rect3.xMin += 24f;
						Widgets.Label(rect3, label);
						if (Widgets.ButtonInvisible(rect, true))
						{
							Find.WindowStack.Add(new Dialog_InfoCard(thing.thing, thing.stuff, null));
						}
					},
					width = Text.CalcSize(label).x + 12f + 22f + 2f
				};
				num = i;
			}
			yield break;
		}

		// Token: 0x0600A475 RID: 42101 RVA: 0x002F1998 File Offset: 0x002EFB98
		public static GenUI.AnonymousStackElement GetStandardRewardStackElement(string label, Texture2D icon, Func<string> tipGetter, Action onClick = null)
		{
			return QuestPartUtility.GetStandardRewardStackElement(label, delegate(Rect r)
			{
				GUI.DrawTexture(r, icon);
			}, tipGetter, onClick);
		}

		// Token: 0x0600A476 RID: 42102 RVA: 0x002F19C8 File Offset: 0x002EFBC8
		public static GenUI.AnonymousStackElement GetStandardRewardStackElement(string label, Action<Rect> iconDrawer, Func<string> tipGetter, Action onClick = null)
		{
			return new GenUI.AnonymousStackElement
			{
				drawer = delegate(Rect rect)
				{
					Widgets.DrawHighlight(rect);
					Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
					if (Mouse.IsOver(rect))
					{
						Widgets.DrawHighlight(rect);
						if (tipGetter != null)
						{
							TooltipHandler.TipRegion(rect, new TipSignal(tipGetter, 90876542 ^ (int)rect.x ^ (int)rect.y));
						}
					}
					Rect obj = new Rect(rect2.x, rect2.y, 22f, 22f);
					iconDrawer(obj);
					Rect rect3 = rect2;
					rect3.xMin += 24f;
					Widgets.Label(rect3, label);
					if (onClick != null && Widgets.ButtonInvisible(rect, true))
					{
						onClick();
					}
				},
				width = Text.CalcSize(label).x + 12f + 22f + 2f
			};
		}

		// Token: 0x0400717E RID: 29054
		public const int RewardStackElementIconSize = 22;

		// Token: 0x0400717F RID: 29055
		public const int RewardStackElementMarginHorizontal = 6;

		// Token: 0x04007180 RID: 29056
		public const int RewardStackElementMarginVertical = 1;

		// Token: 0x04007181 RID: 29057
		public const int RewardStackElementIconGap = 2;

		// Token: 0x04007182 RID: 29058
		private static List<Pair<Thing, int>> tmpThings = new List<Pair<Thing, int>>();

		// Token: 0x04007183 RID: 29059
		private static List<Reward_Items.RememberedItem> tmpThingDefs = new List<Reward_Items.RememberedItem>();
	}
}
