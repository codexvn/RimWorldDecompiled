using System;

namespace Verse
{
	// Token: 0x02000525 RID: 1317
	public static class MapExposeUtility
	{
		// Token: 0x06002254 RID: 8788 RVA: 0x000B2CFC File Offset: 0x000B0EFC
		public static void ExposeUshort(Map map, Func<IntVec3, ushort> shortReader, Action<IntVec3, ushort> shortWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeUshort(map, shortReader);
			}
			DataExposeUtility.LookByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadUshort(arr, map, shortWriter);
			}
		}

		// Token: 0x06002255 RID: 8789 RVA: 0x000B2D34 File Offset: 0x000B0F34
		public static void ExposeInt(Map map, Func<IntVec3, int> intReader, Action<IntVec3, int> intWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeInt(map, intReader);
			}
			DataExposeUtility.LookByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadInt(arr, map, intWriter);
			}
		}

		// Token: 0x06002256 RID: 8790 RVA: 0x000B2D6C File Offset: 0x000B0F6C
		public static void ExposeUint(Map map, Func<IntVec3, uint> uintReader, Action<IntVec3, uint> uintWriter, string label)
		{
			byte[] arr = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				arr = MapSerializeUtility.SerializeUint(map, uintReader);
			}
			DataExposeUtility.LookByteArray(ref arr, label);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				MapSerializeUtility.LoadUint(arr, map, uintWriter);
			}
		}
	}
}
