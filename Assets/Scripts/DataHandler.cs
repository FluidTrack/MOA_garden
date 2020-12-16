﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class DataHandler : MonoBehaviour
{
    internal static string ServerAddress = "http://fluidtrack.site/";

    internal static bool User_isDataLoaded = false;
    internal static bool User_isGardenDataLoaded = false;
    internal static bool User_isWaterDataLoaded = false;
    internal static bool User_isPooDataLoaded = false;
    internal static bool User_isPeeDataLoaded = false;

    internal static bool User_isGardenDataCreated = false;
    internal static bool User_isWaterDataCreated = false;
    internal static bool User_isDrinkDataCreated = false;
    internal static bool User_isPooDataCreated = false;
    internal static bool User_isPeeDataCreated = false;

    internal static bool User_isDataUpdated = false;
    internal static bool User_isGardenDataUpdated = false;
    internal static bool User_isWaterDataUpdated = false;
    internal static bool User_isDrinkDataUpdated = false;
    internal static bool User_isPooDataUpdated = false;
    internal static bool User_isPeeDataUpdated = false;

    internal static bool User_isWaterDataDeleted = false;
    internal static bool User_isDrinkDataDeleted = false;
    internal static bool User_isPooDataDeleted = false;
    internal static bool User_isPeeDataDeleted = false;

    internal static string dataPath;
    internal static int User_id;
    internal static string User_name;
    internal static string User_moa_band_name;
    internal static int User_item_0;
    internal static int User_item_1;
    internal static int User_item_2;
    internal static int User_item_3;
    internal static int User_item_4;
    internal static string User_morning_call_time;
    internal static string User_breakfast_time;
    internal static string User_lunch_time;
    internal static string User_dinner_time;
    internal static string User_school_time;
    internal static string User_home_time;
    internal static string User_water_skip;
    internal static string User_drink_skip;
    internal static string User_pee_skip;
    internal static string User_poop_skip;
    internal static string User_font_family;
    internal static int User_font_size;
    internal static string User_creation_date;
    internal static GardenLogsJson Garden_logs;
    internal static WaterLogsJson  Water_logs;
    internal static PoopLogsJson   Poop_logs;
    internal static PeeLogsJson    Pee_logs;

    public void Start() {
        dataPath = Application.persistentDataPath;
    }

    [System.Serializable]
    public class UserLog {
        public int id;
        public string name;
        public string moa_band_name;
        public int item_0;
        public int item_1;
        public int item_2;
        public int item_3;
        public int item_4;
        public string morning_call_time;
        public string breakfast_time;
        public string lunch_time;
        public string dinner_time;
        public string school_time;
        public string home_time;
        public string water_skip;
        public string drink_skip;
        public string pee_skip;
        public string poop_skip;
        public string font_family;
        public int font_size;
        public string creation_date;
    }

    [System.Serializable]
    public class UserLogsJson {
        public UserLog[] UserLogs;
    }

    [System.Serializable]
    public class GardenLog {
        public int log_id;
        public int id;
        public string timestamp;
        public int flower;
        public int item_0;
        public int item_1;
        public int item_2;
        public int item_3;
        public int item_4;
    }

    [System.Serializable]
    public class GardenLogsJson {
        public GardenLog[] GardenLogs;
    }

    [System.Serializable]
    public class WaterLog {
        public int log_id;
        public int id;
        public string timestamp;
        public int type;
    }

    [System.Serializable]
    public class WaterLogsJson {
        public WaterLog[] WaterLogs;
    }

    [System.Serializable]
    public class PoopLog {
        public int log_id;
        public int id;
        public string timestamp;
        public int type;
    }

    [System.Serializable]
    public class PoopLogsJson {
        public PoopLog[] PoopLogs;
    }

    [System.Serializable]
    public class PeeLog {
        public int log_id;
        public int id;
        public string timestamp;
    }

    [System.Serializable]
    public class PeeLogsJson {
        public PeeLog[] PeeLogs;
    }

    static public T JsonParsing <T>(string jsondata) {
        try {
            return JsonUtility.FromJson<T>(jsondata);
        } catch (System.Exception e) {
            Debug.LogWarning("Parsing Error\n" +  e.ToString());
            throw new System.Exception();
        }
    }

    //=====================================================================================================================================
    //
    //  USER TABLE CRUD (Create, Read, Update, Delete)
    //  @ 2020.12.13 KimYC1223
    /*
        +-------------------+----------+------+-----+---------+----------------+
        | Field             | Type     | Null | Key | Default | Extra          |
        +-------------------+----------+------+-----+---------+----------------+
        | id                | int(11)  | NO   | PRI | NULL    | auto_increment |
        | name              | char(10) | YES  |     | NULL    |                |
        | moa_band_name     | char(30) | YES  |     | NULL    |                |
        | item_0            | int(11)  | YES  |     | NULL    |                |
        | item_1            | int(11)  | YES  |     | NULL    |                |
        | item_2            | int(11)  | YES  |     | NULL    |                |
        | item_3            | int(11)  | YES  |     | NULL    |                |
        | item_4            | int(11)  | YES  |     | NULL    |                |
        | morning_call_time | time     | YES  |     | NULL    |                |
        | breakfast_time    | time     | YES  |     | NULL    |                |
        | lunch_time        | time     | YES  |     | NULL    |                |
        | dinner_time       | time     | YES  |     | NULL    |                |
        | school_time       | time     | YES  |     | NULL    |                |
        | home_time         | time     | YES  |     | NULL    |                |
        | water_skip        | time     | YES  |     | NULL    |                |
        | drink_skip        | time     | YES  |     | NULL    |                |
        | pee_skip          | time     | YES  |     | NULL    |                |
        | poop_skip         | time     | YES  |     | NULL    |                |
        | font_family       | char(20) | YES  |     | NULL    |                |
        | font_size         | int(11)  | YES  |     | NULL    |                |
        | creation_date     | datetime | YES  |     | NULL    |                |
        +-------------------+----------+------+-----+---------+----------------+
    */
    //
    //=====================================================================================================================================
    //  ▶ create_users
    //=====================================================================================================================================
    static public IEnumerator CreateUsers () {
        yield return 0;
        UnityWebRequest request = new UnityWebRequest();
        string url = "create_users";
        url += "?name=" + User_name;
        url += "&moa_band_name=" + User_moa_band_name;
        url += "&item_0=0&item_1=0&item_2=0&item_3=0&item_4=0";
        url += "&morning_call_time=" + User_morning_call_time + ":00";
        url += "&breakfast_time=" + User_breakfast_time + ":00";
        url += "&lunch_time=" + User_lunch_time + ":00";
        url += "&dinner_time=" + User_dinner_time + ":00";
        url += "&school_time=" + User_school_time + ":00";
        url += "&home_time=" + User_home_time + ":00";
        url += "&water_skip=" + User_water_skip + ":00";
        url += "&drink_skip=" + User_drink_skip + ":00";
        url += "&pee_skip=" + User_pee_skip + ":00";
        url += "&poop_skip=" + User_poop_skip + ":00";
        url += "&font_family=" + User_font_family;
        url += "&font_size=" + User_font_size;
        url += "&creation_date=" + User_creation_date;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            Debug.Log(DataHandler.ServerAddress + url);
            if (request.isNetworkError)
                QuitApplication();
            else {
                Debug.Log(request.downloadHandler.text);
                try {
                    User_id = int.Parse(request.downloadHandler.text);
                    FileStream fs =
                        new FileStream(dataPath + "/userData", FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(request.downloadHandler.text);
                    User_isDataLoaded = true;
                    sw.Close();
                    fs.Close();
                } catch (System.Exception e ) {
                    Debug.LogError(e.ToString());
                }
            }
            
        }
    }

    //=====================================================================================================================================
    //  ▶ read_users
    //=====================================================================================================================================
    static public IEnumerator ReadUsers(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "read_users";
        url += "?id=" + target_id;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                string jsonString = request.downloadHandler.text;
                UserLogsJson data = JsonParsing<UserLogsJson>(jsonString);
                Debug.Log(data.UserLogs[0].name);
                User_name = data.UserLogs[0].name;
                User_moa_band_name = data.UserLogs[0].moa_band_name;
                User_item_0 = data.UserLogs[0].item_0;
                User_item_1 = data.UserLogs[0].item_1;
                User_item_2 = data.UserLogs[0].item_2;
                User_item_3 = data.UserLogs[0].item_3;
                User_item_4 = data.UserLogs[0].item_4;
                User_morning_call_time = data.UserLogs[0].morning_call_time;
                User_breakfast_time = data.UserLogs[0].breakfast_time;
                User_lunch_time = data.UserLogs[0].lunch_time;
                User_dinner_time = data.UserLogs[0].dinner_time;
                User_school_time = data.UserLogs[0].school_time;
                User_home_time = data.UserLogs[0].home_time;
                User_water_skip = data.UserLogs[0].water_skip;
                User_drink_skip = data.UserLogs[0].drink_skip;
                User_pee_skip = data.UserLogs[0].pee_skip;
                User_poop_skip = data.UserLogs[0].poop_skip;
                User_font_family = data.UserLogs[0].font_family;
                User_font_size = data.UserLogs[0].font_size;
                User_creation_date = data.UserLogs[0].creation_date;
                TimeHandler.CreationTime = new TimeHandler.DateTimeStamp(User_creation_date);
                User_isDataLoaded = true;
            }
        }
    }

    //=====================================================================================================================================
    //  ▶ update_users
    //=====================================================================================================================================
    static public IEnumerator UpdateUsers(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "update_users";
        url += "?name=" + User_name;
        url += "&moa_band_name=" + User_moa_band_name;
        url += "&item_0=0&item_1=0&item_2=0&item_3=0&item_4=0";
        url += "&morning_call_time=" + User_morning_call_time + ":00";
        url += "&breakfast_time=" + User_breakfast_time + ":00";
        url += "&lunch_time=" + User_lunch_time + ":00";
        url += "&dinner_time=" + User_dinner_time + ":00";
        url += "&school_time=" + User_school_time + ":00";
        url += "&home_time=" + User_home_time + ":00";
        url += "&water_skip=" + User_water_skip + ":00";
        url += "&drink_skip=" + User_drink_skip + ":00";
        url += "&pee_skip=" + User_pee_skip + ":00";
        url += "&poop_skip=" + User_poop_skip + ":00";
        url += "&font_family=" + User_font_family;
        url += "&font_size=" + User_font_size;
        url += "&creation_date=" + User_creation_date;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                Debug.Log(request.downloadHandler.text);
                User_isDataUpdated = true;
            }

        }
    }

    //=====================================================================================================================================
    //
    //  GARDEN LOGS TABLE CRUD (Create, Read, Update, Delete)
    //  @ 2020.12.13 KimYC1223
    //
    /*
        +-----------+----------+------+-----+---------+----------------+
        | Field     | Type     | Null | Key | Default | Extra          |
        +-----------+----------+------+-----+---------+----------------+
        | log_id    | int(11)  | NO   | PRI | NULL    | auto_increment |
        | id        | int(11)  | YES  |     | NULL    |                |
        | timestamp | datetime | YES  |     | NULL    |                |
        | flower    | int(11)  | YES  |     | NULL    |                |
        | item_0    | int(11)  | YES  |     | NULL    |                |
        | item_1    | int(11)  | YES  |     | NULL    |                |
        | item_2    | int(11)  | YES  |     | NULL    |                |
        | item_3    | int(11)  | YES  |     | NULL    |                |
        | item_4    | int(11)  | YES  |     | NULL    |                |
        +-----------+----------+------+-----+---------+----------------+
    */
    //
    //=====================================================================================================================================
    //  ▶ create_garden_logs
    //=====================================================================================================================================
    static public IEnumerator CreateGardenlogs(GardenLog log) {
        yield return 0;
        UnityWebRequest request = new UnityWebRequest();
        string url = "create_garden_logs";
        url += "?id=" + log.id;
        url += "&timestamp=" + log.timestamp;
        url += "&flower=" + log.flower;
        url += "&item_0=" + log.item_0;
        url += "&item_1=" + log.item_1;
        url += "&item_2=" + log.item_2;
        url += "&item_3=" + log.item_3;
        url += "&item_4=" + log.item_4;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isGardenDataCreated = true;
            }

        }
    }

    //=====================================================================================================================================
    //  ▶ read_garden_logs
    //=====================================================================================================================================
    static public IEnumerator ReadGardenLogs(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "read_garden_logs";
        url += "?id=" + target_id;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                try {
                    string jsonString = request.downloadHandler.text;
                    Garden_logs = JsonParsing<GardenLogsJson>(jsonString);
                    User_isGardenDataLoaded = true;
                } catch (System.Exception e) {
                    Debug.LogWarning(e.ToString());
                    Garden_logs = new GardenLogsJson();
                    Garden_logs.GardenLogs = null;
                    User_isGardenDataLoaded = true;
                }
            }
        }
    }

    //=====================================================================================================================================
    //  ▶ update_garden_logs
    //=====================================================================================================================================
    static public IEnumerator UpdateGardenLogs(GardenLog log) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "update_garden_logs";
        url += "?log_id=" + log.log_id;
        url += "&id=" + log.id;
        url += "&timestamp=" + log.timestamp;
        url += "&flower=" + log.flower;
        url += "&item_0=" + log.item_0;
        url += "&item_1=" + log.item_1;
        url += "&item_2=" + log.item_2;
        url += "&item_3=" + log.item_3;
        url += "&item_4=" + log.item_4;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isGardenDataUpdated = true;
            }
        }
    }

    //=====================================================================================================================================
    //
    //  WATER LOGS TABLE CRUD (Create, Read, Update, Delete)
    //  @ 2020.12.13 KimYC1223
    //
    /*
        +-----------+----------+------+-----+---------+----------------+
        | Field     | Type     | Null | Key | Default | Extra          |
        +-----------+----------+------+-----+---------+----------------+
        | log_id    | int(11)  | NO   | PRI | NULL    | auto_increment |
        | id        | int(11)  | YES  |     | NULL    |                |
        | timestamp | datetime | YES  |     | NULL    |                |
        | type      | int(11)  | YES  |     | NULL    |                |
        +-----------+----------+------+-----+---------+----------------+
    */
    //
    //=====================================================================================================================================
    //  ▶ create_water_logs
    //=====================================================================================================================================
    static public IEnumerator CreateWaterlogs(WaterLog log) {
        yield return 0;
        UnityWebRequest request = new UnityWebRequest();
        string url = "create_water_logs";
        url += "?id=" + log.id;
        url += "&timestamp=" + log.timestamp;
        url += "&type=" + log.type;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isWaterDataCreated = true;
            }

        }
    }

    //=====================================================================================================================================
    //  ▶ read_water_logs
    //=====================================================================================================================================
    static public IEnumerator ReadWaterLogs(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "read_water_logs";
        url += "?id=" + target_id;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                try {
                    string jsonString = request.downloadHandler.text;
                    Water_logs = JsonParsing<WaterLogsJson>(jsonString);
                    User_isWaterDataLoaded = true;
                } catch (System.Exception e) {
                    Debug.LogError(e.ToString());
                    Water_logs = new WaterLogsJson();
                    Water_logs.WaterLogs = new WaterLog[0];
                    User_isWaterDataLoaded = true;
                }
            }
        }
    }

    //=====================================================================================================================================
    //  ▶ update_water_logs
    //=====================================================================================================================================
    static public IEnumerator UpdateGardenLogs(WaterLog log) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "update_water_logs";
        url += "?log_id=" + log.log_id;
        url += "&id=" + log.id;
        url += "&timestamp=" + log.timestamp;
        url += "&type=" + log.type;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isWaterDataUpdated = true;
            }
        }
    }

    //=====================================================================================================================================
    //  ▶ delete_water_logs
    //=====================================================================================================================================
    static public IEnumerator DeleteWaterLogs(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "delete_water_logs";
        url += "?log_id=" + target_id;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isWaterDataDeleted = true;
            }
        }
    }

    //=====================================================================================================================================
    //
    //  POOP LOGS TABLE CRUD (Create, Read, Update, Delete)
    //  @ 2020.12.13 KimYC1223
    //
    /*
        +-----------+----------+------+-----+---------+----------------+
        | Field     | Type     | Null | Key | Default | Extra          |
        +-----------+----------+------+-----+---------+----------------+
        | log_id    | int(11)  | NO   | PRI | NULL    | auto_increment |
        | id        | int(11)  | YES  |     | NULL    |                |
        | timestamp | datetime | YES  |     | NULL    |                |
        | type      | int(11)  | YES  |     | NULL    |                |
        +-----------+----------+------+-----+---------+----------------+
    */
    //
    //=====================================================================================================================================
    //  ▶ create_poop_logs
    //=====================================================================================================================================
    static public IEnumerator CreatePooplogs(PoopLog log) {
        yield return 0;
        UnityWebRequest request = new UnityWebRequest();
        string url = "create_poop_logs";
        url += "?id=" + log.id;
        url += "&timestamp=" + log.timestamp;
        url += "&type=" + log.type;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isPooDataCreated = true;
            }

        }
    }

    //=====================================================================================================================================
    //  ▶ read_poop_logs
    //=====================================================================================================================================
    static public IEnumerator ReadPoopLogs(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "read_poop_logs";
        url += "?id=" + target_id;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                try {
                    string jsonString = request.downloadHandler.text;
                    Poop_logs = JsonParsing<PoopLogsJson>(jsonString);
                    User_isPooDataLoaded = true;
                } catch (System.Exception e) {
                    Debug.LogError(e.ToString());
                    Poop_logs = new PoopLogsJson();
                    Poop_logs.PoopLogs = new PoopLog[0];
                    User_isPooDataLoaded = true;
                }
            }
        }
    }

    //=====================================================================================================================================
    //  ▶ update_poop_logs
    //=====================================================================================================================================
    static public IEnumerator UpdatePoopLogs(PoopLog log) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "update_poop_logs";
        url += "?log_id=" + log.log_id;
        url += "&id=" + log.id;
        url += "&timestamp=" + log.timestamp;
        url += "&type=" + log.type;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isPooDataUpdated = true;
            }
        }
    }

    //=====================================================================================================================================
    //  ▶ delete_poop_logs
    //=====================================================================================================================================
    static public IEnumerator DeletePoopLogs(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "delete_poop_logs";
        url += "?log_id=" + target_id;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isPooDataDeleted = true;
            }
        }
    }

    //=====================================================================================================================================
    //
    //  PEE LOGS TABLE CRUD (Create, Read, Update, Delete)
    //  @ 2020.12.13 KimYC1223
    //
    /*
        +-----------+----------+------+-----+---------+----------------+
        | Field     | Type     | Null | Key | Default | Extra          |
        +-----------+----------+------+-----+---------+----------------+
        | log_id    | int(11)  | NO   | PRI | NULL    | auto_increment |
        | id        | int(11)  | YES  |     | NULL    |                |
        | timestamp | datetime | YES  |     | NULL    |                |
        +-----------+----------+------+-----+---------+----------------+
    */
    //
    //=====================================================================================================================================
    //  ▶ create_pee_logs
    //=====================================================================================================================================
    static public IEnumerator CreatePeelogs(PeeLog log) {
        yield return 0;
        UnityWebRequest request = new UnityWebRequest();
        string url = "create_pee_logs";
        url += "?id=" + log.id;
        url += "&timestamp=" + log.timestamp;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isPeeDataCreated = true;
            }

        }
    }

    //=====================================================================================================================================
    //  ▶ read_pee_logs
    //=====================================================================================================================================
    static public IEnumerator ReadPeeLogs(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "read_pee_logs";
        url += "?id=" + target_id;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                try {
                    string jsonString = request.downloadHandler.text;
                    Pee_logs = JsonParsing<PeeLogsJson>(jsonString);
                    User_isPeeDataLoaded = true;
                } catch (System.Exception e) {
                    Debug.LogError(e.ToString());
                    Pee_logs = new PeeLogsJson();
                    Pee_logs.PeeLogs = new PeeLog[0];
                    User_isPeeDataLoaded = true;
                }
            }
        }
    }

    //=====================================================================================================================================
    //  ▶ update_pee_logs
    //=====================================================================================================================================
    static public IEnumerator UpdatePeeLogs(PeeLog log) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "update_pee_logs";
        url += "?log_id=" + log.log_id;
        url += "&id=" + log.id;
        url += "&timestamp=" + log.timestamp;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isPeeDataUpdated = true;
            }
        }
    }

    //=====================================================================================================================================
    //  ▶ delete_pee_logs
    //=====================================================================================================================================
    static public IEnumerator DeletePeeLogs(int target_id) {
        yield return 0;

        UnityWebRequest request = new UnityWebRequest();
        string url = "delete_pee_logs";
        url += "?log_id=" + target_id;

        using (request = UnityWebRequest.Get(DataHandler.ServerAddress + url)) {
            yield return request.SendWebRequest();
            if (request.isNetworkError)
                QuitApplication();
            else {
                User_isPeeDataDeleted = true;
            }
        }
    }
        

    static public void QuitApplication() {
        Debug.Log("Quit");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
             Application.Quit();
        #endif
    }
}
