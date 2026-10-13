using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002232 RID: 8754
	public class CompAbilityEffect_FleckOnTarget : CompAbilityEffect
	{
		// Token: 0x17001D34 RID: 7476
		// (get) Token: 0x0600BF4C RID: 48972 RVA: 0x00377AFF File Offset: 0x00375CFF
		public new CompProperties_AbilityFleckOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityFleckOnTarget)this.props;
			}
		}

		// Token: 0x0600BF4D RID: 48973 RVA: 0x00377B0C File Offset: 0x00375D0C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.preCastTicks <= 0)
			{
				SoundDef sound = this.Props.sound;
				if (sound != null)
				{
					sound.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
				}
				this.SpawnAll(target);
			}
		}

		// Token: 0x0600BF4E RID: 48974 RVA: 0x00377B66 File Offset: 0x00375D66
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			if (this.Props.preCastTicks > 0)
			{
				yield return new PreCastAction
				{
					action = delegate(LocalTargetInfo t, LocalTargetInfo d)
					{
						this.SpawnAll(t);
						SoundDef sound = this.Props.sound;
						if (sound == null)
						{
							return;
						}
						sound.PlayOneShot(new TargetInfo(t.Cell, this.parent.pawn.Map, false));
					},
					ticksAwayFromCast = this.Props.preCastTicks
				};
			}
			yield break;
		}

		// Token: 0x0600BF4F RID: 48975 RVA: 0x00377B78 File Offset: 0x00375D78
		private void SpawnAll(LocalTargetInfo target)
		{
			if (!this.Props.fleckDefs.NullOrEmpty<FleckDef>())
			{
				for (int i = 0; i < this.Props.fleckDefs.Count; i++)
				{
					this.SpawnFleck(target, this.Props.fleckDefs[i]);
				}
				return;
			}
			this.SpawnFleck(target, this.Props.fleckDef);
		}

		// Token: 0x0600BF50 RID: 48976 RVA: 0x00377BE0 File Offset: 0x00375DE0
		private void SpawnFleck(LocalTargetInfo target, FleckDef def)
		{
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned)
			{
				FleckMaker.AttachedOverlay(target.Thing, def, Vector3.zero, this.Props.scale, -1f);
				return;
			}
			FleckMaker.Static(target.Cell, this.parent.pawn.Map, def, this.Props.scale);
		}
	}
}
