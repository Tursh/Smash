using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickEventArgs : EventArgs { }

public class TickSystem : MonoBehaviour
{
	[SerializeField]
	private float tps = 60;
	private float lastTime;
	public EventHandler<TickEventArgs> TickEvent;

    void Update()
    {
        while(Time.time - lastTime > 1/tps)
		{
			TickEvent?.Invoke(this, new TickEventArgs());

			lastTime += 1/tps;

		}
    }
}
