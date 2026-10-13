using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E6D RID: 11885
	public class CompStyleable : ThingComp
	{
		// Token: 0x17002AF3 RID: 10995
		// (get) Token: 0x06010BC2 RID: 68546 RVA: 0x004D2F72 File Offset: 0x004D1172
		public Ideo Ideo
		{
			get
			{
				Precept_ThingStyle precept_ThingStyle = this.SourcePrecept;
				if (precept_ThingStyle == null)
				{
					return null;
				}
				return precept_ThingStyle.ideo;
			}
		}

		// Token: 0x17002AF4 RID: 10996
		// (get) Token: 0x06010BC3 RID: 68547 RVA: 0x004D2F88 File Offset: 0x004D1188
		public StyleCategoryDef StyleCategoryDef
		{
			get
			{
				if (this.cachedStyleCategoryDef == null)
				{
					bool flag = false;
					foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefs)
					{
						for (int i = 0; i < styleCategoryDef.thingDefStyles.Count; i++)
						{
							if (styleCategoryDef.thingDefStyles[i].StyleDef == this.styleDef && styleCategoryDef.thingDefStyles[i].ThingDef == this.parent.def)
							{
								this.cachedStyleCategoryDef = styleCategoryDef;
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				return this.cachedStyleCategoryDef;
			}
		}

		// Token: 0x17002AF5 RID: 10997
		// (get) Token: 0x06010BC4 RID: 68548 RVA: 0x004D3040 File Offset: 0x004D1240
		// (set) Token: 0x06010BC5 RID: 68549 RVA: 0x004D3048 File Offset: 0x004D1248
		public Precept_ThingStyle SourcePrecept
		{
			get
			{
				return this.sourcePrecept;
			}
			set
			{
				if (this.sourcePrecept == value)
				{
					return;
				}
				this.sourcePrecept = value;
				if (this.parent.def.randomStyleChance > 0f)
				{
					return;
				}
				BuildableDef buildableDef = this.parent.def.IsBlueprint ? this.parent.def.entityDefToBuild : this.parent.def;
				Precept_ThingStyle precept_ThingStyle = this.sourcePrecept;
				StyleCategoryPair styleCategoryPair = (precept_ThingStyle != null) ? precept_ThingStyle.ideo.style.StyleForThingDef((ThingDef)buildableDef, this.sourcePrecept) : null;
				this.cachedStyleCategoryDef = ((styleCategoryPair != null) ? styleCategoryPair.category : null);
				if (this.cachedStyleCategoryDef != null)
				{
					this.styleDef = styleCategoryPair.styleDef;
				}
			}
		}

		// Token: 0x06010BC6 RID: 68550 RVA: 0x004D30FD File Offset: 0x004D12FD
		public override string TransformLabel(string label)
		{
			if (this.sourcePrecept != null)
			{
				label = this.sourcePrecept.TransformThingLabel(label);
			}
			return label;
		}

		// Token: 0x06010BC7 RID: 68551 RVA: 0x004D3118 File Offset: 0x004D1318
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.sourcePrecept == null)
			{
				return text;
			}
			string text2 = this.sourcePrecept.InspectStringExtra(this.parent);
			if (text2.NullOrEmpty())
			{
				return text;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + text2;
		}

		// Token: 0x06010BC8 RID: 68552 RVA: 0x004D3170 File Offset: 0x004D1370
		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			StyleCategoryPair styleCategoryPair;
			if (forFaction == null)
			{
				styleCategoryPair = null;
			}
			else
			{
				FactionIdeosTracker ideos = forFaction.ideos;
				if (ideos == null)
				{
					styleCategoryPair = null;
				}
				else
				{
					Ideo primaryIdeo = ideos.PrimaryIdeo;
					if (primaryIdeo == null)
					{
						styleCategoryPair = null;
					}
					else
					{
						IdeoStyleTracker style = primaryIdeo.style;
						styleCategoryPair = ((style != null) ? style.StyleForThingDef(this.parent.def, null) : null);
					}
				}
			}
			StyleCategoryPair styleCategoryPair2 = styleCategoryPair;
			if (styleCategoryPair2 != null)
			{
				this.parent.SetStyleDef(styleCategoryPair2.styleDef);
			}
		}

		// Token: 0x06010BC9 RID: 68553 RVA: 0x004D31CE File Offset: 0x004D13CE
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.StyleCategoryDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_StyleLabel".Translate(), this.StyleCategoryDef.LabelCap, "Stat_Thing_StyleDesc".Translate(), 1108, null, null, false, false);
			}
			if (this.SourcePrecept != null)
			{
				foreach (StatDrawEntry statDrawEntry in this.SourcePrecept.SpecialDisplayStats(this.parent))
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06010BCA RID: 68554 RVA: 0x004D31E0 File Offset: 0x004D13E0
		public override void PostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.sourcePrecept != null && (this.sourcePrecept.ideo == null || !Find.IdeoManager.IdeosListForReading.Contains(this.sourcePrecept.ideo)))
			{
				this.sourcePrecept = null;
			}
			Scribe_References.Look<Precept_ThingStyle>(ref this.sourcePrecept, "sourcePrecept", false);
			Scribe_Defs.Look<ThingStyleDef>(ref this.styleDef, "styleDef");
			Scribe_Values.Look<bool>(ref this.everSeenByPlayer, "everSeenByPlayer", false, false);
		}

		// Token: 0x0400AE6B RID: 44651
		private Precept_ThingStyle sourcePrecept;

		// Token: 0x0400AE6C RID: 44652
		public ThingStyleDef styleDef;

		// Token: 0x0400AE6D RID: 44653
		public bool everSeenByPlayer;

		// Token: 0x0400AE6E RID: 44654
		[Unsaved(false)]
		public StyleCategoryDef cachedStyleCategoryDef;
	}
}
