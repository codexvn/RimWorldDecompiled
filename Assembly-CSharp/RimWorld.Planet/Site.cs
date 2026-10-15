using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

[StaticConstructorOnStartup]
public class Site : MapParent
{
	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__59 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Site _003C_003E4__this;

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
		public _003CGetFloatMenuOptions_003Ed__59(int _003C_003E1__state)
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
				Site site = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)site).GetFloatMenuOptions(caravan).GetEnumerator();
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
					if (site.HasMap)
					{
						break;
					}
					_003C_003E7__wrap1 = CaravanArrivalAction_VisitSite.GetFloatMenuOptions(caravan, site).GetEnumerator();
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
			_003CGetFloatMenuOptions_003Ed__59 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__59(0)
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
	private sealed class _003CGetGizmos_003Ed__62 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Site _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__62(int _003C_003E1__state)
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
				Site site = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)site).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (site.HasMap && Find.WorldSelector.SingleSelectedObject == site)
					{
						_003C_003E2__current = SettleInExistingMapUtility.SettleCommand(site.Map, requiresNoEnemies: true);
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
			_003CGetGizmos_003Ed__62 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__62(0)
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
	private sealed class _003CGetShuttleFloatMenuOptions_003Ed__61 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Site _003C_003E4__this;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

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
		public _003CGetShuttleFloatMenuOptions_003Ed__61(int _003C_003E1__state)
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
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
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
				Site site = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)site).GetShuttleFloatMenuOptions(pods, launchAction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -4;
					goto IL_0100;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_016b;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (site.def.mapGenerator == MapGeneratorDefOf.Space)
					{
						_003C_003E7__wrap1 = TransportersArrivalAction_VisitSpace.GetFloatMenuOptions(launchAction, pods, site).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0100;
					}
					_003C_003E7__wrap1 = TransportersArrivalAction_VisitSite.GetFloatMenuOptions(launchAction, pods, site).GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_016b;
					IL_016b:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					break;
					IL_0100:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
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

		private void _003C_003Em__Finally3()
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
			_003CGetShuttleFloatMenuOptions_003Ed__61 _003CGetShuttleFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetShuttleFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetShuttleFloatMenuOptions_003Ed__ = new _003CGetShuttleFloatMenuOptions_003Ed__61(0)
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
	private sealed class _003CGetTransportersFloatMenuOptions_003Ed__60 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Site _003C_003E4__this;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

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
		public _003CGetTransportersFloatMenuOptions_003Ed__60(int _003C_003E1__state)
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
				Site site = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)site).GetTransportersFloatMenuOptions(pods, launchAction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = TransportersArrivalAction_VisitSite.GetFloatMenuOptions(launchAction, pods, site).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
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
			_003CGetTransportersFloatMenuOptions_003Ed__60 _003CGetTransportersFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTransportersFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetTransportersFloatMenuOptions_003Ed__ = new _003CGetTransportersFloatMenuOptions_003Ed__60(0)
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
	private sealed class _003Cget_ExtraGenStepDefs_003Ed__29 : IEnumerable<GenStepWithParams>, IEnumerable, IEnumerator<GenStepWithParams>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenStepWithParams _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Site _003C_003E4__this;

		private IEnumerator<GenStepWithParams> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

		private GenStepParams _003CpartGenStepParams_003E5__4;

		private List<GenStepDef> _003CpartGenStepDefs_003E5__5;

		private int _003Cj_003E5__6;

		GenStepWithParams IEnumerator<GenStepWithParams>.Current
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
		public _003Cget_ExtraGenStepDefs_003Ed__29(int _003C_003E1__state)
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
			_003CpartGenStepParams_003E5__4 = default(GenStepParams);
			_003CpartGenStepDefs_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Site site = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapParent)site).ExtraGenStepDefs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						_003Cj_003E5__6++;
						goto IL_0130;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GenStepWithParams current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3 = 0;
					goto IL_0168;
					IL_0168:
					if (_003Ci_003E5__3 < site.parts.Count)
					{
						_003CpartGenStepParams_003E5__4 = default(GenStepParams);
						_003CpartGenStepParams_003E5__4.sitePart = site.parts[_003Ci_003E5__3];
						_003CpartGenStepDefs_003E5__5 = site.parts[_003Ci_003E5__3].def.ExtraGenSteps;
						_003Cj_003E5__6 = 0;
						goto IL_0130;
					}
					return false;
					IL_0130:
					if (_003Cj_003E5__6 < _003CpartGenStepDefs_003E5__5.Count)
					{
						_003C_003E2__current = new GenStepWithParams(_003CpartGenStepDefs_003E5__5[_003Cj_003E5__6], _003CpartGenStepParams_003E5__4);
						_003C_003E1__state = 2;
						return true;
					}
					_003CpartGenStepParams_003E5__4 = default(GenStepParams);
					_003CpartGenStepDefs_003E5__5 = null;
					_003Ci_003E5__3++;
					goto IL_0168;
				}
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
		IEnumerator<GenStepWithParams> IEnumerable<GenStepWithParams>.GetEnumerator()
		{
			_003Cget_ExtraGenStepDefs_003Ed__29 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ExtraGenStepDefs_003Ed__29(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GenStepWithParams>)this).GetEnumerator();
		}
	}

	public string customLabel;

	public List<SitePart> parts = new List<SitePart>();

	public bool sitePartsKnown = true;

	public bool factionMustRemainHostile;

	public float desiredThreatPoints;

	private SiteCoreBackCompat coreBackCompat;

	private bool anyEnemiesInitially;

	private bool caravanAssaultSuccessfulTaleRecorded;

	private bool allEnemiesDefeatedSignalSent;

	public bool preventGravshipLanding;

	public bool firedNoActiveThreatsSignal;

	private Material cachedMat;

	private static readonly IntVec3 DefaultMapSize = new IntVec3(200, 1, 200);

	private static List<string> tmpSitePartsLabels = new List<string>();

	public override string Label
	{
		get
		{
			if (!customLabel.NullOrEmpty())
			{
				return customLabel;
			}
			if (MainSitePartDef == SitePartDefOf.PreciousLump && MainSitePart.parms.preciousLumpResources != null)
			{
				return "PreciousLumpLabel".Translate(MainSitePart.parms.preciousLumpResources.label);
			}
			return MainSitePartDef.label;
		}
	}

	public override Texture2D ExpandingIcon => MainSitePartDef.ExpandingIconTexture;

	public override bool HandlesConditionCausers => true;

	public override Material Material
	{
		get
		{
			if (cachedMat == null)
			{
				cachedMat = MaterialPool.MatFrom(color: (!MainSitePartDef.applyFactionColorToSiteTexture || base.Faction == null) ? Color.white : base.Faction.Color, texPath: MainSitePartDef.siteTexture, shader: ShaderDatabase.WorldOverlayTransparentLit, renderQueue: 3550);
			}
			return cachedMat;
		}
	}

	public override bool AppendFactionToInspectString
	{
		get
		{
			if (!MainSitePartDef.applyFactionColorToSiteTexture)
			{
				return MainSitePartDef.showFactionInInspectString;
			}
			return true;
		}
	}

	private SitePart MainSitePart
	{
		get
		{
			if (!parts.Any())
			{
				Log.ErrorOnce("Site without any SitePart at " + base.Tile, ID ^ 0x598A95D);
				return null;
			}
			if (parts[0].hidden)
			{
				Log.ErrorOnce("Site with first SitePart hidden at " + base.Tile, ID ^ 0x2E39CC7);
				return parts[0];
			}
			return parts[0];
		}
	}

	public SitePartDef MainSitePartDef => MainSitePart.def;

	public override bool GravShipCanLandOn
	{
		get
		{
			if (MainSitePartDef.gravShipsCanLandOn)
			{
				return !base.HasMap;
			}
			return false;
		}
	}

	public override IEnumerable<GenStepWithParams> ExtraGenStepDefs
	{
		[IteratorStateMachine(typeof(_003Cget_ExtraGenStepDefs_003Ed__29))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ExtraGenStepDefs_003Ed__29(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public string ApproachOrderString => MainSitePartDef.approachOrderString.NullOrEmpty() ? "ApproachSite".Translate(Label) : MainSitePartDef.approachOrderString.Formatted(Label);

	public string ApproachingReportString => MainSitePartDef.approachingReportString.NullOrEmpty() ? "ApproachingSite".Translate(Label) : MainSitePartDef.approachingReportString.Formatted(Label);

	public float ActualThreatPoints
	{
		get
		{
			float num = 0f;
			for (int i = 0; i < parts.Count; i++)
			{
				num += parts[i].parms.threatPoints;
			}
			return num;
		}
	}

	public bool IncreasesPopulation
	{
		get
		{
			if (base.HasMap)
			{
				return false;
			}
			for (int i = 0; i < parts.Count; i++)
			{
				if (parts[i].def.Worker.IncreasesPopulation(parts[i].parms))
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool BadEvenIfNoMap
	{
		get
		{
			for (int i = 0; i < parts.Count; i++)
			{
				if (parts[i].def.badEvenIfNoMap)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool HasWorldObjectTimeout => WorldObjectTimeoutTicksLeft != -1;

	public int WorldObjectTimeoutTicksLeft
	{
		get
		{
			List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
			for (int i = 0; i < questsListForReading.Count; i++)
			{
				Quest quest = questsListForReading[i];
				if (quest.State != QuestState.Ongoing)
				{
					continue;
				}
				for (int j = 0; j < quest.PartsListForReading.Count; j++)
				{
					if (quest.PartsListForReading[j] is QuestPart_WorldObjectTimeout { State: QuestPartState.Enabled } questPart_WorldObjectTimeout && questPart_WorldObjectTimeout.worldObject == this)
					{
						return questPart_WorldObjectTimeout.TicksLeft;
					}
				}
			}
			return -1;
		}
	}

	public IntVec3 PreferredMapSize
	{
		get
		{
			IntVec3 defaultMapSize = DefaultMapSize;
			for (int i = 0; i < parts.Count; i++)
			{
				SitePart sitePart = parts[i];
				if (sitePart.def.minMapSize.HasValue)
				{
					IntVec3 value = sitePart.def.minMapSize.Value;
					defaultMapSize.x = Mathf.Max(value.x, defaultMapSize.x);
					defaultMapSize.y = Mathf.Max(value.y, defaultMapSize.y);
					defaultMapSize.z = Mathf.Max(value.z, defaultMapSize.z);
				}
			}
			return defaultMapSize;
		}
	}

	public override AcceptanceReport CanBeSettled
	{
		get
		{
			foreach (Quest item in Find.QuestManager.QuestsListForReading)
			{
				if (item.State != QuestState.Ongoing)
				{
					continue;
				}
				foreach (QuestPart item2 in item.PartsListForReading)
				{
					if (item2 is QuestPart_SpawnWorldObject questPart_SpawnWorldObject && questPart_SpawnWorldObject.worldObject == this)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].def.Worker.PostDestroy(parts[i]);
		}
		for (int j = 0; j < parts.Count; j++)
		{
			parts[j].PostDestroy();
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref customLabel, "customLabel");
		Scribe_Deep.Look(ref coreBackCompat, "core");
		Scribe_Collections.Look(ref parts, "parts", LookMode.Deep);
		Scribe_Values.Look(ref anyEnemiesInitially, "anyEnemiesInitially", defaultValue: false);
		Scribe_Values.Look(ref caravanAssaultSuccessfulTaleRecorded, "caravanAssaultSuccessfulTaleRecorded", defaultValue: false);
		Scribe_Values.Look(ref allEnemiesDefeatedSignalSent, "allEnemiesDefeatedSignalSent", defaultValue: false);
		Scribe_Values.Look(ref factionMustRemainHostile, "factionMustRemainHostile", defaultValue: false);
		Scribe_Values.Look(ref desiredThreatPoints, "desiredThreatPoints", 0f);
		Scribe_Values.Look(ref preventGravshipLanding, "preventGravshipLanding", defaultValue: false);
		Scribe_Values.Look(ref firedNoActiveThreatsSignal, "firedNoActiveThreatsSignal", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (coreBackCompat != null && coreBackCompat.def != null)
			{
				parts.Insert(0, new SitePart(this, coreBackCompat.def, coreBackCompat.parms));
				coreBackCompat = null;
			}
			if (parts.RemoveAll((SitePart x) => x == null || x.def == null) != 0)
			{
				Log.Error("Some site parts were null after loading.");
			}
			for (int i = 0; i < parts.Count; i++)
			{
				parts[i].site = this;
			}
			BackCompatibility.PostExposeData(this);
		}
	}

	public void AddPart(SitePart part)
	{
		if (!part.def.forceMutators.NullOrEmpty())
		{
			foreach (TileMutatorDef forceMutator in part.def.forceMutators)
			{
				base.Tile.Tile.AddMutator(forceMutator);
			}
		}
		parts.Add(part);
		part.def.Worker.Init(this, part);
	}

	protected override void Tick()
	{
		base.Tick();
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].def.Worker.SitePartWorkerTick(parts[i]);
		}
		if (base.HasMap && !firedNoActiveThreatsSignal && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map, countDormantPawnsAsHostile: true, !CanReformFoggedEnemies))
		{
			QuestUtility.SendQuestTargetSignals(questTags, "NoActiveThreats");
			firedNoActiveThreatsSignal = true;
		}
	}

	protected override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].SitePartTickInterval(delta);
		}
		if (base.HasMap)
		{
			CheckRecordAssaultSuccessfulTale();
			CheckAllEnemiesDefeated();
		}
	}

	public override void PostMapGenerate()
	{
		base.PostMapGenerate();
		Map map = base.Map;
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].def.Worker.PostMapGenerate(map);
		}
		float num = 0f;
		for (int j = 0; j < parts.Count; j++)
		{
			num = Mathf.Max(num, parts[j].def.forceExitAndRemoveMapCountdownDurationDays);
		}
		num *= MapParentTuning.SiteDetectionCountdownMultiplier.RandomInRange;
		if (!parts.Any((SitePart p) => p.def.disallowsAutomaticDetectionTimerStart) && !map.wasSpawnedViaGravShipLanding)
		{
			int ticks = Mathf.RoundToInt(num * 60000f);
			GetComponent<TimedDetectionRaids>().StartDetectionCountdown(ticks);
		}
		allEnemiesDefeatedSignalSent = false;
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].def.Worker.PostDrawExtraSelectionOverlays(parts[i]);
		}
	}

	public override void Notify_MyMapAboutToBeRemoved()
	{
		base.Notify_MyMapAboutToBeRemoved();
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].def.Worker.Notify_SiteMapAboutToBeRemoved(parts[i]);
		}
	}

	public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
	{
		alsoRemoveWorldObject = false;
		if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
		{
			return false;
		}
		foreach (PocketMapParent item in Find.World.pocketMaps.ToList())
		{
			if (item.sourceMap == base.Map && item.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
		}
		if (base.Map.AnyBuildingBlockingMapRemoval)
		{
			return false;
		}
		alsoRemoveWorldObject = !parts.Any((SitePart x) => (x.def.Worker is SitePartWorker_ConditionCauser && x.conditionCauser != null && !x.conditionCauser.Destroyed) || (x.def.Worker is SitePartWorker_RaidSource && GenHostility.AnyHostileActiveThreatToPlayer(base.Map, countDormantPawnsAsHostile: true)));
		if (parts.Any((SitePart x) => x.def.Worker is SitePartWorker_AncientAltar sitePartWorker_AncientAltar && sitePartWorker_AncientAltar.ShouldKeepMapForRelic(x)))
		{
			alsoRemoveWorldObject = false;
		}
		if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
		{
			return false;
		}
		return true;
	}

	public override void GetChildHolders(List<IThingHolder> outChildren)
	{
		base.GetChildHolders(outChildren);
		for (int i = 0; i < parts.Count; i++)
		{
			outChildren.Add(parts[i]);
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__59))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__59(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__caravan = caravan
		};
	}

	[IteratorStateMachine(typeof(_003CGetTransportersFloatMenuOptions_003Ed__60))]
	public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTransportersFloatMenuOptions_003Ed__60(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	[IteratorStateMachine(typeof(_003CGetShuttleFloatMenuOptions_003Ed__61))]
	public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetShuttleFloatMenuOptions_003Ed__61(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__62))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__62(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void CheckRecordAssaultSuccessfulTale()
	{
		if (anyEnemiesInitially && !caravanAssaultSuccessfulTaleRecorded && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map))
		{
			caravanAssaultSuccessfulTaleRecorded = true;
			if (base.Map.mapPawns.FreeColonists.Any())
			{
				TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, base.Map.mapPawns.FreeColonists.RandomElement());
			}
		}
	}

	private void CheckAllEnemiesDefeated()
	{
		if (!allEnemiesDefeatedSignalSent && base.HasMap && !GenHostility.AnyHostileActiveThreatToPlayer(base.Map, countDormantPawnsAsHostile: true, canBeFogged: true))
		{
			QuestUtility.SendQuestTargetSignals(questTags, "AllEnemiesDefeated", this.Named("SUBJECT"));
			allEnemiesDefeatedSignalSent = true;
		}
	}

	public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
	{
		if (other is Site site)
		{
			return site.MainSitePartDef == MainSitePartDef;
		}
		return false;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		tmpSitePartsLabels.Clear();
		for (int i = 0; i < parts.Count; i++)
		{
			if (parts[i].hidden || !parts[i].def.displayOnInspectPane)
			{
				continue;
			}
			if (MainSitePart == parts[i] && !parts[i].def.mainPartAllThreatsLabel.NullOrEmpty() && ActualThreatPoints > 0f)
			{
				stringBuilder.Length = 0;
				stringBuilder.Append(parts[i].def.mainPartAllThreatsLabel.CapitalizeFirst());
				break;
			}
			string postProcessedThreatLabel = parts[i].def.Worker.GetPostProcessedThreatLabel(this, parts[i]);
			if (!postProcessedThreatLabel.NullOrEmpty())
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(postProcessedThreatLabel.CapitalizeFirst());
			}
		}
		return stringBuilder.ToString();
	}

	public override string GetDescription()
	{
		string text = MainSitePartDef.description;
		string description = base.GetDescription();
		if (!description.NullOrEmpty())
		{
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			text += description;
		}
		return text;
	}
}
