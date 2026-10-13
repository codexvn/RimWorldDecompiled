using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194D RID: 6477
	[StaticConstructorOnStartup]
	public class Command_SetNeuralSuperchargerAutoUse : Command
	{
		// Token: 0x1700165A RID: 5722
		// (get) Token: 0x06009354 RID: 37716 RVA: 0x002B55CF File Offset: 0x002B37CF
		private static Texture2D AutoUseForEveryone
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone == null)
				{
					Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_EveryoneAutoUse", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.autoUseForEveryone;
			}
		}

		// Token: 0x1700165B RID: 5723
		// (get) Token: 0x06009355 RID: 37717 RVA: 0x002B55F3 File Offset: 0x002B37F3
		private static Texture2D AutoUseWithDesire
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire == null)
				{
					Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_AutoUseWithDesire", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.autoUseWithDesire;
			}
		}

		// Token: 0x1700165C RID: 5724
		// (get) Token: 0x06009356 RID: 37718 RVA: 0x002B5617 File Offset: 0x002B3817
		private static Texture2D NoAutoUseTex
		{
			get
			{
				if (Command_SetNeuralSuperchargerAutoUse.noAutoUseTex == null)
				{
					Command_SetNeuralSuperchargerAutoUse.noAutoUseTex = ContentFinder<Texture2D>.Get("UI/Gizmos/NeuralSupercharger_NoAutoUse", true);
				}
				return Command_SetNeuralSuperchargerAutoUse.noAutoUseTex;
			}
		}

		// Token: 0x06009357 RID: 37719 RVA: 0x002B563C File Offset: 0x002B383C
		public Command_SetNeuralSuperchargerAutoUse(CompNeuralSupercharger comp)
		{
			this.comp = comp;
			switch (comp.autoUseMode)
			{
			case CompNeuralSupercharger.AutoUseMode.NoAutoUse:
				this.defaultLabel = "CommandNeuralSuperchargerNoAutoUse".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerNoAutoUseDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.NoAutoUseTex;
				return;
			case CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire:
				this.defaultLabel = "CommandNeuralSuperchargerAutoUseWithDesire".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerAutoUseWithDesireDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.AutoUseWithDesire;
				return;
			case CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone:
				this.defaultLabel = "CommandNeuralSuperchargerAutoForEveryone".Translate();
				this.defaultDesc = "CommandNeuralSuperchargerAutoForEveryoneDescription".Translate();
				this.icon = Command_SetNeuralSuperchargerAutoUse.AutoUseForEveryone;
				return;
			default:
				Log.Error(string.Format("Unknown auto use mode: {0}", comp.autoUseMode));
				return;
			}
		}

		// Token: 0x06009358 RID: 37720 RVA: 0x002B5730 File Offset: 0x002B3930
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerNoAutoUse".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.NoAutoUse;
			}, Command_SetNeuralSuperchargerAutoUse.NoAutoUseTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerAutoUseWithDesire".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseWithDesire;
			}, Command_SetNeuralSuperchargerAutoUse.AutoUseWithDesire, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandNeuralSuperchargerAutoForEveryone".Translate(), delegate()
			{
				this.comp.autoUseMode = CompNeuralSupercharger.AutoUseMode.AutoUseForEveryone;
			}, Command_SetNeuralSuperchargerAutoUse.AutoUseForEveryone, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x04005EBC RID: 24252
		private static Texture2D autoUseForEveryone;

		// Token: 0x04005EBD RID: 24253
		private static Texture2D autoUseWithDesire;

		// Token: 0x04005EBE RID: 24254
		private static Texture2D noAutoUseTex;

		// Token: 0x04005EBF RID: 24255
		private readonly CompNeuralSupercharger comp;
	}
}
