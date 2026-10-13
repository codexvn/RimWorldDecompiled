using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020028D8 RID: 10456
	public static class PlayerPawnsArriveMethodExtension
	{
		// Token: 0x0600E4A1 RID: 58529 RVA: 0x0042C3D4 File Offset: 0x0042A5D4
		public static string ToStringHuman(this PlayerPawnsArriveMethod method)
		{
			switch (method)
			{
			case PlayerPawnsArriveMethod.Standing:
				return "PlayerPawnsArriveMethod_Standing".Translate();
			case PlayerPawnsArriveMethod.DropPods:
				return "PlayerPawnsArriveMethod_DropPods".Translate();
			case PlayerPawnsArriveMethod.Gravship:
				return "PlayerPawnsArriveMethod_Gravship".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
