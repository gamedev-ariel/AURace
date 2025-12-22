//using UnityEngine;
//using Fusion;
//using System.Collections.Generic;

//public static class PlayerDataBackup
//{
//    // מבנה הנתונים שאנחנו רוצים לשמור
//    public struct SavedData
//    {
//        public bool HasCoffee;
//        public bool IsBoostActive;
//        // אתה יכול להוסיף כאן עוד משתנים (כמו חיים, ניקוד וכו')
//    }

//    // מילון שמצמיד בין ה-ID של השחקן לנתונים שלו
//    public static Dictionary<PlayerRef, SavedData> Data = new Dictionary<PlayerRef, SavedData>();

//    public static void Save(PlayerRef player, bool hasCoffee, bool isBoostActive)
//    {
//        var data = new SavedData
//        {
//            HasCoffee = hasCoffee,
//            IsBoostActive = isBoostActive
//        };

//        if (Data.ContainsKey(player))
//            Data[player] = data;
//        else
//            Data.Add(player, data);
//    }

//    public static bool TryLoad(PlayerRef player, out SavedData data)
//    {
//        return Data.TryGetValue(player, out data);
//    }
//}



using Fusion;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataBackup
{
    public struct SavedData
    {
        public bool HasCoffee;
        public bool IsBoostActive;
        // הוספנו משתנה לשמירת המיקום המחושב
        public Vector3? CustomSpawnPosition;
    }

    public static Dictionary<PlayerRef, SavedData> Data = new Dictionary<PlayerRef, SavedData>();

    // עדכנו את הפונקציה לקבל גם מיקום (אופציונלי)
    public static void Save(PlayerRef player, bool hasCoffee, bool isBoostActive, Vector3? spawnPos = null)
    {
        var data = new SavedData
        {
            HasCoffee = hasCoffee,
            IsBoostActive = isBoostActive,
            CustomSpawnPosition = spawnPos
        };

        if (Data.ContainsKey(player))
            Data[player] = data;
        else
            Data.Add(player, data);
    }

    public static bool TryLoad(PlayerRef player, out SavedData data)
    {
        return Data.TryGetValue(player, out data);
    }
}