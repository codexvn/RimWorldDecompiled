using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200192B RID: 6443
	public class BookOutcomeProperties_GainResearch : BookOutcomeProperties
	{
		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x06009287 RID: 37511 RVA: 0x002B2898 File Offset: 0x002B0A98
		public override Type DoerClass
		{
			get
			{
				return typeof(ReadingOutcomeDoerGainResearch);
			}
		}

		// Token: 0x06009288 RID: 37512 RVA: 0x002B28A4 File Offset: 0x002B0AA4
		public void PostLoad()
		{
			if (this.tab != null)
			{
				this.tabs.Add(new BookOutcomeProperties_GainResearch.BookTabItem
				{
					tab = this.tab
				});
			}
		}

		// Token: 0x04005D83 RID: 23939
		public List<BookOutcomeProperties_GainResearch.BookResearchItem> include = new List<BookOutcomeProperties_GainResearch.BookResearchItem>();

		// Token: 0x04005D84 RID: 23940
		public List<BookOutcomeProperties_GainResearch.BookResearchItem> exclude = new List<BookOutcomeProperties_GainResearch.BookResearchItem>();

		// Token: 0x04005D85 RID: 23941
		public ResearchTabDef tab;

		// Token: 0x04005D86 RID: 23942
		public List<BookOutcomeProperties_GainResearch.BookTabItem> tabs = new List<BookOutcomeProperties_GainResearch.BookTabItem>();

		// Token: 0x04005D87 RID: 23943
		public bool ignoreZeroBaseCost = true;

		// Token: 0x04005D88 RID: 23944
		public bool usesHiddenProjects;

		// Token: 0x0200192C RID: 6444
		public class BookResearchItem
		{
			// Token: 0x0600928A RID: 37514 RVA: 0x002B28FA File Offset: 0x002B0AFA
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "project", xmlRoot.Name, null, null, null);
			}

			// Token: 0x04005D89 RID: 23945
			public ResearchProjectDef project;
		}

		// Token: 0x0200192D RID: 6445
		public class BookTabItem
		{
			// Token: 0x0600928C RID: 37516 RVA: 0x002B2910 File Offset: 0x002B0B10
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "tab", xmlRoot.Name, null, null, null);
			}

			// Token: 0x04005D8A RID: 23946
			public ResearchTabDef tab;
		}
	}
}
