using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008AC RID: 2220
	public class Graphic_Random : Graphic_Collection
	{
		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06003829 RID: 14377 RVA: 0x00129230 File Offset: 0x00127430
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x0600382A RID: 14378 RVA: 0x0012A26C File Offset: 0x0012846C
		public int SubGraphicsCount
		{
			get
			{
				return this.subGraphics.Length;
			}
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x0012C03C File Offset: 0x0012A23C
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Random.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Random>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x00128DE2 File Offset: 0x00126FE2
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x0012C079 File Offset: 0x0012A279
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x0012C091 File Offset: 0x0012A291
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			((thing != null) ? this.SubGraphicFor(thing) : this.subGraphics[0]).DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x0012C0D0 File Offset: 0x0012A2D0
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			int num = thing.OverrideGraphicIndex ?? thing.thingIDNumber;
			return this.subGraphics[num % this.subGraphics.Length];
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x0012A351 File Offset: 0x00128551
		public Graphic SubGraphicAtIndex(int index)
		{
			return this.subGraphics[index % this.subGraphics.Length];
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x0012A364 File Offset: 0x00128564
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x0012C11A File Offset: 0x0012A31A
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			((thing != null) ? this.SubGraphicFor(thing) : this.subGraphics[0]).Print(layer, thing, extraRotation);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, extraRotation);
			}
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x0012C154 File Offset: 0x0012A354
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Random(path=",
				this.path,
				", count=",
				this.subGraphics.Length.ToString(),
				")"
			});
		}
	}
}
