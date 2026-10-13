using System;
using LudeonTK;
using RimWorld.Planet;

namespace Verse.Sound
{
	// Token: 0x02000E3D RID: 3645
	public static class SoundStarter
	{
		// Token: 0x06005BF0 RID: 23536 RVA: 0x001D62B0 File Offset: 0x001D44B0
		public static void PlayOneShotOnCamera(this SoundDef soundDef, Map onlyThisMap = null)
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (onlyThisMap != null && (Find.CurrentMap != onlyThisMap || !WorldRendererUtility.DrawingMap))
			{
				return;
			}
			if (soundDef == null)
			{
				return;
			}
			if (soundDef.subSounds.Count > 0)
			{
				bool flag = false;
				for (int i = 0; i < soundDef.subSounds.Count; i++)
				{
					if (soundDef.subSounds[i].onCamera)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Log.Error("Tried to play " + ((soundDef != null) ? soundDef.ToString() : null) + " on camera but it has no on-camera subSounds.");
				}
			}
			soundDef.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
		}

		// Token: 0x06005BF1 RID: 23537 RVA: 0x001D634C File Offset: 0x001D454C
		public static void PlayOneShot(this SoundDef soundDef, SoundInfo info)
		{
			if (!UnityData.IsInMainThread)
			{
				return;
			}
			if (soundDef == null)
			{
				string str = "Tried to PlayOneShot with null SoundDef. Info=";
				SoundInfo soundInfo = info;
				Log.Error(str + soundInfo.ToString());
				return;
			}
			DebugSoundEventsLog.Notify_SoundEvent(soundDef, info);
			if (soundDef.isUndefined)
			{
				if (Prefs.DevMode && Find.WindowStack.IsOpen(typeof(EditWindow_DefEditor)))
				{
					DefDatabase<SoundDef>.Clear();
					DefDatabase<SoundDef>.AddAllInMods();
					SoundDef soundDef2 = SoundDef.Named(soundDef.defName);
					if (!soundDef2.isUndefined)
					{
						soundDef2.PlayOneShot(info);
					}
				}
				return;
			}
			if (soundDef.sustain)
			{
				Log.Error("Tried to play sustainer SoundDef " + ((soundDef != null) ? soundDef.ToString() : null) + " as a one-shot sound.");
				return;
			}
			if (!SoundSlotManager.CanPlayNow(soundDef.slot))
			{
				return;
			}
			for (int i = 0; i < soundDef.subSounds.Count; i++)
			{
				soundDef.subSounds[i].TryPlay(info);
			}
		}

		// Token: 0x06005BF2 RID: 23538 RVA: 0x001D6438 File Offset: 0x001D4638
		public static Sustainer TrySpawnSustainer(this SoundDef soundDef, SoundInfo info)
		{
			DebugSoundEventsLog.Notify_SoundEvent(soundDef, info);
			if (soundDef == null)
			{
				return null;
			}
			if (soundDef.isUndefined)
			{
				if (Prefs.DevMode && Find.WindowStack.IsOpen(typeof(EditWindow_DefEditor)))
				{
					DefDatabase<SoundDef>.Clear();
					DefDatabase<SoundDef>.AddAllInMods();
					SoundDef soundDef2 = SoundDef.Named(soundDef.defName);
					if (!soundDef2.isUndefined)
					{
						return soundDef2.TrySpawnSustainer(info);
					}
				}
				return null;
			}
			if (!soundDef.sustain)
			{
				Log.Error("Tried to spawn a sustainer from non-sustainer sound " + ((soundDef != null) ? soundDef.ToString() : null) + ".");
				return null;
			}
			if (!info.IsOnCamera && info.Maker.Thing != null && info.Maker.Thing.Destroyed)
			{
				return null;
			}
			if (soundDef.sustainStartSound != null)
			{
				soundDef.sustainStartSound.PlayOneShot(info);
			}
			return new Sustainer(soundDef, info);
		}
	}
}
