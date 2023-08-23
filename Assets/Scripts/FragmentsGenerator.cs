using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FragmentsGenerator : MonoBehaviour
{
    public List<GameObject> lvlFragments;
    public List<int> lvlFragmentChance;

    public List<GameObject> fragmentsWithChance = new List<GameObject>();

    public GameObject player;
    public float distanceFromPlayer;

    public Transform startPosition;
    Vector3 currentPosition;

    List<GameObject> currentFragments = new List<GameObject>();

    private void Start()
    {
        currentPosition = startPosition.position;

        if (lvlFragments.Count != lvlFragmentChance.Count)
            Debug.LogError("Кол-во элементов в lvlFragments и lvlFragmentsChance у объекта FragmentsGenerator должно совпадать");

        for (int i = 0; i < lvlFragments.Count; i++)
            for (int chance = 0; chance < lvlFragmentChance[i]; chance++)
                fragmentsWithChance.Add(lvlFragments[i]);

        while (true)
        {
            bool spawned = Spawn();
            if (!spawned)
                break;
        }
    }

    public bool Spawn()
    {
        if (currentPosition.z <= player.transform.position.z + distanceFromPlayer)
        {
            int fragmentNum = Random.Range(0, fragmentsWithChance.Count);

            GameObject lvlFragment = Instantiate(fragmentsWithChance[fragmentNum], transform);
            lvlFragment.transform.position = currentPosition;

            currentFragments.Add(lvlFragment);

            currentPosition += new Vector3(0, 0, 9f);

            return true;
        }

        for (int i = 0; i < currentFragments.Count; i++)
        {
            if (player.transform.position.z - 5 > currentFragments[i].transform.position.z + 10)
            {
                Destroy(currentFragments[i]);
                currentFragments.RemoveAt(i);

                i -= 1;
            }
        }

        return false;
    }
}
