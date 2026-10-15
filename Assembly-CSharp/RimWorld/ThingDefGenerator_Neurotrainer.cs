using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ThingDefGenerator_Neurotrainer
{
	[CompilerGenerated]
	private sealed class _003CImpliedThingDefs_003Ed__3 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private IEnumerator<AbilityDef> _003C_003E7__wrap1;

		private IEnumerator<SkillDef> _003C_003E7__wrap2;

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
		public _003CImpliedThingDefs_003Ed__3(int _003C_003E1__state)
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
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
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
					_003C_003E7__wrap1 = DefDatabase<AbilityDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01d3;
				case 1:
					_003C_003E1__state = -3;
					goto IL_01d3;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_01d3:
					while (_003C_003E7__wrap1.MoveNext())
					{
						AbilityDef current = _003C_003E7__wrap1.Current;
						if (current.IsPsycast)
						{
							ThingDef thingDef = BaseNeurotrainer(PsytrainerDefPrefix + "_" + current.defName, hotReload);
							thingDef.label = "PsycastNeurotrainerLabel".Translate(current.label);
							thingDef.description = "PsycastNeurotrainerDescription".Translate(current.Named("PSYCAST"), current.description.Named("PSYCASTDESCRIPTION"));
							thingDef.comps.Add(new CompProperties_Usable
							{
								useJob = JobDefOf.UseNeurotrainer,
								useLabel = "PsycastNeurotrainerUseLabel".Translate(current.label),
								showUseGizmo = true
							});
							thingDef.comps.Add(new CompProperties_UseEffect_GainAbility
							{
								ability = current
							});
							thingDef.statBases.Add(new StatModifier
							{
								stat = StatDefOf.MarketValue,
								value = Mathf.Round(Mathf.Lerp(100f, 1000f, (float)current.level / 6f))
							});
							thingDef.thingCategories = new List<ThingCategoryDef> { ThingCategoryDefOf.NeurotrainersPsycast };
							thingDef.thingSetMakerTags = new List<string> { "RewardStandardLowFreq" };
							thingDef.modContentPack = current.modContentPack;
							thingDef.descriptionHyperlinks = new List<DefHyperlink>
							{
								new DefHyperlink(current)
							};
							_003C_003E2__current = thingDef;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = DefDatabase<SkillDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					SkillDef current2 = _003C_003E7__wrap2.Current;
					ThingDef thingDef2 = BaseNeurotrainer(NeurotrainerDefPrefix + "_" + current2.defName, hotReload);
					thingDef2.label = "SkillNeurotrainerLabel".Translate(current2.label);
					thingDef2.description = "SkillNeurotrainerDescription".Translate();
					thingDef2.comps.Add(new CompProperties_Usable
					{
						useJob = JobDefOf.UseNeurotrainer,
						useLabel = "SkillNeurotrainerUseLabel".Translate(current2.label),
						showUseGizmo = true
					});
					thingDef2.comps.Add(new CompProperties_UseEffect_LearnSkill
					{
						skill = current2
					});
					thingDef2.statBases.Add(new StatModifier
					{
						stat = StatDefOf.MarketValue,
						value = 750f
					});
					thingDef2.thingCategories = new List<ThingCategoryDef> { ThingCategoryDefOf.NeurotrainersSkill };
					thingDef2.thingSetMakerTags = new List<string> { "RewardStandardMidFreq", "SkillNeurotrainer" };
					thingDef2.modContentPack = current2.modContentPack;
					_003C_003E2__current = thingDef2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CImpliedThingDefs_003Ed__3 _003CImpliedThingDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedThingDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedThingDefs_003Ed__ = new _003CImpliedThingDefs_003Ed__3(0);
			}
			_003CImpliedThingDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedThingDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	public static string NeurotrainerDefPrefix = "Neurotrainer";

	public static string PsytrainerDefPrefix = "Psytrainer";

	private const int MaxAbilityLevel = 6;

	[IteratorStateMachine(typeof(_003CImpliedThingDefs_003Ed__3))]
	public static IEnumerable<ThingDef> ImpliedThingDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedThingDefs_003Ed__3(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	private static ThingDef BaseNeurotrainer(string defName, bool hotReload = false)
	{
		ThingDef obj = (hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, errorOnFail: false) ?? new ThingDef()) : new ThingDef());
		obj.defName = defName;
		obj.category = ThingCategory.Item;
		obj.selectable = true;
		obj.thingClass = typeof(ThingWithComps);
		obj.comps = new List<CompProperties>
		{
			new CompProperties_UseEffectPlaySound
			{
				soundOnUsed = SoundDefOf.MechSerumUsed
			},
			new CompProperties_UseEffectDestroySelf
			{
				compClass = typeof(CompUseEffect_DestroySelf)
			},
			new CompProperties_Forbiddable()
		};
		obj.graphicData = new GraphicData
		{
			texPath = "Things/Item/Special/MechSerumNeurotrainer",
			graphicClass = typeof(Graphic_Single)
		};
		obj.drawGUIOverlay = false;
		obj.statBases = new List<StatModifier>
		{
			new StatModifier
			{
				stat = StatDefOf.MaxHitPoints,
				value = 80f
			},
			new StatModifier
			{
				stat = StatDefOf.Mass,
				value = 0.2f
			},
			new StatModifier
			{
				stat = StatDefOf.DeteriorationRate,
				value = 2f
			},
			new StatModifier
			{
				stat = StatDefOf.Flammability,
				value = 0.2f
			}
		};
		obj.techLevel = TechLevel.Ultra;
		obj.altitudeLayer = AltitudeLayer.Item;
		obj.alwaysHaulable = true;
		obj.rotatable = false;
		obj.pathCost = 14;
		obj.tradeTags = new List<string> { "ExoticMisc" };
		obj.stackLimit = 1;
		obj.tradeNeverStack = true;
		obj.forceDebugSpawnable = true;
		obj.drawerType = DrawerType.MapMeshOnly;
		return obj;
	}
}
