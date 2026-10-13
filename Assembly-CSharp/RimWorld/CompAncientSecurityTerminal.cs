using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BCF RID: 11215
	public class CompAncientSecurityTerminal : CompHackable
	{
		// Token: 0x17002751 RID: 10065
		// (get) Token: 0x0600FAE5 RID: 64229 RVA: 0x0048E5E5 File Offset: 0x0048C7E5
		private new CompProperties_AncientSecurityTerminalHackable Props
		{
			get
			{
				return (CompProperties_AncientSecurityTerminalHackable)this.props;
			}
		}

		// Token: 0x0600FAE6 RID: 64230 RVA: 0x0048E5F4 File Offset: 0x0048C7F4
		protected override void OnHacked(Pawn hacker = null, bool suppressMessages = false)
		{
			base.OnHacked(hacker, suppressMessages);
			List<Building> list = this.parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.AncientBlastDoor).Concat(this.parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.Turret_AncientArmoredTurret)).Where(delegate(Building thing)
			{
				CompHackable compHackable;
				return thing.TryGetComp(out compHackable) && !compHackable.IsHacked;
			}).ToList<Building>();
			if (list.Empty<Building>())
			{
				if (hacker != null && !suppressMessages)
				{
					Messages.Message(this.Props.messageNoValidTarget.Formatted(hacker.Named("HACKER")), hacker, MessageTypeDefOf.NeutralEvent, true);
				}
				return;
			}
			Building building = list.RandomElementByWeight(delegate(Building x)
			{
				if (x.def != ThingDefOf.Turret_AncientArmoredTurret)
				{
					return 0.25f;
				}
				return 0.75f;
			});
			CompHackable comp = building.GetComp<CompHackable>();
			bool flag = false;
			if (comp.parent is Building_Turret && Rand.Chance(0.5f) && hacker != null)
			{
				flag = true;
				comp.parent.SetFaction(hacker.Faction, null);
				this.parent.Map.fogGrid.Unfog(comp.parent.Position);
			}
			else
			{
				comp.Hack(comp.defence, hacker, true);
			}
			if (hacker == null || suppressMessages)
			{
				return;
			}
			TaggedString label;
			TaggedString text;
			if (flag)
			{
				label = this.Props.letterTurretHackedLabel.Formatted(hacker.Named("HACKER"));
				text = this.Props.letterTurretHackedText.Formatted(hacker.Named("HACKER"));
			}
			else if (building.def == ThingDefOf.AncientBlastDoor)
			{
				label = this.Props.letterDoorOpenedLabel.Formatted(hacker.Named("HACKER"));
				text = this.Props.letterDoorOpenedText.Formatted(hacker.Named("HACKER"));
			}
			else
			{
				label = this.Props.letterTurretDisabledLabel.Formatted(hacker.Named("HACKER"));
				text = this.Props.letterTurretDisabledText.Formatted(hacker.Named("HACKER"));
			}
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, building, null, null, null, null, 0, true);
		}
	}
}
