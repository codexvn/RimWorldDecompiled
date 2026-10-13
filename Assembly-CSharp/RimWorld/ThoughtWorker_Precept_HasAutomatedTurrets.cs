using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EA RID: 6122
	public class ThoughtWorker_Precept_HasAutomatedTurrets : ThoughtWorker_Precept
	{
		// Token: 0x06008E52 RID: 36434 RVA: 0x002A2B7C File Offset: 0x002A0D7C
		public static void ResetStaticData()
		{
			ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Clear();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].building != null && allDefsListForReading[i].building.IsTurret && !allDefsListForReading[i].HasComp(typeof(CompMannable)))
				{
					ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Add(allDefsListForReading[i]);
				}
			}
		}

		// Token: 0x06008E53 RID: 36435 RVA: 0x002A2BF4 File Offset: 0x002A0DF4
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.Faction == null || p.IsSlave)
			{
				return false;
			}
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				for (int j = 0; j < ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs.Count; j++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThoughtWorker_Precept_HasAutomatedTurrets.automatedTurretDefs[j]);
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].Faction == p.Faction)
						{
							return true;
						}
					}
				}
				if (ModsConfig.BiotechActive)
				{
					using (List<Pawn>.Enumerator enumerator = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.IsColonyMechPlayerControlled)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x04005BFA RID: 23546
		private static List<ThingDef> automatedTurretDefs = new List<ThingDef>();
	}
}
