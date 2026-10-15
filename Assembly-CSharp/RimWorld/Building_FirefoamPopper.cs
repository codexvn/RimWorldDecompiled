using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Building_FirefoamPopper : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__6 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_FirefoamPopper _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__6(int _003C_003E1__state)
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
				Building_FirefoamPopper CS_0024_003C_003E8__locals6 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals6).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
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
					if (CS_0024_003C_003E8__locals6.CanSetAutoRebuild)
					{
						_003C_003E2__current = new Command_Toggle
						{
							defaultLabel = "CommandAutoRebuild_Building".Translate(),
							defaultDesc = "CommandAutoRebuild_BuildingDesc".Translate(CS_0024_003C_003E8__locals6.Named("BUILDING")),
							hotKey = KeyBindingDefOf.Misc3,
							icon = RebuildCommandTex.Texture,
							isActive = () => CS_0024_003C_003E8__locals6.autoRebuild,
							toggleAction = delegate
							{
								CS_0024_003C_003E8__locals6.autoRebuild = !CS_0024_003C_003E8__locals6.autoRebuild;
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
			_003CGetGizmos_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__6(0)
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

	private bool autoRebuild;

	private static readonly CachedTexture RebuildCommandTex = new CachedTexture("UI/Commands/RearmFirefoamPopper");

	private bool CanSetAutoRebuild
	{
		get
		{
			if (base.Faction == Faction.OfPlayer && def.blueprintDef != null)
			{
				return def.IsResearchFinished;
			}
			return false;
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
		{
			autoRebuild = CanSetAutoRebuild && map.areaManager.Home[base.Position];
		}
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		Map map = base.Map;
		base.Destroy(mode);
		if (mode != DestroyMode.Deconstruct && autoRebuild && map != null && CanSetAutoRebuild && GenConstruct.CanPlaceBlueprintAt(def, base.Position, base.Rotation, map).Accepted)
		{
			GenConstruct.PlaceBlueprintForBuild(def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__6))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref autoRebuild, "autoRebuild", defaultValue: false);
	}
}
