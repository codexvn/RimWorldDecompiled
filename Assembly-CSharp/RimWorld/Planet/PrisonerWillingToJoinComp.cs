using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E30 RID: 15920
	public class PrisonerWillingToJoinComp : ImportantPawnComp, IThingHolder
	{
		// Token: 0x1700364F RID: 13903
		// (get) Token: 0x06015BFA RID: 89082 RVA: 0x006808D7 File Offset: 0x0067EAD7
		protected override string PawnSaveKey
		{
			get
			{
				return "prisoner";
			}
		}

		// Token: 0x06015BFB RID: 89083 RVA: 0x006808DE File Offset: 0x0067EADE
		protected override void RemovePawnOnWorldObjectRemoved()
		{
			this.pawn.ClearAndDestroyContentsOrPassToWorld(DestroyMode.Vanish);
		}

		// Token: 0x06015BFC RID: 89084 RVA: 0x006808EC File Offset: 0x0067EAEC
		public override string CompInspectStringExtra()
		{
			if (this.pawn.Any)
			{
				return "Prisoner".Translate() + ": " + this.pawn[0].LabelCap;
			}
			return null;
		}
	}
}
