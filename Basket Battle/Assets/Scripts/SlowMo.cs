using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMo : MonoBehaviour
{
    public float slowMoValue;

    public void SlowMotion(bool log)
    {
        if (log)
        {
            Time.timeScale = slowMoValue;
            Time.fixedDeltaTime *= Time.timeScale;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }
}
