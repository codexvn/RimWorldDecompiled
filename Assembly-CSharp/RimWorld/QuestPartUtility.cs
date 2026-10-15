using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class QuestPartUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public ThingStuffPairWithQuality thing;

		public string label;

		internal void _003CGetRewardStackElementsForThings_003Eb__0(Rect rect)
		{
			Widgets.DrawHighlight(rect);
			Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TaggedString taggedString = thing.thing.DescriptionDetailed + "\n\n" + "ClickForMoreInfo".Translate().Colorize(ColoredText.SubtleGrayColor);
				TooltipHandler.TipRegion(rect, taggedString);
			}
			Widgets.ThingIcon(new Rect(rect2.x, rect2.y, 22f, 22f), thing.thing, thing.stuff, null, 1f, null, null);
			Rect rect3 = rect2;
			rect3.xMin += 24f;
			Widgets.Label(rect3, label);
			if (Widgets.ButtonInvisible(rect))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(thing.thing, thing.stuff));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public bool detailsHidden;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_1
	{
		public Thing thing;

		public string label;

		public _003C_003Ec__DisplayClass8_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetRewardStackElementsForThings_003Eb__0(Rect rect)
		{
			Widgets.DrawHighlight(rect);
			Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TaggedString ts = (CS_0024_003C_003E8__locals1.detailsHidden ? "NoMoreInfoAvailable".Translate() : "ClickForMoreInfo".Translate());
				ts = ts.Colorize(ColoredText.SubtleGrayColor);
				if (thing is Pawn pawn && pawn.RaceProps.Humanlike)
				{
					string text = pawn.LabelCap;
					if (ModsConfig.BiotechActive && pawn.genes != null)
					{
						text += (" (" + pawn.genes.XenotypeLabel + ")").Colorize(ColoredText.SubtleGrayColor);
						text = text + "\n\n" + pawn.genes.XenotypeDescShort;
					}
					SkillRecord skillRecord = pawn.skills.skills.MaxBy((SkillRecord s) => (!s.TotallyDisabled) ? s.Level : (-1));
					string value = new StringBuilder().Append("BestSkillLabel".Translate().Colorize(ColoredText.TipSectionTitleColor)).Append(": ").Append(skillRecord.def.LabelCap.ToString())
						.Append(" (")
						.Append("BestSkillInfoLevel".Translate(skillRecord.Level).ToString())
						.Append(')')
						.ToString();
					TooltipHandler.TipRegion(rect, $"{text}\n\n{value}\n\n{ts}");
				}
				else
				{
					TooltipHandler.TipRegion(rect, thing.DescriptionDetailed + "\n\n" + ts);
				}
			}
			Widgets.ThingIcon(new Rect(rect2.x, rect2.y, 22f, 22f), thing, 1f, null);
			Rect rect3 = rect2;
			rect3.xMin += 24f;
			Widgets.Label(rect3, label);
			if (Widgets.ButtonInvisible(rect))
			{
				if (CS_0024_003C_003E8__locals1.detailsHidden)
				{
					Messages.Message("NoMoreInfoAvailable".Translate(), MessageTypeDefOf.RejectInput, historical: false);
				}
				else
				{
					Find.WindowStack.Add(new Dialog_InfoCard(thing));
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRewardStackElementsForThings_003Ed__10 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<Reward_Items.RememberedItem> thingDefs;

		public IEnumerable<Reward_Items.RememberedItem> _003C_003E3__thingDefs;

		private int _003Ci_003E5__2;

		GenUI.AnonymousStackElement IEnumerator<GenUI.AnonymousStackElement>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetRewardStackElementsForThings_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				tmpThingDefs.Clear();
				foreach (Reward_Items.RememberedItem item in thingDefs)
				{
					bool flag = false;
					for (int i = 0; i < tmpThingDefs.Count; i++)
					{
						if (tmpThingDefs[i].thing == item.thing && tmpThingDefs[i].label == item.label)
						{
							tmpThingDefs[i] = new Reward_Items.RememberedItem(tmpThingDefs[i].thing, tmpThingDefs[i].stackCount + item.stackCount, tmpThingDefs[i].label);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						tmpThingDefs.Add(item);
					}
				}
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < tmpThingDefs.Count)
			{
				_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass10_0
				{
					thing = tmpThingDefs[_003Ci_003E5__2].thing
				};
				int stackCount = tmpThingDefs[_003Ci_003E5__2].stackCount;
				CS_0024_003C_003E8__locals8.label = tmpThingDefs[_003Ci_003E5__2].label.CapitalizeFirst() + ((stackCount > 1) ? (" x" + stackCount) : "");
				_003C_003E2__current = new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect rect)
					{
						Widgets.DrawHighlight(rect);
						Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
						if (Mouse.IsOver(rect))
						{
							Widgets.DrawHighlight(rect);
							TaggedString taggedString = CS_0024_003C_003E8__locals8.thing.thing.DescriptionDetailed + "\n\n" + "ClickForMoreInfo".Translate().Colorize(ColoredText.SubtleGrayColor);
							TooltipHandler.TipRegion(rect, taggedString);
						}
						Widgets.ThingIcon(new Rect(rect2.x, rect2.y, 22f, 22f), CS_0024_003C_003E8__locals8.thing.thing, CS_0024_003C_003E8__locals8.thing.stuff, null, 1f, null, null);
						Rect rect3 = rect2;
						rect3.xMin += 24f;
						Widgets.Label(rect3, CS_0024_003C_003E8__locals8.label);
						if (Widgets.ButtonInvisible(rect))
						{
							Find.WindowStack.Add(new Dialog_InfoCard(CS_0024_003C_003E8__locals8.thing.thing, CS_0024_003C_003E8__locals8.thing.stuff));
						}
					},
					width = Text.CalcSize(CS_0024_003C_003E8__locals8.label).x + 12f + 22f + 2f
				};
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GenUI.AnonymousStackElement> IEnumerable<GenUI.AnonymousStackElement>.GetEnumerator()
		{
			_003CGetRewardStackElementsForThings_003Ed__10 _003CGetRewardStackElementsForThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRewardStackElementsForThings_003Ed__ = this;
			}
			else
			{
				_003CGetRewardStackElementsForThings_003Ed__ = new _003CGetRewardStackElementsForThings_003Ed__10(0);
			}
			_003CGetRewardStackElementsForThings_003Ed__.thingDefs = _003C_003E3__thingDefs;
			return _003CGetRewardStackElementsForThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GenUI.AnonymousStackElement>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetRewardStackElementsForThings_003Ed__8 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool detailsHidden;

		public bool _003C_003E3__detailsHidden;

		private IEnumerable<Thing> things;

		public IEnumerable<Thing> _003C_003E3__things;

		private _003C_003Ec__DisplayClass8_0 _003C_003E8__1;

		private int _003Ci_003E5__2;

		GenUI.AnonymousStackElement IEnumerator<GenUI.AnonymousStackElement>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetRewardStackElementsForThings_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass8_0();
				_003C_003E8__1.detailsHidden = detailsHidden;
				tmpThings.Clear();
				foreach (Thing item in things)
				{
					bool flag = false;
					for (int i = 0; i < tmpThings.Count; i++)
					{
						if (tmpThings[i].First.CanStackWith(item))
						{
							tmpThings[i] = new Pair<Thing, int>(tmpThings[i].First, tmpThings[i].Second + item.stackCount);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						tmpThings.Add(new Pair<Thing, int>(item, item.stackCount));
					}
				}
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < tmpThings.Count)
			{
				_003C_003Ec__DisplayClass8_1 CS_0024_003C_003E8__locals12 = new _003C_003Ec__DisplayClass8_1
				{
					CS_0024_003C_003E8__locals1 = _003C_003E8__1,
					thing = tmpThings[_003Ci_003E5__2].First.GetInnerIfMinified()
				};
				int second = tmpThings[_003Ci_003E5__2].Second;
				if (CS_0024_003C_003E8__locals12.thing is Pawn pawn)
				{
					CS_0024_003C_003E8__locals12.label = "PawnQuestReward".Translate(pawn);
				}
				else
				{
					CS_0024_003C_003E8__locals12.label = CS_0024_003C_003E8__locals12.thing.LabelCapNoCount + ((second > 1) ? (" x" + second) : "");
				}
				_003C_003E2__current = new GenUI.AnonymousStackElement
				{
					drawer = delegate(Rect rect)
					{
						Widgets.DrawHighlight(rect);
						Rect rect2 = new Rect(rect.x + 6f, rect.y + 1f, rect.width - 12f, rect.height - 2f);
						if (Mouse.IsOver(rect))
						{
							Widgets.DrawHighlight(rect);
							TaggedString ts = (CS_0024_003C_003E8__locals12.CS_0024_003C_003E8__locals1.detailsHidden ? "NoMoreInfoAvailable".Translate() : "ClickForMoreInfo".Translate());
							ts = ts.Colorize(ColoredText.SubtleGrayColor);
							if (CS_0024_003C_003E8__locals12.thing is Pawn pawn2 && pawn2.RaceProps.Humanlike)
							{
								string text = pawn2.LabelCap;
								if (ModsConfig.BiotechActive && pawn2.genes != null)
								{
									text += (" (" + pawn2.genes.XenotypeLabel + ")").Colorize(ColoredText.SubtleGrayColor);
									text = text + "\n\n" + pawn2.genes.XenotypeDescShort;
								}
								SkillRecord skillRecord = pawn2.skills.skills.MaxBy((SkillRecord s) => (!s.TotallyDisabled) ? s.Level : (-1));
								string value = new StringBuilder().Append("BestSkillLabel".Translate().Colorize(ColoredText.TipSectionTitleColor)).Append(": ").Append(skillRecord.def.LabelCap.ToString())
									.Append(" (")
									.Append("BestSkillInfoLevel".Translate(skillRecord.Level).ToString())
									.Append(')')
									.ToString();
								TooltipHandler.TipRegion(rect, $"{text}\n\n{value}\n\n{ts}");
							}
							else
							{
								TooltipHandler.TipRegion(rect, CS_0024_003C_003E8__locals12.thing.DescriptionDetailed + "\n\n" + ts);
							}
						}
						Widgets.ThingIcon(new Rect(rect2.x, rect2.y, 22f, 22f), CS_0024_003C_003E8__locals12.thing, 1f, null);
						Rect rect3 = rect2;
						rect3.xMin += 24f;
						Widgets.Label(rect3, CS_0024_003C_003E8__locals12.label);
						if (Widgets.ButtonInvisible(rect))
						{
							if (CS_0024_003C_003E8__locals12.CS_0024_003C_003E8__locals1.detailsHidden)
							{
								Messages.Message("NoMoreInfoAvailable".Translate(), MessageTypeDefOf.RejectInput, historical: false);
							}
							else
							{
								Find.WindowStack.Add(new Dialog_InfoCard(CS_0024_003C_003E8__locals12.thing));
							}
						}
					},
					width = Text.CalcSize(CS_0024_003C_003E8__locals12.label).x + 12f + 22f + 2f
				};
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GenUI.AnonymousStackElement> IEnumerable<GenUI.AnonymousStackElement>.GetEnumerator()
		{
			_003CGetRewardStackElementsForThings_003Ed__8 _003CGetRewardStackElementsForThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRewardStackElementsForThings_003Ed__ = this;
			}
			else
			{
				_003CGetRewardStackElementsForThings_003Ed__ = new _003CGetRewardStackElementsForThings_003Ed__8(0);
			}
			_003CGetRewardStackElementsForThings_003Ed__.things = _003C_003E3__things;
			_003CGetRewardStackElementsForThings_003Ed__.detailsHidden = _003C_003E3__detailsHidden;
			return _003CGetRewardStackElementsForThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GenUI.AnonymousStackElement>)this).GetEnumerator();
		}
	}

	public const int RewardStackElementIconSize = 22;

	public const int RewardStackElementMarginHorizontal = 6;

	public const int RewardStackElementMarginVertical = 1;

	public const int RewardStackElementIconGap = 2;

	private static List<Pair<Thing, int>> tmpThings = new List<Pair<Thing, int>>();

	private static List<Reward_Items.RememberedItem> tmpThingDefs = new List<Reward_Items.RememberedItem>();

	public static T MakeAndAddEndCondition<T>(Quest quest, string inSignalActivate, QuestEndOutcome outcome, Letter letter = null) where T : QuestPartActivable, new()
	{
		T val = new T
		{
			inSignalEnable = inSignalActivate
		};
		quest.AddPart(val);
		if (letter != null)
		{
			QuestPart_Letter questPart_Letter = new QuestPart_Letter();
			questPart_Letter.letter = letter;
			questPart_Letter.inSignal = val.OutSignalCompleted;
			quest.AddPart(questPart_Letter);
		}
		QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
		questPart_QuestEnd.inSignal = val.OutSignalCompleted;
		questPart_QuestEnd.outcome = outcome;
		quest.AddPart(questPart_QuestEnd);
		return val;
	}

	public static QuestPart_QuestEnd MakeAndAddEndNodeWithLetter(Quest quest, string inSignalActivate, QuestEndOutcome outcome, Letter letter)
	{
		QuestPart_Letter questPart_Letter = new QuestPart_Letter();
		questPart_Letter.letter = letter;
		questPart_Letter.inSignal = inSignalActivate;
		quest.AddPart(questPart_Letter);
		QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
		questPart_QuestEnd.inSignal = inSignalActivate;
		questPart_QuestEnd.outcome = outcome;
		quest.AddPart(questPart_QuestEnd);
		return questPart_QuestEnd;
	}

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
		string text = "Quest" + quest.id + ".DelayingWorldObject";
		QuestUtility.AddQuestTag(ref worldObject.questTags, text);
		questPart_WorldObjectTimeout.inSignalDisable = text + ".MapGenerated";
		QuestPart_QuestEnd questPart_QuestEnd = new QuestPart_QuestEnd();
		questPart_QuestEnd.inSignal = questPart_WorldObjectTimeout.OutSignalCompleted;
		quest.AddPart(questPart_QuestEnd);
		return questPart_WorldObjectTimeout;
	}

	[IteratorStateMachine(typeof(_003CGetRewardStackElementsForThings_003Ed__8))]
	public static IEnumerable<GenUI.AnonymousStackElement> GetRewardStackElementsForThings(IEnumerable<Thing> things, bool detailsHidden = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRewardStackElementsForThings_003Ed__8(-2)
		{
			_003C_003E3__things = things,
			_003C_003E3__detailsHidden = detailsHidden
		};
	}

	[IteratorStateMachine(typeof(_003CGetRewardStackElementsForThings_003Ed__10))]
	public static IEnumerable<GenUI.AnonymousStackElement> GetRewardStackElementsForThings(IEnumerable<Reward_Items.RememberedItem> thingDefs)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRewardStackElementsForThings_003Ed__10(-2)
		{
			_003C_003E3__thingDefs = thingDefs
		};
	}

	public static GenUI.AnonymousStackElement GetStandardRewardStackElement(string label, Texture2D icon, Func<string> tipGetter, Action onClick = null)
	{
		return GetStandardRewardStackElement(label, delegate(Rect r)
		{
			GUI.DrawTexture(r, icon);
		}, tipGetter, onClick);
	}

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
						TooltipHandler.TipRegion(rect, new TipSignal(tipGetter, 0x56AAA7E ^ (int)rect.x ^ (int)rect.y));
					}
				}
				Rect obj = new Rect(rect2.x, rect2.y, 22f, 22f);
				iconDrawer(obj);
				Rect rect3 = rect2;
				rect3.xMin += 24f;
				Widgets.Label(rect3, label);
				if (onClick != null && Widgets.ButtonInvisible(rect))
				{
					onClick();
				}
			},
			width = Text.CalcSize(label).x + 12f + 22f + 2f
		};
	}
}
