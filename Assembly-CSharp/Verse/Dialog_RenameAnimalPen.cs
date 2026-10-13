using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C3B RID: 3131
	public class Dialog_RenameAnimalPen : Dialog_Rename<CompAnimalPenMarker>
	{
		// Token: 0x0600501E RID: 20510 RVA: 0x0019CD1A File Offset: 0x0019AF1A
		public Dialog_RenameAnimalPen(Map map, CompAnimalPenMarker marker) : base(marker)
		{
			this.map = map;
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x0019CD2C File Offset: 0x0019AF2C
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (name != this.renaming.label && this.map.animalPenManager.GetPenNamed(name) != null)
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x040039BE RID: 14782
		private readonly Map map;
	}
}
