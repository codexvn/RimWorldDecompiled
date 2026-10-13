using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020025B2 RID: 9650
	public abstract class LandingOutcomeWorker
	{
		// Token: 0x17002089 RID: 8329
		// (get) Token: 0x0600D374 RID: 54132 RVA: 0x003E540E File Offset: 0x003E360E
		protected virtual LetterDef LetterDef
		{
			get
			{
				return this.def.letterDef;
			}
		}

		// Token: 0x1700208A RID: 8330
		// (get) Token: 0x0600D375 RID: 54133 RVA: 0x003E541B File Offset: 0x003E361B
		protected virtual string LetterLabel
		{
			get
			{
				return this.def.letterLabel;
			}
		}

		// Token: 0x1700208B RID: 8331
		// (get) Token: 0x0600D376 RID: 54134 RVA: 0x003E5428 File Offset: 0x003E3628
		protected virtual string LetterText
		{
			get
			{
				return this.def.letterText;
			}
		}

		// Token: 0x0600D377 RID: 54135 RVA: 0x003E5435 File Offset: 0x003E3635
		public LandingOutcomeWorker(LandingOutcomeDef def)
		{
			this.def = def;
		}

		// Token: 0x0600D378 RID: 54136 RVA: 0x003E5444 File Offset: 0x003E3644
		public virtual void ApplyOutcome(Gravship gravship)
		{
			this.SendStandardLetter(gravship.Engine, null, gravship.Engine);
		}

		// Token: 0x0600D379 RID: 54137 RVA: 0x003E5460 File Offset: 0x003E3660
		protected void SendStandardLetter(Building_GravEngine Engine, string extraText, LookTargets letterTarget)
		{
			TaggedString taggedString = this.LetterText.Formatted(Engine.RenamableLabel.Named("GRAVSHIP"), Engine.launchInfo.pilot.Named("PILOT"), Engine.launchInfo.copilot.Named("COPILOT"));
			if (extraText != null)
			{
				taggedString += "\n\n" + extraText;
			}
			taggedString += "\n\n" + "GravshipOutcomeChanceLetterExtra".Translate(GravshipUtility.NegativeLandingOutcomeFromQuality(Engine.launchInfo.quality));
			Find.LetterStack.ReceiveLetter(this.LetterLabel, taggedString, this.LetterDef, letterTarget, null, null, null, null, 0, true);
		}

		// Token: 0x04008FA6 RID: 36774
		private LandingOutcomeDef def;
	}
}
