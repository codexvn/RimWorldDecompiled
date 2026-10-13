using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002929 RID: 10537
	public class TaleReference : IExposable
	{
		// Token: 0x170022FD RID: 8957
		// (get) Token: 0x0600E683 RID: 59011 RVA: 0x00433CF7 File Offset: 0x00431EF7
		public static TaleReference Taleless
		{
			get
			{
				return new TaleReference(null);
			}
		}

		// Token: 0x170022FE RID: 8958
		// (get) Token: 0x0600E684 RID: 59012 RVA: 0x00433CFF File Offset: 0x00431EFF
		public bool IsNullTale
		{
			get
			{
				return this.tale == null;
			}
		}

		// Token: 0x0600E685 RID: 59013 RVA: 0x000024DF File Offset: 0x000006DF
		public TaleReference()
		{
		}

		// Token: 0x0600E686 RID: 59014 RVA: 0x00433D0A File Offset: 0x00431F0A
		public TaleReference(Tale tale)
		{
			this.tale = tale;
			this.seed = Rand.Range(0, int.MaxValue);
		}

		// Token: 0x0600E687 RID: 59015 RVA: 0x00433D2A File Offset: 0x00431F2A
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.seed, "seed", 0, false);
			Scribe_References.Look<Tale>(ref this.tale, "tale", false);
		}

		// Token: 0x0600E688 RID: 59016 RVA: 0x00433D4F File Offset: 0x00431F4F
		public void ReferenceDestroyed()
		{
			if (this.tale != null)
			{
				this.tale.Notify_ReferenceDestroyed();
				this.tale = null;
			}
		}

		// Token: 0x0600E689 RID: 59017 RVA: 0x00433D6B File Offset: 0x00431F6B
		public TaggedString GenerateText(TextGenerationPurpose purpose, RulePackDef extraInclude, List<Rule> extraRules = null, Dictionary<string, string> extraConstants = null)
		{
			return TaleTextGenerator.GenerateTextFromTale(purpose, this.tale, this.seed, new List<RulePackDef>
			{
				extraInclude
			}, extraRules, extraConstants);
		}

		// Token: 0x0600E68A RID: 59018 RVA: 0x00433D90 File Offset: 0x00431F90
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"TaleReference(tale=",
				(this.tale == null) ? "null" : this.tale.ToString(),
				", seed=",
				this.seed.ToString(),
				")"
			});
		}

		// Token: 0x04009A2C RID: 39468
		private Tale tale;

		// Token: 0x04009A2D RID: 39469
		private int seed;
	}
}
