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
        _particlesModule = particles.main;
        sR = GetComponent<SpriteRenderer>();
        switch(tag)
        {
            case Tag.ZigZag:
                currentColor = color1;
                currentColor2 = color4;
                break;
            case Tag.Flapper:
                currentColor = color2;
                currentColor2 = color5;
                break;
            case Tag.InvFlapper:
                currentColor = color3;
                currentColor2 = color6;
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
