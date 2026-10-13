using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200219F RID: 8607
	public static class LightningBoltMeshPool
	{
		// Token: 0x17001C92 RID: 7314
		// (get) Token: 0x0600BC0C RID: 48140 RVA: 0x00367DA8 File Offset: 0x00365FA8
		public static Mesh RandomBoltMesh
		{
			get
			{
				if (LightningBoltMeshPool.boltMeshes.Count < 20)
				{
					Mesh mesh = LightningBoltMeshMaker.NewBoltMesh();
					LightningBoltMeshPool.boltMeshes.Add(mesh);
					return mesh;
				}
				return LightningBoltMeshPool.boltMeshes.RandomElement<Mesh>();
			}
		}

		// Token: 0x0400817C RID: 33148
		private static List<Mesh> boltMeshes = new List<Mesh>();

		// Token: 0x0400817D RID: 33149
		private const int NumBoltMeshesMax = 20;
	}
}
