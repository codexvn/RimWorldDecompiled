using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

[StaticConstructorOnStartup]
public class SectionLayer_PollutionCloud : SectionLayer_Gas
{
	[CompilerGenerated]
	private sealed class _003CAffectedCells_003Ed__23 : IEnumerable<IntVec3>, IEnumerable, IEnumerator<IntVec3>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private IntVec3 _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SectionLayer_PollutionCloud _003C_003E4__this;

		private CellRect rect;

		public CellRect _003C_003E3__rect;

		private float _003CnumCellsToCover_003E5__2;

		private int _003Ci_003E5__3;

		IntVec3 IEnumerator<IntVec3>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CAffectedCells_003Ed__23(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			SectionLayer_PollutionCloud sectionLayer_PollutionCloud = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_018f;
			}
			_003C_003E1__state = -1;
			sectionLayer_PollutionCloud.cellsTmp.Clear();
			int num2 = 0;
			foreach (IntVec3 cell in rect.Cells)
			{
				if (sectionLayer_PollutionCloud.Map.pollutionGrid.IsPolluted(cell))
				{
					num2++;
				}
			}
			if ((float)num2 / (float)rect.Area < sectionLayer_PollutionCloud.MinPercentage)
			{
				return false;
			}
			_003CnumCellsToCover_003E5__2 = NumCellsPerSectionPollutionLevel.Evaluate(num2);
			_003Ci_003E5__3 = 0;
			goto IL_01a1;
			IL_01a1:
			if ((float)_003Ci_003E5__3 < _003CnumCellsToCover_003E5__2)
			{
				for (int i = 0; i < sectionLayer_PollutionCloud.AttemptsPerCell; i++)
				{
					IntVec3 randomCell = rect.RandomCell;
					if (!sectionLayer_PollutionCloud.Map.pollutionGrid.IsPolluted(randomCell) || sectionLayer_PollutionCloud.Map.terrainGrid.TerrainAt(randomCell).IsWater)
					{
						continue;
					}
					bool flag = !sectionLayer_PollutionCloud.cellsTmp.Contains(randomCell);
					if (flag)
					{
						foreach (IntVec3 item in sectionLayer_PollutionCloud.cellsTmp)
						{
							if (!item.InHorDistOf(randomCell, sectionLayer_PollutionCloud.MinCellDistance))
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						sectionLayer_PollutionCloud.cellsTmp.Add(randomCell);
						_003C_003E2__current = randomCell;
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_018f;
			}
			return false;
			IL_018f:
			_003Ci_003E5__3++;
			goto IL_01a1;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			_003CAffectedCells_003Ed__23 _003CAffectedCells_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAffectedCells_003Ed__ = this;
			}
			else
			{
				_003CAffectedCells_003Ed__ = new _003CAffectedCells_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAffectedCells_003Ed__.rect = _003C_003E3__rect;
			return _003CAffectedCells_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<IntVec3>)this).GetEnumerator();
		}
	}

	private static Material matCached;

	private float MinCellDistance = 5f;

	private int AttemptsPerCell = 15;

	private float MinPercentage = 0.35f;

	private static readonly SimpleCurve NumCellsPerSectionPollutionLevel = new SimpleCurve
	{
		new CurvePoint(10f, 3f),
		new CurvePoint(20f, 6f)
	};

	private static readonly Vector2 FadeTexScrollSpeed = new Vector2(0.035f, 0.0125f);

	private static readonly Vector2 FadeTexScale = new Vector2(0.15f, 0.15f);

	private static readonly CachedTexture PollutionTex = new CachedTexture("Other/PollutionCloud");

	private static readonly CachedTexture PollutionFadeTex = new CachedTexture("Other/PollutionCloudFade");

	private static readonly Color SnowPollutionCloudColor = new Color(1f, 1f, 1f, 0.66f);

	private const float SnowPollutionColorThreshold = 0.4f;

	private List<IntVec3> cellsTmp = new List<IntVec3>();

	protected override FloatRange VertexScaleOffsetRange => new FloatRange(7f, 11f);

	protected override FloatRange VertexPositionOffsetRange => new FloatRange(-1f, 1f);

	public override Material Mat
	{
		get
		{
			if (matCached == null)
			{
				matCached = MaterialPool.MatFrom(PollutionTex.Texture, ShaderDatabase.PollutionCloud, Color.white, 3000);
				matCached.SetTexture(ShaderPropertyIDs.FadeTex, PollutionFadeTex.Texture);
				matCached.SetVector(ShaderPropertyIDs.TexScrollSpeed, FadeTexScrollSpeed);
				matCached.SetVector(ShaderPropertyIDs.TexScale, FadeTexScale);
			}
			return matCached;
		}
	}

	public override bool Visible
	{
		get
		{
			if (base.Visible)
			{
				return ModsConfig.BiotechActive;
			}
			return false;
		}
	}

	public SectionLayer_PollutionCloud(Section section)
		: base(section)
	{
		relevantChangeTypes = MapMeshFlagDefOf.Pollution;
	}

	public override Color ColorAt(IntVec3 cell)
	{
		float depth = base.Map.snowGrid.GetDepth(cell);
		cell.GetSandDepth(base.Map);
		TerrainDef terrainDef = base.Map.terrainGrid.TerrainAt(cell);
		if (!(depth < 0.4f))
		{
			return SnowPollutionCloudColor;
		}
		return terrainDef.pollutionCloudColor;
	}

	public override void Regenerate()
	{
		ClearSubMeshes(MeshParts.All);
		LayerSubMesh subMesh = GetSubMesh(Mat);
		float altitude = AltitudeLayer.Gas.AltitudeFor();
		int num = section.botLeft.x;
		foreach (IntVec3 item in AffectedCells(section.CellRect))
		{
			int count = subMesh.verts.Count;
			AddCell(item, num, count, subMesh, altitude);
			num++;
		}
		if (subMesh.verts.Count > 0)
		{
			subMesh.FinalizeMesh(MeshParts.All);
		}
	}

	[IteratorStateMachine(typeof(_003CAffectedCells_003Ed__23))]
	private IEnumerable<IntVec3> AffectedCells(CellRect rect)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAffectedCells_003Ed__23(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__rect = rect
		};
	}
}
