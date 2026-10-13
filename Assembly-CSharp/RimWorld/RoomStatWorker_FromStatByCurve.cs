using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002175 RID: 8565
	public class RoomStatWorker_FromStatByCurve : RoomStatWorker
	{
		// Token: 0x0600BAED RID: 47853 RVA: 0x0035F598 File Offset: 0x0035D798
		public override float GetScore(Room room)
		{
			return this.def.curve.Evaluate(room.GetStat(this.def.inputStat));
		}
	}
}
