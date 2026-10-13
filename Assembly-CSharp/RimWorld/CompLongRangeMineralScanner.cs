using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F0E RID: 12046
	public class CompLongRangeMineralScanner : CompScanner
	{
		// Token: 0x17002BE6 RID: 11238
		// (get) Token: 0x0601101B RID: 69659 RVA: 0x004E4604 File Offset: 0x004E2804
		public new CompProperties_LongRangeMineralScanner Props
		{
			get
			{
				return this.props as CompProperties_LongRangeMineralScanner;
			}
		}

		// Token: 0x0601101C RID: 69660 RVA: 0x004E4611 File Offset: 0x004E2811
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.targetMineable, "targetMineable");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.targetMineable == null)
			{
				this.SetDefaultTargetMineral();
			}
		}

		// Token: 0x0601101D RID: 69661 RVA: 0x004E463F File Offset: 0x004E283F
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetDefaultTargetMineral();
		}

		// Token: 0x0601101E RID: 69662 RVA: 0x004E464E File Offset: 0x004E284E
		private void SetDefaultTargetMineral()
		{
			this.targetMineable = ThingDefOf.MineableGold;
		}

		// Token: 0x0601101F RID: 69663 RVA: 0x004E465C File Offset: 0x004E285C
		protected override void DoFind(Pawn worker)
		{
			Slate slate = new Slate();
			slate.Set<Map>("map", this.parent.Map, false);
			slate.Set<ThingDef>("targetMineable", this.targetMineable, false);
			slate.Set<ThingDef>("targetMineableThing", this.targetMineable.building.mineableThing, false);
			slate.Set<Pawn>("worker", worker, false);
			if (!QuestScriptDefOf.LongRangeMineralScannerLump.CanRun(slate, this.parent.Map))
			{
				return;
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.LongRangeMineralScannerLump, slate);
			Find.LetterStack.ReceiveLetter(quest.name, quest.description, LetterDefOf.PositiveEvent, null, null, quest, null, null, 0, true);
		}

		// Token: 0x06011020 RID: 69664 RVA: 0x004E470D File Offset: 0x004E290D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.parent.Faction == Faction.OfPlayer)
			{
				ThingDef mineableThing = this.targetMineable.building.mineableThing;
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandSelectMineralToScanFor".Translate() + ": " + mineableThing.LabelCap;
				command_Action.defaultDesc = "CommandSelectMineralToScanForDesc".Translate();
				command_Action.icon = mineableThing.uiIcon;
				command_Action.iconAngle = mineableThing.uiIconAngle;
				command_Action.iconOffset = mineableThing.uiIconOffset;
				command_Action.action = delegate()
				{
					List<ThingDef> mineables = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables;
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (ThingDef localD2 in mineables)
					{
						ThingDef localD = localD2;
						FloatMenuOption item = new FloatMenuOption(localD.building.mineableThing.LabelCap, delegate()
						{
							foreach (object obj in Find.Selector.SelectedObjects)
							{
								Thing thing = obj as Thing;
								if (thing != null)
								{
									CompLongRangeMineralScanner compLongRangeMineralScanner = thing.TryGetComp<CompLongRangeMineralScanner>();
									if (compLongRangeMineralScanner != null)
									{
										compLongRangeMineralScanner.targetMineable = localD;
									}
								}
							}
						}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, localD.building.mineableThing), null, true, 0);
						list.Add(item);
					}
					Find.WindowStack.Add(new FloatMenu(list));
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400B0CA RID: 45258
		private ThingDef targetMineable;
	}
}
