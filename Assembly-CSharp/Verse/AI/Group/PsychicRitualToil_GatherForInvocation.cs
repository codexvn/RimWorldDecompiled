using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200106D RID: 4205
	public class PsychicRitualToil_GatherForInvocation : PsychicRitualToil_Multiplex
	{
		// Token: 0x060066C7 RID: 26311 RVA: 0x002082C0 File Offset: 0x002064C0
		protected PsychicRitualToil_GatherForInvocation()
		{
		}

		// Token: 0x060066C8 RID: 26312 RVA: 0x002082C8 File Offset: 0x002064C8
		protected PsychicRitualToil_GatherForInvocation(PsychicRitualDef_InvocationCircle def, PsychicRitualToil_Goto fallbackToil, PsychicRitualGraph invokerToil) : base(new Dictionary<PsychicRitualRoleDef, PsychicRitualToil>
		{
			{
				def.InvokerRole,
				invokerToil
			}
		}, fallbackToil)
		{
			this.fallbackToil = fallbackToil;
			this.invokerToil = invokerToil;
			this.invokerFinalToil = (PsychicRitualToil_Goto)invokerToil.GetToil(invokerToil.ToilCount - 1);
		}

		// Token: 0x060066C9 RID: 26313 RVA: 0x00208315 File Offset: 0x00206515
		public PsychicRitualToil_GatherForInvocation(PsychicRitual psychicRitual, PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions) : this(def, PsychicRitualToil_GatherForInvocation.FallbackToil(psychicRitual, def, rolePositions), PsychicRitualToil_GatherForInvocation.InvokerToil(def, rolePositions))
		{
		}

		// Token: 0x060066CA RID: 26314 RVA: 0x0020832D File Offset: 0x0020652D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<PsychicRitualToil_Goto>(ref this.fallbackToil, "fallbackToil", false);
			Scribe_References.Look<PsychicRitualGraph>(ref this.invokerToil, "invokerToil", false);
			Scribe_References.Look<PsychicRitualToil_Goto>(ref this.invokerFinalToil, "invokerFinalToil", false);
		}

		// Token: 0x060066CB RID: 26315 RVA: 0x00208368 File Offset: 0x00206568
		public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			PsychicRitualToil_GatherForInvocation.blockingPawns.Clear();
			PsychicRitualToil_GatherForInvocation.blockingPawns.AddRange(this.fallbackToil.BlockingPawns);
			if (this.invokerToil.CurrentToil == this.invokerFinalToil)
			{
				PsychicRitualToil_GatherForInvocation.blockingPawns.AddRange(this.invokerFinalToil.BlockingPawns);
			}
			else
			{
				PsychicRitualToil_GatherForInvocation.blockingPawns.AddRange(this.invokerFinalToil.ControlledPawns(psychicRitual));
			}
			string str = "PsychicRitualToil_GatherForInvocation_Report".Translate();
			string str2 = (from pawn in PsychicRitualToil_GatherForInvocation.blockingPawns
			select pawn.LabelShortCap).ToCommaList(false, false);
			return str + ": " + str2;
		}

		// Token: 0x060066CC RID: 26316 RVA: 0x0020841F File Offset: 0x0020661F
		public static PsychicRitualToil_Goto FallbackToil(PsychicRitual psychicRitual, PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		{
			return new PsychicRitualToil_Goto(rolePositions.Slice(rolePositions.Keys.Except(def.InvokerRole)));
		}

		// Token: 0x060066CD RID: 26317 RVA: 0x0020843D File Offset: 0x0020663D
		public static PsychicRitualGraph InvokerToil(PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		{
			return new PsychicRitualGraph(PsychicRitualToil_GatherForInvocation.InvokerGatherPhaseToils(def, rolePositions))
			{
				willAdvancePastLastToil = false
			};
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x00208452 File Offset: 0x00206652
		public static IEnumerable<PsychicRitualToil> InvokerGatherPhaseToils(PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		{
			if (def.RequiredOffering != null)
			{
				yield return new PsychicRitualToil_GatherOfferings(def.InvokerRole, def.RequiredOffering);
			}
			if (def.TargetRole != null)
			{
				yield return new PsychicRitualToil_CarryAndGoto(def.InvokerRole, def.TargetRole, rolePositions);
			}
			yield return new PsychicRitualToil_Goto(rolePositions.Slice(new PsychicRitualRoleDef[]
			{
				def.InvokerRole
			}));
			yield break;
		}

		// Token: 0x04004955 RID: 18773
		protected PsychicRitualToil_Goto fallbackToil;

		// Token: 0x04004956 RID: 18774
		protected PsychicRitualGraph invokerToil;

		// Token: 0x04004957 RID: 18775
		protected PsychicRitualToil_Goto invokerFinalToil;

		// Token: 0x04004958 RID: 18776
		private static List<Pawn> blockingPawns = new List<Pawn>(16);
	}
}
