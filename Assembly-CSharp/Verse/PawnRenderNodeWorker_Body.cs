using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000638 RID: 1592
	public class PawnRenderNodeWorker_Body : PawnRenderNodeWorker
	{
		// Token: 0x06002855 RID: 10325 RVA: 0x000D5A48 File Offset: 0x000D3C48
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			if (!base.CanDrawNow(node, parms))
			{
				return false;
			}
			if (!parms.Portrait)
			{
				if (parms.flags.FlagSet(PawnRenderFlags.NoBody))
				{
					return false;
				}
				if (parms.posture == PawnPosture.Standing)
				{
					return true;
				}
				Pawn_MindState mindState = parms.pawn.mindState;
				bool flag;
				if (mindState == null)
				{
					flag = false;
				}
				else
				{
					PawnDuty duty = mindState.duty;
					bool? flag2;
					if (duty == null)
					{
						flag2 = null;
					}
					else
					{
						DutyDef def = duty.def;
						flag2 = ((def != null) ? new bool?(def.drawBodyOverride != null) : null);
					}
					bool? flag3 = flag2;
					bool flag4 = true;
					flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
				}
				if (flag)
				{
					return parms.pawn.mindState.duty.def.drawBodyOverride.Value;
				}
				if (parms.bed != null && parms.pawn.RaceProps.Humanlike)
				{
					return parms.bed.def.building.bed_showSleeperBody;
				}
			}
			return true;
		}
	}
}
