using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[DisallowMultipleComponent]
[RequireComponent(typeof(Renderer))]


public class FlickeringEmissiveLight : MonoBehaviour {
    [SerializeField]
    private bool flicker;
    [SerializeField]
    [Min(0)]
    private float flickerSpeed = 1f;
    [SerializeField]
    private AnimationCurve BrightnessCurve;

    private Renderer Renderer;
    private List<Material> Materials = new();
    private List<Color> InitialColors = new();

    private const string EMISSIVE_COLOR_NAME = "_EmissionColor";
    private const string EMISSIVE_KEYWORD = "_EMISSION";


    private void Awake() {
        
        Renderer = GetComponent<Renderer>();
        BrightnessCurve.postWrapMode = WrapMode.Loop;

        foreach (Material material in Renderer.materials) {
            if (Renderer.material.enabledKeywords.Any(item => item.name == EMISSIVE_KEYWORD)
                && Renderer.material.HasColor(EMISSIVE_COLOR_NAME)) 
            {
                Materials.Add(material);
                InitialColors.Add(material.GetColor(EMISSIVE_COLOR_NAME));
            }
            else {
                Debug.LogWarning($"{material.name} is not configured to be emissive" + $"so FlickeringEmissive on {name} cannot animate this material");
            }
        }

        if (Materials.Count == 0){

            enabled = false; // this will disable script if there is no material attached to object

        }

    }

    private void Update() {

        if (flicker && Renderer.isVisible){

            float scaledTime = Time.time * flickerSpeed;

            for (int i = 0; i < Materials.Count; i++) 
            {

                Color color = InitialColors[i];
                float brightness = BrightnessCurve.Evaluate(scaledTime);
                color = new Color(
                    color.r * Mathf.Pow(2, brightness),
                    color.g * Mathf.Pow(2, brightness),
                    color.b * Mathf.Pow(2, brightness),
                    color.a

                    );

                Materials[i].SetColor(EMISSIVE_COLOR_NAME, color);


            }
        }
    }


}
