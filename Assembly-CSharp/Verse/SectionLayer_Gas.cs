using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047C RID: 1148
	[StaticConstructorOnStartup]
	public class SectionLayer_Gas : SectionLayer
	{
		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001E16 RID: 7702 RVA: 0x0009BC8F File Offset: 0x00099E8F
		protected virtual FloatRange VertexScaleOffsetRange
		{
			get
			{
				return new FloatRange(0.4f, 0.6f);
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0009BCA0 File Offset: 0x00099EA0
		protected virtual FloatRange VertexPositionOffsetRange
		{
			get
			{
				return new FloatRange(-0.2f, 0.2f);
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001E18 RID: 7704 RVA: 0x0009BCB1 File Offset: 0x00099EB1
		public override bool Visible
		{
			get
			{
				return DebugViewSettings.drawGas;
			}
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x0009BCB8 File Offset: 0x00099EB8
		public virtual Material Mat
		{
			get
			{
				return SectionLayer_Gas.GasMat;
			}
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x0009BCC0 File Offset: 0x00099EC0
		public SectionLayer_Gas(Section section) : base(section)
		{
			this.relevantChangeTypes = MapMeshFlagDefOf.Gas;
			this.propertyBlock = new MaterialPropertyBlock();
			if (!SectionLayer_Gas.gasMatSet)
			{
				SectionLayer_Gas.gasMatSet = true;
				SectionLayer_Gas.GasMat.SetTexture(ShaderPropertyIDs.ToxGasTex, ContentFinder<Texture2D>.Get("Things/Gas/GasCloudThickA", true));
				SectionLayer_Gas.GasMat.SetTexture(ShaderPropertyIDs.RotGasTex, ContentFinder<Texture2D>.Get("Things/Gas/GasCloudThickA", true));
				SectionLayer_Gas.GasMat.SetTexture(ShaderPropertyIDs.DeadlifeDustTex, ContentFinder<Texture2D>.Get("Things/Gas/DeadlifeDust", true));
			}
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0009BD4C File Offset: 0x00099F4C
		public override void DrawLayer()
		{
			if (!this.Visible)
			{
				return;
			}
			this.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, RealTime.UnpausedRealTime);
			int count = this.subMeshes.Count;
			for (int i = 0; i < count; i++)
			{
				LayerSubMesh layerSubMesh = this.subMeshes[i];
				if (layerSubMesh.finalized && !layerSubMesh.disabled)
				{
					Graphics.DrawMesh(layerSubMesh.mesh, Vector3.zero, Quaternion.identity, layerSubMesh.material, 0, null, 0, this.propertyBlock);
				}
			}
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x0009BDD0 File Offset: 0x00099FD0
		public virtual Color ColorAt(IntVec3 cell)
		{
			return this.DensityAt(cell);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x0009BDDE File Offset: 0x00099FDE
		public virtual Vector4 DensityAt(IntVec3 cell)
		{
			return base.Map.gasGrid.DensitiesAt(cell);
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x0009BDF4 File Offset: 0x00099FF4
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			LayerSubMesh subMesh = base.GetSubMesh(this.Mat);
			float altitude = AltitudeLayer.Gas.AltitudeFor();
			int num = this.section.botLeft.x;
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				if (base.Map.gasGrid.AnyGasAt(intVec))
				{
					int count = subMesh.verts.Count;
					this.AddCell(intVec, num, count, subMesh, altitude);
				}
				num++;
			}
			if (subMesh.verts.Count > 0)
			{
				subMesh.FinalizeMesh(MeshParts.All);
			}
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x0009BEC0 File Offset: 0x0009A0C0
		protected void AddCell(IntVec3 c, int index, int startVertIndex, LayerSubMesh sm, float altitude)
		{
			Rand.PushState(index);
			Color c2 = this.ColorAt(c);
			float randomInRange = this.VertexScaleOffsetRange.RandomInRange;
			float randomInRange2 = this.VertexPositionOffsetRange.RandomInRange;
			float randomInRange3 = this.VertexPositionOffsetRange.RandomInRange;
			float x = (float)c.x - randomInRange + randomInRange2;
			float x2 = (float)(c.x + 1) + randomInRange + randomInRange2;
			float z = (float)c.z - randomInRange + randomInRange3;
			float z2 = (float)(c.z + 1) + randomInRange + randomInRange3;
			float y = altitude + Rand.Range(-0.01f, 0.01f);
			sm.verts.Add(new Vector3(x, y, z));
			sm.verts.Add(new Vector3(x, y, z2));
			sm.verts.Add(new Vector3(x2, y, z2));
			sm.verts.Add(new Vector3(x2, y, z));
			sm.uvs.Add(new Vector3(0f, 0f, (float)index));
			sm.uvs.Add(new Vector3(0f, 1f, (float)index));
			sm.uvs.Add(new Vector3(1f, 1f, (float)index));
			sm.uvs.Add(new Vector3(1f, 0f, (float)index));
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.colors.Add(c2);
			sm.tris.Add(startVertIndex);
			sm.tris.Add(startVertIndex + 1);
			sm.tris.Add(startVertIndex + 2);
			sm.tris.Add(startVertIndex);
			sm.tris.Add(startVertIndex + 2);
			sm.tris.Add(startVertIndex + 3);
			Rand.PopState();
		}

		// Token: 0x04001A06 RID: 6662
		private MaterialPropertyBlock propertyBlock;

		// Token: 0x04001A07 RID: 6663
		private static Material GasMat = MaterialPool.MatFrom("Things/Gas/GasCloudThickA", ShaderDatabase.GasRotating, 3000);

		// Token: 0x04001A08 RID: 6664
		private static bool gasMatSet = false;
	}
}
