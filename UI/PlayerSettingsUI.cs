/*
 using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSettingsUI : MonoBehaviour
{
    [SerializeField] private UIDocument _document;
    [SerializeField] private PlayerSettings _settingsP1;
    [SerializeField] private PlayerSettings _settingsP2;
    
    private void Start() => ShowUI(false);
    private void SetupUI()
    {
        VisualElement root = _document.rootVisualElement;
        VisualElement p1root = root.Q("p1root");
        VisualElement p2root = root.Q("p2root");

        SetupPlayerSettings(p1root, _settingsP1);
        SetupPlayerSettings(p2root, _settingsP2);
    }

    private void SetupPlayerSettings(VisualElement root, PlayerSettings settings)
    {
        SerializedObject so = new SerializedObject(settings);

        //Create Horizontal Sensitivity slider, with correct style.
        Slider xSensitivity = new Slider("Horizontal Sensitivity", 50f, 240f);
        xSensitivity.AddToClassList("sensitivitySlider");

        //Bind property to value in Settings
        SerializedProperty xSensSP = so.FindProperty("XRotateSpeed");
        xSensitivity.bindingPath = xSensSP.propertyPath;
        xSensitivity.Bind(so);

        //Finally, add it to the root element.
        root.Add(xSensitivity);


        //Repeat for Vertical slider
        Slider ySensitivity = new Slider("Vertical Sensitivity", 50f, 150f);
        ySensitivity.AddToClassList("sensitivitySlider");

        SerializedProperty ySensSP = so.FindProperty("YRotateSpeed");
        ySensitivity.bindingPath = ySensSP.propertyPath;
        ySensitivity.Bind(so);

        root.Add(ySensitivity);
    }
    public void ShowUI(bool show)
    {
        gameObject.SetActive(show);
        if (gameObject.activeSelf)
        {
            SetupUI();
        }
    }
    public void ShowUI() => ShowUI(!gameObject.activeSelf);

}
*/