using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002877 RID: 10359
	public class ScenPart_ForcedHediff : ScenPart_PawnModifier
	{
		// Token: 0x1700228C RID: 8844
		// (get) Token: 0x0600E2B8 RID: 58040 RVA: 0x00425F3B File Offset: 0x0042413B
		private float MaxSeverity
		{
			get
			{
				if (this.hediff.lethalSeverity <= 0f)
				{
					return 1f;
				}
				return this.hediff.lethalSeverity * 0.99f;
			}
		}

		// Token: 0x0600E2B9 RID: 58041 RVA: 0x00425F68 File Offset: 0x00424168
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f + 31f);
			if (Widgets.ButtonText(scenPartRect.TopPartPixels(ScenPart.RowHeight), this.hediff.LabelCap, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<HediffDef>(this.PossibleHediffs(), (HediffDef hd) => hd.LabelCap, (HediffDef hd) => delegate()
				{
					this.hediff = hd;
					if (this.severityRange.max > this.MaxSeverity)
					{
						this.severityRange.max = this.MaxSeverity;
					}
					if (this.severityRange.min > this.MaxSeverity)
					{
						this.severityRange.min = this.MaxSeverity;
					}
				});
			}
			Widgets.FloatRange(new Rect(scenPartRect.x, scenPartRect.y + ScenPart.RowHeight, scenPartRect.width, 31f), listing.CurHeight.GetHashCode(), ref this.severityRange, 0f, this.MaxSeverity, "ConfigurableSeverity", ToStringStyle.FloatTwo, 0f, GameFont.Small, null, 0f);
			base.DoPawnModifierEditInterface(scenPartRect.BottomPartPixels(ScenPart.RowHeight * 2f));
		}

		// Token: 0x0600E2BA RID: 58042 RVA: 0x0042606C File Offset: 0x0042426C
		private IEnumerable<HediffDef> PossibleHediffs()
		{
			return from x in DefDatabase<HediffDef>.AllDefsListForReading
			where x.scenarioCanAdd
			select x;
		}

		// Token: 0x0600E2BB RID: 58043 RVA: 0x00426098 File Offset: 0x00424298
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.hediff, "hediff");
			Scribe_Values.Look<FloatRange>(ref this.severityRange, "severityRange", default(FloatRange), false);
		}

		// Token: 0x0600E2BC RID: 58044 RVA: 0x004260D8 File Offset: 0x004242D8
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveHediff".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.hediff.label).CapitalizeFirst();
		}

		// Token: 0x0600E2BD RID: 58045 RVA: 0x0042612C File Offset: 0x0042432C
		public override void Randomize()
		{
			base.Randomize();
			this.hediff = this.PossibleHediffs().RandomElement<HediffDef>();
			this.severityRange.max = Rand.Range(this.MaxSeverity * 0.2f, this.MaxSeverity * 0.95f);
			this.severityRange.min = this.severityRange.max * Rand.Range(0f, 0.95f);
		}

		// Token: 0x0600E2BE RID: 58046 RVA: 0x004261A0 File Offset: 0x004243A0
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_ForcedHediff scenPart_ForcedHediff = other as ScenPart_ForcedHediff;
			if (scenPart_ForcedHediff != null && this.hediff == scenPart_ForcedHediff.hediff)
			{
				this.chance = GenMath.ChanceEitherHappens(this.chance, scenPart_ForcedHediff.chance);
				return true;
			}
			return false;
		}

		// Token: 0x0600E2BF RID: 58047 RVA: 0x004261E0 File Offset: 0x004243E0
		public override bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			if (!base.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
			{
				return false;
			}
			if (this.hideOffMap)
			{
				if (!req.AllowDead && !req.ForceDead && pawn.health.WouldDieAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
				if (!req.AllowDowned && !req.ForceDead && pawn.health.WouldBeDownedAfterAddingHediff(this.hediff, null, this.severityRange.max))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600E2C0 RID: 58048 RVA: 0x00426269 File Offset: 0x00424469
		protected override void ModifyNewPawn(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x0600E2C1 RID: 58049 RVA: 0x00426269 File Offset: 0x00424469
		protected override void ModifyHideOffMapStartingPawnPostMapGenerate(Pawn p)
		{
			this.AddHediff(p);
		}

		// Token: 0x0600E2C2 RID: 58050 RVA: 0x00426274 File Offset: 0x00424474
		private void AddHediff(Pawn p)
		{
			if (Find.GameInitData != null && Find.GameInitData.QuickStarted && Prefs.DisableQuickStartCryptoSickness && this.hediff == HediffDefOf.CryptosleepSickness)
			{
				return;
			}
			Hediff hediff = HediffMaker.MakeHediff(this.hediff, p, null);
			hediff.Severity = this.severityRange.RandomInRange;
			p.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x0600E2C3 RID: 58051 RVA: 0x004262DF File Offset: 0x004244DF
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.hediff == null;
		}

		// Token: 0x0600E2C4 RID: 58052 RVA: 0x004262F4 File Offset: 0x004244F4
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.hediff != null) ? this.hediff.GetHashCode() : 0) ^ this.severityRange.GetHashCode();
		}

		// Token: 0x04009813 RID: 38931
		private HediffDef hediff;

		// Token: 0x04009814 RID: 38932
		private FloatRange severityRange;
	}
}
