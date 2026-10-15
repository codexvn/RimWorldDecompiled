using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

[StaticConstructorOnStartup]
public class TradeRequestComp : WorldObjectComp
{
	[CompilerGenerated]
	private sealed class _003CGetCaravanGizmos_003Ed__8 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TradeRequestComp _003C_003E4__this;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetCaravanGizmos_003Ed__8(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			TradeRequestComp tradeRequestComp = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tradeRequestComp.ActiveRequest && CaravanVisitUtility.SettlementVisitedNow(caravan) == tradeRequestComp.parent)
				{
					_003C_003E2__current = tradeRequestComp.FulfillRequestCommand(caravan);
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetCaravanGizmos_003Ed__8 _003CGetCaravanGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCaravanGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetCaravanGizmos_003Ed__ = new _003CGetCaravanGizmos_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetCaravanGizmos_003Ed__.caravan = _003C_003E3__caravan;
			return _003CGetCaravanGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public ThingDef requestThingDef;

	public int requestCount;

	public int expiration = -1;

	public string outSignalFulfilled;

	private static readonly Texture2D TradeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/FulfillTradeRequest");

	public bool ActiveRequest => expiration > Find.TickManager.TicksGame;

	public override string CompInspectStringExtra()
	{
		if (ActiveRequest)
		{
			return "CaravanRequestInfo".Translate(TradeRequestUtility.RequestedThingLabel(requestThingDef, requestCount).CapitalizeFirst(), (expiration - Find.TickManager.TicksGame).ToStringTicksToDays(), (requestThingDef.GetStatValueAbstract(StatDefOf.MarketValue) * (float)requestCount).ToStringMoney());
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CGetCaravanGizmos_003Ed__8))]
	public override IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCaravanGizmos_003Ed__8(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__caravan = caravan
		};
	}

	public void Disable()
	{
		expiration = -1;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Defs.Look(ref requestThingDef, "requestThingDef");
		Scribe_Values.Look(ref requestCount, "requestCount", 0);
		Scribe_Values.Look(ref expiration, "expiration", 0);
		BackCompatibility.PostExposeData(this);
	}

	private Command FulfillRequestCommand(Caravan caravan)
	{
		Command_Action command_Action = new Command_Action();
		command_Action.defaultLabel = "CommandFulfillTradeOffer".Translate();
		command_Action.defaultDesc = "CommandFulfillTradeOfferDesc".Translate();
		command_Action.icon = TradeCommandTex;
		command_Action.action = delegate
		{
			if (!ActiveRequest)
			{
				Log.Error("Attempted to fulfill an unavailable request");
			}
			else if (!CaravanInventoryUtility.HasThings(caravan, requestThingDef, requestCount, PlayerCanGive))
			{
				Messages.Message("CommandFulfillTradeOfferFailInsufficient".Translate(TradeRequestUtility.RequestedThingLabel(requestThingDef, requestCount)), MessageTypeDefOf.RejectInput, historical: false);
			}
			else
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CommandFulfillTradeOfferConfirm".Translate(GenLabel.ThingLabel(requestThingDef, null, requestCount)), delegate
				{
					Fulfill(caravan);
				}));
			}
		};
		if (!CaravanInventoryUtility.HasThings(caravan, requestThingDef, requestCount, PlayerCanGive))
		{
			command_Action.Disable("CommandFulfillTradeOfferFailInsufficient".Translate(TradeRequestUtility.RequestedThingLabel(requestThingDef, requestCount)));
		}
		return command_Action;
	}

	private void Fulfill(Caravan caravan)
	{
		int remaining = requestCount;
		List<Thing> list = CaravanInventoryUtility.TakeThings(caravan, delegate(Thing thing)
		{
			if (requestThingDef != thing.def)
			{
				return 0;
			}
			if (!PlayerCanGive(thing))
			{
				return 0;
			}
			int num = Mathf.Min(remaining, thing.stackCount);
			remaining -= num;
			return num;
		});
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Destroy();
		}
		if (parent.Faction != null)
		{
			Faction.OfPlayer.TryAffectGoodwillWith(parent.Faction, 12, canSendMessage: true, canSendHostilityLetter: true, HistoryEventDefOf.QuestGoodwillReward, null);
		}
		QuestUtility.SendQuestTargetSignals(parent.questTags, "TradeRequestFulfilled", parent.Named("SUBJECT"), caravan.Named("CARAVAN"));
		Disable();
	}

	private bool PlayerCanGive(Thing thing)
	{
		if (thing.GetRotStage() != 0)
		{
			return false;
		}
		if (thing is Apparel { WornByCorpse: not false })
		{
			return false;
		}
		CompQuality compQuality = thing.TryGetComp<CompQuality>();
		if (compQuality != null && (int)compQuality.Quality < 2)
		{
			return false;
		}
		return true;
	}
}
