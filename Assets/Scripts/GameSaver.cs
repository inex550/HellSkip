using UnityEngine;
using System;

public class GameSaver : MonoBehaviour
{
    public static PointsSaves pointsSaves;

    public static void LoadPoints()
    {
        if (!PlayerPrefs.HasKey("PointsSaves"))
            pointsSaves = new PointsSaves(0, 0);
        else
            pointsSaves = JsonUtility.FromJson<PointsSaves>(PlayerPrefs.GetString("PointsSaves"));
    }

    public static void SavePoints()
    {
        PlayerPrefs.SetString("PointsSaves", JsonUtility.ToJson(pointsSaves));
    }
}

[Serializable]
public class PointsSaves
{
    public int maxScore;
    public int moneys;

    public PointsSaves(int maxScore, int moneys)
    {
        this.maxScore = maxScore;
        this.moneys = moneys;
    }
}