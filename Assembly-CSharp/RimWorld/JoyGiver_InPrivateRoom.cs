using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001513 RID: 5395
	public class JoyGiver_InPrivateRoom : JoyGiver
	{
		// Token: 0x06008096 RID: 32918 RVA: 0x0026C098 File Offset: 0x0026A298
		public override Job TryGiveJob(Pawn pawn)
		{
			JoyGiver_InPrivateRoom.<>c__DisplayClass0_0 CS$<>8__locals1 = new JoyGiver_InPrivateRoom.<>c__DisplayClass0_0();
			CS$<>8__locals1.pawn = pawn;
			Room room = null;
			if (ModsConfig.IdeologyActive)
			{
				room = MeditationUtility.UsableWorshipRooms(CS$<>8__locals1.pawn).RandomElementWithFallback(null);
			}
			if (room == null)
			{
				Pawn_Ownership ownership = CS$<>8__locals1.pawn.ownership;
				room = ((ownership != null) ? ownership.OwnedRoom : null);
			}
			if (room == null)
			{
				return null;
			}
			IntVec3 c;
			if (!room.Cells.Where(new Func<IntVec3, bool>(CS$<>8__locals1.<TryGiveJob>g__Validator|0)).TryRandomElement(out c))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}

		// Token: 0x06008097 RID: 32919 RVA: 0x0026C125 File Offset: 0x0026A325
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return JobMaker.MakeJob(this.def.jobDef, pawn.CurrentBed());
		}
	}
}
