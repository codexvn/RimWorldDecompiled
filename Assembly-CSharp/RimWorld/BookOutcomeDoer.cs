using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001920 RID: 6432
	public abstract class BookOutcomeDoer : ReadingOutcomeDoer
	{
		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x06009245 RID: 37445 RVA: 0x002B207F File Offset: 0x002B027F
		public new BookOutcomeProperties Props
		{
			get
			{
				return (BookOutcomeProperties)this.props;
			}
		}

		// Token: 0x17001622 RID: 5666
		// (get) Token: 0x06009246 RID: 37446 RVA: 0x002B208C File Offset: 0x002B028C
		public Book Book
		{
			get
			{
				return (Book)base.Parent;
			}
		}

		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x06009247 RID: 37447 RVA: 0x002B209C File Offset: 0x002B029C
		private CompQuality CompQuality
		{
			get
			{
				CompQuality result;
				if ((result = this.compQualityCached) == null)
				{
					result = (this.compQualityCached = base.Parent.GetComp<CompQuality>());
				}
				return result;
			}
		}

		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06009248 RID: 37448 RVA: 0x002B20C7 File Offset: 0x002B02C7
		public QualityCategory Quality
		{
			get
			{
				return this.CompQuality.Quality;
			}
		}

		// Token: 0x06009249 RID: 37449
		public abstract bool DoesProvidesOutcome(Pawn reader);

		// Token: 0x0600924A RID: 37450 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnBookGenerated(Pawn author = null)
		{
		}

		// Token: 0x0600924B RID: 37451 RVA: 0x00035ADE File Offset: 0x00033CDE
		public virtual string GetBenefitsString(Pawn reader = null)
		{
			return "";
		}

		// Token: 0x0600924C RID: 37452 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool BenefitDetailsCanChange(Pawn reader = null)
		{
			return false;
		}

		// Token: 0x0600924D RID: 37453 RVA: 0x002B20D4 File Offset: 0x002B02D4
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			yield break;
		}

		// Token: 0x0600924E RID: 37454 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<RulePack> GetTopicRulePacks()
		{
			return null;
		}

		// Token: 0x0600924F RID: 37455 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Rule_String> GetTopicRuleStrings()
		{
			return null;
		}

		// Token: 0x06009250 RID: 37456 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06009251 RID: 37457 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Reset()
		{
		}

		// Token: 0x04005D68 RID: 23912
		private CompQuality compQualityCached;
	}
}
