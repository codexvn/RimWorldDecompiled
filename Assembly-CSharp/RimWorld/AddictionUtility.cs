using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200384B RID: 14411
	public static class AddictionUtility
	{
		// Token: 0x06013EE7 RID: 81639 RVA: 0x005E9E1A File Offset: 0x005E801A
		public static bool HasChemicalDependency(Pawn pawn, Thing drug)
		{
			return AddictionUtility.HasChemicalDependency(pawn, drug.def);
		}

		// Token: 0x06013EE8 RID: 81640 RVA: 0x005E9E28 File Offset: 0x005E8028
		public static bool HasChemicalDependency(Pawn pawn, ThingDef drug)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			CompProperties_Drug compProperties = drug.GetCompProperties<CompProperties_Drug>();
			ChemicalDef chemicalDef = (compProperties != null) ? compProperties.chemical : null;
			if (chemicalDef == null)
			{
				return false;
			}
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				Hediff_ChemicalDependency hediff_ChemicalDependency = hediff as Hediff_ChemicalDependency;
				if (hediff_ChemicalDependency != null && hediff_ChemicalDependency.chemical == chemicalDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013EE9 RID: 81641 RVA: 0x005E9EB8 File Offset: 0x005E80B8
		public static bool IsAddicted(Pawn pawn, Thing drug)
		{
			return AddictionUtility.FindAddictionHediff(pawn, drug) != null;
		}

		// Token: 0x06013EEA RID: 81642 RVA: 0x005E9EC4 File Offset: 0x005E80C4
		public static bool IsAddicted(Pawn pawn, ChemicalDef chemical)
		{
			return AddictionUtility.FindAddictionHediff(pawn, chemical) != null;
		}

		// Token: 0x06013EEB RID: 81643 RVA: 0x005E9ED0 File Offset: 0x005E80D0
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return null;
			}
			CompDrug compDrug = drug.TryGetComp<CompDrug>();
			if (!compDrug.Props.Addictive)
			{
				return null;
			}
			return AddictionUtility.FindAddictionHediff(pawn, compDrug.Props.chemical);
		}

		// Token: 0x06013EEC RID: 81644 RVA: 0x005E9F14 File Offset: 0x005E8114
		public static Hediff_Addiction FindAddictionHediff(Pawn pawn, ChemicalDef chemical)
		{
			return (Hediff_Addiction)pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.addictionHediff);
		}

		// Token: 0x06013EED RID: 81645 RVA: 0x005E9F54 File Offset: 0x005E8154
		public static Hediff FindToleranceHediff(Pawn pawn, ChemicalDef chemical)
		{
			if (chemical.toleranceHediff == null)
			{
				return null;
			}
			return pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == chemical.toleranceHediff);
		}

		// Token: 0x06013EEE RID: 81646 RVA: 0x005E9FA0 File Offset: 0x005E81A0
		public static void ModifyChemicalEffectForToleranceAndBodySize(Pawn pawn, ChemicalDef chemicalDef, ref float effect, bool applyGeneToleranceFactor, bool divideByBodySize = true)
		{
			if (chemicalDef != null)
			{
				List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					hediffs[i].ModifyChemicalEffect(chemicalDef, ref effect);
				}
			}
			if (applyGeneToleranceFactor && ModsConfig.BiotechActive && pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					if (gene.Active && gene.def.chemical == chemicalDef)
					{
						effect *= gene.def.toleranceBuildupFactor;
					}
				}
			}
			if (divideByBodySize)
			{
				effect /= pawn.BodySize;
			}
		}

		// Token: 0x06013EEF RID: 81647 RVA: 0x005EA06C File Offset: 0x005E826C
		public static void CheckDrugAddictionTeachOpportunity(Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh || !pawn.Spawned)
			{
				return;
			}
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return;
			}
			if (!AddictionUtility.AddictedToAnything(pawn))
			{
				return;
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.DrugAddiction, pawn, OpportunityType.Important);
		}

		// Token: 0x06013EF0 RID: 81648 RVA: 0x005EA0C0 File Offset: 0x005E82C0
		public static bool AddictedToAnything(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i] is Hediff_Addiction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06013EF1 RID: 81649 RVA: 0x005EA100 File Offset: 0x005E8300
		public static bool CanBingeOnNow(Pawn pawn, ChemicalDef chemical, DrugCategory drugCategory)
		{
			if (!chemical.canBinge)
			{
				return false;
			}
			if (!pawn.Spawned)
			{
				return false;
			}
			List<Thing> list = pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Drug);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].Position.Fogged(list[i].Map) && (drugCategory == DrugCategory.Any || list[i].def.ingestible.drugCategory == drugCategory) && list[i].TryGetComp<CompDrug>().Props.chemical == chemical && (list[i].Position.Roofed(list[i].Map) || list[i].Position.InHorDistOf(pawn.Position, 45f)) && pawn.CanReach(list[i], PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return true;
				}
			}
			return false;
		}
	}
}
