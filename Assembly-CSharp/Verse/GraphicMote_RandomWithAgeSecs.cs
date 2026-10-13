using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000887 RID: 2183
	[StaticConstructorOnStartup]
	public class GraphicMote_RandomWithAgeSecs : Graphic_Random
	{
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x0600376F RID: 14191 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x00128A58 File Offset: 0x00126C58
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mote mote = (Mote)thing;
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote.AgeSecs);
			GraphicMote_RandomWithAgeSecs.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, mote.AgeSecsPausable);
			Graphic_Mote.DrawMote(this.data, this.SubGraphicFor((Mote)thing).MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, GraphicMote_RandomWithAgeSecs.propertyBlock);
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x00128AE0 File Offset: 0x00126CE0
		public Graphic SubGraphicFor(Mote mote)
		{
			return this.subGraphics[mote.offsetRandom % this.subGraphics.Length];
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x00128AF8 File Offset: 0x00126CF8
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

		// Token: 0x04002980 RID: 10624
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
