using System;

namespace Verse
{
	// Token: 0x0200072B RID: 1835
	public static class HediffMaker
	{
		// Token: 0x06002C9F RID: 11423 RVA: 0x000E6F1C File Offset: 0x000E511C
		public static Hediff MakeHediff(HediffDef def, Pawn pawn, BodyPartRecord partRecord = null)
		{
			if (pawn == null)
			{
				Log.Error("Cannot make hediff " + ((def != null) ? def.ToString() : null) + " for null pawn.");
				return null;
			}
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.pawn = pawn;
			hediff.Part = partRecord;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000E6F8A File Offset: 0x000E518A
		public static Hediff Debug_MakeConcreteExampleHediff(HediffDef def)
		{
			Hediff hediff = (Hediff)Activator.CreateInstance(def.hediffClass);
			hediff.def = def;
			hediff.loadID = Find.UniqueIDsManager.GetNextHediffID();
			hediff.PostMake();
			return hediff;
		}
	}
}
