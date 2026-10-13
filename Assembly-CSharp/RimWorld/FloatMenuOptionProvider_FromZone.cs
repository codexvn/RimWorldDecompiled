using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003303 RID: 13059
	public class FloatMenuOptionProvider_FromZone : FloatMenuOptionProvider
	{
		// Token: 0x17002FA9 RID: 12201
		// (get) Token: 0x060125E6 RID: 75238 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FAA RID: 12202
		// (get) Token: 0x060125E7 RID: 75239 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FAB RID: 12203
		// (get) Token: 0x060125E8 RID: 75240 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060125E9 RID: 75241 RVA: 0x00569D0F File Offset: 0x00567F0F
		public override IEnumerable<FloatMenuOption> GetOptions(FloatMenuContext context)
		{
			if (context.ClickedZone == null)
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
			return context.ClickedZone.GetFloatMenuOptions(context.FirstSelectedPawn);
		}
	}
}
