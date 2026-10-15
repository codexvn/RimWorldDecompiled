using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompProperties_Facility : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__14 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_Facility _003C_003E4__this;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		private List<StatModifier>.Enumerator _003C_003E7__wrap2;

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
		public _003CSpecialDisplayStats_003Ed__14(int _003C_003E1__state)
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
			case 3:
			case 4:
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
			_003C_003E7__wrap2 = default(List<StatModifier>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompProperties_Facility compProperties_Facility = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_Facility).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
					_003C_003E1__state = -4;
					break;
				case 3:
					_003C_003E1__state = -4;
					break;
				case 4:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0080:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_Facility.statOffsets == null)
					{
						return false;
					}
					_003C_003E7__wrap2 = compProperties_Facility.statOffsets.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					StatModifier current2 = _003C_003E7__wrap2.Current;
					if (ModsConfig.AnomalyActive && current2.stat == StatDefOf.ContainmentStrength)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Containment, "StatsReport_ContainmentStrengthOffset".Translate(), current2.value.ToString("F1"), "StatsReport_ContainmentStrengthOffset_Desc".Translate(), 500);
						_003C_003E1__state = 2;
						return true;
					}
					if (ModsConfig.OdysseyActive)
					{
						if (current2.stat == StatDefOf.GravshipRange)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_GravshipRangeOffset".Translate(), current2.value.ToString("F0"), "StatsReport_GravshipRangeOffset_Desc".Translate(), 500);
							_003C_003E1__state = 3;
							return true;
						}
						if (current2.stat == StatDefOf.SubstructureSupport)
						{
							StatDef substructureSupport = StatDefOf.SubstructureSupport;
							_003C_003E2__current = new StatDrawEntry(substructureSupport.category, substructureSupport.LabelCap, current2.value.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset), substructureSupport.description, substructureSupport.displayPriorityInCategory);
							_003C_003E1__state = 4;
							return true;
						}
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<StatModifier>.Enumerator);
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__14 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.req = _003C_003E3__req;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[Unsaved(false)]
	public List<ThingDef> linkableBuildings;

	public List<StatModifier> statOffsets;

	public int maxSimultaneous = 1;

	public bool mustBePlacedAdjacent;

	public bool mustBePlacedAdjacentCardinalToBedHead;

	public bool mustBePlacedAdjacentCardinalToAndFacingBedHead;

	public bool mustBePlacedFacingThingLinear;

	public bool canLinkToMedBedsOnly;

	public float maxDistance = 8f;

	public float minDistance;

	public bool showMaxSimultaneous = true;

	public bool requiresLOS = true;

	public CompProperties_Facility()
	{
		compClass = typeof(CompFacility);
	}

	public override void ResolveReferences(ThingDef parentDef)
	{
		linkableBuildings = new List<ThingDef>();
		List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
		for (int i = 0; i < allDefsListForReading.Count; i++)
		{
			CompProperties_AffectedByFacilities compProperties = allDefsListForReading[i].GetCompProperties<CompProperties_AffectedByFacilities>();
			if (compProperties == null || compProperties.linkableFacilities == null)
			{
				continue;
			}
			for (int j = 0; j < compProperties.linkableFacilities.Count; j++)
			{
				if (compProperties.linkableFacilities[j] == parentDef)
				{
					linkableBuildings.Add(allDefsListForReading[i]);
					break;
				}
			}
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__14))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__14(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}
}
