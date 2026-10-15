using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
{
	[CompilerGenerated]
	private sealed class _003Cget_RightClickFloatMenuOptions_003Ed__8 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Designator_ZoneAdd_Growing _003C_003E4__this;

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
		public _003Cget_RightClickFloatMenuOptions_003Ed__8(int _003C_003E1__state)
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
				Designator_ZoneAdd_Growing designator_ZoneAdd_Growing = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!designator_ZoneAdd_Growing.ShowRightClickHideOptions)
					{
						return false;
					}
					_003C_003E7__wrap1 = Command_Hide_ZoneGrow.GetHideOptions().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003Cget_RightClickFloatMenuOptions_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_RightClickFloatMenuOptions_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected override string NewZoneLabel => "GrowingZone".Translate();

	protected virtual bool ShowRightClickHideOptions => true;

	public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
	{
		[IteratorStateMachine(typeof(_003Cget_RightClickFloatMenuOptions_003Ed__8))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_RightClickFloatMenuOptions_003Ed__8(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public Designator_ZoneAdd_Growing()
	{
		zoneTypeToPlace = typeof(Zone_Growing);
		defaultLabel = "GrowingZone".Translate();
		defaultDesc = "DesignatorGrowingZoneDesc".Translate();
		icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing");
		tutorTag = "ZoneAdd_Growing";
		hotKey = KeyBindingDefOf.Misc2;
		soundSucceeded = SoundDefOf.Designate_ZoneAdd_Growing;
	}

	public override AcceptanceReport CanDesignateCell(IntVec3 c)
	{
		if (!base.CanDesignateCell(c).Accepted)
		{
			return false;
		}
		float num = (ModsConfig.BiotechActive ? 0.5f : ThingDefOf.Plant_Potato.plant.fertilityMin);
		if (ModsConfig.IdeologyActive && BuildCopyCommandUtility.FindAllowedDesignator(TerrainDefOf.FungalGravel) != null)
		{
			num = Mathf.Min(num, ThingDefOf.Plant_Nutrifungus.plant.fertilityMin);
		}
		if (c.GetFertility(base.Map) < num)
		{
			return false;
		}
		return true;
	}

	protected override Zone MakeNewZone()
	{
		PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		return new Zone_Growing(Find.CurrentMap.zoneManager);
	}
}
