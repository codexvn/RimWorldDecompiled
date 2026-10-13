using System;
using System.Collections.Generic;
using RimWorld;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000D33 RID: 3379
	public static class LifeStageUtility
	{
		// Token: 0x06005611 RID: 22033 RVA: 0x001BC844 File Offset: 0x001BAA44
		public static void PlayNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> lifestageGetter, Func<GeneDef, SoundDef> geneGetter, Func<MutantDef, SoundDef> mutantGetter, float volumeFactor = 1f)
		{
			SoundDef soundDef;
			float voxPitch;
			float voxVolume;
			LifeStageUtility.GetNearestLifestageSound(pawn, lifestageGetter, out soundDef, out voxPitch, out voxVolume);
			if (ModsConfig.BiotechActive && pawn.genes != null && geneGetter != null)
			{
				soundDef = pawn.genes.GetSoundOverrideFromGenes(geneGetter, soundDef);
				voxPitch = pawn.ageTracker.CurLifeStage.voxPitch;
				voxVolume = pawn.ageTracker.CurLifeStage.voxVolume;
			}
			if (pawn.IsMutant && mutantGetter != null)
			{
				soundDef = mutantGetter(pawn.mutant.Def);
				voxPitch = pawn.ageTracker.CurLifeStage.voxPitch;
				voxVolume = pawn.ageTracker.CurLifeStage.voxVolume;
			}
			if (soundDef == null)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None);
			info.pitchFactor = voxPitch;
			info.volumeFactor = voxVolume * volumeFactor;
			soundDef.PlayOneShot(info);
		}

		// Token: 0x06005612 RID: 22034 RVA: 0x001BC920 File Offset: 0x001BAB20
		private static void GetNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, out SoundDef def, out float pitch, out float volume)
		{
			int num = pawn.ageTracker.CurLifeStageIndex;
			LifeStageAge lifeStageAge;
			for (;;)
			{
				lifeStageAge = pawn.RaceProps.lifeStageAges[num];
				def = getter(lifeStageAge);
				if (def != null)
				{
					break;
				}
				num++;
				if (num < 0 || num >= pawn.RaceProps.lifeStageAges.Count)
				{
					goto IL_84;
				}
			}
			pitch = pawn.ageTracker.CurLifeStage.voxPitch / lifeStageAge.def.voxPitch;
			volume = pawn.ageTracker.CurLifeStage.voxVolume / lifeStageAge.def.voxVolume;
			return;
			IL_84:
			def = null;
			pitch = (volume = 1f);
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x001BC9C4 File Offset: 0x001BABC4
		private static LifeStageAge GetLifeStageAgeForYears(Pawn pawn, float years)
		{
			List<LifeStageAge> lifeStageAges = pawn.RaceProps.lifeStageAges;
			int num = 0;
			int num2 = 1;
			while (num2 < lifeStageAges.Count && lifeStageAges[num2].minAge < years)
			{
				num++;
				num2++;
			}
			return lifeStageAges[num];
		}

		// Token: 0x06005614 RID: 22036 RVA: 0x001BCA0A File Offset: 0x001BAC0A
		public static DevelopmentalStage CalculateDevelopmentalStage(Pawn pawn, float years)
		{
			return LifeStageUtility.GetLifeStageAgeForYears(pawn, years).def.developmentalStage;
		}

		// Token: 0x06005615 RID: 22037 RVA: 0x001BCA20 File Offset: 0x001BAC20
		public static float GetMaxBabyAge(RaceProperties raceProps)
		{
			foreach (LifeStageAge lifeStageAge in raceProps.lifeStageAges)
			{
				if (!lifeStageAge.def.developmentalStage.Baby())
				{
					return lifeStageAge.minAge;
				}
			}
			return 0f;
		}

		// Token: 0x06005616 RID: 22038 RVA: 0x001BCA90 File Offset: 0x001BAC90
		public static bool AlwaysDowned(Pawn pawn)
		{
			Pawn_AgeTracker ageTracker = pawn.ageTracker;
			bool? flag;
			if (ageTracker == null)
			{
				flag = null;
			}
			else
			{
				LifeStageDef curLifeStage = ageTracker.CurLifeStage;
				flag = ((curLifeStage != null) ? new bool?(curLifeStage.alwaysDowned) : null);
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}
	}
}
