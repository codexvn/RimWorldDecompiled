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

public class Reward_Pawn : Reward
{
	public enum ArrivalMode
	{
		WalkIn,
		DropPod
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public QuestPart_PawnsArrive pawnsArrive;

		internal void _003CGenerateQuestParts_003Eb__0(string x)
		{
			pawnsArrive.customLetterLabel = x;
		}

		internal void _003CGenerateQuestParts_003Eb__1(string x)
		{
			pawnsArrive.customLetterText = x;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGenerateQuestParts_003Ed__8 : IEnumerable<QuestPart>, IEnumerable, IEnumerator<QuestPart>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private QuestPart _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_Pawn _003C_003E4__this;

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
		public _003CGenerateQuestParts_003Ed__8(int _003C_003E1__state)
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
			Reward_Pawn reward_Pawn = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Slate slate = RimWorld.QuestGen.QuestGen.slate;
				RimWorld.QuestGen.QuestGen.AddToGeneratedPawns(reward_Pawn.pawn);
				if (!reward_Pawn.pawn.IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(reward_Pawn.pawn);
				}
				if (parms.giveToCaravan)
				{
					_003C_003E2__current = new QuestPart_GiveToCaravan
					{
						inSignal = slate.Get<string>("inSignal"),
						Things = Gen.YieldSingle(reward_Pawn.pawn)
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Ec__DisplayClass8_0 CS_0024_003C_003E8__locals9 = new _003C_003Ec__DisplayClass8_0();
				CS_0024_003C_003E8__locals9.pawnsArrive = new QuestPart_PawnsArrive();
				CS_0024_003C_003E8__locals9.pawnsArrive.inSignal = slate.Get<string>("inSignal");
				CS_0024_003C_003E8__locals9.pawnsArrive.pawns.Add(reward_Pawn.pawn);
				CS_0024_003C_003E8__locals9.pawnsArrive.arrivalMode = ((reward_Pawn.arrivalMode == ArrivalMode.DropPod) ? PawnsArrivalModeDefOf.CenterDrop : PawnsArrivalModeDefOf.EdgeWalkIn);
				CS_0024_003C_003E8__locals9.pawnsArrive.joinPlayer = true;
				CS_0024_003C_003E8__locals9.pawnsArrive.mapParent = slate.Get<Map>("map").Parent;
				if (!customLetterLabel.NullOrEmpty() || customLetterLabelRules != null)
				{
					RimWorld.QuestGen.QuestGen.AddTextRequest("root", delegate(string x)
					{
						CS_0024_003C_003E8__locals9.pawnsArrive.customLetterLabel = x;
					}, QuestGenUtility.MergeRules(customLetterLabelRules, customLetterLabel, "root"));
				}
				if (!customLetterText.NullOrEmpty() || customLetterTextRules != null)
				{
					RimWorld.QuestGen.QuestGen.AddTextRequest("root", delegate(string x)
					{
						CS_0024_003C_003E8__locals9.pawnsArrive.customLetterText = x;
					}, QuestGenUtility.MergeRules(customLetterTextRules, customLetterText, "root"));
				}
				_003C_003E2__current = CS_0024_003C_003E8__locals9.pawnsArrive;
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
			_003CGenerateQuestParts_003Ed__8 _003CGenerateQuestParts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateQuestParts_003Ed__ = this;
			}
			else
			{
				_003CGenerateQuestParts_003Ed__ = new _003CGenerateQuestParts_003Ed__8(0)
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
	private sealed class _003Cget_StackElements_003Ed__6 : IEnumerable<GenUI.AnonymousStackElement>, IEnumerable, IEnumerator<GenUI.AnonymousStackElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenUI.AnonymousStackElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Reward_Pawn _003C_003E4__this;

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
		public _003Cget_StackElements_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Reward_Pawn reward_Pawn = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (reward_Pawn.pawn == null)
					{
						return false;
					}
					_003C_003E7__wrap1 = QuestPartUtility.GetRewardStackElementsForThings(Gen.YieldSingle(reward_Pawn.pawn), reward_Pawn.detailsHidden).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					GenUI.AnonymousStackElement current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GenUI.AnonymousStackElement> IEnumerable<GenUI.AnonymousStackElement>.GetEnumerator()
		{
			_003Cget_StackElements_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_StackElements_003Ed__6(0)
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

	public Pawn pawn;

	public ArrivalMode arrivalMode;

	public bool detailsHidden;

	private const string RootSymbol = "root";

	public override IEnumerable<GenUI.AnonymousStackElement> StackElements
	{
		[IteratorStateMachine(typeof(_003Cget_StackElements_003Ed__6))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_StackElements_003Ed__6(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
	{
		PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.SpaceRefugee, null, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, forceRecruitable: true);
		pawn = PawnGenerator.GeneratePawn(request);
		arrivalMode = ((!Rand.Bool) ? ArrivalMode.DropPod : ArrivalMode.WalkIn);
		valueActuallyUsed = rewardValue;
	}

	[IteratorStateMachine(typeof(_003CGenerateQuestParts_003Ed__8))]
	public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateQuestParts_003Ed__8(-2)
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
			return "Reward_Pawn_Caravan".Translate(pawn);
		}
		return arrivalMode switch
		{
			ArrivalMode.WalkIn => "Reward_Pawn_WalkIn".Translate(pawn), 
			ArrivalMode.DropPod => "Reward_Pawn_DropPod".Translate(pawn), 
			_ => throw new Exception("Unknown arrival mode: " + arrivalMode), 
		};
	}

	public override string ToString()
	{
		return GetType().Name + " (" + pawn.MarketValue.ToStringMoney() + " pawn=" + pawn.ToStringSafe() + ", arrivalMode=" + arrivalMode.ToString() + ")";
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref pawn, "pawn", saveDestroyedThings: true);
		Scribe_Values.Look(ref arrivalMode, "arrivalMode", ArrivalMode.WalkIn);
		Scribe_Values.Look(ref detailsHidden, "detailsHidden", defaultValue: false);
	}
}
