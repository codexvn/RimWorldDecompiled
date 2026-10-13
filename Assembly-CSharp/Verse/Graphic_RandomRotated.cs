using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008AD RID: 2221
	public class Graphic_RandomRotated : Graphic
	{
		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06003835 RID: 14389 RVA: 0x0012C1A0 File Offset: 0x0012A3A0
		public override Material MatSingle
		{
			get
			{
				return this.subGraphic.MatSingle;
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x0012C1AD File Offset: 0x0012A3AD
		public Graphic SubGraphic
		{
			get
			{
				return this.subGraphic;
			}
		}

		// Token: 0x06003837 RID: 14391 RVA: 0x0012C1B5 File Offset: 0x0012A3B5
		public Graphic_RandomRotated(Graphic subGraphic, float maxAngle)
		{
			this.subGraphic = subGraphic;
			this.maxAngle = maxAngle;
			this.drawSize = subGraphic.drawSize;
		}

		// Token: 0x06003838 RID: 14392 RVA: 0x0012C1D7 File Offset: 0x0012A3D7
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.subGraphic.MatAt(rot, thing);
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x0012C1E6 File Offset: 0x0012A3E6
		public override Material MatSingleFor(Thing thing)
		{
			return this.subGraphic.MatSingleFor(thing);
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x0012C1F4 File Offset: 0x0012A3F4
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			float num = 0f;
			float? rotInRack = this.GetRotInRack(thing, thingDef, loc.ToIntVec3());
			if (rotInRack != null)
			{
				num = rotInRack.Value;
			}
			else if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			Graphics.DrawMesh(mesh, loc, Quaternion.AngleAxis(num, Vector3.up), this.MatSingleFor(thing), 0, null, 0);
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x0012C27A File Offset: 0x0012A47A
		public override string ToString()
		{
			string str = "RandomRotated(subGraphic=";
			Graphic graphic = this.subGraphic;
			return str + ((graphic != null) ? graphic.ToString() : null) + ")";
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x0012C29D File Offset: 0x0012A49D
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_RandomRotated(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo), this.maxAngle)
			{
				data = this.data,
				drawSize = this.drawSize
			};
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x0012C2D0 File Offset: 0x0012A4D0
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			float num = 0f;
			float? rotInRack = this.GetRotInRack(thing, thing.def, thing.Position);
			if (rotInRack != null)
			{
				num = rotInRack.Value;
			}
			else if (thing != null)
			{
				num = -this.maxAngle + (float)(thing.thingIDNumber * 542) % (this.maxAngle * 2f);
			}
			num += extraRotation;
			this.subGraphic.Print(layer, thing, num);
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x0012C342 File Offset: 0x0012A542
		public override void TryInsertIntoAtlas(TextureAtlasGroup group)
		{
			this.subGraphic.TryInsertIntoAtlas(group);
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x0012C350 File Offset: 0x0012A550
		private float? GetRotInRack(Thing thing, ThingDef thingDef, IntVec3 loc)
		{
			if (thing == null || !thingDef.IsWeapon || !thing.Spawned || !loc.InBounds(thing.Map) || loc.GetEdifice(thing.Map) == null || loc.GetItemCount(thing.Map) < 2)
			{
				return null;
			}
			if (thingDef.rotateInShelves)
			{
				return new float?(-90f);
			}
			return new float?(0f);
		}

		// Token: 0x040029B9 RID: 10681
		private Graphic subGraphic;

		// Token: 0x040029BA RID: 10682
		private float maxAngle;
	}
}
