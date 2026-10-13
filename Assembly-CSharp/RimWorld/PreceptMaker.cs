using System;

namespace RimWorld
{
	// Token: 0x0200264A RID: 9802
	public static class PreceptMaker
	{
		// Token: 0x0600D6BF RID: 54975 RVA: 0x003FB063 File Offset: 0x003F9263
		public static Precept MakePrecept(PreceptDef def)
		{
			Precept precept = (Precept)Activator.CreateInstance(def.preceptClass);
			precept.def = def;
			precept.PostMake();
			return precept;
		}
	}
}
