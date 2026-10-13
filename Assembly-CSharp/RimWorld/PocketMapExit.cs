using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002B14 RID: 11028
	public class PocketMapExit : MapPortal
	{
		// Token: 0x1700268B RID: 9867
		// (get) Token: 0x0600F64E RID: 63054 RVA: 0x0047847B File Offset: 0x0047667B
		public override string EnterString
		{
			get
			{
				string key = "ExitPortal";
				MapPortal mapPortal = this.entrance;
				return key.Translate((mapPortal != null) ? mapPortal.def.portal.pocketMapGenerator.label : null);
			}
		}

		// Token: 0x1700268C RID: 9868
		// (get) Token: 0x0600F64F RID: 63055 RVA: 0x004784B2 File Offset: 0x004766B2
		public override string CancelEnterString
		{
			get
			{
				return "CommandCancelExitPortal".Translate();
			}
		}

		// Token: 0x1700268D RID: 9869
		// (get) Token: 0x0600F650 RID: 63056 RVA: 0x004784C3 File Offset: 0x004766C3
		protected override Texture2D EnterTex
		{
			get
			{
				return PocketMapExit.ExitMapTex.Texture;
			}
		}

		// Token: 0x0600F651 RID: 63057 RVA: 0x004784CF File Offset: 0x004766CF
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				if (PocketMapUtility.currentlyGeneratingPortal == null)
				{
					Log.Error("Pocket map exit could not find map portal to connect to");
					return;
				}
				this.entrance = PocketMapUtility.currentlyGeneratingPortal;
				this.entrance.exit = this;
			}
		}

		// Token: 0x0600F652 RID: 63058 RVA: 0x00478505 File Offset: 0x00476705
		public override Map GetOtherMap()
		{
			return this.entrance.Map;
		}

		// Token: 0x0600F653 RID: 63059 RVA: 0x00478512 File Offset: 0x00476712
		public override IntVec3 GetDestinationLocation()
		{
			return this.entrance.Position;
		}

		// Token: 0x0600F654 RID: 63060 RVA: 0x00478520 File Offset: 0x00476720
		public override void OnEntered(Pawn pawn)
		{
			base.Notify_ThingAdded(pawn);
			if (Find.CurrentMap != base.Map)
			{
				if (Find.CurrentMap == this.entrance.Map)
				{
					SoundDef traverseSound = this.entrance.def.portal.traverseSound;
					if (traverseSound == null)
					{
						return;
					}
					traverseSound.PlayOneShot(this.entrance);
				}
				return;
			}
			SoundDef traverseSound2 = this.entrance.def.portal.traverseSound;
			if (traverseSound2 == null)
			{
				return;
			}
			traverseSound2.PlayOneShot(this);
		}

		// Token: 0x0600F655 RID: 63061 RVA: 0x004785A3 File Offset: 0x004767A3
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return new Command_Action
			{
				defaultLabel = "CommandViewSurface".Translate(),
				defaultDesc = "CommandViewSurfaceDesc".Translate(),
				icon = PocketMapExit.ViewEntranceTex.Texture,
				action = delegate()
				{
					CameraJumper.TryJumpAndSelect(this.entrance, CameraJumper.MovementMode.Pan);
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x0600F656 RID: 63062 RVA: 0x004785B3 File Offset: 0x004767B3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapPortal>(ref this.entrance, "pitGate", false);
		}

		// Token: 0x0400A226 RID: 41510
		private static readonly CachedTexture ExitMapTex = new CachedTexture("UI/Commands/ExitCave");

		// Token: 0x0400A227 RID: 41511
		private static readonly CachedTexture ViewEntranceTex = new CachedTexture("UI/Commands/ViewCave");

		// Token: 0x0400A228 RID: 41512
		public MapPortal entrance;
	}
}
