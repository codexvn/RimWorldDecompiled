using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DB2 RID: 3506
	public abstract class ModuleBase : IDisposable
	{
		// Token: 0x17000E2F RID: 3631
		// (get) Token: 0x06005946 RID: 22854 RVA: 0x001CB64C File Offset: 0x001C984C
		public int SourceModuleCount
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules.Length;
				}
				return 0;
			}
		}

		// Token: 0x06005947 RID: 22855 RVA: 0x001CB660 File Offset: 0x001C9860
		protected ModuleBase(int count)
		{
			if (count > 0)
			{
				this.modules = new ModuleBase[count];
			}
		}

		// Token: 0x17000E30 RID: 3632
		public virtual ModuleBase this[int index]
		{
			get
			{
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (this.modules[index] == null)
				{
					throw new ArgumentNullException("Desired element is null");
				}
				return this.modules[index];
			}
			set
			{
				if (index < 0 || index >= this.modules.Length)
				{
					throw new ArgumentOutOfRangeException("Index out of valid module range");
				}
				if (value == null)
				{
					throw new ArgumentNullException("Value should not be null");
				}
				this.modules[index] = value;
			}
		}

		// Token: 0x0600594A RID: 22858
		public abstract double GetValue(double x, double y, double z);

		// Token: 0x0600594B RID: 22859 RVA: 0x001CB6E4 File Offset: 0x001C98E4
		public float GetValue(IntVec2 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, 0.0, (double)coordinate.z);
		}

		// Token: 0x0600594C RID: 22860 RVA: 0x001CB704 File Offset: 0x001C9904
		public float GetValue(IntVec3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x0600594D RID: 22861 RVA: 0x001CB722 File Offset: 0x001C9922
		public float GetValue(Vector3 coordinate)
		{
			return (float)this.GetValue((double)coordinate.x, (double)coordinate.y, (double)coordinate.z);
		}

		// Token: 0x17000E31 RID: 3633
		// (get) Token: 0x0600594E RID: 22862 RVA: 0x001CB740 File Offset: 0x001C9940
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x0600594F RID: 22863 RVA: 0x001CB748 File Offset: 0x001C9948
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005950 RID: 22864 RVA: 0x001CB764 File Offset: 0x001C9964
		protected virtual bool Disposing()
		{
			if (this.modules != null)
			{
				for (int i = 0; i < this.modules.Length; i++)
				{
					this.modules[i].Dispose();
					this.modules[i] = null;
				}
				this.modules = null;
			}
			return true;
		}

		// Token: 0x040040A3 RID: 16547
		protected ModuleBase[] modules;

		// Token: 0x040040A4 RID: 16548
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}
