using System;
using System.Collections.Generic;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002EC8 RID: 11976
	public class CompVoidNode : CompInteractable
	{
		// Token: 0x06010E6D RID: 69229 RVA: 0x004DDB9B File Offset: 0x004DBD9B
		private float AgeSecs()
		{
			return (float)(Find.TickManager.TicksGame - this.parent.TickSpawned) / 60f;
		}

		// Token: 0x06010E6E RID: 69230 RVA: 0x004DDBBA File Offset: 0x004DBDBA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Void node"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.parent.Graphic.MatSingle.SetFloat(ShaderPropertyIDs.VoidNodeDestructionStartAge, float.MaxValue);
				this.ambientSustainer = SoundDefOf.VoidNode_Ambient.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
			});
		}

		// Token: 0x06010E6F RID: 69231 RVA: 0x004DDBF0 File Offset: 0x004DBDF0
		public override void CompTick()
		{
			base.CompTick();
			if (this.closeTick > 0)
			{
				this.ambientSustainer.End();
				if (Find.TickManager.TicksGame + 260 == this.closeTick)
				{
					Find.MusicManagerPlay.ForcePlaySong(SongDefOf.AnomalyCreditsSong, false);
				}
				if (Find.TickManager.TicksGame == this.closeTick)
				{
					Find.TickManager.Pause();
					Thing.allowDestroyNonDestroyable = true;
					this.parent.Destroy(DestroyMode.Vanish);
					Thing.allowDestroyNonDestroyable = false;
					CameraJumper.TryJump(Find.Anomaly.monolith, CameraJumper.MovementMode.Cut);
					if (this.embracing)
					{
						VoidAwakeningUtility.EmbraceTheVoid(this.interactedPawn);
						return;
					}
					VoidAwakeningUtility.DisruptTheLink(this.interactedPawn);
				}
			}
		}

		// Token: 0x06010E70 RID: 69232 RVA: 0x004DDCAC File Offset: 0x004DBEAC
		private void OpenDialog(Pawn pawn)
		{
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(new DiaNode("VoidNodeLetter".Translate(pawn.Named("PAWN")))
			{
				options = 
				{
					new DiaOption("VoidNodeDisrupt".Translate())
					{
						action = new Action(this.DisruptNode),
						resolveTree = true
					},
					new DiaOption("VoidNodeEmbrace".Translate())
					{
						action = delegate()
						{
							this.CloseNode(true, 300, 0);
						},
						resolveTree = true
					},
					new DiaOption("VoidNodePostpone".Translate())
					{
						resolveTree = true
					}
				}
			}, false, false, null);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
		}

		// Token: 0x06010E71 RID: 69233 RVA: 0x004DDD84 File Offset: 0x004DBF84
		private void DisruptNode()
		{
			SoundDefOf.VoidNode_Explode.PlayOneShotOnCamera(null);
			EffecterDefOf.VoidNodeDisrupted.SpawnMaintained(this.parent, this.parent.Map, 1f);
			EffecterDefOf.VoidNodeDisruptedFlashes.SpawnMaintained(this.parent, this.parent.Map, 1f);
			this.parent.Graphic.MatSingle.SetFloat(ShaderPropertyIDs.VoidNodeDestructionStartAge, this.AgeSecs());
			this.CloseNode(false, 24, 600);
		}

		// Token: 0x06010E72 RID: 69234 RVA: 0x004DDE0C File Offset: 0x004DC00C
		private void CloseNode(bool embraceVoid, int fadeTimeTicks, int fadeDelayTicks)
		{
			this.closeTick = Find.TickManager.TicksGame + fadeTimeTicks + fadeDelayTicks;
			this.embracing = embraceVoid;
			this.activated = true;
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "NodeClosed");
			Find.TickManager.slower.SignalForceNormalSpeed();
			Find.MusicManagerPlay.ForceSilenceFor(999f);
			CameraJumper.TryJump(this.parent, CameraJumper.MovementMode.Cut);
			ScreenFader.StartFade(Color.white, (float)(fadeTimeTicks - 5) / 60f, (float)fadeDelayTicks / 60f);
		}

		// Token: 0x06010E73 RID: 69235 RVA: 0x004DDE9B File Offset: 0x004DC09B
		protected override void OnInteracted(Pawn caster)
		{
			this.interactedPawn = caster;
			this.OpenDialog(caster);
		}

		// Token: 0x06010E74 RID: 69236 RVA: 0x004DDEAB File Offset: 0x004DC0AB
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (this.activated)
			{
				return false;
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x06010E75 RID: 69237 RVA: 0x004DDEC4 File Offset: 0x004DC0C4
		public void Reset()
		{
			this.activated = false;
		}

		// Token: 0x06010E76 RID: 69238 RVA: 0x004DDECD File Offset: 0x004DC0CD
		public override string CompInspectStringExtra()
		{
			if (this.activated)
			{
				return "";
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x06010E77 RID: 69239 RVA: 0x004DDEE3 File Offset: 0x004DC0E3
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (this.activated)
			{
				yield break;
			}
			foreach (FloatMenuOption floatMenuOption in base.CompFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06010E78 RID: 69240 RVA: 0x004DDEFA File Offset: 0x004DC0FA
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.activated)
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06010E79 RID: 69241 RVA: 0x004DDF0C File Offset: 0x004DC10C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.activated, "activated", false, false);
			Scribe_References.Look<Pawn>(ref this.interactedPawn, "interactedPawn", false);
			Scribe_Values.Look<bool>(ref this.embracing, "embracing", false, false);
			Scribe_Values.Look<int>(ref this.closeTick, "closeTick", 0, false);
		}

		// Token: 0x0400AFD3 RID: 45011
		private const int SlowFade = 300;

		// Token: 0x0400AFD4 RID: 45012
		private const int QuickFade = 24;

		// Token: 0x0400AFD5 RID: 45013
		private const int MusicIntroLengthTicks = 260;

		// Token: 0x0400AFD6 RID: 45014
		private bool activated;

		// Token: 0x0400AFD7 RID: 45015
		private Pawn interactedPawn;

		// Token: 0x0400AFD8 RID: 45016
		private bool embracing;

		// Token: 0x0400AFD9 RID: 45017
		private int closeTick = -99999;

		// Token: 0x0400AFDA RID: 45018
		private Sustainer ambientSustainer;
	}
}
