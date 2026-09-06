using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000023 RID: 35
public static class NoSlip
{
	// Token: 0x060000FF RID: 255 RVA: 0x00013E5C File Offset: 0x0001205C
	public static void Apply()
	{
		try
		{
			bool flag = !Settings.NoSlipEnabled;
			if (flag)
			{
				bool flag2 = NoSlip.active;
				if (flag2)
				{
					NoSlip.Restore();
				}
			}
			else
			{
				bool flag3 = NoSlip.active;
				if (!flag3)
				{
					NoSlip.savedSurfaces.Clear();
					NoSlip.DisableSlipOn(GameObject.Find("pit lower slippery wall"));
					NoSlip.DisableSlipOn(GameObject.Find("pit upper slippery wall"));
					NoSlip.DisableSlipOn(GameObject.Find("Map (1)/Ice"));
					NoSlip.active = true;
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00013EF0 File Offset: 0x000120F0
	private static void DisableSlipOn(GameObject gameObject)
	{
		bool flag = gameObject == null;
		if (!flag)
		{
			GorillaSurfaceOverride surface = gameObject.GetComponent<GorillaSurfaceOverride>();
			bool flag2 = surface == null;
			if (!flag2)
			{
				NoSlip.savedSurfaces.Add(new NoSlip.SavedSurface
				{
					Surface = surface,
					OriginalSlide = surface.slidePercentageOverride,
					OriginalIndex = surface.overrideIndex
				});
				surface.slidePercentageOverride = 0f;
				surface.overrideIndex = 0;
			}
		}
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00013F6C File Offset: 0x0001216C
	public static void Restore()
	{
		foreach (NoSlip.SavedSurface saved in NoSlip.savedSurfaces)
		{
			bool flag = saved.Surface != null;
			if (flag)
			{
				saved.Surface.slidePercentageOverride = saved.OriginalSlide;
				saved.Surface.overrideIndex = saved.OriginalIndex;
			}
		}
		NoSlip.savedSurfaces.Clear();
		NoSlip.active = false;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00014004 File Offset: 0x00012204
	public static void Reset()
	{
		NoSlip.Restore();
	}

	// Token: 0x04000230 RID: 560
	private static bool active;

	// Token: 0x04000231 RID: 561
	private static readonly List<NoSlip.SavedSurface> savedSurfaces = new List<NoSlip.SavedSurface>();

	// Token: 0x02000071 RID: 113
	private struct SavedSurface
	{
		// Token: 0x0400052B RID: 1323
		public GorillaSurfaceOverride Surface;

		// Token: 0x0400052C RID: 1324
		public float OriginalSlide;

		// Token: 0x0400052D RID: 1325
		public int OriginalIndex;
	}
}
