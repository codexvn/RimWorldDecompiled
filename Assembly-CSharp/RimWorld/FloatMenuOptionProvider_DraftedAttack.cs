using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class FloatMenuOptionProvider_DraftedAttack : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Thing clickedThing;

		public Action rangedAction;

		public Action meleeAction;

		internal void _003CGetSingleSelectAttackOptions_003Eb__0()
		{
			FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, FleckDefOf.FeedbackShoot);
			rangedAction();
		}

		internal void _003CGetSingleSelectAttackOptions_003Eb__1()
		{
			FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, FleckDefOf.FeedbackMelee);
			meleeAction();
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

		public FloatMenuOptionProvider_DraftedAttack _003C_003E4__this;

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
		public _003CGetOptionsFor_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				FloatMenuOptionProvider_DraftedAttack floatMenuOptionProvider_DraftedAttack = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!CanTarget(clickedThing))
					{
						return false;
					}
					if (context.IsMultiselect)
					{
						FloatMenuOption multiselectAttackOption = floatMenuOptionProvider_DraftedAttack.GetMultiselectAttackOption(clickedThing, context);
						if (multiselectAttackOption != null)
						{
							_003C_003E2__current = multiselectAttackOption;
							_003C_003E1__state = 1;
							return true;
						}
						break;
					}
					_003C_003E7__wrap1 = GetSingleSelectAttackOptions(clickedThing, context).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00d1;
				case 1:
					_003C_003E1__state = -1;
					break;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_00d1;
					}
					IL_00d1:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
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
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
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

	[CompilerGenerated]
	private sealed class _003CGetSingleSelectAttackOptions_003Ed__10 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private _003C_003Ec__DisplayClass10_0 _003C_003E8__1;

		private Pawn _003Cpawn_003E5__2;

		private bool _003Chostile_003E5__3;

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
		public _003CGetSingleSelectAttackOptions_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003Cpawn_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			string failStr;
			string label2;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass10_0();
				_003C_003E8__1.clickedThing = clickedThing;
				failStr = null;
				_003Cpawn_003E5__2 = context.FirstSelectedPawn;
				_003Chostile_003E5__3 = _003C_003E8__1.clickedThing.HostileTo(Faction.OfPlayer);
				if (ModsConfig.BiotechActive && _003Cpawn_003E5__2.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(_003Cpawn_003E5__2, _003C_003E8__1.clickedThing))
				{
					failStr = "OutOfCommandRange".Translate();
					_003C_003E2__current = new FloatMenuOption("Attack".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing) + ": " + failStr, null);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E8__1.rangedAction = GetRangedAttackAction(_003Cpawn_003E5__2, _003C_003E8__1.clickedThing, out var label, out failStr);
				if (_003C_003E8__1.rangedAction != null)
				{
					_003C_003E2__current = new FloatMenuOption(label, delegate
					{
						FleckMaker.Static(_003C_003E8__1.clickedThing.DrawPos, _003C_003E8__1.clickedThing.Map, FleckDefOf.FeedbackShoot);
						_003C_003E8__1.rangedAction();
					}, MenuOptionPriority.AttackEnemy)
					{
						Priority = (_003Chostile_003E5__3 ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow),
						autoTakeable = (_003Chostile_003E5__3 || (_003C_003E8__1.clickedThing.def.building?.quickTargetable ?? false)),
						autoTakeablePriority = 40f
					};
					_003C_003E1__state = 2;
					return true;
				}
				if (!failStr.NullOrEmpty())
				{
					_003C_003E2__current = new FloatMenuOption((label ?? ((string)"Attack".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing))) + ": " + failStr, null);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_026b;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				goto IL_026b;
			case 3:
				_003C_003E1__state = -1;
				goto IL_026b;
			case 4:
				_003C_003E1__state = -1;
				break;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_026b:
				_003C_003E8__1.meleeAction = GetMeleeAttackAction(_003Cpawn_003E5__2, _003C_003E8__1.clickedThing, out label2, out failStr);
				if (_003C_003E8__1.meleeAction != null)
				{
					_003C_003E2__current = new FloatMenuOption(label2, delegate
					{
						FleckMaker.Static(_003C_003E8__1.clickedThing.DrawPos, _003C_003E8__1.clickedThing.Map, FleckDefOf.FeedbackMelee);
						_003C_003E8__1.meleeAction();
					}, MenuOptionPriority.AttackEnemy)
					{
						Priority = (_003Chostile_003E5__3 ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow),
						autoTakeable = (_003Chostile_003E5__3 || (_003C_003E8__1.clickedThing.def.building?.quickTargetable ?? false)),
						autoTakeablePriority = 30f
					};
					_003C_003E1__state = 4;
					return true;
				}
				if (!failStr.NullOrEmpty())
				{
					_003C_003E2__current = new FloatMenuOption((label2 ?? ((string)"Attack".Translate(_003C_003E8__1.clickedThing.Label, _003C_003E8__1.clickedThing))) + ": " + failStr, null);
					_003C_003E1__state = 5;
					return true;
				}
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
			_003CGetSingleSelectAttackOptions_003Ed__10 _003CGetSingleSelectAttackOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetSingleSelectAttackOptions_003Ed__ = this;
			}
			else
			{
				_003CGetSingleSelectAttackOptions_003Ed__ = new _003CGetSingleSelectAttackOptions_003Ed__10(0);
			}
			_003CGetSingleSelectAttackOptions_003Ed__.clickedThing = _003C_003E3__clickedThing;
			_003CGetSingleSelectAttackOptions_003Ed__.context = _003C_003E3__context;
			return _003CGetSingleSelectAttackOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private static readonly List<Pawn> tmpPawns = new List<Pawn>();

	protected override bool Drafted => true;

	protected override bool Undrafted => false;

	protected override bool Multiselect => true;

	protected override bool MechanoidCanDo => true;

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__8))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__8(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}

	private static bool CanTarget(Thing clickedThing)
	{
		if (clickedThing.def.noRightClickDraftAttack && clickedThing.HostileTo(Faction.OfPlayer))
		{
			return false;
		}
		if (clickedThing.def.IsNonDeconstructibleAttackableBuilding)
		{
			return true;
		}
		BuildingProperties building = clickedThing.def.building;
		if (building != null && building.quickTargetable)
		{
			return true;
		}
		if (!clickedThing.def.destroyable)
		{
			return false;
		}
		if (clickedThing.HostileTo(Faction.OfPlayer))
		{
			return true;
		}
		if (clickedThing is Pawn p && p.NonHumanlikeOrWildMan())
		{
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetSingleSelectAttackOptions_003Ed__10))]
	private static IEnumerable<FloatMenuOption> GetSingleSelectAttackOptions(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetSingleSelectAttackOptions_003Ed__10(-2)
		{
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}

	private FloatMenuOption GetMultiselectAttackOption(Thing clickedThing, FloatMenuContext context)
	{
		tmpPawns.Clear();
		string label = null;
		bool flag = clickedThing.HostileTo(Faction.OfPlayer);
		string failStr = null;
		foreach (Pawn validSelectedPawn in context.ValidSelectedPawns)
		{
			if (GetAttackAction(validSelectedPawn, clickedThing, out label, out failStr) != null)
			{
				tmpPawns.Add(validSelectedPawn);
			}
		}
		if (tmpPawns.Count == 0)
		{
			if (failStr != null)
			{
				return new FloatMenuOption((label ?? ((string)"Attack".Translate(clickedThing.Label, clickedThing))) + ": " + failStr, null);
			}
			return null;
		}
		FleckDef fleck = (FloatMenuUtility.UseRangedAttack(tmpPawns[0]) ? FleckDefOf.FeedbackShoot : FleckDefOf.FeedbackMelee);
		return new FloatMenuOption(label ?? ((string)"Attack".Translate(clickedThing.Label, clickedThing)), delegate
		{
			FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, fleck);
			foreach (Pawn tmpPawn in tmpPawns)
			{
				GetAttackAction(tmpPawn, clickedThing, out var _, out var _)?.Invoke();
			}
		}, MenuOptionPriority.AttackEnemy)
		{
			Priority = (flag ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow),
			autoTakeable = (flag || (clickedThing.def.building?.quickTargetable ?? false)),
			autoTakeablePriority = 40f
		};
	}

	private static Action GetAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
	{
		failStr = null;
		label = "Attack".Translate(target.Label, target);
		if (ModsConfig.BiotechActive && pawn.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(pawn, target))
		{
			failStr = "OutOfCommandRange".Translate();
			return null;
		}
		Action rangedAttackAction = GetRangedAttackAction(pawn, target, out label, out failStr);
		if (rangedAttackAction != null)
		{
			return rangedAttackAction;
		}
		return GetMeleeAttackAction(pawn, target, out label, out failStr);
	}

	private static Action GetRangedAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
	{
		failStr = null;
		label = "FireAt".Translate(target.Label, target);
		if (!FloatMenuUtility.UseRangedAttack(pawn))
		{
			return null;
		}
		label = "FireAt".Translate(target.Label, target);
		return FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
	}

	private static Action GetMeleeAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
	{
		failStr = null;
		label = "Attack".Translate(target.Label, target);
		if (target is Pawn { Downed: not false })
		{
			label = "MeleeAttackToDeath".Translate(target.Label, target);
		}
		else
		{
			label = "MeleeAttack".Translate(target.Label, target);
		}
		return FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr);
	}

	[Obsolete]
	protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
	{
		return base.GetSingleOptionFor(clickedThing, context);
	}

	[Obsolete]
	private static FloatMenuOption GetSingleSelectAttackOption(Thing clickedThing, FloatMenuContext context)
	{
		string label;
		string failStr;
		Action action = GetAttackAction(context.FirstSelectedPawn, clickedThing, out label, out failStr);
		FleckDef fleck = (FloatMenuUtility.UseRangedAttack(context.FirstSelectedPawn) ? FleckDefOf.FeedbackShoot : FleckDefOf.FeedbackMelee);
		if (action == null)
		{
			if (!failStr.NullOrEmpty())
			{
				return new FloatMenuOption((label ?? ((string)"Attack".Translate(clickedThing.Label, clickedThing))) + ": " + failStr, null);
			}
			return null;
		}
		return new FloatMenuOption(label ?? ((string)"Attack".Translate(clickedThing.Label, clickedThing)), delegate
		{
			FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, fleck);
			action();
		}, MenuOptionPriority.AttackEnemy);
	}
}
