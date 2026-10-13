using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B0F RID: 6927
	public class ThoughtStage
	{
		// Token: 0x170017BC RID: 6076
		// (get) Token: 0x06009AAC RID: 39596 RVA: 0x002CF067 File Offset: 0x002CD267
		public string LabelCap
		{
			get
			{
				if (this.cachedLabelCap == null)
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x170017BD RID: 6077
		// (get) Token: 0x06009AAD RID: 39597 RVA: 0x002CF088 File Offset: 0x002CD288
		public string LabelSocialCap
		{
			get
			{
				if (this.cachedLabelSocialCap == null)
				{
					this.cachedLabelSocialCap = this.labelSocial.CapitalizeFirst();
				}
				return this.cachedLabelSocialCap;
			}
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x06009AAE RID: 39598 RVA: 0x002CF0A9 File Offset: 0x002CD2A9
		public string LabelAbstractCap
		{
			get
			{
				if (this.cachedLabelAbstractCap == null)
				{
					this.cachedLabelAbstractCap = ((this.labelAbstract != null) ? this.labelAbstract.CapitalizeFirst() : this.label.CapitalizeFirst());
				}
				return this.cachedLabelAbstractCap;
			}
		}

		// Token: 0x06009AAF RID: 39599 RVA: 0x002CF0DF File Offset: 0x002CD2DF
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelSocial = this.labelSocial;
		}

		// Token: 0x06009AB0 RID: 39600 RVA: 0x002CF0F9 File Offset: 0x002CD2F9
		public IEnumerable<string> ConfigErrors()
		{
			if (!this.labelSocial.NullOrEmpty() && this.labelSocial == this.label)
			{
				yield return "labelSocial is the same as label. labelSocial is unnecessary in this case";
			}
			if (this.baseMoodEffect != 0f && this.description.NullOrEmpty())
			{
				yield return "affects mood but doesn't have a description";
			}
			yield break;
		}

		// Token: 0x04006AA8 RID: 27304
		[MustTranslate]
		public string label;

		// Token: 0x04006AA9 RID: 27305
		[MustTranslate]
		public string labelSocial;

		// Token: 0x04006AAA RID: 27306
		[MustTranslate]
		public string labelAbstract;

		// Token: 0x04006AAB RID: 27307
		[MustTranslate]
		public string description;

		// Token: 0x04006AAC RID: 27308
		public float baseMoodEffect;

		// Token: 0x04006AAD RID: 27309
		public float baseOpinionOffset;

		// Token: 0x04006AAE RID: 27310
		public bool visible = true;

		// Token: 0x04006AAF RID: 27311
		[Unsaved(false)]
		private string cachedLabelCap;

		// Token: 0x04006AB0 RID: 27312
		[Unsaved(false)]
		private string cachedLabelSocialCap;

		// Token: 0x04006AB1 RID: 27313
		[Unsaved(false)]
		private string cachedLabelAbstractCap;

		// Token: 0x04006AB2 RID: 27314
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabel;

		// Token: 0x04006AB3 RID: 27315
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelSocial;
	}
}
