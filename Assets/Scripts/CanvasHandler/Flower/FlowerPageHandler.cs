﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPageHandler : MonoBehaviour
{
    public GameObject WateringAnim;
    public static DataHandler.GardenLog CurrentLog;
    public static FlowerPageHandler Instance;
    public FlowerPageSpotHandler SpotHandler;
    public GameObject WaterPrefab;
    public GameObject PeePrefab;
    public GameObject PooPrefab;
    public Transform WaterSlot;
    public Transform EffectSpawnZone;
    public GameObject Ring;
    public List<GameObject> WaterIcons;
    public RectTransform TargetZone;

    public string DateString;
    public static DataHandler.GardenLog TargetGardenLog;

    public void Awake() {
        Instance = this;
    }

    public void OnEnable() {
        EffectSpawnZone.gameObject.SetActive(false);
        if (TimeHandler.LogCanvasTime == null)
            TimeHandler.GetCurrentTime();
        DateString = TimeHandler.LogCanvasTime.ToDateString();
        
        StartCoroutine(FetchUser());
    }

    public void OnDisable() {
        for(int i = 0; i < WaterIcons.Count; i++) {
            GameObject temp = WaterIcons[i];
            WaterIcons[i] = null;
            Destroy(temp);
        }
        WaterIcons.Clear();
        EffectSpawnZone.gameObject.SetActive(false);
    }

    public IEnumerator FetchUser() {
        while (!DataHandler.User_isDataLoaded)
            yield return 0;
        DataHandler.User_isDataLoaded = false;
        StartCoroutine(DataHandler.ReadGardenLogs(DataHandler.User_id));

        int index = 0;
        TimeHandler.DateTimeStamp indexTime =
            new TimeHandler.DateTimeStamp(DataHandler.User_creation_date);
        for(int i = 0; i < 56; i ++) {
            if(TimeHandler.DateTimeStamp.CmpDateTimeStamp(
                indexTime, new TimeHandler.DateTimeStamp(DateString)) >= 0) {
                break;
            }
            indexTime = indexTime + 1;
            index++;
        }
        SpotHandler.Step = ( index / 7 );
        StartCoroutine(FetchData());
    }

    IEnumerator writeGardenLogId(DataHandler.GardenLog log) {
        while (!DataHandler.User_isGardenDataCreated)
            yield return 0;
        DataHandler.User_isGardenDataCreated = false;
        log.log_id = DataHandler.User_isGardenDataCreatedId;
        Debug.Log("Log_id : " + DataHandler.User_isGardenDataCreatedId);
    }

    public IEnumerator FetchData() {
        while(!DataHandler.User_isGardenDataLoaded)
            yield return 0;
        DataHandler.User_isGardenDataLoaded = false;

        TargetGardenLog = null;
        for(int i = 0; i < DataHandler.Garden_logs.GardenLogs.Length; i ++) {
            if(TimeHandler.DateTimeStamp.CmpDateTimeStamp( DataHandler.Garden_logs.GardenLogs[i].timestamp, DateString ) == 0 ){
                TargetGardenLog = DataHandler.Garden_logs.GardenLogs[i];
                break;
            }
        }
        if(TargetGardenLog == null) {
            DataHandler.GardenLog newGarden = new DataHandler.GardenLog();
            newGarden.id = DataHandler.User_id;
            newGarden.timestamp = TimeHandler.LogCanvasTime.ToString();
            newGarden.flower = 0;
            newGarden.log_water = 0; newGarden.log_poop = 0; newGarden.log_pee = 0;
            newGarden.item_0 = 0; newGarden.item_1 = 0; newGarden.item_2 = 0; newGarden.item_3 = 0; newGarden.item_4 = 0;
            StartCoroutine(DataHandler.CreateGardenlogs(newGarden));
            StartCoroutine(writeGardenLogId(newGarden));
            TargetGardenLog = newGarden;
        }

        SpotHandler.InitSpot(TargetGardenLog);

        int waterIconCount = (TargetGardenLog.log_water + TargetGardenLog.flower >= 10 ) ? 0 : TargetGardenLog.log_water;
        int peeIconCount = (TargetGardenLog.item_0 == 0 && TargetGardenLog.log_pee > 0) ? 1 : 0;
        int pooIconCount = ( TargetGardenLog.item_1 == 0 && TargetGardenLog.log_poop > 0 ) ? 1 : 0;
        Debug.Log("TargetGardenLog.item_0 : " + TargetGardenLog.item_0);
        Debug.Log("TargetGardenLog.log_pee : " + TargetGardenLog.log_pee);
        Debug.Log("peeIconCount : " + peeIconCount);
        Debug.Log("TargetGardenLog.item_1 : " + TargetGardenLog.item_1);
        Debug.Log("TargetGardenLog.log_poop : " + TargetGardenLog.log_poop);
        Debug.Log("pooIconCount : " + pooIconCount);
        int totalIconCount = waterIconCount + peeIconCount + pooIconCount;
        WaterSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(( totalIconCount * 90f ) + ( totalIconCount - 1 ) * 40, 131f);
        int k = 0;
        for(k = 0; k < waterIconCount; k++) {
            GameObject go = Instantiate(WaterPrefab, WaterSlot);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2((k * 130),0);
            go.GetComponent<DraggableWaterIcon>().SetinitPos();
            WaterIcons.Add(go);
        }
        if(peeIconCount > 0) {
            GameObject go = Instantiate(PeePrefab, WaterSlot);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(( k * 130 ), 0);
            go.GetComponent<DraggablePeeIcon>().SetinitPos();
            WaterIcons.Add(go);
            k++;
        }
        if(pooIconCount > 0) {
            GameObject go = Instantiate(PooPrefab, WaterSlot);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(( k * 130 ), 0);
            go.GetComponent<DraggablePooIcon>().SetinitPos();
            WaterIcons.Add(go);
        }
    }

    public void BackButtonClicked() {
        SoundHandler.Instance.Play_SFX(SoundHandler.SFX.BACK);
        TotalManager.instance.TargetDateString = DateString;
        FooterBarHandler.Instance.FooterButtonClick(1);
    }

    public void Watering() {
        EffectSpawnZone.gameObject.SetActive(true);
        StartCoroutine(EffectSpwanZoneOff());
        Instantiate(Ring, EffectSpawnZone);
        SoundHandler.Instance.Play_SFX(SoundHandler.SFX.TADA3);
        TargetGardenLog.flower = ( TargetGardenLog.flower >= 10 ) ? 10 : TargetGardenLog.flower + 1;
        TargetGardenLog.log_water = ( TargetGardenLog.log_water <= 0 ) ? 0 : TargetGardenLog.log_water - 1;
        SpotHandler.Watering();
        StartCoroutine(ReDrawSpot());
        WateringAnim.SetActive(true);
    }

    public void DragPee() {
        EffectSpawnZone.gameObject.SetActive(true);
        StartCoroutine(EffectSpwanZoneOff());
        Instantiate(Ring, EffectSpawnZone);
        SoundHandler.Instance.Play_SFX(SoundHandler.SFX.TADA5);
        TargetGardenLog.item_0 = 1;
        SpotHandler.DragPee();
        StartCoroutine(ReDrawSpot());
    }

    public void DragPoo() {
        EffectSpawnZone.gameObject.SetActive(true);
        StartCoroutine(EffectSpwanZoneOff());
        Instantiate(Ring, EffectSpawnZone);
        SoundHandler.Instance.Play_SFX(SoundHandler.SFX.TADA5);
        TargetGardenLog.item_1 = 1;
        SpotHandler.DragPoo();
        StartCoroutine(ReDrawSpot());
    }

    public void SpawnEffect() {
        EffectSpawnZone.gameObject.SetActive(true);
        StartCoroutine(EffectSpwanZoneOff());
        Instantiate(Ring, EffectSpawnZone);
    }

    public IEnumerator EffectSpwanZoneOff() {
        yield return new WaitForSeconds(1f);
        EffectSpawnZone.gameObject.SetActive(false);
    }

    IEnumerator ReDrawSpot() {
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(DataHandler.UpdateGardenLogs(TargetGardenLog));
        for (int i = 0; i < WaterIcons.Count; i++) {
            GameObject temp = WaterIcons[i];
            WaterIcons[i] = null;
            Destroy(temp);
        }
        WaterIcons.Clear();
        int waterIconCount = ( TargetGardenLog.log_water + TargetGardenLog.flower >= 10 ) ? 0 : TargetGardenLog.log_water;
        int peeIconCount = ( TargetGardenLog.item_0 == 0 && TargetGardenLog.log_pee > 0 ) ? 1 : 0;
        int pooIconCount = ( TargetGardenLog.item_1 == 0 && TargetGardenLog.log_poop > 0 ) ? 1 : 0;
        int totalIconCount = waterIconCount + peeIconCount + pooIconCount;
        Debug.Log("TargetGardenLog.item_0 : " + TargetGardenLog.item_0);
        Debug.Log("TargetGardenLog.log_pee : " + TargetGardenLog.log_pee);
        Debug.Log("peeIconCount : " + peeIconCount);
        Debug.Log("TargetGardenLog.item_1 : " + TargetGardenLog.item_1);
        Debug.Log("TargetGardenLog.log_poop : " + TargetGardenLog.log_poop);
        Debug.Log("pooIconCount : " + pooIconCount);
        WaterSlot.GetComponent<RectTransform>().sizeDelta = new Vector2(( totalIconCount * 90f ) + ( totalIconCount - 1 ) * 40, 131f);
        int k = 0;
        for (k = 0; k < waterIconCount; k++) {
            GameObject go = Instantiate(WaterPrefab, WaterSlot);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(( k * 130 ), 0);
            go.GetComponent<DraggableWaterIcon>().SetinitPos();
            WaterIcons.Add(go);
        }
        if (peeIconCount > 0) {
            GameObject go = Instantiate(PeePrefab, WaterSlot);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(( k * 130 ), 0);
            go.GetComponent<DraggablePeeIcon>().SetinitPos();
            WaterIcons.Add(go);
            k++;
        }
        if (pooIconCount > 0) {
            GameObject go = Instantiate(PooPrefab, WaterSlot);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(( k * 130 ), 0);
            go.GetComponent<DraggablePooIcon>().SetinitPos();
            WaterIcons.Add(go);
        }
    }
}
