using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200088C RID: 2188
	public class Graphic_ActivityStaged : Graphic_Collection
	{
		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x0600377D RID: 14205 RVA: 0x00128DB6 File Offset: 0x00126FB6
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[0].MatSingle;
			}
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x00128DC5 File Offset: 0x00126FC5
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_ActivityStaged>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x00128DE2 File Offset: 0x00126FE2
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.MatSingleFor(thing);
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x00128DF5 File Offset: 0x00126FF5
		public override Material MatSingleFor(Thing thing)
		{
			if (thing == null)
			{
				return this.MatSingle;
			}
			return this.SubGraphicFor(thing).MatSingle;
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x00128E0D File Offset: 0x0012700D
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			((thing != null) ? this.SubGraphicFor(thing) : this.subGraphics[0]).DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x00128E4B File Offset: 0x0012704B
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			((thing != null) ? this.SubGraphicFor(thing) : this.subGraphics[0]).Print(layer, thing, extraRotation);
			if (base.ShadowGraphic != null && thing != null)
			{
				base.ShadowGraphic.Print(layer, thing, extraRotation);
			}
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x00128E84 File Offset: 0x00127084
		private Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			CompActivity compActivity;
			if (!thing.TryGetComp(out compActivity))
			{
				Log.ErrorOnce(thing.Label + ": Graphic_ActivityStaged requires CompActivity.", 4627811);
				return null;
			}
			int num = Mathf.Min(Mathf.FloorToInt((float)this.subGraphics.Length * compActivity.ActivityLevel), this.subGraphics.Length - 1);
			return this.subGraphics[num];
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x00128EF0 File Offset: 0x001270F0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"ActivityStaged(path=",
				this.path,
				", count=",
				this.subGraphics.Length.ToString(),
				")"
			});
		}
	}
}
