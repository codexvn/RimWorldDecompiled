using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C05 RID: 11269
	[StaticConstructorOnStartup]
	public class CompBandNode : ThingComp
	{
		// Token: 0x170027AB RID: 10155
		// (get) Token: 0x0600FC7D RID: 64637 RVA: 0x00493805 File Offset: 0x00491A05
		private CompPowerTrader PowerTrader
		{
			get
			{
				return this.parent.TryGetComp<CompPowerTrader>();
			}
		}

		// Token: 0x170027AC RID: 10156
		// (get) Token: 0x0600FC7E RID: 64638 RVA: 0x00493812 File Offset: 0x00491A12
		public CompProperties_BandNode Props
		{
			get
			{
				return (CompProperties_BandNode)this.props;
			}
		}

		// Token: 0x170027AD RID: 10157
		// (get) Token: 0x0600FC7F RID: 64639 RVA: 0x0049381F File Offset: 0x00491A1F
		private int RetuneTimeTicks
		{
			get
			{
				return (int)(60000f * this.Props.retuneDays);
			}
		}

		// Token: 0x170027AE RID: 10158
		// (get) Token: 0x0600FC80 RID: 64640 RVA: 0x00493833 File Offset: 0x00491A33
		private int TuningTimeTicks
		{
			get
			{
				return (int)(this.Props.tuneSeconds * 60f);
			}
		}

		// Token: 0x170027AF RID: 10159
		// (get) Token: 0x0600FC81 RID: 64641 RVA: 0x00493847 File Offset: 0x00491A47
		private BandNodeState State
		{
			get
			{
				if (this.tunedTo != null && this.tuningTo != null)
				{
					return BandNodeState.Retuning;
				}
				if (this.tuningTo != null)
				{
					return BandNodeState.Tuning;
				}
				if (this.tunedTo != null)
				{
					return BandNodeState.Tuned;
				}
				return BandNodeState.Untuned;
			}
		}

		// Token: 0x0600FC82 RID: 64642 RVA: 0x00493870 File Offset: 0x00491A70
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Band node"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x0600FC83 RID: 64643 RVA: 0x00493892 File Offset: 0x00491A92
		public override void PostExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.tunedTo, "tunedTo", false);
			Scribe_References.Look<Pawn>(ref this.tuningTo, "tuningTo", false);
			Scribe_Values.Look<int>(ref this.tuningTimeLeft, "tuningTimeLeft", 0, false);
		}

		// Token: 0x0600FC84 RID: 64644 RVA: 0x004938C8 File Offset: 0x00491AC8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = ((this.tunedTo == null) ? ("BandNodeTuneTo".Translate() + "...") : ("BandNodeRetuneTo".Translate() + "..."));
			command_Action.defaultDesc = ((this.tunedTo == null) ? "BandNodeTuningDesc".Translate("PeriodSeconds".Translate(this.Props.tuneSeconds)) : "BandNodeRetuningDesc".Translate(this.Props.retuneDays.ToString() + " " + "Days".Translate()));
			Command_Action command_Action2 = command_Action;
			command_Action2.onHover = (Action)Delegate.Combine(command_Action2.onHover, new Action(delegate()
			{
				Pawn pawn2 = (this.tuningTo != null) ? this.tuningTo : this.tunedTo;
				if (pawn2 != null)
				{
					GenDraw.DrawLineBetween(this.parent.DrawPos, pawn2.DrawPos);
				}
			}));
			bool flag = false;
			Pawn pawn;
			using (IEnumerator<Pawn> enumerator = this.parent.Map.mapPawns.AllPawnsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					pawn = enumerator.Current;
					if (MechanitorUtility.IsMechanitor(pawn) && this.tunedTo != pawn && this.tuningTo != pawn)
					{
						flag = true;
						break;
					}
				}
			}
			command_Action.Disabled = !flag;
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/BandNodeTuning", true);
			Command_Action command_Action3 = command_Action;
			command_Action3.action = (Action)Delegate.Combine(command_Action3.action, new Action(delegate()
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				using (IEnumerator<Pawn> enumerator2 = this.parent.Map.mapPawns.AllPawnsSpawned.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Pawn pawn = enumerator2.Current;
						if (MechanitorUtility.IsMechanitor(pawn))
						{
							IEnumerable<CompBandNode> bandNodes = from t in Find.Selector.SelectedObjects.Where(delegate(object t)
							{
								Thing thing = t as Thing;
								return thing != null && thing.TryGetComp<CompBandNode>() != null;
							})
							select ((Thing)t).TryGetComp<CompBandNode>() into n
							where n.tunedTo != pawn && n.tuningTo != pawn
							select n;
							if (bandNodes.Any<CompBandNode>())
							{
								Pawn localPawn = pawn;
								string text = pawn.Name.ToStringFull;
								if (bandNodes.All((CompBandNode b) => b.tunedTo == null))
								{
									goto IL_13D;
								}
								if (bandNodes.All((CompBandNode b) => b.tunedTo != null))
								{
									goto IL_13D;
								}
								IL_1B0:
								list.Add(new FloatMenuOption(text, delegate()
								{
									foreach (CompBandNode compBandNode in bandNodes)
									{
										compBandNode.TuneTo(localPawn);
									}
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								continue;
								IL_13D:
								if (this.tunedTo == null)
								{
									text += " (" + this.Props.tuneSeconds.ToString() + " " + "SecondsLower".Translate() + ")";
									goto IL_1B0;
								}
								text = text + " (" + this.RetuneTimeTicks.ToStringTicksToPeriod(true, false, true, true, false) + ")";
								goto IL_1B0;
							}
						}
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}));
			yield return command_Action;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: complete tuning",
					action = delegate()
					{
						this.tuningTimeLeft = 0;
					}
				};
			}
			yield break;
		}

		// Token: 0x0600FC85 RID: 64645 RVA: 0x004938D8 File Offset: 0x00491AD8
		public void TuneTo(Pawn pawn)
		{
			this.tuningTimeLeft = ((this.tunedTo == null) ? this.TuningTimeTicks : this.RetuneTimeTicks);
			this.tuningTo = pawn;
		}

		// Token: 0x0600FC86 RID: 64646 RVA: 0x00493900 File Offset: 0x00491B00
		public override void PostDraw()
		{
			base.PostDraw();
			BandNodeState state = this.State;
			Material material;
			if (state != BandNodeState.Untuned)
			{
				if (state == BandNodeState.Tuned)
				{
					material = CompBandNode.TunedMaterial;
				}
				else
				{
					material = CompBandNode.TuningMaterial;
				}
			}
			else
			{
				material = CompBandNode.UntunedMaterial;
			}
			Vector3 s = new Vector3(CompBandNode.TuningBarSize.x, 1f, CompBandNode.TuningBarSize.y);
			Vector3 pos = this.parent.DrawPos + new Vector3(0f, 0f, -0.4f);
			pos.y = this.parent.def.altitudeLayer.AltitudeFor() + 0.03658537f;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, this.parent.Rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
		}

		// Token: 0x0600FC87 RID: 64647 RVA: 0x004939D4 File Offset: 0x00491BD4
		public override void CompTick()
		{
			this.PowerTrader.PowerOutput = ((this.tunedTo == null && this.tuningTo == null) ? ((float)(-(float)this.Props.powerConsumptionIdle)) : (-this.PowerTrader.Props.PowerConsumption));
			if (this.tunedTo != null && this.tunedTo.Dead)
			{
				this.tunedTo = null;
			}
			if (this.tuningTo != null && this.tuningTo.Dead)
			{
				this.tuningTo = null;
			}
			if (this.PowerTrader != null && !this.PowerTrader.PowerOn)
			{
				Effecter effecter = this.effecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effecter = null;
				return;
			}
			if (this.tuningTo != null)
			{
				this.tuningTimeLeft--;
				if (this.tuningTimeLeft <= 0)
				{
					this.tunedTo = this.tuningTo;
					this.tuningTo = null;
					if (this.Props.tuningCompleteSound != null)
					{
						this.Props.tuningCompleteSound.PlayOneShot(this.parent);
					}
				}
			}
			if (this.tuningTo == null && this.tunedTo != null && !this.tunedTo.health.hediffSet.HasHediff(this.Props.hediff, false))
			{
				this.tunedTo.health.AddHediff(this.Props.hediff, this.tunedTo.health.hediffSet.GetBrain(), null, null);
			}
			if (this.State == BandNodeState.Untuned)
			{
				if (this.effecter == null || this.effecter.def != this.Props.untunedEffect)
				{
					Effecter effecter2 = this.effecter;
					if (effecter2 != null)
					{
						effecter2.Cleanup();
					}
					this.effecter = this.Props.untunedEffect.Spawn();
				}
			}
			else if (this.State == BandNodeState.Tuned)
			{
				if (this.effecter == null || this.effecter.def != this.Props.tunedEffect)
				{
					Effecter effecter3 = this.effecter;
					if (effecter3 != null)
					{
						effecter3.Cleanup();
					}
					this.effecter = this.Props.tunedEffect.Spawn();
				}
			}
			else if (this.State == BandNodeState.Tuning)
			{
				if (this.effecter == null || this.effecter.def != this.Props.tuningEffect)
				{
					Effecter effecter4 = this.effecter;
					if (effecter4 != null)
					{
						effecter4.Cleanup();
					}
					this.effecter = this.Props.tuningEffect.Spawn();
				}
			}
			else if (this.State == BandNodeState.Retuning)
			{
				if (this.effecter == null || this.effecter.def != this.Props.retuningEffect)
				{
					Effecter effecter5 = this.effecter;
					if (effecter5 != null)
					{
						effecter5.Cleanup();
					}
					this.effecter = this.Props.retuningEffect.Spawn();
				}
			}
			else
			{
				Effecter effecter6 = this.effecter;
				if (effecter6 != null)
				{
					effecter6.Cleanup();
				}
				this.effecter = null;
			}
			if (this.effecter != null)
			{
				this.effecter.EffectTick(this.parent, this.parent);
			}
		}

		// Token: 0x0600FC88 RID: 64648 RVA: 0x00493CE4 File Offset: 0x00491EE4
		public override string CompInspectStringExtra()
		{
			string t = null;
			if (!this.PowerTrader.PowerOn)
			{
				t = "\n" + "Unpowered".Translate().CapitalizeFirst().Resolve();
			}
			if (this.tuningTo != null)
			{
				return "BandNodeTuningTo".Translate() + ": " + this.tuningTo.Name.ToStringFull + " - " + this.tuningTimeLeft.ToStringTicksToPeriod(true, false, true, true, false) + t;
			}
			return "BandNodeTunedTo".Translate() + ": " + ((this.tunedTo == null) ? "Nobody".Translate().Resolve() : this.tunedTo.Name.ToStringFull) + t;
		}

		// Token: 0x0400A581 RID: 42369
		private static readonly Material UntunedMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.red, false);

		// Token: 0x0400A582 RID: 42370
		private static readonly Material TuningMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.yellow, false);

		// Token: 0x0400A583 RID: 42371
		private static readonly Material TunedMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.green, false);

		// Token: 0x0400A584 RID: 42372
		private static readonly Vector2 TuningBarSize = new Vector3(0.255f, 0.035f);

		// Token: 0x0400A585 RID: 42373
		private const float TuningBarYOffset = -0.4f;

		// Token: 0x0400A586 RID: 42374
		public Pawn tunedTo;

		// Token: 0x0400A587 RID: 42375
		public int tuningTimeLeft;

		// Token: 0x0400A588 RID: 42376
		public Pawn tuningTo;

		// Token: 0x0400A589 RID: 42377
		private Effecter effecter;
	}
}
