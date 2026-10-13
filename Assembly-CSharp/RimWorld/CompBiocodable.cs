using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C0B RID: 11275
	public class CompBiocodable : ThingComp
	{
		// Token: 0x170027B2 RID: 10162
		// (get) Token: 0x0600FCA1 RID: 64673 RVA: 0x00494407 File Offset: 0x00492607
		public bool Biocoded
		{
			get
			{
				return this.biocoded;
			}
		}

		// Token: 0x170027B3 RID: 10163
		// (get) Token: 0x0600FCA2 RID: 64674 RVA: 0x0049440F File Offset: 0x0049260F
		public Pawn CodedPawn
		{
			get
			{
				return this.codedPawn;
			}
		}

		// Token: 0x170027B4 RID: 10164
		// (get) Token: 0x0600FCA3 RID: 64675 RVA: 0x00494417 File Offset: 0x00492617
		public string CodedPawnLabel
		{
			get
			{
				return this.codedPawnLabel;
			}
		}

		// Token: 0x170027B5 RID: 10165
		// (get) Token: 0x0600FCA4 RID: 64676 RVA: 0x0049441F File Offset: 0x0049261F
		public CompProperties_Biocodable Props
		{
			get
			{
				return (CompProperties_Biocodable)this.props;
			}
		}

		// Token: 0x170027B6 RID: 10166
		// (get) Token: 0x0600FCA5 RID: 64677 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Biocodable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600FCA6 RID: 64678 RVA: 0x0049442C File Offset: 0x0049262C
		public static bool IsBiocoded(Thing thing)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.Biocoded;
		}

		// Token: 0x0600FCA7 RID: 64679 RVA: 0x0049444C File Offset: 0x0049264C
		public static bool IsBiocodedFor(Thing thing, Pawn pawn)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.CodedPawn == pawn;
		}

		// Token: 0x0600FCA8 RID: 64680 RVA: 0x0049446E File Offset: 0x0049266E
		public virtual void CodeFor(Pawn p)
		{
			if (!this.Biocodable)
			{
				return;
			}
			this.biocoded = true;
			this.codedPawn = p;
			this.codedPawnLabel = p.Name.ToStringFull;
			this.OnCodedFor(p);
		}

		// Token: 0x0600FCA9 RID: 64681 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void OnCodedFor(Pawn p)
		{
		}

		// Token: 0x0600FCAA RID: 64682 RVA: 0x0049449F File Offset: 0x0049269F
		public virtual void UnCode()
		{
			this.biocoded = false;
			this.codedPawn = null;
			this.codedPawnLabel = null;
		}

		// Token: 0x0600FCAB RID: 64683 RVA: 0x004944B6 File Offset: 0x004926B6
		public override void Notify_Equipped(Pawn pawn)
		{
			if (this.Biocodable && this.Props.biocodeOnEquip)
			{
				this.CodeFor(pawn);
			}
		}

		// Token: 0x0600FCAC RID: 64684 RVA: 0x004944D4 File Offset: 0x004926D4
		public override string TransformLabel(string label)
		{
			if (!this.biocoded)
			{
				return label;
			}
			return "Biocoded".Translate(label, this.parent.def).Resolve();
		}

		// Token: 0x0600FCAD RID: 64685 RVA: 0x00494514 File Offset: 0x00492714
		public override string CompInspectStringExtra()
		{
			if (!this.biocoded)
			{
				return string.Empty;
			}
			return "CodedFor".Translate(this.codedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
		}

		// Token: 0x0600FCAE RID: 64686 RVA: 0x00494553 File Offset: 0x00492753
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.biocoded)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_Biocoded_Name".Translate(), this.codedPawnLabel, "Stat_Thing_Biocoded_Desc".Translate(), 1104, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x0600FCAF RID: 64687 RVA: 0x00494564 File Offset: 0x00492764
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.biocoded, "biocoded", false, false);
			Scribe_Values.Look<string>(ref this.codedPawnLabel, "biocodedPawnLabel", null, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.codedPawn != null && this.codedPawn.Discarded)
			{
				this.codedPawn = null;
			}
			Scribe_References.Look<Pawn>(ref this.codedPawn, "codedPawn", true);
		}

		// Token: 0x0400A597 RID: 42391
		protected bool biocoded;

		// Token: 0x0400A598 RID: 42392
		protected string codedPawnLabel;

		// Token: 0x0400A599 RID: 42393
		protected Pawn codedPawn;
	}
}
