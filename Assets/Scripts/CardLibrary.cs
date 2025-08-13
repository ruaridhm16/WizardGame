using UnityEngine;

public static class CardLibrary
{
    public static Fireball FireballTemplate;
    public static Lifegel LifegelTemplate;
    public static ManaBerry ManaBerryTemplate;

    public static void Initialize()
    {
        FireballTemplate = new Fireball();
        LifegelTemplate = new Lifegel();
        ManaBerryTemplate = new ManaBerry();
    }
}
