using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200345F RID: 13407
	public static class DrugPolicyUIUtility
	{
		// Token: 0x06012DAF RID: 77231 RVA: 0x00596920 File Offset: 0x00594B20
		public static void DoAssignDrugPolicyButtons(Rect rect, Pawn pawn)
		{
			if (pawn.drugs.CurrentPolicy == null)
			{
				return;
			}
			Rect rect2 = rect.ContractedBy(0f, 2f);
			string text = pawn.drugs.CurrentPolicy.label;
			Pawn_StoryTracker story = pawn.story;
			if (((story != null) ? story.traits : null) != null)
			{
				Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
				Gene_ChemicalDependency gene_ChemicalDependency;
				if (trait != null)
				{
					text = text + " (" + trait.Label + ")";
				}
				else if (ModsConfig.BiotechActive && PawnUtility.TryGetChemicalDependencyGene(pawn, out gene_ChemicalDependency))
				{
					text = text + " (" + gene_ChemicalDependency.Label + ")";
				}
			}
			Widgets.Dropdown<Pawn, DrugPolicy>(rect2, pawn, (Pawn p) => p.drugs.CurrentPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>>(DrugPolicyUIUtility.Button_GenerateMenu), text.Truncate(rect2.width, null), null, pawn.drugs.CurrentPolicy.label, null, delegate()
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
			}, true);
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
		}

		// Token: 0x06012DB0 RID: 77232 RVA: 0x00596A48 File Offset: 0x00594C48
		private static IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<DrugPolicy>.Enumerator enumerator = Current.Game.drugPolicyDatabase.AllPolicies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DrugPolicy assignedDrugs = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<DrugPolicy>
					{
						option = new FloatMenuOption(assignedDrugs.label, delegate()
						{
							pawn.drugs.CurrentPolicy = assignedDrugs;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = assignedDrugs
					};
				}
			}
			List<DrugPolicy>.Enumerator enumerator = default(List<DrugPolicy>.Enumerator);
			yield return new Widgets.DropdownMenuElement<DrugPolicy>
			{
				option = new FloatMenuOption(string.Format("{0}...", "AssignTabEdit".Translate()), delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageDrugPolicies(pawn.drugs.CurrentPolicy));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			};
			yield break;
			yield break;
		}

		// Token: 0x0400C38E RID: 50062
		public const string AssigningDrugsTutorHighlightTag = "ButtonAssignDrugs";
	}
}
