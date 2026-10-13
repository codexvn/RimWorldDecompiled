using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003038 RID: 12344
	public class Alert_RolesEmpty : Alert
	{
		// Token: 0x060115A1 RID: 71073 RVA: 0x004FA34E File Offset: 0x004F854E
		public Alert_RolesEmpty()
		{
			this.requireIdeology = true;
		}

		// Token: 0x060115A2 RID: 71074 RVA: 0x004FA380 File Offset: 0x004F8580
		private void GetTargets()
		{
			this.emptyRoles.Clear();
			this.targets.Clear();
			this.targetLabels.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					if (!precept.def.leaderRole && precept.def.createsRoleEmptyThought)
					{
						Precept_Role precept_Role = precept as Precept_Role;
						if (precept_Role != null && precept_Role.ChosenPawnSingle() == null && precept_Role.Active)
						{
							this.emptyRoles.Add(precept_Role);
						}
					}
				}
			}
			if (this.emptyRoles.Any<Precept_Role>())
			{
				using (List<Pawn>.Enumerator enumerator3 = Find.CurrentMap.mapPawns.FreeColonistsSpawned.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Pawn p = enumerator3.Current;
						if (!p.IsSlave && this.emptyRoles.Any((Precept_Role x) => x.ideo == p.Ideo))
						{
							this.targets.Add(p);
							this.targetLabels.Add(p.NameFullColored.Resolve());
						}
					}
				}
			}
		}

		// Token: 0x060115A3 RID: 71075 RVA: 0x004FA52C File Offset: 0x004F872C
		public override string GetLabel()
		{
			if (this.emptyRoles.Count == 1)
			{
				return "IdeoRoleEmpty".Translate(this.emptyRoles[0].LabelCap);
			}
			return "IdeoRolesEmpty".Translate(this.emptyRoles.Count);
		}

		// Token: 0x060115A4 RID: 71076 RVA: 0x004FA58C File Offset: 0x004F878C
		public override TaggedString GetExplanation()
		{
			return "IdeoRolesEmptyDesc".Translate((from x in this.emptyRoles
			select x.LabelCap.ApplyTag(x.ideo).Resolve()).ToLineList("  - ", false)) + ":\n" + this.targetLabels.ToLineList("  - ");
		}

		// Token: 0x060115A5 RID: 71077 RVA: 0x004FA5FC File Offset: 0x004F87FC
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 10)
			{
				return false;
			}
			if (Find.CurrentMap == null)
			{
				return false;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3B1 RID: 46001
		private List<Precept_Role> emptyRoles = new List<Precept_Role>();

		// Token: 0x0400B3B2 RID: 46002
		private List<string> targetLabels = new List<string>();

		// Token: 0x0400B3B3 RID: 46003
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B3B4 RID: 46004
		public const int DayEnable = 10;
	}
}
