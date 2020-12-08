public class PlayerInfo
{
    public int health;
    public int maxHealth;
    public int stamina;
    public int maxStamina;
    public int healthStat;
    public int staminaStat;
    public int strengthStat;
    public int dexterityStat;
    public int defenseStat;

    public PlayerInfo(int health = 100,
        int maxHealth = 100,
        int stamina = 100,
        int maxStamina = 100,
        int healthStat = 1,
        int staminaStat = 1,
        int strengthStat = 1,
        int dexterityStat = 1,
        int defenseStat = 1)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        this.stamina = stamina;
        this.maxStamina = maxStamina;
        this.healthStat = healthStat;
        this.staminaStat = staminaStat;
        this.strengthStat = strengthStat;
        this.dexterityStat = dexterityStat;
        this.defenseStat = defenseStat;
    }
}
