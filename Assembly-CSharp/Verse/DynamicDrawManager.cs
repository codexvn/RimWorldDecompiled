using System;
using System.Collections.Generic;
using Gilzoide.ManagedJobs;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EC RID: 1004
	public sealed class DynamicDrawManager
	{
		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x060019AA RID: 6570 RVA: 0x000817D0 File Offset: 0x0007F9D0
		public IReadOnlyList<Thing> DrawThings
		{
			get
			{
				return this.drawThings;
			}
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x000817D8 File Offset: 0x0007F9D8
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x000817F2 File Offset: 0x0007F9F2
		public void RegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning(string.Format("Cannot register drawable {0} while drawing is in progress. Things shouldn't be spawned in Draw methods.", t));
				}
				this.drawThings.Add(t);
			}
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00081825 File Offset: 0x0007FA25
		public void DeRegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					Log.Warning(string.Format("Cannot deregister drawable {0} while drawing is in progress. Things shouldn't be despawned in Draw methods.", t));
				}
				this.drawThings.Remove(t);
			}
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0008185C File Offset: 0x0007FA5C
		public void DrawDynamicThings()
		{
			if (!DebugViewSettings.drawThingsDynamic || this.map.Disposed)
			{
				return;
			}
			this.drawingNow = true;
			bool flag = SilhouetteUtility.CanHighlightAny();
			NativeArray<DynamicDrawManager.ThingCullDetails> details = new NativeArray<DynamicDrawManager.ThingCullDetails>(this.drawThings.Count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			this.ComputeCulledThings(details);
			if (!DebugViewSettings.singleThreadedDrawing)
			{
				using (new ProfilerBlock("Ensure Graphics Initialized"))
				{
					for (int i = 0; i < details.Length; i++)
					{
						if (details[i].shouldDraw)
						{
							this.drawThings[i].DynamicDrawPhase(DrawPhase.EnsureInitialized);
						}
					}
				}
				this.PreDrawVisibleThings(details);
			}
			try
			{
				using (new ProfilerBlock("Draw Visible"))
				{
					for (int j = 0; j < details.Length; j++)
					{
						if (details[j].shouldDraw || details[j].shouldDrawShadow)
						{
							try
							{
								if (details[j].shouldDraw)
								{
									this.drawThings[j].DynamicDrawPhase(DrawPhase.Draw);
								}
								else
								{
									Pawn pawn = this.drawThings[j] as Pawn;
									if (pawn != null)
									{
										pawn.DrawShadowAt(pawn.DrawPos);
									}
								}
							}
							catch (Exception arg)
							{
								Log.Error(string.Format("Exception drawing {0}: {1}", this.drawThings[j], arg));
							}
						}
					}
				}
				if (flag)
				{
					this.DrawSilhouettes(details);
				}
			}
			catch (Exception arg2)
			{
				Log.Error(string.Format("Exception drawing dynamic things: {0}", arg2));
			}
			finally
			{
				details.Dispose();
			}
			this.drawingNow = false;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00081A3C File Offset: 0x0007FC3C
		private void PreDrawVisibleThings(NativeArray<DynamicDrawManager.ThingCullDetails> details)
		{
			using (new ProfilerBlock("Pre draw job"))
			{
				new ManagedJobParallelFor(new DynamicDrawManager.PreDrawThings
				{
					details = details,
					things = this.drawThings
				}).Schedule(details.Length, UnityData.GetIdealBatchCount(details.Length), default(JobHandle)).Complete();
			}
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x00081AC0 File Offset: 0x0007FCC0
		private void ComputeCulledThings(NativeArray<DynamicDrawManager.ThingCullDetails> details)
		{
			CellRect cellRect = Find.CameraDriver.CurrentViewRect;
			cellRect = cellRect.ExpandedBy(1);
			if (WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				cellRect = cellRect.Encapsulate(WorldComponent_GravshipController.GravshipRenderBounds);
			}
			cellRect.ClipInsideMap(this.map);
			using (new ProfilerBlock("Prepare cull job"))
			{
				for (int i = 0; i < this.drawThings.Count; i++)
				{
					Thing thing = this.drawThings[i];
					DynamicDrawManager.ThingCullDetails value = new DynamicDrawManager.ThingCullDetails
					{
						cell = ((thing is Pawn) ? thing.DrawPos.ToIntVec3() : thing.Position),
						coarseBounds = thing.OccupiedDrawRect(),
						hideAtSnowOrSandDepth = thing.def.hideAtSnowOrSandDepth,
						seeThroughFog = thing.def.seeThroughFog,
						hasSunShadows = thing.def.HasSunShadows,
						drawOffscreen = thing.def.drawOffscreen
					};
					details[i] = value;
				}
			}
			using (new ProfilerBlock("Cull job"))
			{
				new DynamicDrawManager.CullJob
				{
					indices = this.map.cellIndices,
					viewRect = cellRect,
					fogGrid = this.map.fogGrid.FogGrid_Unsafe,
					depthGrid = this.map.snowGrid.DepthGrid_Unsafe,
					details = details,
					checkShadows = MatBases.SunShadow.shader.isSupported,
					shadowViewRect = SectionLayer_SunShadows.GetSunShadowsViewRect(this.map, cellRect)
				}.Schedule(details.Length, UnityData.GetIdealBatchCount(details.Length), default(JobHandle)).Complete();
			}
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00081CBC File Offset: 0x0007FEBC
		private void DrawSilhouettes(NativeArray<DynamicDrawManager.ThingCullDetails> details)
		{
			using (new ProfilerBlock("Prepare matrices job"))
			{
				for (int i = 0; i < details.Length; i++)
				{
					if (details[i].shouldDraw)
					{
						Thing thing = this.drawThings[i];
						if (SilhouetteUtility.ShouldDrawSilhouette(thing))
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								DynamicDrawManager.ThingCullDetails value = details[i];
								value.pos = pawn.Drawer.renderer.SilhouettePos;
								value.drawSize = pawn.Drawer.renderer.SilhouetteGraphic.drawSize;
								value.drawSilhouette = true;
								details[i] = value;
							}
						}
					}
				}
			}
			using (new ProfilerBlock("Compute matrices"))
			{
				new DynamicDrawManager.ComputeSilhouetteMatricesJob
				{
					inverseFovScale = Find.CameraDriver.InverseFovScale,
					altitude = AltitudeLayer.Silhouettes.AltitudeFor(),
					details = details
				}.Schedule(details.Length, UnityData.GetIdealBatchCount(details.Length), default(JobHandle)).Complete();
			}
			using (new ProfilerBlock("Draw silhouettes"))
			{
				for (int j = 0; j < details.Length; j++)
				{
					if (details[j].drawSilhouette)
					{
						Pawn pawn2 = this.drawThings[j] as Pawn;
						if (pawn2 != null)
						{
							SilhouetteUtility.DrawSilhouetteJob(pawn2, details[j].trs);
						}
					}
				}
			}
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00081E8C File Offset: 0x0008008C
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings));
		}

		// Token: 0x04001684 RID: 5764
		private Map map;

		// Token: 0x04001685 RID: 5765
		private readonly List<Thing> drawThings = new List<Thing>();

		// Token: 0x04001686 RID: 5766
		private bool drawingNow;

		// Token: 0x020003ED RID: 1005
		private struct ThingCullDetails
		{
			// Token: 0x04001687 RID: 5767
			public IntVec3 cell;

			// Token: 0x04001688 RID: 5768
			public CellRect coarseBounds;

			// Token: 0x04001689 RID: 5769
			public bool seeThroughFog;

			// Token: 0x0400168A RID: 5770
			public float hideAtSnowOrSandDepth;

			// Token: 0x0400168B RID: 5771
			public bool drawOffscreen;

			// Token: 0x0400168C RID: 5772
			public Vector3 pos;

			// Token: 0x0400168D RID: 5773
			public Vector2 drawSize;

			// Token: 0x0400168E RID: 5774
			public bool drawSilhouette;

			// Token: 0x0400168F RID: 5775
			public bool hasSunShadows;

			// Token: 0x04001690 RID: 5776
			public Matrix4x4 trs;

			// Token: 0x04001691 RID: 5777
			public bool shouldDraw;

			// Token: 0x04001692 RID: 5778
			public bool shouldDrawShadow;
		}

		// Token: 0x020003EE RID: 1006
		[BurstCompile]
		private struct CullJob : IJobParallelFor
		{
			// Token: 0x060019B3 RID: 6579 RVA: 0x00081EA0 File Offset: 0x000800A0
			[BurstCompile]
			public void Execute(int index)
			{
				DynamicDrawManager.ThingCullDetails thingCullDetails = this.details[index];
				int num = this.indices[thingCullDetails.cell];
				if (!this.indices.Contains(num))
				{
					return;
				}
				if (!thingCullDetails.seeThroughFog && this.fogGrid.IsSet(num))
				{
					return;
				}
				if (thingCullDetails.hideAtSnowOrSandDepth < 1f && this.depthGrid[num] > thingCullDetails.hideAtSnowOrSandDepth)
				{
					return;
				}
				if (!thingCullDetails.drawOffscreen && !this.viewRect.Overlaps(thingCullDetails.coarseBounds))
				{
					if (this.checkShadows && thingCullDetails.hasSunShadows)
					{
						thingCullDetails.shouldDrawShadow = this.shadowViewRect.Contains(thingCullDetails.cell);
					}
					return;
				}
				thingCullDetails.shouldDraw = true;
				this.details[index] = thingCullDetails;
			}

			// Token: 0x04001693 RID: 5779
			[ReadOnly]
			public CellRect viewRect;

			// Token: 0x04001694 RID: 5780
			[ReadOnly]
			public CellRect shadowViewRect;

			// Token: 0x04001695 RID: 5781
			[ReadOnly]
			public CellIndices indices;

			// Token: 0x04001696 RID: 5782
			[ReadOnly]
			public bool checkShadows;

			// Token: 0x04001697 RID: 5783
			[ReadOnly]
			public NativeBitArray fogGrid;

			// Token: 0x04001698 RID: 5784
			[ReadOnly]
			public NativeArray<float> depthGrid;

			// Token: 0x04001699 RID: 5785
			public NativeArray<DynamicDrawManager.ThingCullDetails> details;
		}

		// Token: 0x020003EF RID: 1007
		[BurstCompile]
		private struct ComputeSilhouetteMatricesJob : IJobParallelFor
		{
			// Token: 0x060019B4 RID: 6580 RVA: 0x00081F70 File Offset: 0x00080170
			[BurstCompile]
			public void Execute(int index)
			{
				DynamicDrawManager.ThingCullDetails thingCullDetails = this.details[index];
				if (!thingCullDetails.drawSilhouette)
				{
					return;
				}
				Vector3 vector = new Vector3(thingCullDetails.drawSize.x, 0f, thingCullDetails.drawSize.y);
				Vector3 s = this.inverseFovScale;
				if (vector.x < 2.5f)
				{
					s.x *= vector.x + SilhouetteUtility.AdjustScale(vector.x);
				}
				else
				{
					s.x *= vector.x;
				}
				if (vector.z < 2.5f)
				{
					s.z *= vector.z + SilhouetteUtility.AdjustScale(vector.z);
				}
				else
				{
					s.z *= vector.z;
				}
				Vector3 pos = thingCullDetails.pos;
				pos.y = this.altitude;
				thingCullDetails.trs = Matrix4x4.TRS(pos, Quaternion.AngleAxis(0f, Vector3.up), s);
				this.details[index] = thingCullDetails;
			}

			// Token: 0x0400169A RID: 5786
			public Vector3 inverseFovScale;

			// Token: 0x0400169B RID: 5787
			public float altitude;

			// Token: 0x0400169C RID: 5788
			public NativeArray<DynamicDrawManager.ThingCullDetails> details;
		}

		// Token: 0x020003F0 RID: 1008
		private class PreDrawThings : IJobParallelFor
		{
			// Token: 0x060019B5 RID: 6581 RVA: 0x00082074 File Offset: 0x00080274
			public void Execute(int index)
			{
				Thing thing = this.things[index];
				if (this.details[index].shouldDraw)
				{
					thing.DynamicDrawPhase(DrawPhase.ParallelPreDraw);
				}
			}

			// Token: 0x0400169D RID: 5789
			public NativeArray<DynamicDrawManager.ThingCullDetails> details;

			// Token: 0x0400169E RID: 5790
			public List<Thing> things;
		}
	}
}
