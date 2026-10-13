using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020017D9 RID: 6105
	public class ThinkNode_Duty : ThinkNode
	{
		// Token: 0x06008E08 RID: 36360 RVA: 0x002A1C04 File Offset: 0x0029FE04
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " doing ThinkNode_Duty with no Lord.");
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.duty == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " doing ThinkNode_Duty with no duty.");
				return ThinkResult.NoJob;
			}
			ThinkResult result = this.subNodes[(int)pawn.mindState.duty.def.index].TryIssueJobPackage(pawn, jobParams);
			result = lord.Notify_DutyResult(result, pawn, jobParams);
			if (result.Job != null)
			{
				result.Job.lord = pawn.GetLord();
				result.Job.source = pawn.mindState.duty.source;
				result.Job.dutyTag = pawn.mindState.duty.tag;
			}
			return result;
		}

		// Token: 0x06008E09 RID: 36361 RVA: 0x002A1CF4 File Offset: 0x0029FEF4
		protected override void ResolveSubnodes()
		{
			foreach (DutyDef dutyDef in DefDatabase<DutyDef>.AllDefs)
			{
				dutyDef.thinkNode.ResolveSubnodesAndRecur();
				this.subNodes.Add(dutyDef.thinkNode.DeepCopy(true));
			}
		}
	}
}
