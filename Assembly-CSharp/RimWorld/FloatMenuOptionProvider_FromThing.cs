using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003301 RID: 13057
	public class FloatMenuOptionProvider_FromThing : FloatMenuOptionProvider
	{
		// Token: 0x17002FA3 RID: 12195
		// (get) Token: 0x060125D6 RID: 75222 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FA4 RID: 12196
		// (get) Token: 0x060125D7 RID: 75223 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FA5 RID: 12197
		// (get) Token: 0x060125D8 RID: 75224 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FA6 RID: 12198
		// (get) Token: 0x060125D9 RID: 75225 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanSelfTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060125DA RID: 75226 RVA: 0x00569AB3 File Offset: 0x00567CB3
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			if (context.IsMultiselect)
			{
				foreach (FloatMenuOption floatMenuOption in clickedThing.GetMultiSelectFloatMenuOptions(context.ValidSelectedPawns))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
			}
			else
			{
				foreach (FloatMenuOption floatMenuOption2 in clickedThing.GetFloatMenuOptions(context.FirstSelectedPawn))
				{
					yield return floatMenuOption2;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
			}
			yield break;
			yield break;
		}
	}
}
