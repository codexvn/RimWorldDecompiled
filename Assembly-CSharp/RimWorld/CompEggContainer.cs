using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E8E RID: 11918
	public class CompEggContainer : CompThingContainer
	{
		// Token: 0x17002B21 RID: 11041
		// (get) Token: 0x06010C90 RID: 68752 RVA: 0x004D5523 File Offset: 0x004D3723
		public bool CanEmpty
		{
			get
			{
				return !base.Empty && (this.EggsRuined || base.ContainedThing.stackCount >= base.Props.minCountToEmpty);
			}
		}

		// Token: 0x17002B22 RID: 11042
		// (get) Token: 0x06010C91 RID: 68753 RVA: 0x004D5554 File Offset: 0x004D3754
		private bool EggsRuined
		{
			get
			{
				if (base.Empty)
				{
					return false;
				}
				CompTemperatureRuinable compTemperatureRuinable = base.ContainedThing.TryGetComp<CompTemperatureRuinable>();
				if (compTemperatureRuinable != null && compTemperatureRuinable.Ruined)
				{
					return true;
				}
				CompHatcher compHatcher = base.ContainedThing.TryGetComp<CompHatcher>();
				return compHatcher != null && compHatcher.TemperatureDamaged;
			}
		}

		// Token: 0x06010C92 RID: 68754 RVA: 0x004D55A0 File Offset: 0x004D37A0
		public override bool Accepts(ThingDef thingDef)
		{
			return (base.ContainedThing == null || !this.EggsRuined) && (thingDef.thingCategories != null && (thingDef.thingCategories.Contains(ThingCategoryDefOf.EggsFertilized) || thingDef.thingCategories.Contains(ThingCategoryDefOf.EggsUnfertilized))) && base.Accepts(thingDef);
		}

		// Token: 0x06010C93 RID: 68755 RVA: 0x004D55F4 File Offset: 0x004D37F4
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!base.Empty && !this.EggsRuined)
			{
				CompHatcher compHatcher = base.ContainedThing.TryGetComp<CompHatcher>();
				if (compHatcher != null)
				{
					string text2 = compHatcher.CompInspectStringExtra();
					if (!text2.NullOrEmpty())
					{
						text = text + "\n" + text2;
					}
				}
			}
			return text;
		}

		// Token: 0x06010C94 RID: 68756 RVA: 0x004D5644 File Offset: 0x004D3844
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos && base.Empty)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Fill with eggs",
					action = delegate()
					{
						Thing thing = ThingMaker.MakeThing(ThingDef.Named("EggChickenUnfertilized"), null);
						thing.stackCount = base.Props.stackLimit;
						this.innerContainer.TryAdd(thing, true);
					}
				};
			}
			yield break;
			yield break;
		}
	}
}
