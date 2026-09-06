using System;
using UnityEngine;

// Token: 0x02000037 RID: 55
public static class SurfaceSlip
{
	// Token: 0x0600019C RID: 412 RVA: 0x0001C4E0 File Offset: 0x0001A6E0
	public static void Update()
	{
		bool surfaceSlipEnabled = Settings.SurfaceSlipEnabled;
		if (surfaceSlipEnabled)
		{
			SurfaceSlip.SurfaceEntry[] array = SurfaceSlip.entries;
			int num2 = 0;
			for (;;)
			{
				bool flag = num2 >= array.Length;
				if (flag)
				{
					break;
				}
				SurfaceSlip.SurfaceEntry @class = array[num2];
				bool flag2 = !(@class.Surface == null);
				if (!flag2)
				{
					GameObject val = GameObject.Find(@class.ObjectName);
					bool flag3 = val != null;
					if (flag3)
					{
						@class.Surface = val.GetComponent<GorillaSurfaceOverride>();
						bool flag4 = !(@class.Surface != null);
						if (!flag4)
						{
							bool flag5 = !float.IsNaN(@class.DefaultSlide);
							if (flag5)
							{
							}
						}
					}
				}
				num2++;
			}
			SurfaceSlip.active = true;
			SurfaceSlip.SurfaceEntry[] array2 = SurfaceSlip.entries;
			SurfaceSlip.ApplyEntry(array2[0], Settings.SurfaceSlipWalls);
			SurfaceSlip.SurfaceEntry[] array3 = SurfaceSlip.entries;
			SurfaceSlip.ApplyEntry(array3[1], Settings.SurfaceSlipLowerSlippery);
			SurfaceSlip.SurfaceEntry[] array4 = SurfaceSlip.entries;
			SurfaceSlip.ApplyEntry(array4[2], Settings.SurfaceSlipUpperSlippery);
		}
		else
		{
			bool flag6 = !SurfaceSlip.active;
			if (!flag6)
			{
				SurfaceSlip.Restore();
			}
		}
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0001C614 File Offset: 0x0001A814
	private static void ApplyEntry(SurfaceSlip.SurfaceEntry entry, float value)
	{
		bool flag = entry.Surface == null;
		if (flag)
		{
		}
	}

	// Token: 0x0600019E RID: 414 RVA: 0x0001C638 File Offset: 0x0001A838
	private static void Restore()
	{
		SurfaceSlip.SurfaceEntry[] array = SurfaceSlip.entries;
		int num2 = 0;
		for (;;)
		{
			bool flag = num2 < array.Length;
			if (!flag)
			{
				break;
			}
			SurfaceSlip.SurfaceEntry @class = array[num2];
			bool flag2 = @class.Surface != null;
			if (flag2)
			{
				bool flag3 = !float.IsNaN(@class.DefaultSlide);
				if (flag3)
				{
				}
				num2++;
			}
			else
			{
				num2++;
			}
		}
	}

	// Token: 0x0600019F RID: 415 RVA: 0x0001C6A5 File Offset: 0x0001A8A5
	public static void Reset()
	{
		SurfaceSlip.Restore();
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0001C6B0 File Offset: 0x0001A8B0
	public static float GetSlideFor(int index)
	{
		bool flag = index < 0;
		float num;
		if (flag)
		{
			num = 0f;
		}
		else
		{
			bool flag2 = index < SurfaceSlip.entries.Length;
			if (flag2)
			{
				bool flag3 = float.IsNaN(SurfaceSlip.entries[index].DefaultSlide);
				if (flag3)
				{
					num = 0f;
				}
				else
				{
					num = SurfaceSlip.entries[index].DefaultSlide;
				}
			}
			else
			{
				num = 0f;
			}
		}
		return num;
	}

	// Token: 0x04000443 RID: 1091
	private static readonly SurfaceSlip.SurfaceEntry[] entries = new SurfaceSlip.SurfaceEntry[]
	{
		new SurfaceSlip.SurfaceEntry
		{
			ObjectName = "sa"
		},
		new SurfaceSlip.SurfaceEntry
		{
			ObjectName = "pit lower slippery walls"
		},
		new SurfaceSlip.SurfaceEntry
		{
			ObjectName = "pit upper slippery wall"
		}
	};

	// Token: 0x04000444 RID: 1092
	private static bool active = false;

	// Token: 0x0200007C RID: 124
	private class SurfaceEntry
	{
		// Token: 0x0400053E RID: 1342
		public string ObjectName;

		// Token: 0x0400053F RID: 1343
		public GorillaSurfaceOverride Surface;

		// Token: 0x04000540 RID: 1344
		public float DefaultSlide = float.NaN;
	}
}
