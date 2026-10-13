using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003338 RID: 13112
	public class FloatMenuOptionProvider_Romance : FloatMenuOptionProvider
	{
		// Token: 0x17002FFA RID: 12282
		// (get) Token: 0x060126E1 RID: 75489 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Drafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FFB RID: 12283
		// (get) Token: 0x060126E2 RID: 75490 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FFC RID: 12284
		// (get) Token: 0x060126E3 RID: 75491 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060126E4 RID: 75492 RVA: 0x00563089 File Offset: 0x00561289
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x060126E5 RID: 75493 RVA: 0x0056E71C File Offset: 0x0056C91C
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.IsColonist || clickedPawn.HostFaction != null)
			{
				return null;
			}
			if (clickedPawn.Drafted)
			{
				return null;
			}
			FloatMenuOption floatMenuOption;
			float num;
			bool flag = RelationsUtility.RomanceOption(context.FirstSelectedPawn, clickedPawn, out floatMenuOption, out num);
			if (floatMenuOption == null)
			{
				return null;
			}
			floatMenuOption.Label = (flag ? "CanRomance" : "CannotRomance").Translate(floatMenuOption.Label);
			return floatMenuOption;
		}
	}
}
