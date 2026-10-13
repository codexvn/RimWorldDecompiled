using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAA RID: 6826
	public class PawnRelationDef : Def
	{
		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x060098E2 RID: 39138 RVA: 0x002C9114 File Offset: 0x002C7314
		public PawnRelationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnRelationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060098E3 RID: 39139 RVA: 0x002C9146 File Offset: 0x002C7346
		public string GetGenderSpecificLabel(Pawn pawn)
		{
			if (pawn.gender == Gender.Female && !this.labelFemale.NullOrEmpty())
			{
				return this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x060098E4 RID: 39140 RVA: 0x002C916B File Offset: 0x002C736B
		public string GetGenderSpecificLabelCap(Pawn pawn)
		{
			return this.GetGenderSpecificLabel(pawn).CapitalizeFirst();
		}

		// Token: 0x060098E5 RID: 39141 RVA: 0x002C9179 File Offset: 0x002C7379
		public ThoughtDef GetGenderSpecificDiedThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.diedThoughtFemale;
			}
			return this.diedThought;
		}

		// Token: 0x060098E6 RID: 39142 RVA: 0x002C9199 File Offset: 0x002C7399
		public ThoughtDef GetGenderSpecificLostThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.diedThoughtFemale != null)
			{
				return this.lostThoughtFemale;
			}
			return this.lostThought;
		}

		// Token: 0x060098E7 RID: 39143 RVA: 0x002C91B9 File Offset: 0x002C73B9
		public ThoughtDef GetGenderSpecificKilledThought(Pawn killed)
		{
			if (killed.gender == Gender.Female && this.killedThoughtFemale != null)
			{
				return this.killedThoughtFemale;
			}
			return this.killedThought;
		}

		// Token: 0x060098E8 RID: 39144 RVA: 0x002C91D9 File Offset: 0x002C73D9
		public ThoughtDef GetGenderSpecificThought(Pawn pawn, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
			{
				return this.GetGenderSpecificLostThought(pawn);
			}
			return this.GetGenderSpecificDiedThought(pawn);
		}

		// Token: 0x060098E9 RID: 39145 RVA: 0x002C91EE File Offset: 0x002C73EE
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.implied && this.reflexive)
			{
				yield return this.defName + ": implied relations can't use the \"reflexive\" option.";
				this.reflexive = false;
			}
			yield break;
			yield break;
		}

		// Token: 0x040067B6 RID: 26550
		public Type workerClass = typeof(PawnRelationWorker);

		// Token: 0x040067B7 RID: 26551
		[MustTranslate]
		public string labelFemale;

		// Token: 0x040067B8 RID: 26552
		public float importance;

		// Token: 0x040067B9 RID: 26553
		public bool implied;

		// Token: 0x040067BA RID: 26554
		public bool reflexive;

		// Token: 0x040067BB RID: 26555
		public int opinionOffset;

		// Token: 0x040067BC RID: 26556
		public float generationChanceFactor;

		// Token: 0x040067BD RID: 26557
		public float romanceChanceFactor = 1f;

		// Token: 0x040067BE RID: 26558
		public float incestOpinionOffset;

		// Token: 0x040067BF RID: 26559
		public bool familyByBloodRelation;

		// Token: 0x040067C0 RID: 26560
		public bool removeOnDeath;

		// Token: 0x040067C1 RID: 26561
		public bool removeOnLeftBehind;

		// Token: 0x040067C2 RID: 26562
		public float inbredChanceOnChild;

		// Token: 0x040067C3 RID: 26563
		public ThoughtDef diedThought;

		// Token: 0x040067C4 RID: 26564
		public ThoughtDef diedThoughtFemale;

		// Token: 0x040067C5 RID: 26565
		public ThoughtDef lostThought;

		// Token: 0x040067C6 RID: 26566
		public ThoughtDef lostThoughtFemale;

		// Token: 0x040067C7 RID: 26567
		public List<ThoughtDef> soldThoughts;

		// Token: 0x040067C8 RID: 26568
		public ThoughtDef killedThought;

		// Token: 0x040067C9 RID: 26569
		public ThoughtDef killedThoughtFemale;

		// Token: 0x040067CA RID: 26570
		[Unsaved(false)]
		private PawnRelationWorker workerInt;
	}
}
