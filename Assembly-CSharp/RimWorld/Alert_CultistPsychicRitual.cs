using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002FCE RID: 12238
	public class Alert_CultistPsychicRitual : Alert_Critical
	{
		// Token: 0x17002CBB RID: 11451
		// (get) Token: 0x060113F6 RID: 70646 RVA: 0x004F127C File Offset: 0x004EF47C
		private PsychicRitual CurPsychicRitual
		{
			get
			{
				LordToil_PsychicRitual lordToil_PsychicRitual = this.ritualLord.CurLordToil as LordToil_PsychicRitual;
				if (lordToil_PsychicRitual != null)
				{
					return lordToil_PsychicRitual.RitualData.psychicRitual;
				}
				return null;
			}
		}

		// Token: 0x17002CBC RID: 11452
		// (get) Token: 0x060113F7 RID: 70647 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool DoMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060113F8 RID: 70648 RVA: 0x004F12AA File Offset: 0x004EF4AA
		public Alert_CultistPsychicRitual()
		{
			this.defaultLabel = "Alert_CultistPsychicRitual".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x060113F9 RID: 70649 RVA: 0x004F12CE File Offset: 0x004EF4CE
		public override string GetLabel()
		{
			return this.defaultLabel + ": " + this.invokeToil.TicksLeft.ToStringTicksToPeriodVerbose(true, true).CapitalizeFirst();
		}

		// Token: 0x060113FA RID: 70650 RVA: 0x004F12F8 File Offset: 0x004EF4F8
		public override TaggedString GetExplanation()
		{
			Pawn pawn = this.invoker.Pawn;
			if (pawn == null)
			{
				return TaggedString.Empty;
			}
			return "Alert_CultistPsychicRitualDesc".Translate(this.CurPsychicRitual.def.Named("RITUAL"), pawn.Named("INVOKER")).CapitalizeFirst();
		}

		// Token: 0x060113FB RID: 70651 RVA: 0x004F134C File Offset: 0x004EF54C
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Lord> lords = maps[i].lordManager.lords;
				for (int j = 0; j < lords.Count; j++)
				{
					Lord lord = lords[j];
					if (lord.faction == Faction.OfHoraxCult)
					{
						LordToil_PsychicRitual lordToil_PsychicRitual = lord.CurLordToil as LordToil_PsychicRitual;
						if (lordToil_PsychicRitual != null && !lordToil_PsychicRitual.RitualData.done)
						{
							PsychicRitualToil_InvokeHorax psychicRitualToil_InvokeHorax = lordToil_PsychicRitual.RitualData.CurPsychicRitualToil as PsychicRitualToil_InvokeHorax;
							if (psychicRitualToil_InvokeHorax != null && psychicRitualToil_InvokeHorax.invokerRole != null)
							{
								PsychicRitualRoleAssignments assignments = lordToil_PsychicRitual.RitualData.psychicRitual.assignments;
								Pawn pawn = (assignments != null) ? assignments.FirstAssignedPawn(psychicRitualToil_InvokeHorax.invokerRole) : null;
								if (pawn != null)
								{
									this.ritualLord = lord;
									this.invokeToil = psychicRitualToil_InvokeHorax;
									this.invoker = pawn;
									return AlertReport.CulpritIs(this.invoker);
								}
							}
						}
					}
				}
			}
			this.ritualLord = null;
			this.invokeToil = null;
			return false;
		}

		// Token: 0x0400B2F9 RID: 45817
		private Lord ritualLord;

		// Token: 0x0400B2FA RID: 45818
		private PsychicRitualToil_InvokeHorax invokeToil;

		// Token: 0x0400B2FB RID: 45819
		private GlobalTargetInfo invoker;
	}
}
