using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; private set; }

    [SerializeField] private StatsUI statsUI;

    public float tick { get; private set; } = 3f;

    [HideInInspector] public int day = 1;
    [HideInInspector] public int hour = 0;

    [Header("Current stats")]
    public int coin;
    public int population;
    public int maxPopulation;
    public int polution;
    public int maxPolution = 100;

    [Header("Stat changes")]
    public int populationChange = 2;
    public int coinChange;
    public int polutionChange;

    [Header("Structure prices")]
    public int housePrice = 1000;
    public int pabrikPrice = 3000;
    public int treesPrice = 2000;

    [Header("Objectives")]
    public int dayLimit;
    public float polutionPercentageLimit = 100f;
    public int coinGoal;
    public int populationGoal;
    [HideInInspector] public bool objectiveComplete = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        statsUI.objectivesStart.text = DisplayObjectiveText();
        statsUI.objectivesSide.text = DisplayObjectiveText();
        StartCoroutine(TimeTick());
    }

    private IEnumerator TimeTick()
    {
        UpdateDate();

        population = Mathf.Clamp(population + populationChange, 0, maxPopulation);
        polution = Mathf.Clamp(polution + polutionChange, 0, maxPolution);
        coin += coinChange;

        UpdateUI();

        CheckObjectiveCompletion();

        yield return new WaitForSeconds(tick);

        StartCoroutine(TimeTick());
    }

    public void PlacedHouse()
    {
        coin -= housePrice;
        maxPopulation += 10;
        polutionChange += 1;

        UpdateUI();
    }

    public void PlacedPabrik()
    {
        coin -= pabrikPrice;
        coinChange += 200;
        polutionChange += 5;

        UpdateUI();
    }

    public void PlacedTrees()
    {
        coin -= treesPrice;
        coinChange -= 30;
        polutionChange -= 6;

        UpdateUI();
    }

    private void CheckObjectiveCompletion()
    {
        if(CalculatePolutionPercentage() >= polutionPercentageLimit)
        {
            NotificationHandler.Instance.SendNotification("Polution reached the limit");
            objectiveComplete = false;
            StateManager.Instance.ChangeState(StateManager.GameState.end);
        }
        if(coin >= coinGoal && population >= populationGoal)
        {
            UnityEngine.Debug.Log("Objective completed");
            objectiveComplete = true;
            StateManager.Instance.ChangeState(StateManager.GameState.end);
        }
        if(day > dayLimit)
        {
            NotificationHandler.Instance.SendNotification("Reached maximum days");
            objectiveComplete = false;
            StateManager.Instance.ChangeState(StateManager.GameState.end);
        }
    }

    private void UpdateDate()
    {
        hour += 1;

        if(hour >= 24)
        {
            hour = 0;
            day += 1;
        }
    }

    public void UpdateUI()
    {
        statsUI.day.text = "Day " + day;
        statsUI.hour.text = TimeDisplay();
        statsUI.population.text = population.ToString();
        statsUI.coin.text = coin.ToString();
        statsUI.polution.text = CalculatePolutionPercentage().ToString("0") + "%";
    }

    private string TimeDisplay()
    {
        if(hour < 10)
        {
            return "0" + hour + ":00";
        }
        else
        {
            return hour + ":00";
        }
    }

    private float CalculatePolutionPercentage()
    {
        float result = ((float)polution / (float)maxPolution) * 100;
        return Mathf.Min(result, 100);
    }

    private string DisplayObjectiveText()
    {
        string text = "Objective:\n" +
                      "- Reach " + populationGoal + " population\n" +
                      "- Obtain " + coinGoal + " coins\n" +
                      "- Keep pollution level below " + polutionPercentageLimit + "%\n" +
                      "- Time limit: " + dayLimit + " days";
        return text;
    }
}
