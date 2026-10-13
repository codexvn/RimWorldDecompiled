using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003667 RID: 13927
	public static class ImpactSoundUtility
	{
		// Token: 0x06013567 RID: 79207 RVA: 0x005C0C3C File Offset: 0x005BEE3C
		public static void PlayImpactSound(Thing hitThing, ImpactSoundTypeDef ist, Map map)
		{
			if (ist == null)
			{
				return;
			}
			if (ist.playOnlyIfHitPawn && !(hitThing is Pawn))
			{
				return;
			}
			if (map == null)
			{
				Log.Warning("Can't play impact sound because map is null.");
				return;
			}
			SoundDef soundDef;
			if (hitThing is Pawn)
			{
				soundDef = ist.soundDef;
			}
			else
			{
				if (hitThing.Stuff != null)
				{
					if (ist == ImpactSoundTypeDefOf.Bullet)
					{
						soundDef = hitThing.Stuff.stuffProps.soundImpactBullet;
					}
					else
					{
						soundDef = hitThing.Stuff.stuffProps.soundImpactMelee;
					}
				}
				else
				{
					soundDef = hitThing.def.soundImpactDefault;
				}
				if (soundDef.NullOrUndefined())
				{
					soundDef = SoundDefOf.Pawn_Melee_Punch_HitBuilding_Generic;
				}
			}
			if (!soundDef.NullOrUndefined())
			{
				soundDef.PlayOneShot(new TargetInfo(hitThing.PositionHeld, map, false));
			}
		}
	}
}
