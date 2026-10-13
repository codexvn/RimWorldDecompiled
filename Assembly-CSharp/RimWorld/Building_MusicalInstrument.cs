using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A37 RID: 10807
	public class Building_MusicalInstrument : Building
	{
		// Token: 0x0600EFD4 RID: 61396 RVA: 0x00459B47 File Offset: 0x00457D47
		public static bool IsAffectedByInstrument(ThingDef instrumentDef, IntVec3 instrumentPos, IntVec3 pawnPos, Map map)
		{
			return instrumentPos.DistanceTo(pawnPos) < instrumentDef.building.instrumentRange && instrumentPos.GetRoom(map) == pawnPos.GetRoom(map);
		}

		// Token: 0x17002550 RID: 9552
		// (get) Token: 0x0600EFD5 RID: 61397 RVA: 0x00459B6F File Offset: 0x00457D6F
		public bool IsBeingPlayed
		{
			get
			{
				return this.currentPlayer != null;
			}
		}

		// Token: 0x17002551 RID: 9553
		// (get) Token: 0x0600EFD6 RID: 61398 RVA: 0x00459B7C File Offset: 0x00457D7C
		public FloatRange SoundRange
		{
			get
			{
				if (this.soundPlaying == null)
				{
					return FloatRange.Zero;
				}
				if (this.soundPlaying.def.subSounds.NullOrEmpty<SubSoundDef>())
				{
					return FloatRange.Zero;
				}
				return this.soundPlaying.def.subSounds.First<SubSoundDef>().distRange;
			}
		}

		// Token: 0x0600EFD7 RID: 61399 RVA: 0x00459BCE File Offset: 0x00457DCE
		public void StartPlaying(Pawn player)
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Musical instrument"))
			{
				return;
			}
			this.currentPlayer = player;
		}

		// Token: 0x0600EFD8 RID: 61400 RVA: 0x00459BE4 File Offset: 0x00457DE4
		protected override void Tick()
		{
			base.Tick();
			if (this.currentPlayer != null)
			{
				if (this.def.soundPlayInstrument != null && this.soundPlaying == null)
				{
					this.soundPlaying = this.def.soundPlayInstrument.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.PerTick));
				}
			}
			else
			{
				this.soundPlaying = null;
			}
			Sustainer sustainer = this.soundPlaying;
			if (sustainer == null)
			{
				return;
			}
			sustainer.Maintain();
		}

		// Token: 0x0600EFD9 RID: 61401 RVA: 0x00459C5B File Offset: 0x00457E5B
		public void StopPlaying()
		{
			this.currentPlayer = null;
		}

		// Token: 0x0600EFDA RID: 61402 RVA: 0x00459C64 File Offset: 0x00457E64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.currentPlayer, "currentPlayer", false);
		}

		// Token: 0x0600EFDB RID: 61403 RVA: 0x00459C7D File Offset: 0x00457E7D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Musical instrument"))
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Toggle is playing",
					action = delegate()
					{
						this.currentPlayer = ((this.currentPlayer == null) ? PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>() : null);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04009E6F RID: 40559
		private Pawn currentPlayer;

		// Token: 0x04009E70 RID: 40560
		private Sustainer soundPlaying;
	}
}
