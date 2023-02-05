using UnityEngine;
using UnityEngine.Sprites;

public class NutrientZone : MonoBehaviour
{
    [SerializeField]
    private Tag tag;
    private SpriteRenderer sR;
    [SerializeField]
    private Color color1, color2, color3, color4, color5, color6; //= new Color(120, 185, 75, 78); //ZigZag
    public Color currentColor, currentColor2;

    private ParticleSystem particles;
    private ParticleSystem.MainModule _particlesModule;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentInChildren<ParticleSystem>();

        sR = GetComponent<SpriteRenderer>();
        switch(tag)
        {
            case Tag.ZigZag:
                
                currentColor = color1;
                break;
            case Tag.Flapper:
                sR.color = color2;
                currentColor = color2;
                break;
            case Tag.InvFlapper:
                sR.color = color3;
                currentColor = color3;
                break;
        }
        sR.color = currentColor;
        _particlesModule.startColor = currentColor2;
    }
}
public enum Tag
{
    ZigZag,
    Flapper,
    InvFlapper
}
