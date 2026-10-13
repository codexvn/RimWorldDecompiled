using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005DC RID: 1500
	internal struct PawnStatusEffecters
	{
		// Token: 0x06002675 RID: 9845 RVA: 0x000CB844 File Offset: 0x000C9A44
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x06002676 RID: 9846 RVA: 0x000CB858 File Offset: 0x000C9A58
		public void EffectersTick(bool suspended)
		{
			if (!suspended)
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					HediffComp_Effecter hediffComp_Effecter = hediffs[i].TryGetComp<HediffComp_Effecter>();
					if (hediffComp_Effecter != null)
					{
						EffecterDef effecterDef = hediffComp_Effecter.CurrentStateEffecter();
						if (effecterDef != null)
						{
							this.AddOrMaintain(effecterDef);
						}
					}
				}
				Pawn_MindState mindState = this.pawn.mindState;
				if (((mindState != null) ? mindState.mentalStateHandler.CurState : null) != null)
				{
					EffecterDef effecterDef2 = this.pawn.mindState.mentalStateHandler.CurState.CurrentStateEffecter();
					if (effecterDef2 != null)
					{
						this.AddOrMaintain(effecterDef2);
					}
				}
			}
			for (int j = this.pairs.Count - 1; j >= 0; j--)
			{
				if (this.pairs[j].Expired)
				{
					this.pairs[j].Cleanup();
					this.pairs.RemoveAt(j);
				}
				else
				{
					this.pairs[j].Tick(this.pawn, 1);
				}
			}
		}

		// Token: 0x06002677 RID: 9847 RVA: 0x000CB964 File Offset: 0x000C9B64
		private void AddOrMaintain(EffecterDef def)
		{
			for (int i = 0; i < this.pairs.Count; i++)
			{
				if (this.pairs[i].def == def)
				{
					this.pairs[i].Maintain();
					return;
				}
			}
			PawnStatusEffecters.LiveEffecter liveEffecter = FullPool<PawnStatusEffecters.LiveEffecter>.Get();
			liveEffecter.def = def;
			liveEffecter.Maintain();
			this.pairs.Add(liveEffecter);
		}

		// Token: 0x04001FE1 RID: 8161
		public Pawn pawn;

		// Token: 0x04001FE2 RID: 8162
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x020005DD RID: 1501
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x170006D0 RID: 1744
			// (get) Token: 0x06002678 RID: 9848 RVA: 0x000CB9CC File Offset: 0x000C9BCC
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x0600267A RID: 9850 RVA: 0x000CB9E0 File Offset: 0x000C9BE0
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x0600267B RID: 9851 RVA: 0x000CB9FB File Offset: 0x000C9BFB
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x0600267C RID: 9852 RVA: 0x000CBA12 File Offset: 0x000C9C12
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x0600267D RID: 9853 RVA: 0x000CBA24 File Offset: 0x000C9C24
			public void Tick(Pawn pawn, int delta)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.SpawnAttached(pawn, pawn.MapHeld, 1f);
				}
				this.effecter.EffectTick(pawn, pawn);
			}

			// Token: 0x04001FE3 RID: 8163
			public EffecterDef def;

			// Token: 0x04001FE4 RID: 8164
			public Effecter effecter;

			// Token: 0x04001FE5 RID: 8165
			public int lastMaintainTick;
		}
	}
}
