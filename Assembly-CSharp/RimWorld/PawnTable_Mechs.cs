using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200348A RID: 13450
	public class PawnTable_Mechs : PawnTable
	{
		// Token: 0x06012EBC RID: 77500 RVA: 0x0059A754 File Offset: 0x00598954
		protected override IEnumerable<Pawn> LabelSortFunction(IEnumerable<Pawn> input)
		{
			return input.OrderBy(new Func<Pawn, int>(PawnTable_Mechs.<LabelSortFunction>g__Overseer|0_0)).ThenBy(new Func<Pawn, int>(PawnTable_Mechs.<LabelSortFunction>g__ControlGroupIndex|0_1)).ThenBy((Pawn p) => p.KindLabel).ThenBy((Pawn p) => p.Label);
		}

		// Token: 0x06012EBD RID: 77501 RVA: 0x0059A6E6 File Offset: 0x005988E6
		public PawnTable_Mechs(PawnTableDef def, Func<IEnumerable<Pawn>> pawnsGetter, int uiWidth, int uiHeight) : base(def, pawnsGetter, uiWidth, uiHeight)
		{
		}

		// Token: 0x06012EBE RID: 77502 RVA: 0x0059A7CC File Offset: 0x005989CC
		[CompilerGenerated]
		internal static int <LabelSortFunction>g__Overseer|0_0(Pawn pawn)
		{
			Pawn overseer = pawn.GetOverseer();
			if (overseer == null)
			{
				return int.MaxValue;
			}
			return overseer.thingIDNumber;
		}

		// Token: 0x06012EBF RID: 77503 RVA: 0x0059A7F0 File Offset: 0x005989F0
		[CompilerGenerated]
		internal static int <LabelSortFunction>g__ControlGroupIndex|0_1(Pawn pawn)
		{
			MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
			if (mechControlGroup == null)
			{
				return int.MaxValue;
			}
			return mechControlGroup.Index;
		}
	}
}
