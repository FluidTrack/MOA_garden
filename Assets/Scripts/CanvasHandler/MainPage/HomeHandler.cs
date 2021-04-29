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


    //======================================================================================
    // WEEK 4
    //======================================================================================
    public float[] Week4Pivot_x = {
        -84, -836, -1529, -1618,-984, -209, 0,
        0,-895, -1545, -571, 0, -294, -1238,
        -2222, -2392, -2716, -3197, -3492, -3867, -3867,
        -3867, -3291, -2474, -2598, -3265, -3837, -3867
    };
    public float[] Week4Pivot_y = {
        0,0,0,271, 388,401, 395,
        950, 899, 1415, 1460, 1473, 1894, 1894,
        1894, 1485, 1093, 1500, 1894, 1894, 1436,
        851, 725, 653, 195, 140, 231, 78
    };

    public float[] Week4MongMong_x = {
        -3367, -2222, -1122, -91, -1777, -2897, -3853,
        -3036, -1879, -873, -1615, -3043, -3420, -1482,
        68, 276, 763, 2329,  1868, 2815, 2851,
        2011, 2212, 312, 1244, 1550, 3557, 2952
    };
    public float[] Week4MongMong_y = {
        1116, 1261, 1129, 400, 1167, 1048, 1174,
        407, 470, -404, -385, -385, -1762, -1059,
        -1246, -1146, -170, -486, -1748, -1755, -464,
        -249,- 698, 65, 947, 911, 690, 1630
    };
    public bool[] Week4MongMong_isRight = {
        true , true , true , false, false, false, true ,
        true , false, false, true , true , true , true ,
        false, false, false, false, false, true , false,
        true , true , true , true , true , false, false
    };

    //======================================================================================
    // WEEK 6
    //======================================================================================
    public float[] Week6Pivot_x = {
        -234, -933, -1622, -1446, -780, -56, 0,
        0, -923, -1511, -640, 0, -249, -1305, 
        -2132, -2507, -2866, -3242, -3616, -4170, -4233,
        -3849, -3396, -2587, -2788, -3496, -4296, -4485,
        -5253, -5699, -5190, -5240, -6039, -6325, -6325,
        -6325, -5789, -5179, -5056, -5691, -6226, -6325
    };
    public float[] Week6Pivot_y = {
        451, 261, 353, 969, 839,  934, 1057,
        1501, 1455, 1760, 2010, 2216, 2563, 2665, 
        2542, 2111, 1686, 2409, 2660, 2531, 1914,
        1423, 1386, 1294, 800, 618, 860, 413,
        211, 582, 752, 1281, 1089, 652, 1265,
        1694, 1690, 1867, 2613, 2543, 2471, 2279
    };

    public float[] Week6MongMong_x = {
        -3406, -2714, -1829, -1739, -2791, -2791, -3916, 
        -3553, -2724, -1872, -2088, -3576, -3276, -2164,
        -1419, -1030, -1080, -524, -68, 1133, 794,
        451, 451, -1333, -132, -165, 477, 1043,
        1590, 2710, 1524, 1769, 2565, 3830, 3129,
        3411, 2881, 1334, 1496, 2496, 3417, 3397
    };
    public float[] Week6MongMong_y = {
        882, 1374, 1387, 765, 785, 785, 758,
        139, 216, -196, -229, -422, -848, -971,
        -731, -814, -249, -1084, -1383, -1283, -651,
        -132, -132, 141, 514, 1173, 471, 1329,
        1618, 946, 934, 92, 486, 1157, 108,
        -282, 149, -62, -1327, -763, -1207, -549
    };
    public bool[] Week6MongMong_isRight = {
        true , true , false, false, true , false, true ,
        true , true , false, false, true , true , false,
        true , false, true , true , true , false, false,
        true , false, true , false, true , true , false,
        true , true , true , false, false, false, false,
        true , false, true , true , false, false, true
    };

    //======================================================================================
    // WEEK 8
    //======================================================================================
    public float[] Week8Pivot_x = {
    };
    public float[] Week8Pivot_y = {
    };

    public float[] Week8MongMong_x = {
    };
    public float[] Week8MongMong_y = {
    };
    public bool[] Week8MongMong_isRight = {
    };

    private float[] Week4TempPivot = { -3197, 1500 };
    private float[] Week6TempPivot = { -3872, 1305 };
    private float[] Week8TempPivot = { -2912, 2570 };
    private float[] Week4MongMongPivot = { 0, 0 };
    private float[] Week6MongMongPivot = { 0, 0 };
    private float[] Week8MongMongPivot = { 0, 0 };
    private bool Week4MongMongIsRight = false;
    private bool Week6MongMongIsRight = false;
    private bool Week8MongMongIsRight = false;

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

        int index = 0;
        TimeHandler.DateTimeStamp start  = new TimeHandler.DateTimeStamp(DataHandler.User_creation_date);
        TimeHandler.DateTimeStamp target = TimeHandler.HomeCanvasTime;
        for(int i = 0; i < DataHandler.User_periode * 7; i++) {
            if(TimeHandler.DateTimeStamp.CmpDateTimeStamp(start,target) == 0)
                break;
            start = start + 1;
            index++;
        }
        if (DataHandler.User_periode == 4) {
            Debug.Log(Week4Pivot_x[index] + "," + Week4Pivot_y[index]);
            Week4TempPivot[0] = Week4Pivot_x[index];
            Week4TempPivot[1] = Week4Pivot_y[index];
            Week4MongMongPivot[0] = Week4Pivot_x[index];
            Week4MongMongPivot[1] = Week4Pivot_y[index];
            Week4MongMongIsRight = Week4MongMong_isRight[index];
        } else if (DataHandler.User_periode == 6) {
            Debug.Log(Week4Pivot_x[index] + "," + Week4Pivot_y[index]);
            Week6TempPivot[0] = Week6Pivot_x[index];
            Week6TempPivot[1] = Week6Pivot_y[index];
            Week6MongMongPivot[0] = Week6Pivot_x[index];
            Week6MongMongPivot[1] = Week6Pivot_y[index];
            Week6MongMongIsRight = Week6MongMong_isRight[index];
        } else if (DataHandler.User_periode == 8) {
            Debug.Log(Week4Pivot_x[index] + "," + Week4Pivot_y[index]);
            Week8TempPivot[0] = Week8Pivot_x[index];
            Week8TempPivot[1] = Week8Pivot_y[index];
            Week8MongMongPivot[0] = Week8Pivot_x[index];
            Week8MongMongPivot[1] = Week8Pivot_y[index];
            Week8MongMongIsRight = Week8MongMong_isRight[index];
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
