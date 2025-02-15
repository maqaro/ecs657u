using UnityEngine;
using UnityEngine.Events;

public class LevelTrigger : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Pause the game by setting time scale to 0
            Time.timeScale = 0f;
            Debug.Log("Player entered trigger");
            // Unlock and show the cursor for menu interaction
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // Pause all audio
            AudioListener.pause = true;
            // Call assigned events (if any)
            OnPlayerEnter?.Invoke();
        }
    }
    public void ResumeGame()
    {
        Debug.Log("Resuming game");
        Time.timeScale = 1f;  // Resume the game
        Cursor.lockState = CursorLockMode.Locked;  // Lock the cursor again
        Cursor.visible = false;  // Hide the cursor
    }
}