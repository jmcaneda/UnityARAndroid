using UnityEngine;

public class MaterialChecker : MonoBehaviour
{
    public Material replacementMaterialURP;
    public string problematicMaterialName = "SimulationLitGray";
    public MessageController messageController;

    void Start()
    {
        if (messageController == null)
            messageController = Object.FindAnyObjectByType<MessageController>();

        ReplaceProblematicMaterials();
    }

    void ReplaceProblematicMaterials()
    {
        Renderer[] renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        int replacements = 0;

        foreach (Renderer renderer in renderers)
        {
            for (int i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                Material mat = renderer.sharedMaterials[i];
                if (mat != null &&
                    (mat.name == problematicMaterialName || mat.shader.name == "Simulation/Standard Lit"))
                {
                    renderer.materials[i] = replacementMaterialURP;
                    replacements++;
                    messageController?.ShowInfoMessage("[!] Reemplazo aplicado en: " + renderer.gameObject.name);
                }
            }
        }

        if (replacements > 0)
        {
            messageController?.ShowInfoMessage("[OK] Materiales conflictivos reemplazados.");
        }
        else
        {
            messageController?.ShowInfoMessage("[OK] No se detectaron materiales conflictivos en esta escena.");
        }
    }
}
