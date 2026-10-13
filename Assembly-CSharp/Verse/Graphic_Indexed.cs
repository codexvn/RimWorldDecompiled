using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200089B RID: 2203
	public class Graphic_Indexed : Graphic_Collection
	{
		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x060037CB RID: 14283 RVA: 0x00128DB6 File Offset: 0x00126FB6
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[0].MatSingle;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x060037CC RID: 14284 RVA: 0x0012A26C File Offset: 0x0012846C
		public int SubGraphicsCount
		{
			get
			{
				return this.subGraphics.Length;
			}
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x0012A276 File Offset: 0x00128476
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			if (newColorTwo != Color.white)
			{
				Log.ErrorOnce("Cannot use Graphic_Indexed.GetColoredVersion with a non-white colorTwo.", 9910251);
			}
			return GraphicDatabase.Get<Graphic_Indexed>(this.path, newShader, this.drawSize, newColor, Color.white, this.data, null);
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x00128DE2 File Offset: 0x00126FE2
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x0012A2B3 File Offset: 0x001284B3
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0012A2CC File Offset: 0x001284CC
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic graphic;
			if (thing != null)
			{
				graphic = this.SubGraphicFor(thing);
			}
			else
			{
				graphic = this.subGraphics[0];
			}
			graphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x0012A318 File Offset: 0x00128518
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			int valueOrDefault = thing.OverrideGraphicIndex.GetValueOrDefault();
			return this.subGraphics[valueOrDefault % this.subGraphics.Length];
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x0012A351 File Offset: 0x00128551
		public Graphic SubGraphicAtIndex(int index)
		{
			return this.subGraphics[index % this.subGraphics.Length];
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x0012A364 File Offset: 0x00128564
		public Graphic FirstSubgraphic()
		{
			return this.subGraphics[0];
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x0012A370 File Offset: 0x00128570
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Indexed(path=",
				this.path,
				", count=",
				this.subGraphics.Length.ToString(),
				")"
			});
		}
	}
}
