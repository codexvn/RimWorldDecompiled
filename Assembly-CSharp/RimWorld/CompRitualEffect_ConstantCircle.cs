using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027F0 RID: 10224
	public class CompRitualEffect_ConstantCircle : CompRitualEffect_Constant
	{
		// Token: 0x0600DFD3 RID: 57299 RVA: 0x001E5B28 File Offset: 0x001E3D28
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x17002229 RID: 8745
		// (get) Token: 0x0600DFD4 RID: 57300 RVA: 0x0041BB56 File Offset: 0x00419D56
		protected CompProperties_RitualEffectConstantCircle Props
		{
			get
			{
				return (CompProperties_RitualEffectConstantCircle)this.props;
			}
		}

		// Token: 0x0600DFD5 RID: 57301 RVA: 0x0041BB63 File Offset: 0x00419D63
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
			this.Spawn(ritual);
		}

		// Token: 0x0600DFD6 RID: 57302 RVA: 0x0041BB74 File Offset: 0x00419D74
		protected override void Spawn(LordJob_Ritual ritual)
		{
			float num = 360f / (float)this.Props.numCopies;
			for (int i = 0; i < this.Props.numCopies; i++)
			{
				Vector3 a = Quaternion.AngleAxis(num * (float)i, Vector3.up) * Vector3.forward;
				Mote mote = this.SpawnMote(ritual, new Vector3?(ritual.selectedTarget.Cell.ToVector3Shifted() + a * this.Props.radius));
				if (mote != null)
				{
					this.parent.AddMoteToMaintain(mote);
					if (this.props.colorOverride != null)
					{
						mote.instanceColor = this.props.colorOverride.Value;
					}
					else
					{
						mote.instanceColor = this.parent.def.tintColor;
					}
				}
			}
			this.spawned = true;
		}
	}
}
