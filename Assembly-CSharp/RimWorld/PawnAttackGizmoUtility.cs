using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class PawnAttackGizmoUtility
{
	[CompilerGenerated]
	private sealed class _003CGetAttackGizmos_003Ed__0 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

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
		public _003CGetAttackGizmos_003Ed__0(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (ShouldUseMeleeAttackGizmo(pawn))
				{
					_003C_003E2__current = GetMeleeAttackGizmo(pawn);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0050;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0050;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0050:
				if (ShouldUseSquadAttackGizmo())
				{
					_003C_003E2__current = GetSquadAttackGizmo(pawn);
					_003C_003E1__state = 2;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetAttackGizmos_003Ed__0 _003CGetAttackGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAttackGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetAttackGizmos_003Ed__ = new _003CGetAttackGizmos_003Ed__0(0);
			}
			_003CGetAttackGizmos_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGetAttackGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CGetAttackGizmos_003Ed__0))]
	public static IEnumerable<Gizmo> GetAttackGizmos(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAttackGizmos_003Ed__0(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public static bool CanShowEquipmentGizmos()
	{
		return !AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons();
	}

	private static bool ShouldUseSquadAttackGizmo()
	{
		if (AtLeastOneSelectedPlayerPawnHasRangedWeapon())
		{
			return AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons();
		}
		return false;
	}

	private static bool CanOrderPlayerPawn(Pawn pawn)
	{
		if (!pawn.IsColonistPlayerControlled && !pawn.IsColonyMechPlayerControlled)
		{
			return pawn.IsColonySubhumanPlayerControlled;
		}
		return true;
	}

	private static Gizmo GetSquadAttackGizmo(Pawn pawn)
	{
		Command_Target command_Target = new Command_Target();
		command_Target.defaultLabel = "CommandSquadAttack".Translate();
		command_Target.defaultDesc = "CommandSquadAttackDesc".Translate();
		command_Target.hotKey = KeyBindingDefOf.Misc1;
		command_Target.icon = TexCommand.SquadAttack;
		command_Target.targetingParams = TargetingParameters.ForAttackAny();
		command_Target.targetingParams.canTargetLocations = AllSelectedPlayerPawnsCanTargetLocations();
		if (FloatMenuUtility.GetAttackAction(pawn, LocalTargetInfo.Invalid, out var failStr) == null)
		{
			command_Target.Disable(failStr.CapitalizeFirst() + ".");
		}
		command_Target.action = delegate(LocalTargetInfo target)
		{
			foreach (Pawn item in Find.Selector.SelectedObjects.Where((object x) => x is Pawn pawn2 && CanOrderPlayerPawn(pawn2) && pawn2.Drafted).Cast<Pawn>())
			{
				string failStr2;
				Action attackAction = FloatMenuUtility.GetAttackAction(item, target, out failStr2);
				if (attackAction != null)
				{
					attackAction();
				}
				else if (!failStr2.NullOrEmpty())
				{
					Messages.Message(failStr2, target.Thing, MessageTypeDefOf.RejectInput, historical: false);
				}
			}
		};
		command_Target.onUpdate = delegate
		{
			foreach (Pawn item2 in Find.Selector.SelectedObjects.Where((object x) => x is Pawn pawn3 && CanOrderPlayerPawn(pawn3) && pawn3.Drafted).Cast<Pawn>())
			{
				ThingWithComps thingWithComps = item2.equipment.AllEquipmentListForReading.FirstOrDefault((ThingWithComps w) => w.def.IsRangedWeapon);
				if (thingWithComps != null && thingWithComps.TryGetComp<CompEquippable>(out var comp))
				{
					Verb verb = comp.AllVerbs.FirstOrDefault((Verb v) => v.verbProps.hasStandardCommand);
					verb?.verbProps.DrawRadiusRing(item2.Position, verb);
				}
			}
		};
		return command_Target;
	}

	private static bool ShouldUseMeleeAttackGizmo(Pawn pawn)
	{
		if (!pawn.Drafted)
		{
			return false;
		}
		if (!AtLeastOneSelectedPlayerPawnHasRangedWeapon() && !AtLeastOneSelectedPlayerPawnHasNoWeapon())
		{
			return AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons();
		}
		return true;
	}

	public static Gizmo GetMeleeAttackGizmo(Pawn pawn)
	{
		Command_Target command_Target = new Command_Target();
		command_Target.defaultLabel = "CommandMeleeAttack".Translate();
		command_Target.defaultDesc = "CommandMeleeAttackDesc".Translate();
		command_Target.targetingParams = TargetingParameters.ForAttackAny();
		command_Target.hotKey = KeyBindingDefOf.Misc2;
		command_Target.icon = TexCommand.AttackMelee;
		if (FloatMenuUtility.GetMeleeAttackAction(pawn, LocalTargetInfo.Invalid, out var failStr) == null)
		{
			command_Target.Disable(failStr.CapitalizeFirst() + ".");
		}
		command_Target.action = delegate(LocalTargetInfo target)
		{
			foreach (Pawn item in Find.Selector.SelectedObjects.Where((object x) => x is Pawn pawn2 && CanOrderPlayerPawn(pawn2) && pawn2.Drafted).Cast<Pawn>())
			{
				string failStr2;
				Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(item, target, out failStr2);
				if (meleeAttackAction != null)
				{
					meleeAttackAction();
				}
				else if (!failStr2.NullOrEmpty())
				{
					Messages.Message(failStr2, target.Thing, MessageTypeDefOf.RejectInput, historical: false);
				}
			}
		};
		return command_Target;
	}

	private static bool AtLeastOneSelectedPlayerPawnHasRangedWeapon()
	{
		List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
		for (int i = 0; i < selectedObjectsListForReading.Count; i++)
		{
			if (selectedObjectsListForReading[i] is Pawn pawn && CanOrderPlayerPawn(pawn) && pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
			{
				return true;
			}
		}
		return false;
	}

	private static bool AtLeastOneSelectedPlayerPawnHasNoWeapon()
	{
		List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
		for (int i = 0; i < selectedObjectsListForReading.Count; i++)
		{
			if (selectedObjectsListForReading[i] is Pawn pawn && CanOrderPlayerPawn(pawn) && (pawn.equipment == null || pawn.equipment.Primary == null))
			{
				return true;
			}
		}
		return false;
	}

	private static bool AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons()
	{
		if (Find.Selector.NumSelected <= 1)
		{
			return false;
		}
		ThingDef thingDef = null;
		bool flag = false;
		List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
		for (int i = 0; i < selectedObjectsListForReading.Count; i++)
		{
			if (selectedObjectsListForReading[i] is Pawn pawn && CanOrderPlayerPawn(pawn))
			{
				ThingDef thingDef2 = ((pawn.equipment != null && pawn.equipment.Primary != null) ? pawn.equipment.Primary.def : null);
				if (!flag)
				{
					thingDef = thingDef2;
					flag = true;
				}
				else if (thingDef2 != thingDef)
				{
					return true;
				}
			}
		}
		return false;
	}

	private static bool AllSelectedPlayerPawnsCanTargetLocations()
	{
		foreach (object selectedObject in Find.Selector.SelectedObjects)
		{
			if (selectedObject is Pawn pawn && CanOrderPlayerPawn(pawn) && pawn.Drafted)
			{
				if (pawn.equipment.Primary == null || pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
				{
					return false;
				}
				if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.targetParams.canTargetLocations)
				{
					return false;
				}
			}
		}
		return true;
	}
}
