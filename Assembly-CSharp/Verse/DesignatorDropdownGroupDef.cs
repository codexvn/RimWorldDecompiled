using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000274 RID: 628
	public class DesignatorDropdownGroupDef : Def
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x0004B7FA File Offset: 0x000499FA
		public IEnumerable<BuildableDef> BuildablesWithoutDefaultDesignators()
		{
			return from x in DefDatabase<ThingDef>.AllDefs.Concat(DefDatabase<TerrainDef>.AllDefs)
			where x.designatorDropdown == this && !x.canGenerateDefaultDesignator
			select x;
		}

		// Token: 0x04000C3F RID: 3135
		public DesignatorDropdownGroupDef.IconSource iconSource;

		// Token: 0x04000C40 RID: 3136
		public bool useGridMenu;

		// Token: 0x04000C41 RID: 3137
		public bool includeEyeDropperTool;

		// Token: 0x02000275 RID: 629
		public enum IconSource : byte
		{
			// Token: 0x04000C43 RID: 3139
			Cost,
			// Token: 0x04000C44 RID: 3140
			Placed
		}
	}
}
