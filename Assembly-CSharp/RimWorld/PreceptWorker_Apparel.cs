using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class PreceptWorker_Apparel : PreceptWorker
{
	[CompilerGenerated]
	private sealed class _003Cget_ThingDefs_003Ed__5 : IEnumerable<PreceptThingChance>, IEnumerable, IEnumerator<PreceptThingChance>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PreceptThingChance _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PreceptWorker_Apparel _003C_003E4__this;

		private IEnumerator<ThingDef> _003C_003E7__wrap1;

		PreceptThingChance IEnumerator<PreceptThingChance>.Current
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
		public _003Cget_ThingDefs_003Ed__5(int _003C_003E1__state)
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
				PreceptWorker_Apparel CS_0024_003C_003E8__locals1 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => CS_0024_003C_003E8__locals1.IsValidApparel(x)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap1.Current;
					PreceptThingChance preceptThingChance = default(PreceptThingChance);
					preceptThingChance.def = current;
					preceptThingChance.chance = 1f;
					_003C_003E2__current = preceptThingChance;
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
		IEnumerator<PreceptThingChance> IEnumerable<PreceptThingChance>.GetEnumerator()
		{
			_003Cget_ThingDefs_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_ThingDefs_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PreceptThingChance>)this).GetEnumerator();
		}
	}

	public override IEnumerable<PreceptThingChance> ThingDefs
	{
		[IteratorStateMachine(typeof(_003Cget_ThingDefs_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ThingDefs_003Ed__5(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override bool ShouldSkipThing(Ideo ideo, ThingDef thingDef)
	{
		foreach (Precept item in ideo.PreceptsListForReading)
		{
			if (item is Precept_Apparel precept_Apparel && precept_Apparel.apparelDef == thingDef)
			{
				return true;
			}
		}
		return false;
	}

	public override float GetThingOrder(PreceptThingChance thingChance)
	{
		if (thingChance.def.IsApparel)
		{
			return (thingChance.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || thingChance.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover) ? 1 : 2;
		}
		return 0f;
	}

	private bool IsValidApparel(ThingDef td)
	{
		if (!td.IsApparel || !td.MadeFromStuff)
		{
			return false;
		}
		if (!td.apparel.canBeDesiredForIdeo)
		{
			return false;
		}
		if (td.thingCategories != null && (td.thingCategories.Contains(ThingCategoryDefOf.ArmorHeadgear) || td.thingCategories.Contains(ThingCategoryDefOf.ApparelArmor)))
		{
			return false;
		}
		for (int i = 0; i < def.comps.Count; i++)
		{
			if (def.comps[i] is PreceptComp_Apparel preceptComp_Apparel && !preceptComp_Apparel.CanApplyToApparel(td))
			{
				return false;
			}
		}
		return true;
	}

	public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
	{
		if (IsValidApparel(def))
		{
			if (generatingFor != null)
			{
				AcceptanceReport result = CheckFaction(generatingFor);
				if (!result.Accepted)
				{
					return result;
				}
			}
			if (Find.World != null && Find.FactionManager != null)
			{
				foreach (Faction allFaction in Find.FactionManager.AllFactions)
				{
					if (allFaction.def != generatingFor && allFaction.ideos != null && allFaction.ideos.PrimaryIdeo == ideo)
					{
						AcceptanceReport acceptanceReport = CheckFaction(allFaction.def);
						if (!acceptanceReport)
						{
							return acceptanceReport;
						}
					}
				}
			}
			return true;
		}
		return false;
		AcceptanceReport CheckFaction(FactionDef faction)
		{
			if (def.apparel.ideoDesireAllowedFactionCategoryTags != null && !def.apparel.ideoDesireAllowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle(def.apparel.ideoDesireAllowedFactionCategoryTags.Select((string t) => ("RoleApparelRequirementIncompatibleFaction_Allowed_" + t).Translate().Resolve()).ToCommaListOr())));
			}
			if (def.apparel.ideoDesireDisallowedFactionCategoryTags != null && def.apparel.ideoDesireDisallowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle(def.apparel.ideoDesireDisallowedFactionCategoryTags.Select((string t) => ("RoleApparelRequirementIncompatibleFaction_Disallowed_" + t).Translate().Resolve()).ToCommaListOr())));
			}
			return true;
		}
	}
}
