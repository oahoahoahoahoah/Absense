using System;
using GorillaLocomotion;

// Token: 0x02000034 RID: 52
public static class SlideControl
{
	// Token: 0x0600018C RID: 396 RVA: 0x0001BAE4 File Offset: 0x00019CE4
	public static void Update()
	{
		bool slideControlEnabled = Settings.SlideControlEnabled;
		if (slideControlEnabled)
		{
			bool flag = GorillaTagger.Instance == null;
			if (!flag)
			{
				try
				{
					GTPlayer player = GTPlayer.Instance;
					bool flag2 = player != null;
					if (flag2)
					{
						bool flag3 = !SlideControl.captured;
						if (flag3)
						{
							SlideControl.originalSlideControl = player.slideControl;
							SlideControl.captured = true;
						}
						player.slideControl = Settings.SlideControlAmount;
					}
				}
				catch
				{
				}
			}
		}
		else
		{
			bool flag4 = !SlideControl.captured;
			if (!flag4)
			{
				try
				{
					GTPlayer player2 = GTPlayer.Instance;
					bool flag5 = player2 != null && SlideControl.originalSlideControl >= 0f;
					if (flag5)
					{
						player2.slideControl = SlideControl.originalSlideControl;
					}
				}
				catch
				{
				}
				SlideControl.captured = false;
			}
		}
	}

	// Token: 0x04000430 RID: 1072
	private static bool captured = false;

	// Token: 0x04000431 RID: 1073
	private static float originalSlideControl = -1f;
}
