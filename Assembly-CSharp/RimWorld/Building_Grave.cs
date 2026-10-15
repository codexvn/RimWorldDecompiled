using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

public class Building_Grave : Building_CorpseCasket, INotifyHauledTo
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__13 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Grave _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__13(int _003C_003E1__state)
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
				Building_Grave building_Grave = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building_Casket)building_Grave).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00dc;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!building_Grave.StorageTabVisible)
					{
						break;
					}
					_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(building_Grave.storageSettings).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00dc;
					IL_00dc:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__13(0)
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

	private Graphic cachedGraphicFull;

	public Pawn AssignedPawn
	{
		get
		{
			if (CompAssignableToPawn == null || !CompAssignableToPawn.AssignedPawnsForReading.Any())
			{
				return null;
			}
			return CompAssignableToPawn.AssignedPawnsForReading[0];
		}
	}

	public CompAssignableToPawn_Grave CompAssignableToPawn => GetComp<CompAssignableToPawn_Grave>();

	public override Graphic Graphic
	{
		get
		{
			if (base.HasCorpse)
			{
				if (def.building.fullGraveGraphicData == null)
				{
					return base.Graphic;
				}
				if (cachedGraphicFull == null)
				{
					cachedGraphicFull = def.building.fullGraveGraphicData.GraphicColoredFor(this);
				}
				return cachedGraphicFull;
			}
			return base.Graphic;
		}
	}

	public override bool StorageTabVisible
	{
		get
		{
			if (base.StorageTabVisible)
			{
				return AssignedPawn == null;
			}
			return false;
		}
	}

	public override void EjectContents()
	{
		base.EjectContents();
		if (base.Spawned)
		{
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
		}
	}

	public virtual void Notify_HauledTo(Pawn hauler, Thing thing, int count)
	{
		CompArt comp = GetComp<CompArt>();
		if (comp != null && !comp.Active && hauler.RaceProps.Humanlike)
		{
			comp.JustCreatedBy(hauler);
			comp.InitializeArt(base.Corpse.InnerPawn);
		}
		base.Map.mapDrawer.MapMeshDirty(base.Position, (ulong)MapMeshFlagDefOf.Buildings | (ulong)MapMeshFlagDefOf.Things);
		hauler.records.Increment(RecordDefOf.CorpsesBuried);
		TaleRecorder.RecordTale(TaleDefOf.BuriedCorpse, hauler, base.Corpse?.InnerPawn);
	}

	public override bool Accepts(Thing thing)
	{
		if (!base.Accepts(thing))
		{
			return false;
		}
		if (base.HasCorpse)
		{
			return false;
		}
		if (AssignedPawn != null)
		{
			if (!(thing is Corpse corpse))
			{
				return false;
			}
			if (corpse.InnerPawn != AssignedPawn)
			{
				return false;
			}
		}
		else if (!storageSettings.AllowedToAccept(thing))
		{
			return false;
		}
		return true;
	}

	public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
	{
		if (base.TryAcceptThing(thing, allowSpecialEffects))
		{
			if (thing is Corpse corpse && corpse.InnerPawn.ownership != null && corpse.InnerPawn.ownership.AssignedGrave != this)
			{
				corpse.InnerPawn.ownership.UnclaimGrave();
			}
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__13))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Notify_ColorChanged()
	{
		base.Notify_ColorChanged();
		cachedGraphicFull = null;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		if (base.HasCorpse)
		{
			if (base.Tile.Valid)
			{
				string text = GenDate.DateFullStringAt(GenDate.TickGameToAbs(base.Corpse.timeOfDeath), Find.WorldGrid.LongLatOf(base.Tile));
				stringBuilder.AppendLine();
				stringBuilder.Append("DiedOn".Translate(text));
			}
		}
		else if (AssignedPawn != null)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append("AssignedColonist".Translate());
			stringBuilder.Append(": ");
			stringBuilder.Append(AssignedPawn.LabelCap);
		}
		return stringBuilder.ToString();
	}
}
