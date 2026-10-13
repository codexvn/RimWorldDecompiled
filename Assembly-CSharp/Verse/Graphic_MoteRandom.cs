using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008A4 RID: 2212
	[StaticConstructorOnStartup]
	public class Graphic_MoteRandom : Graphic_Random
	{
		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x0012B0D4 File Offset: 0x001292D4
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic_Mote.DrawMote(this.data, this.SubGraphicFor((Mote)thing).MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, null);
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x00128AE0 File Offset: 0x00126CE0
		public Graphic SubGraphicFor(Mote mote)
		{
			return this.subGraphics[mote.offsetRandom % this.subGraphics.Length];
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x0012B114 File Offset: 0x00129314
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Mote(path=";
			array[1] = this.path;
			array[2] = ", shader=";
			int num = 3;
			Shader shader = base.Shader;
			array[num] = ((shader != null) ? shader.ToString() : null);
			array[4] = ", color=";
			int num2 = 5;
			Color color = this.color;
			array[num2] = color.ToString();
			array[6] = ", colorTwo=unsupported)";
			return string.Concat(array);
		}

		// Token: 0x040029AA RID: 10666
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
