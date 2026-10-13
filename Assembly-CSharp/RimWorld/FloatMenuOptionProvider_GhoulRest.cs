using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003304 RID: 13060
	public class FloatMenuOptionProvider_GhoulRest : FloatMenuOptionProvider
	{
		// Token: 0x17002FAC RID: 12204
		// (get) Token: 0x060125EB RID: 75243 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FAD RID: 12205
		// (get) Token: 0x060125EC RID: 75244 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FAE RID: 12206
		// (get) Token: 0x060125ED RID: 75245 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060125EE RID: 75246 RVA: 0x00569D30 File Offset: 0x00567F30
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return base.AppliesInt(context) && context.FirstSelectedPawn.IsMutant && context.FirstSelectedPawn.mutant.Def == MutantDefOf.Ghoul;
		}

		// Token: 0x060125EF RID: 75247 RVA: 0x00569D64 File Offset: 0x00567F64
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Building_Bed building_Bed = clickedThing as Building_Bed;
			if (building_Bed == null)
			{
				return null;
			}
			return building_Bed.GetBedRestFloatMenuOption(context.FirstSelectedPawn);
		}
	}
}
