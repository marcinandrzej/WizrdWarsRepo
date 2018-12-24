using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameView : MonoBehaviour
{
    //Power point panel dimension
    private const int POWER_POINT_SIZE = 50;
    private const int TEXT_BLOCK_SIZE = 30;

    //Power point panel dimension
    private const int POWER_BUTTON_SIZE = 60;
    private const int POWER_BUTTON_OFFSET_X = 5;
    private const int POWER_BUTTON_OFFSET_Y = 5;

    //Hp panel dimension
    private const int HP_IMG_SIZE = 50;
    private const int HP_IMG_OFFSET_X = 10;
    private const int HP_IMG_OFFSET_Y = 10;

    private int COLORS_COUNT;
    private int CELL_COUNT;
    private int BASE_POWER_COST;
    private int BASE_POWER_DAMAGE;
    private int POWERS_COUNT;
    private int MOVES_IN_TURN;

    private Sprite[] elementsSprites;
    private Sprite spr;
    private Sprite waitSprite;
    private Sprite blockSprite;
    private Sprite[] powerSprites;
    private Sprite[] hpResistanceSprites;
    private Sprite menuSpr;

    private Color32[] colors;

    private GameObject powerPointsPanel;
    private GameObject[] powerText;
    private GameObject movesText;

    private GameObject gamePanel;
    private GameObject[,] tab;

    private GameObject powerPanel;
    private GameObject[,] powerTab;

    private GameObject[] hpPanels;
    private GameObject[] hpImgP1;
    private GameObject[] hpImgP2;
    private GameObject[] hpTextP1;
    private GameObject[] hpTextP2;

    private GameObject[] statePanels;

    private GameObject gameOverPanel;
    private GameObject exitButton;

    private GuiManagerScript guiScr;
    private GameManagerScript gameManager;

    private action onClick;

    public void SetUp(int _colCount, int _celCount, int _powerCost, int _powerDamage,
        int _powersCount, int _movesInTurn, GameManagerScript _gameManager, Sprite[] _elementsSprites,
        Sprite _spr, Sprite _waitSprite, Sprite _blockSprite, Sprite[] _powerSprites, Sprite[] _hpResistanceSprites,
        Sprite _menuSpr)
    {
        guiScr = new GuiManagerScript();
        gameManager = _gameManager;

        COLORS_COUNT = _colCount;
        CELL_COUNT = _celCount;
        BASE_POWER_COST = _powerCost;
        BASE_POWER_DAMAGE = _powerDamage;
        POWERS_COUNT = _powersCount;
        MOVES_IN_TURN = _movesInTurn;

        elementsSprites = _elementsSprites;
        spr = _spr;
        waitSprite = _waitSprite;
        blockSprite = _blockSprite;
        powerSprites = _powerSprites;
        hpResistanceSprites = _hpResistanceSprites;
        menuSpr = _menuSpr;

        colors = new Color32[COLORS_COUNT];
        colors[0] = new Color32(255, 0, 0, 255);
        colors[1] = new Color32(100, 50, 0, 255);
        colors[2] = new Color32(0, 255, 255, 255);
        colors[3] = new Color32(255, 255, 0, 255);
        colors[4] = new Color32(150, 200, 200, 255);
    }

    public void SetUpGamePanel()
    {
        gameManager.SetMoves(MOVES_IN_TURN);
        gamePanel = guiScr.CreatePanel(gameManager.gameObject, "GamePanel", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
            new Vector3(0.5f, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(320, 320),
            new Vector2(0, -130), spr, new Color32(0, 255, 0, 100));
        tab = guiScr.FillWithButtons(gamePanel, CELL_COUNT, CELL_COUNT, spr, colors);
        onClick = new action(gameManager.Execute);
        guiScr.SetAction(tab, onClick);
    }

    public void SetUpPowerPanel(RuntimeAnimatorController powerAnimCon)
    {
        powerPanel = guiScr.CreatePanel(gameManager.gameObject, "PowerPanel", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
            new Vector3(0.5f, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
            new Vector2(POWERS_COUNT * (POWER_BUTTON_SIZE + POWER_BUTTON_OFFSET_X) - POWER_BUTTON_OFFSET_X,
            COLORS_COUNT * (POWER_BUTTON_SIZE + POWER_BUTTON_OFFSET_Y) + POWER_BUTTON_SIZE),
            new Vector2(0, -90), spr, new Color32(0, 255, 0, 100));
        powerTab = new GameObject[POWERS_COUNT, COLORS_COUNT];

        for (int x = 0; x < POWERS_COUNT; x++)
        {
            for (int y = 0; y < COLORS_COUNT; y++)
            {
                powerTab[x, y] = guiScr.CreateButton(powerPanel, "PowerButton", new Vector2(0, 1), new Vector2(0, 1),
                    new Vector2(0, 1), new Vector3(1, 1, 1), new Vector3(0, 0),
                    new Vector2(POWER_BUTTON_SIZE, POWER_BUTTON_SIZE),
                    new Vector2((x * (POWER_BUTTON_SIZE + POWER_BUTTON_OFFSET_X)),
                    -(y * (POWER_BUTTON_SIZE + POWER_BUTTON_OFFSET_Y))),
                    powerSprites[(x + (y * POWERS_COUNT))], new Color32(255,255,255,255));

                GameObject txt = guiScr.CreateText(powerTab[x, y], "Txt", new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
                new Vector3(0, 0, 0), new Vector2(30, 25), new Vector2(0, -5),
                (BASE_POWER_COST * (x + 1)).ToString(), new Color32(0, 0, 255, 255));

                powerTab[x, y].AddComponent<PowerScript>();
                powerTab[x, y].GetComponent<PowerScript>().SetUpPower(BASE_POWER_COST * (x + 1),
                    BASE_POWER_DAMAGE, y, x, powerAnimCon);

                int _x = x;
                int _y = y;
                guiScr.SetAction(powerTab[x, y], _x, _y, new action3(gameManager.Attack));
            }
        }

        GameObject but = guiScr.CreateButton(powerPanel, "PassButton", new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                    new Vector2(0.5f, 0), new Vector3(1, 1, 1), new Vector3(0, 0),
                    new Vector2(POWER_BUTTON_SIZE, POWER_BUTTON_SIZE),
                    new Vector2(0, 0), waitSprite, new Color32(255, 255, 255, 255));
        guiScr.SetAction(but, new action2(gameManager.EnemyMove));
    }

    public void UpdatePowerPanelView(int[] elemPowPoi)
    {
        for (int x = 0; x < POWERS_COUNT; x++)
        {
            for (int y = 0; y < COLORS_COUNT; y++)
            {
                if (elemPowPoi[y] < powerTab[x, y].GetComponent<PowerScript>().Cost)
                {
                    powerTab[x, y].GetComponent<Button>().enabled = false;
                    powerTab[x, y].GetComponent<Image>().sprite = blockSprite;
                    powerTab[x, y].GetComponent<Image>().color = colors[y];
                }
                else
                {
                    powerTab[x, y].GetComponent<Button>().enabled = true;
                    powerTab[x, y].GetComponent<Image>().sprite = powerSprites[(x + (y * POWERS_COUNT))];
                    powerTab[x, y].GetComponent<Image>().color = new Color32(255,255,255,255);
                }
            }
        }
    }

    public void SetUpPowerPointsPanel()
    {
        powerText = new GameObject[5];

        powerPointsPanel = guiScr.CreatePanel(gameManager.gameObject, "PowerPointsPanel", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
            new Vector3(0.5f, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(6 * POWER_POINT_SIZE,
            POWER_POINT_SIZE + TEXT_BLOCK_SIZE), new Vector2(0, 0), spr, new Color32(0, 255, 0, 100));

        guiScr.CreateImage(powerPointsPanel, "Fire", new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1),
            new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, POWER_POINT_SIZE),
            new Vector2(0, 0), elementsSprites[0]);
        powerText[0] = guiScr.CreateText(powerPointsPanel, "FirePowerPoints", new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, TEXT_BLOCK_SIZE),
            new Vector2(0, 0), "0", new Color32(255, 255, 255, 255));

        guiScr.CreateImage(powerPointsPanel, "Earth", new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1),
            new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, POWER_POINT_SIZE),
            new Vector2(POWER_POINT_SIZE, 0), elementsSprites[1]);
        powerText[1] = guiScr.CreateText(powerPointsPanel, "EarthPowerPoints", new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, TEXT_BLOCK_SIZE),
            new Vector2(POWER_POINT_SIZE, 0), "0", new Color32(255, 255, 255, 255));

        guiScr.CreateImage(powerPointsPanel, "Water", new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1),
            new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, POWER_POINT_SIZE),
            new Vector2(2 * POWER_POINT_SIZE, 0), elementsSprites[2]);
        powerText[2] = guiScr.CreateText(powerPointsPanel, "WaterPowerPoints", new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, TEXT_BLOCK_SIZE),
            new Vector2(2 * POWER_POINT_SIZE, 0), "0", new Color32(255, 255, 255, 255));

        guiScr.CreateImage(powerPointsPanel, "Electric", new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1),
            new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, POWER_POINT_SIZE),
            new Vector2(3 * POWER_POINT_SIZE, 0), elementsSprites[3]);
        powerText[3] = guiScr.CreateText(powerPointsPanel, "ElectricPowerPoints", new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, TEXT_BLOCK_SIZE),
            new Vector2(3 * POWER_POINT_SIZE, 0), "0", new Color32(255, 255, 255, 255));

        guiScr.CreateImage(powerPointsPanel, "Wind", new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1),
            new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, POWER_POINT_SIZE),
            new Vector2(4 * POWER_POINT_SIZE, 0), elementsSprites[4]);
        powerText[4] = guiScr.CreateText(powerPointsPanel, "WindPowerPoints", new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, TEXT_BLOCK_SIZE),
            new Vector2(4 * POWER_POINT_SIZE, 0), "0", new Color32(255, 255, 255, 255));

        guiScr.CreateText(powerPointsPanel, "MovesTxt", new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1),
            new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(POWER_POINT_SIZE, TEXT_BLOCK_SIZE),
            new Vector2(5 * POWER_POINT_SIZE, 0), "Left", new Color32(255, 255, 255, 255));
        movesText = guiScr.CreateText(powerPointsPanel, "MovesLeft", new Vector2(0, 0), new Vector2(0, 0),
            new Vector2(0, 0), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
            new Vector2(POWER_POINT_SIZE, POWER_POINT_SIZE),
            new Vector2(5 * POWER_POINT_SIZE, 0), "0", new Color32(255, 255, 255, 255));
    }

    public void UpdatePowerPointsView(int[] elemPowPoi, int moves)
    {
        for (int i = 0; i < powerText.Length; i++)
        {
            powerText[i].GetComponent<Text>().text = elemPowPoi[i].ToString();
        }
        movesText.GetComponent<Text>().text = moves.ToString();
    }

    public void SetUpHpPanels()
    {
        hpPanels = new GameObject[2];
        hpImgP1 = new GameObject[6];
        hpImgP2 = new GameObject[6];
        hpTextP1 = new GameObject[6];
        hpTextP2 = new GameObject[6];

        GameObject panel = guiScr.CreatePanel(gameManager.gameObject, "HpPanelP1", new Vector2(0, 1),
            new Vector2(0, 1), new Vector2(0, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
            new Vector2(180, 130), new Vector2(0, 0), spr, new Color32(0, 255, 0, 100));
        hpPanels[0] = panel;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject image = guiScr.CreateImage(panel, "hpResImg", new Vector2(0, 1),
                new Vector2(0, 1), new Vector2(0, 1), new Vector3(1, 1, 1),
                new Vector3(0, 0, 0), new Vector2(HP_IMG_SIZE, HP_IMG_SIZE),
                new Vector2(j * (HP_IMG_OFFSET_X + HP_IMG_SIZE) + HP_IMG_OFFSET_X,
                -(i * (HP_IMG_OFFSET_Y + HP_IMG_SIZE) + HP_IMG_OFFSET_Y)), hpResistanceSprites[(i * 3 + j)]);

                GameObject txt = guiScr.CreateText(image, "hpResTxt", new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
                new Vector3(0, 0, 0), new Vector2(40, 20), new Vector2(0, 0), "0", new Color32(0, 0, 0, 255));

                hpImgP1[(i * 3 + j)] = image;
                hpTextP1[(i * 3 + j)] = txt;
            }
        }

        GameObject panel2 = guiScr.CreatePanel(gameManager.gameObject, "HpPanelP2", new Vector2(1, 1),
           new Vector2(1, 1), new Vector2(1, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
           new Vector2(180, 130), new Vector2(0, 0), spr, new Color32(0, 255, 0, 100));
        hpPanels[1] = panel2;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject image = guiScr.CreateImage(panel2, "hpResImg", new Vector2(0, 1),
                new Vector2(0, 1), new Vector2(0, 1), new Vector3(1, 1, 1),
                new Vector3(0, 0, 0), new Vector2(HP_IMG_SIZE, HP_IMG_SIZE),
                new Vector2(j * (HP_IMG_OFFSET_X + HP_IMG_SIZE) + HP_IMG_OFFSET_X,
                -(i * (HP_IMG_OFFSET_Y + HP_IMG_SIZE) + HP_IMG_OFFSET_Y)), hpResistanceSprites[(i * 3 + j)]);

                GameObject txt = guiScr.CreateText(image, "hpResTxt", new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
                new Vector3(0, 0, 0), new Vector2(40, 20), new Vector2(0, 0), "0", new Color32(0, 0, 0, 255));

                hpImgP2[(i * 3 + j)] = image;
                hpTextP2[(i * 3 + j)] = txt;
            }
        }
    }
    
    public void UpdateHpPanels(GameObject player1, GameObject player2)
    {
        PlayerScript p1 = player1.GetComponent<PlayerScript>();
        PlayerScript p2 = player2.GetComponent<PlayerScript>();

        hpTextP1[0].GetComponent<Text>().text = p1.CurrentHp.ToString();
        hpTextP2[0].GetComponent<Text>().text = p2.CurrentHp.ToString();

        for (int i = 1; i < 6; i++)
        {
            hpTextP1[i].GetComponent<Text>().text = p1.ElementalShield[i - 1].ToString();
            hpTextP2[i].GetComponent<Text>().text = p2.ElementalShield[i - 1].ToString();
        }
    }

    private void SetUpStatePanels()
    {
        statePanels = new GameObject[2];

        GameObject panel = guiScr.CreatePanel(gameManager.gameObject, "StatePanelP1", new Vector2(0, 1),
            new Vector2(0, 1), new Vector2(0, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
            new Vector2(180, 50), new Vector2(0, -130), spr, new Color32(0, 0, 0, 0));
        statePanels[0] = panel;

        GameObject panel2 = guiScr.CreatePanel(gameManager.gameObject, "StatePanelP2", new Vector2(1, 1),
           new Vector2(1, 1), new Vector2(1, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
           new Vector2(180, 50), new Vector2(0, -130), spr, new Color32(0, 0, 0, 0));
        statePanels[1] = panel2;
    }

    public void UpDateStatePanels(GameObject player1, GameObject player2, Sprite poisonSpr, Sprite freezeSpr)
    {
        if (statePanels != null)
        {
            Destroy(statePanels[0]);
            Destroy(statePanels[1]);
        }

        SetUpStatePanels();

        if (player1.GetComponent<PlayerScript>().States.Count > 0)
        {
            float sizeX = Mathf.Min((180.0f / (player1.GetComponent<PlayerScript>().States.Count)), 50.0f);
            for (int i = 0; i < player1.GetComponent<PlayerScript>().States.Count; i++)
            {
                Sprite spr = poisonSpr;
                if (!player1.GetComponent<PlayerScript>().States[i].IsPoison())
                {
                    spr = freezeSpr;
                }
                GameObject image = guiScr.CreateImage(statePanels[0], "Img", new Vector2(0, 0.5f),
                    new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector3(1, 1, 1),
                    new Vector3(0, 0, 0), new Vector2(sizeX, 50),
                    new Vector2(i * sizeX, 0), spr);

                GameObject txt = guiScr.CreateText(image, "Txt", new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
                    new Vector3(0, 0, 0), new Vector2(40, 20), new Vector2(0, 0),
                    player1.GetComponent<PlayerScript>().States[i].Duration.ToString(), new Color32(0, 0, 0, 255));
            }
        }

        if (player2.GetComponent<PlayerScript>().States.Count > 0)
        {
            float sizeX = Mathf.Min((180.0f / (player2.GetComponent<PlayerScript>().States.Count)), 50.0f);
            for (int i = 0; i < player2.GetComponent<PlayerScript>().States.Count; i++)
            {
                Sprite spr2 = poisonSpr;
                if (!player2.GetComponent<PlayerScript>().States[i].IsPoison())
                {
                    spr2 = freezeSpr;
                }
                GameObject image = guiScr.CreateImage(statePanels[1], "Img", new Vector2(0, 0.5f),
                    new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector3(1, 1, 1),
                    new Vector3(0, 0, 0), new Vector2(sizeX, 50),
                    new Vector2(i * sizeX, 0), spr2);

                GameObject txt = guiScr.CreateText(image, "Txt", new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
                    new Vector3(0, 0, 0), new Vector2(40, 20), new Vector2(0, 0),
                    player2.GetComponent<PlayerScript>().States[i].Duration.ToString(), new Color32(0, 0, 0, 255));
            }
        }
    }

    public void SetUpGameOver()
    {
        gameOverPanel = guiScr.CreatePanel(gameManager.gameObject, "GameOverPanel", new Vector2(0.5f, 1),
            new Vector2(0.5f, 1), new Vector3(0.5f, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0),
            new Vector2(320, 320), new Vector2(0, -130), spr, new Color32(255, 255, 255, 255));
        exitButton = guiScr.CreateButton(gameOverPanel, "ExitButton", new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1), new Vector2(0, 0),
            new Vector2(200, 70), new Vector2(0, -70), spr, new Color32(255, 255, 255, 255));
        GameObject txt2 = guiScr.CreateText(exitButton, "Text", new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
            new Vector2(0, 0), new Vector2(140, 30), new Vector2(0, 0), "Main menu", new Color32(0, 0, 0, 255));
        GameObject txt3 = guiScr.CreateText(gameOverPanel, "Text", new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector3(1, 1, 1),
            new Vector2(0, 0), new Vector2(200, 70), new Vector2(0, 90), "Game over", new Color32(0, 0, 0, 255));

        exitButton.GetComponent<Button>().onClick.AddListener(
            delegate
            {
                SceneManager.LoadScene("MenuScene");
            });
    }

    public void SetUpExitButton()
    {
        GameObject but = guiScr.CreateButton(gameManager.gameObject, "Main menu", new Vector2(0, 0),
            new Vector2(0, 0), new Vector2(0, 0), new Vector3(1, 1, 1), new Vector2(0, 0),
            new Vector2(50, 50), new Vector2(0, 0), menuSpr, new Color32(255, 255, 255, 255));
        but.GetComponent<Button>().onClick.AddListener(
        delegate
        {
            SceneManager.LoadScene("MenuScene");
        });
    }
    
    public GameObject GetPowerButton(int x, int y)
    {
        return powerTab[x, y];
    }

    public GameObject GetPowerPanel()
    {
        return powerPanel;
    }

    public GameObject GetGamePanel()
    {
        return gamePanel;
    }

    public GameObject[,] GetGameGrid()
    {
        return tab;
    }
}
