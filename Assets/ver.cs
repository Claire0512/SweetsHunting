using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class ver : MonoBehaviour
{
    public float curveStrength = 1f;

    private void OnEnable()
    {
        var textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.havePropertiesChanged = true; // Make sure the text updates
        TextMeshProUGUI m_textMeshPro = GetComponent<TextMeshProUGUI>();
        m_textMeshPro.enableVertexGradient = true;

        // Modify vertex data in the text
        textMesh.ForceMeshUpdate();
        var textInfo = textMesh.textInfo;
        var vertexColors = textMesh.mesh.colors32;
        var vertices = textInfo.meshInfo[0].vertices;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            var bottomLeft = vertices[charInfo.vertexIndex + 0];
            var topLeft = vertices[charInfo.vertexIndex + 1];
            var topRight = vertices[charInfo.vertexIndex + 2];
            var bottomRight = vertices[charInfo.vertexIndex + 3];

            Vector3 offset = Vector3.up * Mathf.Sin(i * 0.2f) * curveStrength;

            vertices[charInfo.vertexIndex + 0] = bottomLeft + offset;
            vertices[charInfo.vertexIndex + 1] = topLeft + offset;
            vertices[charInfo.vertexIndex + 2] = topRight + offset;
            vertices[charInfo.vertexIndex + 3] = bottomRight + offset;
        }

        // Update the mesh with the modified vertex data
        textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
