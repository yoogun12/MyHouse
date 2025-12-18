using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [Header("이동할 씬 이름")]
    public string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}