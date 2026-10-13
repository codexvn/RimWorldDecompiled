using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000911 RID: 2321
	[StaticConstructorOnStartup]
	public class DesignationDragger
	{
		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x06003C22 RID: 15394 RVA: 0x0013B5B8 File Offset: 0x001397B8
		public bool Dragging
		{
			get
			{
				return this.dragging;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x06003C23 RID: 15395 RVA: 0x0013B5C0 File Offset: 0x001397C0
		private Designator SelDes
		{
			get
			{
				return Find.DesignatorManager.SelectedDesignator;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06003C24 RID: 15396 RVA: 0x0013B5CC File Offset: 0x001397CC
		private DrawStyleDef SelStyle
		{
			get
			{
				return Find.DesignatorManager.SelectedStyle;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06003C25 RID: 15397 RVA: 0x0013B5D8 File Offset: 0x001397D8
		public List<IntVec3> CellBuffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06003C26 RID: 15398 RVA: 0x0013B5E0 File Offset: 0x001397E0
		public List<IntVec3> DragCells
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.dragCells;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06003C27 RID: 15399 RVA: 0x0013B5EE File Offset: 0x001397EE
		public string FailureReason
		{
			get
			{
				this.UpdateDragCellsIfNeeded();
				return this.failureReasonInt;
			}
		}

		// Token: 0x06003C28 RID: 15400 RVA: 0x0013B5FC File Offset: 0x001397FC
		public void StartDrag()
		{
			this.dragging = !this.SelStyle.DrawStyleWorker.SingleCell;
			this.startDragCell = UI.MouseCell();
		}

		// Token: 0x06003C29 RID: 15401 RVA: 0x0013B622 File Offset: 0x00139822
		public void EndDrag()
		{
			this.dragging = false;
			this.lastDragRealTime = -99999f;
			this.lastFrameDragCellsDrawn = 0;
			if (this.sustainer != null)
			{
				this.sustainer.End();
				this.sustainer = null;
			}
		}

		// Token: 0x06003C2A RID: 15402 RVA: 0x0013B658 File Offset: 0x00139858
		public void DraggerUpdate()
		{
			if (!this.dragging)
			{
				DrawStyleDef selStyle = this.SelStyle;
				if (((selStyle != null) ? selStyle.DrawStyleWorker : null) == null || !this.SelStyle.DrawStyleWorker.SingleCell)
				{
					return;
				}
			}
			this.tmpHighlightCells.Clear();
			this.numSelectedCells = 0;
			this.UpdateCellBuffer();
			CellRect cellRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(3).ClipInsideMap(this.SelDes.Map);
			for (int i = this.buffer.Count - 1; i >= 0; i--)
			{
				IntVec3 intVec = this.buffer[i];
				if (cellRect.Contains(intVec) && this.SelDes.CanDesignateCell(intVec))
				{
					if (cellRect.Contains(intVec))
					{
						this.tmpHighlightCells.Add(intVec);
					}
					this.numSelectedCells++;
				}
			}
			if (this.SelDes.DrawHighlight)
			{
				this.SelDes.RenderHighlight(this.tmpHighlightCells);
			}
			if (this.numSelectedCells != this.lastFrameDragCellsDrawn)
			{
				if (this.SelDes.soundDragChanged != null)
				{
					SoundInfo info = SoundInfo.OnCamera(MaintenanceType.None);
					info.SetParameter("TimeSinceDrag", Time.realtimeSinceStartup - this.lastDragRealTime);
					this.SelDes.soundDragChanged.PlayOneShot(info);
				}
				this.lastDragRealTime = Time.realtimeSinceStartup;
				this.lastFrameDragCellsDrawn = this.numSelectedCells;
			}
			if (this.sustainer == null || this.sustainer.Ended)
			{
				if (this.SelDes.soundDragSustain != null)
				{
					this.sustainer = this.SelDes.soundDragSustain.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
					return;
				}
			}
			else
			{
				this.sustainer.externalParams["TimeSinceDrag"] = Time.realtimeSinceStartup - this.lastDragRealTime;
				this.sustainer.Maintain();
			}
		}

		// Token: 0x06003C2B RID: 15403 RVA: 0x0013B828 File Offset: 0x00139A28
		public void DraggerOnGUI()
		{
			if (this.dragging)
			{
				DrawStyleDef selStyle = this.SelStyle;
				if (((selStyle != null) ? selStyle.DrawStyleWorker : null) != null && !this.SelStyle.DrawStyleWorker.SingleCell)
				{
					ValueTuple<IntVec3, IntVec3> currentBoundary = this.GetCurrentBoundary();
					IntVec3 item = currentBoundary.Item1;
					IntVec3 item2 = currentBoundary.Item2;
					IntVec3 intVec = item - item2;
					intVec.x = Mathf.Abs(intVec.x) + 1;
					intVec.z = Mathf.Abs(intVec.z) + 1;
					if (this.SelStyle.drawOutline && (intVec.x > 1 || intVec.z > 1))
					{
						Vector3 v = new Vector3((float)Mathf.Min(item.x, item2.x), 0f, (float)Mathf.Min(item.z, item2.z));
						Vector3 v2 = new Vector3((float)(Mathf.Max(item.x, item2.x) + 1), 0f, (float)(Mathf.Max(item.z, item2.z) + 1));
						Vector2 vector = v.MapToUIPosition();
						Vector2 vector2 = v2.MapToUIPosition();
						Widgets.DrawBox(Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y), 1, DesignationDragger.OutlineTex);
					}
					if (this.SelDes.DragDrawMeasurements)
					{
						bool flag = intVec.x >= intVec.z;
						if (intVec.x >= 5 && (this.SelStyle.drawShortSideMeasurement || flag))
						{
							Vector2 screenPos = (item.ToUIPosition() + item2.ToUIPosition()) / 2f;
							screenPos.y = item.ToUIPosition().y;
							Widgets.DrawNumberOnMap(screenPos, intVec.x, Color.white);
						}
						if (intVec.z >= 5 && (this.SelStyle.drawShortSideMeasurement || !flag))
						{
							Vector2 screenPos2 = (item.ToUIPosition() + item2.ToUIPosition()) / 2f;
							screenPos2.x = item.ToUIPosition().x;
							Widgets.DrawNumberOnMap(screenPos2, intVec.z, Color.white);
						}
					}
					if (intVec.Magnitude >= 3f && intVec.x >= 3 && intVec.z >= 3 && this.lastFrameDragCellsDrawn > 0 && this.SelStyle.drawArea)
					{
						Widgets.DrawNumberOnMap((item.ToUIPosition() + item2.ToUIPosition()) / 2f, this.lastFrameDragCellsDrawn, Color.white);
					}
					return;
				}
			}
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x0013BAAC File Offset: 0x00139CAC
		public void UpdateCellBuffer()
		{
			ValueTuple<IntVec3, IntVec3> currentBoundary = this.GetCurrentBoundary();
			IntVec3 item = currentBoundary.Item1;
			IntVec3 item2 = currentBoundary.Item2;
			if (this.lastStart == item && this.lastEnd == item2 && this.lastStyleDef == this.SelStyle && this.lastTick == GenTicks.TicksGame)
			{
				return;
			}
			this.buffer.Clear();
			this.SelStyle.DrawStyleWorker.Update(item, item2, this.buffer);
			this.lastStart = item;
			this.lastEnd = item2;
			this.lastStyleDef = this.SelStyle;
			this.lastTick = GenTicks.TicksGame;
			if (!this.SelStyle.DrawStyleWorker.CanHaveDuplicates)
			{
				return;
			}
			for (int i = this.buffer.Count - 1; i >= 0; i--)
			{
				for (int j = 0; j < i; j++)
				{
					if (this.buffer[i] == this.buffer[j])
					{
						this.buffer.RemoveAt(i);
						break;
					}
				}
			}
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x0013BBB0 File Offset: 0x00139DB0
		[return: TupleElementNames(new string[]
		{
			"start",
			"end"
		})]
		private ValueTuple<IntVec3, IntVec3> GetCurrentBoundary()
		{
			IntVec3 intVec = this.startDragCell;
			IntVec3 intVec2 = UI.MouseCell();
			if (this.SelStyle.DrawStyleWorker.SingleCell)
			{
				return new ValueTuple<IntVec3, IntVec3>(intVec2, intVec2);
			}
			if (KeyBindingDefOf.Designator_ShapeSnap.IsDown && this.SelStyle.canSnap)
			{
				IntVec3 intVec3 = intVec2 - intVec;
				int num = (Mathf.Abs(intVec3.x) >= Mathf.Abs(intVec3.z)) ? intVec3.x : intVec3.z;
				intVec2.x = intVec.x + ((!Mathf.Approximately(Mathf.Sign((float)intVec3.x), Mathf.Sign((float)num))) ? (-num) : num);
				intVec2.z = intVec.z + ((!Mathf.Approximately(Mathf.Sign((float)intVec3.z), Mathf.Sign((float)num))) ? (-num) : num);
			}
			return new ValueTuple<IntVec3, IntVec3>(intVec, intVec2);
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x0013BC98 File Offset: 0x00139E98
		public void UpdateDragCellsIfNeeded()
		{
			if (Time.frameCount == this.lastUpdateFrame && this.lastStyle == this.SelStyle)
			{
				return;
			}
			this.lastUpdateFrame = Time.frameCount;
			this.dragCells.Clear();
			this.failureReasonInt = null;
			this.lastStyle = this.SelStyle;
			if (this.SelStyle == null)
			{
				return;
			}
			this.UpdateCellBuffer();
			foreach (IntVec3 intVec in this.buffer)
			{
				this.TryAddDragCell(new IntVec3(intVec.x, this.startDragCell.y, intVec.z));
			}
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x0013BD5C File Offset: 0x00139F5C
		private void TryAddDragCell(IntVec3 c)
		{
			AcceptanceReport acceptanceReport = this.SelDes.CanDesignateCell(c);
			if (acceptanceReport.Accepted)
			{
				this.dragCells.Add(c);
				return;
			}
			if (!acceptanceReport.Reason.NullOrEmpty())
			{
				this.failureReasonInt = acceptanceReport.Reason;
			}
		}

		// Token: 0x04002B47 RID: 11079
		private bool dragging;

		// Token: 0x04002B48 RID: 11080
		private IntVec3 startDragCell;

		// Token: 0x04002B49 RID: 11081
		private int lastFrameDragCellsDrawn;

		// Token: 0x04002B4A RID: 11082
		private Sustainer sustainer;

		// Token: 0x04002B4B RID: 11083
		private float lastDragRealTime = -1000f;

		// Token: 0x04002B4C RID: 11084
		private readonly List<IntVec3> buffer = new List<IntVec3>();

		// Token: 0x04002B4D RID: 11085
		private readonly List<IntVec3> dragCells = new List<IntVec3>();

		// Token: 0x04002B4E RID: 11086
		private string failureReasonInt;

		// Token: 0x04002B4F RID: 11087
		private int lastUpdateFrame = -1;

		// Token: 0x04002B50 RID: 11088
		private static readonly Texture2D OutlineTex = SolidColorMaterials.NewSolidColorTexture(new Color32(109, 139, 79, 100));

		// Token: 0x04002B51 RID: 11089
		private const string TimeSinceDragParam = "TimeSinceDrag";

		// Token: 0x04002B52 RID: 11090
		private readonly List<IntVec3> tmpHighlightCells = new List<IntVec3>();

		// Token: 0x04002B53 RID: 11091
		private int numSelectedCells;

		// Token: 0x04002B54 RID: 11092
		private IntVec3 lastStart;

		// Token: 0x04002B55 RID: 11093
		private IntVec3 lastEnd;

		// Token: 0x04002B56 RID: 11094
		private DrawStyleDef lastStyleDef;

		// Token: 0x04002B57 RID: 11095
		private int lastTick;

		// Token: 0x04002B58 RID: 11096
		private DrawStyleDef lastStyle;
	}
}
