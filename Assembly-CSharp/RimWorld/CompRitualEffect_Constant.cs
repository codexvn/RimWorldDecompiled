using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027ED RID: 10221
	public abstract class CompRitualEffect_Constant : RitualVisualEffectComp
	{
		// Token: 0x0600DFCC RID: 57292 RVA: 0x0041BA4F File Offset: 0x00419C4F
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			base.OnSetup(parent, ritual, loading);
			this.Spawn(ritual);
		}

		// Token: 0x0600DFCD RID: 57293 RVA: 0x0041BA64 File Offset: 0x00419C64
		protected virtual void Spawn(LordJob_Ritual ritual)
		{
			Mote mote = this.SpawnMote(ritual, null);
			if (mote != null)
			{
				this.parent.AddMoteToMaintain(mote);
				if (this.props.colorOverride != null)
				{
					mote.instanceColor = this.props.colorOverride.Value;
				}
				else
				{
					mote.instanceColor = this.parent.def.tintColor;
				}
				this.spawned = true;
			}
		}

		// Token: 0x0600DFCE RID: 57294 RVA: 0x0041BAD8 File Offset: 0x00419CD8
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (!this.spawned)
			{
				this.Spawn(this.parent.ritual);
			}
		}

		// Token: 0x040096A2 RID: 38562
		protected bool spawned;
	}
}
