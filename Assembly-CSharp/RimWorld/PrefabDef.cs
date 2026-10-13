using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A7D RID: 6781
	public class PrefabDef : Def
	{
		// Token: 0x06009808 RID: 38920 RVA: 0x002C5CD2 File Offset: 0x002C3ED2
		[return: TupleElementNames(new string[]
		{
			"data",
			"cell"
		})]
		public IEnumerable<ValueTuple<PrefabThingData, IntVec3>> GetThings()
		{
			foreach (PrefabThingData data in this.things)
			{
				bool flag = false;
				if (!data.rects.NullOrEmpty<CellRect>())
				{
					foreach (CellRect cellRect in data.rects)
					{
						foreach (IntVec3 item in cellRect.Cells)
						{
							yield return new ValueTuple<PrefabThingData, IntVec3>(data, item);
							flag = true;
						}
						IEnumerator<IntVec3> enumerator3 = null;
					}
					List<CellRect>.Enumerator enumerator2 = default(List<CellRect>.Enumerator);
				}
				if (!data.positions.NullOrEmpty<IntVec3>())
				{
					foreach (IntVec3 item2 in data.positions)
					{
						yield return new ValueTuple<PrefabThingData, IntVec3>(data, item2);
						flag = true;
					}
					List<IntVec3>.Enumerator enumerator4 = default(List<IntVec3>.Enumerator);
				}
				if (!flag)
				{
					yield return new ValueTuple<PrefabThingData, IntVec3>(data, data.position);
				}
				data = null;
			}
			List<PrefabThingData>.Enumerator enumerator = default(List<PrefabThingData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06009809 RID: 38921 RVA: 0x002C5CE2 File Offset: 0x002C3EE2
		[return: TupleElementNames(new string[]
		{
			"data",
			"cell"
		})]
		public IEnumerable<ValueTuple<SubPrefabData, IntVec3>> GetPrefabs()
		{
			foreach (SubPrefabData data in this.prefabs)
			{
				if (!data.positions.NullOrEmpty<IntVec3>())
				{
					foreach (IntVec3 item in data.positions)
					{
						yield return new ValueTuple<SubPrefabData, IntVec3>(data, item);
					}
					List<IntVec3>.Enumerator enumerator2 = default(List<IntVec3>.Enumerator);
				}
				else
				{
					yield return new ValueTuple<SubPrefabData, IntVec3>(data, data.position);
				}
				data = null;
			}
			List<SubPrefabData>.Enumerator enumerator = default(List<SubPrefabData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600980A RID: 38922 RVA: 0x002C5CF2 File Offset: 0x002C3EF2
		[return: TupleElementNames(new string[]
		{
			"data",
			"cell"
		})]
		public IEnumerable<ValueTuple<PrefabTerrainData, IntVec3>> GetTerrain()
		{
			foreach (PrefabTerrainData data in this.terrain)
			{
				if (!data.rects.NullOrEmpty<CellRect>())
				{
					foreach (CellRect cellRect in data.rects)
					{
						foreach (IntVec3 item in cellRect.Cells)
						{
							yield return new ValueTuple<PrefabTerrainData, IntVec3>(data, item);
						}
						IEnumerator<IntVec3> enumerator3 = null;
					}
					List<CellRect>.Enumerator enumerator2 = default(List<CellRect>.Enumerator);
					data = null;
				}
			}
			List<PrefabTerrainData>.Enumerator enumerator = default(List<PrefabTerrainData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x040065D0 RID: 26064
		internal List<PrefabThingData> things = new List<PrefabThingData>();

		// Token: 0x040065D1 RID: 26065
		internal List<SubPrefabData> prefabs = new List<SubPrefabData>();

		// Token: 0x040065D2 RID: 26066
		internal List<PrefabTerrainData> terrain = new List<PrefabTerrainData>();

		// Token: 0x040065D3 RID: 26067
		public IntVec2 size;

		// Token: 0x040065D4 RID: 26068
		public RotEnum rotations = RotEnum.All;

		// Token: 0x040065D5 RID: 26069
		public bool edgeOnly;
	}
}
