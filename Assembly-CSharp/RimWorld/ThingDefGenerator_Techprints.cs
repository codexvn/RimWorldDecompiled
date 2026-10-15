using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class ThingDefGenerator_Techprints
{
	[CompilerGenerated]
	private sealed class _003CImpliedTechprintDefs_003Ed__1 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private List<ResearchProjectDef>.Enumerator _003C_003E7__wrap1;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CImpliedTechprintDefs_003Ed__1(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<ResearchProjectDef>.Enumerator);
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
					if (!ModLister.RoyaltyInstalled)
					{
						return false;
					}
					_003C_003E7__wrap1 = DefDatabase<ResearchProjectDef>.AllDefsListForReading.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					ResearchProjectDef current = _003C_003E7__wrap1.Current;
					if (current.TechprintCount > 0)
					{
						string defName = "Techprint_" + current.defName;
						ThingDef thingDef = (hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, errorOnFail: false) ?? new ThingDef()) : new ThingDef());
						thingDef.resourceReadoutPriority = ResourceCountPriority.Middle;
						thingDef.drawerType = DrawerType.MapMeshOnly;
						thingDef.category = ThingCategory.Item;
						thingDef.thingClass = typeof(ThingWithComps);
						thingDef.thingCategories = new List<ThingCategoryDef>();
						thingDef.thingCategories.Add(ThingCategoryDefOf.Techprints);
						thingDef.graphicData = new GraphicData();
						thingDef.graphicData.graphicClass = typeof(Graphic_Single);
						thingDef.useHitPoints = true;
						thingDef.selectable = true;
						thingDef.thingSetMakerTags = new List<string>();
						thingDef.thingSetMakerTags.Add("Techprint");
						thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, 100f);
						thingDef.SetStatBaseValue(StatDefOf.Flammability, 1f);
						thingDef.SetStatBaseValue(StatDefOf.MarketValue, current.techprintMarketValue);
						thingDef.SetStatBaseValue(StatDefOf.Mass, 0.03f);
						thingDef.SetStatBaseValue(StatDefOf.SellPriceFactor, 0.1f);
						thingDef.altitudeLayer = AltitudeLayer.Item;
						thingDef.comps.Add(new CompProperties_Forbiddable());
						thingDef.comps.Add(new CompProperties_Techprint
						{
							project = current
						});
						thingDef.tickerType = TickerType.Never;
						thingDef.alwaysHaulable = true;
						thingDef.rotatable = false;
						thingDef.pathCost = 14;
						thingDef.drawGUIOverlay = true;
						thingDef.modContentPack = current.modContentPack;
						thingDef.tradeTags = new List<string>();
						thingDef.tradeTags.Add("Techprint");
						thingDef.category = ThingCategory.Item;
						thingDef.description = "TechprintDesc".Translate(NamedArgumentUtility.Named(current, "PROJECT")) + "\n\n" + current.LabelCap + "\n\n" + current.description;
						thingDef.useHitPoints = true;
						if (thingDef.thingCategories == null)
						{
							thingDef.thingCategories = new List<ThingCategoryDef>();
						}
						thingDef.graphicData.texPath = "Things/Item/Special/TechprintUltratech";
						thingDef.defName = defName;
						thingDef.label = "TechprintLabel".Translate(NamedArgumentUtility.Named(current, "PROJECT"));
						_003C_003E2__current = thingDef;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ResearchProjectDef>.Enumerator);
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CImpliedTechprintDefs_003Ed__1 _003CImpliedTechprintDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedTechprintDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedTechprintDefs_003Ed__ = new _003CImpliedTechprintDefs_003Ed__1(0);
			}
			_003CImpliedTechprintDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedTechprintDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	public const string Tag = "Techprint";

	[IteratorStateMachine(typeof(_003CImpliedTechprintDefs_003Ed__1))]
	public static IEnumerable<ThingDef> ImpliedTechprintDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedTechprintDefs_003Ed__1(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}
}
