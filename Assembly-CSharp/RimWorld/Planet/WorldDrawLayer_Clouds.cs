using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C7A RID: 15482
	public class WorldDrawLayer_Clouds : WorldDrawLayer
	{
		// Token: 0x06015083 RID: 86147 RVA: 0x0064BA15 File Offset: 0x00649C15
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			if (this.material == null)
			{
				this.material = new Material(WorldMaterials.Clouds);
				this.material.SetVector(ShaderPropertyIDs.Seed, new Vector2(UnityEngine.Random.value, UnityEngine.Random.value));
				this.material.SetFloat(ShaderPropertyIDs.PlanetRadius, this.planetLayer.Radius);
				this.material.SetVector(ShaderPropertyIDs.PlanetOrigin, this.planetLayer.Origin);
				this.noiseTexture = ContentFinder<Texture2D>.Get("Other/Perlin", true);
				this.sunsetColorRamp = ContentFinder<Texture2D>.Get("World/SunsetGradient", true);
				this.cloudTexture = ContentFinder<Texture2D>.Get("World/CloudMap", true);
				this.material.SetTexture(ShaderPropertyIDs.NoiseTex, this.noiseTexture);
				this.material.SetTexture(ShaderPropertyIDs.SunsetColorRamp, this.sunsetColorRamp);
				this.material.SetTexture(ShaderPropertyIDs.CloudMap, this.cloudTexture);
				this.set = false;
			}
			List<Vector3> collection;
			List<int> collection2;
			SphereGenerator.Generate(4, this.planetLayer.Radius + 0.2f, Vector3.forward, 180f, out collection, out collection2);
			LayerSubMesh subMesh = base.GetSubMesh(this.material);
			subMesh.verts.AddRange(collection);
			subMesh.tris.AddRange(collection2);
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x06015084 RID: 86148 RVA: 0x0064BA25 File Offset: 0x00649C25
		private float GetTargetOpacity()
		{
			if (PlanetLayer.Selected == this.planetLayer && !WorldRendererUtility.WorldBackgroundNow)
			{
				return 0f;
			}
			return 1f;
		}

		// Token: 0x06015085 RID: 86149 RVA: 0x0064BA48 File Offset: 0x00649C48
		public override void Render()
		{
			this.opacity = ((!this.set) ? this.GetTargetOpacity() : Mathf.SmoothDamp(this.opacity, this.GetTargetOpacity(), ref this.opacityVel, 0.15f));
			this.material.SetFloat(ShaderPropertyIDs.CloudShaderOpacity, this.opacity);
			this.set = true;
			base.Render();
		}

		// Token: 0x0400EAA2 RID: 60066
		private Material material;

		// Token: 0x0400EAA3 RID: 60067
		private Texture2D noiseTexture;

		// Token: 0x0400EAA4 RID: 60068
		private Texture2D sunsetColorRamp;

		// Token: 0x0400EAA5 RID: 60069
		private Texture2D cloudTexture;

		// Token: 0x0400EAA6 RID: 60070
		private float opacity = 1f;

		// Token: 0x0400EAA7 RID: 60071
		private float opacityVel;

		// Token: 0x0400EAA8 RID: 60072
		private bool set;

		// Token: 0x0400EAA9 RID: 60073
		private const int SubdivisionsCount = 4;

		// Token: 0x0400EAAA RID: 60074
		private const float OpacityChangeTime = 0.15f;
	}
}
