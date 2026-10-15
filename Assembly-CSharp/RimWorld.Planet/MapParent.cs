using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet;

[StaticConstructorOnStartup]
public class MapParent : WorldObject, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass38_0
	{
		public MapParent _003C_003E4__this;

		public Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<LocalTargetInfo> _003C_003E9__1;

		internal void _003CGetTransportersFloatMenuOptions_003Eb__0()
		{
			Map map = _003C_003E4__this.Map;
			Current.Game.CurrentMap = map;
			CameraJumper.TryHideWorld();
			Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
			{
				launchAction(_003C_003E4__this.Tile, new TransportersArrivalAction_LandInSpecificCell(_003C_003E4__this, x.Cell, Rot4.North, landInShuttle: false));
			}, null, null, CompLaunchable.TargeterMouseAttachment);
		}

		internal void _003CGetTransportersFloatMenuOptions_003Eb__1(LocalTargetInfo x)
		{
			launchAction(_003C_003E4__this.Tile, new TransportersArrivalAction_LandInSpecificCell(_003C_003E4__this, x.Cell, Rot4.North, landInShuttle: false));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass40_0
	{
		public MapParent _003C_003E4__this;

		public ThingWithComps shuttle;

		public Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<LocalTargetInfo> _003C_003E9__1;

		internal void _003CGetShuttleFloatMenuOptions_003Eb__0()
		{
			_003C_003Ec__DisplayClass40_1 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass40_1
			{
				CS_0024_003C_003E8__locals1 = this
			};
			Map map = _003C_003E4__this.Map;
			Current.Game.CurrentMap = map;
			CameraJumper.TryHideWorld();
			CS_0024_003C_003E8__locals8.shuttleDef = shuttle?.def ?? ThingDefOf.Shuttle;
			shuttleRotation = CS_0024_003C_003E8__locals8.shuttleDef.defaultPlacingRot;
			Find.Targeter.BeginTargeting(TargetingParameters.ForCell(), delegate(LocalTargetInfo x)
			{
				launchAction(_003C_003E4__this.Tile, new TransportersArrivalAction_LandInSpecificCell(_003C_003E4__this, x.Cell, shuttleRotation, landInShuttle: true));
			}, delegate(LocalTargetInfo x)
			{
				RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(x, CS_0024_003C_003E8__locals8.CS_0024_003C_003E8__locals1._003C_003E4__this.Map, CS_0024_003C_003E8__locals8.shuttleDef, shuttleRotation);
			}, delegate(LocalTargetInfo x)
			{
				AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(x, CS_0024_003C_003E8__locals8.CS_0024_003C_003E8__locals1._003C_003E4__this.Map, CS_0024_003C_003E8__locals8.shuttleDef, shuttleRotation);
				if (!acceptanceReport.Accepted)
				{
					Messages.Message(acceptanceReport.Reason, new LookTargets(CS_0024_003C_003E8__locals8.CS_0024_003C_003E8__locals1._003C_003E4__this), MessageTypeDefOf.RejectInput, historical: false);
				}
				return acceptanceReport.Accepted;
			}, null, null, CompLaunchable.TargeterMouseAttachment, playSoundOnAction: true, delegate
			{
				if (CS_0024_003C_003E8__locals8.shuttleDef.rotatable)
				{
					if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
					{
						shuttleRotation = shuttleRotation.Rotated(RotationDirection.Clockwise);
					}
					if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
					{
						shuttleRotation = shuttleRotation.Rotated(RotationDirection.Counterclockwise);
					}
				}
			});
		}

		internal void _003CGetShuttleFloatMenuOptions_003Eb__1(LocalTargetInfo x)
		{
			launchAction(_003C_003E4__this.Tile, new TransportersArrivalAction_LandInSpecificCell(_003C_003E4__this, x.Cell, shuttleRotation, landInShuttle: true));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass40_1
	{
		public ThingDef shuttleDef;

		public _003C_003Ec__DisplayClass40_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetShuttleFloatMenuOptions_003Eb__2(LocalTargetInfo x)
		{
			RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(x, CS_0024_003C_003E8__locals1._003C_003E4__this.Map, shuttleDef, shuttleRotation);
		}

		internal bool _003CGetShuttleFloatMenuOptions_003Eb__3(LocalTargetInfo x)
		{
			AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(x, CS_0024_003C_003E8__locals1._003C_003E4__this.Map, shuttleDef, shuttleRotation);
			if (!acceptanceReport.Accepted)
			{
				Messages.Message(acceptanceReport.Reason, new LookTargets(CS_0024_003C_003E8__locals1._003C_003E4__this), MessageTypeDefOf.RejectInput, historical: false);
			}
			return acceptanceReport.Accepted;
		}

		internal void _003CGetShuttleFloatMenuOptions_003Eb__4(LocalTargetInfo _)
		{
			if (shuttleDef.rotatable)
			{
				if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
				{
					shuttleRotation = shuttleRotation.Rotated(RotationDirection.Clockwise);
				}
				if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
				{
					shuttleRotation = shuttleRotation.Rotated(RotationDirection.Counterclockwise);
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__37 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MapParent _003C_003E4__this;

		private Caravan caravan;

		public Caravan _003C_003E3__caravan;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptions_003Ed__37(int _003C_003E1__state)
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
				MapParent mapParent = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldObject)mapParent).GetFloatMenuOptions(caravan).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00e3;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!mapParent.UseGenericEnterMapFloatMenuOption)
					{
						break;
					}
					_003C_003E7__wrap1 = CaravanArrivalAction_Enter.GetFloatMenuOptions(caravan, mapParent).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00e3;
					IL_00e3:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current2 = _003C_003E7__wrap1.Current;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__37 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__37(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.caravan = _003C_003E3__caravan;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__35 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MapParent _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__35(int _003C_003E1__state)
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
				MapParent CS_0024_003C_003E8__locals3 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldObject)CS_0024_003C_003E8__locals3).GetGizmos().GetEnumerator();
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
					if (!CS_0024_003C_003E8__locals3.HasMap)
					{
						break;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandShowMap".Translate(),
						defaultDesc = "CommandShowMapDesc".Translate(),
						icon = ShowMapCommand,
						hotKey = KeyBindingDefOf.Misc1,
						action = delegate
						{
							Current.Game.CurrentMap = CS_0024_003C_003E8__locals3.Map;
							if (!CameraJumper.TryHideWorld())
							{
								SoundDefOf.TabClose.PlayOneShotOnCamera();
							}
						}
					};
					_003C_003E1__state = 2;
					return true;
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
			_003CGetGizmos_003Ed__35 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__35(0)
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

	[CompilerGenerated]
	private sealed class _003CGetShuttleFloatMenuOptions_003Ed__40 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MapParent _003C_003E4__this;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetShuttleFloatMenuOptions_003Ed__40(int _003C_003E1__state)
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
			MapParent mapParent = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass40_0 CS_0024_003C_003E8__locals9 = new _003C_003Ec__DisplayClass40_0
				{
					_003C_003E4__this = _003C_003E4__this,
					launchAction = launchAction,
					shuttle = ((pods.FirstOrDefault() is CompTransporter compTransporter) ? compTransporter.parent : null)
				};
				if (!TransportersArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, mapParent))
				{
					break;
				}
				_003C_003E2__current = new FloatMenuOption("LandInExistingMap".Translate(mapParent.Label), delegate
				{
					_003C_003Ec__DisplayClass40_1 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass40_1
					{
						CS_0024_003C_003E8__locals1 = CS_0024_003C_003E8__locals9
					};
					Map map = CS_0024_003C_003E8__locals9._003C_003E4__this.Map;
					Current.Game.CurrentMap = map;
					CameraJumper.TryHideWorld();
					CS_0024_003C_003E8__locals15.shuttleDef = CS_0024_003C_003E8__locals9.shuttle?.def ?? ThingDefOf.Shuttle;
					shuttleRotation = CS_0024_003C_003E8__locals15.shuttleDef.defaultPlacingRot;
					Find.Targeter.BeginTargeting(TargetingParameters.ForCell(), delegate(LocalTargetInfo x)
					{
						CS_0024_003C_003E8__locals9.launchAction(CS_0024_003C_003E8__locals9._003C_003E4__this.Tile, new TransportersArrivalAction_LandInSpecificCell(CS_0024_003C_003E8__locals9._003C_003E4__this, x.Cell, shuttleRotation, landInShuttle: true));
					}, delegate(LocalTargetInfo x)
					{
						RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(x, CS_0024_003C_003E8__locals15.CS_0024_003C_003E8__locals1._003C_003E4__this.Map, CS_0024_003C_003E8__locals15.shuttleDef, shuttleRotation);
					}, delegate(LocalTargetInfo x)
					{
						AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(x, CS_0024_003C_003E8__locals15.CS_0024_003C_003E8__locals1._003C_003E4__this.Map, CS_0024_003C_003E8__locals15.shuttleDef, shuttleRotation);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, new LookTargets(CS_0024_003C_003E8__locals15.CS_0024_003C_003E8__locals1._003C_003E4__this), MessageTypeDefOf.RejectInput, historical: false);
						}
						return acceptanceReport.Accepted;
					}, null, null, CompLaunchable.TargeterMouseAttachment, playSoundOnAction: true, delegate
					{
						if (CS_0024_003C_003E8__locals15.shuttleDef.rotatable)
						{
							if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
							{
								shuttleRotation = shuttleRotation.Rotated(RotationDirection.Clockwise);
							}
							if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
							{
								shuttleRotation = shuttleRotation.Rotated(RotationDirection.Counterclockwise);
							}
						}
					});
				});
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetShuttleFloatMenuOptions_003Ed__40 _003CGetShuttleFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetShuttleFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetShuttleFloatMenuOptions_003Ed__ = new _003CGetShuttleFloatMenuOptions_003Ed__40(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetShuttleFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetShuttleFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			return _003CGetShuttleFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTransportersFloatMenuOptions_003Ed__38 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MapParent _003C_003E4__this;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private _003C_003Ec__DisplayClass38_0 _003C_003E8__1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetTransportersFloatMenuOptions_003Ed__38(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MapParent mapParent = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass38_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.launchAction = launchAction;
					_003C_003E7__wrap1 = ((WorldObject)mapParent).GetTransportersFloatMenuOptions(pods, _003C_003E8__1.launchAction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b0;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00b0;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00b0:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!TransportersArrivalAction_LandInSpecificCell.CanLandInSpecificCell(pods, mapParent))
					{
						break;
					}
					_003C_003E2__current = new FloatMenuOption("LandInExistingMap".Translate(mapParent.Label), delegate
					{
						Map map = _003C_003E8__1._003C_003E4__this.Map;
						Current.Game.CurrentMap = map;
						CameraJumper.TryHideWorld();
						Find.Targeter.BeginTargeting(TargetingParameters.ForDropPodsDestination(), delegate(LocalTargetInfo x)
						{
							_003C_003E8__1.launchAction(_003C_003E8__1._003C_003E4__this.Tile, new TransportersArrivalAction_LandInSpecificCell(_003C_003E8__1._003C_003E4__this, x.Cell, Rot4.North, landInShuttle: false));
						}, null, null, CompLaunchable.TargeterMouseAttachment);
					});
					_003C_003E1__state = 2;
					return true;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetTransportersFloatMenuOptions_003Ed__38 _003CGetTransportersFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTransportersFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetTransportersFloatMenuOptions_003Ed__ = new _003CGetTransportersFloatMenuOptions_003Ed__38(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTransportersFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetTransportersFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			return _003CGetTransportersFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CIncidentTargetTags_003Ed__36 : IEnumerable<IncidentTargetTagDef>, IEnumerable, IEnumerator<IncidentTargetTagDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IncidentTargetTagDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MapParent _003C_003E4__this;

		private IEnumerator<IncidentTargetTagDef> _003C_003E7__wrap1;

		private HashSet<IncidentTargetTagDef>.Enumerator _003C_003E7__wrap2;

		IncidentTargetTagDef IEnumerator<IncidentTargetTagDef>.Current
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
		public _003CIncidentTargetTags_003Ed__36(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = default(HashSet<IncidentTargetTagDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MapParent mapParent = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldObject)mapParent).IncidentTargetTags().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00e9;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						IncidentTargetTagDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (mapParent.hibernatableIncidentTargets != null && mapParent.hibernatableIncidentTargets.Count > 0)
					{
						_003C_003E7__wrap2 = mapParent.hibernatableIncidentTargets.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00e9;
					}
					goto IL_0108;
					IL_00e9:
					if (_003C_003E7__wrap2.MoveNext())
					{
						IncidentTargetTagDef current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(HashSet<IncidentTargetTagDef>.Enumerator);
					goto IL_0108;
					IL_0108:
					if (mapParent.HasMap && mapParent.Map.wasSpawnedViaGravShipLanding)
					{
						_003C_003E2__current = IncidentTargetTagDefOf.Map_PlayerHome;
						_003C_003E1__state = 3;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IncidentTargetTagDef> IEnumerable<IncidentTargetTagDef>.GetEnumerator()
		{
			_003CIncidentTargetTags_003Ed__36 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CIncidentTargetTags_003Ed__36(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IncidentTargetTagDef>)this).GetEnumerator();
		}
	}

	public bool forceRemoveWorldObjectWhenMapRemoved;

	public bool doorsAlwaysOpenForPlayerPawns;

	private HashSet<IncidentTargetTagDef> hibernatableIncidentTargets;

	private static readonly Texture2D ShowMapCommand = ContentFinder<Texture2D>.Get("UI/Commands/ShowMap");

	private static Rot4 shuttleRotation = Rot4.East;

	public bool HasMap => Map != null;

	public virtual AcceptanceReport CanBeSettled => true;

	protected virtual bool UseGenericEnterMapFloatMenuOption => true;

	public Map Map => Current.Game.FindMap(this);

	public virtual MapGeneratorDef MapGeneratorDef => def.mapGenerator ?? MapGeneratorDefOf.Encounter;

	public virtual IEnumerable<GenStepWithParams> ExtraGenStepDefs => Enumerable.Empty<GenStepWithParams>();

	public override bool ExpandMore
	{
		get
		{
			if (!base.ExpandMore)
			{
				return HasMap;
			}
			return true;
		}
	}

	public virtual bool HandlesConditionCausers => false;

	public virtual Vector3 WorldCameraPosition => base.Tile.Layer.GetTileCenter(base.Tile);

	public virtual void PostMapGenerate()
	{
		List<WorldObjectComp> allComps = base.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			allComps[i].PostMapGenerate();
		}
		QuestUtility.SendQuestTargetSignals(questTags, "MapGenerated", this.Named("SUBJECT"));
	}

	public virtual void Notify_MyMapAboutToBeRemoved()
	{
	}

	public virtual void Notify_MyMapRemoved(Map map)
	{
		List<WorldObjectComp> allComps = base.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			allComps[i].PostMyMapRemoved();
		}
		QuestUtility.SendQuestTargetSignals(questTags, "MapRemoved", this.Named("SUBJECT"));
	}

	public virtual void Notify_MyMapSettled(Map map)
	{
		List<WorldObjectComp> allComps = base.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			allComps[i].PostMyMapSettled();
		}
		QuestUtility.SendQuestTargetSignals(questTags, "MapSettled", this.Named("SUBJECT"));
	}

	public virtual void Notify_CaravanFormed(Caravan caravan)
	{
		List<WorldObjectComp> allComps = base.AllComps;
		for (int i = 0; i < allComps.Count; i++)
		{
			allComps[i].PostCaravanFormed(caravan);
		}
	}

	public virtual void Notify_HibernatableChanged()
	{
		RecalculateHibernatableIncidentTargets();
	}

	public virtual void FinalizeLoading()
	{
		RecalculateHibernatableIncidentTargets();
	}

	public virtual bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
	{
		alsoRemoveWorldObject = false;
		return false;
	}

	public override void PostRemove()
	{
		base.PostRemove();
		if (HasMap)
		{
			Current.Game.DeinitAndRemoveMap(Map, notifyPlayer: true);
		}
	}

	public virtual void Abandon(bool wasGravshipLaunch)
	{
		bool flag = false;
		int count = Find.Maps.Count;
		for (int i = 0; i < count; i++)
		{
			if (Find.Maps[i].IsPlayerHome && Find.Maps[i] != Map)
			{
				flag = true;
				break;
			}
		}
		if (Map.IsPlayerHome && !flag)
		{
			foreach (Pawn allMapsCaravansAndTravellingTransporters_Alive_Colonist in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				allMapsCaravansAndTravellingTransporters_Alive_Colonist.genes?.Notify_NewColony();
				if (allMapsCaravansAndTravellingTransporters_Alive_Colonist.BeingTransportedOnGravship)
				{
					continue;
				}
				MemoryThoughtHandler memoryThoughtHandler = allMapsCaravansAndTravellingTransporters_Alive_Colonist.needs?.mood?.thoughts?.memories;
				if (memoryThoughtHandler != null)
				{
					memoryThoughtHandler.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyOptimism);
					memoryThoughtHandler.RemoveMemoriesOfDef(ThoughtDefOf.NewColonyHope);
					if (allMapsCaravansAndTravellingTransporters_Alive_Colonist.IsFreeNonSlaveColonist)
					{
						memoryThoughtHandler.TryGainMemory(ThoughtDefOf.NewColonyOptimism);
					}
				}
			}
		}
		Destroy();
		if (wasGravshipLaunch && base.Tile.LayerDef == PlanetLayerDefOf.Surface)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.GravshipLaunch);
			worldObject.Tile = base.Tile;
			worldObject.SetFaction(base.Faction);
			Find.WorldObjects.Add(worldObject);
		}
	}

	public override void Destroy()
	{
		if (Map != null)
		{
			for (int num = Map.listerThings.AllThings.Count - 1; num >= 0; num--)
			{
				Map.listerThings.AllThings[num].Notify_LeftBehind();
			}
		}
		base.Destroy();
	}

	protected override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		CheckRemoveMapNow();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref forceRemoveWorldObjectWhenMapRemoved, "forceRemoveWorldObjectWhenMapRemoved", defaultValue: false);
		Scribe_Values.Look(ref doorsAlwaysOpenForPlayerPawns, "doorsAlwaysOpenForPlayerPawns", defaultValue: false);
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__35))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__35(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CIncidentTargetTags_003Ed__36))]
	public override IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CIncidentTargetTags_003Ed__36(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__37))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__37(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__caravan = caravan
		};
	}

	[IteratorStateMachine(typeof(_003CGetTransportersFloatMenuOptions_003Ed__38))]
	public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTransportersFloatMenuOptions_003Ed__38(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	[IteratorStateMachine(typeof(_003CGetShuttleFloatMenuOptions_003Ed__40))]
	public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetShuttleFloatMenuOptions_003Ed__40(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	public void CheckRemoveMapNow()
	{
		if (HasMap && ShouldRemoveMapNow(out var alsoRemoveWorldObject))
		{
			Map map = Map;
			Current.Game.DeinitAndRemoveMap(map, notifyPlayer: true);
			if (!base.Destroyed && (alsoRemoveWorldObject || forceRemoveWorldObjectWhenMapRemoved))
			{
				Destroy();
			}
		}
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (HasMap && GravshipUtility.TryGetNameOfGravshipOnMap(Map, out var name))
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "GravshipOnTileInspectString".Translate().CapitalizeFirst() + ": " + name;
		}
		if (this.EnterCooldownBlocksEntering())
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "EnterCooldown".Translate(this.EnterCooldownTicksLeft().ToStringTicksToPeriod());
		}
		if (!HandlesConditionCausers && HasMap)
		{
			List<Thing> list = Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
			for (int i = 0; i < list.Count; i++)
			{
				text += "\n" + list[i].LabelShortCap + " (" + "ConditionCauserRadius".Translate(list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange) + ")";
			}
		}
		return text;
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (!HandlesConditionCausers && HasMap)
		{
			int num = 0;
			List<Thing> list = Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser);
			for (int i = 0; i < list.Count; i++)
			{
				num = Mathf.Max(num, list[i].TryGetComp<CompCauseGameCondition>().Props.worldRange);
			}
			if (num > 0)
			{
				GenDraw.DrawWorldRadiusRing(base.Tile, num);
			}
		}
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return null;
	}

	public virtual void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
		if (HasMap)
		{
			outChildren.Add(Map);
		}
	}

	private void RecalculateHibernatableIncidentTargets()
	{
		hibernatableIncidentTargets = null;
		foreach (ThingWithComps item in Map.listerThings.ThingsOfDef(ThingDefOf.Ship_Reactor).OfType<ThingWithComps>())
		{
			CompHibernatable compHibernatable = item.TryGetComp<CompHibernatable>();
			if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Starting && compHibernatable.Props.incidentTargetWhileStarting != null)
			{
				if (hibernatableIncidentTargets == null)
				{
					hibernatableIncidentTargets = new HashSet<IncidentTargetTagDef>();
				}
				hibernatableIncidentTargets.Add(compHibernatable.Props.incidentTargetWhileStarting);
			}
		}
	}
}
