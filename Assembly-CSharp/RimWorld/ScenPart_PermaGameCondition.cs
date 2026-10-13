using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002873 RID: 10355
	public class ScenPart_PermaGameCondition : ScenPart
	{
		// Token: 0x17002289 RID: 8841
		// (get) Token: 0x0600E29D RID: 58013 RVA: 0x00425C24 File Offset: 0x00423E24
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x0600E29E RID: 58014 RVA: 0x00425C68 File Offset: 0x00423E68
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.gameCondition.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<GameConditionDef>(this.AllowedGameConditions(), (GameConditionDef d) => d.LabelCap, (GameConditionDef d) => delegate()
				{
					this.gameCondition = d;
				});
			}
		}

		// Token: 0x0600E29F RID: 58015 RVA: 0x00425CDA File Offset: 0x00423EDA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x0600E2A0 RID: 58016 RVA: 0x00425CF2 File Offset: 0x00423EF2
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x0600E2A1 RID: 58017 RVA: 0x00425D05 File Offset: 0x00423F05
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x0600E2A2 RID: 58018 RVA: 0x00425D30 File Offset: 0x00423F30
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x0600E2A3 RID: 58019 RVA: 0x00425D4C File Offset: 0x00423F4C
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x0600E2A4 RID: 58020 RVA: 0x00425D64 File Offset: 0x00423F64
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x0600E2A5 RID: 58021 RVA: 0x00425D8C File Offset: 0x00423F8C
		public override bool CanCoexistWith(ScenPart other)
		{
			if (this.gameCondition == null)
			{
				return true;
			}
			ScenPart_PermaGameCondition scenPart_PermaGameCondition = other as ScenPart_PermaGameCondition;
			return scenPart_PermaGameCondition == null || this.gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition);
		}

		// Token: 0x0600E2A6 RID: 58022 RVA: 0x00425DC3 File Offset: 0x00423FC3
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.gameCondition == null;
		}

		// Token: 0x0600E2A7 RID: 58023 RVA: 0x00425DD8 File Offset: 0x00423FD8
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.gameCondition != null) ? this.gameCondition.GetHashCode() : 0);
		}

		// Token: 0x04009806 RID: 38918
		private GameConditionDef gameCondition;

		// Token: 0x04009807 RID: 38919
		public const string PermaGameConditionTag = "PermaGameCondition";
	}
}
