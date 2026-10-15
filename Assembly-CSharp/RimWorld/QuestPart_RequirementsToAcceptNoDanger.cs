using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_RequirementsToAcceptNoDanger : QuestPart_RequirementsToAccept
{
	[CompilerGenerated]
	private sealed class _003Cget_Culprits_003Ed__6 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_RequirementsToAcceptNoDanger _003C_003E4__this;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_Culprits_003Ed__6(int _003C_003E1__state)
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
			QuestPart_RequirementsToAcceptNoDanger questPart_RequirementsToAcceptNoDanger = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (questPart_RequirementsToAcceptNoDanger.TargetMap != null && GenHostility.AnyHostileActiveThreatTo(questPart_RequirementsToAcceptNoDanger.TargetMap, questPart_RequirementsToAcceptNoDanger.dangerTo, out var threat))
				{
					_003C_003E2__current = (Thing)threat;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_Culprits_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Culprits_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public MapParent mapParent;

	public Pawn mapPawn;

	public Faction dangerTo;

	public Map TargetMap
	{
		get
		{
			if (mapParent != null && mapParent.HasMap)
			{
				return mapParent.Map;
			}
			if (mapPawn != null)
			{
				return mapPawn.MapHeld;
			}
			return null;
		}
	}

	public override IEnumerable<GlobalTargetInfo> Culprits
	{
		[IteratorStateMachine(typeof(_003Cget_Culprits_003Ed__6))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Culprits_003Ed__6(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override AcceptanceReport CanAccept()
	{
		if (TargetMap != null && GenHostility.AnyHostileActiveThreatTo(TargetMap, dangerTo))
		{
			return new AcceptanceReport("QuestRequiresNoDangerOnMap".Translate());
		}
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_References.Look(ref mapPawn, "mapPawn");
		Scribe_References.Look(ref dangerTo, "dangerTo");
	}
}
