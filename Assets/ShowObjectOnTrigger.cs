using UnityEngine;

public class ShowObjectOnTrigger : MonoBehaviour
{
    public GameObject objectToShow;  // 要顯示的物件

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))  // 檢查進入觸發器的物件標籤是否為"Player"
        {
            objectToShow.SetActive(true);  // 顯示物件
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))  // 檢查離開觸發器的物件標籤是否為"Player"
        {
            objectToShow.SetActive(false);  // 隱藏物件
        }
    }
}
