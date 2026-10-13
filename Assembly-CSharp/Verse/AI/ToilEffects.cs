using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI
{
	// Token: 0x02000EE2 RID: 3810
	public static class ToilEffects
	{
		// Token: 0x06005FBE RID: 24510 RVA: 0x001E6E74 File Offset: 0x001E5074
		public static Toil PlaySoundAtStart(this Toil toil, SoundDef sound)
		{
			toil.AddPreInitAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06005FBF RID: 24511 RVA: 0x001E6EB4 File Offset: 0x001E50B4
		public static Toil PlaySoundAtEnd(this Toil toil, SoundDef sound)
		{
			toil.AddFinishAction(delegate
			{
				sound.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
			});
			return toil;
		}

		// Token: 0x06005FC0 RID: 24512 RVA: 0x001E6EF4 File Offset: 0x001E50F4
		public static Toil PlaySustainerOrSound(this Toil toil, SoundDef soundDef, float pitchFactor = 1f)
		{
			return toil.PlaySustainerOrSound(() => soundDef, pitchFactor);
		}

		// Token: 0x06005FC1 RID: 24513 RVA: 0x001E6F24 File Offset: 0x001E5124
		public static Toil PlaySustainerOrSound(this Toil toil, Func<SoundDef> soundDefGetter, float pitchFactor = 1f)
		{
			Sustainer sustainer = null;
			toil.AddPreInitAction(delegate
			{
				SoundDef soundDef = soundDefGetter();
				if (soundDef != null && !soundDef.sustain)
				{
					soundDef.PlayOneShot(new TargetInfo(toil.GetActor().Position, toil.GetActor().Map, false));
				}
			});
			toil.AddPreTickAction(delegate
			{
				if (sustainer == null || sustainer.Ended)
				{
					SoundDef soundDef = soundDefGetter();
					if (soundDef != null && soundDef.sustain)
					{
						SoundInfo info = SoundInfo.InMap(toil.actor, MaintenanceType.PerTick);
						info.pitchFactor = pitchFactor;
						sustainer = soundDef.TrySpawnSustainer(info);
						return;
					}
				}
				else
				{
					sustainer.Maintain();
				}
			});
			return toil;
		}

		// Token: 0x06005FC2 RID: 24514 RVA: 0x001E6F88 File Offset: 0x001E5188
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, TargetIndex ind, Color? overrideColor = null)
		{
			return toil.WithEffect(() => effectDef, ind, overrideColor);
		}

		// Token: 0x06005FC3 RID: 24515 RVA: 0x001E6FB8 File Offset: 0x001E51B8
		public static Toil WithEffect(this Toil toil, EffecterDef effectDef, Func<LocalTargetInfo> effectTargetGetter, Color? overrideColor = null)
		{
			return toil.WithEffect(() => effectDef, effectTargetGetter, overrideColor);
		}

		// Token: 0x06005FC4 RID: 24516 RVA: 0x001E6FE8 File Offset: 0x001E51E8
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, TargetIndex ind, Color? overrideColor = null)
		{
			return toil.WithEffect(effecterDefGetter, () => toil.actor.CurJob.GetTarget(ind), overrideColor);
		}

		// Token: 0x06005FC5 RID: 24517 RVA: 0x001E7024 File Offset: 0x001E5224
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Thing thing, Color? overrideColor = null)
		{
			return toil.WithEffect(effecterDefGetter, () => thing, overrideColor);
		}

		// Token: 0x06005FC6 RID: 24518 RVA: 0x001E7054 File Offset: 0x001E5254
		public static Toil WithEffect(this Toil toil, Func<EffecterDef> effecterDefGetter, Func<LocalTargetInfo> effectTargetGetter, Color? overrideColor = null)
		{
			Effecter effecter = null;
			toil.AddPreTickAction(delegate
			{
				if (effecter == null)
				{
					EffecterDef effecterDef = effecterDefGetter();
					if (effecterDef == null)
					{
						return;
					}
					toil.actor.rotationTracker.FaceTarget(effectTargetGetter());
					effecter = effecterDef.Spawn();
					effecter.Trigger(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map), -1);
					if (overrideColor == null)
					{
						return;
					}
					using (List<SubEffecter>.Enumerator enumerator = effecter.children.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SubEffecter subEffecter = enumerator.Current;
							SubEffecter_Sprayer subEffecter_Sprayer = subEffecter as SubEffecter_Sprayer;
							if (subEffecter_Sprayer != null)
							{
								subEffecter_Sprayer.colorOverride = overrideColor;
							}
						}
						return;
					}
				}
				effecter.EffectTick(toil.actor, effectTargetGetter().ToTargetInfo(toil.actor.Map));
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		// Token: 0x06005FC7 RID: 24519 RVA: 0x001E70C0 File Offset: 0x001E52C0
		public static Toil WithProgressBar(this Toil toil, TargetIndex ind, Func<float> progressGetter, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f, bool alwaysShow = false)
		{
			Effecter effecter = null;
			toil.AddPreTickIntervalAction(delegate(int delta)
			{
				if (toil.actor.Faction != Faction.OfPlayer)
				{
					return;
				}
				if (effecter == null)
				{
					EffecterDef progressBar = EffecterDefOf.ProgressBar;
					effecter = progressBar.Spawn();
					return;
				}
				LocalTargetInfo localTargetInfo = (ind == TargetIndex.None) ? LocalTargetInfo.Invalid : toil.actor.CurJob.GetTarget(ind);
				if (!localTargetInfo.IsValid || (localTargetInfo.HasThing && !localTargetInfo.Thing.Spawned))
				{
					effecter.EffectTick(toil.actor, TargetInfo.Invalid);
				}
				else if (interpolateBetweenActorAndTarget)
				{
					effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), toil.actor);
				}
				else
				{
					effecter.EffectTick(toil.actor.CurJob.GetTarget(ind).ToTargetInfo(toil.actor.Map), TargetInfo.Invalid);
				}
				MoteProgressBar mote = ((SubEffecter_ProgressBar)effecter.children[0]).mote;
				if (mote != null)
				{
					mote.progress = Mathf.Clamp01(progressGetter());
					mote.offsetZ = offsetZ;
					mote.alwaysShow = alwaysShow;
				}
			});
			toil.AddFinishAction(delegate
			{
				if (effecter != null)
				{
					effecter.Cleanup();
					effecter = null;
				}
			});
			return toil;
		}

		// Token: 0x06005FC8 RID: 24520 RVA: 0x001E713C File Offset: 0x001E533C
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toil.defaultDuration, interpolateBetweenActorAndTarget, offsetZ, false);
		}

		// Token: 0x06005FC9 RID: 24521 RVA: 0x001E7174 File Offset: 0x001E5374
		public static Toil WithProgressBarToilDelay(this Toil toil, TargetIndex ind, int toilDuration, bool interpolateBetweenActorAndTarget = false, float offsetZ = -0.5f)
		{
			return toil.WithProgressBar(ind, () => 1f - (float)toil.actor.jobs.curDriver.ticksLeftThisToil / (float)toilDuration, interpolateBetweenActorAndTarget, offsetZ, false);
		}
	}
}
