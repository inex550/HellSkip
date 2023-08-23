using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapOrPlatformController : MonoBehaviour
{
    float time = 0;

    public float timer;

    public GameObject trap;
    public GameObject platform;

    private void Update()
    {
        time += Time.deltaTime;

        if (time >= timer)
        {
            if (tag == "Trap")
            {
                tag = "platform";

                trap.SetActive(false);
                platform.SetActive(true);
            }
            else if (tag == "platform")
            {
                tag = "Trap";

                trap.SetActive(true);
                platform.SetActive(false);
            }

            time = 0;
        }
    }
}
