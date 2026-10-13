using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using UnityEngine.Rendering;

namespace Verse
{
	// Token: 0x020003E2 RID: 994
	public sealed class DesignationManager : IExposable
	{
		// Token: 0x0600196C RID: 6508 RVA: 0x00080378 File Offset: 0x0007E578
		public DesignationManager(Map map)
		{
			this.map = map;
			this.designationsAtCell = new List<Designation>[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x0600196D RID: 6509 RVA: 0x000803E0 File Offset: 0x0007E5E0
		public List<Designation> AllDesignations
		{
			get
			{
				DesignationManager.tmpDesignations.Clear();
				foreach (KeyValuePair<DesignationDef, List<Designation>> keyValuePair in this.designationsByDef)
				{
					DesignationManager.tmpDesignations.AddRange(keyValuePair.Value);
				}
				return DesignationManager.tmpDesignations;
			}
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x00080448 File Offset: 0x0007E648
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.saveDesignations.Clear();
				this.saveDesignations.AddRange(this.AllDesignations);
			}
			Scribe_Collections.Look<Designation>(ref this.saveDesignations, "allDesignations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.saveDesignations.RemoveAll((Designation x) => x == null) != 0)
				{
					Log.Warning("Some designations were null after loading.");
				}
				if (this.saveDesignations.RemoveAll((Designation x) => x.def == null) != 0)
				{
					Log.Warning("Some designations had null def after loading.");
				}
				foreach (Designation designation in this.saveDesignations)
				{
					designation.designationManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Plan plan = null;
				int i = this.saveDesignations.Count - 1;
				while (i >= 0)
				{
					TargetType targetType = this.saveDesignations[i].def.targetType;
					if (targetType != TargetType.Thing)
					{
						if (targetType != TargetType.Cell)
						{
							goto IL_19E;
						}
						if (this.saveDesignations[i].target.Cell.IsValid)
						{
							goto IL_19E;
						}
						Log.Error(string.Format("Cell-needing designation {0} had no cell target. Removing...", this.saveDesignations[i]));
					}
					else
					{
						if (this.saveDesignations[i].target.HasThing)
						{
							goto IL_19E;
						}
						Log.Error(string.Format("Thing-needing designation {0} had no thing target. Removing...", this.saveDesignations[i]));
					}
					IL_22E:
					i--;
					continue;
					IL_19E:
					if (this.saveDesignations[i].def == DesignationDefOf.Plan)
					{
						Map map = this.map;
						if (map.planManager == null)
						{
							map.planManager = new PlanManager(this.map);
						}
						if (plan == null)
						{
							plan = new Plan(ColorDefOf.PlanGray, this.map.planManager);
						}
						plan.AddCell(this.saveDesignations[i].target.Cell);
						this.saveDesignations.RemoveAt(i);
						goto IL_22E;
					}
					this.IndexDesignation(this.saveDesignations[i]);
					goto IL_22E;
				}
				if (plan != null)
				{
					plan.CheckContiguous();
				}
				this.saveDesignations.Clear();
			}
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x000806B4 File Offset: 0x0007E8B4
		public void DrawDesignations()
		{
			CellRect cellRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(3);
			if (this.tmpPropertyBlock == null)
			{
				this.tmpPropertyBlock = new MaterialPropertyBlock();
			}
			foreach (DesignationDef designationDef in DefDatabase<DesignationDef>.AllDefsListForReading)
			{
				if (designationDef.targetType == TargetType.Cell && designationDef.shouldBatchDraw && SystemInfo.supportsInstancing)
				{
					int count = this.designationsByDef[designationDef].Count;
					if (this.designationsByDef[designationDef].Count != 0)
					{
						this.CalculateCellDesignationDrawMatricies(designationDef);
						List<Matrix4x4[]> list = this.cellDesignationDrawMatricies[designationDef];
						designationDef.iconMat.enableInstancing = true;
						int num = count / 1023;
						for (int i = 0; i < num; i++)
						{
							Graphics.DrawMeshInstanced(MeshPool.plane10, 0, designationDef.iconMat, list[i], 1023, this.tmpPropertyBlock, ShadowCastingMode.Off, true, 0);
						}
						int num2 = count % 1023;
						if (num2 > 0)
						{
							Graphics.DrawMeshInstanced(MeshPool.plane10, 0, designationDef.iconMat, list[num], num2, this.tmpPropertyBlock, ShadowCastingMode.Off, true, 0);
						}
					}
				}
				else
				{
					foreach (Designation designation in this.designationsByDef[designationDef])
					{
						if ((!designation.target.HasThing || designation.target.Thing.Map == this.map) && cellRect.Contains(designation.target.Cell))
						{
							designation.DesignationDraw();
						}
					}
				}
			}
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x000808B4 File Offset: 0x0007EAB4
		private void DirtyCellDesignationsCache(DesignationDef def)
		{
			if (def.targetType != TargetType.Cell)
			{
				return;
			}
			this.designationMatriciesDirty[def] = true;
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x000808D0 File Offset: 0x0007EAD0
		public void AddDesignation(Designation newDes)
		{
			if (newDes.def.targetType == TargetType.Cell && this.DesignationAt(newDes.target.Cell, newDes.def) != null)
			{
				Log.Error(string.Format("Tried to double-add designation at location {0}", newDes.target));
				return;
			}
			if (newDes.def.targetType == TargetType.Thing && this.DesignationOn(newDes.target.Thing, newDes.def) != null)
			{
				Log.Error(string.Format("Tried to double-add designation on Thing {0}", newDes.target));
				return;
			}
			if (newDes.def.targetType == TargetType.Thing)
			{
				newDes.target.Thing.SetForbidden(false, false);
			}
			this.IndexDesignation(newDes);
			newDes.designationManager = this;
			newDes.Notify_Added();
			Map map = newDes.target.HasThing ? newDes.target.Thing.Map : this.map;
			if (map != null)
			{
				FleckMaker.ThrowMetaPuffs(newDes.target.ToTargetInfo(map));
			}
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x000809D0 File Offset: 0x0007EBD0
		private void IndexDesignation(Designation designation)
		{
			this.designationsByDef[designation.def].Add(designation);
			this.DirtyCellDesignationsCache(designation.def);
			if (designation.def.targetType == TargetType.Thing)
			{
				Thing thing = designation.target.Thing;
				if (!this.thingDesignations.ContainsKey(thing))
				{
					this.thingDesignations[thing] = new List<Designation>();
				}
				this.thingDesignations[thing].Add(designation);
				return;
			}
			if (designation.def.targetType != TargetType.Cell)
			{
				Log.Error(string.Format("Tried to index unexpected designation type: {0}", designation.def.targetType));
				return;
			}
			List<Designation> list;
			if (this.TryGetCellDesignations(designation.target.Cell, out list, true))
			{
				list.Add(designation);
				return;
			}
			Log.Error(string.Format("Tried to create cell target designation at invalid cell: {0}", designation.target.Cell));
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x00080AB8 File Offset: 0x0007ECB8
		private void CalculateCellDesignationDrawMatricies(DesignationDef def)
		{
			if (def.targetType != TargetType.Cell || !this.designationMatriciesDirty[def])
			{
				return;
			}
			List<Designation> list = this.designationsByDef[def];
			int count = list.Count;
			while (this.cellDesignationDrawMatricies[def].Count * 1023 < count)
			{
				this.cellDesignationDrawMatricies[def].Add(new Matrix4x4[1023]);
			}
			for (int i = 0; i < count; i++)
			{
				Designation designation = list[i];
				int index = i / 1023;
				this.cellDesignationDrawMatricies[def][index][i % 1023] = Matrix4x4.TRS(designation.DrawLoc(), Quaternion.identity, Vector3.one);
			}
			this.designationMatriciesDirty[def] = false;
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x00080B88 File Offset: 0x0007ED88
		public Designation DesignationOn(Thing t)
		{
			List<Designation> list;
			this.thingDesignations.TryGetValue(t, out list);
			if (list.NullOrEmpty<Designation>())
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x00080BB8 File Offset: 0x0007EDB8
		public Designation DesignationOn(Thing t, DesignationDef def)
		{
			if (def.targetType == TargetType.Cell)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by location only and you are trying to get one on a Thing.");
				return null;
			}
			List<Designation> list;
			this.thingDesignations.TryGetValue(t, out list);
			if (list.NullOrEmpty<Designation>())
			{
				return null;
			}
			foreach (Designation designation in list)
			{
				if (designation.def == def)
				{
					return designation;
				}
			}
			return null;
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x00080C50 File Offset: 0x0007EE50
		public Designation DesignationAt(IntVec3 c, DesignationDef def)
		{
			if (def.targetType == TargetType.Thing)
			{
				Log.Error("Designations of type " + def.defName + " are indexed by Thing only and you are trying to get one on a location.");
				return null;
			}
			List<Designation> list;
			if (this.TryGetCellDesignations(c, out list, false))
			{
				for (int i = 0; i < list.Count; i++)
				{
					Designation designation = list[i];
					if (designation.def == def)
					{
						return designation;
					}
				}
			}
			return null;
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x00080CB2 File Offset: 0x0007EEB2
		public List<Designation> AllDesignationsOn(Thing t)
		{
			if (!this.thingDesignations.ContainsKey(t))
			{
				return DesignationManager.EmptyList;
			}
			return this.thingDesignations[t];
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x00080CD4 File Offset: 0x0007EED4
		public List<Designation> AllDesignationsAt(IntVec3 c)
		{
			DesignationManager.tmpDesignations.Clear();
			List<Designation> collection;
			if (this.TryGetCellDesignations(c, out collection, false))
			{
				DesignationManager.tmpDesignations.AddRange(collection);
			}
			foreach (Thing t in this.map.thingGrid.ThingsListAt(c))
			{
				DesignationManager.tmpDesignations.AddRange(this.AllDesignationsOn(t));
			}
			return DesignationManager.tmpDesignations;
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x00080D64 File Offset: 0x0007EF64
		public bool HasMapDesignationAt(IntVec3 c)
		{
			List<Designation> list;
			return this.TryGetCellDesignations(c, out list, false) && !list.NullOrEmpty<Designation>();
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x00080D88 File Offset: 0x0007EF88
		public bool HasMapDesignationOn(Thing t)
		{
			return this.DesignationOn(t) != null;
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x00080D94 File Offset: 0x0007EF94
		public IEnumerable<Designation> SpawnedDesignationsOfDef(DesignationDef def)
		{
			foreach (Designation designation in this.designationsByDef[def])
			{
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					yield return designation;
				}
			}
			List<Designation>.Enumerator enumerator = default(List<Designation>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x00080DAC File Offset: 0x0007EFAC
		public bool AnySpawnedDesignationOfDef(DesignationDef def)
		{
			foreach (Designation designation in this.designationsByDef[def])
			{
				if (designation.def == def && (!designation.target.HasThing || designation.target.Thing.Map == this.map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x00080E34 File Offset: 0x0007F034
		private bool TryGetCellDesignations(IntVec3 cell, out List<Designation> foundDesignations, bool initializeIfNull = false)
		{
			int num = this.map.cellIndices.CellToIndex(cell);
			if (num < 0 || num >= this.designationsAtCell.Length)
			{
				foundDesignations = null;
				return false;
			}
			foundDesignations = this.designationsAtCell[num];
			if (foundDesignations == null)
			{
				if (initializeIfNull)
				{
					foundDesignations = new List<Designation>();
					this.designationsAtCell[num] = foundDesignations;
				}
				else
				{
					foundDesignations = DesignationManager.EmptyList;
				}
			}
			return true;
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x00080E94 File Offset: 0x0007F094
		public void RemoveDesignation(Designation des)
		{
			des.Notify_Removing();
			if (des.def.targetType == TargetType.Cell)
			{
				List<Designation> list;
				if (this.TryGetCellDesignations(des.target.Cell, out list, false))
				{
					list.Remove(des);
				}
				else
				{
					Log.Warning(string.Format("Tried to remove designation with target cell that couldn't be found in index: {0}", des.target.Cell));
				}
			}
			else if (des.def.targetType == TargetType.Thing)
			{
				Thing thing = des.target.Thing;
				if (this.thingDesignations.ContainsKey(thing))
				{
					List<Designation> list2 = this.thingDesignations[thing];
					list2.Remove(des);
					if (list2.Count == 0)
					{
						this.thingDesignations.Remove(des.target.Thing);
					}
				}
				else
				{
					Log.Warning("Tried to remove thing designation that wasn't indexed");
				}
			}
			else
			{
				Log.Error(string.Format("Tried to remove designation with unexpected type: {0}", des.def.targetType));
			}
			this.designationsByDef[des.def].Remove(des);
			this.DirtyCellDesignationsCache(des.def);
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x00080FA8 File Offset: 0x0007F1A8
		public void TryRemoveDesignation(IntVec3 c, DesignationDef def)
		{
			Designation designation = this.DesignationAt(c, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x00080FC8 File Offset: 0x0007F1C8
		public void RemoveAllDesignationsOn(Thing t, bool standardCanceling = false)
		{
			List<Designation> list = this.AllDesignationsOn(t);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Designation designation = list[i];
				if (!standardCanceling || designation.def.designateCancelable)
				{
					this.RemoveDesignation(list[i]);
				}
			}
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x00081018 File Offset: 0x0007F218
		public void TryRemoveDesignationOn(Thing t, DesignationDef def)
		{
			Designation designation = this.DesignationOn(t, def);
			if (designation != null)
			{
				this.RemoveDesignation(designation);
			}
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x00081038 File Offset: 0x0007F238
		public void RemoveAllDesignationsOfDef(DesignationDef def)
		{
			List<Designation> list = this.designationsByDef[def];
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.RemoveDesignation(list[i]);
			}
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x00081074 File Offset: 0x0007F274
		public void Notify_BuildingDespawned(Thing b)
		{
			CellRect cellRect = b.OccupiedRect();
			foreach (KeyValuePair<DesignationDef, List<Designation>> keyValuePair in this.designationsByDef)
			{
				if (keyValuePair.Key.removeIfBuildingDespawned)
				{
					List<Designation> value = keyValuePair.Value;
					for (int i = value.Count - 1; i >= 0; i--)
					{
						if (cellRect.Contains(value[i].target.Cell))
						{
							this.RemoveDesignation(value[i]);
						}
					}
				}
			}
		}

		// Token: 0x0400166E RID: 5742
		public readonly Map map;

		// Token: 0x0400166F RID: 5743
		public readonly DefMap<DesignationDef, List<Designation>> designationsByDef = new DefMap<DesignationDef, List<Designation>>();

		// Token: 0x04001670 RID: 5744
		private readonly DefMap<DesignationDef, List<Matrix4x4[]>> cellDesignationDrawMatricies = new DefMap<DesignationDef, List<Matrix4x4[]>>();

		// Token: 0x04001671 RID: 5745
		private readonly DefMap<DesignationDef, bool> designationMatriciesDirty = new DefMap<DesignationDef, bool>();

		// Token: 0x04001672 RID: 5746
		private readonly List<Designation>[] designationsAtCell;

		// Token: 0x04001673 RID: 5747
		private readonly Dictionary<Thing, List<Designation>> thingDesignations = new Dictionary<Thing, List<Designation>>();

		// Token: 0x04001674 RID: 5748
		private MaterialPropertyBlock tmpPropertyBlock;

		// Token: 0x04001675 RID: 5749
		private List<Designation> saveDesignations = new List<Designation>();

		// Token: 0x04001676 RID: 5750
		private static readonly List<Designation> EmptyList = new List<Designation>();

		// Token: 0x04001677 RID: 5751
		private static readonly List<Designation> tmpDesignations = new List<Designation>();
	}
}
