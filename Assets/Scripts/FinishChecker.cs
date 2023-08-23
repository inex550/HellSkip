using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishChecker : MonoBehaviour
{
    public Transform flag;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "player")
            StartCoroutine(FlagUp());
    }

    IEnumerator FlagUp()
    {
        float targerY = 1.2f;

        while (flag.position.y < targerY)
        {
            flag.transform.position = Vector3.MoveTowards(flag.position, new Vector3(flag.position.x, targerY, flag.position.z), Time.deltaTime * 2);
            yield return null;
        }

        //Новый уровень
    }
}
