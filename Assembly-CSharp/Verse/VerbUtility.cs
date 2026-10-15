using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public static class VerbUtility
{
	public struct VerbPropertiesWithSource
	{
		public VerbProperties verbProps;

		public Tool tool;

		public ManeuverDef maneuver;

		public ToolCapacityDef ToolCapacity
		{
			get
			{
				if (maneuver == null)
				{
					return null;
				}
				return maneuver.requiredCapacity;
			}
		}

		public VerbPropertiesWithSource(VerbProperties verbProps)
		{
			this.verbProps = verbProps;
			tool = null;
			maneuver = null;
		}

		public VerbPropertiesWithSource(VerbProperties verbProps, Tool tool, ManeuverDef maneuver)
		{
			this.verbProps = verbProps;
			this.tool = tool;
			this.maneuver = maneuver;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetAllVerbProperties_003Ed__12 : IEnumerable<VerbPropertiesWithSource>, IEnumerable, IEnumerator<VerbPropertiesWithSource>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private VerbPropertiesWithSource _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<VerbProperties> verbProps;

		public List<VerbProperties> _003C_003E3__verbProps;

		private List<Tool> tools;

		public List<Tool> _003C_003E3__tools;

		private int _003Ci_003E5__2;

		private IEnumerator<ManeuverDef> _003C_003E7__wrap2;

		VerbPropertiesWithSource IEnumerator<VerbPropertiesWithSource>.Current
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
		public _003CGetAllVerbProperties_003Ed__12(int _003C_003E1__state)
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
					if (verbProps != null)
					{
						_003Ci_003E5__2 = 0;
						goto IL_0079;
					}
					goto IL_008c;
				case 1:
					_003C_003E1__state = -1;
					_003Ci_003E5__2++;
					goto IL_0079;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_0110;
					}
					IL_013a:
					if (_003Ci_003E5__2 >= tools.Count)
					{
						break;
					}
					_003C_003E7__wrap2 = tools[_003Ci_003E5__2].Maneuvers.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0110;
					IL_0079:
					if (_003Ci_003E5__2 < verbProps.Count)
					{
						_003C_003E2__current = new VerbPropertiesWithSource(verbProps[_003Ci_003E5__2]);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_008c;
					IL_008c:
					if (tools == null)
					{
						break;
					}
					_003Ci_003E5__2 = 0;
					goto IL_013a;
					IL_0110:
					if (_003C_003E7__wrap2.MoveNext())
					{
						ManeuverDef current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = new VerbPropertiesWithSource(current.verb, tools[_003Ci_003E5__2], current);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003Ci_003E5__2++;
					goto IL_013a;
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
		IEnumerator<VerbPropertiesWithSource> IEnumerable<VerbPropertiesWithSource>.GetEnumerator()
		{
			_003CGetAllVerbProperties_003Ed__12 _003CGetAllVerbProperties_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAllVerbProperties_003Ed__ = this;
			}
			else
			{
				_003CGetAllVerbProperties_003Ed__ = new _003CGetAllVerbProperties_003Ed__12(0);
			}
			_003CGetAllVerbProperties_003Ed__.verbProps = _003C_003E3__verbProps;
			_003CGetAllVerbProperties_003Ed__.tools = _003C_003E3__tools;
			return _003CGetAllVerbProperties_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<VerbPropertiesWithSource>)this).GetEnumerator();
		}
	}

	private static readonly List<Thing> cellThingsFiltered = new List<Thing>();

	public static ThingDef GetProjectile(this Verb verb)
	{
		if (!(verb is Verb_LaunchProjectile verb_LaunchProjectile))
		{
			return null;
		}
		return verb_LaunchProjectile.Projectile;
	}

	public static DamageDef GetDamageDef(this Verb verb)
	{
		if (verb.verbProps.LaunchesProjectile)
		{
			return verb.GetProjectile()?.projectile.damageDef;
		}
		return verb.verbProps.meleeDamageDef;
	}

	public static bool IsIncendiary_Ranged(this Verb verb)
	{
		if (verb.verbProps != null && verb.verbProps.ai_BeamIsIncendiary)
		{
			return true;
		}
		return verb.GetProjectile()?.projectile.ai_IsIncendiary ?? false;
	}

	public static bool IsIncendiary_Melee(this Verb verb)
	{
		if (verb.tool != null && !verb.tool.extraMeleeDamages.NullOrEmpty())
		{
			for (int i = 0; i < verb.tool.extraMeleeDamages.Count; i++)
			{
				if (verb.tool.extraMeleeDamages[i].def == DamageDefOf.Flame)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool ProjectileFliesOverhead(this Verb verb)
	{
		return verb.GetProjectile()?.projectile.flyOverhead ?? false;
	}

	public static bool HarmsHealth(this Verb verb)
	{
		return verb.GetDamageDef()?.harmsHealth ?? false;
	}

	public static bool IsEMP(this Verb verb)
	{
		return verb.GetDamageDef() == DamageDefOf.EMP;
	}

	public static bool UsesExplosiveProjectiles(this Verb verb)
	{
		ThingDef projectile = verb.GetProjectile();
		if (projectile != null)
		{
			return projectile.projectile.explosionRadius > 0f;
		}
		return false;
	}

	public static List<Verb> GetConcreteExampleVerbs(Def def, ThingDef stuff = null)
	{
		List<Verb> result = null;
		if (def is ThingDef thingDef)
		{
			Thing concreteExample = thingDef.GetConcreteExample(stuff);
			result = ((concreteExample is Pawn) ? ((Pawn)concreteExample).VerbTracker.AllVerbs : ((!(concreteExample is ThingWithComps)) ? null : ((ThingWithComps)concreteExample).GetComp<CompEquippable>().AllVerbs));
		}
		if (def is HediffDef hediffDef)
		{
			result = hediffDef.ConcreteExample.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
		}
		return result;
	}

	public static float CalculateAdjustedForcedMiss(float forcedMiss, IntVec3 vector)
	{
		float num = vector.LengthHorizontalSquared;
		if (num < 9f)
		{
			return 0f;
		}
		if (num < 25f)
		{
			return forcedMiss * 0.5f;
		}
		if (num < 49f)
		{
			return forcedMiss * 0.8f;
		}
		return forcedMiss;
	}

	public static float InterceptChanceFactorFromDistance(Vector3 origin, IntVec3 c)
	{
		float num = (c.ToVector3Shifted() - origin).MagnitudeHorizontalSquared();
		if (num <= 25f)
		{
			return 0f;
		}
		if (num >= 144f)
		{
			return 1f;
		}
		return Mathf.InverseLerp(25f, 144f, num);
	}

	[IteratorStateMachine(typeof(_003CGetAllVerbProperties_003Ed__12))]
	public static IEnumerable<VerbPropertiesWithSource> GetAllVerbProperties(List<VerbProperties> verbProps, List<Tool> tools)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAllVerbProperties_003Ed__12(-2)
		{
			_003C_003E3__verbProps = verbProps,
			_003C_003E3__tools = tools
		};
	}

	public static bool AllowAdjacentShot(LocalTargetInfo target, Thing caster)
	{
		if (!(caster is Pawn))
		{
			return true;
		}
		if (target.Thing is Pawn pawn && pawn.HostileTo(caster))
		{
			return pawn.Downed;
		}
		return true;
	}

	public static VerbSelectionCategory GetSelectionCategory(this Verb v, Pawn p, float highestWeight)
	{
		float num = InitialVerbWeight(v, p);
		if (num >= highestWeight * 0.95f)
		{
			return VerbSelectionCategory.Best;
		}
		if (num < highestWeight * 0.25f)
		{
			return VerbSelectionCategory.Worst;
		}
		return VerbSelectionCategory.Mid;
	}

	public static float InitialVerbWeight(Verb v, Pawn p)
	{
		return DPS(v, p) * AdditionalSelectionFactor(v);
	}

	public static float DPS(Verb v, Pawn p)
	{
		return v.verbProps.AdjustedMeleeDamageAmount(v, p) * (1f + v.verbProps.AdjustedArmorPenetration(v, p)) * v.verbProps.accuracyTouch / v.verbProps.AdjustedFullCycleTime(v, p);
	}

	private static float AdditionalSelectionFactor(Verb v)
	{
		float num = ((v.tool != null) ? v.tool.chanceFactor : 1f);
		if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty())
		{
			foreach (DamageDefAdditionalHediff additionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
			{
				_ = additionalHediff;
				num += 0.1f;
			}
		}
		return num;
	}

	public static float FinalSelectionWeight(Verb verb, Pawn p, List<Verb> allMeleeVerbs, float highestWeight)
	{
		VerbSelectionCategory selectionCategory = verb.GetSelectionCategory(p, highestWeight);
		if (selectionCategory == VerbSelectionCategory.Worst)
		{
			return 0f;
		}
		int num = 0;
		foreach (Verb allMeleeVerb in allMeleeVerbs)
		{
			if (allMeleeVerb.GetSelectionCategory(p, highestWeight) == selectionCategory)
			{
				num++;
			}
		}
		return 1f / (float)num * ((selectionCategory == VerbSelectionCategory.Mid) ? 0.25f : 0.75f);
	}

	public static List<Thing> ThingsToHit(IntVec3 cell, Map map, Func<Thing, bool> filter)
	{
		cellThingsFiltered.Clear();
		List<Thing> thingList = cell.GetThingList(map);
		for (int i = 0; i < thingList.Count; i++)
		{
			Thing thing = thingList[i];
			if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && filter(thing))
			{
				cellThingsFiltered.Add(thing);
			}
		}
		return cellThingsFiltered;
	}
}
