using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ExitToWin : MonoBehaviour
{
    private float waitToLoadTime = 1f;

    // Chỉ chuyển scene khi người chơi vào khu vực này
    private void OnTriggerEnter2D(Collider2D other)
    {

        // Kiểm tra nếu đối tượng va chạm là player (nếu cần, bạn có thể gắn tag cho player)
        if (other.gameObject.GetComponent<PlayerController>())
        {
            
            UIFade.Instance.FadeToBack();
            StartCoroutine(LoadSceneRoutine());
          
        }
    }
    private IEnumerator LoadSceneRoutine()
    {
        
        while (waitToLoadTime >= 0)
        {
            waitToLoadTime -= Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("Win");
        
        UIFade .Instance.FadeToClear();
    }
}
