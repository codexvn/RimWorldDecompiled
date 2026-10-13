using System;

namespace Verse.AI.Group
{
	// Token: 0x0200105F RID: 4191
	public static class PsychicRitualToilRunner
	{
		// Token: 0x0600662F RID: 26159 RVA: 0x00205B30 File Offset: 0x00203D30
		public static void Start(PsychicRitualToil toil, PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			try
			{
				if (toil != null)
				{
					toil.Start(psychicRitual, parent);
				}
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("PsychicRitualToil {0} threw an exception during Start(): {1}", toil.ToStringSafe<PsychicRitualToil>(), arg));
			}
		}

		// Token: 0x06006630 RID: 26160 RVA: 0x00205B74 File Offset: 0x00203D74
		public static bool Tick(PsychicRitualToil toil, PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			try
			{
				return toil == null || toil.Tick(psychicRitual, parent);
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("PsychicRitualToil {0} threw an exception during Tick(): {1}", toil.ToStringSafe<PsychicRitualToil>(), arg));
			}
			return true;
		}

		// Token: 0x06006631 RID: 26161 RVA: 0x00205BC0 File Offset: 0x00203DC0
		public static void UpdateAllDuties(PsychicRitualToil toil, PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			try
			{
				if (toil != null)
				{
					toil.UpdateAllDuties(psychicRitual, parent);
				}
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("PsychicRitualToil {0} threw an exception during UpdateAllDuties(): {1}", toil.ToStringSafe<PsychicRitualToil>(), arg));
			}
		}

		// Token: 0x06006632 RID: 26162 RVA: 0x00205C04 File Offset: 0x00203E04
		public static void End(PsychicRitualToil toil, PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
			try
			{
				if (toil != null)
				{
					toil.End(psychicRitual, parent, success);
				}
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("PsychicRitualToil {0} threw an exception during End(): {1}", toil.ToStringSafe<PsychicRitualToil>(), arg));
			}
		}
	}
}
