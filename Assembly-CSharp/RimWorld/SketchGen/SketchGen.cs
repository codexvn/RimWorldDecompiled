using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200389C RID: 14492
	public static class SketchGen
	{
		// Token: 0x0601405E RID: 82014 RVA: 0x005F24C8 File Offset: 0x005F06C8
		public static Sketch Generate(SketchResolverDef root, SketchResolveParams parms)
		{
			if (SketchGen.working)
			{
				Log.Error("Cannot call Generate() while already generating. Nested calls are not allowed.");
				return parms.sketch;
			}
			SketchGen.working = true;
			Sketch sketch;
			try
			{
				root.Resolve(parms);
				sketch = parms.sketch;
			}
			catch (Exception ex)
			{
				string str = "Error in SketchGen: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				sketch = parms.sketch;
			}
			finally
			{
				SketchGen.working = false;
			}
			return sketch;
		}

		// Token: 0x0400DD2E RID: 56622
		private static bool working;
	}
}
