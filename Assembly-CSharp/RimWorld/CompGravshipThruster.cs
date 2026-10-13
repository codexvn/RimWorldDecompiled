using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EEE RID: 12014
	public class CompGravshipThruster : CompGravshipFacility
	{
		// Token: 0x17002BCC RID: 11212
		// (get) Token: 0x06010F5B RID: 69467 RVA: 0x004E1240 File Offset: 0x004DF440
		public CompBreakdownable Breakdownable
		{
			get
			{
				CompBreakdownable result;
				if ((result = this.breakdownableComp) == null)
				{
					result = (this.breakdownableComp = this.parent.GetComp<CompBreakdownable>());
				}
				return result;
			}
		}

		// Token: 0x17002BCD RID: 11213
		// (get) Token: 0x06010F5C RID: 69468 RVA: 0x004E126B File Offset: 0x004DF46B
		public new CompProperties_GravshipThruster Props
		{
			get
			{
				return (CompProperties_GravshipThruster)this.props;
			}
		}

		// Token: 0x17002BCE RID: 11214
		// (get) Token: 0x06010F5D RID: 69469 RVA: 0x004E1278 File Offset: 0x004DF478
		public bool Blocked
		{
			get
			{
				if (this.blockedBy == null && !this.blockedBySubstructure)
				{
					bool? flag = this.outdoors;
					bool flag2 = false;
					return flag.GetValueOrDefault() == flag2 & flag != null;
				}
				return true;
			}
		}

		// Token: 0x17002BCF RID: 11215
		// (get) Token: 0x06010F5E RID: 69470 RVA: 0x004E12B4 File Offset: 0x004DF4B4
		public override bool CanBeActive
		{
			get
			{
				if (!base.CanBeActive)
				{
					return false;
				}
				if (this.Blocked)
				{
					return false;
				}
				if (this.Breakdownable.BrokenDown)
				{
					return false;
				}
				if (this.outdoors == null)
				{
					this.RecalculateOutdoors();
				}
				if (base.LinkedBuildings.NullOrEmpty<Thing>())
				{
					return false;
				}
				bool? flag = this.outdoors;
				bool flag2 = true;
				return flag.GetValueOrDefault() == flag2 & flag != null;
			}
		}

		// Token: 0x06010F5F RID: 69471 RVA: 0x004E1321 File Offset: 0x004DF521
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.RecalculateBlocked();
			this.RecalculateOutdoors();
		}

		// Token: 0x06010F60 RID: 69472 RVA: 0x004E1336 File Offset: 0x004DF536
		public override void PostMapInit()
		{
			this.RecalculateBlocked();
			this.RecalculateOutdoors();
			base.PostMapInit();
		}

		// Token: 0x06010F61 RID: 69473 RVA: 0x004E134A File Offset: 0x004DF54A
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecalculateBlocked();
			this.RecalculateOutdoors();
		}

		// Token: 0x06010F62 RID: 69474 RVA: 0x004E1360 File Offset: 0x004DF560
		public override bool CanLink()
		{
			if (this.Blocked)
			{
				return false;
			}
			if (this.outdoors == null)
			{
				this.RecalculateOutdoors();
			}
			bool? flag = this.outdoors;
			bool flag2 = true;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}

		// Token: 0x06010F63 RID: 69475 RVA: 0x004E13A8 File Offset: 0x004DF5A8
		private void RecalculateBlocked()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.lastCheckedBlockedTick = Find.TickManager.TicksGame;
			bool flag = this.blockedBy != null || this.blockedBySubstructure;
			CompGravshipThruster.IsBlocked(this.parent.def, this.parent.Map, this.parent.Position, this.parent.Rotation, out this.blockedBy, out this.blockedBySubstructure);
			if (flag != (this.blockedBy != null || this.blockedBySubstructure))
			{
				base.Notify_ThingChanged();
			}
		}

		// Token: 0x06010F64 RID: 69476 RVA: 0x004E143C File Offset: 0x004DF63C
		private void RecalculateOutdoors()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			bool valueOrDefault = this.outdoors.GetValueOrDefault();
			this.outdoors = new bool?(CompGravshipThruster.IsOutdoors(this.parent.def, this.parent.Map, this.parent.Position, this.parent.Rotation));
			bool? flag = this.outdoors;
			bool flag2 = valueOrDefault;
			if (!(flag.GetValueOrDefault() == flag2 & flag != null))
			{
				base.Notify_ThingChanged();
			}
		}

		// Token: 0x06010F65 RID: 69477 RVA: 0x004E14C0 File Offset: 0x004DF6C0
		public static bool IsBlocked(ThingDef thrusterDef, Map map, IntVec3 position, Rot4 rotation, out Thing blockedBy, out bool blockedBySubstructure)
		{
			blockedBy = null;
			blockedBySubstructure = false;
			thrusterDef.GetCompProperties<CompProperties_GravshipThruster>().GetExclusionZone(position, rotation, ref CompGravshipThruster.tmpExclusionCells);
			for (int i = 0; i < CompGravshipThruster.tmpExclusionCells.Count; i++)
			{
				IntVec3 c = CompGravshipThruster.tmpExclusionCells[i];
				if (c.InBounds(map))
				{
					TerrainDef terrainDef = map.terrainGrid.FoundationAt(c);
					if (terrainDef != null && terrainDef.IsSubstructure)
					{
						blockedBySubstructure = true;
					}
					foreach (Thing thing in map.thingGrid.ThingsListAt(c))
					{
						if (thing.def.blockWind && !(thing is Plant))
						{
							blockedBy = thing;
							return true;
						}
					}
				}
			}
			return blockedBySubstructure;
		}

		// Token: 0x06010F66 RID: 69478 RVA: 0x004E15A0 File Offset: 0x004DF7A0
		public static bool IsOutdoors(ThingDef thrusterDef, Map map, IntVec3 position, Rot4 rotation)
		{
			if (!map.regionAndRoomUpdater.Enabled)
			{
				return false;
			}
			foreach (IntVec3 intVec in GenAdj.CellsAdjacentAlongEdge(position, rotation, thrusterDef.size, rotation.Opposite.AsInt))
			{
				if (intVec.InBounds(map))
				{
					Room room = intVec.GetRoom(map);
					if (room != null && !room.UsesOutdoorTemperature)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06010F67 RID: 69479 RVA: 0x004E1630 File Offset: 0x004DF830
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Props.showMaxSimultaneous)
			{
				stringBuilder.Append(this.MaxConnectedString);
				stringBuilder.Append(": " + this.Props.maxSimultaneous.ToString());
			}
			string text = "";
			if (this.parent.Spawned && Find.TickManager.TicksGame != this.lastCheckedBlockedTick)
			{
				this.RecalculateBlocked();
				this.RecalculateOutdoors();
			}
			if (this.blockedBy != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "ThrusterBlockedBy".Translate(this.blockedBy);
			}
			else if (this.blockedBySubstructure)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "BlockedBySubstructure".Translate();
			}
			bool? flag = this.outdoors;
			bool flag2 = false;
			if (flag.GetValueOrDefault() == flag2 & flag != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "MustBeOutside".Translate();
			}
			if (text.NullOrEmpty() && this.parent.Spawned && (base.LinkedBuildings.NullOrEmpty<Thing>() || !this.CanBeActive))
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "ThrusterNotConnected".Translate();
			}
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLineIfNotEmpty().Append(("ThrusterNotFunctional".Translate() + ": " + text.CapitalizeFirst()).Colorize(ColorLibrary.RedReadable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400B06A RID: 45162
		[Unsaved(false)]
		private Thing blockedBy;

		// Token: 0x0400B06B RID: 45163
		[Unsaved(false)]
		private bool blockedBySubstructure;

		// Token: 0x0400B06C RID: 45164
		[Unsaved(false)]
		private bool? outdoors;

		// Token: 0x0400B06D RID: 45165
		[Unsaved(false)]
		private int lastCheckedBlockedTick = -99999;

		// Token: 0x0400B06E RID: 45166
		[Unsaved(false)]
		private CompBreakdownable breakdownableComp;

		// Token: 0x0400B06F RID: 45167
		private static List<IntVec3> tmpExclusionCells = new List<IntVec3>();
	}
}
