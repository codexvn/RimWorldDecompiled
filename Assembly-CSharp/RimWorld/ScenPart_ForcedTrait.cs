using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200287A RID: 10362
	public class ScenPart_ForcedTrait : ScenPart_PawnModifier
	{
		// Token: 0x0600E2CD RID: 58061 RVA: 0x004263F2 File Offset: 0x004245F2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TraitDef>(ref this.trait, "trait");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
		}

		// Token: 0x0600E2CE RID: 58062 RVA: 0x0042641C File Offset: 0x0042461C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			if (Widgets.ButtonText(scenPartRect.TopPart(0.333f), this.trait.DataAtDegree(this.degree).LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (TraitDef traitDef in from td in DefDatabase<TraitDef>.AllDefs
				orderby td.label
				select td)
				{
					foreach (TraitDegreeData localDeg2 in traitDef.degreeDatas)
					{
						TraitDef localDef = traitDef;
						TraitDegreeData localDeg = localDeg2;
						list.Add(new FloatMenuOption(localDeg.LabelCap, delegate()
						{
							this.trait = localDef;
							this.degree = localDeg.degree;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			base.DoPawnModifierEditInterface(scenPartRect.BottomPart(0.666f));
		}

		// Token: 0x0600E2CF RID: 58063 RVA: 0x0042658C File Offset: 0x0042478C
		public override string Summary(Scenario scen)
		{
			return "ScenPart_PawnsHaveTrait".Translate(this.context.ToStringHuman(), this.chance.ToStringPercent(), this.trait.DataAtDegree(this.degree).LabelCap).CapitalizeFirst();
		}

		// Token: 0x0600E2D0 RID: 58064 RVA: 0x004265EB File Offset: 0x004247EB
		public override void Randomize()
		{
			base.Randomize();
			this.trait = DefDatabase<TraitDef>.GetRandom();
			this.degree = this.trait.degreeDatas.RandomElement<TraitDegreeData>().degree;
		}

		// Token: 0x0600E2D1 RID: 58065 RVA: 0x0042661C File Offset: 0x0042481C
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_ForcedTrait scenPart_ForcedTrait = other as ScenPart_ForcedTrait;
			return scenPart_ForcedTrait == null || this.trait != scenPart_ForcedTrait.trait || !this.context.OverlapsWith(scenPart_ForcedTrait.context);
		}

		// Token: 0x0600E2D2 RID: 58066 RVA: 0x00426658 File Offset: 0x00424858
		protected override void ModifyPawnPostGenerate(Pawn pawn, bool redressed)
		{
			if (pawn.story == null || pawn.story.traits == null || this.trait == null)
			{
				return;
			}
			if (pawn.story.traits.HasTrait(this.trait) && pawn.story.traits.DegreeOfTrait(this.trait) == this.degree)
			{
				return;
			}
			if (pawn.story.traits.HasTrait(this.trait))
			{
				pawn.story.traits.allTraits.RemoveAll((Trait tr) => tr.def == this.trait);
			}
			else
			{
				IEnumerable<Trait> source = from tr in pawn.story.traits.allTraits
				where !tr.ScenForced && !ScenPart_ForcedTrait.PawnHasTraitForcedByBackstory(pawn, tr.def)
				select tr;
				if (source.Any<Trait>())
				{
					Trait trait = (from tr in source
					where this.trait.ConflictsWith(tr.def)
					select tr).FirstOrDefault<Trait>();
					if (trait != null)
					{
						pawn.story.traits.allTraits.Remove(trait);
					}
					else
					{
						pawn.story.traits.allTraits.Remove(source.RandomElement<Trait>());
					}
				}
			}
			Trait trait2 = new Trait(this.trait, this.degree, true);
			pawn.story.traits.GainTrait(trait2, false);
			TraitUtility.ApplySkillGainFromTrait(pawn, trait2);
		}

		// Token: 0x0600E2D3 RID: 58067 RVA: 0x004267EC File Offset: 0x004249EC
		private static bool PawnHasTraitForcedByBackstory(Pawn pawn, TraitDef trait)
		{
			return (pawn.story.Childhood != null && pawn.story.Childhood.forcedTraits != null && pawn.story.Childhood.forcedTraits.Any((BackstoryTrait te) => te.def == trait)) || (pawn.story.Adulthood != null && pawn.story.Adulthood.forcedTraits != null && pawn.story.Adulthood.forcedTraits.Any((BackstoryTrait te) => te.def == trait));
		}

		// Token: 0x0600E2D4 RID: 58068 RVA: 0x0042688F File Offset: 0x00424A8F
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.trait == null;
		}

		// Token: 0x0600E2D5 RID: 58069 RVA: 0x004268A4 File Offset: 0x00424AA4
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.trait != null) ? this.trait.GetHashCode() : 0) ^ this.degree;
		}

		// Token: 0x0400981A RID: 38938
		private TraitDef trait;

		// Token: 0x0400981B RID: 38939
		private int degree;
	}
}
