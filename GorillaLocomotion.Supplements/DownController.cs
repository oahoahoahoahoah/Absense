using System;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000011 RID: 17
internal static class DownController
{
	// Token: 0x06000049 RID: 73 RVA: 0x00006390 File Offset: 0x00004590
	public static void Apply()
	{
		GTPlayer instance = GTPlayer.Instance;
		bool flag2 = instance == null;
		if (!flag2)
		{
			bool flag3 = !Settings.DownControllerEnabled;
			if (flag3)
			{
				DownController.leftOffset = Vector3.zero;
				DownController.rightOffset = Vector3.zero;
			}
			else
			{
				bool flag = Settings.IsPressed(Settings.DownControllerKeybind);
				float num = Mathf.Max(0f, Settings.DownControllerDistance) * 0.1f;
				Vector3 val = (flag ? (Vector3.down * num) : Vector3.zero);
				float num2 = Mathf.Max(0.01f, Settings.DownControllerSpeed);
				float num3 = Mathf.Clamp01(Time.deltaTime * num2);
				DownController.leftOffset = Vector3.Lerp(DownController.leftOffset, val, num3);
				DownController.rightOffset = Vector3.Lerp(DownController.rightOffset, val, num3);
				bool flag4 = DownController.leftOffset.sqrMagnitude < 1E-07f;
				if (flag4)
				{
					DownController.leftOffset = Vector3.zero;
				}
				bool flag5 = DownController.rightOffset.sqrMagnitude < 1E-07f;
				if (flag5)
				{
					DownController.rightOffset = Vector3.zero;
				}
				Transform controllerTransform = instance.LeftHand.controllerTransform;
				Transform controllerTransform2 = instance.RightHand.controllerTransform;
				bool flag6 = controllerTransform != null && DownController.leftOffset.sqrMagnitude > 0f;
				if (flag6)
				{
					controllerTransform.position += DownController.leftOffset;
				}
				bool flag7 = controllerTransform2 != null && DownController.rightOffset.sqrMagnitude > 0f;
				if (flag7)
				{
					controllerTransform2.position += DownController.rightOffset;
				}
			}
		}
	}

	// Token: 0x0400017B RID: 379
	private static Vector3 leftOffset = Vector3.zero;

	// Token: 0x0400017C RID: 380
	private static Vector3 rightOffset = Vector3.zero;
}
