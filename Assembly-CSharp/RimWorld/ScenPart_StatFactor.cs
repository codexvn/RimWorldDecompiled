using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028EF RID: 10479
	public class ScenPart_StatFactor : ScenPart
	{
		// Token: 0x0600E52A RID: 58666 RVA: 0x0042E4F1 File Offset: 0x0042C6F1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<StatDef>(ref this.stat, "stat");
			Scribe_Values.Look<float>(ref this.factor, "factor", 0f, false);
		}

		// Token: 0x0600E52B RID: 58667 RVA: 0x0042E520 File Offset: 0x0042C720
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			if (Widgets.ButtonText(scenPartRect.TopHalf(), this.stat.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (StatDef statDef in DefDatabase<StatDef>.AllDefs)
				{
					if (!statDef.forInformationOnly && statDef.CanShowWithLoadedMods())
					{
						StatDef localSd = statDef;
						list.Add(new FloatMenuOption(localSd.LabelForFullStatListCap, delegate()
						{
							this.stat = localSd;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Rect rect = scenPartRect.BottomHalf();
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "multiplier".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect3, ref this.factor, ref this.factorBuf, 0f, 100f);
		}

		// Token: 0x0600E52C RID: 58668 RVA: 0x0042E66C File Offset: 0x0042C86C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StatFactor".Translate(this.stat.label, this.factor.ToStringPercent());
		}

		// Token: 0x0600E52D RID: 58669 RVA: 0x0042E6A0 File Offset: 0x0042C8A0
		public override void Randomize()
		{
			this.stat = (from d in DefDatabase<StatDef>.AllDefs
			where d.scenarioRandomizable
			select d).RandomElement<StatDef>();
			this.factor = GenMath.RoundedHundredth(Rand.Range(0.1f, 3f));
		}

		// Token: 0x0600E52E RID: 58670 RVA: 0x0042E6FC File Offset: 0x0042C8FC
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StatFactor scenPart_StatFactor = other as ScenPart_StatFactor;
			if (scenPart_StatFactor != null && scenPart_StatFactor.stat == this.stat)
			{
				this.factor *= scenPart_StatFactor.factor;
				return true;
			}
			return false;
		}

		// Token: 0x0600E52F RID: 58671 RVA: 0x0042E737 File Offset: 0x0042C937
		public float GetStatFactor(StatDef stat)
		{
			if (stat == this.stat)
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x0600E530 RID: 58672 RVA: 0x0042E74E File Offset: 0x0042C94E
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.stat == null;
		}

		// Token: 0x0600E531 RID: 58673 RVA: 0x0042E763 File Offset: 0x0042C963
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.stat != null) ? this.stat.GetHashCode() : 0) ^ this.factor.GetHashCode();
		}

		// Token: 0x04009959 RID: 39257
		private StatDef stat;

		// Token: 0x0400995A RID: 39258
		private float factor;

		// Token: 0x0400995B RID: 39259
		private string factorBuf;
	}
}
