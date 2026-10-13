using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002889 RID: 10377
	public class ScenPart_SetNeedLevel : ScenPart_PawnModifier
	{
		// Token: 0x0600E30F RID: 58127 RVA: 0x00427128 File Offset: 0x00425328
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.need.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<NeedDef>(this.PossibleNeeds(), (NeedDef hd) => hd.LabelCap, (NeedDef n) => delegate()
				{
					this.need = n;
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.levelRange, 0f, 1f, "ConfigurableLevel", ToStringStyle.FloatTwo, 0f, GameFont.Small, null, 0f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x0600E310 RID: 58128 RVA: 0x0042722B File Offset: 0x0042542B
		private IEnumerable<NeedDef> PossibleNeeds()
		{
			return from x in DefDatabase<NeedDef>.AllDefsListForReading
			where x.major
			select x;
		}

		// Token: 0x0600E311 RID: 58129 RVA: 0x00427258 File Offset: 0x00425458
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<FloatRange>(ref this.levelRange, "levelRange", default(FloatRange), false);
		}

		// Token: 0x0600E312 RID: 58130 RVA: 0x00427298 File Offset: 0x00425498
		public override string Summary(Scenario scen)
		{
			return "ScenPart_SetNeed".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.need.label, this.levelRange.min.ToStringPercent(), this.levelRange.max.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x0600E313 RID: 58131 RVA: 0x00427318 File Offset: 0x00425518
		public override void Randomize()
		{
			base.Randomize();
			this.need = this.PossibleNeeds().RandomElement<NeedDef>();
			this.levelRange.max = Rand.Range(0f, 1f);
			this.levelRange.min = this.levelRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x0600E314 RID: 58132 RVA: 0x0042737C File Offset: 0x0042557C
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_SetNeedLevel scenPart_SetNeedLevel = other as ScenPart_SetNeedLevel;
			if (scenPart_SetNeedLevel != null && this.need == scenPart_SetNeedLevel.need)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_SetNeedLevel.chance);
				return true;
			}
			return false;
		}

		// Token: 0x0600E315 RID: 58133 RVA: 0x004273BC File Offset: 0x004255BC
		protected override void ModifyPawnPostGenerate(Pawn p, bool redressed)
		{
			Need need;
			if (p.needs != null && p.needs.TryGetNeed(this.need, out need))
			{
				need.CurLevelPercentage = this.levelRange.RandomInRange;
			}
		}

		// Token: 0x0600E316 RID: 58134 RVA: 0x004273F7 File Offset: 0x004255F7
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.need == null;
		}

		// Token: 0x0600E317 RID: 58135 RVA: 0x0042740C File Offset: 0x0042560C
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.need != null) ? this.need.GetHashCode() : 0) ^ this.levelRange.GetHashCode();
		}

		// Token: 0x04009838 RID: 38968
		private NeedDef need;

		// Token: 0x04009839 RID: 38969
		private FloatRange levelRange;
	}
}
