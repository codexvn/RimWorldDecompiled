using System;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine;

namespace Verse.Glow
{
	// Token: 0x02000DF4 RID: 3572
	[BurstCompile]
	public struct ComputeGlowGridsJob : IJobParallelFor
	{
		// Token: 0x06005ADC RID: 23260 RVA: 0x001D2608 File Offset: 0x001D0808
		[BurstCompile]
		public void Execute(int index)
		{
			GlowLight glowLight = this.lights[index];
			if (!glowLight.dirty)
			{
				return;
			}
			glowLight.dirty = false;
			this.lights[index] = glowLight;
			GlowUniqueState glowUniqueState = this.statePool[this.threadIndex];
			glowUniqueState.PrepareComparer(ref glowLight);
			LocalGlowArea localGlowArea = this.glowPool[glowLight.localGlowPoolIndex];
			this.PrepareFill(ref glowLight, ref glowUniqueState, ref localGlowArea);
			this.Flood(ref glowLight, ref glowUniqueState, ref localGlowArea);
			glowUniqueState.Clear();
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x001D268C File Offset: 0x001D088C
		[BurstCompile]
		private void Flood(ref GlowLight glowLight, ref GlowUniqueState filler, ref LocalGlowArea localArea)
		{
			int num = Mathf.RoundToInt(glowLight.glowRadius * 100f);
			int num2 = glowLight.diameter * glowLight.diameter;
			IntVec3 a;
			while (filler.queue.TryPop(out a))
			{
				int index = glowLight.DeltaToLocalIndex(a);
				GlowCell value = filler.area[index];
				value.status = GlowCellStatus.Finalized;
				filler.area[index] = value;
				this.SetGlowFromDist(ref glowLight, ref filler, ref localArea, a);
				for (int i = 0; i < ComputeGlowGridsJob.Directions.Length; i++)
				{
					IntVec3 b = a + ComputeGlowGridsJob.Directions[i];
					int num3 = glowLight.DeltaToLocalIndex(b);
					if (num3 >= 0 && num3 < num2)
					{
						GlowCell glowCell = filler.area[num3];
						if (glowCell.status != GlowCellStatus.Finalized)
						{
							IntVec3 intVec = glowLight.position + b;
							if (intVec.x < this.mapSize.x && intVec.z < this.mapSize.z && intVec.x >= 0 && intVec.z >= 0)
							{
								bool flag = this.lightBlockers.IsSet(this.indices.CellToIndex(intVec));
								filler.blockers.Set(i, flag);
								if (!flag)
								{
									int num4 = (i < 4) ? 100 : 141;
									int num5 = filler.area[index].intDist + num4;
									if (num5 <= num)
									{
										if (i >= 4)
										{
											switch (i)
											{
											case 4:
												if (filler.blockers.IsSet(0) && filler.blockers.IsSet(1))
												{
													goto IL_278;
												}
												break;
											case 5:
												if (filler.blockers.IsSet(1) && filler.blockers.IsSet(2))
												{
													goto IL_278;
												}
												break;
											case 6:
												if (filler.blockers.IsSet(2) && filler.blockers.IsSet(3))
												{
													goto IL_278;
												}
												break;
											case 7:
												if (filler.blockers.IsSet(0) && filler.blockers.IsSet(3))
												{
													goto IL_278;
												}
												break;
											}
										}
										if (glowCell.status == GlowCellStatus.Unvisited)
										{
											glowCell.intDist = int.MaxValue;
											glowCell.status = GlowCellStatus.Open;
											filler.area[num3] = glowCell;
										}
										if (num5 < glowCell.intDist)
										{
											glowCell.intDist = num5;
											glowCell.status = GlowCellStatus.Open;
											filler.area[num3] = glowCell;
											filler.queue.Insert(b);
										}
									}
								}
							}
						}
					}
					IL_278:;
				}
			}
		}

		// Token: 0x06005ADE RID: 23262 RVA: 0x001D292C File Offset: 0x001D0B2C
		[BurstCompile]
		private void PrepareFill(ref GlowLight glowLight, ref GlowUniqueState filler, ref LocalGlowArea localArea)
		{
			NativeArrayUtility.MemClear<Color32>(localArea.colors);
			IntVec3 intVec = new IntVec3(0, 0, 0);
			int num = glowLight.DeltaToLocalIndex(intVec);
			for (int i = 0; i < glowLight.diameter * glowLight.diameter; i++)
			{
				GlowCell value = default(GlowCell);
				if (i == num)
				{
					value.intDist = 100;
				}
				filler.area[i] = value;
			}
			filler.queue.Insert(IntVec3.Zero);
		}

		// Token: 0x06005ADF RID: 23263 RVA: 0x001D29A4 File Offset: 0x001D0BA4
		[BurstCompile]
		private void SetGlowFromDist(ref GlowLight glowLight, ref GlowUniqueState filler, ref LocalGlowArea localArea, in IntVec3 delta)
		{
			float num = -1f / glowLight.glowRadius;
			int index = glowLight.DeltaToLocalIndex(delta);
			float num2 = (float)filler.area[index].intDist / 100f;
			ColorInt colorInt = default(ColorInt);
			if (num2 <= glowLight.glowRadius)
			{
				float b = 1f / (num2 * num2);
				float b2 = Mathf.Lerp(1f + num * num2, b, 0.4f);
				colorInt = glowLight.glowColor * b2;
				colorInt.a = 0;
			}
			if (colorInt.r <= 0 && colorInt.g <= 0 && colorInt.b <= 0)
			{
				return;
			}
			colorInt.ClampToNonNegative();
			colorInt.a = (int)num2;
			Color32 value;
			colorInt.ProjectToColor32Fast(out value);
			localArea.colors[index] = value;
		}

		// Token: 0x040041C7 RID: 16839
		private static readonly IntVec3[] Directions = new IntVec3[]
		{
			new IntVec3(0, 0, -1),
			new IntVec3(1, 0, 0),
			new IntVec3(0, 0, 1),
			new IntVec3(-1, 0, 0),
			new IntVec3(1, 0, -1),
			new IntVec3(1, 0, 1),
			new IntVec3(-1, 0, 1),
			new IntVec3(-1, 0, -1)
		};

		// Token: 0x040041C8 RID: 16840
		private const int CardinalCost = 100;

		// Token: 0x040041C9 RID: 16841
		private const int DiagonalCost = 141;

		// Token: 0x040041CA RID: 16842
		[NativeSetThreadIndex]
		[ReadOnly]
		private int threadIndex;

		// Token: 0x040041CB RID: 16843
		[NativeDisableParallelForRestriction]
		public NativeList<GlowLight> lights;

		// Token: 0x040041CC RID: 16844
		[ReadOnly]
		public NativeBitArray lightBlockers;

		// Token: 0x040041CD RID: 16845
		[ReadOnly]
		public CellIndices indices;

		// Token: 0x040041CE RID: 16846
		[NativeDisableParallelForRestriction]
		public NativeList<LocalGlowArea> glowPool;

		// Token: 0x040041CF RID: 16847
		[NativeDisableParallelForRestriction]
		public NativeArray<GlowUniqueState> statePool;

		// Token: 0x040041D0 RID: 16848
		public IntVec3 mapSize;
	}
}
