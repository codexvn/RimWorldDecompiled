using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D72 RID: 3442
	public class VerbTracker : IExposable
	{
		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x060057CE RID: 22478 RVA: 0x001C4AC3 File Offset: 0x001C2CC3
		public List<Verb> AllVerbs
		{
			get
			{
				if (this.verbs == null)
				{
					this.InitVerbsFromZero();
				}
				return this.verbs;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x060057CF RID: 22479 RVA: 0x001C4ADC File Offset: 0x001C2CDC
		public Verb PrimaryVerb
		{
			get
			{
				if (this.verbs == null)
				{
					this.InitVerbsFromZero();
				}
				for (int i = 0; i < this.verbs.Count; i++)
				{
					if (this.verbs[i].verbProps.isPrimary)
					{
						return this.verbs[i];
					}
				}
				return null;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x060057D0 RID: 22480 RVA: 0x001C4B34 File Offset: 0x001C2D34
		public bool AnyVerbBursting
		{
			get
			{
				for (int i = 0; i < this.verbs.Count; i++)
				{
					if (this.verbs[i].state == VerbState.Bursting)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060057D1 RID: 22481 RVA: 0x001C4B6E File Offset: 0x001C2D6E
		public VerbTracker(IVerbOwner directOwner)
		{
			this.directOwner = directOwner;
		}

		// Token: 0x060057D2 RID: 22482 RVA: 0x001C4B80 File Offset: 0x001C2D80
		public void VerbsTick()
		{
			if (this.verbs == null)
			{
				return;
			}
			for (int i = 0; i < this.verbs.Count; i++)
			{
				this.verbs[i].VerbTick();
			}
		}

		// Token: 0x060057D3 RID: 22483 RVA: 0x001C4BBD File Offset: 0x001C2DBD
		public IEnumerable<Command> GetVerbsCommands()
		{
			IVerbOwner verbOwner = this.directOwner;
			CompEquippable ce = verbOwner as CompEquippable;
			if (ce == null)
			{
				yield break;
			}
			Thing ownerThing = ce.parent;
			List<Verb> verbs = this.AllVerbs;
			int num;
			for (int i = 0; i < verbs.Count; i = num + 1)
			{
				Verb verb = verbs[i];
				if (verb.verbProps.hasStandardCommand)
				{
					yield return this.CreateVerbTargetCommand(ownerThing, verb);
				}
				num = i;
			}
			if (!this.directOwner.Tools.NullOrEmpty<Tool>() && ce != null && ce.parent.def.IsMeleeWeapon)
			{
				yield return this.CreateVerbTargetCommand(ownerThing, verbs.FirstOrDefault((Verb v) => v.verbProps.IsMeleeAttack));
			}
			yield break;
		}

		// Token: 0x060057D4 RID: 22484 RVA: 0x001C4BD0 File Offset: 0x001C2DD0
		private Command_VerbTarget CreateVerbTargetCommand(Thing ownerThing, Verb verb)
		{
			Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
			command_VerbTarget.defaultDesc = ownerThing.LabelCap + ": " + ownerThing.def.description.CapitalizeFirst();
			command_VerbTarget.ownerThing = ownerThing;
			command_VerbTarget.tutorTag = "VerbTarget";
			command_VerbTarget.verb = verb;
			if (verb.caster.Faction != Faction.OfPlayer && !DebugSettings.ShowDevGizmos)
			{
				command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
			}
			else if (verb.CasterIsPawn)
			{
				string reason;
				if (verb.CasterPawn.RaceProps.IsMechanoid && !MechanitorUtility.EverControllable(verb.CasterPawn) && !DebugSettings.ShowDevGizmos)
				{
					command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
				}
				else if (verb.CasterPawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					command_VerbTarget.Disable("IsIncapableOfViolence".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
				}
				else if (!verb.CasterPawn.Drafted && !DebugSettings.ShowDevGizmos)
				{
					command_VerbTarget.Disable("IsNotDrafted".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
				}
				else if (verb is Verb_LaunchProjectile)
				{
					Apparel apparel = verb.FirstApparelPreventingShooting();
					if (apparel != null)
					{
						command_VerbTarget.Disable("ApparelPreventsShooting".Translate(verb.CasterPawn.Named("PAWN"), apparel.Named("APPAREL")).CapitalizeFirst());
					}
				}
				else if (EquipmentUtility.RolePreventsFromUsing(verb.CasterPawn, verb.EquipmentSource, out reason))
				{
					command_VerbTarget.Disable(reason);
				}
			}
			CompUniqueWeapon compUniqueWeapon;
			if ((!verb.EquipmentSource.TryGetComp(out compUniqueWeapon) || !compUniqueWeapon.IgnoreAccuracyMaluses) && verb.caster.Spawned && verb.caster.Map.weatherManager.CurWeatherMaxRangeCap >= 0f)
			{
				command_VerbTarget.defaultDescPostfix = "\n\n" + ("WeatherMaxRangeCap".Translate() + ": " + verb.caster.Map.weatherManager.curWeather.LabelCap).Colorize(ColoredText.WarningColor);
			}
			return command_VerbTarget;
		}

		// Token: 0x060057D5 RID: 22485 RVA: 0x001C4E24 File Offset: 0x001C3024
		public Verb GetVerb(VerbCategory category)
		{
			List<Verb> allVerbs = this.AllVerbs;
			if (allVerbs != null)
			{
				for (int i = 0; i < allVerbs.Count; i++)
				{
					if (allVerbs[i].verbProps.category == category)
					{
						return allVerbs[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060057D6 RID: 22486 RVA: 0x001C4E6C File Offset: 0x001C306C
		public void ExposeData()
		{
			Scribe_Collections.Look<Verb>(ref this.verbs, "verbs", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.verbs != null)
			{
				if (this.verbs.RemoveAll((Verb x) => x == null) != 0)
				{
					Log.Error("Some verbs were null after loading. directOwner=" + this.directOwner.ToStringSafe<IVerbOwner>());
				}
				List<Verb> sources = this.verbs;
				this.verbs = new List<Verb>();
				this.InitVerbs(delegate(Type type, string id)
				{
					Verb verb = sources.FirstOrDefault((Verb v) => v.loadID == id && v.GetType() == type);
					if (verb == null)
					{
						Log.Warning(string.Format("Replaced verb {0}/{1}; may have been changed through a version update or a mod change", type, id));
						verb = (Verb)Activator.CreateInstance(type);
					}
					this.verbs.Add(verb);
					return verb;
				});
			}
		}

		// Token: 0x060057D7 RID: 22487 RVA: 0x001C4F1B File Offset: 0x001C311B
		public void InitVerbsFromZero()
		{
			this.verbs = new List<Verb>();
			this.InitVerbs(delegate(Type type, string id)
			{
				Verb verb = (Verb)Activator.CreateInstance(type);
				this.verbs.Add(verb);
				return verb;
			});
		}

		// Token: 0x060057D8 RID: 22488 RVA: 0x001C4F3C File Offset: 0x001C313C
		private void InitVerbs(Func<Type, string, Verb> creator)
		{
			List<VerbProperties> verbProperties = this.directOwner.VerbProperties;
			if (verbProperties != null)
			{
				for (int i = 0; i < verbProperties.Count; i++)
				{
					try
					{
						VerbProperties verbProperties2 = verbProperties[i];
						string text = Verb.CalculateUniqueLoadID(this.directOwner, i);
						this.InitVerb(creator(verbProperties2.verbClass, text), verbProperties2, null, null, text);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate Verb (directOwner=";
						string str2 = this.directOwner.ToStringSafe<IVerbOwner>();
						string str3 = "): ";
						Exception ex2 = ex;
						Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			List<Tool> tools = this.directOwner.Tools;
			if (tools != null)
			{
				for (int j = 0; j < tools.Count; j++)
				{
					Tool tool = tools[j];
					foreach (ManeuverDef maneuverDef in tool.Maneuvers)
					{
						try
						{
							VerbProperties verb = maneuverDef.verb;
							string text2 = Verb.CalculateUniqueLoadID(this.directOwner, tool, maneuverDef);
							this.InitVerb(creator(verb.verbClass, text2), verb, tool, maneuverDef, text2);
						}
						catch (Exception ex3)
						{
							string str4 = "Could not instantiate Verb (directOwner=";
							string str5 = this.directOwner.ToStringSafe<IVerbOwner>();
							string str6 = "): ";
							Exception ex4 = ex3;
							Log.Error(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null));
						}
					}
				}
			}
		}

		// Token: 0x060057D9 RID: 22489 RVA: 0x001C50C8 File Offset: 0x001C32C8
		private void InitVerb(Verb verb, VerbProperties properties, Tool tool, ManeuverDef maneuver, string id)
		{
			verb.loadID = id;
			verb.verbProps = properties;
			verb.verbTracker = this;
			verb.tool = tool;
			verb.maneuver = maneuver;
			verb.caster = this.directOwner.ConstantCaster;
		}

		// Token: 0x060057DA RID: 22490 RVA: 0x001C5100 File Offset: 0x001C3300
		public void VerbsNeedReinitOnLoad()
		{
			this.verbs = null;
		}

		// Token: 0x04003FE5 RID: 16357
		public IVerbOwner directOwner;

		// Token: 0x04003FE6 RID: 16358
		private List<Verb> verbs;
	}
}
