using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C78 RID: 3192
	public struct DamageInfo
	{
		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06005181 RID: 20865 RVA: 0x001A787C File Offset: 0x001A5A7C
		// (set) Token: 0x06005182 RID: 20866 RVA: 0x001A7884 File Offset: 0x001A5A84
		public DamageDef Def
		{
			get
			{
				return this.defInt;
			}
			set
			{
				this.defInt = value;
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06005183 RID: 20867 RVA: 0x001A788D File Offset: 0x001A5A8D
		public float Amount
		{
			get
			{
				if (!DebugSettings.enableDamage)
				{
					return 0f;
				}
				return this.amountInt;
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06005184 RID: 20868 RVA: 0x001A78A2 File Offset: 0x001A5AA2
		public float ArmorPenetrationInt
		{
			get
			{
				return this.armorPenetrationInt;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x06005185 RID: 20869 RVA: 0x001A78AA File Offset: 0x001A5AAA
		public Thing Instigator
		{
			get
			{
				return this.instigatorInt;
			}
		}

		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x06005186 RID: 20870 RVA: 0x001A78B2 File Offset: 0x001A5AB2
		public DamageInfo.SourceCategory Category
		{
			get
			{
				return this.categoryInt;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x06005187 RID: 20871 RVA: 0x001A78BA File Offset: 0x001A5ABA
		public Thing IntendedTarget
		{
			get
			{
				return this.intendedTargetInt;
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x06005188 RID: 20872 RVA: 0x001A78C2 File Offset: 0x001A5AC2
		public float Angle
		{
			get
			{
				return this.angleInt;
			}
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x06005189 RID: 20873 RVA: 0x001A78CA File Offset: 0x001A5ACA
		public BodyPartRecord HitPart
		{
			get
			{
				return this.hitPartInt;
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x0600518A RID: 20874 RVA: 0x001A78D2 File Offset: 0x001A5AD2
		public BodyPartHeight Height
		{
			get
			{
				return this.heightInt;
			}
		}

		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x0600518B RID: 20875 RVA: 0x001A78DA File Offset: 0x001A5ADA
		public BodyPartDepth Depth
		{
			get
			{
				return this.depthInt;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x0600518C RID: 20876 RVA: 0x001A78E2 File Offset: 0x001A5AE2
		public ThingDef Weapon
		{
			get
			{
				return this.weaponInt;
			}
		}

		// Token: 0x17000C60 RID: 3168
		// (get) Token: 0x0600518D RID: 20877 RVA: 0x001A78EA File Offset: 0x001A5AEA
		public BodyPartGroupDef WeaponBodyPartGroup
		{
			get
			{
				return this.weaponBodyPartGroupInt;
			}
		}

		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x001A78F2 File Offset: 0x001A5AF2
		public HediffDef WeaponLinkedHediff
		{
			get
			{
				return this.weaponHediffInt;
			}
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x001A78FA File Offset: 0x001A5AFA
		public Tool Tool
		{
			get
			{
				return this.tool;
			}
		}

		// Token: 0x17000C63 RID: 3171
		// (get) Token: 0x06005190 RID: 20880 RVA: 0x001A7902 File Offset: 0x001A5B02
		public bool InstantPermanentInjury
		{
			get
			{
				return this.instantPermanentInjuryInt;
			}
		}

		// Token: 0x17000C64 RID: 3172
		// (get) Token: 0x06005191 RID: 20881 RVA: 0x001A790A File Offset: 0x001A5B0A
		public bool InstigatorGuilty
		{
			get
			{
				return this.instigatorGuilty;
			}
		}

		// Token: 0x17000C65 RID: 3173
		// (get) Token: 0x06005192 RID: 20882 RVA: 0x001A7912 File Offset: 0x001A5B12
		public bool SpawnFilth
		{
			get
			{
				return this.spawnFilth;
			}
		}

		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x06005193 RID: 20883 RVA: 0x001A791A File Offset: 0x001A5B1A
		public bool CheckForJobOverride
		{
			get
			{
				return this.checkForJobOverride;
			}
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x06005194 RID: 20884 RVA: 0x001A7922 File Offset: 0x001A5B22
		public QualityCategory WeaponQuality
		{
			get
			{
				return this.weaponQuality;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x06005195 RID: 20885 RVA: 0x001A792A File Offset: 0x001A5B2A
		public IntRange DamagePropagationPartsRange
		{
			get
			{
				return this.damagePropagationPartsRange;
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x06005196 RID: 20886 RVA: 0x001A7932 File Offset: 0x001A5B32
		public bool AllowDamagePropagation
		{
			get
			{
				return !this.InstantPermanentInjury && this.allowDamagePropagationInt;
			}
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x06005197 RID: 20887 RVA: 0x001A7944 File Offset: 0x001A5B44
		public bool PreventCascade
		{
			get
			{
				return this.preventCascadeInt;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x06005198 RID: 20888 RVA: 0x001A794C File Offset: 0x001A5B4C
		public bool ApplyAllDamage
		{
			get
			{
				return this.applyAllDamage;
			}
		}

		// Token: 0x17000C6C RID: 3180
		// (get) Token: 0x06005199 RID: 20889 RVA: 0x001A7954 File Offset: 0x001A5B54
		public bool IgnoreArmor
		{
			get
			{
				return this.ignoreArmorInt;
			}
		}

		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x0600519A RID: 20890 RVA: 0x001A795C File Offset: 0x001A5B5C
		public bool IgnoreInstantKillProtection
		{
			get
			{
				return this.ignoreInstantKillProtectionInt;
			}
		}

		// Token: 0x0600519B RID: 20891 RVA: 0x001A7964 File Offset: 0x001A5B64
		public DamageInfo(DamageDef def, float amount, float armorPenetration = 0f, float angle = -1f, Thing instigator = null, BodyPartRecord hitPart = null, ThingDef weapon = null, DamageInfo.SourceCategory category = DamageInfo.SourceCategory.ThingOrUnknown, Thing intendedTarget = null, bool instigatorGuilty = true, bool spawnFilth = true, QualityCategory weaponQuality = QualityCategory.Normal, bool checkForJobOverride = true, bool preventCascade = false)
		{
			this.defInt = def;
			this.amountInt = amount;
			this.armorPenetrationInt = armorPenetration;
			if (angle < 0f)
			{
				this.angleInt = (float)Rand.RangeInclusive(0, 359);
			}
			else
			{
				this.angleInt = angle;
			}
			this.instigatorInt = instigator;
			this.categoryInt = category;
			this.hitPartInt = hitPart;
			this.heightInt = BodyPartHeight.Undefined;
			this.depthInt = BodyPartDepth.Undefined;
			this.weaponInt = weapon;
			this.weaponBodyPartGroupInt = null;
			this.weaponHediffInt = null;
			this.tool = null;
			this.weaponQuality = weaponQuality;
			this.instantPermanentInjuryInt = false;
			this.allowDamagePropagationInt = true;
			this.preventCascadeInt = preventCascade;
			this.ignoreArmorInt = false;
			this.ignoreInstantKillProtectionInt = false;
			this.instigatorGuilty = instigatorGuilty;
			this.intendedTargetInt = intendedTarget;
			this.spawnFilth = spawnFilth;
			this.checkForJobOverride = checkForJobOverride;
			this.damagePropagationPartsRange = new IntRange(2, 4);
			this.applyAllDamage = false;
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x001A7A50 File Offset: 0x001A5C50
		public DamageInfo(DamageInfo cloneSource)
		{
			this.defInt = cloneSource.defInt;
			this.amountInt = cloneSource.amountInt;
			this.armorPenetrationInt = cloneSource.armorPenetrationInt;
			this.angleInt = cloneSource.angleInt;
			this.instigatorInt = cloneSource.instigatorInt;
			this.categoryInt = cloneSource.categoryInt;
			this.hitPartInt = cloneSource.hitPartInt;
			this.heightInt = cloneSource.heightInt;
			this.depthInt = cloneSource.depthInt;
			this.weaponInt = cloneSource.weaponInt;
			this.weaponBodyPartGroupInt = cloneSource.weaponBodyPartGroupInt;
			this.weaponHediffInt = cloneSource.weaponHediffInt;
			this.tool = cloneSource.tool;
			this.weaponQuality = cloneSource.weaponQuality;
			this.instantPermanentInjuryInt = cloneSource.instantPermanentInjuryInt;
			this.allowDamagePropagationInt = cloneSource.allowDamagePropagationInt;
			this.preventCascadeInt = cloneSource.preventCascadeInt;
			this.intendedTargetInt = cloneSource.intendedTargetInt;
			this.ignoreArmorInt = cloneSource.ignoreArmorInt;
			this.ignoreInstantKillProtectionInt = cloneSource.ignoreInstantKillProtectionInt;
			this.instigatorGuilty = cloneSource.instigatorGuilty;
			this.spawnFilth = cloneSource.spawnFilth;
			this.checkForJobOverride = cloneSource.checkForJobOverride;
			this.damagePropagationPartsRange = cloneSource.damagePropagationPartsRange;
			this.applyAllDamage = cloneSource.applyAllDamage;
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x001A7B89 File Offset: 0x001A5D89
		public void SetAmount(float newAmount)
		{
			this.amountInt = newAmount;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x001A7B92 File Offset: 0x001A5D92
		public void SetIgnoreArmor(bool ignoreArmor)
		{
			this.ignoreArmorInt = ignoreArmor;
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x001A7B9B File Offset: 0x001A5D9B
		public void SetIgnoreInstantKillProtection(bool ignore)
		{
			this.ignoreInstantKillProtectionInt = ignore;
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x001A7BA4 File Offset: 0x001A5DA4
		public void SetBodyRegion(BodyPartHeight height = BodyPartHeight.Undefined, BodyPartDepth depth = BodyPartDepth.Undefined)
		{
			this.heightInt = height;
			this.depthInt = depth;
		}

		// Token: 0x060051A1 RID: 20897 RVA: 0x001A7BB4 File Offset: 0x001A5DB4
		public void SetHitPart(BodyPartRecord forceHitPart)
		{
			this.hitPartInt = forceHitPart;
		}

		// Token: 0x060051A2 RID: 20898 RVA: 0x001A7BBD File Offset: 0x001A5DBD
		public void SetInstantPermanentInjury(bool val)
		{
			this.instantPermanentInjuryInt = val;
		}

		// Token: 0x060051A3 RID: 20899 RVA: 0x001A7BC6 File Offset: 0x001A5DC6
		public void SetWeaponBodyPartGroup(BodyPartGroupDef gr)
		{
			this.weaponBodyPartGroupInt = gr;
		}

		// Token: 0x060051A4 RID: 20900 RVA: 0x001A7BCF File Offset: 0x001A5DCF
		public void SetWeaponHediff(HediffDef hd)
		{
			this.weaponHediffInt = hd;
		}

		// Token: 0x060051A5 RID: 20901 RVA: 0x001A7BD8 File Offset: 0x001A5DD8
		public void SetTool(Tool tool)
		{
			this.tool = tool;
		}

		// Token: 0x060051A6 RID: 20902 RVA: 0x001A7BE1 File Offset: 0x001A5DE1
		public void SetWeaponQuality(QualityCategory weaponQuality)
		{
			this.weaponQuality = weaponQuality;
		}

		// Token: 0x060051A7 RID: 20903 RVA: 0x001A7BEA File Offset: 0x001A5DEA
		public void SetAllowDamagePropagation(bool val)
		{
			this.allowDamagePropagationInt = val;
		}

		// Token: 0x060051A8 RID: 20904 RVA: 0x001A7BF3 File Offset: 0x001A5DF3
		public void SetAllowDamagePropagation(bool val, IntRange partsRange)
		{
			this.allowDamagePropagationInt = val;
			this.damagePropagationPartsRange = partsRange;
		}

		// Token: 0x060051A9 RID: 20905 RVA: 0x001A7C03 File Offset: 0x001A5E03
		public void SetApplyAllDamage(bool value)
		{
			this.allowDamagePropagationInt = value;
			this.applyAllDamage = value;
		}

		// Token: 0x060051AA RID: 20906 RVA: 0x001A7C14 File Offset: 0x001A5E14
		public void SetAngle(Vector3 vec)
		{
			if (vec.x != 0f || vec.z != 0f)
			{
				this.angleInt = Quaternion.LookRotation(vec).eulerAngles.y;
				return;
			}
			this.angleInt = (float)Rand.RangeInclusive(0, 359);
		}

		// Token: 0x060051AB RID: 20907 RVA: 0x001A7C68 File Offset: 0x001A5E68
		public override string ToString()
		{
			string[] array = new string[9];
			array[0] = "(def=";
			int num = 1;
			DamageDef damageDef = this.defInt;
			array[num] = ((damageDef != null) ? damageDef.ToString() : null);
			array[2] = ", amount= ";
			array[3] = this.amountInt.ToString();
			array[4] = ", instigator=";
			array[5] = ((this.instigatorInt != null) ? this.instigatorInt.ToString() : this.categoryInt.ToString());
			array[6] = ", angle=";
			array[7] = this.angleInt.ToString("F1");
			array[8] = ")";
			return string.Concat(array);
		}

		// Token: 0x04003AAF RID: 15023
		private DamageDef defInt;

		// Token: 0x04003AB0 RID: 15024
		private float amountInt;

		// Token: 0x04003AB1 RID: 15025
		private float armorPenetrationInt;

		// Token: 0x04003AB2 RID: 15026
		private float angleInt;

		// Token: 0x04003AB3 RID: 15027
		private Thing instigatorInt;

		// Token: 0x04003AB4 RID: 15028
		private DamageInfo.SourceCategory categoryInt;

		// Token: 0x04003AB5 RID: 15029
		public Thing intendedTargetInt;

		// Token: 0x04003AB6 RID: 15030
		private bool ignoreArmorInt;

		// Token: 0x04003AB7 RID: 15031
		private bool ignoreInstantKillProtectionInt;

		// Token: 0x04003AB8 RID: 15032
		private BodyPartRecord hitPartInt;

		// Token: 0x04003AB9 RID: 15033
		private BodyPartHeight heightInt;

		// Token: 0x04003ABA RID: 15034
		private BodyPartDepth depthInt;

		// Token: 0x04003ABB RID: 15035
		private ThingDef weaponInt;

		// Token: 0x04003ABC RID: 15036
		private BodyPartGroupDef weaponBodyPartGroupInt;

		// Token: 0x04003ABD RID: 15037
		private HediffDef weaponHediffInt;

		// Token: 0x04003ABE RID: 15038
		private Tool tool;

		// Token: 0x04003ABF RID: 15039
		private QualityCategory weaponQuality;

		// Token: 0x04003AC0 RID: 15040
		private bool instantPermanentInjuryInt;

		// Token: 0x04003AC1 RID: 15041
		private bool allowDamagePropagationInt;

		// Token: 0x04003AC2 RID: 15042
		private bool preventCascadeInt;

		// Token: 0x04003AC3 RID: 15043
		private bool instigatorGuilty;

		// Token: 0x04003AC4 RID: 15044
		private bool spawnFilth;

		// Token: 0x04003AC5 RID: 15045
		private bool checkForJobOverride;

		// Token: 0x04003AC6 RID: 15046
		private bool applyAllDamage;

		// Token: 0x04003AC7 RID: 15047
		private IntRange damagePropagationPartsRange;

		// Token: 0x02000C79 RID: 3193
		public enum SourceCategory
		{
			// Token: 0x04003AC9 RID: 15049
			ThingOrUnknown,
			// Token: 0x04003ACA RID: 15050
			Collapse
		}
	}
}
