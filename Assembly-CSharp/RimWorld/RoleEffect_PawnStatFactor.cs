using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200281E RID: 10270
	public class RoleEffect_PawnStatFactor : RoleEffect_PawnStatModifier
	{
		// Token: 0x0600E0A7 RID: 57511 RVA: 0x0041F3ED File Offset: 0x0041D5ED
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.statDef.LabelCap + ": " + this.statDef.ValueToString(this.modifier, ToStringNumberSense.Factor, false);
		}
	}
}
