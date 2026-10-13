using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200089C RID: 2204
	public class Graphic_Indexed_SquashNStretch : Graphic_Indexed
	{
		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x060037D6 RID: 14294 RVA: 0x0012A3BC File Offset: 0x001285BC
		protected override Type SingleGraphicType
		{
			get
			{
				return typeof(Graphic_Single_SquashNStretch);
			}
		}

		// Token: 0x060037D7 RID: 14295 RVA: 0x0012A3C8 File Offset: 0x001285C8
		public float AgeSecs(Thing thing)
		{
			return (float)(Find.TickManager.TicksGame - thing.TickSpawned) / 60f;
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x0012A3E4 File Offset: 0x001285E4
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
			this.snsProps = new Vector4(this.data.maxSnS.x, this.data.maxSnS.y, this.data.offsetSnS.x, this.data.offsetSnS.y);
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x0012A444 File Offset: 0x00128644
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic_Single_SquashNStretch graphic_Single_SquashNStretch;
			if (thing != null)
			{
				graphic_Single_SquashNStretch = (base.SubGraphicFor(thing) as Graphic_Single_SquashNStretch);
			}
			else
			{
				graphic_Single_SquashNStretch = (this.subGraphics[0] as Graphic_Single_SquashNStretch);
			}
			graphic_Single_SquashNStretch.propertyBlock.SetVector(ShaderPropertyIDs.SquashNStretch, this.snsProps);
			graphic_Single_SquashNStretch.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, (float)thing.thingIDNumber.HashOffset());
			graphic_Single_SquashNStretch.MatSingle.SetFloat(ShaderPropertyIDs.AgeSecs, this.AgeSecs(thing));
			graphic_Single_SquashNStretch.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			if (base.ShadowGraphic != null)
			{
				base.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x0400299B RID: 10651
		private Vector4 snsProps;
	}
}
