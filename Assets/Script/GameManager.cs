using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public RoadManager roadManager;
    public InputManager inputManager;

    public UIController uiController;

    public StructureManager structureManager;

    public SaveSystem saveSystem;

    private void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler;
        uiController.OnHousePlacement += HousePlacementHandler;
        uiController.OnPabrikPlacement += PabrikPlacementHandler;
        uiController.OnTreesPlacement += TreesPlacementHandler;
    }

    private void PabrikPlacementHandler()
    {
        ClearInputActions();
        Debug.Log("Bisa");
        inputManager.OnMouseClick += structureManager.PlacePabrik;
    }

    private void HousePlacementHandler()
    {
        Debug.Log("Bisa");
        ClearInputActions();
        inputManager.OnMouseClick += structureManager.PlaceHouse;
        
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();
        Debug.Log("Bisa");
        inputManager.OnMouseClick += roadManager.PlaceRoad;
        inputManager.OnMouseHold += roadManager.PlaceRoad;
        inputManager.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void TreesPlacementHandler()
    {
        ClearInputActions();
        Debug.Log("Bisa");
        inputManager.OnMouseClick += structureManager.PlaceTrees;
    }

    private void ClearInputActions()
    {
        inputManager.OnMouseClick = null;
        inputManager.OnMouseHold = null;
        inputManager.OnMouseUp = null;
    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitApplication()
    {
        Debug.Log("Quitting application");
        Application.Quit();
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector3(inputManager.CameraMovementVector.x,0,
        inputManager.CameraMovementVector.y));
    }

    public void SaveGame()
    {
        SaveDataSerialization saveData = new SaveDataSerialization();
        foreach (var structureData in structureManager.GetAllStructures())
        {
            saveData.AddStructureData(structureData.Key, structureData.Value.BuildingPrefabIndex, structureData.Value.BuildingType);
        }
        var jsonFormat = JsonUtility.ToJson(saveData);
        Debug.Log(jsonFormat);
        saveSystem.SaveData(jsonFormat);
    }

    public void LoadGame()
    {
        var jsonFormatData = saveSystem.LoadData();
        if (String.IsNullOrEmpty(jsonFormatData))
            return;
        SaveDataSerialization saveData = JsonUtility.FromJson<SaveDataSerialization>(jsonFormatData);
        foreach (var structureData in saveData.structuresData)
        {
            Vector3Int position = Vector3Int.RoundToInt(structureData.position.GetValue());
            if(structureData.buildingType == CellType.Road)
            {
                roadManager.PlaceRoad(position);
                roadManager.FinishPlacingRoad();
            }
            else
            {
                structureManager.PlaceLoadedStructure(position, structureData.buildingPrefabindex, structureData.buildingType);
            }
        }
    }
}

