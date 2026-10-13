using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200291F RID: 10527
	public static class TaleFactory
	{
		// Token: 0x0600E64C RID: 58956 RVA: 0x00432C8C File Offset: 0x00430E8C
		public static Tale MakeRawTale(TaleDef def, params object[] args)
		{
			Tale result;
			try
			{
				Tale tale = (Tale)Activator.CreateInstance(def.taleClass, args);
				tale.def = def;
				tale.id = Find.UniqueIDsManager.GetNextTaleID();
				tale.date = Find.TickManager.TicksAbs;
				result = tale;
			}
			catch (Exception arg)
			{
				Exception arg2;
				Log.Error(string.Format("Failed to create tale object {0} with parameters {1}: {2}", def, (from arg in args
				select arg.ToStringSafe<object>()).ToCommaList(false, false), arg2));
				result = null;
			}
			return result;
		}

		// Token: 0x0600E64D RID: 58957 RVA: 0x00432D28 File Offset: 0x00430F28
		public static Tale MakeRandomTestTale(TaleDef def = null)
		{
			if (def == null)
			{
				def = (from d in DefDatabase<TaleDef>.AllDefs
				where d.usableForArt
				select d).RandomElement<TaleDef>();
			}
			Tale tale = TaleFactory.MakeRawTale(def, Array.Empty<object>());
			tale.GenerateTestData();
			return tale;
		}
	}
}
