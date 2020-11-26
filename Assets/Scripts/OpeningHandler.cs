﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;

public class OpeningHandler : MonoBehaviour
{
    public RectTransform ProgressBar;
    public Text ProgressLog;
    public Animator NetworkError;

    void Start() {
        ProgressLog.text = "어플리케이션 초기화 중";
        ProgressBar.sizeDelta = new Vector2(80f,58.3f);
        StartCoroutine(CheckNetwork());
    }

    IEnumerator CheckNetwork() {
        yield return new WaitForSeconds(0.5f);
        ProgressLog.text = "네트워크 연결 확인 중";
        ProgressBar.sizeDelta = new Vector2(500f, 58.3f);
        UnityWebRequest request = new UnityWebRequest();

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + "read_users")) {
            yield return request.SendWebRequest();

            if (request.isNetworkError) {
                yield return new WaitForSeconds(2f);
                NetworkError.SetTrigger("active");
            } else {
                Debug.Log(request.downloadHandler.text);
                yield return new WaitForSeconds(0.9f);
                ProgressLog.text = "이전 데이터 확인 중";
                ProgressBar.sizeDelta = new Vector2(1200f, 58.3f);
                StartCoroutine(CheckUser());
            }
        }
    }

    public void QuitApplication() {
        Debug.Log("Quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator CheckUser() {
        bool flag = true;
        yield return 0;
        try {
            FileStream fs = new FileStream(DataHandler.dataPath + "/userData", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            Debug.Log(sr.ReadLine());
        } catch ( System.Exception e ) {
            Debug.Log(e.ToString());
            flag = false;
            FileStream fs = new FileStream(DataHandler.dataPath + "/userData", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("test");
        }

        if (flag) { 
            yield return new WaitForSeconds(0.5f);
            ProgressLog.text = "초기화 완료";
            ProgressBar.sizeDelta = new Vector2(1800f, 58.3f);
        } else {
            yield return new WaitForSeconds(0.5f);
            ProgressLog.text = "초기화 실패";
            ProgressBar.sizeDelta = new Vector2(1800f, 58.3f);
        }
    }
}
