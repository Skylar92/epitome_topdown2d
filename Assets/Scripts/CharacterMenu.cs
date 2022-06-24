using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : Stateful
{
    // Text fields
    public TMP_Text levelText, hitPointText, moneyText, upgradeCostText, xpText;

    // Logic
    private int _currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    public void OnWeaponUpgradeClick()
    {
        var upgradeWeapon = GameManager.Instance.UpgradeWeapon();
        if (!upgradeWeapon) return;

        UpdateMenu();
    }

    // Character Selection
    public void OnArrowClick(bool isRight)
    {
        if (isRight)
        {
            _currentCharacterSelection++;
            if (_currentCharacterSelection == GameManager.Instance.playerSprites.Count)
                _currentCharacterSelection = 0;

            OnSelectionChange();
        }
        else
        {
            _currentCharacterSelection--;
            if (_currentCharacterSelection < 0)
                _currentCharacterSelection = GameManager.Instance.playerSprites.Count - 1;

            OnSelectionChange();
        }
    }

    private void OnSelectionChange()
    {
        characterSelectionSprite.sprite = GameManager.Instance.playerSprites[_currentCharacterSelection];
        GameManager.Instance.player.UpdatePlayerSprite(characterSelectionSprite.sprite);
    }

    public void UpdateMenu()
    {
        var gameManager = GameManager.Instance;

        // Weapon
        if (gameManager.weaponList.Count != gameManager.weapon.weaponLevel)
        {
            var weaponMetaNext = gameManager.weaponList[Mathf.Max(0, gameManager.weapon.weaponLevel)];
            weaponSprite.sprite = weaponMetaNext.sprite;
            upgradeCostText.text = weaponMetaNext.cost.ToString();
        }
        else
        {
            var weaponMeta = gameManager.weaponList.Last();
            weaponSprite.sprite = weaponMeta.sprite;
            upgradeCostText.text = "MAX";
        }


        // Meta
        hitPointText.text = $"{gameManager.player.hitPoint} / {gameManager.player.maxHitPoint}";
        moneyText.text = gameManager.money.ToString();
        levelText.text = gameManager.player.level.ToString();

        // XP Bar
        var nextLevelExperienceNeed = gameManager.xpTable[Mathf.Max(0, gameManager.player.level - 1)];
        var experience = gameManager.experience;
        xpText.text = $"{gameManager.experience} / {nextLevelExperienceNeed}";
        var levelExperienceNeed = Mathf.Min((float)experience / nextLevelExperienceNeed, 1);

        xpBar.localScale = new Vector3(levelExperienceNeed, 1, 1);
    }
}