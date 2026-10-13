using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200288C RID: 10380
	public class ScenPart_DisallowBuilding : ScenPart_Rule
	{
		// Token: 0x0600E320 RID: 58144 RVA: 0x00427484 File Offset: 0x00425684
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowBuilding(this.building, false);
		}

		// Token: 0x0600E321 RID: 58145 RVA: 0x0042749C File Offset: 0x0042569C
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "DisallowBuilding", "ScenPart_DisallowBuilding".Translate());
		}

		// Token: 0x0600E322 RID: 58146 RVA: 0x004274B8 File Offset: 0x004256B8
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "DisallowBuilding")
			{
				yield return this.building.LabelCap;
			}
			yield break;
		}

		// Token: 0x0600E323 RID: 58147 RVA: 0x004274CF File Offset: 0x004256CF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.building, "building");
		}

		// Token: 0x0600E324 RID: 58148 RVA: 0x004274E7 File Offset: 0x004256E7
		public override void Randomize()
		{
			this.building = this.RandomizableBuildingDefs().RandomElement<ThingDef>();
		}

		// Token: 0x0600E325 RID: 58149 RVA: 0x004274FC File Offset: 0x004256FC
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.building.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localTd2 in from t in this.PossibleBuildingDefs()
				orderby t.label
				select t)
				{
					ThingDef localTd = localTd2;
					list.Add(new FloatMenuOption(localTd.LabelCap, delegate()
					{
						this.building = localTd;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600E326 RID: 58150 RVA: 0x004275FC File Offset: 0x004257FC
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_DisallowBuilding scenPart_DisallowBuilding = other as ScenPart_DisallowBuilding;
			return scenPart_DisallowBuilding != null && scenPart_DisallowBuilding.building == this.building;
		}

		// Token: 0x0600E327 RID: 58151 RVA: 0x00427624 File Offset: 0x00425824
		protected virtual IEnumerable<ThingDef> PossibleBuildingDefs()
		{
			return from d in DefDatabase<ThingDef>.AllDefs
			where d.category == ThingCategory.Building && d.BuildableByPlayer
			select d;
		}

		// Token: 0x0600E328 RID: 58152 RVA: 0x0042764F File Offset: 0x0042584F
		private IEnumerable<ThingDef> RandomizableBuildingDefs()
		{
			yield return ThingDefOf.Wall;
			yield return ThingDefOf.Turret_MiniTurret;
			yield return ThingDefOf.OrbitalTradeBeacon;
			yield return ThingDefOf.Battery;
			yield return ThingDefOf.TrapSpike;
			yield return ThingDefOf.Cooler;
			yield return ThingDefOf.Heater;
			yield break;
		}

		// Token: 0x0600E329 RID: 58153 RVA: 0x00427658 File Offset: 0x00425858
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.building == null;
		}

		// Token: 0x0600E32A RID: 58154 RVA: 0x0042766D File Offset: 0x0042586D
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.building != null) ? this.building.GetHashCode() : 0);
		}

		// Token: 0x0400983F RID: 38975
		private ThingDef building;

		// Token: 0x04009840 RID: 38976
		private const string DisallowBuildingTag = "DisallowBuilding";
	}
}
