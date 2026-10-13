using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200224C RID: 8780
	public class CompAbilityEffect_MoteOnTarget : CompAbilityEffect
	{
		// Token: 0x17001D43 RID: 7491
		// (get) Token: 0x0600BFA1 RID: 49057 RVA: 0x00378C08 File Offset: 0x00376E08
		public new CompProperties_AbilityMoteOnTarget Props
		{
			get
			{
				return (CompProperties_AbilityMoteOnTarget)this.props;
			}
		}

		// Token: 0x0600BFA2 RID: 49058 RVA: 0x00378C18 File Offset: 0x00376E18
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

		// Token: 0x0600BFA3 RID: 49059 RVA: 0x00378C72 File Offset: 0x00376E72
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

		// Token: 0x0600BFA4 RID: 49060 RVA: 0x00378C84 File Offset: 0x00376E84
		private void SpawnAll(LocalTargetInfo target)
		{
			if (!this.Props.moteDefs.NullOrEmpty<ThingDef>())
			{
				for (int i = 0; i < this.Props.moteDefs.Count; i++)
				{
					this.SpawnMote(target, this.Props.moteDefs[i]);
				}
				return;
			}
			this.SpawnMote(target, this.Props.moteDef);
		}

		// Token: 0x0600BFA5 RID: 49061 RVA: 0x00378CEC File Offset: 0x00376EEC
		private void SpawnMote(LocalTargetInfo target, ThingDef def)
		{
			if (target.HasThing)
			{
				MoteMaker.MakeAttachedOverlay(target.Thing, def, Vector3.zero, this.Props.scale, -1f);
				return;
			}
			MoteMaker.MakeStaticMote(target.Cell, this.parent.pawn.Map, def, this.Props.scale);
		}
	}
}
