using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200259C RID: 9628
	public class GoodwillSituationWorker_MemeCompatibility : GoodwillSituationWorker
	{
		// Token: 0x0600D309 RID: 54025 RVA: 0x003E2908 File Offset: 0x003E0B08
		public override string GetPostProcessedLabel(Faction other)
		{
			if (this.def.otherMeme != null)
			{
				return base.GetPostProcessedLabel(other);
			}
			if (this.Applies(Faction.OfPlayer, other))
			{
				return "MemeGoodwillImpact_Player".Translate(base.GetPostProcessedLabel(other));
			}
			return "MemeGoodwillImpact_Other".Translate(base.GetPostProcessedLabel(other));
		}

		// Token: 0x0600D30A RID: 54026 RVA: 0x003E296F File Offset: 0x003E0B6F
		public override int GetNaturalGoodwillOffset(Faction other)
		{
			if (!this.Applies(other))
			{
				return 0;
			}
			return this.def.naturalGoodwillOffset;
		}

		// Token: 0x0600D30B RID: 54027 RVA: 0x003E2987 File Offset: 0x003E0B87
		private bool Applies(Faction other)
		{
			return this.Applies(Faction.OfPlayer, other) || this.Applies(other, Faction.OfPlayer);
		}

		// Token: 0x0600D30C RID: 54028 RVA: 0x003E29A8 File Offset: 0x003E0BA8
		private bool Applies(Faction a, Faction b)
		{
			Ideo primaryIdeo = a.ideos.PrimaryIdeo;
			if (primaryIdeo == null)
			{
				return false;
			}
			if (this.def.versusAll)
			{
				return primaryIdeo.memes.Contains(this.def.meme);
			}
			Ideo primaryIdeo2 = b.ideos.PrimaryIdeo;
			return primaryIdeo2 != null && primaryIdeo.memes.Contains(this.def.meme) && primaryIdeo2.memes.Contains(this.def.otherMeme);
		}
	}
}
