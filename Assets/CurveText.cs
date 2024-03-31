using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CurveText : MonoBehaviour
{
    public float radius = 5f;

    private void OnEnable()
    {
        var textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.havePropertiesChanged = true; // 確保文字更新
        var textInfo = textMesh.textInfo;

        textMesh.ForceMeshUpdate();
        var vertices = textInfo.meshInfo[0].vertices;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            float angle = (i / (float)textInfo.characterCount) * 180f * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);

            vertices[charInfo.vertexIndex + 0] += offset;
            vertices[charInfo.vertexIndex + 1] += offset;
            vertices[charInfo.vertexIndex + 2] += offset;
            vertices[charInfo.vertexIndex + 3] += offset;
        }

        textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
