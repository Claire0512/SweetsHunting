using UnityEngine;

public class ShowObjectOnTrigger : MonoBehaviour
{
    public GameObject objectToShow;  // �n��ܪ�����

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))  // �ˬd�i�JĲ�o����������ҬO�_��"Player"
        {
            objectToShow.SetActive(true);  // ��ܪ���
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))  // �ˬd���}Ĳ�o����������ҬO�_��"Player"
        {
            objectToShow.SetActive(false);  // ���ê���
        }
    }
}
