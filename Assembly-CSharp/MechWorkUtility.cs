using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse;

public static class MechWorkUtility
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__0 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__0(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				if (!parentDef.race.IsMechanoid || parentDef.race.mechEnabledWorkTypes.NullOrEmpty())
				{
					return false;
				}
				TaggedString taggedString = "MechWorkActivitiesExplanation".Translate() + ":\n";
				foreach (WorkTypeDef item in parentDef.race.mechEnabledWorkTypes.OrderBy((WorkTypeDef wt) => wt.label))
				{
					IEnumerable<WorkGiverDef> source = item.workGiversByPriority.Where((WorkGiverDef wg) => wg.canBeDoneByMechs);
					if (!source.Any())
					{
						continue;
					}
					taggedString += "\n - " + item.gerundLabel.CapitalizeFirst();
					foreach (WorkGiverDef item2 in source.OrderBy((WorkGiverDef wg) => wg.label))
					{
						taggedString += "\n  - " + item2.LabelCap;
					}
				}
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnWork, "MechWorkActivities".Translate(), parentDef.race.mechEnabledWorkTypes.Select((WorkTypeDef w) => w.gerundLabel).ToCommaList(useAnd: true).CapitalizeFirst(), taggedString, 502);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnWork, "MechWorkSkill".Translate(), parentDef.race.mechFixedSkillLevel.ToString(), "MechWorkSkillDesc".Translate(), 501);
				_003C_003E1__state = 2;
				return true;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__0 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__0(0);
			}
			_003CSpecialDisplayStats_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__0))]
	public static IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__0(-2)
		{
			_003C_003E3__parentDef = parentDef
		};
	}

	public static bool AnyWorkMechCouldDo(RecipeDef recipe)
	{
		IEnumerable<WorkTypeDef> mechEnabledWorkTypes = DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef p) => p.RaceProps.IsWorkMech).SelectMany((PawnKindDef p) => p.RaceProps.mechEnabledWorkTypes).Distinct();
		if (recipe.requiredGiverWorkType != null && !mechEnabledWorkTypes.Contains(recipe.requiredGiverWorkType))
		{
			return false;
		}
		IEnumerable<ThingDef> recipeUsers = recipe.AllRecipeUsers;
		return DefDatabase<WorkGiverDef>.AllDefs.Where((WorkGiverDef wg) => !wg.fixedBillGiverDefs.NullOrEmpty() && wg.fixedBillGiverDefs.Intersect(recipeUsers).Any()).Any((WorkGiverDef wg) => mechEnabledWorkTypes.Contains(wg.workType));
	}
}
