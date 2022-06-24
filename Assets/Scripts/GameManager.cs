using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string PlayerDataMoneyKey = "playerDataMoney";
    private const string PlayerDataExperienceKey = "playerDataExperience";

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
    }

    public void LoadState(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey(PlayerDataMoneyKey)) return;
        
        money = PlayerPrefs.GetInt(PlayerDataMoneyKey);
        experience = PlayerPrefs.GetInt(PlayerDataExperienceKey);

        player.transform.position = GameObject.Find("PlayerStart").transform.position;
    }
}