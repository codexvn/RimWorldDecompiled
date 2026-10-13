using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C7F RID: 15487
	public class WorldDrawLayer_Glow : WorldDrawLayer
	{
		// Token: 0x060150A6 RID: 86182 RVA: 0x0064C23B File Offset: 0x0064A43B
		public override IEnumerable Regenerate()
		{
			foreach (object obj in base.Regenerate())
			{
				yield return obj;
			}
			IEnumerator enumerator = null;
			if (this.material == null)
			{
				this.material = new Material(ShaderDatabase.PlanetGlow);
				this.material.SetFloat(ShaderPropertyIDs.GlowRadius, 16f);
				this.material.SetFloat(ShaderPropertyIDs.PlanetRadius, this.planetLayer.Radius);
				this.material.SetVector(ShaderPropertyIDs.PlanetOrigin, this.planetLayer.Origin);
			}
			List<Vector3> collection;
			List<int> collection2;
			SphereGenerator.Generate(4, this.planetLayer.Radius + 16f + 0.1f, Vector3.forward, 180f, out collection, out collection2);
			LayerSubMesh subMesh = base.GetSubMesh(this.material);
			subMesh.verts.AddRange(collection);
			subMesh.tris.AddRange(collection2);
			base.FinalizeMesh(MeshParts.All);
			yield break;
			yield break;
		}

		// Token: 0x0400EAB9 RID: 60089
		private Material material;

		// Token: 0x0400EABA RID: 60090
		private const int SubdivisionsCount = 4;

		// Token: 0x0400EABB RID: 60091
		public const float GlowRadius = 16f;
	}
}
