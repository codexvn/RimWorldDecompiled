using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002971 RID: 10609
	[StaticConstructorOnStartup]
	public class OverlayDrawer
	{
		// Token: 0x0600E8E9 RID: 59625 RVA: 0x0043CDFC File Offset: 0x0043AFFC
		public ThingOverlaysHandle GetOverlaysHandle(Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			ThingOverlaysHandle thingOverlaysHandle;
			if (!this.overlayHandles.TryGetValue(thing, out thingOverlaysHandle))
			{
				thingOverlaysHandle = new ThingOverlaysHandle(this, thing);
				this.overlayHandles.Add(thing, thingOverlaysHandle);
			}
			return thingOverlaysHandle;
		}

		// Token: 0x0600E8EA RID: 59626 RVA: 0x0043CE3C File Offset: 0x0043B03C
		public void DisposeHandle(Thing thing)
		{
			ThingOverlaysHandle thingOverlaysHandle;
			if (this.overlayHandles.TryGetValue(thing, out thingOverlaysHandle))
			{
				thingOverlaysHandle.Dispose();
			}
			this.overlayHandles.Remove(thing);
		}

		// Token: 0x0600E8EB RID: 59627 RVA: 0x0043CE6C File Offset: 0x0043B06C
		public OverlayHandle Enable(Thing thing, OverlayTypes types)
		{
			return this.GetOverlaysHandle(thing).Enable(types);
		}

		// Token: 0x0600E8EC RID: 59628 RVA: 0x0043CE7B File Offset: 0x0043B07B
		public void Disable(Thing thing, ref OverlayHandle? handle)
		{
			this.GetOverlaysHandle(thing).Disable(ref handle);
		}

		// Token: 0x0600E8ED RID: 59629 RVA: 0x0043CE8C File Offset: 0x0043B08C
		public void DrawOverlay(Thing t, OverlayTypes overlayType)
		{
			if (overlayType == OverlayTypes.None || WorldComponent_GravshipController.CutsceneInProgress)
			{
				return;
			}
			OverlayTypes overlayTypes;
			if (this.overlaysToDraw.TryGetValue(t, out overlayTypes))
			{
				this.overlaysToDraw[t] = (overlayTypes | overlayType);
				return;
			}
			this.overlaysToDraw.Add(t, overlayType);
		}

		// Token: 0x0600E8EE RID: 59630 RVA: 0x0043CED4 File Offset: 0x0043B0D4
		public void DrawAllOverlays()
		{
			if (WorldComponent_GravshipController.CutsceneInProgress)
			{
				return;
			}
			try
			{
				foreach (KeyValuePair<Thing, ThingOverlaysHandle> keyValuePair in this.overlayHandles)
				{
					if (!keyValuePair.Key.Fogged())
					{
						this.DrawOverlay(keyValuePair.Key, keyValuePair.Value.OverlayTypes);
					}
				}
				foreach (KeyValuePair<Thing, OverlayTypes> keyValuePair2 in this.overlaysToDraw)
				{
					this.curOffset = Vector3.zero;
					Thing key = keyValuePair2.Key;
					OverlayTypes value = keyValuePair2.Value;
					if ((value & OverlayTypes.BurningWick) != OverlayTypes.None)
					{
						this.RenderBurningWick(key);
					}
					else
					{
						OverlayTypes overlayTypes = OverlayTypes.NeedsPower | OverlayTypes.PowerOff;
						int bitCountOf = Gen.GetBitCountOf((long)(value & overlayTypes));
						float num = this.StackOffsetFor(key);
						switch (bitCountOf)
						{
						case 1:
							this.curOffset = Vector3.zero;
							break;
						case 2:
							this.curOffset = new Vector3(-0.5f * num, 0f, 0f);
							break;
						case 3:
							this.curOffset = new Vector3(-1.5f * num, 0f, 0f);
							break;
						}
						if ((value & OverlayTypes.NeedsPower) != OverlayTypes.None)
						{
							this.RenderNeedsPowerOverlay(key);
						}
						if ((value & OverlayTypes.PowerOff) != OverlayTypes.None)
						{
							this.RenderPowerOffOverlay(key);
						}
						if ((value & OverlayTypes.BrokenDown) != OverlayTypes.None)
						{
							this.RenderBrokenDownOverlay(key);
						}
						if ((value & OverlayTypes.OutOfFuel) != OverlayTypes.None)
						{
							this.RenderOutOfFuelOverlay(key);
						}
					}
					if ((value & OverlayTypes.ForbiddenBig) != OverlayTypes.None)
					{
						this.RenderForbiddenBigOverlay(key);
					}
					if ((value & OverlayTypes.Forbidden) != OverlayTypes.None)
					{
						this.RenderForbiddenOverlay(key);
					}
					if ((value & OverlayTypes.ForbiddenRefuel) != OverlayTypes.None)
					{
						this.RenderForbiddenRefuelOverlay(key);
					}
					if ((value & OverlayTypes.QuestionMark) != OverlayTypes.None)
					{
						this.RenderQuestionMarkOverlay(key);
					}
					if ((value & OverlayTypes.SelfShutdown) != OverlayTypes.None && ModsConfig.BiotechActive)
					{
						this.RenderRechargineOverlay(key);
					}
					if ((value & OverlayTypes.ForbiddenAtomizer) != OverlayTypes.None && ModsConfig.BiotechActive)
					{
						this.RenderForbiddenAtomizerOverlay(key);
					}
				}
			}
			finally
			{
				this.overlaysToDraw.Clear();
			}
			this.drawBatch.Flush(true);
		}

		// Token: 0x0600E8EF RID: 59631 RVA: 0x0043D134 File Offset: 0x0043B334
		private float StackOffsetFor(Thing t)
		{
			return (float)t.RotatedSize.x * 0.25f;
		}

		// Token: 0x0600E8F0 RID: 59632 RVA: 0x0043D148 File Offset: 0x0043B348
		private void RenderNeedsPowerOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.NeedsPowerMat, 2, true);
		}

		// Token: 0x0600E8F1 RID: 59633 RVA: 0x0043D158 File Offset: 0x0043B358
		private void RenderPowerOffOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.PowerOffMat, 3, true);
		}

		// Token: 0x0600E8F2 RID: 59634 RVA: 0x0043D168 File Offset: 0x0043B368
		private void RenderBrokenDownOverlay(Thing t)
		{
			this.RenderPulsingOverlay(t, OverlayDrawer.BrokenDownMat, 4, true);
		}

		// Token: 0x0600E8F3 RID: 59635 RVA: 0x0043D178 File Offset: 0x0043B378
		private void RenderRechargineOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.21951221f;
			this.RenderPulsingOverlayInternal(t, OverlayDrawer.SelfShutdownMat, drawPos, MeshPool.plane05);
		}

		// Token: 0x0600E8F4 RID: 59636 RVA: 0x0043D1B0 File Offset: 0x0043B3B0
		private void RenderOutOfFuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material mat = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlay, Color.white);
			this.RenderPulsingOverlay(t, mat, 5, false);
			this.RenderPulsingOverlay(t, OverlayDrawer.OutOfFuelMat, 6, true);
		}

		// Token: 0x0600E8F5 RID: 59637 RVA: 0x0043D208 File Offset: 0x0043B408
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, bool incrementOffset = true)
		{
			Mesh plane = MeshPool.plane08;
			this.RenderPulsingOverlay(thing, mat, altInd, plane, incrementOffset);
		}

		// Token: 0x0600E8F6 RID: 59638 RVA: 0x0043D228 File Offset: 0x0043B428
		private void RenderPulsingOverlay(Thing thing, Material mat, int altInd, Mesh mesh, bool incrementOffset = true)
		{
			Vector3 vector = thing.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.03658537f * (float)altInd;
			vector += this.curOffset;
			if (thing.def.building != null && thing.def.building.isAttachment)
			{
				vector += (thing.Rotation.AsVector2 * 0.5f).ToVector3();
			}
			vector.y = Mathf.Min(vector.y, Find.Camera.transform.position.y - 0.1f);
			if (incrementOffset)
			{
				this.curOffset.x = this.curOffset.x + this.StackOffsetFor(thing);
			}
			this.RenderPulsingOverlayInternal(thing, mat, vector, mesh);
		}

		// Token: 0x0600E8F7 RID: 59639 RVA: 0x0043D2F4 File Offset: 0x0043B4F4
		private void RenderPulsingOverlayInternal(Thing thing, Material mat, Vector3 drawPos, Mesh mesh)
		{
			float num = ((float)Math.Sin((double)((Time.realtimeSinceStartup + 397f * (float)(thing.thingIDNumber % 571)) * 4f)) + 1f) * 0.5f;
			num = 0.3f + num * 0.7f;
			Material material = FadedMaterialPool.FadedVersionOf(mat, num);
			this.drawBatch.DrawMesh(mesh, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), material, 0, true);
		}

		// Token: 0x0600E8F8 RID: 59640 RVA: 0x0043D36C File Offset: 0x0043B56C
		private void RenderForbiddenRefuelOverlay(Thing t)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			Material material = MaterialPool.MatFrom((compRefuelable != null) ? compRefuelable.Props.FuelIcon : ThingDefOf.Chemfuel.uiIcon, ShaderDatabase.MetaOverlayDesaturated, Color.white);
			Vector3 vector = t.TrueCenter();
			vector.y = OverlayDrawer.BaseAlt + 0.18292683f;
			new Vector3(vector.x, vector.y + 0.03658537f, vector.z);
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), material, 0, true);
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x0600E8F9 RID: 59641 RVA: 0x0043D42C File Offset: 0x0043B62C
		private void RenderForbiddenAtomizerOverlay(Thing t)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			t.TryGetComp<CompAtomizer>();
			Material material = MaterialPool.MatFrom(ThingDefOf.Wastepack.uiIcon, ShaderDatabase.MetaOverlayDesaturated, Color.white);
			Vector3 pos = t.TrueCenter();
			pos.y = OverlayDrawer.BaseAlt + 0.18292683f;
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one), material, 0, true);
			this.drawBatch.DrawMesh(MeshPool.plane08, Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x0600E8FA RID: 59642 RVA: 0x0043D4C8 File Offset: 0x0043B6C8
		private void RenderForbiddenOverlay(Thing t)
		{
			Vector3 vector = t.DrawPos;
			ThingDef thingDef = t.def.entityDefToBuild as ThingDef;
			bool? flag;
			if (thingDef == null)
			{
				flag = null;
			}
			else
			{
				BuildingProperties building = thingDef.building;
				flag = ((building != null) ? new bool?(building.isAttachment) : null);
			}
			bool? flag2 = flag;
			bool flag3;
			if (flag2 == null)
			{
				BuildingProperties building2 = t.def.building;
				flag3 = (building2 != null && building2.isAttachment);
			}
			else
			{
				flag3 = flag2.GetValueOrDefault();
			}
			if (flag3)
			{
				vector += (t.Rotation.AsVector2 * 0.5f).ToVector3();
			}
			if (t.RotatedSize.z == 1)
			{
				vector.z -= OverlayDrawer.SingleCellForbiddenOffset;
			}
			else
			{
				vector.z -= (float)t.RotatedSize.z * 0.3f;
			}
			vector.y = OverlayDrawer.BaseAlt + 0.14634147f;
			this.drawBatch.DrawMesh(MeshPool.plane05, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x0600E8FB RID: 59643 RVA: 0x0043D5E0 File Offset: 0x0043B7E0
		private void RenderForbiddenBigOverlay(Thing t)
		{
			Vector3 vector = t.DrawPos;
			ThingDef thingDef = t.def.entityDefToBuild as ThingDef;
			bool? flag;
			if (thingDef == null)
			{
				flag = null;
			}
			else
			{
				BuildingProperties building = thingDef.building;
				flag = ((building != null) ? new bool?(building.isAttachment) : null);
			}
			bool? flag2 = flag;
			bool flag3;
			if (flag2 == null)
			{
				BuildingProperties building2 = t.def.building;
				flag3 = (building2 != null && building2.isAttachment);
			}
			else
			{
				flag3 = flag2.GetValueOrDefault();
			}
			if (flag3)
			{
				vector += (t.Rotation.AsVector2 * 0.5f).ToVector3();
			}
			vector.y = OverlayDrawer.BaseAlt + 0.14634147f;
			this.drawBatch.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(vector, Quaternion.identity, Vector3.one), OverlayDrawer.ForbiddenMat, 0, true);
		}

		// Token: 0x0600E8FC RID: 59644 RVA: 0x0043D6BC File Offset: 0x0043B8BC
		private void RenderBurningWick(Thing parent)
		{
			Material material;
			if ((parent.thingIDNumber + Find.TickManager.TicksGame) % 6 < 3)
			{
				material = OverlayDrawer.WickMaterialA;
			}
			else
			{
				material = OverlayDrawer.WickMaterialB;
			}
			Vector3 drawPos = parent.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.18292683f;
			this.drawBatch.DrawMesh(MeshPool.plane20, Matrix4x4.TRS(drawPos, Quaternion.identity, Vector3.one), material, 0, true);
		}

		// Token: 0x0600E8FD RID: 59645 RVA: 0x0043D72C File Offset: 0x0043B92C
		private void RenderQuestionMarkOverlay(Thing t)
		{
			Vector3 drawPos = t.DrawPos;
			drawPos.y = OverlayDrawer.BaseAlt + 0.21951221f;
			if (t is Pawn)
			{
				drawPos.x += (float)t.def.size.x - 0.52f;
				drawPos.z += (float)t.def.size.z - 0.45f;
			}
			this.RenderPulsingOverlayInternal(t, OverlayDrawer.QuestionMarkMat, drawPos, MeshPool.plane05);
		}

		// Token: 0x04009B27 RID: 39719
		private Dictionary<Thing, OverlayTypes> overlaysToDraw = new Dictionary<Thing, OverlayTypes>();

		// Token: 0x04009B28 RID: 39720
		private Dictionary<Thing, ThingOverlaysHandle> overlayHandles = new Dictionary<Thing, ThingOverlaysHandle>();

		// Token: 0x04009B29 RID: 39721
		private Vector3 curOffset;

		// Token: 0x04009B2A RID: 39722
		private DrawBatch drawBatch = new DrawBatch();

		// Token: 0x04009B2B RID: 39723
		private static readonly Material ForbiddenMat = MaterialPool.MatFrom("Things/Special/ForbiddenOverlay", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B2C RID: 39724
		private static readonly Material NeedsPowerMat = MaterialPool.MatFrom("UI/Overlays/NeedsPower", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B2D RID: 39725
		private static readonly Material PowerOffMat = MaterialPool.MatFrom("UI/Overlays/PowerOff", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B2E RID: 39726
		private static readonly Material QuestionMarkMat = MaterialPool.MatFrom("UI/Overlays/QuestionMark", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B2F RID: 39727
		private static readonly Material BrokenDownMat = MaterialPool.MatFrom("UI/Overlays/BrokenDown", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B30 RID: 39728
		private static readonly Material OutOfFuelMat = MaterialPool.MatFrom("UI/Overlays/OutOfFuel", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B31 RID: 39729
		private static readonly Material WickMaterialA = MaterialPool.MatFrom("Things/Special/BurningWickA", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B32 RID: 39730
		private static readonly Material WickMaterialB = MaterialPool.MatFrom("Things/Special/BurningWickB", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B33 RID: 39731
		private static readonly Material SelfShutdownMat = MaterialPool.MatFrom("UI/Overlays/SelfShutdown", ShaderDatabase.MetaOverlay);

		// Token: 0x04009B34 RID: 39732
		private const int AltitudeIndex_Forbidden = 4;

		// Token: 0x04009B35 RID: 39733
		private const int AltitudeIndex_BurningWick = 5;

		// Token: 0x04009B36 RID: 39734
		private const int AltitudeIndex_QuestionMark = 6;

		// Token: 0x04009B37 RID: 39735
		private static float SingleCellForbiddenOffset = 0.3f;

		// Token: 0x04009B38 RID: 39736
		public const float PulseFrequency = 4f;

		// Token: 0x04009B39 RID: 39737
		public const float PulseAmplitude = 0.7f;

		// Token: 0x04009B3A RID: 39738
		private static readonly float BaseAlt = AltitudeLayer.MetaOverlays.AltitudeFor();

		// Token: 0x04009B3B RID: 39739
		private const float StackOffsetMultipiler = 0.25f;
	}
}
