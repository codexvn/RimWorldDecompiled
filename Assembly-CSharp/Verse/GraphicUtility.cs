using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000888 RID: 2184
	public static class GraphicUtility
	{
		// Token: 0x06003775 RID: 14197 RVA: 0x00128B7C File Offset: 0x00126D7C
		public static Graphic ExtractInnerGraphicFor(this Graphic outerGraphic, Thing thing, int? indexOverride = null)
		{
			GraphicUtility.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.indexOverride = indexOverride;
			CS$<>8__locals1.thing = thing;
			Graphic_RandomRotated graphic_RandomRotated = outerGraphic as Graphic_RandomRotated;
			if (graphic_RandomRotated != null)
			{
				return GraphicUtility.<ExtractInnerGraphicFor>g__ResolveGraphicInner|0_0(graphic_RandomRotated.SubGraphic, ref CS$<>8__locals1);
			}
			return GraphicUtility.<ExtractInnerGraphicFor>g__ResolveGraphicInner|0_0(outerGraphic, ref CS$<>8__locals1);
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x00128BBC File Offset: 0x00126DBC
		public static Graphic_Linked WrapLinked(Graphic subGraphic, LinkDrawerType linkDrawerType)
		{
			switch (linkDrawerType)
			{
			case LinkDrawerType.None:
				return null;
			case LinkDrawerType.Basic:
				return new Graphic_Linked(subGraphic);
			case LinkDrawerType.CornerFiller:
				return new Graphic_LinkedCornerFiller(subGraphic);
			case LinkDrawerType.CornerOverlay:
				return new Graphic_LinkedCornerOverlay(subGraphic);
			case LinkDrawerType.Transmitter:
				return new Graphic_LinkedTransmitter(subGraphic);
			case LinkDrawerType.TransmitterOverlay:
				return new Graphic_LinkedTransmitterOverlay(subGraphic);
			case LinkDrawerType.Asymmetric:
				return new Graphic_LinkedAsymmetric(subGraphic);
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x00128C20 File Offset: 0x00126E20
		[CompilerGenerated]
		internal static Graphic <ExtractInnerGraphicFor>g__ResolveGraphicInner|0_0(Graphic g, ref GraphicUtility.<>c__DisplayClass0_0 A_1)
		{
			Graphic_Random graphic_Random = g as Graphic_Random;
			if (graphic_Random != null)
			{
				if (A_1.indexOverride != null)
				{
					return graphic_Random.SubGraphicAtIndex(A_1.indexOverride.Value);
				}
				return graphic_Random.SubGraphicFor(A_1.thing);
			}
			else
			{
				Graphic_Cluster graphic_Cluster = g as Graphic_Cluster;
				if (graphic_Cluster != null)
				{
					return graphic_Cluster.SubGraphicFor(A_1.thing);
				}
				Graphic_Appearances graphic_Appearances = g as Graphic_Appearances;
				if (graphic_Appearances != null)
				{
					return graphic_Appearances.SubGraphicFor(A_1.thing);
				}
				Graphic_Genepack graphic_Genepack = g as Graphic_Genepack;
				if (graphic_Genepack != null)
				{
					return graphic_Genepack.SubGraphicFor(A_1.thing);
				}
				Graphic_MealVariants graphic_MealVariants = g as Graphic_MealVariants;
				if (graphic_MealVariants != null)
				{
					return graphic_MealVariants.SubGraphicFor(A_1.thing);
				}
				return g;
			}
		}
	}
}
