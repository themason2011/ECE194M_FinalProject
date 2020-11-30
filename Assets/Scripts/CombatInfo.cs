public class CombatInfo
{
    public string enemy;
    public int enemyHealth;
    public int enemyStrength;
    public int enemyDexterity;
    public int enemyDefense;
    public int enemyStamina;

    public CombatInfo(string enemy = "",
        int enemyHealth = 0,
        int enemyStrength = 0,
        int enemyDexterity = 0,
        int enemyDefense = 0,
        int enemyStamina = 0)
    {
        this.enemy = enemy;
        this.enemyHealth = enemyHealth;
        this.enemyStrength = enemyStrength;
        this.enemyDexterity = enemyDexterity;
        this.enemyDefense = enemyDefense;
        this.enemyStamina = enemyStamina;
    }
}
