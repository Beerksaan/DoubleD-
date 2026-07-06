using UnityEngine;

public class flag : MonoBehaviour
{
    public GameObject winUi;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0;
            winUi.SetActive(true);
        }
    }
}