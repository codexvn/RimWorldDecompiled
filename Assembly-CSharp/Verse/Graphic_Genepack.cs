using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200089A RID: 2202
	public class Graphic_Genepack : Graphic_Collection
	{
		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x060037C4 RID: 14276 RVA: 0x00128DB6 File Offset: 0x00126FB6
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[0].MatSingle;
			}
		}

		// Token: 0x060037C5 RID: 14277 RVA: 0x0012A178 File Offset: 0x00128378
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Genepack>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x060037C6 RID: 14278 RVA: 0x0012A198 File Offset: 0x00128398
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing != null)
			{
				Genepack genepack = thing as Genepack;
				if (genepack != null && genepack.GeneSet != null)
				{
					using (List<GeneDef>.Enumerator enumerator = genepack.GeneSet.GenesListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.biostatArc > 0)
							{
								return this.subGraphics[this.subGraphics.Length - 1];
							}
						}
					}
					return this.SubGraphicForGeneCount(genepack.GeneSet.GenesListForReading.Count);
				}
			}
			return this.subGraphics[0];
		}

		// Token: 0x060037C7 RID: 14279 RVA: 0x00128DE2 File Offset: 0x00126FE2
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x0012A23C File Offset: 0x0012843C
		public override Material MatSingleFor(Thing thing)
		{
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x0012A24A File Offset: 0x0012844A
		public Graphic SubGraphicForGeneCount(int geneCount)
		{
			geneCount = Mathf.Min(geneCount, 4);
			return this.subGraphics[Mathf.Min(geneCount, this.subGraphics.Length - 1)];
		}

		// Token: 0x0400299A RID: 10650
		private const int MaxDisplayedGenes = 4;
	}
}
