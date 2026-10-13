using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A2A RID: 14890
	public class QuestNode_GeneratePawn : QuestNode
	{
		// Token: 0x06014634 RID: 83508 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014635 RID: 83509 RVA: 0x0061770B File Offset: 0x0061590B
		protected virtual DevelopmentalStage GetDevelopmentalStage(Slate slate)
		{
			if (!Find.Storyteller.difficulty.ChildrenAllowed || !this.isChild.GetValue(slate))
			{
				return DevelopmentalStage.Adult;
			}
			return DevelopmentalStage.Child;
		}

		// Token: 0x06014636 RID: 83510 RVA: 0x00617730 File Offset: 0x00615930
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			PawnKindDef value = this.kindDef.GetValue(slate);
			Faction value2 = this.faction.GetValue(slate);
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			bool flag = this.allowAddictions.GetValue(slate) ?? true;
			bool value3 = this.allowPregnant.GetValue(slate);
			IEnumerable<TraitDef> value4 = this.forcedTraits.GetValue(slate);
			IEnumerable<TraitDef> value5 = this.prohibitedTraits.GetValue(slate);
			float value6 = this.biocodeWeaponChance.GetValue(slate);
			bool value7 = this.mustBeCapableOfViolence.GetValue(slate);
			Pawn value8 = this.extraPawnForExtraRelationChance.GetValue(slate);
			float value9 = this.relationWithExtraPawnChanceFactor.GetValue(slate);
			Gender? value10 = this.fixedGender.GetValue(slate);
			float value11 = this.biocodeApparelChance.GetValue(slate);
			DevelopmentalStage developmentalStage = this.GetDevelopmentalStage(slate);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(value, value2, context, null, false, false, false, true, value7, 1f, false, true, value3, true, flag, false, false, false, false, value6, value11, value8, value9, null, null, value4, value5, null, null, null, value10, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStage, null, null, null, false, false, false, -1, 0, false)
			{
				BiocodeApparelChance = this.biocodeApparelChance.GetValue(slate),
				ForbidAnyTitle = this.forbidAnyTitle.GetValue(slate)
			});
			if (this.ensureNonNumericName.GetValue(slate) && (pawn.Name == null || pawn.Name.Numerical))
			{
				pawn.Name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, null, false, null);
			}
			if (this.giveDependentDrugs.GetValue(slate) && ModsConfig.BiotechActive && pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					if (gene.Active)
					{
						Gene_ChemicalDependency dep = gene as Gene_ChemicalDependency;
						if (dep != null)
						{
							ThingDef def;
							if ((from x in DefDatabase<ThingDef>.AllDefs
							where x.IsDrug && x.GetCompProperties<CompProperties_Drug>().chemical == dep.def.chemical
							select x).TryRandomElementByWeight((ThingDef x) => x.generateCommonality, out def))
							{
								Thing thing = ThingMaker.MakeThing(def, null);
								thing.stackCount = Rand.Range(1, 3);
								pawn.inventory.innerContainer.TryAddOrTransfer(thing, true);
							}
						}
					}
				}
			}
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Pawn>(this.storeAs.GetValue(slate), pawn, false);
			}
			if (this.addToList.GetValue(slate) != null)
			{
				QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.addToList.GetValue(slate), pawn);
			}
			if (this.addToLists.GetValue(slate) != null)
			{
				foreach (string name in this.addToLists.GetValue(slate))
				{
					QuestGenUtility.AddToOrMakeList(QuestGen.slate, name, pawn);
				}
			}
			QuestGen.AddToGeneratedPawns(pawn);
			if (!pawn.IsWorldPawn())
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
			}
		}

		// Token: 0x0400E12F RID: 57647
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E130 RID: 57648
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x0400E131 RID: 57649
		[NoTranslate]
		public SlateRef<IEnumerable<string>> addToLists;

		// Token: 0x0400E132 RID: 57650
		public SlateRef<PawnKindDef> kindDef;

		// Token: 0x0400E133 RID: 57651
		public SlateRef<Faction> faction;

		// Token: 0x0400E134 RID: 57652
		public SlateRef<bool> forbidAnyTitle;

		// Token: 0x0400E135 RID: 57653
		public SlateRef<bool> ensureNonNumericName;

		// Token: 0x0400E136 RID: 57654
		public SlateRef<IEnumerable<TraitDef>> forcedTraits;

		// Token: 0x0400E137 RID: 57655
		public SlateRef<IEnumerable<TraitDef>> prohibitedTraits;

		// Token: 0x0400E138 RID: 57656
		public SlateRef<Pawn> extraPawnForExtraRelationChance;

		// Token: 0x0400E139 RID: 57657
		public SlateRef<float> relationWithExtraPawnChanceFactor;

		// Token: 0x0400E13A RID: 57658
		public SlateRef<bool?> allowAddictions;

		// Token: 0x0400E13B RID: 57659
		public SlateRef<float> biocodeWeaponChance;

		// Token: 0x0400E13C RID: 57660
		public SlateRef<float> biocodeApparelChance;

		// Token: 0x0400E13D RID: 57661
		public SlateRef<bool> mustBeCapableOfViolence;

		// Token: 0x0400E13E RID: 57662
		public SlateRef<bool> isChild;

		// Token: 0x0400E13F RID: 57663
		public SlateRef<bool> allowPregnant;

		// Token: 0x0400E140 RID: 57664
		public SlateRef<Gender?> fixedGender;

		// Token: 0x0400E141 RID: 57665
		public SlateRef<bool> giveDependentDrugs;

		// Token: 0x0400E142 RID: 57666
		private const int MinExpertSkill = 11;
	}
}
