using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020017DA RID: 6106
	public class ThinkNode_DutyConstant : ThinkNode
	{
		// Token: 0x06008E0B RID: 36363 RVA: 0x002A1D5C File Offset: 0x0029FF5C
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			Lord lord = pawn.GetLord();
			if (lord == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " doing ThinkNode_DutyConstant with no Lord.");
				return ThinkResult.NoJob;
			}
			if (pawn.mindState.duty == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " doing ThinkNode_DutyConstant with no duty.");
				return ThinkResult.NoJob;
			}
			if (this.dutyDefToSubNode == null)
			{
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " has null dutyDefToSubNode. Recovering by calling ResolveSubnodes() (though that should have been called already).");
				this.ResolveSubnodes();
			}
			int num = this.dutyDefToSubNode[pawn.mindState.duty.def];
			if (num < 0)
			{
				return ThinkResult.NoJob;
			}
			ThinkResult result = this.subNodes[num].TryIssueJobPackage(pawn, jobParams);
			result = lord.Notify_DutyConstantResult(result, pawn, jobParams);
			if (result.Job != null)
			{
				result.Job.lord = pawn.GetLord();
				result.Job.source = pawn.mindState.duty.source;
				result.Job.dutyTag = pawn.mindState.duty.tag;
			}
			return result;
		}

		// Token: 0x06008E0C RID: 36364 RVA: 0x002A1E88 File Offset: 0x002A0088
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_DutyConstant thinkNode_DutyConstant = (ThinkNode_DutyConstant)base.DeepCopy(resolve);
			if (this.dutyDefToSubNode != null)
			{
				thinkNode_DutyConstant.dutyDefToSubNode = new DefMap<DutyDef, int>();
				thinkNode_DutyConstant.dutyDefToSubNode.SetAll(-1);
				foreach (DutyDef def in DefDatabase<DutyDef>.AllDefs)
				{
					thinkNode_DutyConstant.dutyDefToSubNode[def] = this.dutyDefToSubNode[def];
				}
			}
			return thinkNode_DutyConstant;
		}

		// Token: 0x06008E0D RID: 36365 RVA: 0x002A1F14 File Offset: 0x002A0114
		protected override void ResolveSubnodes()
		{
			this.dutyDefToSubNode = new DefMap<DutyDef, int>();
			this.dutyDefToSubNode.SetAll(-1);
			foreach (DutyDef dutyDef in DefDatabase<DutyDef>.AllDefs)
			{
				if (dutyDef.constantThinkNode != null)
				{
					this.dutyDefToSubNode[dutyDef] = this.subNodes.Count;
					dutyDef.constantThinkNode.ResolveSubnodesAndRecur();
					this.subNodes.Add(dutyDef.constantThinkNode.DeepCopy(true));
				}
			}
		}

		// Token: 0x04005BE9 RID: 23529
		private DefMap<DutyDef, int> dutyDefToSubNode;
	}
}
