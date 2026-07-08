using UnityEngine;

public class Flag : MonoBehaviour
{
    public GameObject winUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something entered the flag: " + collision.name);

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player reached the flag!");

            Time.timeScale = 0f;
            winUI.SetActive(true);
        }
    }
}