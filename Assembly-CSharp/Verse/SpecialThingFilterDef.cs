using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D63 RID: 3427
	public class SpecialThingFilterDef : Def
	{
		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x0600572A RID: 22314 RVA: 0x001C0FA2 File Offset: 0x001BF1A2
		public SpecialThingFilterWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SpecialThingFilterWorker)Activator.CreateInstance(this.workerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x001C0FC8 File Offset: 0x001BF1C8
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.workerClass == null)
			{
				yield return "SpecialThingFilterDef " + this.defName + " has no worker class.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x001C0FD8 File Offset: 0x001BF1D8
		public static SpecialThingFilterDef Named(string defName)
		{
			return DefDatabase<SpecialThingFilterDef>.GetNamed(defName, true);
		}

		// Token: 0x04003F86 RID: 16262
		public ThingCategoryDef parentCategory;

		// Token: 0x04003F87 RID: 16263
		public string saveKey;

		// Token: 0x04003F88 RID: 16264
		public bool allowedByDefault;

		// Token: 0x04003F89 RID: 16265
		public bool configurable = true;

		// Token: 0x04003F8A RID: 16266
		public Type workerClass;

		// Token: 0x04003F8B RID: 16267
		[Unsaved(false)]
		private SpecialThingFilterWorker workerInt;
	}
}
