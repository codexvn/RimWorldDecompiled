using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F51 RID: 12113
	public abstract class CompUseEffect : ThingComp
	{
		// Token: 0x17002C2B RID: 11307
		// (get) Token: 0x06011143 RID: 69955 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float OrderPriority
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17002C2C RID: 11308
		// (get) Token: 0x06011144 RID: 69956 RVA: 0x004E7B17 File Offset: 0x004E5D17
		private CompProperties_UseEffect Props
		{
			get
			{
				return (CompProperties_UseEffect)this.props;
			}
		}

		// Token: 0x06011145 RID: 69957 RVA: 0x004E7B24 File Offset: 0x004E5D24
		public virtual void DoEffect(Pawn usedBy)
		{
			if (usedBy.Map == Find.CurrentMap)
			{
				if (this.Props.doCameraShake && usedBy.Spawned)
				{
					Find.CameraDriver.shaker.DoShake(1f);
				}
				if (this.Props.moteOnUsed != null)
				{
					MoteMaker.MakeAttachedOverlay(usedBy, this.Props.moteOnUsed, Vector3.zero, this.Props.moteOnUsedScale, -1f);
				}
				if (this.Props.fleckOnUsed != null)
				{
					FleckMaker.AttachedOverlay(usedBy, this.Props.fleckOnUsed, Vector3.zero, this.Props.fleckOnUsedScale, -1f);
				}
				if (this.Props.effecterOnUsed != null)
				{
					this.Props.effecterOnUsed.SpawnMaintained(usedBy, new TargetInfo(this.parent.Position, this.parent.Map, false), 1f);
				}
				Effecter effecter = this.effecter;
				if (effecter == null)
				{
					return;
				}
				effecter.Cleanup();
			}
		}

		// Token: 0x06011146 RID: 69958 RVA: 0x004E7C28 File Offset: 0x004E5E28
		public virtual void PrepareTick()
		{
			if (this.Props.warmupEffecter != null)
			{
				if (this.effecter == null)
				{
					this.effecter = this.Props.warmupEffecter.Spawn(this.parent, this.parent.Map, 1f);
				}
				Effecter effecter = this.effecter;
				if (effecter == null)
				{
					return;
				}
				effecter.EffectTick(this.parent, this.parent);
			}
		}

		// Token: 0x06011147 RID: 69959 RVA: 0x00048A14 File Offset: 0x00046C14
		public virtual TaggedString ConfirmMessage(Pawn p)
		{
			return null;
		}

		// Token: 0x06011148 RID: 69960 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool SelectedUseOption(Pawn p)
		{
			return false;
		}

		// Token: 0x06011149 RID: 69961 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport CanBeUsedBy(Pawn p)
		{
			return true;
		}

		// Token: 0x0400B157 RID: 45399
		private const float CameraShakeMag = 1f;

		// Token: 0x0400B158 RID: 45400
		private Effecter effecter;
	}
}
