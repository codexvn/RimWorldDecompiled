using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200302E RID: 12334
	public class Alert_AnimalPenNeeded : Alert
	{
		// Token: 0x06011576 RID: 71030 RVA: 0x004F9500 File Offset: 0x004F7700
		public Alert_AnimalPenNeeded()
		{
			this.defaultLabel = "AlertAnimalPenNeeded".Translate();
		}

		// Token: 0x06011577 RID: 71031 RVA: 0x004F9534 File Offset: 0x004F7734
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.anyHandlers = false;
			foreach (Map map in Find.Maps)
			{
				if (map.IsPlayerHome)
				{
					Pawn pawn = null;
					int num = -1;
					foreach (Pawn pawn2 in map.mapPawns.FreeColonistsSpawned)
					{
						if (pawn2.workSettings.WorkIsActive(WorkTypeDefOf.Handling))
						{
							int level = pawn2.skills.GetSkill(SkillDefOf.Animals).Level;
							if (level > num)
							{
								pawn = pawn2;
								num = level;
							}
						}
					}
					foreach (Pawn pawn3 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
					{
						if (AnimalPenUtility.NeedsToBeManagedByRope(pawn3) && !pawn3.InAggroMentalState && AnimalPenUtility.GetCurrentPenOf(pawn3, true) == null && pawn3.CanReachMapEdge())
						{
							bool flag = !AnimalPenUtility.AnySuitablePens(pawn3, true) && !AnimalPenUtility.AnySuitableHitch(pawn3);
							if (pawn == null)
							{
								flag = true;
							}
							else
							{
								this.anyHandlers = true;
							}
							if (flag)
							{
								this.targets.Add(pawn3);
							}
						}
					}
				}
			}
		}

		// Token: 0x06011578 RID: 71032 RVA: 0x004F96EC File Offset: 0x004F78EC
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			this.sb.AppendLine("AlertAnimalPenNeededDesc".Translate());
			if (!this.anyHandlers)
			{
				this.sb.AppendLine();
				this.sb.AppendLine("AlertAnimalPenNeededNoHandlers".Translate());
			}
			this.sb.AppendLine();
			this.sb.Append("AlertAnimalPenNeededDescExplanation".Translate());
			return this.sb.ToString();
		}

		// Token: 0x06011579 RID: 71033 RVA: 0x004F9786 File Offset: 0x004F7986
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B39C RID: 45980
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B39D RID: 45981
		private bool anyHandlers;

		// Token: 0x0400B39E RID: 45982
		private StringBuilder sb = new StringBuilder();
	}
}
