using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200212F RID: 8495
	public class CompFlickable : ThingComp
	{
		// Token: 0x17001C23 RID: 7203
		// (get) Token: 0x0600B94A RID: 47434 RVA: 0x00357A03 File Offset: 0x00355C03
		private CompProperties_Flickable Props
		{
			get
			{
				return (CompProperties_Flickable)this.props;
			}
		}

		// Token: 0x17001C24 RID: 7204
		// (get) Token: 0x0600B94B RID: 47435 RVA: 0x00357A10 File Offset: 0x00355C10
		private Texture2D CommandTex
		{
			get
			{
				if (this.cachedCommandTex == null)
				{
					this.cachedCommandTex = ContentFinder<Texture2D>.Get(this.Props.commandTexture, true);
				}
				return this.cachedCommandTex;
			}
		}

		// Token: 0x17001C25 RID: 7205
		// (get) Token: 0x0600B94C RID: 47436 RVA: 0x00357A3D File Offset: 0x00355C3D
		// (set) Token: 0x0600B94D RID: 47437 RVA: 0x00357A48 File Offset: 0x00355C48
		public bool SwitchIsOn
		{
			get
			{
				return this.switchOnInt;
			}
			set
			{
				if (this.switchOnInt == value)
				{
					return;
				}
				this.switchOnInt = value;
				if (this.switchOnInt)
				{
					this.parent.BroadcastCompSignal("FlickedOn");
				}
				else
				{
					this.parent.BroadcastCompSignal("FlickedOff");
				}
				if (this.parent.Spawned)
				{
					this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Buildings | MapMeshFlagDefOf.Things);
				}
			}
		}

		// Token: 0x17001C26 RID: 7206
		// (get) Token: 0x0600B94E RID: 47438 RVA: 0x00357AD4 File Offset: 0x00355CD4
		public Graphic CurrentGraphic
		{
			get
			{
				if (this.SwitchIsOn)
				{
					return this.parent.DefaultGraphic;
				}
				if (this.offGraphic == null)
				{
					this.offGraphic = GraphicDatabase.Get(this.parent.def.graphicData.graphicClass, this.parent.def.graphicData.texPath + "_Off", this.parent.def.graphicData.shaderType.Shader, this.parent.def.graphicData.drawSize, this.parent.DrawColor, this.parent.DrawColorTwo, null);
				}
				return this.offGraphic;
			}
		}

		// Token: 0x0600B94F RID: 47439 RVA: 0x00357B8B File Offset: 0x00355D8B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.switchOnInt, "switchOn", true, false);
			Scribe_Values.Look<bool>(ref this.wantSwitchOn, "wantSwitchOn", true, false);
		}

		// Token: 0x0600B950 RID: 47440 RVA: 0x00357BB7 File Offset: 0x00355DB7
		public bool WantsFlick()
		{
			return this.wantSwitchOn != this.switchOnInt;
		}

		// Token: 0x0600B951 RID: 47441 RVA: 0x00357BCA File Offset: 0x00355DCA
		public void DoFlick()
		{
			this.SwitchIsOn = !this.SwitchIsOn;
			SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
		}

		// Token: 0x0600B952 RID: 47442 RVA: 0x00357C06 File Offset: 0x00355E06
		public void ResetToOn()
		{
			this.switchOnInt = true;
			this.wantSwitchOn = true;
		}

		// Token: 0x0600B953 RID: 47443 RVA: 0x00357C16 File Offset: 0x00355E16
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.parent.Faction == Faction.OfPlayer)
			{
				yield return new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Command_TogglePower,
					icon = this.CommandTex,
					defaultLabel = this.Props.commandLabelKey.Translate(),
					defaultDesc = this.Props.commandDescKey.Translate(),
					isActive = (() => this.wantSwitchOn),
					toggleAction = delegate()
					{
						this.wantSwitchOn = !this.wantSwitchOn;
						FlickUtility.UpdateFlickDesignation(this.parent);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04007FCE RID: 32718
		private bool switchOnInt = true;

		// Token: 0x04007FCF RID: 32719
		private bool wantSwitchOn = true;

		// Token: 0x04007FD0 RID: 32720
		private Graphic offGraphic;

		// Token: 0x04007FD1 RID: 32721
		private Texture2D cachedCommandTex;

		// Token: 0x04007FD2 RID: 32722
		private const string OffGraphicSuffix = "_Off";

		// Token: 0x04007FD3 RID: 32723
		public const string FlickedOnSignal = "FlickedOn";

		// Token: 0x04007FD4 RID: 32724
		public const string FlickedOffSignal = "FlickedOff";
	}
}
