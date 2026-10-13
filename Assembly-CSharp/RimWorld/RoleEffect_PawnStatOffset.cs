using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200281D RID: 10269
	public class RoleEffect_PawnStatOffset : RoleEffect_PawnStatModifier
	{
		// Token: 0x0600E0A5 RID: 57509 RVA: 0x0041F3B1 File Offset: 0x0041D5B1
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.statDef.LabelCap + ": " + this.statDef.ValueToString(this.modifier, ToStringNumberSense.Offset, false);
		}
	}
}
