using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Grammar
{
	// Token: 0x02000DEE RID: 3566
	public class Rule_File : Rule
	{
		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06005AC9 RID: 23241 RVA: 0x001D2254 File Offset: 0x001D0454
		public override float BaseSelectionWeight
		{
			get
			{
				return (float)this.cachedStrings.Count;
			}
		}

		// Token: 0x06005ACA RID: 23242 RVA: 0x001D2264 File Offset: 0x001D0464
		public override Rule DeepCopy()
		{
			Rule_File rule_File = (Rule_File)base.DeepCopy();
			rule_File.path = this.path;
			if (this.pathList != null)
			{
				rule_File.pathList = this.pathList.ToList<string>();
			}
			if (this.cachedStrings != null)
			{
				rule_File.cachedStrings = this.cachedStrings.ToList<string>();
			}
			return rule_File;
		}

		// Token: 0x06005ACB RID: 23243 RVA: 0x001D22BC File Offset: 0x001D04BC
		public override string Generate()
		{
			if (this.cachedStrings.NullOrEmpty<string>())
			{
				return "Filestring";
			}
			return this.cachedStrings.RandomElement<string>();
		}

		// Token: 0x06005ACC RID: 23244 RVA: 0x001D22DC File Offset: 0x001D04DC
		public override void Init()
		{
			if (!this.path.NullOrEmpty())
			{
				this.LoadStringsFromFile(this.path);
			}
			foreach (string filePath in this.pathList)
			{
				this.LoadStringsFromFile(filePath);
			}
		}

		// Token: 0x06005ACD RID: 23245 RVA: 0x001D2348 File Offset: 0x001D0548
		private void LoadStringsFromFile(string filePath)
		{
			List<string> list;
			if (Translator.TryGetTranslatedStringsForFile(filePath, out list))
			{
				foreach (string item in list)
				{
					this.cachedStrings.Add(item);
				}
			}
		}

		// Token: 0x06005ACE RID: 23246 RVA: 0x001D23A8 File Offset: 0x001D05A8
		public override string ToString()
		{
			if (!this.path.NullOrEmpty())
			{
				return string.Concat(new string[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count.ToString(),
					" strings from file: ",
					this.path,
					")"
				});
			}
			if (this.pathList.Count > 0)
			{
				return string.Concat(new string[]
				{
					this.keyword,
					"->(",
					this.cachedStrings.Count.ToString(),
					" strings from ",
					this.pathList.Count.ToString(),
					" files)"
				});
			}
			return this.keyword + "->(Rule_File with no configuration)";
		}

		// Token: 0x040041B9 RID: 16825
		[MayTranslate]
		public string path;

		// Token: 0x040041BA RID: 16826
		[MayTranslate]
		[TranslationCanChangeCount]
		public List<string> pathList = new List<string>();

		// Token: 0x040041BB RID: 16827
		[Unsaved(false)]
		private List<string> cachedStrings = new List<string>();
	}
}
