using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002900 RID: 10496
	public static class AmbientSoundManager
	{
		// Token: 0x170022D3 RID: 8915
		// (get) Token: 0x0600E581 RID: 58753 RVA: 0x0042F604 File Offset: 0x0042D804
		private static bool WorldAmbientSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_Space);
			}
		}

		// Token: 0x170022D4 RID: 8916
		// (get) Token: 0x0600E582 RID: 58754 RVA: 0x0042F61A File Offset: 0x0042D81A
		private static bool AltitudeWindSoundCreated
		{
			get
			{
				return Find.SoundRoot.sustainerManager.SustainerExists(SoundDefOf.Ambient_AltitudeWind);
			}
		}

		// Token: 0x0600E583 RID: 58755 RVA: 0x0042F630 File Offset: 0x0042D830
		public static void EnsureWorldAmbientSoundCreated()
		{
			if (!AmbientSoundManager.WorldAmbientSoundCreated)
			{
				SoundDefOf.Ambient_Space.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		// Token: 0x0600E584 RID: 58756 RVA: 0x0042F64A File Offset: 0x0042D84A
		public static void Notify_SwitchedMap()
		{
			LongEventHandler.ExecuteWhenFinished(new Action(AmbientSoundManager.RecreateMapSustainers));
		}

		// Token: 0x0600E585 RID: 58757 RVA: 0x0042F660 File Offset: 0x0042D860
		private static void RecreateMapSustainers()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			BiomeDef biome = Find.CurrentMap.Biome;
			if (!AmbientSoundManager.AltitudeWindSoundCreated && !biome.noAmbientWind)
			{
				AmbientSoundManager.altitudeWindSustainer = SoundDefOf.Ambient_AltitudeWind.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
			}
			else if (AmbientSoundManager.altitudeWindSustainer != null && biome.noAmbientWind)
			{
				AmbientSoundManager.altitudeWindSustainer.End();
				AmbientSoundManager.altitudeWindSustainer = null;
			}
			SustainerManager sustainerManager = Find.SoundRoot.sustainerManager;
			for (int i = 0; i < AmbientSoundManager.biomeAmbientSustainers.Count; i++)
			{
				Sustainer sustainer = AmbientSoundManager.biomeAmbientSustainers[i];
				if (sustainerManager.AllSustainers.Contains(sustainer) && !sustainer.Ended)
				{
					sustainer.End();
				}
			}
			AmbientSoundManager.biomeAmbientSustainers.Clear();
			if (Find.CurrentMap != null)
			{
				List<SoundDef> soundsAmbient = Find.CurrentMap.Biome.soundsAmbient;
				for (int j = 0; j < soundsAmbient.Count; j++)
				{
					Sustainer item = soundsAmbient[j].TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.None));
					AmbientSoundManager.biomeAmbientSustainers.Add(item);
				}
			}
		}

		// Token: 0x04009989 RID: 39305
		private static readonly List<Sustainer> biomeAmbientSustainers = new List<Sustainer>();

		// Token: 0x0400998A RID: 39306
		private static Sustainer altitudeWindSustainer;
	}
}
