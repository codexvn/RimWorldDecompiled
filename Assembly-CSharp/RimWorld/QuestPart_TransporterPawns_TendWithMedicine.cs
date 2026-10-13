using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC9 RID: 7369
	public class QuestPart_TransporterPawns_TendWithMedicine : QuestPart_TransporterPawns_Tend
	{
		// Token: 0x0600A429 RID: 42025 RVA: 0x002F10CC File Offset: 0x002EF2CC
		protected override void DoTend(Pawn pawn)
		{
			Pawn doctor = null;
			if (this.allowSelfTend && pawn.playerSettings != null && pawn.playerSettings.selfTend && pawn.GetStatValue(StatDefOf.MedicalTendQuality, true, -1) > 0.75f)
			{
				doctor = pawn;
			}
			Medicine medicine = (Medicine)ThingMaker.MakeThing(this.medicineDef, null);
			TendUtility.DoTend(doctor, pawn, medicine);
		}

		// Token: 0x0600A42A RID: 42026 RVA: 0x002F1128 File Offset: 0x002EF328
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.medicineDef, "medicineDef");
			Scribe_Values.Look<bool>(ref this.allowSelfTend, "allowSelfTend", false, false);
		}

		// Token: 0x04007162 RID: 29026
		public ThingDef medicineDef;

		// Token: 0x04007163 RID: 29027
		public bool allowSelfTend;
	}
}
