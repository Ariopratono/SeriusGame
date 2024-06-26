using System;
using SVS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeighted[] housesPrefabs, PabrikPrefabs, treesPrefabs;
    public PlacementManager placementManager;

    private float[] houseWeights, pabrikWeights, treesWeights;

    private void Start()
    {
        houseWeights = housesPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        pabrikWeights = PabrikPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        treesWeights = treesPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
    }

    public void PlaceHouse(Vector3Int position)
    {
        int price = StatsManager.Instance.housePrice;
        if (StatsManager.Instance.coin < price)
        {
            NotificationHandler.Instance.SendNotification("Need " + price + " coins!");
            Debug.Log("Need " + price + " coins!");
            return;
        }
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeightedIndex(houseWeights);
            placementManager.PlaceObjectOnTheMap(position, housesPrefabs[randomIndex].prefab, CellType.Structure, buildingPrefabindex:randomIndex);
            // AudioPlayer.instance.PlayPlacementSound();

            StatsManager.Instance.PlacedHouse();
        }
    }

    public void PlacePabrik(Vector3Int position)
    {
        int price = StatsManager.Instance.pabrikPrice;
        if(StatsManager.Instance.coin < price)
        {
            NotificationHandler.Instance.SendNotification("Need " + price + " coins!");
            Debug.Log("Need " + price + " coins!");
            return;
        }
        if (CheckPositionBeforePlacement(position))
        {
            int randomIndex = GetRandomWeightedIndex(pabrikWeights);
            placementManager.PlaceObjectOnTheMap(position, PabrikPrefabs[randomIndex].prefab, CellType.Structure, buildingPrefabindex: randomIndex);
            // AudioPlayer.instance.PlayPlacementSound();

            StatsManager.Instance.PlacedPabrik();
        }
    }

    public void PlaceTrees(Vector3Int position)
    {
        int price = StatsManager.Instance.treesPrice;
        if (StatsManager.Instance.coin < price)
        {
            NotificationHandler.Instance.SendNotification("Need " + price + " coins!");
            Debug.Log("Need " + price + " coins!");
            return;
        }
        if (CheckTreePosition(position))
        {
            int randomIndex = GetRandomWeightedIndex(treesWeights);
            placementManager.PlaceObjectOnTheMap(position, treesPrefabs[randomIndex].prefab, CellType.Structure);
            // AudioPlayer.instance.PlayPlacementSound();

            StatsManager.Instance.PlacedTrees();
        }
    }

    private int GetRandomWeightedIndex(float[] weights)
    {
        float sum = 0f;
        for(int i = 0; i < weights.Length; i++)
        {
            sum += weights[i];
        }
        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempSum = 0;
        for (int i = 0; i<weights.Length; i++)
        {
            //0->weight[0] weight[0]->weight[0] + weight[1]
            if(randomValue >= tempSum && randomValue < tempSum + weights[i])
            {
                return i;
            }
            tempSum += weights[i];
        }
        return 0;
    }

    private bool CheckPositionBeforePlacement(Vector3Int position)
    {                                                                   
        if (placementManager.CheckIfPositionInBound(position)== false)
        {
            NotificationHandler.Instance.SendNotification("This Position is out of bounds");
            Debug.Log("This Position is out of bounds");
            return false;
        }
        if (placementManager.CheckIfPositionIsFree(position)== false)
        {
            NotificationHandler.Instance.SendNotification("This Position is not empty");
            Debug.Log("This position is not empty");
            return false;
        }
        if (placementManager.GetNeighboursOfTypeFor(position,CellType.Road).Count <= 0)
        {
            NotificationHandler.Instance.SendNotification("Must be placed near a road");
            Debug.Log("Must be placed near a road");
            return false;
        }
        return true;
    }

    private bool CheckTreePosition(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
        {
            NotificationHandler.Instance.SendNotification("This Position is out of bounds");
            Debug.Log("This Position is out of bounds");
            return false;
        }
        if (placementManager.CheckIfPositionIsFree(position) == false)
        {
            NotificationHandler.Instance.SendNotification("This position is not empty");
            Debug.Log("This position is not empty");
            return false;
        }
        return true;
    }

    internal void PlaceLoadedStructure(Vector3Int position, int buildingPrefabindex, CellType buildingType)
    {
        switch (buildingType)
        {
            case CellType.Structure:
                placementManager.PlaceObjectOnTheMap(position, housesPrefabs[buildingPrefabindex].prefab, CellType.Structure, buildingPrefabindex:buildingPrefabindex);
                break;
            case CellType.SpecialStructure:
                placementManager.PlaceObjectOnTheMap(position, PabrikPrefabs[buildingPrefabindex].prefab, CellType.Structure, buildingPrefabindex:buildingPrefabindex);
                break;
            // case CellType.TreeStructure:
            //     placementManager.PlaceObjectOnTheMap(position, treesPrefabs[buildingPrefabindex].prefab, CellType.Structure, buildingPrefabindex:buildingPrefabindex);
            //     break;
            default:
                break;
        }
    }

    public Dictionary<Vector3Int, StructureModel> GetAllStructures()
    {
        return placementManager.GetAllStructures();     
    }
}

[Serializable]
public struct StructurePrefabWeighted
{
    public GameObject prefab;
    [Range(0,1)]
    public float weight;
}
