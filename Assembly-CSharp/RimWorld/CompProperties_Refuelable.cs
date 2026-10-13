using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DDA RID: 11738
	public class CompProperties_Refuelable : CompProperties
	{
		// Token: 0x17002A25 RID: 10789
		// (get) Token: 0x06010820 RID: 67616 RVA: 0x004C298B File Offset: 0x004C0B8B
		public string FuelLabel
		{
			get
			{
				if (this.fuelLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.fuelLabel;
			}
		}

		// Token: 0x17002A26 RID: 10790
		// (get) Token: 0x06010821 RID: 67617 RVA: 0x004C29AB File Offset: 0x004C0BAB
		public string FuelGizmoLabel
		{
			get
			{
				if (this.fuelGizmoLabel.NullOrEmpty())
				{
					return "Fuel".TranslateSimple();
				}
				return this.fuelGizmoLabel;
			}
		}

		// Token: 0x17002A27 RID: 10791
		// (get) Token: 0x06010822 RID: 67618 RVA: 0x004C29CC File Offset: 0x004C0BCC
		public Texture2D FuelIcon
		{
			get
			{
				if (this.fuelIcon == null)
				{
					if (!this.fuelIconPath.NullOrEmpty())
					{
						this.fuelIcon = ContentFinder<Texture2D>.Get(this.fuelIconPath, true);
					}
					else
					{
						ThingDef thingDef;
						if (this.fuelFilter.AnyAllowedDef != null)
						{
							thingDef = this.fuelFilter.AnyAllowedDef;
						}
						else
						{
							thingDef = ThingDefOf.Chemfuel;
						}
						this.fuelIcon = thingDef.uiIcon;
					}
				}
				return this.fuelIcon;
			}
		}

		// Token: 0x17002A28 RID: 10792
		// (get) Token: 0x06010823 RID: 67619 RVA: 0x004C2A3B File Offset: 0x004C0C3B
		public float FuelMultiplierCurrentDifficulty
		{
			get
			{
				if (this.factorByDifficulty)
				{
					Storyteller storyteller = Find.Storyteller;
					if (((storyteller != null) ? storyteller.difficulty : null) != null)
					{
						return this.fuelMultiplier / Find.Storyteller.difficulty.maintenanceCostFactor;
					}
				}
				return this.fuelMultiplier;
			}
		}

		// Token: 0x06010824 RID: 67620 RVA: 0x004C2A78 File Offset: 0x004C0C78
		public CompProperties_Refuelable()
		{
			this.compClass = typeof(CompRefuelable);
		}

		// Token: 0x06010825 RID: 67621 RVA: 0x004C2AE3 File Offset: 0x004C0CE3
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			this.fuelFilter.ResolveReferences();
		}

		// Token: 0x06010826 RID: 67622 RVA: 0x004C2AF7 File Offset: 0x004C0CF7
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.destroyOnNoFuel && this.initialFuelPercent <= 0f)
			{
				yield return "Refuelable component has destroyOnNoFuel, but initialFuelPercent <= 0";
			}
			if ((!this.consumeFuelOnlyWhenUsed || this.fuelConsumptionPerTickInRain > 0f) && parentDef.tickerType != TickerType.Normal)
			{
				yield return string.Format("Refuelable component set to consume fuel per tick, but parent tickertype is {0} instead of {1}", parentDef.tickerType, TickerType.Normal);
			}
			yield break;
			yield break;
		}

		// Token: 0x06010827 RID: 67623 RVA: 0x004C2B0E File Offset: 0x004C0D0E
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (((ThingDef)req.Def).building.IsTurret)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Building, "ShotsBeforeRearm".Translate(), ((int)this.fuelCapacity).ToString(), "ShotsBeforeRearmExplanation".Translate(), 3171, null, null, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400AC25 RID: 44069
		public float fuelConsumptionRate = 1f;

		// Token: 0x0400AC26 RID: 44070
		public float fuelCapacity = 2f;

		// Token: 0x0400AC27 RID: 44071
		public float initialFuelPercent;

		// Token: 0x0400AC28 RID: 44072
		public float autoRefuelPercent = 0.3f;

		// Token: 0x0400AC29 RID: 44073
		public float fuelConsumptionPerTickInRain;

		// Token: 0x0400AC2A RID: 44074
		public ThingFilter fuelFilter;

		// Token: 0x0400AC2B RID: 44075
		public bool destroyOnNoFuel;

		// Token: 0x0400AC2C RID: 44076
		public bool consumeFuelOnlyWhenUsed;

		// Token: 0x0400AC2D RID: 44077
		public bool consumeFuelOnlyWhenPowered;

		// Token: 0x0400AC2E RID: 44078
		public bool showFuelGizmo;

		// Token: 0x0400AC2F RID: 44079
		public bool initialAllowAutoRefuel = true;

		// Token: 0x0400AC30 RID: 44080
		public bool showAllowAutoRefuelToggle;

		// Token: 0x0400AC31 RID: 44081
		public bool allowRefuelIfNotEmpty = true;

		// Token: 0x0400AC32 RID: 44082
		public bool fuelIsMortarBarrel;

		// Token: 0x0400AC33 RID: 44083
		public bool targetFuelLevelConfigurable;

		// Token: 0x0400AC34 RID: 44084
		public float initialConfigurableTargetFuelLevel;

		// Token: 0x0400AC35 RID: 44085
		public bool drawOutOfFuelOverlay = true;

		// Token: 0x0400AC36 RID: 44086
		public float minimumFueledThreshold;

		// Token: 0x0400AC37 RID: 44087
		public bool drawFuelGaugeInMap;

		// Token: 0x0400AC38 RID: 44088
		public bool atomicFueling;

		// Token: 0x0400AC39 RID: 44089
		private float fuelMultiplier = 1f;

		// Token: 0x0400AC3A RID: 44090
		public bool factorByDifficulty;

		// Token: 0x0400AC3B RID: 44091
		[MustTranslate]
		public string fuelLabel;

		// Token: 0x0400AC3C RID: 44092
		[MustTranslate]
		public string fuelGizmoLabel;

		// Token: 0x0400AC3D RID: 44093
		[MustTranslate]
		public string outOfFuelMessage;

		// Token: 0x0400AC3E RID: 44094
		[NoTranslate]
		public string fuelIconPath;

		// Token: 0x0400AC3F RID: 44095
		public bool externalTicking;

		// Token: 0x0400AC40 RID: 44096
		public bool hideGizmosIfNotPlayerFaction;

		// Token: 0x0400AC41 RID: 44097
		public bool functionsInVacuum = true;

		// Token: 0x0400AC42 RID: 44098
		public bool canEjectFuel;

		// Token: 0x0400AC43 RID: 44099
		private Texture2D fuelIcon;
	}
}
