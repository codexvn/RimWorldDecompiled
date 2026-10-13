using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C6C RID: 15468
	public class GlobalDrawLayer_Sun : WorldDrawLayerBase
	{
		// Token: 0x1700342B RID: 13355
		// (get) Token: 0x0601501C RID: 86044 RVA: 0x0064A32B File Offset: 0x0064852B
		protected override int RenderLayer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x1700342C RID: 13356
		// (get) Token: 0x0601501D RID: 86045 RVA: 0x0064A667 File Offset: 0x00648867
		protected override Quaternion Rotation
		{
			get
			{
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x0601501E RID: 86046 RVA: 0x0064A673 File Offset: 0x00648873
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Sun);
			WorldRendererUtility.PrintQuadTangentialToPlanet(Vector3.forward * 20f, 15f, 0f, subMesh, true, 0f, true);
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x0400EA71 RID: 60017
		private const float SunDrawSize = 15f;
	}
}
