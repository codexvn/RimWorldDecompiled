using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EEA RID: 12010
	public class CompGravshipFacility : CompFacility
	{
		// Token: 0x17002BC5 RID: 11205
		// (get) Token: 0x06010F42 RID: 69442 RVA: 0x004E0B37 File Offset: 0x004DED37
		public new CompProperties_GravshipFacility Props
		{
			get
			{
				return (CompProperties_GravshipFacility)this.props;
			}
		}

		// Token: 0x17002BC6 RID: 11206
		// (get) Token: 0x06010F43 RID: 69443 RVA: 0x004E0B44 File Offset: 0x004DED44
		protected override string MaxConnectedString
		{
			get
			{
				return "GravFacilityMaxSimultaneousConnections".Translate();
			}
		}

		// Token: 0x17002BC7 RID: 11207
		// (get) Token: 0x06010F44 RID: 69444 RVA: 0x004E0B58 File Offset: 0x004DED58
		public override bool CanBeActive
		{
			get
			{
				if (!ModsConfig.OdysseyActive)
				{
					return false;
				}
				if (!base.CanBeActive)
				{
					return false;
				}
				if (Find.TickManager.TicksGame != this.lastGotEnginesTick)
				{
					this.tmpEngines.Clear();
					this.tmpEngines.AddRange(this.parent.Map.listerBuildings.AllBuildingsColonistOfClass<Building_GravEngine>());
					this.lastGotEnginesTick = Find.TickManager.TicksGame;
				}
				foreach (Building_GravEngine building_GravEngine in this.tmpEngines)
				{
					if (!this.parent.Position.InHorDistOf(building_GravEngine.Position, this.Props.maxDistance))
					{
						return false;
					}
					if (building_GravEngine.LooselyConnectedToGravEngine(this.parent))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06010F45 RID: 69445 RVA: 0x004E0C44 File Offset: 0x004DEE44
		public override void PostSpawnSetup(bool respawningAfterReload)
		{
			base.OnLinkAdded += this.Notify_LinkAdded;
			base.OnLinkRemoved += this.Notify_LinkRemoved;
			base.PostSpawnSetup(respawningAfterReload);
		}

		// Token: 0x06010F46 RID: 69446 RVA: 0x004E0C73 File Offset: 0x004DEE73
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			base.OnLinkAdded -= this.Notify_LinkAdded;
			base.OnLinkRemoved -= this.Notify_LinkRemoved;
		}

		// Token: 0x06010F47 RID: 69447 RVA: 0x004E0CA4 File Offset: 0x004DEEA4
		protected virtual void Notify_LinkAdded(CompFacility facility, Thing thing)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Building_GravEngine building_GravEngine = thing as Building_GravEngine;
			if (building_GravEngine != null && this.engine == null)
			{
				this.engine = building_GravEngine;
				this.engine.AddComponent(this);
			}
		}

		// Token: 0x06010F48 RID: 69448 RVA: 0x004E0CE0 File Offset: 0x004DEEE0
		protected virtual void Notify_LinkRemoved(CompFacility facility, Thing thing)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			Building_GravEngine building_GravEngine = thing as Building_GravEngine;
			if (building_GravEngine != null && building_GravEngine == this.engine)
			{
				this.engine.RemoveComponent(this);
				this.engine = null;
			}
		}

		// Token: 0x06010F49 RID: 69449 RVA: 0x004E0D1C File Offset: 0x004DEF1C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder(base.CompInspectStringExtra());
			if (!this.parent.Spawned)
			{
				return stringBuilder.ToString();
			}
			if (this.parent.Spawned && (base.LinkedBuildings.NullOrEmpty<Thing>() || !this.CanBeActive))
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("NotConnectedToGravEngine".Translate().Colorize(ColorLibrary.RedReadable));
			}
			bool flag = false;
			foreach (Building_GravEngine building_GravEngine in this.parent.Map.listerBuildings.AllBuildingsColonistOfClass<Building_GravEngine>())
			{
				if (this.parent.Position.InHorDistOf(building_GravEngine.Position, this.Props.maxDistance))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				stringBuilder.AppendLineIfNotEmpty().Append("MessageMustBePlacedInRangeOfGravEngine".Translate().Colorize(ColorLibrary.RedReadable));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400B04C RID: 45132
		[Unsaved(false)]
		public Building_GravEngine engine;

		// Token: 0x0400B04D RID: 45133
		private List<Building_GravEngine> tmpEngines = new List<Building_GravEngine>();

		// Token: 0x0400B04E RID: 45134
		private int lastGotEnginesTick = -1;
	}
}
