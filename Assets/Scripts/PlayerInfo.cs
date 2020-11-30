public class PlayerInfo
{
    public int health;
    public int stamina;
    public int strength;
    public int dexterity;
    public int defense;

    public PlayerInfo(int health = 100,
        int stamina = 100,
        int strength = 3,
        int dexterity = 3,
        int defense = 3)
    {
        this.health = health;
        this.stamina = stamina;
        this.strength = strength;
        this.dexterity = dexterity;
        this.defense = defense;
    }
}
