using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C6A RID: 15466
	public class GlobalDrawLayer_Stars : WorldDrawLayerBase
	{
		// Token: 0x17003425 RID: 13349
		// (get) Token: 0x0601500B RID: 86027 RVA: 0x0064A32B File Offset: 0x0064852B
		protected override int RenderLayer
		{
			get
			{
				return WorldCameraManager.WorldSkyboxLayer;
			}
		}

		// Token: 0x17003426 RID: 13350
		// (get) Token: 0x0601500C RID: 86028 RVA: 0x0064A332 File Offset: 0x00648532
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || (Find.GameInitData != null && Find.GameInitData.startingTile != this.calculatedForStartingTile) || this.UseStaticRotation != this.calculatedForStaticRotation;
			}
		}

		// Token: 0x17003427 RID: 13351
		// (get) Token: 0x0601500D RID: 86029 RVA: 0x0064A36D File Offset: 0x0064856D
		private bool UseStaticRotation
		{
			get
			{
				return Current.ProgramState == ProgramState.Entry;
			}
		}

		// Token: 0x17003428 RID: 13352
		// (get) Token: 0x0601500E RID: 86030 RVA: 0x0064A377 File Offset: 0x00648577
		protected override Quaternion Rotation
		{
			get
			{
				if (this.UseStaticRotation)
				{
					return Quaternion.identity;
				}
				return Quaternion.LookRotation(GenCelestial.CurSunPositionInWorldSpace());
			}
		}

		// Token: 0x0601500F RID: 86031 RVA: 0x0064A391 File Offset: 0x00648591
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			for (int i = 0; i < 1500; i++)
			{
				Vector3 unitVector = Rand.UnitVector3;
				Vector3 pos = unitVector * 20f;
				LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Stars);
				float num = GlobalDrawLayer_Stars.StarsDrawSize.RandomInRange;
				Vector3 rhs = this.UseStaticRotation ? GenCelestial.CurSunPositionInWorldSpace().normalized : Vector3.forward;
				float num2 = Vector3.Dot(unitVector, rhs);
				if (num2 > 0.8f)
				{
					num *= GenMath.LerpDouble(0.8f, 1f, 1f, 0.35f, num2);
				}
				WorldRendererUtility.PrintQuadTangentialToPlanet(pos, num, 0f, subMesh, true, Rand.Range(0f, 360f), true);
			}
			this.calculatedForStartingTile = ((Find.GameInitData != null) ? Find.GameInitData.startingTile : PlanetTile.Invalid);
			this.calculatedForStaticRotation = this.UseStaticRotation;
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x0400EA66 RID: 60006
		private bool calculatedForStaticRotation;

		// Token: 0x0400EA67 RID: 60007
		private PlanetTile calculatedForStartingTile = PlanetTile.Invalid;

		// Token: 0x0400EA68 RID: 60008
		public const float DistanceToStars = 20f;

		// Token: 0x0400EA69 RID: 60009
		private static readonly FloatRange StarsDrawSize = new FloatRange(1f, 3.8f);

		// Token: 0x0400EA6A RID: 60010
		private const int StarsCount = 1500;

		// Token: 0x0400EA6B RID: 60011
		private const float DistToSunToReduceStarSize = 0.8f;
	}
}
