using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject guidePanel;

   // private float waitToLoadTime = 1f;

    public void StartGame()
    {
        GameObject playerIn = GameObject.FindWithTag("Player");
        if (playerIn != null)
        {
            PlayerHealth setOri = playerIn.GetComponent<PlayerHealth>();
            if (setOri.isDead == false)
            {
                setOri.isDead = false;
                setOri.currHealth = 0;
                setOri.CheckIfPlayerDeath();
                //setOri.DeathLoadSceneRoutine();
            }
            else
            {
                Debug.Log("KKKKKKKK");
            }
        }
        else
        {

            //UIFade.Instance.FadeToBack();
            //StartCoroutine(LoadSceneStartRoutine());
            SceneManager.LoadScene("Scene1");
            //UIFade.Instance.FadeToClear();
            Debug.Log("NULLLLLL");
        }

        
    }

    //private IEnumerator LoadSceneStartRoutine()
    //{
    //    while (waitToLoadTime >= 0)
    //    {
    //        waitToLoadTime -= Time.deltaTime;
    //        yield return null;
    //    }
    //    SceneManager.LoadScene("Scene1");
    //    UIFade.Instance.FadeToClear();

    //}
    public void QuitToStartScene()
    {
        // Load Scene Start
        SceneManager.LoadScene("Start"); // Thay "StartScene" bằng tên Scene của bạn
    }

    public void QuitGame()
    {
        // Thoát ứng dụng
        Debug.Log("Quit Game"); // Chỉ hiển thị trong Editor
        Application.Quit();
    }

    public void ShowGuide()
    {
        guidePanel.SetActive(true);
    }

    // Hàm tắt giao diện hướng dẫn
    public void HideGuide()
    {
        guidePanel.SetActive(false);
    }
}