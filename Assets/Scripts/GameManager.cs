using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string PlayerDataMoneyKey = "playerDataMoney";
    private const string PlayerDataExperienceKey = "playerDataExperience";
    private const string PlayerDataLevelKey = "playerDataLevel";
    private const string PlayerDataWeaponLevelKey = "playerWeaponLevel";

    public static GameManager Instance { get; private set; }

    // Resources
    public List<Sprite> playerSprites;
    public List<int> xpTable;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;

    public int money;
    public int experience;

    public List<WeaponMeta> weaponList;
    public List<Stateful> statefulObjects;

    private GameManager()
    {
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += LoadState;
        }
        else
        {
            Destroy(gameObject);
            statefulObjects.ForEach(stateful => Destroy(stateful.gameObject));
        }
    }

    public void ShowText(string message, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(message, fontSize, color, position, motion, duration);
    }

    public bool UpgradeWeapon()
    {
        // If it max lvl of available weapon, we can't upgrade 
        if (weapon.weaponLevel == weaponList.Count) return false;

        var weaponPrice = weaponList[weapon.weaponLevel].cost;
        // If not enough money, return false 
        if (money < weaponPrice) return false;

        // take money from player
        money -= weaponPrice;
        // increment weapon level
        weapon.weaponLevel += 1;
        // update player weapon
        var weaponMeta = weaponList[weapon.weaponLevel - 1];
        weapon.UpdateWeapon(weaponMeta);

        return true;
    }

    public void GrandXp(int xpValue)
    {
        var nextLevelXp = xpTable[player.level - 1];

        if (player.level == xpTable.Count)
            return;

        if (experience + xpValue >= nextLevelXp)
        {
            experience = experience + xpValue - nextLevelXp;
            player.OnLevelUp();
        }
        else
        {
            experience += xpValue;
        }
    }

    public void SaveState()
    {
        PlayerPrefs.SetInt(PlayerDataMoneyKey, money);
        PlayerPrefs.SetInt(PlayerDataExperienceKey, experience);
        PlayerPrefs.SetInt(PlayerDataLevelKey, player.level);
        PlayerPrefs.SetInt(PlayerDataWeaponLevelKey, weapon.weaponLevel);
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        // Some useful things
        if (PlayerPrefs.HasKey(PlayerDataMoneyKey))
            money = PlayerPrefs.GetInt(PlayerDataMoneyKey);

        if (PlayerPrefs.HasKey(PlayerDataExperienceKey))
            experience = PlayerPrefs.GetInt(PlayerDataExperienceKey);

        // Load weapon
        if (PlayerPrefs.HasKey(PlayerDataWeaponLevelKey))
        {
            var weaponLevel = PlayerPrefs.GetInt(PlayerDataWeaponLevelKey);
            var weaponMeta = weaponList[weaponLevel - 1];
            weapon.weaponLevel = weaponLevel;
            weapon.UpdateWeapon(weaponMeta);
        }

        // Update player setting and other stuff
        if (PlayerPrefs.HasKey(PlayerDataLevelKey))
        {
            player.level = Mathf.Max(player.level, PlayerPrefs.GetInt(PlayerDataLevelKey));
            player.OnPlayerLoaded();
        }

        player.transform.position = GameObject.Find("PlayerStart").transform.position;
    }
}