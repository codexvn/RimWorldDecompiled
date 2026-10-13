using System;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038B6 RID: 14518
	public abstract class SketchResolver
	{
		// Token: 0x060140D4 RID: 82132 RVA: 0x005F4AC4 File Offset: 0x005F2CC4
		public void Resolve(SketchResolveParams parms)
		{
			try
			{
				this.ResolveInt(parms);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception resolving ";
				array[1] = base.GetType().Name;
				array[2] = ": ";
				int num = 3;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				array[4] = "\n\nParms:\n";
				array[5] = parms.ToString();
				Log.Error(string.Concat(array));
			}
		}

		// Token: 0x060140D5 RID: 82133 RVA: 0x005F4B44 File Offset: 0x005F2D44
		public bool CanResolve(SketchResolveParams parms)
		{
			bool result;
			try
			{
				result = this.CanResolveInt(parms);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception test running ";
				array[1] = base.GetType().Name;
				array[2] = ": ";
				int num = 3;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				array[4] = "\n\nParms:\n";
				array[5] = parms.ToString();
				Log.Error(string.Concat(array));
				result = false;
			}
			return result;
		}

		// Token: 0x060140D6 RID: 82134
		protected abstract void ResolveInt(SketchResolveParams parms);

		// Token: 0x060140D7 RID: 82135
		protected abstract bool CanResolveInt(SketchResolveParams parms);
	}
}
