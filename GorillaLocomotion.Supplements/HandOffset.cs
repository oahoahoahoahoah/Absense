using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000014 RID: 20
public static class HandOffset
{
	// Token: 0x06000086 RID: 134 RVA: 0x0000BD38 File Offset: 0x00009F38
	public static void Apply()
	{
		bool flag = !Settings.HandOffsetLeftEnabled && !Settings.HandOffsetRightEnabled;
		if (!flag)
		{
			try
			{
				bool flag2 = GorillaTagger.Instance == null || GorillaTagger.Instance.headCollider == null;
				if (!flag2)
				{
					GTPlayer player = GTPlayer.Instance;
					bool flag3 = player == null;
					if (!flag3)
					{
						Transform leftHand = player.GetControllerTransform(true);
						Transform rightHand = player.GetControllerTransform(false);
						bool flag4 = leftHand == null || rightHand == null;
						if (!flag4)
						{
							Transform head = GorillaTagger.Instance.headCollider.transform;
							Vector3 forward = head.forward;
							Vector3 up = head.up;
							Vector3 sideways = Vector3.Cross(forward, up);
							bool handOffsetLeftEnabled = Settings.HandOffsetLeftEnabled;
							if (handOffsetLeftEnabled)
							{
								leftHand.position += forward * Settings.HandOffsetLeftX + up * Settings.HandOffsetLeftY + sideways * Settings.HandOffsetLeftZ;
							}
							bool handOffsetRightEnabled = Settings.HandOffsetRightEnabled;
							if (handOffsetRightEnabled)
							{
								rightHand.position += forward * Settings.HandOffsetRightX + up * Settings.HandOffsetRightY + sideways * Settings.HandOffsetRightZ;
							}
						}
					}
				}
			}
			catch
			{
			}
		}
	}
}
