using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002CEA RID: 11498
	public class CompHunterDrone : ThingComp
	{
		// Token: 0x170028EC RID: 10476
		// (get) Token: 0x06010275 RID: 66165 RVA: 0x004AC8A6 File Offset: 0x004AAAA6
		private CompProperties_HunterDrone Props
		{
			get
			{
				return (CompProperties_HunterDrone)this.props;
			}
		}

		// Token: 0x06010276 RID: 66166 RVA: 0x004AC8B3 File Offset: 0x004AAAB3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.wickStarted, "wickStarted", false, false);
			Scribe_Values.Look<int>(ref this.wickTicks, "wickTicks", 0, false);
		}

		// Token: 0x06010277 RID: 66167 RVA: 0x004AC8E0 File Offset: 0x004AAAE0
		public override void CompTickInterval(int delta)
		{
			if (this.wickStarted)
			{
				return;
			}
			if (!this.parent.IsHashIntervalTick(30, delta))
			{
				return;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn == null)
			{
				return;
			}
			if (pawn.Spawned && !pawn.Downed && PawnUtility.EnemiesAreNearby(pawn, 9, true, 1.5f, 1, false))
			{
				this.StartWick();
			}
		}

		// Token: 0x06010278 RID: 66168 RVA: 0x004AC93F File Offset: 0x004AAB3F
		public override void CompTick()
		{
			if (!this.wickStarted)
			{
				return;
			}
			this.wickSoundSustainer.Maintain();
			this.wickTicks--;
			if (this.wickTicks <= 0)
			{
				this.Detonate(null);
			}
		}

		// Token: 0x06010279 RID: 66169 RVA: 0x004AC974 File Offset: 0x004AAB74
		private void StartWick()
		{
			if (this.wickStarted)
			{
				return;
			}
			this.wickStarted = true;
			this.overlayBurningWick = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.BurningWick));
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
			this.wickTicks = 120;
		}

		// Token: 0x0601027A RID: 66170 RVA: 0x004AC9E1 File Offset: 0x004AABE1
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			if (dinfo == null)
			{
				return;
			}
			this.Detonate(prevMap);
		}

		// Token: 0x0601027B RID: 66171 RVA: 0x004AC9F4 File Offset: 0x004AABF4
		private void Detonate(Map map = null)
		{
			IntVec3 position = this.parent.Position;
			if (map == null)
			{
				map = this.parent.Map;
			}
			if (!this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
			GenExplosion.DoExplosion(position, map, this.Props.explosionRadius, this.Props.explosionDamageType, this.parent, this.Props.explosionDamageAmount, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			Corpse corpse = base.ParentHolder as Corpse;
			if (corpse != null)
			{
				corpse.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0400A8C0 RID: 43200
		private bool wickStarted;

		// Token: 0x0400A8C1 RID: 43201
		private int wickTicks;

		// Token: 0x0400A8C2 RID: 43202
		[Unsaved(false)]
		private Sustainer wickSoundSustainer;

		// Token: 0x0400A8C3 RID: 43203
		[Unsaved(false)]
		private OverlayHandle? overlayBurningWick;
	}
}
