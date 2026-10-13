using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C56 RID: 11350
	public class CompDestroyHeart : CompInteractable
	{
		// Token: 0x17002815 RID: 10261
		// (get) Token: 0x0600FE88 RID: 65160 RVA: 0x0049C89B File Offset: 0x0049AA9B
		public Building_FleshmassHeart Heart
		{
			get
			{
				return this.parent as Building_FleshmassHeart;
			}
		}

		// Token: 0x17002816 RID: 10262
		// (get) Token: 0x0600FE89 RID: 65161 RVA: 0x0049C8A8 File Offset: 0x0049AAA8
		public override bool HideInteraction
		{
			get
			{
				return !this.CanInteract(null, true);
			}
		}

		// Token: 0x17002817 RID: 10263
		// (get) Token: 0x0600FE8A RID: 65162 RVA: 0x0049C8BA File Offset: 0x0049AABA
		public override NamedArgument? ExtraNamedArg
		{
			get
			{
				return new NamedArgument?(this.Heart.BiosignatureName.Named("BIOSIGNATURE"));
			}
		}

		// Token: 0x0600FE8B RID: 65163 RVA: 0x0049C8D6 File Offset: 0x0049AAD6
		protected override void OnInteracted(Pawn caster)
		{
			Building_FleshmassHeart heart = this.Heart;
			if (heart == null)
			{
				return;
			}
			heart.StartTachycardiacOverload();
		}

		// Token: 0x0600FE8C RID: 65164 RVA: 0x0049C8E8 File Offset: 0x0049AAE8
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AnalysisDetails analysisDetails;
			Find.AnalysisManager.TryGetAnalysisProgress(this.Heart.Biosignature, out analysisDetails);
			if (analysisDetails == null || !analysisDetails.Satisfied)
			{
				return string.Format("{0}: {1}/{2}", "DestroyHeartDisabled".Translate(), (analysisDetails != null) ? analysisDetails.timesDone : 0, (analysisDetails != null) ? analysisDetails.required : 0);
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}
	}
}
