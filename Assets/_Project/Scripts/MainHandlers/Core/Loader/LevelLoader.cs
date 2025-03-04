namespace Utilities
{
    using Constants;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LevelLoader : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.LoadScene(
                PlayerPrefs.GetInt(PlayerPrefsKey.LEVEL, 1));
        }
    }
}