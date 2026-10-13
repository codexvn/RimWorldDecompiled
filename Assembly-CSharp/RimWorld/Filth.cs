using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AD6 RID: 10966
	public class Filth : Thing
	{
		// Token: 0x17002630 RID: 9776
		// (get) Token: 0x0600F45B RID: 62555 RVA: 0x0047016F File Offset: 0x0046E36F
		public bool CanFilthAttachNow
		{
			get
			{
				return this.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		// Token: 0x17002631 RID: 9777
		// (get) Token: 0x0600F45C RID: 62556 RVA: 0x004701A6 File Offset: 0x0046E3A6
		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		// Token: 0x17002632 RID: 9778
		// (get) Token: 0x0600F45D RID: 62557 RVA: 0x004701B1 File Offset: 0x0046E3B1
		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		// Token: 0x17002633 RID: 9779
		// (get) Token: 0x0600F45E RID: 62558 RVA: 0x004701C4 File Offset: 0x0046E3C4
		public int DisappearAfterTicks
		{
			get
			{
				return this.disappearAfterTicks;
			}
		}

		// Token: 0x17002634 RID: 9780
		// (get) Token: 0x0600F45F RID: 62559 RVA: 0x004701CC File Offset: 0x0046E3CC
		public override string Label
		{
			get
			{
				if (this.sources.NullOrEmpty<string>())
				{
					return "FilthLabel".Translate(base.Label, this.thickness.ToString());
				}
				return "FilthLabelWithSource".Translate(base.Label, this.sources.ToCommaList(true, false), this.thickness.ToString());
			}
		}

		// Token: 0x0600F460 RID: 62560 RVA: 0x00470250 File Offset: 0x0046E450
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.growTick, "growTick", 0, false);
			Scribe_Values.Look<int>(ref this.thickness, "thickness", 1, false);
			Scribe_Values.Look<int>(ref this.disappearAfterTicks, "disappearAfterTicks", 0, false);
			if (Scribe.mode != LoadSaveMode.Saving || this.drawInstances != null)
			{
				Scribe_Collections.Look<Filth.FilthInstance>(ref this.drawInstances, "drawInstances", LookMode.Deep, Array.Empty<object>());
			}
			if (Scribe.mode != LoadSaveMode.Saving || this.sources != null)
			{
				Scribe_Collections.Look<string>(ref this.sources, "sources", LookMode.Value, Array.Empty<object>());
			}
		}

		// Token: 0x0600F461 RID: 62561 RVA: 0x004702E8 File Offset: 0x0046E4E8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing)
			{
				base.Map.listerFilthInHomeArea.Notify_FilthSpawned(this);
			}
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.growTick = Find.TickManager.TicksGame;
				this.disappearAfterTicks = (int)(this.def.filth.disappearsInDays.RandomInRange * 60000f);
			}
		}

		// Token: 0x0600F462 RID: 62562 RVA: 0x00470354 File Offset: 0x0046E554
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		// Token: 0x0600F463 RID: 62563 RVA: 0x00470384 File Offset: 0x0046E584
		public void AddSource(string newSource)
		{
			if (this.sources == null)
			{
				this.sources = new List<string>();
			}
			for (int i = 0; i < this.sources.Count; i++)
			{
				if (this.sources[i] == newSource)
				{
					return;
				}
			}
			while (this.sources.Count > 3)
			{
				this.sources.RemoveAt(0);
			}
			this.sources.Add(newSource);
		}

		// Token: 0x0600F464 RID: 62564 RVA: 0x004703F8 File Offset: 0x0046E5F8
		public void AddSources(IEnumerable<string> sources)
		{
			if (sources == null)
			{
				return;
			}
			foreach (string newSource in sources)
			{
				this.AddSource(newSource);
			}
		}

		// Token: 0x0600F465 RID: 62565 RVA: 0x00470444 File Offset: 0x0046E644
		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < this.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

		// Token: 0x0600F466 RID: 62566 RVA: 0x00470482 File Offset: 0x0046E682
		public void ThinFilth()
		{
			this.thickness--;
			if (base.Spawned)
			{
				if (this.thickness == 0)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				this.UpdateMesh();
			}
		}

		// Token: 0x0600F467 RID: 62567 RVA: 0x004704B0 File Offset: 0x0046E6B0
		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600F468 RID: 62568 RVA: 0x004704DC File Offset: 0x0046E6DC
		public void SetOverrideDrawPositionAndRotation(Vector3 drawPos, float rotation)
		{
			if (this.drawInstances == null)
			{
				this.drawInstances = new List<Filth.FilthInstance>();
			}
			this.drawInstances.Add(new Filth.FilthInstance
			{
				drawPos = drawPos,
				rotation = rotation
			});
			this.UpdateMesh();
		}

		// Token: 0x0600F469 RID: 62569 RVA: 0x00470526 File Offset: 0x0046E726
		public bool CanDropAt(IntVec3 c, Map map, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.CanMakeFilth(c, map, this.def, additionalFlags);
		}

		// Token: 0x0400A10D RID: 41229
		public int thickness = 1;

		// Token: 0x0400A10E RID: 41230
		public List<string> sources;

		// Token: 0x0400A10F RID: 41231
		private int growTick;

		// Token: 0x0400A110 RID: 41232
		private int disappearAfterTicks;

		// Token: 0x0400A111 RID: 41233
		public List<Filth.FilthInstance> drawInstances;

		// Token: 0x0400A112 RID: 41234
		private const int MaxThickness = 5;

		// Token: 0x0400A113 RID: 41235
		private const int MinAgeToPickUp = 400;

		// Token: 0x0400A114 RID: 41236
		private const int MaxNumSources = 3;

		// Token: 0x02002AD7 RID: 10967
		public struct FilthInstance : IExposable
		{
			// Token: 0x0600F46B RID: 62571 RVA: 0x00470548 File Offset: 0x0046E748
			public void ExposeData()
			{
				Scribe_Values.Look<float>(ref this.rotation, "rotation", 0f, false);
				Scribe_Values.Look<Vector3>(ref this.drawPos, "drawPos", default(Vector3), false);
			}

			// Token: 0x0400A115 RID: 41237
			public float rotation;

			// Token: 0x0400A116 RID: 41238
			public Vector3 drawPos;
		}
	}
}
