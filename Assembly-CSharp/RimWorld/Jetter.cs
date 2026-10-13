using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002B09 RID: 11017
	public class Jetter : Thing
	{
		// Token: 0x0600F605 RID: 62981 RVA: 0x00476EF0 File Offset: 0x004750F0
		protected override void Tick()
		{
			if (this.JState == Jetter.JetterState.WickBurning)
			{
				base.Map.overlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
				this.WickTicksLeft--;
				if (this.WickTicksLeft == 0)
				{
					this.StartJetting();
					return;
				}
			}
			else if (this.JState == Jetter.JetterState.Jetting)
			{
				this.TicksUntilMove--;
				if (this.TicksUntilMove <= 0)
				{
					this.MoveJetter();
					this.TicksUntilMove = 3;
				}
			}
		}

		// Token: 0x0600F606 RID: 62982 RVA: 0x00476F62 File Offset: 0x00475162
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (!base.Destroyed && dinfo.Def.harmsHealth && this.JState == Jetter.JetterState.Resting)
			{
				this.StartWick();
			}
		}

		// Token: 0x0600F607 RID: 62983 RVA: 0x00476F90 File Offset: 0x00475190
		protected void StartWick()
		{
			this.JState = Jetter.JetterState.WickBurning;
			this.WickTicksLeft = 25;
			SoundDefOf.MetalHitImportant.PlayOneShot(this);
			this.wickSoundSustainer = SoundDefOf.HissSmall.TrySpawnSustainer(this);
		}

		// Token: 0x0600F608 RID: 62984 RVA: 0x00476FC7 File Offset: 0x004751C7
		protected void StartJetting()
		{
			this.JState = Jetter.JetterState.Jetting;
			this.TicksUntilMove = 3;
			this.wickSoundSustainer.End();
			this.wickSoundSustainer = null;
			this.wickSoundSustainer = SoundDefOf.HissJet.TrySpawnSustainer(this);
		}

		// Token: 0x0600F609 RID: 62985 RVA: 0x00477000 File Offset: 0x00475200
		protected void MoveJetter()
		{
			IntVec3 intVec = base.Position + base.Rotation.FacingCell;
			if (!intVec.Walkable(base.Map) || base.Map.thingGrid.CellContains(intVec, ThingCategory.Pawn) || intVec.GetEdifice(base.Map) != null)
			{
				this.Destroy(DestroyMode.Vanish);
				GenExplosion.DoExplosion(base.Position, base.Map, 2.9f, DamageDefOf.Bomb, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				return;
			}
			base.Position = intVec;
		}

		// Token: 0x0600F60A RID: 62986 RVA: 0x004770E5 File Offset: 0x004752E5
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			if (this.wickSoundSustainer != null)
			{
				this.wickSoundSustainer.End();
				this.wickSoundSustainer = null;
			}
			if (this.jetSoundSustainer != null)
			{
				this.jetSoundSustainer.End();
				this.jetSoundSustainer = null;
			}
		}

		// Token: 0x0400A1EB RID: 41451
		private Jetter.JetterState JState;

		// Token: 0x0400A1EC RID: 41452
		private int WickTicksLeft;

		// Token: 0x0400A1ED RID: 41453
		private int TicksUntilMove;

		// Token: 0x0400A1EE RID: 41454
		protected Sustainer wickSoundSustainer;

		// Token: 0x0400A1EF RID: 41455
		protected Sustainer jetSoundSustainer;

		// Token: 0x0400A1F0 RID: 41456
		private const int TicksBeforeBeginAccelerate = 25;

		// Token: 0x0400A1F1 RID: 41457
		private const int TicksBetweenMoves = 3;

		// Token: 0x02002B0A RID: 11018
		private enum JetterState
		{
			// Token: 0x0400A1F3 RID: 41459
			Resting,
			// Token: 0x0400A1F4 RID: 41460
			WickBurning,
			// Token: 0x0400A1F5 RID: 41461
			Jetting
		}
	}
}
