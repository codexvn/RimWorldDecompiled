using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class Reward_Items : Reward
{
	public struct RememberedItem : IExposable
	{
		public ThingStuffPairWithQuality thing;

		public int stackCount;

		public string label;

		public RememberedItem(ThingStuffPairWithQuality thing, int stackCount, string label)
		{
			this.thing = thing;
			this.stackCount = stackCount;
			this.label = label;
		}

		public void ExposeData()
		{
			Scribe_Deep.Look(ref thing, "thing");
			Scribe_Values.Look(ref stackCount, "stackCount", 0);
			Scribe_Values.Look(ref label, "label");
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass15_0
	{
		public QuestPart_DropPods dropPods;

		internal void _003CGenerateQuestParts_003Eb__0(string x)
		{
			dropPods.customLetterLabel = x;
		}

		internal void _003CGenerateQuestParts_003Eb__1(string x)
		{
			dropPods.customLetterText = x;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGenerateQuestParts_003Ed__15 : IEnumerable<QuestPart>, IEnumerable, IEnumerator<QuestPart>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private QuestPart _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_Items _003C_003E4__this;

		private RewardsGeneratorParams parms;

		public RewardsGeneratorParams _003C_003E3__parms;

		private string customLetterLabel;

		public string _003C_003E3__customLetterLabel;

		private RulePack customLetterLabelRules;

		public RulePack _003C_003E3__customLetterLabelRules;

		private string customLetterText;

		public string _003C_003E3__customLetterText;

		private RulePack customLetterTextRules;

		public RulePack _003C_003E3__customLetterTextRules;

		private Slate _003Cslate_003E5__2;

		QuestPart IEnumerator<QuestPart>.Current
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
		public _003CGenerateQuestParts_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cslate_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Reward_Items reward_Items = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003Cslate_003E5__2 = RimWorld.QuestGen.QuestGen.slate;
				for (int i = 0; i < reward_Items.items.Count; i++)
				{
					if (reward_Items.items[i] is Pawn pawn)
					{
						RimWorld.QuestGen.QuestGen.AddToGeneratedPawns(pawn);
						if (!pawn.IsWorldPawn())
						{
							Find.WorldPawns.PassToWorld(pawn);
						}
					}
				}
				if (parms.giveToCaravan)
				{
					_003C_003E2__current = new QuestPart_GiveToCaravan
					{
						inSignal = _003Cslate_003E5__2.Get<string>("inSignal"),
						Things = reward_Items.items
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Ec__DisplayClass15_0 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass15_0();
				CS_0024_003C_003E8__locals8.dropPods = new QuestPart_DropPods();
				CS_0024_003C_003E8__locals8.dropPods.inSignal = _003Cslate_003E5__2.Get<string>("inSignal");
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					RimWorld.QuestGen.QuestGen.AddTextRequest("root", delegate(string x)
					{
						CS_0024_003C_003E8__locals8.dropPods.customLetterLabel = x;
					}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					RimWorld.QuestGen.QuestGen.AddTextRequest("root", delegate(string x)
					{
						CS_0024_003C_003E8__locals8.dropPods.customLetterText = x;
					}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				CS_0024_003C_003E8__locals8.dropPods.mapParent = _003Cslate_003E5__2.Get<Map>("map").Parent;
				CS_0024_003C_003E8__locals8.dropPods.useTradeDropSpot = true;
				CS_0024_003C_003E8__locals8.dropPods.Things = reward_Items.items;
				_003C_003E2__current = CS_0024_003C_003E8__locals8.dropPods;
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			_003Cslate_003E5__2.Set("itemsReward_items", reward_Items.items);
			_003Cslate_003E5__2.Set("itemsReward_totalMarketValue", reward_Items.TotalMarketValue);
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
		IEnumerator<QuestPart> IEnumerable<QuestPart>.GetEnumerator()
		{
			_003CGenerateQuestParts_003Ed__15 _003CGenerateQuestParts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateQuestParts_003Ed__ = this;
			}
			else
			{
				_003CGenerateQuestParts_003Ed__ = new _003CGenerateQuestParts_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGenerateQuestParts_003Ed__.parms = _003C_003E3__parms;
			_003CGenerateQuestParts_003Ed__.customLetterLabel = _003C_003E3__customLetterLabel;
			_003CGenerateQuestParts_003Ed__.customLetterText = _003C_003E3__customLetterText;
			_003CGenerateQuestParts_003Ed__.customLetterLabelRules = _003C_003E3__customLetterLabelRules;
			_003CGenerateQuestParts_003Ed__.customLetterTextRules = _003C_003E3__customLetterTextRules;
			return _003CGenerateQuestParts_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<QuestPart>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_StackElements_003Ed__8 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_Items _003C_003E4__this;

		private IEnumerator<GenUI.AnonymousStackElement> _003C_003E7__wrap1;

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
		public _003Cget_StackElements_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Reward_Items reward_Items = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (reward_Items.usedOrCleanedUp)
					{
						_003C_003E7__wrap1 = QuestPartUtility.GetRewardStackElementsForThings(reward_Items.itemDefs).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_007f;
					}
					_003C_003E7__wrap1 = QuestPartUtility.GetRewardStackElementsForThings(reward_Items.items).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00e4;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007f;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00e4;
					}
					IL_007f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GenUI.AnonymousStackElement current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					break;
					IL_00e4:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GenUI.AnonymousStackElement current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GenUI.AnonymousStackElement> IEnumerable<GenUI.AnonymousStackElement>.GetEnumerator()
		{
			_003Cget_StackElements_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_StackElements_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GenUI.AnonymousStackElement>)this).GetEnumerator();
		}
	}

	public List<Thing> items = new List<Thing>();

	private List<RememberedItem> itemDefs = new List<RememberedItem>();

	private float lastTotalMarketValue;

	private const string RootSymbol = "root";

	public List<Thing> ItemsListForReading => items;

	public override IEnumerable<GenUI.AnonymousStackElement> StackElements
	{
		[IteratorStateMachine(typeof(_003Cget_StackElements_003Ed__8))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_StackElements_003Ed__8(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override float TotalMarketValue
	{
		get
		{
			if (usedOrCleanedUp)
			{
				return lastTotalMarketValue;
			}
			float num = 0f;
			for (int i = 0; i < items.Count; i++)
			{
				Thing innerIfMinified = items[i].GetInnerIfMinified();
				num += innerIfMinified.MarketValue * (float)items[i].stackCount;
			}
			return num;
		}
	}

	public override void Notify_Used()
	{
		RememberItems();
		base.Notify_Used();
	}

	public override void Notify_PreCleanup()
	{
		RememberItems();
		base.Notify_PreCleanup();
	}

	private void RememberItems()
	{
		if (usedOrCleanedUp)
		{
			return;
		}
		itemDefs.Clear();
		lastTotalMarketValue = 0f;
		for (int i = 0; i < items.Count; i++)
		{
			Thing innerIfMinified = items[i].GetInnerIfMinified();
			if (innerIfMinified != null && !innerIfMinified.Destroyed)
			{
				if (!innerIfMinified.TryGetQuality(out var qc))
				{
					qc = QualityCategory.Normal;
				}
				itemDefs.Add(new RememberedItem(new ThingStuffPairWithQuality(innerIfMinified.def, innerIfMinified.Stuff, qc), items[i].stackCount, items[i].LabelNoCount));
				lastTotalMarketValue += innerIfMinified.MarketValue * (float)items[i].stackCount;
			}
		}
	}

	public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
	{
		items.Clear();
		bool flag = true;
		float x2 = (Find.TickManager.TicksGame - Find.History.lastPsylinkAvailable).TicksToDays();
		if (Rand.Chance(QuestTuning.DaysSincePsylinkAvailableToGuaranteedNeuroformerChance.Evaluate(x2)) && ModsConfig.RoyaltyActive && (parms.disallowedThingDefs == null || !parms.disallowedThingDefs.Contains(ThingDefOf.PsychicAmplifier)) && rewardValue >= 600f && (Faction.OfEmpire == null || parms.giverFaction != Faction.OfEmpire))
		{
			items.Add(ThingMaker.MakeThing(ThingDefOf.PsychicAmplifier));
			rewardValue -= items[0].MarketValue;
			if (rewardValue < 100f)
			{
				flag = false;
			}
		}
		if (flag)
		{
			FloatRange value = rewardValue * new FloatRange(0.7f, 1.3f);
			ThingSetMakerParams parms2 = default(ThingSetMakerParams);
			parms2.totalMarketValueRange = value;
			parms2.makingFaction = parms.giverFaction;
			if (!parms.disallowedThingDefs.NullOrEmpty())
			{
				parms2.validator = (ThingDef x) => !parms.disallowedThingDefs.Contains(x);
			}
			items.AddRange(ThingSetMakerDefOf.Reward_ItemsStandard.root.Generate(parms2));
		}
		valueActuallyUsed = TotalMarketValue;
	}

	[IteratorStateMachine(typeof(_003CGenerateQuestParts_003Ed__15))]
	public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateQuestParts_003Ed__15(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parms = parms,
			_003C_003E3__customLetterLabel = customLetterLabel,
			_003C_003E3__customLetterText = customLetterText,
			_003C_003E3__customLetterLabelRules = customLetterLabelRules,
			_003C_003E3__customLetterTextRules = customLetterTextRules
		};
	}

	public override string GetDescription(RewardsGeneratorParams parms)
	{
		if (parms.giveToCaravan)
		{
			return "Reward_Items_Caravan".Translate(GenLabel.ThingsLabel(items), TotalMarketValue.ToStringMoney());
		}
		return "Reward_Items".Translate(GenLabel.ThingsLabel(items), TotalMarketValue.ToStringMoney());
	}

	public override string ToString()
	{
		string name = GetType().Name;
		name = name + "(value " + TotalMarketValue.ToStringMoney() + ")";
		foreach (Thing item in items)
		{
			name = name + "\n  -" + item.LabelCap + " " + (item.MarketValue * (float)item.stackCount).ToStringMoney();
		}
		return name;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref items, "items", LookMode.Reference);
		Scribe_Collections.Look(ref itemDefs, "itemDefs", LookMode.Deep);
		Scribe_Values.Look(ref lastTotalMarketValue, "lastTotalMarketValue", 0f);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			items.RemoveAll((Thing x) => x == null);
		}
	}
}
