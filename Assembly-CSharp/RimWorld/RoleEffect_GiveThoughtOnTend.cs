using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002821 RID: 10273
	public class RoleEffect_GiveThoughtOnTend : RoleEffect
	{
		// Token: 0x0600E0AD RID: 57517 RVA: 0x0041F495 File Offset: 0x0041D695
		public RoleEffect_GiveThoughtOnTend()
		{
			this.labelKey = "RoleEffectGiveThoughtOnTended";
		}

		// Token: 0x0600E0AE RID: 57518 RVA: 0x0041F4A8 File Offset: 0x0041D6A8
		public override string Label(Pawn pawn, Precept_Role role)
		{
			return this.labelKey.Translate() + ": " + this.thoughtDef.LabelCap.Formatted(role.LabelCap.ResolveTags());
		}

		// Token: 0x0600E0AF RID: 57519 RVA: 0x0041F4F4 File Offset: 0x0041D6F4
		public override void Notify_Tended(Pawn doctor, Pawn patient)
		{
			base.Notify_Tended(doctor, patient);
			if (doctor != patient && doctor.Ideo == patient.Ideo)
			{
				patient.needs.mood.thoughts.memories.TryGainMemory(this.thoughtDef, doctor, null);
			}
		}

		// Token: 0x04009701 RID: 38657
		public ThoughtDef thoughtDef;
	}
}
