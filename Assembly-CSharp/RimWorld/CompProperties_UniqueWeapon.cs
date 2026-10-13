using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EB8 RID: 11960
	public class CompProperties_UniqueWeapon : CompProperties
	{
		// Token: 0x06010E01 RID: 69121 RVA: 0x004DBF7F File Offset: 0x004DA17F
		public CompProperties_UniqueWeapon()
		{
			this.compClass = typeof(CompUniqueWeapon);
		}

		// Token: 0x0400AFA7 RID: 44967
		public List<WeaponCategoryDef> weaponCategories = new List<WeaponCategoryDef>();

		// Token: 0x0400AFA8 RID: 44968
		[MustTranslate]
		public List<string> namerLabels = new List<string>();
	}
}
