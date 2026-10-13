using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019EF RID: 6639
	public class BackstoryCategoryFilter
	{
		// Token: 0x060095D8 RID: 38360 RVA: 0x002BDF80 File Offset: 0x002BC180
		public bool Matches(PawnBio bio)
		{
			if (this.exclude != null && this.exclude.Any((string e) => bio.adulthood.spawnCategories.Contains(e) || bio.childhood.spawnCategories.Contains(e)))
			{
				return false;
			}
			if (this.excludeChildhood != null && bio.childhood.spawnCategories.Any((string e) => this.excludeChildhood.Contains(e)))
			{
				return false;
			}
			if (this.excludeAdulthood != null && bio.adulthood.spawnCategories.Any((string e) => this.excludeAdulthood.Contains(e)))
			{
				return false;
			}
			bool flag = true;
			if (this.categoriesChildhood != null)
			{
				flag &= bio.childhood.spawnCategories.Any((string e) => this.categoriesChildhood.Contains(e));
			}
			if (this.categoriesAdulthood != null)
			{
				flag &= bio.adulthood.spawnCategories.Any((string e) => this.categoriesAdulthood.Contains(e));
			}
			if (this.categoriesChildhood != null || this.categoriesAdulthood != null)
			{
				return flag;
			}
			return this.categories == null || this.categories.Any((string c) => bio.adulthood.spawnCategories.Contains(c) || bio.childhood.spawnCategories.Contains(c));
		}

		// Token: 0x060095D9 RID: 38361 RVA: 0x002BE0AC File Offset: 0x002BC2AC
		public bool Matches(BackstoryDef backstory)
		{
			if (this.exclude != null && backstory.spawnCategories.Any((string e) => this.exclude.Contains(e)))
			{
				return false;
			}
			if (!this.excludeChildhood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Childhood && backstory.spawnCategories.Any((string e) => this.excludeChildhood.Contains(e)))
			{
				return false;
			}
			if (!this.excludeAdulthood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Adulthood && backstory.spawnCategories.Any((string e) => this.excludeAdulthood.Contains(e)))
			{
				return false;
			}
			if (!this.categoriesChildhood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Childhood && !backstory.spawnCategories.Any((string c) => this.categoriesChildhood.Contains(c)))
			{
				return false;
			}
			if (!this.categoriesAdulthood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Adulthood && !backstory.spawnCategories.Any((string c) => this.categoriesAdulthood.Contains(c)))
			{
				return false;
			}
			if (backstory.requiresSpawnCategory)
			{
				if (this.categories.NullOrEmpty<string>() && this.categoriesChildhood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Childhood)
				{
					return false;
				}
				if (this.categories.NullOrEmpty<string>() && this.categoriesAdulthood.NullOrEmpty<string>() && backstory.slot == BackstorySlot.Adulthood)
				{
					return false;
				}
			}
			return this.categories == null || backstory.spawnCategories.Any((string c) => this.categories.Contains(c));
		}

		// Token: 0x040061E4 RID: 25060
		public List<string> categories;

		// Token: 0x040061E5 RID: 25061
		public List<string> exclude;

		// Token: 0x040061E6 RID: 25062
		public List<string> categoriesChildhood;

		// Token: 0x040061E7 RID: 25063
		public List<string> excludeChildhood;

		// Token: 0x040061E8 RID: 25064
		public List<string> categoriesAdulthood;

		// Token: 0x040061E9 RID: 25065
		public List<string> excludeAdulthood;

		// Token: 0x040061EA RID: 25066
		public float commonality = 1f;
	}
}
