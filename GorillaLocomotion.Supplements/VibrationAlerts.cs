using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x0200003E RID: 62
public static class VibrationAlerts
{
	// Token: 0x060001CC RID: 460 RVA: 0x0001FD3C File Offset: 0x0001DF3C
	public static void Update()
	{
		bool flag = !Settings.VibrationAlertsEnabled || GorillaTagger.Instance == null || GorillaTagger.Instance.offlineVRRig == null || GorillaParent.instance == null || Time.time - VibrationAlerts.lastAlertTime < VibrationAlerts.cooldown || VibrationAlerts.IsTagged(GorillaTagger.Instance.offlineVRRig);
		if (!flag)
		{
			Vector3 position = GorillaTagger.Instance.bodyCollider.transform.position;
			bool flag2 = VRRigCache.ActiveRigs == null;
			if (!flag2)
			{
				foreach (VRRig activeRig in VRRigCache.ActiveRigs)
				{
					bool flag3 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig) && !(activeRig.headMesh == null) && VibrationAlerts.IsTagged(activeRig);
					if (flag3)
					{
						float num = Vector3.Distance(activeRig.headMesh.transform.position, position);
						bool flag4 = num <= Settings.VibrationAlertDistance;
						if (flag4)
						{
							VibrationAlerts.Pulse(Mathf.Clamp01(1f - num / Settings.VibrationAlertDistance));
							VibrationAlerts.lastAlertTime = Time.time;
							break;
						}
					}
				}
			}
		}
	}

	// Token: 0x060001CD RID: 461 RVA: 0x0001FEA8 File Offset: 0x0001E0A8
	private static bool IsTagged(VRRig rig)
	{
		bool flag = !(rig != null);
		bool flag2;
		if (flag)
		{
			flag2 = false;
		}
		else
		{
			bool flag3 = !(rig.mainSkin != null);
			if (flag3)
			{
				flag2 = false;
			}
			else
			{
				bool flag4 = !(rig.mainSkin.material != null);
				flag2 = !flag4 && rig.mainSkin.material.name.ToLower().Contains("fected");
			}
		}
		return flag2;
	}

	// Token: 0x060001CE RID: 462 RVA: 0x0001FF24 File Offset: 0x0001E124
	private static void Pulse(float strength)
	{
		try
		{
			InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(4);
			InputDevice deviceAtXRNode2 = InputDevices.GetDeviceAtXRNode(5);
			float num = strength * Settings.VibrationAlertStrength;
			bool isValid = deviceAtXRNode.isValid;
			if (isValid)
			{
				deviceAtXRNode.SendHapticImpulse(0U, num, 0.1f);
			}
			bool isValid2 = deviceAtXRNode2.isValid;
			if (isValid2)
			{
				deviceAtXRNode2.SendHapticImpulse(0U, num, 0.1f);
			}
		}
		catch
		{
		}
	}

	// Token: 0x04000491 RID: 1169
	private static float lastAlertTime = 0f;

	// Token: 0x04000492 RID: 1170
	private static float cooldown = 0.5f;
}
