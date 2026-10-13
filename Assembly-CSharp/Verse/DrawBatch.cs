using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Verse
{
	// Token: 0x020003F3 RID: 1011
	public class DrawBatch
	{
		// Token: 0x060019B9 RID: 6585 RVA: 0x00082160 File Offset: 0x00080360
		public DrawBatchPropertyBlock GetPropertyBlock()
		{
			DrawBatchPropertyBlock drawBatchPropertyBlock;
			if (this.propertyBlockCache.Count == 0)
			{
				drawBatchPropertyBlock = new DrawBatchPropertyBlock();
				this.myPropertyBlocks.Add(drawBatchPropertyBlock);
			}
			else
			{
				drawBatchPropertyBlock = this.propertyBlockCache.Pop<DrawBatchPropertyBlock>();
			}
			if (DrawBatch.PropertyBlockLeakDebug)
			{
				drawBatchPropertyBlock.leakDebugString = "Allocated from:\n\n---------------\n\n" + StackTraceUtility.ExtractStackTrace();
			}
			return drawBatchPropertyBlock;
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x000821BA File Offset: 0x000803BA
		public void ReturnPropertyBlock(DrawBatchPropertyBlock propertyBlock)
		{
			if (this.myPropertyBlocks.Contains(propertyBlock))
			{
				this.propertyBlockCache.Add(propertyBlock);
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x000821D6 File Offset: 0x000803D6
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Color? color = null, bool renderInstanced = false, DrawBatchPropertyBlock propertyBlock = null)
		{
			this.GetBatchDataForInsertion(new DrawBatch.BatchKey(mesh, material, layer, renderInstanced, propertyBlock)).Add(matrix, color);
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x000821F3 File Offset: 0x000803F3
		public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, bool renderInstanced = false)
		{
			this.GetBatchDataForInsertion(new DrawBatch.BatchKey(mesh, material, layer, renderInstanced, null)).Add(matrix);
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00082210 File Offset: 0x00080410
		public void Flush(bool draw = true)
		{
			if (this.tmpPropertyBlock == null)
			{
				this.tmpPropertyBlock = new MaterialPropertyBlock();
			}
			this.tmpPropertyBlocks.Clear();
			this.tmpPropertyBlocks.AddRange(this.propertyBlockCache);
			try
			{
				foreach (KeyValuePair<DrawBatch.BatchKey, List<DrawBatch.BatchData>> keyValuePair in this.batches)
				{
					DrawBatch.BatchKey key = keyValuePair.Key;
					try
					{
						foreach (DrawBatch.BatchData batchData in keyValuePair.Value)
						{
							DrawBatch.BatchData batchData2 = batchData;
							if (draw)
							{
								this.tmpPropertyBlock.Clear();
								if (key.propertyBlock != null)
								{
									key.propertyBlock.Write(this.tmpPropertyBlock);
								}
								if (key.renderInstanced)
								{
									key.material.enableInstancing = true;
									if (batchData2.hasAnyColors)
									{
										this.tmpPropertyBlock.SetVectorArray("_Color", batchData2.colors);
									}
									Graphics.DrawMeshInstanced(key.mesh, 0, key.material, batchData.matrices, batchData.ptr, this.tmpPropertyBlock, ShadowCastingMode.On, true, key.layer);
								}
								else
								{
									for (int i = 0; i < batchData2.ptr; i++)
									{
										Matrix4x4 matrix = batchData2.matrices[i];
										Vector4 v = batchData2.colors[i];
										if (batchData2.hasAnyColors)
										{
											this.tmpPropertyBlock.SetColor("_Color", v);
										}
										Graphics.DrawMesh(key.mesh, matrix, key.material, key.layer, null, 0, this.tmpPropertyBlock);
									}
								}
							}
							batchData2.Clear();
							this.batchDataListCache.Add(batchData2);
						}
					}
					finally
					{
						if (key.propertyBlock != null && this.myPropertyBlocks.Contains(key.propertyBlock))
						{
							this.tmpPropertyBlocks.Add(key.propertyBlock);
							key.propertyBlock.Clear();
							this.propertyBlockCache.Add(key.propertyBlock);
						}
						this.batchListCache.Add(keyValuePair.Value);
						keyValuePair.Value.Clear();
					}
				}
			}
			finally
			{
				foreach (DrawBatchPropertyBlock drawBatchPropertyBlock in this.myPropertyBlocks)
				{
					if (!this.tmpPropertyBlocks.Contains(drawBatchPropertyBlock))
					{
						Log.Warning("Property block from FleckDrawBatch leaked!" + ((drawBatchPropertyBlock.leakDebugString == null) ? null : ("Leak debug information: \n" + drawBatchPropertyBlock.leakDebugString)));
					}
				}
				HashSet<DrawBatchPropertyBlock> hashSet = this.myPropertyBlocks;
				this.myPropertyBlocks = this.tmpPropertyBlocks;
				this.tmpPropertyBlocks = hashSet;
				this.batches.Clear();
				this.lastBatchKey = default(DrawBatch.BatchKey);
				this.lastBatchList = null;
			}
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0008256C File Offset: 0x0008076C
		private DrawBatch.BatchData GetBatchDataForInsertion(DrawBatch.BatchKey key)
		{
			List<DrawBatch.BatchData> list;
			if (this.lastBatchList != null && key.GetHashCode() == this.lastBatchKey.GetHashCode() && key.Equals(this.lastBatchKey))
			{
				list = this.lastBatchList;
			}
			else
			{
				if (!this.batches.TryGetValue(key, out list))
				{
					list = ((this.batchListCache.Count == 0) ? new List<DrawBatch.BatchData>() : this.batchListCache.Pop<List<DrawBatch.BatchData>>());
					this.batches.Add(key, list);
					list.Add((this.batchDataListCache.Count == 0) ? new DrawBatch.BatchData() : this.batchDataListCache.Pop<DrawBatch.BatchData>());
				}
				this.lastBatchList = list;
				this.lastBatchKey = key;
			}
			int index = list.Count - 1;
			if (list[index].ptr < 1023)
			{
				return list[index];
			}
			DrawBatch.BatchData batchData = (this.batchDataListCache.Count == 0) ? new DrawBatch.BatchData() : this.batchDataListCache.Pop<DrawBatch.BatchData>();
			list.Add(batchData);
			return batchData;
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x00082678 File Offset: 0x00080878
		public void MergeWith(DrawBatch other)
		{
			foreach (KeyValuePair<DrawBatch.BatchKey, List<DrawBatch.BatchData>> keyValuePair in other.batches)
			{
				foreach (DrawBatch.BatchData batchData in keyValuePair.Value)
				{
					while (batchData.ptr > 0)
					{
						DrawBatch.BatchData batchDataForInsertion = this.GetBatchDataForInsertion(keyValuePair.Key);
						int num = Mathf.Min(batchData.ptr, 1023 - batchDataForInsertion.ptr);
						Array.Copy(batchData.matrices, 0, batchDataForInsertion.matrices, batchDataForInsertion.ptr, num);
						Array.Copy(batchData.colors, 0, batchDataForInsertion.colors, batchDataForInsertion.ptr, num);
						batchDataForInsertion.ptr += num;
						batchData.ptr -= num;
					}
				}
			}
		}

		// Token: 0x040016A3 RID: 5795
		private Dictionary<DrawBatch.BatchKey, List<DrawBatch.BatchData>> batches = new Dictionary<DrawBatch.BatchKey, List<DrawBatch.BatchData>>();

		// Token: 0x040016A4 RID: 5796
		private List<DrawBatch.BatchData> batchDataListCache = new List<DrawBatch.BatchData>();

		// Token: 0x040016A5 RID: 5797
		private List<List<DrawBatch.BatchData>> batchListCache = new List<List<DrawBatch.BatchData>>();

		// Token: 0x040016A6 RID: 5798
		private HashSet<DrawBatchPropertyBlock> myPropertyBlocks = new HashSet<DrawBatchPropertyBlock>();

		// Token: 0x040016A7 RID: 5799
		private List<DrawBatchPropertyBlock> propertyBlockCache = new List<DrawBatchPropertyBlock>();

		// Token: 0x040016A8 RID: 5800
		private MaterialPropertyBlock tmpPropertyBlock;

		// Token: 0x040016A9 RID: 5801
		private HashSet<DrawBatchPropertyBlock> tmpPropertyBlocks = new HashSet<DrawBatchPropertyBlock>();

		// Token: 0x040016AA RID: 5802
		public const int MaxCountPerBatch = 1023;

		// Token: 0x040016AB RID: 5803
		private static bool PropertyBlockLeakDebug;

		// Token: 0x040016AC RID: 5804
		private DrawBatch.BatchKey lastBatchKey;

		// Token: 0x040016AD RID: 5805
		private List<DrawBatch.BatchData> lastBatchList;

		// Token: 0x020003F4 RID: 1012
		private class BatchData
		{
			// Token: 0x060019C1 RID: 6593 RVA: 0x000827F5 File Offset: 0x000809F5
			public BatchData()
			{
				this.matrices = new Matrix4x4[1023];
				this.colors = new Vector4[1023];
				this.ptr = 0;
			}

			// Token: 0x060019C2 RID: 6594 RVA: 0x00082824 File Offset: 0x00080A24
			public void Clear()
			{
				this.ptr = 0;
				this.hasAnyColors = false;
			}

			// Token: 0x060019C3 RID: 6595 RVA: 0x00082834 File Offset: 0x00080A34
			public void Add(Matrix4x4 matrix)
			{
				this.matrices[this.ptr] = matrix;
				this.colors[this.ptr] = DrawBatch.BatchData.WhiteColor;
				this.ptr++;
			}

			// Token: 0x060019C4 RID: 6596 RVA: 0x0008286C File Offset: 0x00080A6C
			public void Add(Matrix4x4 matrix, Color? color)
			{
				this.matrices[this.ptr] = matrix;
				this.colors[this.ptr] = (color ?? DrawBatch.BatchData.WhiteColor);
				this.ptr++;
				this.hasAnyColors = true;
			}

			// Token: 0x040016AE RID: 5806
			public Matrix4x4[] matrices;

			// Token: 0x040016AF RID: 5807
			public int ptr;

			// Token: 0x040016B0 RID: 5808
			public Vector4[] colors;

			// Token: 0x040016B1 RID: 5809
			public bool hasAnyColors;

			// Token: 0x040016B2 RID: 5810
			private static readonly Vector4 WhiteColor = Color.white;
		}

		// Token: 0x020003F5 RID: 1013
		private struct BatchKey : IEquatable<DrawBatch.BatchKey>
		{
			// Token: 0x060019C6 RID: 6598 RVA: 0x000828E8 File Offset: 0x00080AE8
			public BatchKey(Mesh mesh, Material material, int layer, bool renderInstanced, DrawBatchPropertyBlock propertyBlock)
			{
				this.mesh = mesh;
				this.material = material;
				this.layer = layer;
				this.renderInstanced = (renderInstanced && SystemInfo.supportsInstancing);
				this.propertyBlock = propertyBlock;
				this.hash = mesh.GetHashCode();
				this.hash = Gen.HashCombineInt(this.hash, material.GetHashCode());
				this.hash = Gen.HashCombineInt(this.hash, layer | (renderInstanced ? 1 : 0) << 8);
				this.hash = ((propertyBlock == null) ? this.hash : Gen.HashCombineInt(this.hash, propertyBlock.GetHashCode()));
			}

			// Token: 0x060019C7 RID: 6599 RVA: 0x00082988 File Offset: 0x00080B88
			public override bool Equals(object obj)
			{
				if (obj != null && obj is DrawBatch.BatchKey)
				{
					DrawBatch.BatchKey other = (DrawBatch.BatchKey)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x060019C8 RID: 6600 RVA: 0x000829B4 File Offset: 0x00080BB4
			public bool Equals(DrawBatch.BatchKey other)
			{
				return this.mesh == other.mesh && this.material == other.material && this.layer == other.layer && this.renderInstanced == other.renderInstanced && this.propertyBlock == other.propertyBlock;
			}

			// Token: 0x060019C9 RID: 6601 RVA: 0x00082A09 File Offset: 0x00080C09
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x040016B3 RID: 5811
			public readonly Mesh mesh;

			// Token: 0x040016B4 RID: 5812
			public readonly Material material;

			// Token: 0x040016B5 RID: 5813
			public readonly int layer;

			// Token: 0x040016B6 RID: 5814
			public readonly bool renderInstanced;

			// Token: 0x040016B7 RID: 5815
			public readonly DrawBatchPropertyBlock propertyBlock;

			// Token: 0x040016B8 RID: 5816
			private int hash;
		}
	}
}
