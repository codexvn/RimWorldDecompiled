using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public static class SettlementProximityGoodwillUtility
{
	[CompilerGenerated]
	private sealed class _003CGetConfirmationDescriptions_003Ed__8 : IEnumerable<TaggedString>, IEnumerable, IEnumerator<TaggedString>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TaggedString _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private PlanetTile tile;

		public PlanetTile _003C_003E3__tile;

		private Building_GravEngine gravEngine;

		public Building_GravEngine _003C_003E3__gravEngine;

		private IEnumerator<BiomeDef> _003C_003E7__wrap1;

		private IEnumerator<TaggedString> _003C_003E7__wrap2;

		TaggedString IEnumerator<TaggedString>.Current
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
		public _003CGetConfirmationDescriptions_003Ed__8(int _003C_003E1__state)
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
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 8:
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
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				float mtb;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					Settlement settlement = Find.WorldObjects.SettlementAt(tile);
					if (ModsConfig.OdysseyActive && gravEngine != null && settlement != null && !settlement.Faction.IsPlayer)
					{
						if (settlement.Faction.HostileTo(Faction.OfPlayer))
						{
							_003C_003E2__current = "ConfirmLandOnHostileFactionBase".Translate(settlement.Faction);
							_003C_003E1__state = 1;
							return true;
						}
						_003C_003E2__current = "ConfirmLandOnNeutralFactionBase".Translate(settlement.Faction);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ec;
				}
				case 1:
					_003C_003E1__state = -1;
					goto IL_00ec;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ec;
				case 3:
					_003C_003E1__state = -1;
					goto IL_014c;
				case 4:
					_003C_003E1__state = -1;
					goto IL_018d;
				case 5:
					_003C_003E1__state = -3;
					goto IL_01f5;
				case 6:
					_003C_003E1__state = -1;
					if (gravEngine != null)
					{
						_003C_003E2__current = "OrbitalWarning_Gravship".Translate();
						_003C_003E1__state = 7;
						return true;
					}
					break;
				case 7:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = gravEngine.GetOrbitalWarnings().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_02c8;
				case 8:
					{
						_003C_003E1__state = -4;
						goto IL_02c8;
					}
					IL_02c8:
					if (_003C_003E7__wrap2.MoveNext())
					{
						TaggedString current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 8;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					break;
					IL_018d:
					_003C_003E7__wrap1 = tile.Tile.Biomes.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01f5;
					IL_014c:
					if (ModsConfig.BiotechActive && NoxiousHazeUtility.TryGetNoxiousHazeMTB(tile, out mtb))
					{
						_003C_003E2__current = "ConfirmSettleNearPollution".Translate(mtb);
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_018d;
					IL_01f5:
					while (_003C_003E7__wrap1.MoveNext())
					{
						BiomeDef current2 = _003C_003E7__wrap1.Current;
						if (!current2.settleWarning.NullOrEmpty())
						{
							_003C_003E2__current = current2.settleWarning;
							_003C_003E1__state = 5;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (ModsConfig.OdysseyActive && tile.LayerDef == PlanetLayerDefOf.Orbit)
					{
						_003C_003E2__current = "OrbitalWarning".Translate();
						_003C_003E1__state = 6;
						return true;
					}
					break;
					IL_00ec:
					tmpGoodwillOffsets.Clear();
					AppendProximityGoodwillOffsets(tile, tmpGoodwillOffsets, ignoreIfAlreadyMinGoodwill: false, ignorePermanentlyHostile: true);
					if (tmpGoodwillOffsets.Any())
					{
						_003C_003E2__current = "ConfirmSettleNearFactionBase".Translate(MaxDist - 1, 15);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_014c;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<TaggedString> IEnumerable<TaggedString>.GetEnumerator()
		{
			_003CGetConfirmationDescriptions_003Ed__8 _003CGetConfirmationDescriptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetConfirmationDescriptions_003Ed__ = this;
			}
			else
			{
				_003CGetConfirmationDescriptions_003Ed__ = new _003CGetConfirmationDescriptions_003Ed__8(0);
			}
			_003CGetConfirmationDescriptions_003Ed__.tile = _003C_003E3__tile;
			_003CGetConfirmationDescriptions_003Ed__.gravEngine = _003C_003E3__gravEngine;
			return _003CGetConfirmationDescriptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TaggedString>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetConfirmationEffects_003Ed__9 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool gravship;

		public bool _003C_003E3__gravship;

		private PlanetTile tile;

		public PlanetTile _003C_003E3__tile;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CGetConfirmationEffects_003Ed__9(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					tmpGoodwillOffsets.Clear();
					if (gravship && ModsConfig.OdysseyActive)
					{
						AppendGravshipAttackOffsets(tile, tmpGoodwillOffsets, ignoreIfAlreadyMinGoodwill: false, ignorePermanentlyHostile: true);
					}
					AppendProximityGoodwillOffsets(tile, tmpGoodwillOffsets, ignoreIfAlreadyMinGoodwill: false, ignorePermanentlyHostile: true);
					SortProximityGoodwillOffsets(tmpGoodwillOffsets);
					_003C_003E7__wrap1 = ProximityGoodwillOffsetsToString(tmpGoodwillOffsets).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					string current = _003C_003E7__wrap1.Current;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetConfirmationEffects_003Ed__9 _003CGetConfirmationEffects_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetConfirmationEffects_003Ed__ = this;
			}
			else
			{
				_003CGetConfirmationEffects_003Ed__ = new _003CGetConfirmationEffects_003Ed__9(0);
			}
			_003CGetConfirmationEffects_003Ed__.tile = _003C_003E3__tile;
			_003CGetConfirmationEffects_003Ed__.gravship = _003C_003E3__gravship;
			return _003CGetConfirmationEffects_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CProximityGoodwillOffsetsToString_003Ed__7 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Pair<Settlement, int>> offsets;

		public List<Pair<Settlement, int>> _003C_003E3__offsets;

		private int _003Ci_003E5__2;

		string IEnumerator<string>.Current
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
		public _003CProximityGoodwillOffsetsToString_003Ed__7(int _003C_003E1__state)
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
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < offsets.Count)
			{
				_003C_003E2__current = offsets[_003Ci_003E5__2].First.LabelCap + ": " + "ProximitySingleGoodwillChange".Translate(offsets[_003Ci_003E5__2].Second.ToStringWithSign(), offsets[_003Ci_003E5__2].First.Faction.Name);
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CProximityGoodwillOffsetsToString_003Ed__7 _003CProximityGoodwillOffsetsToString_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CProximityGoodwillOffsetsToString_003Ed__ = this;
			}
			else
			{
				_003CProximityGoodwillOffsetsToString_003Ed__ = new _003CProximityGoodwillOffsetsToString_003Ed__7(0);
			}
			_003CProximityGoodwillOffsetsToString_003Ed__.offsets = _003C_003E3__offsets;
			return _003CProximityGoodwillOffsetsToString_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private static readonly List<Pair<Settlement, int>> tmpGoodwillOffsets = new List<Pair<Settlement, int>>();

	public static int MaxDist => Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Last().x);

	public static void CheckSettlementProximityGoodwillChange()
	{
		if (Find.TickManager.TicksGame == 0 || Find.TickManager.TicksGame % 900000 != 0)
		{
			return;
		}
		List<Settlement> settlements = Find.WorldObjects.Settlements;
		tmpGoodwillOffsets.Clear();
		for (int i = 0; i < settlements.Count; i++)
		{
			Settlement settlement = settlements[i];
			if (settlement.Faction == Faction.OfPlayer)
			{
				AppendProximityGoodwillOffsets(settlement.Tile, tmpGoodwillOffsets, ignoreIfAlreadyMinGoodwill: true, ignorePermanentlyHostile: false);
			}
		}
		if (!tmpGoodwillOffsets.Any())
		{
			return;
		}
		SortProximityGoodwillOffsets(tmpGoodwillOffsets);
		List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
		bool flag = false;
		TaggedString text = "LetterFactionBaseProximity".Translate() + "\n\n" + ProximityGoodwillOffsetsToString(tmpGoodwillOffsets).ToLineList(" - ");
		for (int j = 0; j < allFactionsListForReading.Count; j++)
		{
			Faction faction = allFactionsListForReading[j];
			if (faction == Faction.OfPlayer)
			{
				continue;
			}
			int num = 0;
			for (int k = 0; k < tmpGoodwillOffsets.Count; k++)
			{
				if (tmpGoodwillOffsets[k].First.Faction == faction)
				{
					num += tmpGoodwillOffsets[k].Second;
				}
			}
			if (num != 0)
			{
				FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
				Faction.OfPlayer.TryAffectGoodwillWith(faction, num, canSendMessage: false, canSendHostilityLetter: false, HistoryEventDefOf.SettlementProximity, null);
				flag = true;
				faction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, faction.PlayerRelationKind);
			}
		}
		if (flag)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelFactionBaseProximity".Translate(), text, LetterDefOf.NegativeEvent);
		}
	}

	public static void AppendProximityGoodwillOffsets(PlanetTile tile, List<Pair<Settlement, int>> outOffsets, bool ignoreIfAlreadyMinGoodwill, bool ignorePermanentlyHostile)
	{
		int maxDist = MaxDist;
		List<Settlement> settlements = Find.WorldObjects.Settlements;
		for (int i = 0; i < settlements.Count; i++)
		{
			Settlement settlement = settlements[i];
			if (settlement.Faction == null || settlement.Faction == Faction.OfPlayer || (ignorePermanentlyHostile && settlement.Faction.def.permanentEnemy) || (ignoreIfAlreadyMinGoodwill && settlement.Faction.PlayerGoodwill == -100) || tile == settlement.Tile)
			{
				continue;
			}
			int num = Find.WorldGrid.TraversalDistanceBetween(tile, settlement.Tile, passImpassable: false, maxDist);
			if (num != int.MaxValue)
			{
				float x = (float)num * settlement.Tile.LayerDef.rangeDistanceFactor;
				int num2 = Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Evaluate(x));
				if (num2 != 0)
				{
					outOffsets.Add(new Pair<Settlement, int>(settlement, num2));
				}
			}
		}
	}

	private static void AppendGravshipAttackOffsets(PlanetTile tile, List<Pair<Settlement, int>> outOffsets, bool ignoreIfAlreadyMinGoodwill, bool ignorePermanentlyHostile)
	{
		Settlement settlement = Find.WorldObjects.SettlementAt(tile);
		if (settlement != null && !settlement.Faction.IsPlayer && (!ignorePermanentlyHostile || !settlement.Faction.def.permanentEnemy) && (!ignoreIfAlreadyMinGoodwill || settlement.Faction.PlayerGoodwill != -100))
		{
			outOffsets.Add(new Pair<Settlement, int>(settlement, Faction.OfPlayer.GoodwillToMakeHostile(settlement.Faction)));
		}
	}

	public static void SortProximityGoodwillOffsets(List<Pair<Settlement, int>> offsets)
	{
		offsets.SortBy((Pair<Settlement, int> x) => x.First.Faction.loadID, (Pair<Settlement, int> x) => -Mathf.Abs(x.Second));
	}

	[IteratorStateMachine(typeof(_003CProximityGoodwillOffsetsToString_003Ed__7))]
	public static IEnumerable<string> ProximityGoodwillOffsetsToString(List<Pair<Settlement, int>> offsets)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CProximityGoodwillOffsetsToString_003Ed__7(-2)
		{
			_003C_003E3__offsets = offsets
		};
	}

	[IteratorStateMachine(typeof(_003CGetConfirmationDescriptions_003Ed__8))]
	private static IEnumerable<TaggedString> GetConfirmationDescriptions(PlanetTile tile, Building_GravEngine gravEngine = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConfirmationDescriptions_003Ed__8(-2)
		{
			_003C_003E3__tile = tile,
			_003C_003E3__gravEngine = gravEngine
		};
	}

	[IteratorStateMachine(typeof(_003CGetConfirmationEffects_003Ed__9))]
	private static IEnumerable<string> GetConfirmationEffects(PlanetTile tile, bool gravship)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConfirmationEffects_003Ed__9(-2)
		{
			_003C_003E3__tile = tile,
			_003C_003E3__gravship = gravship
		};
	}

	public static void CheckConfirmSettle(PlanetTile tile, Action settleAction, Action cancelAction = null, Building_GravEngine gravEngine = null)
	{
		IEnumerable<TaggedString> confirmationDescriptions = GetConfirmationDescriptions(tile, gravEngine);
		if (confirmationDescriptions.Any())
		{
			TaggedString text = new TaggedString("");
			foreach (TaggedString item in confirmationDescriptions)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += item;
			}
			if (gravEngine != null)
			{
				text += "\n\n" + "ConfirmLand".Translate();
			}
			else
			{
				text += "\n\n" + "ConfirmSettle".Translate();
			}
			IEnumerable<string> confirmationEffects = GetConfirmationEffects(tile, gravEngine != null);
			if (confirmationEffects.Any())
			{
				text += "\n\n" + confirmationEffects.ToLineList(" - ");
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, settleAction, cancelAction));
		}
		else
		{
			settleAction();
		}
	}
}
