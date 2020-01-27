using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsLock : MonoBehaviour
{
	[SerializeField]
	int targetFPS = 60;
	[SerializeField]
	bool vSyncEnabled = false;

	void Awake()
	{
		QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
		Application.targetFrameRate = targetFPS;
	}
}
