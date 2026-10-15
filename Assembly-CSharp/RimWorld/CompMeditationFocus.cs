using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class CompMeditationFocus : CompStatOffsetBase
{
	[CompilerGenerated]
	private sealed class _003CGetExplanation_003Ed__6 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompMeditationFocus _003C_003E4__this;

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
		public _003CGetExplanation_003Ed__6(int _003C_003E1__state)
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
			CompMeditationFocus compMeditationFocus = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0068;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_0078;
			IL_0068:
			_003Ci_003E5__2++;
			goto IL_0078;
			IL_0078:
			if (_003Ci_003E5__2 < compMeditationFocus.Props.offsets.Count)
			{
				string explanation = compMeditationFocus.Props.offsets[_003Ci_003E5__2].GetExplanation(compMeditationFocus.parent);
				if (!explanation.NullOrEmpty())
				{
					_003C_003E2__current = explanation;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0068;
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
			_003CGetExplanation_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetExplanation_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__11 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompMeditationFocus _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__11(int _003C_003E1__state)
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
			CompMeditationFocus compMeditationFocus = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (ModsConfig.RoyaltyActive)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Meditation, "MeditationFocuses".Translate(), compMeditationFocus.Props.focusTypes.Select((MeditationFocusDef f) => f.label).ToCommaList().CapitalizeFirst(), "MeditationFocusesDesc".Translate(), 4011);
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private int lastUsedTick = -1;

	public new CompProperties_MeditationFocus Props => (CompProperties_MeditationFocus)props;

	public override Pawn LastUser
	{
		get
		{
			if (lastUsedTick < 0 || Find.TickManager.TicksGame - lastUsedTick > 5)
			{
				return null;
			}
			return base.LastUser;
		}
	}

	public override float GetStatOffset(Pawn pawn = null)
	{
		if (!ModLister.CheckRoyalty("Meditation focus"))
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < Props.offsets.Count; i++)
		{
			if (Props.offsets[i].CanApply(parent, pawn))
			{
				num += Props.offsets[i].GetOffset(parent, pawn);
			}
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CGetExplanation_003Ed__6))]
	public override IEnumerable<string> GetExplanation()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetExplanation_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	public bool CanPawnUse(Pawn pawn)
	{
		for (int i = 0; i < Props.focusTypes.Count; i++)
		{
			if (Props.focusTypes[i].CanPawnUse(pawn))
			{
				return true;
			}
		}
		return false;
	}

	public bool WillBeAffectedBy(ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
	{
		CellRect cellRect = GenAdj.OccupiedRect(pos, rotation, def.size);
		foreach (FocusStrengthOffset offset in Props.offsets)
		{
			if (offset is FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings && MeditationUtility.CountsAsArtificialBuilding(def, faction) && cellRect.ClosestCellTo(parent.Position).DistanceTo(parent.Position) <= focusStrengthOffset_ArtificialBuildings.radius)
			{
				return true;
			}
		}
		return false;
	}

	public override void Used(Pawn pawn)
	{
		base.Used(pawn);
		lastUsedTick = Find.TickManager.TicksGame;
	}

	public override string CompInspectStringExtra()
	{
		if (!ModsConfig.RoyaltyActive)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (LastUser != null)
		{
			stringBuilder.Append("UserMeditationFocusStrength".Translate(LastUser.Named("LASTUSER")) + ": " + Props.statDef.ValueToString(parent.GetStatValueForPawn(Props.statDef, LastUser)));
		}
		for (int i = 0; i < Props.offsets.Count; i++)
		{
			string text = Props.offsets[i].InspectStringExtra(parent);
			if (!text.NullOrEmpty())
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(text);
			}
		}
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__11))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostDrawExtraSelectionOverlays()
	{
		if (!parent.Spawned)
		{
			return;
		}
		base.PostDrawExtraSelectionOverlays();
		if (ModsConfig.RoyaltyActive)
		{
			for (int i = 0; i < Props.offsets.Count; i++)
			{
				Props.offsets[i].PostDrawExtraSelectionOverlays(parent, LastUser);
			}
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref lastUsedTick, "lastUsedTick", -1);
	}
}
