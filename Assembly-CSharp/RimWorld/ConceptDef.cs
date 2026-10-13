using System;
using System.Collections.Generic;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02001B24 RID: 6948
	public class ConceptDef : Def
	{
		// Token: 0x170017DC RID: 6108
		// (get) Token: 0x06009B2B RID: 39723 RVA: 0x002D0B3F File Offset: 0x002CED3F
		public bool TriggeredDirect
		{
			get
			{
				return this.priority <= 0f;
			}
		}

		// Token: 0x170017DD RID: 6109
		// (get) Token: 0x06009B2C RID: 39724 RVA: 0x002D0B51 File Offset: 0x002CED51
		public string HelpTextAdjusted
		{
			get
			{
				if (!SteamDeck.IsSteamDeckInNonKeyboardMode || this.helpTextController.NullOrEmpty())
				{
					return this.helpText.AdjustedForKeys(null, true);
				}
				return this.helpTextController.AdjustedForKeys(null, true);
			}
		}

		// Token: 0x06009B2D RID: 39725 RVA: 0x002D0B82 File Offset: 0x002CED82
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.priority > 9999999f)
			{
				yield return "priority isn't set";
			}
			if (this.helpText.NullOrEmpty())
			{
				yield return "no help text";
			}
			if (this.TriggeredDirect && this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			ConceptDef.tmpParseErrors.Clear();
			this.helpText.AdjustedForKeys(ConceptDef.tmpParseErrors, false);
			int num;
			for (int i = 0; i < ConceptDef.tmpParseErrors.Count; i = num + 1)
			{
				yield return "helpText error: " + ConceptDef.tmpParseErrors[i];
				num = i;
			}
			if (!this.helpTextController.NullOrEmpty())
			{
				ConceptDef.tmpParseErrors.Clear();
				this.helpTextController.AdjustedForKeys(ConceptDef.tmpParseErrors, false);
				for (int i = 0; i < ConceptDef.tmpParseErrors.Count; i = num + 1)
				{
					yield return "helpText error (controller): " + ConceptDef.tmpParseErrors[i];
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06009B2E RID: 39726 RVA: 0x002D0B92 File Offset: 0x002CED92
		public static ConceptDef Named(string defName)
		{
			return DefDatabase<ConceptDef>.GetNamed(defName, true);
		}

		// Token: 0x06009B2F RID: 39727 RVA: 0x002D0B9C File Offset: 0x002CED9C
		public void HighlightAllTags()
		{
			if (this.highlightTags != null)
			{
				for (int i = 0; i < this.highlightTags.Count; i++)
				{
					UIHighlighter.HighlightTag(this.highlightTags[i]);
				}
			}
		}

		// Token: 0x04006B75 RID: 27509
		public float priority = float.MaxValue;

		// Token: 0x04006B76 RID: 27510
		public bool noteTeaches;

		// Token: 0x04006B77 RID: 27511
		public bool needsOpportunity;

		// Token: 0x04006B78 RID: 27512
		public bool opportunityDecays = true;

		// Token: 0x04006B79 RID: 27513
		public ProgramState gameMode = ProgramState.Playing;

		// Token: 0x04006B7A RID: 27514
		[MustTranslate]
		private string helpText;

		// Token: 0x04006B7B RID: 27515
		[MustTranslate]
		private string helpTextController;

		// Token: 0x04006B7C RID: 27516
		[NoTranslate]
		public List<string> highlightTags;

		// Token: 0x04006B7D RID: 27517
		private static List<string> tmpParseErrors = new List<string>();
	}
}
