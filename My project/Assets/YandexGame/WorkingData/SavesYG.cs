
using System.Collections.Generic;
 


using static GameSetting;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;
 
        public ScoreData scoreData;
        public List<CharacterData> characterData = new List<CharacterData>();
        public GameRecordData gameRecordData;
        public bool tutorialCompleted;
        public bool isFirstTime;
        public bool isMovileInput;
        public AudioData audioData;
    }
}
