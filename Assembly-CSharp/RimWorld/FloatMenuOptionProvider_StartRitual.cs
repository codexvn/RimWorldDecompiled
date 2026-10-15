using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class FloatMenuOptionProvider_StartRitual : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public Thing clickedThing;

		public FloatMenuContext context;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_1
	{
		public Precept_Ritual ritual;

		public _003C_003Ec__DisplayClass8_0 CS_0024_003C_003E8__locals1;

		internal bool _003CGetOptionsFor_003Eb__2(Precept_Role r)
		{
			return r.def == ritual.def.sourcePawnRoleDef;
		}

		internal bool _003CGetOptionsFor_003Eb__3(Ability a)
		{
			return a.def == ritual.def.sourceAbilityDef;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_2
	{
		public RitualObligation obligation;

		public _003C_003Ec__DisplayClass8_1 CS_0024_003C_003E8__locals2;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			CS_0024_003C_003E8__locals2.ritual.ShowRitualBeginWindow(CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.clickedThing, obligation, CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__8 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private _003C_003Ec__DisplayClass8_0 _003C_003E8__1;

		private _003C_003Ec__DisplayClass8_1 _003C_003E8__2;

		private _003C_003Ec__DisplayClass8_2 _003C_003E8__3;

		private IEnumerator<Ideo> _003C_003E7__wrap1;

		private Ideo _003Cideo_003E5__3;

		private int _003Ci_003E5__4;

		private bool _003CdisabledReasonSet_003E5__5;

		private string _003CdisableReason_003E5__6;

		private List<RitualObligation>.Enumerator _003C_003E7__wrap6;

		private bool _003Cdisabled_003E5__8;

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
		public _003CGetOptionsFor_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 1u)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E8__2 = null;
			_003C_003E8__3 = null;
			_003C_003E7__wrap1 = null;
			_003Cideo_003E5__3 = null;
			_003CdisableReason_003E5__6 = null;
			_003C_003E7__wrap6 = default(List<RitualObligation>.Enumerator);
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
					_003C_003E8__1 = new _003C_003Ec__DisplayClass8_0();
					_003C_003E8__1.clickedThing = clickedThing;
					_003C_003E8__1.context = context;
					showingGizmosForRitualsTmp.Clear();
					_003C_003E7__wrap1 = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0750;
				case 1:
					_003C_003E1__state = -4;
					showingGizmosForRitualsTmp.Add(_003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.sourcePattern);
					if (_003Cdisabled_003E5__8 || !_003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.mergeGizmosForObligations)
					{
						_003C_003E8__3 = null;
						goto IL_0414;
					}
					goto IL_0424;
				case 2:
					{
						_003C_003E1__state = -3;
						showingGizmosForRitualsTmp.Add(_003C_003E8__2.ritual.sourcePattern);
						goto IL_0715;
					}
					IL_0414:
					while (_003C_003E7__wrap6.MoveNext())
					{
						_003C_003E8__3 = new _003C_003Ec__DisplayClass8_2();
						_003C_003E8__3.CS_0024_003C_003E8__locals2 = _003C_003E8__2;
						_003C_003E8__3.obligation = _003C_003E7__wrap6.Current;
						if (!_003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.CanUseTarget(_003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.clickedThing, _003C_003E8__3.obligation).canUse)
						{
							continue;
						}
						if (!_003CdisabledReasonSet_003E5__5)
						{
							_003CdisabledReasonSet_003E5__5 = true;
							_003CdisableReason_003E5__6 = _003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.behavior.CanStartRitualNow(_003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.clickedThing, _003C_003E8__3.CS_0024_003C_003E8__locals2.ritual, _003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn);
							if (_003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.abilityOnCooldownUntilTick > Find.TickManager.TicksGame)
							{
								_003CdisableReason_003E5__6 = "AbilityOnCooldown".Translate((_003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.abilityOnCooldownUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod()).Resolve();
							}
						}
						string text = _003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.GetBeginRitualText(_003C_003E8__3.obligation);
						_003Cdisabled_003E5__8 = !_003CdisableReason_003E5__6.NullOrEmpty();
						if (_003Cdisabled_003E5__8)
						{
							text = text + " (" + _003CdisableReason_003E5__6 + ")";
						}
						Action action = delegate
						{
							_003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.ShowRitualBeginWindow(_003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.clickedThing, _003C_003E8__3.obligation, _003C_003E8__3.CS_0024_003C_003E8__locals2.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn);
						};
						_003C_003E2__current = new FloatMenuOption(text, (!_003Cdisabled_003E5__8) ? action : null, _003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.Icon, (!_003C_003E8__3.CS_0024_003C_003E8__locals2.ritual.def.mergeRitualGizmosFromAllIdeos) ? _003Cideo_003E5__3.Color : Color.white);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0424;
					IL_0715:
					_003C_003E8__2 = null;
					goto IL_071c;
					IL_0750:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cideo_003E5__3 = _003C_003E7__wrap1.Current;
						_003Ci_003E5__4 = 0;
						goto IL_072e;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
					IL_0424:
					_003C_003Em__Finally2();
					_003C_003E7__wrap6 = default(List<RitualObligation>.Enumerator);
					_003CdisableReason_003E5__6 = null;
					goto IL_0715;
					IL_072e:
					if (_003Ci_003E5__4 < _003Cideo_003E5__3.PreceptsListForReading.Count)
					{
						_003C_003E8__2 = new _003C_003Ec__DisplayClass8_1();
						_003C_003E8__2.CS_0024_003C_003E8__locals1 = _003C_003E8__1;
						Precept precept = _003Cideo_003E5__3.PreceptsListForReading[_003Ci_003E5__4];
						if (precept.def.showRitualFloatMenuOption)
						{
							_003C_003E8__2.ritual = precept as Precept_Ritual;
							if (_003C_003E8__2.ritual != null)
							{
								if (_003C_003E8__2.ritual.def.mergeRitualGizmosFromAllIdeos && showingGizmosForRitualsTmp.Contains(_003C_003E8__2.ritual.sourcePattern))
								{
									goto IL_071c;
								}
								if (!_003C_003E8__2.ritual.activeObligations.NullOrEmpty())
								{
									_003CdisabledReasonSet_003E5__5 = false;
									_003CdisableReason_003E5__6 = null;
									_003C_003E7__wrap6 = _003C_003E8__2.ritual.activeObligations.GetEnumerator();
									_003C_003E1__state = -4;
									goto IL_0414;
								}
								if (_003C_003E8__2.ritual.isAnytime && _003C_003E8__2.ritual.ShouldShowGizmo(_003C_003E8__2.CS_0024_003C_003E8__locals1.clickedThing))
								{
									TaggedString beginRitualText = _003C_003E8__2.ritual.GetBeginRitualText();
									RitualTargetUseReport ritualTargetUseReport = _003C_003E8__2.ritual.CanUseTarget(_003C_003E8__2.CS_0024_003C_003E8__locals1.clickedThing, null);
									string reason = _003C_003E8__2.ritual.behavior.CanStartRitualNow(_003C_003E8__2.CS_0024_003C_003E8__locals1.clickedThing, _003C_003E8__2.ritual, _003C_003E8__2.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn);
									if (_003C_003E8__2.ritual.abilityOnCooldownUntilTick > Find.TickManager.TicksGame)
									{
										reason = "AbilityOnCooldown".Translate((_003C_003E8__2.ritual.abilityOnCooldownUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod()).Resolve();
									}
									else if (_003C_003E8__2.ritual.def.sourcePawnRoleDef != null && _003C_003E8__2.ritual.def.sourceAbilityDef != null)
									{
										Precept_Role precept_Role = _003C_003E8__2.ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == _003C_003E8__2.ritual.def.sourcePawnRoleDef);
										if (precept_Role != null && precept_Role is Precept_RoleSingle precept_RoleSingle && precept_RoleSingle.ChosenPawnSingle() != null)
										{
											precept_RoleSingle.AbilitiesForReading.FirstOrDefault((Ability a) => a.def == _003C_003E8__2.ritual.def.sourceAbilityDef)?.GizmoDisabled(out reason);
										}
									}
									if (!reason.NullOrEmpty())
									{
										beginRitualText += " (" + reason + ")";
									}
									else if (!ritualTargetUseReport.failReason.NullOrEmpty())
									{
										beginRitualText += " (" + ritualTargetUseReport.failReason + ")";
									}
									_003C_003E2__current = new FloatMenuOption(beginRitualText, (reason.NullOrEmpty() && ritualTargetUseReport.failReason.NullOrEmpty()) ? new Action(Action) : null, _003C_003E8__2.ritual.Icon, (!_003C_003E8__2.ritual.def.mergeRitualGizmosFromAllIdeos) ? _003Cideo_003E5__3.Color : Color.white);
									_003C_003E1__state = 2;
									return true;
								}
							}
							goto IL_0715;
						}
						goto IL_071c;
					}
					_003Cideo_003E5__3 = null;
					goto IL_0750;
					IL_071c:
					_003Ci_003E5__4++;
					goto IL_072e;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			void Action()
			{
				((_003C_003Ec__DisplayClass8_1)this).ritual.ShowRitualBeginWindow(((_003C_003Ec__DisplayClass8_1)this).CS_0024_003C_003E8__locals1.clickedThing, null, ((_003C_003Ec__DisplayClass8_1)this).CS_0024_003C_003E8__locals1.context.FirstSelectedPawn);
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
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap6/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__8 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__8(0);
			}
			_003CGetOptionsFor_003Ed__.clickedThing = _003C_003E3__clickedThing;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private static HashSet<RitualPatternDef> showingGizmosForRitualsTmp = new HashSet<RitualPatternDef>();

	protected override bool Drafted => false;

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	protected override bool AppliesInt(FloatMenuContext context)
	{
		return ModsConfig.IdeologyActive;
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__8))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__8(-2)
		{
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}
}
