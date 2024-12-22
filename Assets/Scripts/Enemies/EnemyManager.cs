using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    private int enemyCount;

    void Start()
    {
        // Cập nhật số lượng enemy ban đầu
        UpdateEnemyCount();
    }

    void UpdateEnemyCount()
    {
        // Tìm tất cả các enemy bằng Tag "Enemy"
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // Nếu không còn enemy, chuyển sang Scene Win
        if (enemyCount == 0)
        {
            LoadWinScene();
        }
    }

    public void EnemyDefeated()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            LoadWinScene();
        }
    }

    void LoadWinScene()
    {
        SceneManager.LoadScene("Win");
    }
}
