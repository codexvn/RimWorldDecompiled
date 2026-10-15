using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_Hunt : Designator
{
	[CompilerGenerated]
	private sealed class _003CHuntablesInCell_003Ed__11 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 c;

		public IntVec3 _003C_003E3__c;

		public Designator_Hunt _003C_003E4__this;

		private List<Thing> _003CthingList_003E5__2;

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
		public _003CHuntablesInCell_003Ed__11(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CthingList_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Designator_Hunt designator_Hunt = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00a3;
			}
			_003C_003E1__state = -1;
			if (c.Fogged(designator_Hunt.Map))
			{
				return false;
			}
			_003CthingList_003E5__2 = c.GetThingList(designator_Hunt.Map);
			_003Ci_003E5__3 = 0;
			goto IL_00b3;
			IL_00b3:
			if (_003Ci_003E5__3 < _003CthingList_003E5__2.Count)
			{
				if (designator_Hunt.CanDesignateThing(_003CthingList_003E5__2[_003Ci_003E5__3]).Accepted)
				{
					_003C_003E2__current = (Pawn)_003CthingList_003E5__2[_003Ci_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00a3;
			}
			return false;
			IL_00a3:
			_003Ci_003E5__3++;
			goto IL_00b3;
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
			_003CHuntablesInCell_003Ed__11 _003CHuntablesInCell_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CHuntablesInCell_003Ed__ = this;
			}
			else
			{
				_003CHuntablesInCell_003Ed__ = new _003CHuntablesInCell_003Ed__11(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CHuntablesInCell_003Ed__.c = _003C_003E3__c;
			return _003CHuntablesInCell_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	private readonly List<Pawn> justDesignated = new List<Pawn>();

	protected override DesignationDef Designation => DesignationDefOf.Hunt;

	public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.FilledRectangle;

	public Designator_Hunt()
	{
		defaultLabel = "DesignatorHunt".Translate();
		defaultDesc = "DesignatorHuntDesc".Translate();
		icon = ContentFinder<Texture2D>.Get("UI/Designators/Hunt");
		soundDragSustain = SoundDefOf.Designate_DragStandard;
		soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
		useMouseIcon = true;
		soundSucceeded = SoundDefOf.Designate_Hunt;
		hotKey = KeyBindingDefOf.Misc11;
	}

	public override AcceptanceReport CanDesignateCell(IntVec3 c)
	{
		if (!c.InBounds(base.Map))
		{
			return false;
		}
		if (!HuntablesInCell(c).Any())
		{
			return "MessageMustDesignateHuntable".Translate();
		}
		return true;
	}

	public override void DesignateSingleCell(IntVec3 loc)
	{
		foreach (Pawn item in HuntablesInCell(loc))
		{
			DesignateThing(item);
		}
	}

	public override AcceptanceReport CanDesignateThing(Thing t)
	{
		if (t is Pawn pawn && pawn.AnimalOrWildMan() && !pawn.IsPrisonerInPrisonCell() && (pawn.Faction == null || !pawn.Faction.def.humanlikeFaction) && base.Map.designationManager.DesignationOn(pawn, Designation) == null)
		{
			return true;
		}
		return false;
	}

	public override void DesignateThing(Thing t)
	{
		base.Map.designationManager.RemoveAllDesignationsOn(t);
		base.Map.designationManager.AddDesignation(new Designation(t, Designation));
		justDesignated.Add((Pawn)t);
	}

	protected override void FinalizeDesignationSucceeded()
	{
		base.FinalizeDesignationSucceeded();
		foreach (PawnKindDef kind in justDesignated.Select((Pawn p) => p.kindDef).Distinct())
		{
			ShowDesignationWarnings(justDesignated.First((Pawn x) => x.kindDef == kind));
		}
		justDesignated.Clear();
	}

	[IteratorStateMachine(typeof(_003CHuntablesInCell_003Ed__11))]
	private IEnumerable<Pawn> HuntablesInCell(IntVec3 c)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CHuntablesInCell_003Ed__11(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__c = c
		};
	}

	public static void ShowDesignationWarnings(Pawn pawn)
	{
		CheckHunters(pawn.MapHeld, out var anyAssignedHunting, out var anyProperWeapon);
		if (!anyAssignedHunting)
		{
			Messages.Message("MessageNoHuntersAvailable".Translate(), pawn, MessageTypeDefOf.CautionInput, historical: false);
		}
		else if (!anyProperWeapon)
		{
			Messages.Message("MessageNoHuntersWithProperWeapon".Translate(), pawn, MessageTypeDefOf.CautionInput, historical: false);
		}
		float manhunterOnDamageChance = pawn.RaceProps.manhunterOnDamageChance;
		float manhunterOnDamageChance2 = PawnUtility.GetManhunterOnDamageChance(pawn);
		if (manhunterOnDamageChance >= 0.015f)
		{
			Messages.Message("MessageAnimalsGoPsychoHunted".Translate(pawn.kindDef.GetLabelPlural().CapitalizeFirst(), manhunterOnDamageChance2.ToStringPercent(), pawn.Named("ANIMAL")).CapitalizeFirst(), pawn, MessageTypeDefOf.CautionInput, historical: false);
		}
		SlaughterDesignatorUtility.CheckWarnAboutVeneratedAnimal(pawn);
	}

	private static void CheckHunters(Map map, out bool anyAssignedHunting, out bool anyProperWeapon)
	{
		anyAssignedHunting = false;
		anyProperWeapon = false;
		foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
		{
			bool flag = item.workSettings.WorkIsActive(WorkTypeDefOf.Hunting);
			if (flag)
			{
				anyAssignedHunting = true;
			}
			if (!item.Downed && flag && WorkGiver_HunterHunt.HasHuntingWeapon(item))
			{
				anyProperWeapon = true;
				break;
			}
		}
	}
}
