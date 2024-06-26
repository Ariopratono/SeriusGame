using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Action OnRoadPlacement, OnHousePlacement, OnPabrikPlacement, OnTreesPlacement;
    public Button placeRoadButton, placeHouseButton, placePabrikButton, placeTreesButton;

    public Color outlineColor;
    List<Button> buttonList;

    private void Start()
    {
        buttonList = new List<Button> { placeHouseButton, placeRoadButton, placePabrikButton, placeTreesButton };

        placeRoadButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeRoadButton);
            OnRoadPlacement?.Invoke();
        });
        placeHouseButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeHouseButton);
            OnHousePlacement?.Invoke();
        });
        placePabrikButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placePabrikButton);
            OnPabrikPlacement?.Invoke();
        });
        placeTreesButton.onClick.AddListener(() =>
        {
            ResetButtonColor();
            ModifyOutline(placeTreesButton);
            OnTreesPlacement?.Invoke();
        });
    }

    private void ModifyOutline(Button button)
    {
        var outline = button.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }

    private void ResetButtonColor()
    {
        foreach (Button button in buttonList)
        {
            button.GetComponent<Outline>().enabled = false;
        }
    }
}
