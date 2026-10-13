using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003049 RID: 12361
	public class Alert_NeedMechChargers : Alert
	{
		// Token: 0x060115E5 RID: 71141 RVA: 0x004FBA64 File Offset: 0x004F9C64
		public Alert_NeedMechChargers()
		{
			this.defaultLabel = "AlertNeedMechChargers".Translate();
			this.requireBiotech = true;
		}

		// Token: 0x060115E6 RID: 71142 RVA: 0x004FBABF File Offset: 0x004F9CBF
		public override AlertReport GetReport()
		{
			this.RecacheMechsAndChargers();
			return this.requiredChargerDefs.Count > 0;
		}

		// Token: 0x060115E7 RID: 71143 RVA: 0x004FBADC File Offset: 0x004F9CDC
		private void RecacheMechsAndChargers()
		{
			this.chargers.Clear();
			this.mechs.Clear();
			List<Map> maps = Find.Maps;
			if (this.allChargerDefs == null)
			{
				this.allChargerDefs = new List<ThingDef>();
				this.allChargerDefs.AddRange(from t in DefDatabase<ThingDef>.AllDefs
				where ThingRequestGroup.MechCharger.Includes(t)
				select t);
			}
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.MechCharger);
				for (int j = 0; j < list.Count; j++)
				{
					Building_MechCharger building_MechCharger = (Building_MechCharger)list[j];
					if (building_MechCharger.Faction == Faction.OfPlayer)
					{
						this.chargers.Add(building_MechCharger);
					}
				}
			}
			for (int k = 0; k < maps.Count; k++)
			{
				List<Pawn> list2 = maps[k].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int l = 0; l < list2.Count; l++)
				{
					if (list2[l].IsColonyMech)
					{
						this.mechs.Add(list2[l]);
					}
				}
			}
			this.requiredChargerDefs.Clear();
			for (int m = 0; m < this.mechs.Count; m++)
			{
				Pawn mech = this.mechs[m];
				if (!this.chargers.Any((Building_MechCharger c) => c.IsCompatibleWithCharger(mech.kindDef)))
				{
					ThingDef thingDef = this.allChargerDefs.FirstOrDefault((ThingDef c) => Building_MechCharger.IsCompatibleWithCharger(c, mech.kindDef));
					if (thingDef != null && !this.requiredChargerDefs.Contains(thingDef))
					{
						this.requiredChargerDefs.Add(thingDef);
					}
				}
			}
		}

		// Token: 0x060115E8 RID: 71144 RVA: 0x004FBCAC File Offset: 0x004F9EAC
		public override TaggedString GetExplanation()
		{
			TaggedString taggedString = "AlertNeedMechChargersDesc".Translate((from c in this.requiredChargerDefs
			select c.LabelCap.Resolve()).ToLineList("  - ", false));
			if (!ResearchProjectDefOf.BasicMechtech.IsFinished)
			{
				taggedString += "\n\n" + "AlertNeedMechChargerBasicMechtech".Translate();
			}
			return taggedString;
		}

		// Token: 0x0400B3D6 RID: 46038
		private List<Pawn> mechs = new List<Pawn>();

		// Token: 0x0400B3D7 RID: 46039
		private List<Building_MechCharger> chargers = new List<Building_MechCharger>();

		// Token: 0x0400B3D8 RID: 46040
		private List<ThingDef> allChargerDefs = new List<ThingDef>();

		// Token: 0x0400B3D9 RID: 46041
		private List<ThingDef> requiredChargerDefs = new List<ThingDef>();
	}
}
