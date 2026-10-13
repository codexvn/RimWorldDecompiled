using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DF3 RID: 11763
	public class CompRoyalImplant : ThingComp
	{
		// Token: 0x17002A52 RID: 10834
		// (get) Token: 0x060108E7 RID: 67815 RVA: 0x004C5F6A File Offset: 0x004C416A
		public CompProperties_RoyalImplant Props
		{
			get
			{
				return (CompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x060108E8 RID: 67816 RVA: 0x004C5F77 File Offset: 0x004C4177
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(this.Props.implantHediff);
			if (minTitleForImplantAllFactions.First != null)
			{
				Faction first = minTitleForImplantAllFactions.First;
				StringBuilder stringBuilder = new StringBuilder("Stat_Thing_MinimumRoyalTitle_Desc".Translate(first.Named("FACTION")));
				if (typeof(Hediff_Level).IsAssignableFrom(this.Props.implantHediff.hediffClass))
				{
					stringBuilder.Append("\n\n" + "Stat_Thing_MinimumRoyalTitle_Level_Desc".Translate(first.Named("FACTION")) + ":\n\n");
					int num = 1;
					while ((float)num <= this.Props.implantHediff.maxSeverity)
					{
						stringBuilder.Append(string.Concat(new string[]
						{
							" -  x",
							num.ToString(),
							", ",
							first.GetMinTitleForImplant(this.Props.implantHediff, num).GetLabelCapForBothGenders(),
							"\n"
						}));
						num++;
					}
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(first.Named("FACTION")).Resolve(), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders(), stringBuilder.ToTaggedString().Resolve(), 2100, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x060108E9 RID: 67817 RVA: 0x004C5F88 File Offset: 0x004C4188
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(this.Props.implantHediff);
			if (minTitleForImplantAllFactions.First != null)
			{
				stringBuilder.AppendLine("MinimumRoyalTitleInspectString".Translate(minTitleForImplantAllFactions.First.Named("FACTION"), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders().Named("TITLE")).Resolve());
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060108EA RID: 67818 RVA: 0x004C600C File Offset: 0x004C420C
		public static TaggedString CheckForViolations(Pawn pawn, HediffDef hediff, int levelOffset)
		{
			if (levelOffset < 0)
			{
				return "";
			}
			if (pawn.Faction != Faction.OfPlayer || !hediff.HasComp(typeof(HediffComp_RoyalImplant)))
			{
				return "";
			}
			Hediff_Level hediff_Level = (Hediff_Level)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == hediff);
			int num = (levelOffset != 0 && hediff_Level != null) ? (hediff_Level.level + levelOffset) : 0;
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (pawn.Faction != null && !faction.Hidden && !faction.HostileTo(Faction.OfPlayer) && ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(hediff, pawn, faction, num))
				{
					RoyalTitleDef minTitleForImplant = faction.GetMinTitleForImplant(hediff, num);
					HediffCompProperties_RoyalImplant hediffCompProperties_RoyalImplant = hediff.CompProps<HediffCompProperties_RoyalImplant>();
					string arg = hediff.label + ((num == 0) ? "" : (" (" + num.ToString() + "x)"));
					TaggedString taggedString = hediffCompProperties_RoyalImplant.violationTriggerDescriptionKey.Translate(pawn.Named("PAWN"));
					TaggedString taggedString2 = "RoyalImplantIllegalUseWarning".Translate(pawn.Named("PAWN"), arg.Named("IMPLANT"), faction.Named("FACTION"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE"), taggedString.Named("VIOLATIONTRIGGER"));
					if (levelOffset != 0)
					{
						taggedString2 += "\n\n" + "RoyalImplantUpgradeConfirmation".Translate();
					}
					else
					{
						taggedString2 += "\n\n" + "RoyalImplantInstallConfirmation".Translate();
					}
					return taggedString2;
				}
			}
			return "";
		}
	}
}
