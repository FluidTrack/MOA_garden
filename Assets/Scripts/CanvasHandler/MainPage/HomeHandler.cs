﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeHandler : MonoBehaviour
{
    public static HomeHandler Instance;
    public MainPageHeaderHandler HeaderHandler;
    public Transform GardenSpotParents;
    public GameObject ReturnButton;
    public GameObject[] Week4Objects;
    public GameObject[] Week6Objects;
    public GameObject[] Week8Objects;
    public ZoomedViewHandler Week4ComponentHandler;
    public ZoomedViewHandler Week6ComponentHandler;
    public ZoomedViewHandler Week8ComponentHandler;
    internal int DateCount = 0;
    internal int totalWeek;
    public enum PAGE { WEEK_4, WEEK_6, WEEK_8 };
    public PAGE currentPage;
    public GardenSpotHandler[] Spots;

    private float[] Week4Pivot = {
        -613, 30,   -1994,0,    -3288, 8,   -3430, 868,    -2310, 852,    -961,828,     0,1009,
        -537, 1940, -2147,1743, -3275,2603, -1594,2769,    -95,2903,    -1160, 3700,  -2651,3716
    };

    private float[] Week4TempPivot = { -540, 1452 };
    private float[] Week6TempPivot = { -597, 1992 };
    private float[] Week8TempPivot = { -669, 2300 };

    public void Awake() {
        Instance = this;
    }

    public void Start() {
        Spots = GardenSpotParents.GetComponentsInChildren<GardenSpotHandler>();
    }

    public void OnEnable() {
        TimeHandler.GetCurrentTime();
        StartCoroutine(CheckUserDataLoad());
        HeaderHandler.WriteTodayDate(TimeHandler.HomeCanvasTime);
        ReturnButton.SetActive(false);
    }

    public IEnumerator CheckUserDataLoad() {
        while (!DataHandler.User_isDataLoaded) {
            yield return 0;
        }
        //==================================================================================================
        //  TODAY POSITION
        //==================================================================================================
        totalWeek = DataHandler.User_periode;
        if (DataHandler.User_periode == 4) {
            Week4Objects[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            Week4Objects[0].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Week4TempPivot[0], Week4TempPivot[1]);
            Week4Objects[0].GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        if (DataHandler.User_periode == 6) {
            Week6Objects[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            Week6Objects[0].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Week6TempPivot[0], Week6TempPivot[1]);
            Week6Objects[0].GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        if (DataHandler.User_periode == 8) {
            Week8Objects[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            Week8Objects[0].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Week8TempPivot[0], Week8TempPivot[1]);
            Week8Objects[0].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }


        if (DataHandler.User_periode == 4) { 
            currentPage = PAGE.WEEK_4;
            Week4Objects[0].SetActive(true);
            Week6Objects[0].SetActive(false);
            Week8Objects[0].SetActive(false);
            TouchAndMouseManager.Instance.ChangePeriode(Week4Objects[0].GetComponent<RectTransform>());
            Spots = Week4Objects[1].GetComponentsInChildren<GardenSpotHandler>();
        }
        else if (DataHandler.User_periode == 6) { 
            currentPage = PAGE.WEEK_6;
            Week4Objects[0].SetActive(false);
            Week6Objects[0].SetActive(true);
            Week8Objects[0].SetActive(false);
            TouchAndMouseManager.Instance.ChangePeriode(Week6Objects[0].GetComponent<RectTransform>());
            Spots = Week6Objects[1].GetComponentsInChildren<GardenSpotHandler>();
        }
        else if (DataHandler.User_periode == 8) { 
            currentPage = PAGE.WEEK_8;
            Week4Objects[0].SetActive(false);
            Week6Objects[0].SetActive(false);
            Week8Objects[0].SetActive(true);
            TouchAndMouseManager.Instance.ChangePeriode(Week8Objects[0].GetComponent<RectTransform>());
            Spots = Week8Objects[1].GetComponentsInChildren<GardenSpotHandler>();
        }

        StartCoroutine(DataHandler.ReadGardenLogs(DataHandler.User_id));
        StartCoroutine(CheckGardenDataLoad());

    }

    public IEnumerator CheckGardenDataLoad() {
        yield return 0;
        while (!DataHandler.User_isGardenDataLoaded) {
            yield return 0;
        }
        DataHandler.User_isGardenDataLoaded = false;
        InitGardenSpot();
    }

    public void InitGardenSpot() {
        int passedDay = 0;
        if (!DataHandler.User_isDataLoaded) {
            Debug.LogWarning("Please User data Load first!");
            return;
        }
        TimeHandler.DateTimeStamp inputDate =
            new TimeHandler.DateTimeStamp(DataHandler.User_creation_date);
        DataHandler.GardenLog[] logs = DataHandler.Garden_logs.GardenLogs;
        int index = 0;

        if (GardenSpotHandler.weeklyData == null) {
            GardenSpotHandler.weeklyData = new List<int>();
            for (int i = 0; i < 8; i++) {
                GardenSpotHandler.weeklyData.Add(0);
            }
        }

        for (int i = 0; i < 8; i++)
            GardenSpotHandler.weeklyData[i] = 0;

        foreach (GardenSpotHandler spot in Spots) {
            DataHandler.GardenLog inputData = null;
            while(true) {
                if (index < logs.Length) {
                    TimeHandler.DateTimeStamp targetDate =
                        new TimeHandler.DateTimeStamp(logs[index].timestamp);
                    int cmpResult = TimeHandler.DateTimeStamp.CmpDateTimeStamp(
                        inputDate.ToDateString(), targetDate.ToDateString());
                    if (cmpResult == 0) {
                        inputData = logs[index];
                        index++;
                        break;
                    } else if ( cmpResult == -1 ) { break; }
                      else if ( cmpResult ==  1 ) { index++; }
                }else { break; }
            }
            spot.InitSpot(inputData,inputDate);
            if(TimeHandler.DateTimeStamp.CmpDateTimeStamp(
                TimeHandler.HomeCanvasTime,inputDate    
            ) >= 0 ) passedDay++;
            inputDate = inputDate+1;
        }
        DateCount = passedDay;
        if(DataHandler.User_periode == 4) {
            Week4Objects[2].GetComponent<BigCloudController>().ChangeCloudState(DateCount);
            Week4Objects[3].GetComponent<SmallCloudController>().ChangeCloudState(DateCount);
            Week4ComponentHandler.Day = DateCount;
        }
        if (DataHandler.User_periode == 6) {
            Week6Objects[2].GetComponent<BigCloudController>().ChangeCloudState(DateCount);
            Week6Objects[3].GetComponent<SmallCloudController>().ChangeCloudState(DateCount);
            Week6ComponentHandler.Day = DateCount;
        }
        if (DataHandler.User_periode == 8) {
            Week8Objects[2].GetComponent<BigCloudController>().ChangeCloudState(DateCount);
            Week8Objects[3].GetComponent<SmallCloudController>().ChangeCloudState(DateCount);
            Week8ComponentHandler.Day = DateCount;
        }
    }

    public void ReturnButtonClick(bool isNoSound) {
        if(!isNoSound)
            SoundHandler.Instance.Play_SFX(SoundHandler.SFX.CLICKED);

        if (DataHandler.User_periode == 4) {
            Week4Objects[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            Week4Objects[0].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Week4TempPivot[0], Week4TempPivot[1]);
            Week4Objects[0].GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Week4ComponentHandler.ResetButton();
        }
        if (DataHandler.User_periode == 6) {
            Week6Objects[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            Week6Objects[0].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Week6TempPivot[0], Week6TempPivot[1]);
            Week6Objects[0].GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
            Week6ComponentHandler.ResetButton();
        }
        if (DataHandler.User_periode == 8) {
            Week8Objects[0].GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            Week8Objects[0].GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Week8TempPivot[0], Week8TempPivot[1]);
            Week8Objects[0].GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
            Week8ComponentHandler.ResetButton();
        }

        ReturnButton.SetActive(false);
        return;
    }

    public void ReturnButtonClick() {
        ReturnButtonClick(false);
    }
}
