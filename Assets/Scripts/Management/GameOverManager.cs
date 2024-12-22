using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : Singleton<GameOverManager>
{
    [SerializeField] private CanvasGroup gameOverCanvas; // CanvasGroup cho UI GameOver
    [SerializeField] private float fadeDuration = 1f;   // Thời gian hiệu ứng fade

    protected override void Awake()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.alpha = 0; // Đặt màn hình trong suốt ban đầu
            gameOverCanvas.gameObject.SetActive(false); // Ẩn Canvas
        }
    }

    public void TriggerGameOver()
    {
        if (gameOverCanvas != null)
        {
            gameOverCanvas.gameObject.SetActive(true); // Hiển thị Canvas
            StartCoroutine(FadeToBlack());
        }
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;

        // Tăng alpha dần để làm tối màn hình
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            gameOverCanvas.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        gameOverCanvas.alpha = 1f; // Đảm bảo màn hình hoàn toàn tối
        Time.timeScale = 0f;       // Dừng thời gian game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian game
        StartCoroutine(FadeToClear());
    }

    private IEnumerator FadeToClear()
    {
        float elapsedTime = 0f;

        // Giảm alpha dần để làm sáng màn hình
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            gameOverCanvas.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        gameOverCanvas.alpha = 0f; // Đảm bảo màn hình hoàn toàn sáng
        gameOverCanvas.gameObject.SetActive(false); // Ẩn Canvas

        // Tải lại cảnh
       // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
