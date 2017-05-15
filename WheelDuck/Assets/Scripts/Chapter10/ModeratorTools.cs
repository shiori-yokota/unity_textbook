using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TreasurePositionsInfo
{
    public string name;
    public Vector3 position;
    public Vector3 eulerAngles;
}

public class ModeratorTools {

    private static List<GameObject> treasureCandidates; // 全てのお宝候補 : 12
    private static List<GameObject> treasures;          // 実際に迷路上に置くお宝たち : 7

    private static List<GameObject> treasurePositions;  // お宝を迷路に置く位置 : 7

    public static List<GameObject> InitializeAndGetTreasures()
    {
        // 全てのお宝候補を取得
        ModeratorTools.treasureCandidates = GameObject.FindGameObjectsWithTag("Treasures").ToList<GameObject>();

        if (ModeratorTools.treasureCandidates.Count == 0)
        {
            throw new Exception("Count of TreasureCandidates is zero.");
        }

        // お宝を迷路に置く位置
        ModeratorTools.treasurePositions = GameObject.FindGameObjectsWithTag("TreasurePosition").ToList<GameObject>();

        return ModeratorTools.treasureCandidates;
    }

    public static void DeactivateTreasuresPositions()
    {
        foreach (GameObject treasurePosition in ModeratorTools.treasurePositions)
        {
            treasurePosition.SetActive(false);
        }
    }

    public static void DeactivateTreasuresCandidates(List<GameObject> objects)
    {
        foreach (GameObject treasure in ModeratorTools.treasureCandidates)
        {
            if (!objects.Contains(treasure)) treasure.SetActive(false);
        }
    }

    public static Dictionary<TreasurePositionsInfo, GameObject> CreateTreasuresPositionMap()
    {
        // お宝を置く位置を見えないようにする
        ModeratorTools.DeactivateTreasuresPositions();

        // 迷路上に置くお宝を決める
        // ModeratorTools.treasures = ModeratorTools.treasureCandidates.OrderBy(i => Guid.NewGuid()).ToList();
        ModeratorTools.treasures = new List<GameObject>();

        List <int> randomList = new List<int>();
        while (true)
        {
            int value = UnityEngine.Random.Range(0, ModeratorTools.treasureCandidates.Count);
            if (randomList.Contains(value)) continue;
            randomList.Add(value);
            treasures.Add(ModeratorTools.treasureCandidates[value]);
            if (randomList.Count == ModeratorTools.treasurePositions.Count) break;
        }

        // 配置しないお宝を見えないようにする
        ModeratorTools.DeactivateTreasuresCandidates(treasures);

        Dictionary<TreasurePositionsInfo, GameObject> treasureCandidatesMap = new Dictionary<TreasurePositionsInfo, GameObject>();
        for (int i = 0; i < ModeratorTools.treasures.Count; i++)
        {
            TreasurePositionsInfo treasurePositionInfo = new TreasurePositionsInfo();

            treasurePositionInfo.name = ModeratorTools.treasures[i].name;
            treasurePositionInfo.position = treasurePositions[i].transform.position;
            treasurePositionInfo.eulerAngles = treasurePositions[i].transform.eulerAngles;

            treasureCandidatesMap.Add(treasurePositionInfo, ModeratorTools.treasures[i]);
        }

        return treasureCandidatesMap;
    }
}
