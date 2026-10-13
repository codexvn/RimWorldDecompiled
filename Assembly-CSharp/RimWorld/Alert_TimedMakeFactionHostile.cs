using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200300D RID: 12301
	public class Alert_TimedMakeFactionHostile : Alert
	{
		// Token: 0x17002CDE RID: 11486
		// (get) Token: 0x060114FB RID: 70907 RVA: 0x004F67AC File Offset: 0x004F49AC
		private List<GlobalTargetInfo> WorldObjects
		{
			get
			{
				this.worldObjectsResult.Clear();
				foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
				{
					TimedMakeFactionHostile component = worldObject.GetComponent<TimedMakeFactionHostile>();
					if (component != null && component.TicksLeft != null)
					{
						this.worldObjectsResult.Add(worldObject);
					}
				}
				return this.worldObjectsResult;
			}
		}

		// Token: 0x060114FC RID: 70908 RVA: 0x004F683C File Offset: 0x004F4A3C
		public Alert_TimedMakeFactionHostile()
		{
			this.defaultLabel = "FactionWillBecomeHostileIfNotLeavingWithin".Translate();
		}

		// Token: 0x060114FD RID: 70909 RVA: 0x004F6870 File Offset: 0x004F4A70
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (GlobalTargetInfo globalTargetInfo in this.worldObjectsResult)
			{
				this.sb.Append("  - ");
				this.sb.Append(globalTargetInfo.Label);
				this.sb.Append(" (");
				this.sb.Append(globalTargetInfo.WorldObject.GetComponent<TimedMakeFactionHostile>().TicksLeft.Value.ToStringTicksToPeriodVerbose(true, true));
				this.sb.AppendLine(")");
			}
			return "FactionWillBecomeHostileIfNotLeavingWithinDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x060114FE RID: 70910 RVA: 0x004F695C File Offset: 0x004F4B5C
		public override AlertReport GetReport()
		{
			List<GlobalTargetInfo> worldObjects = this.WorldObjects;
			Map currentMap = Find.CurrentMap;
			List<Pawn> culprits;
			if (!WorldRendererUtility.WorldSelected && currentMap != null && worldObjects.Contains(currentMap.Parent) && !(culprits = currentMap.mapPawns.FreeHumanlikesSpawnedOfFaction(currentMap.ParentFaction)).NullOrEmpty<Pawn>())
			{
				return AlertReport.CulpritsAre(culprits);
			}
			if (worldObjects.Count > 0)
			{
				return AlertReport.CulpritsAre(worldObjects);
			}
			return false;
		}

		// Token: 0x0400B369 RID: 45929
		private List<GlobalTargetInfo> worldObjectsResult = new List<GlobalTargetInfo>();

		// Token: 0x0400B36A RID: 45930
		private StringBuilder sb = new StringBuilder();
	}
}
