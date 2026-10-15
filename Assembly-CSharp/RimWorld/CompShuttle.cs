using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompShuttle : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass55_0
	{
		public CompShuttle _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			if (!_003C_003E4__this.Transporter.LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle(_003C_003E4__this.Transporter));
			}
			Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, _003C_003E4__this.parent);
			selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__55 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompShuttle _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

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
		public _003CCompFloatMenuOptions_003Ed__55(int _003C_003E1__state)
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
			CompShuttle compShuttle = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass55_0 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass55_0
				{
					_003C_003E4__this = _003C_003E4__this,
					selPawn = selPawn
				};
				if (!CS_0024_003C_003E8__locals6.selPawn.CanReach(compShuttle.parent, PathEndMode.Touch, Danger.Deadly))
				{
					return false;
				}
				string text = "EnterShuttle".Translate();
				if (!compShuttle.IsAllowedNow(CS_0024_003C_003E8__locals6.selPawn))
				{
					_003C_003E2__current = new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new FloatMenuOption(text, delegate
				{
					if (!CS_0024_003C_003E8__locals6._003C_003E4__this.Transporter.LoadingInProgressOrReadyToLaunch)
					{
						TransporterUtility.InitiateLoading(Gen.YieldSingle(CS_0024_003C_003E8__locals6._003C_003E4__this.Transporter));
					}
					Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, CS_0024_003C_003E8__locals6._003C_003E4__this.parent);
					CS_0024_003C_003E8__locals6.selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				});
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__55 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__55(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__54 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompShuttle _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__54(int _003C_003E1__state)
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
			case 4:
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
				CompShuttle CS_0024_003C_003E8__locals14 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals14).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_0101;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01ad;
				case 4:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals14.shipParent != null && CS_0024_003C_003E8__locals14.shipParent.ShowGizmos)
					{
						IEnumerable<Gizmo> jobGizmos = CS_0024_003C_003E8__locals14.shipParent.curJob.GetJobGizmos();
						if (jobGizmos != null)
						{
							_003C_003E7__wrap1 = jobGizmos.GetEnumerator();
							_003C_003E1__state = -4;
							goto IL_0101;
						}
					}
					goto IL_011b;
					IL_0101:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_011b;
					IL_01ad:
					_003C_003E7__wrap1 = QuestUtility.GetQuestRelatedGizmos(CS_0024_003C_003E8__locals14.parent).GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_011b:
					if (CS_0024_003C_003E8__locals14.ShowLoadingGizmos && CS_0024_003C_003E8__locals14.Autoloadable)
					{
						_003C_003E2__current = new Command_Toggle
						{
							defaultLabel = "CommandAutoloadTransporters".Translate(),
							defaultDesc = "CommandAutoloadTransportersDesc".Translate(),
							icon = AutoloadToggleTex,
							isActive = () => CS_0024_003C_003E8__locals14.autoload,
							toggleAction = delegate
							{
								CS_0024_003C_003E8__locals14.autoload = !CS_0024_003C_003E8__locals14.autoload;
								if (CS_0024_003C_003E8__locals14.autoload && !CS_0024_003C_003E8__locals14.Transporter.LoadingInProgressOrReadyToLaunch)
								{
									TransporterUtility.InitiateLoading(Gen.YieldSingle(CS_0024_003C_003E8__locals14.Transporter));
								}
								CS_0024_003C_003E8__locals14.CheckAutoload();
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01ad;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current3 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 4;
					return true;
				}
				_003C_003Em__Finally3();
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__54 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__54(0)
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
	private sealed class _003CCompMultiSelectFloatMenuOptions_003Ed__57 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<Pawn> selPawns;

		public IEnumerable<Pawn> _003C_003E3__selPawns;

		public CompShuttle _003C_003E4__this;

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
		public _003CCompMultiSelectFloatMenuOptions_003Ed__57(int _003C_003E1__state)
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
			CompShuttle CS_0024_003C_003E8__locals6 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				tmpAllowedPawns.Clear();
				string text = "EnterShuttle".Translate();
				foreach (Pawn item in selPawns)
				{
					if (item.CanReach(CS_0024_003C_003E8__locals6.parent, PathEndMode.Touch, Danger.Deadly))
					{
						tmpAllowedPawns.Add(item);
					}
				}
				if (!tmpAllowedPawns.Any())
				{
					_003C_003E2__current = new FloatMenuOption(text + " (" + "NoPath".Translate() + ")", null);
					_003C_003E1__state = 1;
					return true;
				}
				for (int num2 = tmpAllowedPawns.Count - 1; num2 >= 0; num2--)
				{
					if (!CS_0024_003C_003E8__locals6.IsAllowedNow(tmpAllowedPawns[num2]))
					{
						tmpAllowedPawns.RemoveAt(num2);
					}
				}
				if (!tmpAllowedPawns.Any())
				{
					_003C_003E2__current = new FloatMenuOption(text + " (" + "NotAllowed".Translate() + ")", null);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = new FloatMenuOption(text, delegate
				{
					if (!CS_0024_003C_003E8__locals6.Transporter.LoadingInProgressOrReadyToLaunch)
					{
						TransporterUtility.InitiateLoading(Gen.YieldSingle(CS_0024_003C_003E8__locals6.Transporter));
					}
					for (int i = 0; i < tmpAllowedPawns.Count; i++)
					{
						Pawn pawn = tmpAllowedPawns[i];
						if (pawn.CanReach(CS_0024_003C_003E8__locals6.parent, PathEndMode.Touch, Danger.Deadly) && !pawn.Downed && !pawn.Dead && pawn.Spawned)
						{
							Job job = JobMaker.MakeJob(JobDefOf.EnterTransporter, CS_0024_003C_003E8__locals6.parent);
							tmpAllowedPawns[i].jobs.TryTakeOrderedJob(job, JobTag.Misc);
						}
					}
				});
				_003C_003E1__state = 3;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompMultiSelectFloatMenuOptions_003Ed__57 _003CCompMultiSelectFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompMultiSelectFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompMultiSelectFloatMenuOptions_003Ed__ = new _003CCompMultiSelectFloatMenuOptions_003Ed__57(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompMultiSelectFloatMenuOptions_003Ed__.selPawns = _003C_003E3__selPawns;
			return _003CCompMultiSelectFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_ContainedPawns_003Ed__40 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompShuttle _003C_003E4__this;

		private ThingOwner _003Cthings_003E5__2;

		private int _003Ci_003E5__3;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_ContainedPawns_003Ed__40(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cthings_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompShuttle compShuttle = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0069;
			}
			_003C_003E1__state = -1;
			_003Cthings_003E5__2 = compShuttle.Transporter.innerContainer;
			_003Ci_003E5__3 = 0;
			goto IL_0079;
			IL_0069:
			_003Ci_003E5__3++;
			goto IL_0079;
			IL_0079:
			if (_003Ci_003E5__3 < _003Cthings_003E5__2.Count)
			{
				if (_003Cthings_003E5__2[_003Ci_003E5__3] is Pawn pawn)
				{
					_003C_003E2__current = pawn;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0069;
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_ContainedPawns_003Ed__40 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ContainedPawns_003Ed__40(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_RequiredPawns_003Ed__45 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompShuttle _003C_003E4__this;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		Pawn IEnumerator<Pawn>.Current
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
		public _003Cget_RequiredPawns_003Ed__45(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompShuttle compShuttle = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = compShuttle.requiredPawns.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					if (!current.Dead && ((!current.Downed && current.MapHeld == compShuttle.parent.MapHeld) || !compShuttle.pawnsToIgnoreIfDownedOfNotOnTheMap.Contains(current)))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003Cget_RequiredPawns_003Ed__45 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_RequiredPawns_003Ed__45(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public List<ThingDefCount> requiredItems = new List<ThingDefCount>();

	public List<Pawn> requiredPawns = new List<Pawn>();

	public List<Pawn> pawnsToIgnoreIfDownedOfNotOnTheMap = new List<Pawn>();

	public int requiredColonistCount;

	public bool requireAllColonistsOnMap;

	public int maxColonistCount = -1;

	public bool acceptColonists;

	public bool acceptChildren;

	public bool acceptColonyPrisoners;

	public bool onlyAcceptColonists;

	public bool onlyAcceptHealthy;

	public bool permitShuttle;

	public bool allowSlaves;

	public float minAge;

	private bool autoload;

	public TransportShip shipParent;

	private CompRefuelable cachedCompRefuelable;

	private CompTransporter cachedCompTransporter;

	private List<CompTransporter> cachedTransporterList;

	private const int CheckAutoloadIntervalTicks = 120;

	private const int CheckUnloadPawnIntervalTicks = 600;

	private static readonly Texture2D AutoloadToggleTex = ContentFinder<Texture2D>.Get("UI/Commands/Autoload");

	private const float MinPilotingSkill = 0.1f;

	private static readonly List<ThingDefCount> tmpRequiredItemsWithoutDuplicates = new List<ThingDefCount>();

	private static List<Pawn> tmpAllowedPawns = new List<Pawn>();

	private static List<string> tmpRequiredLabels = new List<string>();

	private static List<ThingDefCount> tmpRequiredItems = new List<ThingDefCount>();

	private static List<Pawn> tmpRequiredPawns = new List<Pawn>();

	private static List<Pawn> tmpAllSendablePawns = new List<Pawn>();

	private static List<Thing> tmpAllSendableItems = new List<Thing>();

	private static List<Pawn> tmpRequiredPawnsPossibleToSend = new List<Pawn>();

	public bool IsPlayerShuttle
	{
		get
		{
			if (ModLister.OdysseyInstalled)
			{
				return Props.shipDef?.playerShuttle ?? false;
			}
			return false;
		}
	}

	public bool Autoload => autoload;

	public List<CompTransporter> TransportersInGroup => Transporter.TransportersInGroup(parent.Map);

	public CompProperties_Shuttle Props => (CompProperties_Shuttle)props;

	public bool ShowLoadingGizmos
	{
		get
		{
			if (IsPlayerShuttle)
			{
				return true;
			}
			if (shipParent != null && !shipParent.ShowGizmos)
			{
				return false;
			}
			if (parent.Faction != null)
			{
				return parent.Faction == Faction.OfPlayer;
			}
			return true;
		}
	}

	public CompTransporter Transporter => cachedCompTransporter ?? (cachedCompTransporter = parent.GetComp<CompTransporter>());

	private bool Autoloadable
	{
		get
		{
			if (IsPlayerShuttle)
			{
				return false;
			}
			if (cachedTransporterList == null)
			{
				cachedTransporterList = new List<CompTransporter> { Transporter };
			}
			foreach (Pawn item in TransporterUtility.AllSendablePawns(cachedTransporterList, parent.Map))
			{
				if (!IsRequired(item))
				{
					return false;
				}
			}
			foreach (Thing item2 in TransporterUtility.AllSendableItems(cachedTransporterList, parent.Map))
			{
				if (!IsRequired(item2))
				{
					return false;
				}
			}
			return true;
		}
	}

	private int ContainedColonistCount
	{
		get
		{
			int num = 0;
			ThingOwner innerContainer = Transporter.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i] is Pawn { IsFreeColonist: not false })
				{
					num++;
				}
			}
			return num;
		}
	}

	private IEnumerable<Pawn> ContainedPawns
	{
		[IteratorStateMachine(typeof(_003Cget_ContainedPawns_003Ed__40))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ContainedPawns_003Ed__40(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool AllRequiredThingsLoaded
	{
		get
		{
			ThingOwner innerContainer = Transporter.innerContainer;
			foreach (Pawn requiredPawn in RequiredPawns)
			{
				if (!innerContainer.Contains(requiredPawn))
				{
					return false;
				}
			}
			if (requireAllColonistsOnMap || requiredColonistCount > 0)
			{
				int containedColonistCount = ContainedColonistCount;
				int num = parent.Map.mapPawns.FreeColonistsCount;
				foreach (Map childPocketMap in parent.Map.ChildPocketMaps)
				{
					num += childPocketMap.mapPawns.FreeColonistsCount;
				}
				if (requireAllColonistsOnMap && containedColonistCount < num)
				{
					return false;
				}
				if (requiredColonistCount > 0 && containedColonistCount < requiredColonistCount)
				{
					return false;
				}
			}
			tmpRequiredItemsWithoutDuplicates.Clear();
			for (int i = 0; i < requiredItems.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < tmpRequiredItemsWithoutDuplicates.Count; j++)
				{
					if (tmpRequiredItemsWithoutDuplicates[j].ThingDef == requiredItems[i].ThingDef)
					{
						tmpRequiredItemsWithoutDuplicates[j] = tmpRequiredItemsWithoutDuplicates[j].WithCount(tmpRequiredItemsWithoutDuplicates[j].Count + requiredItems[i].Count);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					tmpRequiredItemsWithoutDuplicates.Add(requiredItems[i]);
				}
			}
			for (int k = 0; k < tmpRequiredItemsWithoutDuplicates.Count; k++)
			{
				int num2 = 0;
				for (int l = 0; l < innerContainer.Count; l++)
				{
					if (innerContainer[l].def == tmpRequiredItemsWithoutDuplicates[k].ThingDef)
					{
						num2 += innerContainer[l].stackCount;
					}
				}
				if (num2 < tmpRequiredItemsWithoutDuplicates[k].Count)
				{
					return false;
				}
			}
			return true;
		}
	}

	public IEnumerable<Pawn> RequiredPawns
	{
		[IteratorStateMachine(typeof(_003Cget_RequiredPawns_003Ed__45))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_RequiredPawns_003Ed__45(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public TaggedString RequiredThingsLabel
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn requiredPawn in RequiredPawns)
			{
				stringBuilder.AppendLine("  - " + requiredPawn.NameShortColored.Resolve());
			}
			for (int i = 0; i < requiredItems.Count; i++)
			{
				stringBuilder.AppendLine("  - " + requiredItems[i].LabelCap);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}
	}

	public bool HasPilot
	{
		get
		{
			ThingOwner innerContainer = Transporter.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i] is Pawn { IsFreeColonist: not false } pawn && !StatDefOf.PilotingAbility.Worker.IsDisabledFor(pawn) && pawn.GetStatValue(StatDefOf.PilotingAbility) > 0.1f)
				{
					return true;
				}
			}
			return false;
		}
	}

	public AcceptanceReport CanLaunch
	{
		get
		{
			TransportShipDef shipDef = Props.shipDef;
			if (shipDef != null && !shipDef.playerShuttle)
			{
				if (!Transporter.LoadingInProgressOrReadyToLaunch)
				{
					return "CommandLaunchGroupFailNotLoaded".Translate();
				}
				return true;
			}
			if (parent.Spawned && !HasPilot)
			{
				return "CommandShuttleNoPilot".Translate();
			}
			if (parent.Spawned)
			{
				WeatherDef curWeatherPerceived = parent.Map.weatherManager.CurWeatherPerceived;
				if (curWeatherPerceived.preventsShuttleLaunch)
				{
					return curWeatherPerceived.LabelCap;
				}
				foreach (GameCondition activeCondition in parent.Map.GameConditionManager.ActiveConditions)
				{
					if (activeCondition.def.preventShuttleLaunch)
					{
						return activeCondition.LabelCap;
					}
				}
			}
			return true;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckAnyExpansion("Shuttle"))
		{
			return;
		}
		if (!respawningAfterLoad && Props.shipDef != null)
		{
			TransportShipDef shipDef = Props.shipDef;
			if (shipDef != null && shipDef.playerShuttle)
			{
				acceptColonists = true;
				acceptChildren = true;
				acceptColonyPrisoners = true;
				allowSlaves = true;
			}
			if (shipParent == null)
			{
				shipParent = TransportShipMaker.MakeTransportShip(Props.shipDef, null, parent);
			}
		}
		base.PostSpawnSetup(respawningAfterLoad);
	}

	public void SetPawnToLeaveBehind(Func<Pawn, bool> predicate)
	{
		foreach (Pawn requiredPawn in requiredPawns)
		{
			if (predicate(requiredPawn))
			{
				pawnsToIgnoreIfDownedOfNotOnTheMap.AddUnique(requiredPawn);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__54))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__54(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__55))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__55(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CCompMultiSelectFloatMenuOptions_003Ed__57))]
	public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompMultiSelectFloatMenuOptions_003Ed__57(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawns = selPawns
		};
	}

	public override void CompTick()
	{
		if (!parent.Spawned)
		{
			return;
		}
		if (parent.IsHashIntervalTick(120))
		{
			CheckAutoload();
		}
		if (parent.IsHashIntervalTick(600))
		{
			foreach (Pawn containedPawn in ContainedPawns)
			{
				if (containedPawn.TickDeSpawned > 0 && Find.TickManager.TicksGame - containedPawn.TickDeSpawned > 60000)
				{
					ShipJob_Unload.UnloadThingFromShuttle(shipParent, containedPawn);
					containedPawn.GetLord()?.Notify_PawnLost(containedPawn, PawnLostCondition.LeftVoluntarily, null);
				}
			}
		}
		TransportShipDef shipDef = Props.shipDef;
		if (shipDef != null && shipDef.playerShuttle && !Transporter.LoadingInProgressOrReadyToLaunch && ContainedColonistCount > 0)
		{
			TransporterUtility.InitiateLoading(Gen.YieldSingle(Transporter));
		}
	}

	public override string CompInspectStringExtra()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (shipParent != null && shipParent.curJob != null)
		{
			string text = shipParent.curJob.ShipThingExtraInspectString();
			if (!text.NullOrEmpty())
			{
				stringBuilder.Append(text);
			}
		}
		tmpRequiredLabels.Clear();
		if (requireAllColonistsOnMap)
		{
			int freeColonistsCount = parent.Map.mapPawns.FreeColonistsCount;
			tmpRequiredLabels.Add(freeColonistsCount + " " + ((freeColonistsCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
		}
		else if (requiredColonistCount > 0)
		{
			tmpRequiredLabels.Add(requiredColonistCount + " " + ((requiredColonistCount > 1) ? Faction.OfPlayer.def.pawnsPlural : Faction.OfPlayer.def.pawnSingular));
		}
		foreach (Pawn requiredPawn in RequiredPawns)
		{
			if (!Transporter.innerContainer.Contains(requiredPawn))
			{
				tmpRequiredLabels.Add(requiredPawn.LabelShort);
			}
		}
		for (int i = 0; i < requiredItems.Count; i++)
		{
			if (Transporter.innerContainer.TotalStackCountOfDef(requiredItems[i].ThingDef) < requiredItems[i].Count)
			{
				tmpRequiredLabels.Add(requiredItems[i].Label);
			}
		}
		if (tmpRequiredLabels.Any())
		{
			stringBuilder.AppendInNewLine("Required".Translate() + ": " + tmpRequiredLabels.ToCommaList().CapitalizeFirst());
		}
		return stringBuilder.ToString();
	}

	public void SendLaunchedSignals()
	{
		List<CompTransporter> transportersInGroup = TransportersInGroup;
		List<Pawn> list = new List<Pawn>();
		for (int i = 0; i < transportersInGroup.Count; i++)
		{
			for (int j = 0; j < transportersInGroup[i].innerContainer.Count; j++)
			{
				if (transportersInGroup[i].innerContainer[j] is Pawn { IsColonist: not false } pawn && !requiredPawns.Contains(pawn))
				{
					list.Add(pawn);
				}
			}
		}
		if (list.Count != 0)
		{
			for (int k = 0; k < transportersInGroup.Count; k++)
			{
				QuestUtility.SendQuestTargetSignals(transportersInGroup[k].parent.questTags, "SentWithExtraColonists", transportersInGroup[k].parent.Named("SUBJECT"), list.Named("SENTCOLONISTS"));
			}
		}
		string signalPart = (AllRequiredThingsLoaded ? "SentSatisfied" : "SentUnsatisfied");
		for (int l = 0; l < transportersInGroup.Count; l++)
		{
			QuestUtility.SendQuestTargetSignals(transportersInGroup[l].parent.questTags, signalPart, transportersInGroup[l].parent.Named("SUBJECT"), transportersInGroup[l].innerContainer.ToList().Named("SENT"));
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Collections.Look(ref requiredItems, "requiredItems", LookMode.Deep);
		Scribe_Collections.Look(ref requiredPawns, "requiredPawns", LookMode.Reference);
		Scribe_Collections.Look(ref pawnsToIgnoreIfDownedOfNotOnTheMap, "pawnsToIgnoreIfDownedOfNotOnTheMap", LookMode.Reference);
		Scribe_Values.Look(ref requiredColonistCount, "requiredColonistCount", 0);
		Scribe_Values.Look(ref requireAllColonistsOnMap, "requireAllColonistsOnMap", defaultValue: false);
		Scribe_Values.Look(ref acceptColonists, "acceptColonists", defaultValue: false);
		Scribe_Values.Look(ref acceptChildren, "acceptChildren", defaultValue: true);
		Scribe_Values.Look(ref onlyAcceptColonists, "onlyAcceptColonists", defaultValue: false);
		Scribe_Values.Look(ref allowSlaves, "allowSlaves", defaultValue: false);
		Scribe_Values.Look(ref autoload, "autoload", defaultValue: false);
		Scribe_Values.Look(ref permitShuttle, "permitShuttle", defaultValue: false);
		Scribe_Values.Look(ref acceptColonyPrisoners, "acceptColonyPrisoners", defaultValue: false);
		Scribe_Values.Look(ref maxColonistCount, "maxColonistCount", -1);
		Scribe_References.Look(ref shipParent, "shipParent");
		Scribe_Values.Look(ref minAge, "minAge", 0f);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			requiredPawns.RemoveAll((Pawn x) => x == null);
		}
		if (maxColonistCount == 0)
		{
			maxColonistCount = -1;
		}
	}

	private void CheckAutoload()
	{
		if (!autoload || !Transporter.LoadingInProgressOrReadyToLaunch || !parent.Spawned)
		{
			return;
		}
		tmpRequiredItems.Clear();
		tmpRequiredItems.AddRange(requiredItems);
		tmpRequiredPawns.Clear();
		tmpRequiredPawns.AddRange(requiredPawns);
		ThingOwner innerContainer = Transporter.innerContainer;
		for (int i = 0; i < innerContainer.Count; i++)
		{
			if (innerContainer[i] is Pawn item)
			{
				tmpRequiredPawns.Remove(item);
				continue;
			}
			int num = innerContainer[i].stackCount;
			for (int j = 0; j < tmpRequiredItems.Count; j++)
			{
				if (tmpRequiredItems[j].ThingDef == innerContainer[i].def)
				{
					int num2 = Mathf.Min(tmpRequiredItems[j].Count, num);
					if (num2 > 0)
					{
						tmpRequiredItems[j] = tmpRequiredItems[j].WithCount(tmpRequiredItems[j].Count - num2);
						num -= num2;
					}
				}
			}
		}
		for (int num3 = tmpRequiredItems.Count - 1; num3 >= 0; num3--)
		{
			if (tmpRequiredItems[num3].Count <= 0)
			{
				tmpRequiredItems.RemoveAt(num3);
			}
		}
		if (tmpRequiredItems.Any() || tmpRequiredPawns.Any())
		{
			if (Transporter.leftToLoad != null)
			{
				Transporter.leftToLoad.Clear();
			}
			tmpAllSendablePawns.Clear();
			tmpAllSendablePawns.AddRange(TransporterUtility.AllSendablePawns(TransportersInGroup, parent.Map));
			tmpAllSendableItems.Clear();
			tmpAllSendableItems.AddRange(TransporterUtility.AllSendableItems(TransportersInGroup, parent.Map));
			tmpAllSendableItems.AddRange(TransporterUtility.ThingsBeingHauledTo(TransportersInGroup, parent.Map));
			tmpRequiredPawnsPossibleToSend.Clear();
			for (int k = 0; k < tmpRequiredPawns.Count; k++)
			{
				if (tmpAllSendablePawns.Contains(tmpRequiredPawns[k]))
				{
					TransferableOneWay transferableOneWay = new TransferableOneWay();
					transferableOneWay.things.Add(tmpRequiredPawns[k]);
					Transporter.AddToTheToLoadList(transferableOneWay, 1);
					tmpRequiredPawnsPossibleToSend.Add(tmpRequiredPawns[k]);
				}
			}
			for (int l = 0; l < tmpRequiredItems.Count; l++)
			{
				if (tmpRequiredItems[l].Count <= 0)
				{
					continue;
				}
				int num4 = 0;
				for (int m = 0; m < tmpAllSendableItems.Count; m++)
				{
					if (tmpAllSendableItems[m].def == tmpRequiredItems[l].ThingDef)
					{
						num4 += tmpAllSendableItems[m].stackCount;
					}
				}
				if (num4 <= 0)
				{
					continue;
				}
				TransferableOneWay transferableOneWay2 = new TransferableOneWay();
				for (int n = 0; n < tmpAllSendableItems.Count; n++)
				{
					if (tmpAllSendableItems[n].def == tmpRequiredItems[l].ThingDef)
					{
						transferableOneWay2.things.Add(tmpAllSendableItems[n]);
					}
				}
				int count = Mathf.Min(tmpRequiredItems[l].Count, num4);
				Transporter.AddToTheToLoadList(transferableOneWay2, count);
			}
			TransporterUtility.MakeLordsAsAppropriate(tmpRequiredPawnsPossibleToSend, TransportersInGroup, parent.Map);
			tmpAllSendablePawns.Clear();
			tmpAllSendableItems.Clear();
			tmpRequiredItems.Clear();
			tmpRequiredPawns.Clear();
			tmpRequiredPawnsPossibleToSend.Clear();
		}
		else
		{
			if (Transporter.leftToLoad != null)
			{
				Transporter.leftToLoad.Clear();
			}
			TransporterUtility.MakeLordsAsAppropriate(tmpRequiredPawnsPossibleToSend, TransportersInGroup, parent.Map);
		}
	}

	public virtual bool IsRequired(Thing thing)
	{
		if (thing is Pawn item)
		{
			return requiredPawns.Contains(item);
		}
		for (int i = 0; i < requiredItems.Count; i++)
		{
			if (requiredItems[i].ThingDef == thing.def && requiredItems[i].Count != 0)
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool IsAllowed(Thing t)
	{
		TransportShipDef shipDef = Props.shipDef;
		if (shipDef != null && shipDef.playerShuttle)
		{
			return true;
		}
		if (IsRequired(t))
		{
			return true;
		}
		if (acceptColonists && t is Pawn pawn && (pawn.IsColonist || (pawn.IsPrisonerOfColony && acceptColonyPrisoners) || (!onlyAcceptColonists && pawn.IsAnimal && pawn.Faction == Faction.OfPlayer)) && (!pawn.IsSlave || allowSlaves) && (!onlyAcceptColonists || !pawn.IsQuestLodger()) && (!onlyAcceptHealthy || PawnIsHealthyEnoughForShuttle(pawn)) && pawn.ageTracker.AgeBiologicalYearsFloat >= minAge)
		{
			return true;
		}
		if (!acceptChildren && t is Pawn pawn2 && pawn2.RaceProps.Humanlike && !pawn2.DevelopmentalStage.Adult())
		{
			return false;
		}
		if (permitShuttle)
		{
			if (!(t is Pawn pawn3))
			{
				return true;
			}
			if ((pawn3.Faction == Faction.OfPlayer && !pawn3.IsQuestLodger()) || pawn3.IsPrisonerOfColony)
			{
				return true;
			}
		}
		if (!(t is Pawn) && !parent.Map.IsPlayerHome && !GenHostility.AnyHostileActiveThreatToPlayer(parent.Map, countDormantPawnsAsHostile: true))
		{
			return true;
		}
		return false;
	}

	public virtual bool IsAllowedNow(Thing t)
	{
		if (!IsAllowed(t))
		{
			return false;
		}
		if (maxColonistCount == -1)
		{
			return true;
		}
		int num = 0;
		foreach (Thing item in (IEnumerable<Thing>)Transporter.innerContainer)
		{
			if (item != t && item is Pawn { IsColonist: not false })
			{
				num++;
			}
		}
		foreach (Pawn allPawn in parent.Map.mapPawns.AllPawns)
		{
			if (allPawn.jobs == null || allPawn.jobs.curDriver == null || allPawn == t)
			{
				continue;
			}
			foreach (QueuedJob item2 in allPawn.jobs.jobQueue)
			{
				if (CheckJob(item2.job))
				{
					num++;
				}
			}
			if (CheckJob(allPawn.jobs.curJob))
			{
				num++;
			}
		}
		return num < maxColonistCount;
		bool CheckJob(Job job)
		{
			if (typeof(JobDriver_EnterTransporter).IsAssignableFrom(job.def.driverClass) && job.GetTarget(TargetIndex.A).Thing == parent)
			{
				return true;
			}
			return false;
		}
	}

	private bool PawnIsHealthyEnoughForShuttle(Pawn p)
	{
		if (p.Downed || p.InMentalState || !p.health.capacities.CanBeAwake || !p.health.capacities.CapableOf(PawnCapacityDefOf.Moving) || !p.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			return false;
		}
		return true;
	}

	public virtual void CleanUpLoadingVars()
	{
		autoload = false;
	}
}
