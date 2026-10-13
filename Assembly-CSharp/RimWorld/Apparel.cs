using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AF8 RID: 11000
	public class Apparel : ThingWithComps
	{
		// Token: 0x1700265F RID: 9823
		// (get) Token: 0x0600F573 RID: 62835 RVA: 0x00475054 File Offset: 0x00473254
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				if (pawn_ApparelTracker == null)
				{
					return null;
				}
				return pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x17002660 RID: 9824
		// (get) Token: 0x0600F574 RID: 62836 RVA: 0x00475078 File Offset: 0x00473278
		// (set) Token: 0x0600F575 RID: 62837 RVA: 0x00475080 File Offset: 0x00473280
		public bool WornByCorpse
		{
			get
			{
				return this.wornByCorpseInt;
			}
			set
			{
				this.wornByCorpseInt = value;
			}
		}

		// Token: 0x17002661 RID: 9825
		// (get) Token: 0x0600F576 RID: 62838 RVA: 0x0047508C File Offset: 0x0047328C
		public string WornGraphicPath
		{
			get
			{
				if (this.StyleDef != null && !this.StyleDef.wornGraphicPath.NullOrEmpty())
				{
					return this.StyleDef.wornGraphicPath;
				}
				if (!this.def.apparel.wornGraphicPaths.NullOrEmpty<string>())
				{
					return this.def.apparel.wornGraphicPaths[this.thingIDNumber % this.def.apparel.wornGraphicPaths.Count];
				}
				return this.def.apparel.wornGraphicPath;
			}
		}

		// Token: 0x17002662 RID: 9826
		// (get) Token: 0x0600F577 RID: 62839 RVA: 0x00475118 File Offset: 0x00473318
		public override string DescriptionDetailed
		{
			get
			{
				string text = base.DescriptionDetailed;
				if (this.WornByCorpse)
				{
					text += "\n" + "WasWornByCorpse".Translate();
				}
				return text;
			}
		}

		// Token: 0x17002663 RID: 9827
		// (get) Token: 0x0600F578 RID: 62840 RVA: 0x00475158 File Offset: 0x00473358
		public override Color DrawColor
		{
			get
			{
				Color color;
				if (this.StyleDef != null && this.StyleDef.color != default(Color))
				{
					color = this.StyleDef.color;
				}
				else
				{
					color = base.DrawColor;
				}
				if (this.WornByCorpse)
				{
					color = PawnRenderUtility.GetRottenColor(color);
				}
				Pawn wearer = this.Wearer;
				if (wearer != null)
				{
					Pawn_DrawTracker drawer = wearer.Drawer;
					if (drawer != null)
					{
						PawnRenderer renderer = drawer.renderer;
						if (renderer != null)
						{
							Color? statueColor = renderer.StatueColor;
							if (statueColor != null)
							{
								Color valueOrDefault = statueColor.GetValueOrDefault();
								color = valueOrDefault;
							}
						}
					}
				}
				return color;
			}
		}

		// Token: 0x17002664 RID: 9828
		// (get) Token: 0x0600F579 RID: 62841 RVA: 0x004751EC File Offset: 0x004733EC
		// (set) Token: 0x0600F57A RID: 62842 RVA: 0x00475214 File Offset: 0x00473414
		public Color? DesiredColor
		{
			get
			{
				CompColorable comp = base.GetComp<CompColorable>();
				if (comp == null)
				{
					return null;
				}
				return comp.DesiredColor;
			}
			set
			{
				CompColorable comp = base.GetComp<CompColorable>();
				if (comp != null)
				{
					comp.DesiredColor = value;
					return;
				}
				Log.Error("Tried setting Apparel.DesiredColor without having CompColorable comp!");
			}
		}

		// Token: 0x17002665 RID: 9829
		// (get) Token: 0x0600F57B RID: 62843 RVA: 0x0047523D File Offset: 0x0047343D
		public IEnumerable<Ability> AllAbilitiesForReading
		{
			get
			{
				if (this.abilities == null && this.def.apparel.abilities != null)
				{
					this.FillOrUpdateAbilities();
				}
				if (this.abilities == null)
				{
					yield break;
				}
				foreach (Ability ability in this.abilities)
				{
					yield return ability;
				}
				List<Ability>.Enumerator enumerator = default(List<Ability>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600F57C RID: 62844 RVA: 0x00475250 File Offset: 0x00473450
		public override string GetInspectStringLowPriority()
		{
			string text = base.GetInspectStringLowPriority();
			if (this.StyleDef != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "VariantOf".Translate().CapitalizeFirst() + ": " + this.def.LabelCap;
			}
			if (ModsConfig.BiotechActive)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "WearableBy".Translate() + ": " + this.def.apparel.developmentalStageFilter.ToCommaList(false).CapitalizeFirst();
			}
			return text;
		}

		// Token: 0x0600F57D RID: 62845 RVA: 0x00475314 File Offset: 0x00473514
		public bool PawnCanWear(Pawn pawn, bool ignoreGender = false)
		{
			return this.def.IsApparel && this.def.apparel.PawnCanWear(pawn, ignoreGender);
		}

		// Token: 0x0600F57E RID: 62846 RVA: 0x0047533C File Offset: 0x0047353C
		public Color GetColorIgnoringTainted()
		{
			if (this.StyleDef != null && this.StyleDef.color != default(Color))
			{
				return this.StyleDef.color;
			}
			return base.DrawColor;
		}

		// Token: 0x0600F57F RID: 62847 RVA: 0x00475380 File Offset: 0x00473580
		public void Notify_PawnKilled()
		{
			if (this.def.apparel.careIfWornByCorpse)
			{
				this.wornByCorpseInt = true;
			}
			foreach (ThingComp thingComp in base.AllComps)
			{
				thingComp.Notify_WearerDied();
			}
		}

		// Token: 0x0600F580 RID: 62848 RVA: 0x004753EC File Offset: 0x004735EC
		public void Notify_PawnResurrected(Pawn pawn)
		{
			if (pawn.IsMutant && pawn.mutant.Def.isConsideredCorpse)
			{
				return;
			}
			this.wornByCorpseInt = false;
		}

		// Token: 0x0600F581 RID: 62849 RVA: 0x00475410 File Offset: 0x00473610
		public override void Notify_ColorChanged()
		{
			if (this.Wearer != null)
			{
				this.Wearer.apparel.Notify_ApparelChanged();
			}
			base.Notify_ColorChanged();
		}

		// Token: 0x0600F582 RID: 62850 RVA: 0x00475430 File Offset: 0x00473630
		public override void Notify_Equipped(Pawn pawn)
		{
			base.Notify_Equipped(pawn);
			foreach (Ability ability in this.AllAbilitiesForReading)
			{
				ability.pawn = pawn;
			}
		}

		// Token: 0x0600F583 RID: 62851 RVA: 0x00475484 File Offset: 0x00473684
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wornByCorpseInt, "wornByCorpse", false, false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.FillOrUpdateAbilities();
			}
		}

		// Token: 0x0600F584 RID: 62852 RVA: 0x004754C4 File Offset: 0x004736C4
		public void FillOrUpdateAbilities()
		{
			if (this.def.apparel.abilities == null)
			{
				return;
			}
			if (this.abilities == null)
			{
				this.abilities = new List<Ability>();
				foreach (AbilityDef def in this.def.apparel.abilities)
				{
					this.abilities.Add(AbilityUtility.MakeAbility(def, this.Wearer));
				}
			}
			foreach (Ability ability in this.abilities)
			{
				ability.pawn = this.Wearer;
				ability.verb.caster = this.Wearer;
			}
		}

		// Token: 0x0600F585 RID: 62853 RVA: 0x004755B0 File Offset: 0x004737B0
		public virtual void DrawWornExtras()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].CompDrawWornExtras();
			}
		}

		// Token: 0x0600F586 RID: 62854 RVA: 0x004755E4 File Offset: 0x004737E4
		public virtual bool CheckPreAbsorbDamage(DamageInfo dinfo)
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				bool flag;
				allComps[i].PostPreApplyDamage(ref dinfo, out flag);
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600F587 RID: 62855 RVA: 0x00475620 File Offset: 0x00473820
		public virtual bool AllowVerbCast(Verb verb)
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				if (!allComps[i].CompAllowVerbCast(verb))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600F588 RID: 62856 RVA: 0x00475657 File Offset: 0x00473857
		public virtual IEnumerable<Gizmo> GetWornGizmos()
		{
			List<ThingComp> comps = base.AllComps;
			int num;
			for (int i = 0; i < comps.Count; i = num + 1)
			{
				ThingComp thingComp = comps[i];
				foreach (Gizmo gizmo in thingComp.CompGetWornGizmosExtra())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F589 RID: 62857 RVA: 0x00475667 File Offset: 0x00473867
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			RoyalTitleDef royalTitleDef = (from t in DefDatabase<FactionDef>.AllDefsListForReading.SelectMany((FactionDef f) => f.RoyalTitlesAwardableInSeniorityOrderForReading)
			where t.requiredApparel != null && t.requiredApparel.Any((ApparelRequirement req) => req.ApparelMeetsRequirement(this.def, false))
			orderby t.seniority descending
			select t).FirstOrDefault<RoyalTitleDef>();
			if (royalTitleDef != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Apparel, "Stat_Thing_Apparel_MaxSatisfiedTitle".Translate(), royalTitleDef.GetLabelCapForBothGenders(), "Stat_Thing_Apparel_MaxSatisfiedTitle_Desc".Translate(), 2752, null, new Dialog_InfoCard.Hyperlink[]
				{
					new Dialog_InfoCard.Hyperlink(royalTitleDef, -1)
				}, false, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F58A RID: 62858 RVA: 0x00475678 File Offset: 0x00473878
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.WornByCorpse)
			{
				if (text.Length > 0)
				{
					text += "\n";
				}
				text += "WasWornByCorpse".Translate();
			}
			return text;
		}

		// Token: 0x0600F58B RID: 62859 RVA: 0x004756C0 File Offset: 0x004738C0
		public virtual float GetSpecialApparelScoreOffset()
		{
			float num = 0f;
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				num += allComps[i].CompGetSpecialApparelScoreOffset();
			}
			return num;
		}

		// Token: 0x0600F58C RID: 62860 RVA: 0x004756FC File Offset: 0x004738FC
		[DebugOutput]
		private static void ApparelValidLifeStages()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("name", (ThingDef t) => t.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("valid life stage", (ThingDef t) => t.apparel.developmentalStageFilter.ToCommaList(false)));
			DebugTables.MakeTablesDialog<ThingDef>(from t in DefDatabase<ThingDef>.AllDefs
			where t.IsApparel
			select t, list.ToArray());
		}

		// Token: 0x0400A1A9 RID: 41385
		private bool wornByCorpseInt;

		// Token: 0x0400A1AA RID: 41386
		private List<Ability> abilities;
	}
}
