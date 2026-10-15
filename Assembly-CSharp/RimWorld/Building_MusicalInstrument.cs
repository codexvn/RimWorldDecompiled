using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class Building_MusicalInstrument : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__11 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_MusicalInstrument _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__11(int _003C_003E1__state)
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
				Building_MusicalInstrument CS_0024_003C_003E8__locals3 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!ModLister.CheckRoyaltyOrIdeology("Musical instrument"))
					{
						return false;
					}
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals3).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Toggle is playing",
							action = delegate
							{
								CS_0024_003C_003E8__locals3.currentPlayer = ((CS_0024_003C_003E8__locals3.currentPlayer == null) ? PawnsFinder.AllMaps_FreeColonists.FirstOrDefault() : null);
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private Pawn currentPlayer;

	private Sustainer soundPlaying;

	public bool IsBeingPlayed => currentPlayer != null;

	public FloatRange SoundRange
	{
		get
		{
			if (soundPlaying == null)
			{
				return FloatRange.Zero;
			}
			if (soundPlaying.def.subSounds.NullOrEmpty())
			{
				return FloatRange.Zero;
			}
			return soundPlaying.def.subSounds.First().distRange;
		}
	}

	public static bool IsAffectedByInstrument(ThingDef instrumentDef, IntVec3 instrumentPos, IntVec3 pawnPos, Map map)
	{
		if (instrumentPos.DistanceTo(pawnPos) < instrumentDef.building.instrumentRange)
		{
			return instrumentPos.GetRoom(map) == pawnPos.GetRoom(map);
		}
		return false;
	}

	public void StartPlaying(Pawn player)
	{
		if (ModLister.CheckRoyaltyOrIdeology("Musical instrument"))
		{
			currentPlayer = player;
		}
	}

	protected override void Tick()
	{
		base.Tick();
		if (currentPlayer != null)
		{
			if (def.soundPlayInstrument != null && soundPlaying == null)
			{
				soundPlaying = def.soundPlayInstrument.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(base.Position, base.Map), MaintenanceType.PerTick));
			}
		}
		else
		{
			soundPlaying = null;
		}
		soundPlaying?.Maintain();
	}

	public void StopPlaying()
	{
		currentPlayer = null;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref currentPlayer, "currentPlayer");
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__11))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}
}
