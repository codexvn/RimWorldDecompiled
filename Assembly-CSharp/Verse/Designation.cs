using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003E1 RID: 993
	public class Designation : IExposable
	{
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001960 RID: 6496 RVA: 0x000800A0 File Offset: 0x0007E2A0
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001961 RID: 6497 RVA: 0x000800AD File Offset: 0x0007E2AD
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x000800B8 File Offset: 0x0007E2B8
		public Material IconMat
		{
			get
			{
				if (this.cachedMaterial == null)
				{
					if (this.colorDef != null)
					{
						this.cachedMaterial = new Material(this.def.iconMat);
						this.cachedMaterial.color = this.colorDef.color;
					}
					else
					{
						this.cachedMaterial = this.def.iconMat;
					}
				}
				return this.cachedMaterial;
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x000024DF File Offset: 0x000006DF
		public Designation()
		{
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x00080120 File Offset: 0x0007E320
		public Designation(LocalTargetInfo target, DesignationDef def, ColorDef colorDef = null)
		{
			this.target = target;
			this.def = def;
			this.colorDef = colorDef;
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x00080140 File Offset: 0x0007E340
		public void ExposeData()
		{
			Scribe_Defs.Look<DesignationDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Defs.Look<ColorDef>(ref this.colorDef, "colorDef");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == DesignationDefOf.Haul && !this.target.HasThing)
			{
				Log.Error("Haul designation has no target! Deleting.");
				this.Delete();
			}
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x000801AF File Offset: 0x0007E3AF
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x000801D9 File Offset: 0x0007E3D9
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x00080210 File Offset: 0x0007E410
		public Vector3 DrawLoc()
		{
			if (this.target.HasThing)
			{
				Vector3 vector = this.target.Thing.DrawPos;
				vector.y = this.DesignationDrawAltitude;
				if (this.target.Thing.def.building != null && this.target.Thing.def.building.isAttachment)
				{
					vector += (this.target.Thing.Rotation.AsVector2 * 0.5f).ToVector3();
				}
				return vector;
			}
			return this.target.Cell.ToVector3ShiftedWithAltitude(this.DesignationDrawAltitude);
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000802C7 File Offset: 0x0007E4C7
		public virtual void DesignationDraw()
		{
			if (this.target.HasThing && !this.target.Thing.Spawned)
			{
				return;
			}
			Graphics.DrawMesh(MeshPool.plane10, this.DrawLoc(), Quaternion.identity, this.IconMat, 0);
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x00080305 File Offset: 0x0007E505
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x00080318 File Offset: 0x0007E518
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			array[1] = this.def.defName;
			array[2] = " target=";
			int num = 3;
			LocalTargetInfo localTargetInfo = this.target;
			array[num] = localTargetInfo.ToString();
			array[4] = ")";
			return string.Format(string.Concat(array), Array.Empty<object>());
		}

		// Token: 0x04001668 RID: 5736
		public DesignationManager designationManager;

		// Token: 0x04001669 RID: 5737
		public DesignationDef def;

		// Token: 0x0400166A RID: 5738
		public LocalTargetInfo target;

		// Token: 0x0400166B RID: 5739
		public ColorDef colorDef;

		// Token: 0x0400166C RID: 5740
		[Unsaved(false)]
		private Material cachedMaterial;

		// Token: 0x0400166D RID: 5741
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}
