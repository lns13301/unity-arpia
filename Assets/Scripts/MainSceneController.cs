﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainSceneController : MonoBehaviour
{
    public GameObject main_scene;
    public GameObject book_scene;
    public GameObject loading_scene;

    private Button _game_start_button;

    public int type = 7;
    public GameObject[] character_type;

    public PlayerData playerData;

    public Text[] text;

    // Start is called before the first frame update
    void Start()
    {
        main_scene.SetActive(true);
        book_scene.SetActive(false);
        loading_scene.SetActive(false);

        _game_start_button = main_scene.transform.GetChild(0).gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("setTextAlpha", 5f);
    }

    public void DoStartGame()
    {
        main_scene.SetActive(false);
        book_scene.SetActive(true);
    }

    public void MovePreviousBook()
    {
        main_scene.SetActive(true);
        book_scene.SetActive(false);
    }

    public void MoveNextBook()
    {
        if (type.Equals(7))
        {
            return;
        }

        loading_scene.gameObject.SetActive(true);
        startFromNew();

        SceneManager.LoadScene(1);
    }

    public void SelectType(int num)
    {
        OffType();

        switch (num)
        {
            case 0:
                type = 0;
                character_type[0].SetActive(true);
                break;
            case 1:
                type = 1;
                character_type[1].SetActive(true);
                break;
            case 2:
                type = 2;
                character_type[2].SetActive(true);
                break;
            case 3:
                type = 0;
                character_type[3].SetActive(true);
                break;
            case 4:
                type = 4;
                character_type[4].SetActive(true);
                break;
            case 5:
                type = 5;
                character_type[5].SetActive(true);
                break;
        }
    }

    public void setTextAlpha()
    {
        for (int i = 0; i < text.Length; i++)
        {
            Color alpha;
            alpha = text[i].color;
            alpha.a = Mathf.Lerp(alpha.a, 1f, Time.deltaTime / 3);
            text[i].color = alpha;
        }
    }

    private void OffType()
    {
        for (int i = 0; i < character_type.Length; i++)
        {
            character_type[i].SetActive(false);
        }
    }

    public void loadAndPlay()
    {
        try
        {
            Debug.Log("저장된 게임 시작!");
            SceneManager.LoadScene(1);

            string jsonData = File.ReadAllText(Path.Combine(Application.persistentDataPath, "playerData.json"));

            if (jsonData == null || jsonData.Length < 100)
            {
                startFromNew();
                return;
            }

            playerData = JsonUtility.FromJson<PlayerData>(jsonData);

            playerData = Calculator.calcAll(playerData);
            PlayerManager.instance.gameObject.transform.position = new Vector2(playerData.playerX, playerData.playerY);
            GameObject.Find("MainCamera").transform.position = new Vector2(playerData.playerX, playerData.playerY);
        }
        catch (FileNotFoundException)
        {
            startFromNew();
        }
    }

    public void startFromNew()
    {
        Debug.Log("새로 시작!");
        SceneManager.LoadScene(1);

        PlayerData playerData = new PlayerData();

        playerData.job = Job.APPRENTICE;
        playerData.element = Element.ICE;
        playerData.playerX = 2.5f;
        playerData.playerY = -0.35f;
        playerData.sortingIndex = 3;

        playerData.accuracy = 0;
        playerData.armor = 1;
        playerData.avoid = 2;
        playerData.charm = 3;
        playerData.clearQuest = null;
        playerData.concentrationPoint = 4;
        playerData.critDam = 5;
        playerData.critRate = 6;
        playerData.currentQuest = null;
        playerData.startQuest = null;
        playerData.statPoint = 7;
        playerData.weight = 8;
        playerData.weightMax = 9;
        playerData.wisdomPoint = 10;
        playerData.dexterityPoint = 11;
        playerData.exp = 12;
        playerData.expEff = 13;
        playerData.fame = 14;
        playerData.gold = 15;
        playerData.healthPoint = 100;
        playerData.healthPointMax = 100;
        playerData.intellectPoint = 18;
        playerData.level = 1;
        playerData.manaPoint = 100;
        playerData.manaPointMax = 100;
        playerData.money = 22;
        playerData.name = "최초의 마술사";
        playerData.nextExp = 20;
        playerData.power = 23;
        playerData.questId = 100;
        playerData.questActionIndex = 0;
        playerData.inventorySize = 10;

        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        //File.Delete(path);
        File.WriteAllText(path, jsonData);

/*        playerData.items = new List<Item>();
        //playerData.equipments = new Item[11];

        string jsonData = JsonUtility.ToJson(playerData, false);
        // pc 저장
        //string path = Path.Combine(Application.dataPath, "playerData.json");
        // 모바일 저장
        //string path = Path.Combine(Application.persistentDataPath, "playerData.json");
        File.WriteAllText(saveOrLoad(true, true), jsonData);*/
    }

    public string saveOrLoad(bool isMobile, bool isSave)
    {
        if (isSave)
        {
            if (isMobile)
            {
                // 모바일 저장
                return Path.Combine(Application.persistentDataPath, "playerData.json");
            }
            else
            {
                // pc 저장
                return Path.Combine(Application.dataPath, "playerData.json");
            }
        }
        else
        {
            if (isMobile)
            {
                // 모바일 로드
                return Path.Combine(Application.persistentDataPath, "playerData.json");
            }
            else
            {
                // pc 로드
                return Path.Combine(Application.dataPath, "playerData.json");
            }
        }
    }

    public void gameExit()
    {
        Application.Quit();
    }
}
