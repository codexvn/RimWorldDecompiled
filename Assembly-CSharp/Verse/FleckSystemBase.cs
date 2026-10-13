using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Verse
{
	// Token: 0x02000402 RID: 1026
	public class FleckSystemBase<TFleck> : FleckSystem where TFleck : struct, IFleck
	{
		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x06001A06 RID: 6662 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool ParallelizedDrawing
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00083320 File Offset: 0x00081520
		public FleckSystemBase(FleckManager parent) : base(parent)
		{
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x00083360 File Offset: 0x00081560
		public override void Update(float deltaTime)
		{
			try
			{
				for (int i = this.dataRealtime.Count - 1; i >= 0; i--)
				{
					TFleck value = this.dataRealtime[i];
					if (value.TimeInterval(deltaTime, this.parent.parent))
					{
						this.tmpRemoveIndices.Add(i);
					}
					else
					{
						this.dataRealtime[i] = value;
					}
				}
				this.dataRealtime.RemoveBatchUnordered(this.tmpRemoveIndices);
			}
			finally
			{
				this.tmpRemoveIndices.Clear();
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x000833F8 File Offset: 0x000815F8
		public override void Tick()
		{
			try
			{
				for (int i = this.dataGametime.Count - 1; i >= 0; i--)
				{
					TFleck value = this.dataGametime[i];
					if (value.TimeInterval(0.016666668f, this.parent.parent))
					{
						this.tmpRemoveIndices.Add(i);
					}
					else
					{
						this.dataGametime[i] = value;
					}
				}
				this.dataGametime.RemoveBatchUnordered(this.tmpRemoveIndices);
			}
			finally
			{
				this.tmpRemoveIndices.Clear();
			}
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00083494 File Offset: 0x00081694
		private static void DrawParallel(object state)
		{
			if (WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				return;
			}
			FleckParallelizationInfo fleckParallelizationInfo = (FleckParallelizationInfo)state;
			try
			{
				List<FleckThrown> list = (List<FleckThrown>)fleckParallelizationInfo.data;
				for (int i = fleckParallelizationInfo.startIndex; i < fleckParallelizationInfo.endIndex; i++)
				{
					list[i].Draw(fleckParallelizationInfo.drawBatch);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			finally
			{
				fleckParallelizationInfo.doneEvent.Set();
			}
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00083520 File Offset: 0x00081720
		public override void ForceDraw(DrawBatch drawBatch)
		{
			FleckSystemBase<TFleck>.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.drawBatch = drawBatch;
			foreach (FleckDef fleckDef in this.handledDefs)
			{
				if (fleckDef.graphicData != null)
				{
					fleckDef.graphicData.ExplicitlyInitCachedGraphic();
				}
				if (fleckDef.randomGraphics != null)
				{
					foreach (GraphicData graphicData in fleckDef.randomGraphics)
					{
						graphicData.ExplicitlyInitCachedGraphic();
					}
				}
			}
			if (this.ParallelizedDrawing)
			{
				if (this.CachedDrawParallelWaitCallback == null)
				{
					this.CachedDrawParallelWaitCallback = new WaitCallback(FleckSystemBase<TFleck>.DrawParallel);
				}
				FleckSystemBase<TFleck>.<>c__DisplayClass12_1 CS$<>8__locals2;
				CS$<>8__locals2.parallelizationDegree = Environment.ProcessorCount;
				this.<ForceDraw>g__Process|12_1(this.dataRealtime, ref CS$<>8__locals1, ref CS$<>8__locals2);
				this.<ForceDraw>g__Process|12_1(this.dataGametime, ref CS$<>8__locals1, ref CS$<>8__locals2);
				return;
			}
			this.<ForceDraw>g__Process|12_0(this.dataRealtime, ref CS$<>8__locals1);
			this.<ForceDraw>g__Process|12_0(this.dataGametime, ref CS$<>8__locals1);
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x00083644 File Offset: 0x00081844
		public override void CreateFleck(FleckCreationData creationData)
		{
			TFleck item = Activator.CreateInstance<TFleck>();
			item.Setup(creationData);
			if (creationData.def.realTime)
			{
				this.dataRealtime.Add(item);
				return;
			}
			this.dataGametime.Add(item);
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0008368C File Offset: 0x0008188C
		public override void MergeWith(FleckSystem system)
		{
			FleckSystemBase<TFleck> fleckSystemBase = system as FleckSystemBase<TFleck>;
			if (fleckSystemBase != null)
			{
				this.handledDefs.AddRangeUnique(fleckSystemBase.handledDefs);
				this.dataRealtime.AddRange(fleckSystemBase.dataRealtime);
				this.dataGametime.AddRange(fleckSystemBase.dataGametime);
				return;
			}
			throw new NotImplementedException("FleckSystemBase does not yet support merging with " + system.GetType().Name);
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x000836F1 File Offset: 0x000818F1
		public override IEnumerable<IFleck> EnumerateFlecks()
		{
			int num;
			for (int i = this.dataGametime.Count - 1; i >= 0; i = num - 1)
			{
				yield return this.dataGametime[i];
				num = i;
			}
			for (int i = this.dataRealtime.Count - 1; i >= 0; i = num - 1)
			{
				yield return this.dataRealtime[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x00083704 File Offset: 0x00081904
		public override void RemoveAllFlecks(Predicate<IFleck> shouldRemove)
		{
			for (int i = this.dataGametime.Count - 1; i >= 0; i--)
			{
				if (shouldRemove(this.dataGametime[i]))
				{
					this.dataGametime.RemoveAt(i);
				}
			}
			for (int j = this.dataRealtime.Count - 1; j >= 0; j--)
			{
				if (shouldRemove(this.dataRealtime[j]))
				{
					this.dataRealtime.RemoveAt(j);
				}
			}
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x000026EA File Offset: 0x000008EA
		public override void ExposeData()
		{
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x0008378C File Offset: 0x0008198C
		[CompilerGenerated]
		private void <ForceDraw>g__Process|12_1(List<TFleck> data, ref FleckSystemBase<TFleck>.<>c__DisplayClass12_0 A_2, ref FleckSystemBase<TFleck>.<>c__DisplayClass12_1 A_3)
		{
			if (data.Count > 0)
			{
				try
				{
					this.tmpParallelizationSlices.Clear();
					GenThreading.SliceWorkNoAlloc(0, data.Count, A_3.parallelizationDegree, this.tmpParallelizationSlices);
					foreach (GenThreading.Slice slice in this.tmpParallelizationSlices)
					{
						FleckParallelizationInfo parallelizationInfo = FleckUtility.GetParallelizationInfo();
						parallelizationInfo.startIndex = slice.fromInclusive;
						parallelizationInfo.endIndex = slice.toExclusive;
						parallelizationInfo.data = data;
						ThreadPool.QueueUserWorkItem(this.CachedDrawParallelWaitCallback, parallelizationInfo);
						this.tmpParallelizationInfo.Add(parallelizationInfo);
					}
					foreach (FleckParallelizationInfo fleckParallelizationInfo in this.tmpParallelizationInfo)
					{
						fleckParallelizationInfo.doneEvent.WaitOne();
						A_2.drawBatch.MergeWith(fleckParallelizationInfo.drawBatch);
					}
				}
				finally
				{
					foreach (FleckParallelizationInfo info in this.tmpParallelizationInfo)
					{
						FleckUtility.ReturnParallelizationInfo(info);
					}
					this.tmpParallelizationInfo.Clear();
				}
			}
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x000838F8 File Offset: 0x00081AF8
		[CompilerGenerated]
		private void <ForceDraw>g__Process|12_0(List<TFleck> data, ref FleckSystemBase<TFleck>.<>c__DisplayClass12_0 A_2)
		{
			for (int i = data.Count - 1; i >= 0; i--)
			{
				TFleck tfleck = data[i];
				tfleck.Draw(A_2.drawBatch);
			}
		}

		// Token: 0x040016FE RID: 5886
		private List<TFleck> dataRealtime = new List<TFleck>();

		// Token: 0x040016FF RID: 5887
		private List<TFleck> dataGametime = new List<TFleck>();

		// Token: 0x04001700 RID: 5888
		private List<GenThreading.Slice> tmpParallelizationSlices = new List<GenThreading.Slice>();

		// Token: 0x04001701 RID: 5889
		private List<FleckParallelizationInfo> tmpParallelizationInfo = new List<FleckParallelizationInfo>();

		// Token: 0x04001702 RID: 5890
		private List<int> tmpRemoveIndices = new List<int>();

		// Token: 0x04001703 RID: 5891
		private WaitCallback CachedDrawParallelWaitCallback;
	}
}
