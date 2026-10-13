using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000491 RID: 1169
	public class SectionLayer_ThingsGeneral : SectionLayer_Things
	{
		// Token: 0x06001E8C RID: 7820 RVA: 0x000A0644 File Offset: 0x0009E844
		public SectionLayer_ThingsGeneral(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Things;
			this.requireAddToMapMesh = true;
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x000A0664 File Offset: 0x0009E864
		protected override void TakePrintFrom(Thing t)
		{
			try
			{
				t.Print(this);
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception printing ";
				array[1] = ((t != null) ? t.ToString() : null);
				array[2] = " at ";
				array[3] = t.Position.ToString();
				array[4] = ": ";
				int num = 5;
				Exception ex2 = ex;
				array[num] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
			}
		}
	}
}
