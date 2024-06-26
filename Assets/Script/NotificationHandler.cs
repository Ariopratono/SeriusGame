using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class NotificationHandler : MonoBehaviour
{
    public static NotificationHandler Instance { get; private set; }

    [SerializeField] private TMP_Text notificationText;
    
    public float delay;
    public float timeRemaining;

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

    private void Start()
    {
        notificationText.text = string.Empty;
        delay = 3f;
        timeRemaining = delay;
    }

    public void SendNotification(string text)
    {
        notificationText.text = text;
        timeRemaining = delay;
        StopCoroutine(WaitToDeleteNotification());
        StartCoroutine(WaitToDeleteNotification());
    }

    private IEnumerator WaitToDeleteNotification()
    {
        yield return new WaitForSeconds(delay);
        notificationText.text = string.Empty;
    }
}
