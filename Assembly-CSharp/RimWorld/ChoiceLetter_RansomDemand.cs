using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ChoiceLetter_RansomDemand : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__5 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_RansomDemand _003C_003E4__this;

		DiaOption IEnumerator<DiaOption>.Current
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
		public _003Cget_Choices_003Ed__5(int _003C_003E1__state)
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
			ChoiceLetter_RansomDemand CS_0024_003C_003E8__locals26 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals26.ArchivedOnly)
				{
					_003C_003E2__current = CS_0024_003C_003E8__locals26.Option_Close;
					_003C_003E1__state = 1;
					return true;
				}
				DiaOption diaOption = new DiaOption("RansomDemand_Accept".Translate())
				{
					action = delegate
					{
						CS_0024_003C_003E8__locals26.faction.kidnapped.RemoveKidnappedPawn(CS_0024_003C_003E8__locals26.kidnapped);
						Find.WorldPawns.RemovePawn(CS_0024_003C_003E8__locals26.kidnapped);
						IntVec3 result;
						if ((int)CS_0024_003C_003E8__locals26.faction.def.techLevel < 4)
						{
							if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(CS_0024_003C_003E8__locals26.map) && CS_0024_003C_003E8__locals26.map.reachability.CanReachColony(c), CS_0024_003C_003E8__locals26.map, CellFinder.EdgeRoadChance_Friendly, out result) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(CS_0024_003C_003E8__locals26.map), CS_0024_003C_003E8__locals26.map, CellFinder.EdgeRoadChance_Friendly, out result))
							{
								Log.Warning("Could not find any edge cell.");
								result = DropCellFinder.TradeDropSpot(CS_0024_003C_003E8__locals26.map);
							}
							GenSpawn.Spawn(CS_0024_003C_003E8__locals26.kidnapped, result, CS_0024_003C_003E8__locals26.map);
						}
						else
						{
							result = DropCellFinder.TradeDropSpot(CS_0024_003C_003E8__locals26.map);
							TradeUtility.SpawnDropPod(result, CS_0024_003C_003E8__locals26.map, CS_0024_003C_003E8__locals26.kidnapped);
						}
						CameraJumper.TryJump(result, CS_0024_003C_003E8__locals26.map);
						TradeUtility.LaunchSilver(CS_0024_003C_003E8__locals26.map, CS_0024_003C_003E8__locals26.fee);
						Find.LetterStack.RemoveLetter(CS_0024_003C_003E8__locals26);
					},
					resolveTree = true
				};
				if (!TradeUtility.ColonyHasEnoughSilver(CS_0024_003C_003E8__locals26.map, CS_0024_003C_003E8__locals26.fee))
				{
					diaOption.Disable("NeedSilverLaunchable".Translate(CS_0024_003C_003E8__locals26.fee.ToString()));
				}
				_003C_003E2__current = diaOption;
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals26.Option_Reject;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals26.Option_Postpone;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
		{
			_003Cget_Choices_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DiaOption>)this).GetEnumerator();
		}
	}

	public Map map;

	public Faction faction;

	public Pawn kidnapped;

	public int fee;

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__5(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool CanShowInLetterStack
	{
		get
		{
			if (!base.CanShowInLetterStack)
			{
				return false;
			}
			if (!Find.Maps.Contains(map))
			{
				return false;
			}
			return faction.kidnapped.KidnappedPawnsListForReading.Contains(kidnapped);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref map, "map");
		Scribe_References.Look(ref faction, "faction");
		Scribe_References.Look(ref kidnapped, "kidnapped");
		Scribe_Values.Look(ref fee, "fee", 0);
	}
}
