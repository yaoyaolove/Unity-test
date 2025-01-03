// =============================================================================
// 文件名称: UI.cs
// 作者: 刘垚
// 创建日期: 2024.11.19
// 更新日期：2024.11.24
// 使用的设计模式：
// 备注：所有的UI接口，这里命名之后需要更改
// =============================================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Shop heroShop;

    public GameObject[] herosFrameArray;
    public GameObject[] bonusPanels;

    public Text timerText;
    public Text heroCountText;
    public Text goldText;
    public Text hpText;

    public GameObject shop;
    public GameObject restartButton;
    public GameObject gold;
    public GameObject placementText;
    public GameObject bonusContainer;
    public GameObject bonusUIPrefab;

    //UI接口，点击英雄卡片时调用，这里命名还需要进行规范
    public void HeroFrameUI()
    {
        //得到的是父容器的名字，这个在游戏对象中查看
        string name = EventSystem.current.currentSelectedGameObject.transform.parent.name;

        //由名字给出索引，分别为1、2、3、4
        string defaultName = "champion container ";
        int heroFrameIndex = int.Parse(name.Substring(defaultName.Length, 1));

        //调用商店类的方法
        heroShop.BuyHero(heroFrameIndex);
    }


    //UI接口，点击更新商店时调用
    public void RefreshUI()
    {
        heroShop.RefreshShop(false);
    }

    //UI接口，点击购买经验时调用
    public void BuyXPUI()
    {
        heroShop.BuyLevel();
    }

    //UI接口，点击重新开始游戏时调用
    public void RestartUI()
    {
        GameManager.GetInstance().RestartGame();
    }

    //方法，隐藏英雄卡片时调用，命名还需规范
    public void HideHeroFrame(int index)
    {
        herosFrameArray[index].transform.Find("champion").gameObject.SetActive(false);
    }

    //方法，展示所有的英雄卡片时调用
    public void ShowHeroFrames()
    {
        for (int i = 0; i < herosFrameArray.Length; i++)
        {
            herosFrameArray[i].transform.Find("champion").gameObject.SetActive(true);
        }
    }

    //为UI对象添加信息组件
    public void LoadShopItem(int heroIndex, int index)
    {
        //获取unity的对象
        Transform heroUI = herosFrameArray[index].transform.Find("champion");
        Transform top = heroUI.Find("top");
        Transform bottom = heroUI.Find("bottom");
        Transform type1 = top.Find("type 1");
        Transform type2 = top.Find("type 2");
        Transform name = bottom.Find("Name");
        Transform cost = bottom.Find("Cost");
        Transform icon1 = top.Find("icon 1");
        Transform icon2 = top.Find("icon 2");

        IHero hero = GameManager.GetInstance().gameHeroData.herosArray[heroIndex];
        //将英雄信息作为组件附加在对象上
        name.GetComponent<Text>().text = hero.UIName;
        cost.GetComponent<Text>().text = hero.Cost.ToString();
        type1.GetComponent<Text>().text = hero.Type1.displayName;
        type2.GetComponent<Text>().text = hero.Type2.displayName;
        icon1.GetComponent<Image>().sprite = hero.Type1.icon;
        icon2.GetComponent<Image>().sprite = hero.Type2.icon;
    }


    //需要的时候更新UI界面
    public void UpdateUI()
    {
        goldText.text = GameManager.GetInstance().currentGold.ToString();
        heroCountText.text = GameManager.GetInstance().currentHeroCount.ToString() + " / " + GameManager.GetInstance().currentLevel.ToString();
        hpText.text = "HP " + GameManager.GetInstance().currentHP.ToString();

        //hide bonusus UI
        foreach (GameObject go in bonusPanels)
        {
            go.SetActive(false);
        }

        //if not null
        if (GameManager.GetInstance().heroTypeCount != null)
        {
            int i = 0;
            //iterate bonuses
            foreach (KeyValuePair<HeroType, int> m in GameManager.GetInstance().heroTypeCount)
            {
                //Now you can access the key and value both separately from this attachStat as:
                GameObject bonusUI = bonusPanels[i];
                bonusUI.transform.SetParent(bonusContainer.transform);
                bonusUI.transform.Find("icon").GetComponent<Image>().sprite = m.Key.icon;
                bonusUI.transform.Find("name").GetComponent<Text>().text = m.Key.displayName;
                bonusUI.transform.Find("count").GetComponent<Text>().text = m.Value.ToString() + " / " + m.Key.heroBonus.heroCount.ToString();

                bonusUI.SetActive(true);

                i++;
            }
        }
    }

    //更新计时器
    public void UpdateTimerText()
    {
       timerText.text=GameManager.GetInstance().timerDisplay.ToString();
    }

    //设置计时器的可见性
    public void SetTimerTextActive(bool b)
    {
        timerText.gameObject.SetActive(b);

        //这个我还不懂是啥意思
        placementText.SetActive(b);
    }

    //游戏结束时画面
    public void ShowLossScreen()
    {
        SetTimerTextActive(false);
        shop.SetActive(false);
        gold.SetActive(false);

        restartButton.SetActive(true);
    }

    //游戏开始时显示游戏界面
    public void ShowGameScreen()
    {
        SetTimerTextActive(true);
        shop.SetActive(true);
        gold.SetActive(true);

        restartButton.SetActive(false);
    }
}
