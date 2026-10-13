using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D78 RID: 3448
	public static class VerbUtility
	{
		// Token: 0x060057EC RID: 22508 RVA: 0x001C53BC File Offset: 0x001C35BC
		public static ThingDef GetProjectile(this Verb verb)
		{
			Verb_LaunchProjectile verb_LaunchProjectile = verb as Verb_LaunchProjectile;
			if (verb_LaunchProjectile == null)
			{
				return null;
			}
			return verb_LaunchProjectile.Projectile;
		}

		// Token: 0x060057ED RID: 22509 RVA: 0x001C53DC File Offset: 0x001C35DC
		public static DamageDef GetDamageDef(this Verb verb)
		{
			if (!verb.verbProps.LaunchesProjectile)
			{
				return verb.verbProps.meleeDamageDef;
			}
			ThingDef projectile = verb.GetProjectile();
			if (projectile != null)
			{
				return projectile.projectile.damageDef;
			}
			return null;
		}

		// Token: 0x060057EE RID: 22510 RVA: 0x001C541C File Offset: 0x001C361C
		public static bool IsIncendiary_Ranged(this Verb verb)
		{
			if (verb.verbProps != null && verb.verbProps.ai_BeamIsIncendiary)
			{
				return true;
			}
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.ai_IsIncendiary;
		}

		// Token: 0x060057EF RID: 22511 RVA: 0x001C5458 File Offset: 0x001C3658
		public static bool IsIncendiary_Melee(this Verb verb)
		{
			if (verb.tool != null && !verb.tool.extraMeleeDamages.NullOrEmpty<ExtraDamage>())
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

		// Token: 0x060057F0 RID: 22512 RVA: 0x001C54BC File Offset: 0x001C36BC
		public static bool ProjectileFliesOverhead(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.flyOverhead;
		}

		// Token: 0x060057F1 RID: 22513 RVA: 0x001C54E0 File Offset: 0x001C36E0
		public static bool HarmsHealth(this Verb verb)
		{
			DamageDef damageDef = verb.GetDamageDef();
			return damageDef != null && damageDef.harmsHealth;
		}

		// Token: 0x060057F2 RID: 22514 RVA: 0x001C54FF File Offset: 0x001C36FF
		public static bool IsEMP(this Verb verb)
		{
			return verb.GetDamageDef() == DamageDefOf.EMP;
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x001C5510 File Offset: 0x001C3710
		public static bool UsesExplosiveProjectiles(this Verb verb)
		{
			ThingDef projectile = verb.GetProjectile();
			return projectile != null && projectile.projectile.explosionRadius > 0f;
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x001C553C File Offset: 0x001C373C
		public static List<Verb> GetConcreteExampleVerbs(Def def, ThingDef stuff = null)
		{
			List<Verb> result = null;
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null)
			{
				Thing concreteExample = thingDef.GetConcreteExample(stuff);
				if (concreteExample is Pawn)
				{
					result = ((Pawn)concreteExample).VerbTracker.AllVerbs;
				}
				else if (concreteExample is ThingWithComps)
				{
					result = ((ThingWithComps)concreteExample).GetComp<CompEquippable>().AllVerbs;
				}
				else
				{
					result = null;
				}
			}
			HediffDef hediffDef = def as HediffDef;
			if (hediffDef != null)
			{
				result = hediffDef.ConcreteExample.TryGetComp<HediffComp_VerbGiver>().VerbTracker.AllVerbs;
			}
			return result;
		}

		// Token: 0x060057F5 RID: 22517 RVA: 0x001C55B8 File Offset: 0x001C37B8
		public static float CalculateAdjustedForcedMiss(float forcedMiss, IntVec3 vector)
		{
			float num = (float)vector.LengthHorizontalSquared;
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

		// Token: 0x060057F6 RID: 22518 RVA: 0x001C5600 File Offset: 0x001C3800
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

		// Token: 0x060057F7 RID: 22519 RVA: 0x001C564C File Offset: 0x001C384C
		public static IEnumerable<VerbUtility.VerbPropertiesWithSource> GetAllVerbProperties(List<VerbProperties> verbProps, List<Tool> tools)
		{
			if (verbProps != null)
			{
				int num;
				for (int i = 0; i < verbProps.Count; i = num + 1)
				{
					yield return new VerbUtility.VerbPropertiesWithSource(verbProps[i]);
					num = i;
				}
			}
			if (tools != null)
			{
				int num;
				for (int i = 0; i < tools.Count; i = num + 1)
				{
					foreach (ManeuverDef maneuverDef in tools[i].Maneuvers)
					{
						yield return new VerbUtility.VerbPropertiesWithSource(maneuverDef.verb, tools[i], maneuverDef);
					}
					IEnumerator<ManeuverDef> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060057F8 RID: 22520 RVA: 0x001C5664 File Offset: 0x001C3864
		public static bool AllowAdjacentShot(LocalTargetInfo target, Thing caster)
		{
			if (!(caster is Pawn))
			{
				return true;
			}
			Pawn pawn = target.Thing as Pawn;
			return pawn == null || !pawn.HostileTo(caster) || pawn.Downed;
		}

		// Token: 0x060057F9 RID: 22521 RVA: 0x001C569C File Offset: 0x001C389C
		public static VerbSelectionCategory GetSelectionCategory(this Verb v, Pawn p, float highestWeight)
		{
			float num = VerbUtility.InitialVerbWeight(v, p);
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

		// Token: 0x060057FA RID: 22522 RVA: 0x001C56CA File Offset: 0x001C38CA
		public static float InitialVerbWeight(Verb v, Pawn p)
		{
			return VerbUtility.DPS(v, p) * VerbUtility.AdditionalSelectionFactor(v);
		}

		// Token: 0x060057FB RID: 22523 RVA: 0x001C56DA File Offset: 0x001C38DA
		public static float DPS(Verb v, Pawn p)
		{
			return v.verbProps.AdjustedMeleeDamageAmount(v, p) * (1f + v.verbProps.AdjustedArmorPenetration(v, p)) * v.verbProps.accuracyTouch / v.verbProps.AdjustedFullCycleTime(v, p);
		}

		// Token: 0x060057FC RID: 22524 RVA: 0x001C5718 File Offset: 0x001C3918
		private static float AdditionalSelectionFactor(Verb v)
		{
			float num = (v.tool != null) ? v.tool.chanceFactor : 1f;
			if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
			{
				foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
				{
					num += 0.1f;
				}
			}
			return num;
		}

		// Token: 0x060057FD RID: 22525 RVA: 0x001C57B8 File Offset: 0x001C39B8
		public static float FinalSelectionWeight(Verb verb, Pawn p, List<Verb> allMeleeVerbs, float highestWeight)
		{
			VerbSelectionCategory selectionCategory = verb.GetSelectionCategory(p, highestWeight);
			if (selectionCategory == VerbSelectionCategory.Worst)
			{
				return 0f;
			}
			int num = 0;
			using (List<Verb>.Enumerator enumerator = allMeleeVerbs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetSelectionCategory(p, highestWeight) == selectionCategory)
					{
						num++;
					}
				}
			}
			return 1f / (float)num * ((selectionCategory == VerbSelectionCategory.Mid) ? 0.25f : 0.75f);
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x001C583C File Offset: 0x001C3A3C
		public static List<Thing> ThingsToHit(IntVec3 cell, Map map, Func<Thing, bool> filter)
		{
			VerbUtility.cellThingsFiltered.Clear();
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if ((thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Pawn || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Plant) && filter(thing))
				{
					VerbUtility.cellThingsFiltered.Add(thing);
				}
			}
			return VerbUtility.cellThingsFiltered;
		}

		// Token: 0x04003FFA RID: 16378
		private static readonly List<Thing> cellThingsFiltered = new List<Thing>();

		// Token: 0x02000D79 RID: 3449
		public struct VerbPropertiesWithSource
		{
			// Token: 0x17000DEF RID: 3567
			// (get) Token: 0x06005800 RID: 22528 RVA: 0x001C58D1 File Offset: 0x001C3AD1
			public ToolCapacityDef ToolCapacity
			{
				get
				{
					if (this.maneuver == null)
					{
						return null;
					}
					return this.maneuver.requiredCapacity;
				}
			}

			// Token: 0x06005801 RID: 22529 RVA: 0x001C58E8 File Offset: 0x001C3AE8
			public VerbPropertiesWithSource(VerbProperties verbProps)
			{
				this.verbProps = verbProps;
				this.tool = null;
				this.maneuver = null;
			}

			// Token: 0x06005802 RID: 22530 RVA: 0x001C58FF File Offset: 0x001C3AFF
			public VerbPropertiesWithSource(VerbProperties verbProps, Tool tool, ManeuverDef maneuver)
			{
				this.verbProps = verbProps;
				this.tool = tool;
				this.maneuver = maneuver;
			}

			// Token: 0x04003FFB RID: 16379
			public VerbProperties verbProps;

			// Token: 0x04003FFC RID: 16380
			public Tool tool;

			// Token: 0x04003FFD RID: 16381
			public ManeuverDef maneuver;
		}
	}
}
