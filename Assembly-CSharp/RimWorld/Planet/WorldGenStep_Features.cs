using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CF1 RID: 15601
	public class WorldGenStep_Features : WorldGenStep
	{
		// Token: 0x170034EB RID: 13547
		// (get) Token: 0x060153B0 RID: 86960 RVA: 0x00659D9E File Offset: 0x00657F9E
		public override int SeedPart
		{
			get
			{
				return 26611132;
			}
		}

		// Token: 0x060153B1 RID: 86961 RVA: 0x00659DA8 File Offset: 0x00657FA8
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			foreach (FeatureDef featureDef in from x in DefDatabase<FeatureDef>.AllDefsListForReading
			orderby x.order, x.index
			select x)
			{
				try
				{
					featureDef.Worker.GenerateWhereAppropriate(layer);
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Could not generate world features of def {0}: {1}", featureDef, arg));
				}
			}
		}
	}
}
