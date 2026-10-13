using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02003300 RID: 13056
	public class FloatMenuOptionProvider_FromLord : FloatMenuOptionProvider
	{
		// Token: 0x17002FA0 RID: 12192
		// (get) Token: 0x060125D1 RID: 75217 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FA1 RID: 12193
		// (get) Token: 0x060125D2 RID: 75218 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FA2 RID: 12194
		// (get) Token: 0x060125D3 RID: 75219 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060125D4 RID: 75220 RVA: 0x00569A84 File Offset: 0x00567C84
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			Lord lord = clickedPawn.GetLord();
			if (lord == null)
			{
				return Enumerable.Empty<FloatMenuOption>();
			}
			return lord.CurLordToil.ExtraFloatMenuOptions(clickedPawn, context.FirstSelectedPawn);
		}
	}
}
