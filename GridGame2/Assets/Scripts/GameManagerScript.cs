using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private const int CELL_COUNT = 8;
    private const int MOVES_IN_TURN = 3;
    private const int BASE_POWER_COST = 5;
    private const int BASE_POWER_DAMAGE = 10;
    private const int POWERS_COUNT = 3;
    private const int COLORS_COUNT = 5;

    private const float AI_PROBABILITY = 0.3f;
    /*
    //Power point panel dimension
    private const int POWER_POINT_SIZE = 50;
    private const int TEXT_BLOCK_SIZE = 30;

    //Power point panel dimension
    private const int POWER_BUTTON_SIZE = 60;
    private const int POWER_BUTTON_OFFSET_X = 5;
    private const int POWER_BUTTON_OFFSET_Y = 5;
    */
    public Sprite[] elementsSprites;
    public Sprite menuSpr;
    public Sprite spr;
    public Sprite waitSprite;
    public Sprite blockSprite;
    public Sprite[] powerSprites;
    public Sprite[] hpResistanceSprites;
    public RuntimeAnimatorController powerAnimCon;
    public RuntimeAnimatorController[] playerAnimCon;
    public AudioClip[] playerSounds;
    public AudioMixerGroup playerOutput;
    /*private Color32[] colors;*/
    private int[] elementsPowerPoints;
    private int[] elementsPowerPoints2;
    /*
    private GameObject powerPointsPanel;
    private GameObject[] powerText;
    private GameObject movesText;

    private GameObject gamePanel;
    private GameObject[,] tab;

    private GameObject powerPanel;
    private GameObject[,] powerTab;
    */
    // private GuiManagerScript guiScr;
    private GridScript grid;
    private GameView gameView;
    private GameObject player;
    private GameObject player2;

    private int currentPlayer;

    //private action onClick;

    private int moves;
    private int playersNumber;

    void Awake()
    {/*
        colors = new Color32[COLORS_COUNT];
        colors[0] = new Color32(255,0,0,255);
        colors[1] = new Color32(100, 50, 0, 255);
        colors[2] = new Color32(0, 255, 255, 255);
        colors[3] = new Color32(255, 255, 0, 255);
        colors[4] = new Color32(150, 200, 200, 255);
        */
        elementsPowerPoints = new int[COLORS_COUNT];
        elementsPowerPoints[0] = 0;
        elementsPowerPoints[1] = 0;
        elementsPowerPoints[2] = 0;
        elementsPowerPoints[3] = 0;
        elementsPowerPoints[4] = 0;

        elementsPowerPoints2 = new int[COLORS_COUNT];
        elementsPowerPoints2[0] = 0;
        elementsPowerPoints2[1] = 0;
        elementsPowerPoints2[2] = 0;
        elementsPowerPoints2[3] = 0;
        elementsPowerPoints2[4] = 0;

        currentPlayer = 1;
        playersNumber = 2;
    }

    // Use this for initialization
    void Start()
    {
        //guiScr = new GuiManagerScript();
        playersNumber = DataScript.instance.PlayersNumber;
        grid = new GridScript();
        gameView = gameObject.AddComponent<GameView>();

        gameView.SetUp(COLORS_COUNT, CELL_COUNT, BASE_POWER_COST, BASE_POWER_DAMAGE, POWERS_COUNT, MOVES_IN_TURN,
            this, elementsSprites, spr, waitSprite, blockSprite, powerSprites, hpResistanceSprites, menuSpr);
        SetUpPlayers();
        gameView.SetUpGamePanel();
        gameView.SetUpPowerPointsPanel();
        gameView.SetUpHpPanels();
        gameView.UpdateHpPanels(player, player2);
        gameView.UpDateStatePanels(player, player2, powerSprites[5], powerSprites[8]);
        gameView.SetUpExitButton();

        Destroy(DataScript.instance.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SetMoves(int _moves)
    {
        moves = _moves;
    }

    private void SetUpPlayers()
    {
        player = new GameObject("Player1");
        player.AddComponent<PlayerScript>();
        player.GetComponent<PlayerScript>().SetUpPlayer(new Vector3(-6, -1.5f, 0), new Vector3(1, 1.5f, 1),
            playerAnimCon[DataScript.instance.Player1Model], COLORS_COUNT, false, playerSounds, playerOutput);

        player2 = new GameObject("Player2");
        player2.AddComponent<PlayerScript>();
        player2.GetComponent<PlayerScript>().SetUpPlayer(new Vector3(6, -1.5f, 0), new Vector3(1, 1.5f, 1),
            playerAnimCon[DataScript.instance.Player2Model], COLORS_COUNT, true, playerSounds, playerOutput);
    }
    /*
    private void SetUpGamePanel()
    {
        moves = MOVES_IN_TURN;
        gamePanel = guiScr.CreatePanel(gameObject, "GamePanel", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
            new Vector3(0.5f, 1), new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(400, 400),
            new Vector2(0, -90), spr, new Color32(0, 255, 0, 100));
        tab = guiScr.FillWithButtons(gamePanel, CELL_COUNT, CELL_COUNT, spr, colors);
        onClick = new action(Execute);
        guiScr.SetAction(tab, onClick);
    }*/
    /*
    private void SetUpPowerPanel()
    {
        powerPanel = guiScr.CreatePanel(gameObject, "PowerPanel", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
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
                    powerSprites[x], colors[y]);

                powerTab[x, y].AddComponent<PowerScript>();
                powerTab[x, y].GetComponent<PowerScript>().SetUpPower(BASE_POWER_COST * (x+1),
                    BASE_POWER_DAMAGE * (x + 1), y, new Vector3(-3, 0, 0), x, powerAnimCon);

                int _x = x;
                int _y = y;
                guiScr.SetAction(powerTab[x, y], _x, _y,new action3(Attack));
            }
        }
        
        GameObject but = guiScr.CreateButton(powerPanel, "PassButton", new Vector2(0.5f, 0), new Vector2(0.5f, 0),
                    new Vector2(0.5f, 0), new Vector3(1, 1, 1), new Vector3(0, 0),
                    new Vector2(POWER_BUTTON_SIZE, POWER_BUTTON_SIZE),
                    new Vector2(0, 0), waitSprite, new Color32(255, 255, 255, 255));
        guiScr.SetAction(but, new action2(EnemyMove));
    }*/
    /*
    private void UpdatePowerPanelView()
    {
        for (int x = 0; x < POWERS_COUNT; x++)
        {
            for (int y = 0; y < COLORS_COUNT; y++)
            {
                if (elementsPowerPoints[y] < powerTab[x,y].GetComponent<PowerScript>().Cost)
                {
                    powerTab[x, y].GetComponent<Button>().enabled = false;
                    powerTab[x, y].GetComponent<Image>().sprite = blockSprite;
                }
                else
                {
                    powerTab[x, y].GetComponent<Button>().enabled = true;
                    powerTab[x, y].GetComponent<Image>().sprite = powerSprites[x];
                }
            }
        }
    }*/
    /*
    private void SetUpPowerPointsPanel()
    {
        powerText = new GameObject[5];

        powerPointsPanel = guiScr.CreatePanel(gameObject, "PowerPointsPanel", new Vector2(0.5f, 1), new Vector2(0.5f, 1),
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
            new Vector2(5 * POWER_POINT_SIZE, 0), moves.ToString(), new Color32(255, 255, 255, 255));
    }
    */
    /*
    private void UpdatePowerPointsView()
    {
        for (int i = 0; i < powerText.Length; i++)
        {
            powerText[i].GetComponent<Text>().text = elementsPowerPoints[i].ToString();
        }
        movesText.GetComponent<Text>().text = moves.ToString();
    }
    */
    //Enemy move
    public void EnemyMove()
    {
        StartCoroutine(ChangePlayerCoroutine());
    }

    private IEnumerator ChangePlayerCoroutine()
    {
        if (gameView.GetPowerPanel() != null)
        {
            gameView.GetPowerPanel().SetActive(false);
        }

        yield return new WaitForSeconds(1);

        if (currentPlayer == 1)
        {
            for (int i = 0; i < player2.GetComponent<PlayerScript>().States.Count; i++)
            {
                player2.GetComponent<PlayerScript>().States[i].Execute();
                gameView.UpdateHpPanels(player, player2);
                gameView.UpDateStatePanels(player, player2, powerSprites[5], powerSprites[8]);
                yield return new WaitForSeconds(1);
            }

            int g = 0;

            while (g < player2.GetComponent<PlayerScript>().States.Count)
            {
                if (player2.GetComponent<PlayerScript>().States[g].IsOver())
                {
                    player2.GetComponent<PlayerScript>().States.RemoveAt(g);
                }
                else
                {
                    g++;
                }
            }
        }
        else
        {
            for (int i = 0; i < player.GetComponent<PlayerScript>().States.Count; i++)
            {
                player.GetComponent<PlayerScript>().States[i].Execute();
                gameView.UpdateHpPanels(player, player2);
                gameView.UpDateStatePanels(player, player2, powerSprites[5], powerSprites[8]);
                yield return new WaitForSeconds(1);
            }

            int z = 0;

            while (z < player.GetComponent<PlayerScript>().States.Count)
            {
                if (player.GetComponent<PlayerScript>().States[z].IsOver())
                {
                    player.GetComponent<PlayerScript>().States.RemoveAt(z);
                }
                else
                {
                    z++;
                }
            }
        }
        gameView.UpDateStatePanels(player, player2, powerSprites[5], powerSprites[8]);
        NextTurn();
    }
    //Attack
    public void Attack(int x, int y)
    {
        StartCoroutine(AttackCoroutine(x, y));
    }

    public IEnumerator AttackCoroutine(int x, int y)
    {
        if (gameView.GetPowerPanel() != null)
        {
            gameView.GetPowerPanel().SetActive(false);
        }

        if (currentPlayer != 1)
        {
            elementsPowerPoints2[y] -= gameView.GetPowerButton(x, y).GetComponent<PowerScript>().Cost;
            gameView.UpdatePowerPointsView(elementsPowerPoints2, moves);
            yield return new WaitForSeconds(0.5f);
            player2.GetComponent<PlayerScript>().PlayAttack();
            yield return new WaitForSeconds(0.5f);
            gameView.GetPowerButton(x, y).GetComponent<PowerScript>().Action(player2, player,
                new Vector3(3, -1, 0), currentPlayer, this);
        }
        else
        {
            elementsPowerPoints[y] -= gameView.GetPowerButton(x, y).GetComponent<PowerScript>().Cost;
            gameView.UpdatePowerPointsView(elementsPowerPoints, moves);
            yield return new WaitForSeconds(0.5f);
            player.GetComponent<PlayerScript>().PlayAttack();
            yield return new WaitForSeconds(0.5f);
            gameView.GetPowerButton(x, y).GetComponent<PowerScript>().Action(player, player2,
                new Vector3(-3, -1, 0), currentPlayer,this);
        }
        if (x == 2 && y == 1 || x == 2 && y == 2)
        {
            gameView.UpDateStatePanels(player, player2, powerSprites[5], powerSprites[8]);
        }       
        yield return new WaitForSeconds(1.2f);
        gameView.UpdatePowerPointsView(elementsPowerPoints2, moves);
        gameView.UpdateHpPanels(player, player2);
        UpdatePowerView();
    }

    //Go to game
    private void NextTurn()
    {
        moves = MOVES_IN_TURN;
        currentPlayer = -currentPlayer;
        if (currentPlayer !=1)
        {
            if (!player2.GetComponent<PlayerScript>().IsDead)
            {
                if (playersNumber != 1)
                {
                    gameView.UpdatePowerPointsView(elementsPowerPoints2, moves);
                    gameView.SetUpGamePanel();
                }
                else
                {
                    gameView.UpdatePowerPointsView(elementsPowerPoints2, moves);
                    gameView.SetUpGamePanel();
                    grid.BlockGrid(false, gameView.GetGameGrid(), CELL_COUNT);
                    StartCoroutine(EnemyCoroutine());
                }
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            if (!player.GetComponent<PlayerScript>().IsDead)
            {
                gameView.UpdatePowerPointsView(elementsPowerPoints, moves);
                gameView.SetUpGamePanel();
            }
            else
            {
                GameOver();
            }
        }        
    }

    private void GameOver()
    {

        gameView.SetUpGameOver();
    }

    //Add power points
    public void AddPowerPoints(int index, int add, int currentPlay)
    {
        if (currentPlay != 1)
        {
            elementsPowerPoints2[index] += add;
        }
        else
        {
            elementsPowerPoints[index] += add;
        }
    }

    public int[] GetPowerPoints(int currentPlay)
    {
        if (currentPlay != 1)
        {
            return elementsPowerPoints2;
        }
        else
        {
            return elementsPowerPoints;
        }
    }

    private void UpdatePowerView()
    {
        if (currentPlayer != 1)
        {
            if (!player.GetComponent<PlayerScript>().IsDead)
            {
                gameView.UpdatePowerPointsView(elementsPowerPoints2, moves);
                if (playersNumber != 1)
                {
                    if (gameView.GetPowerPanel() == null)
                    {
                        gameView.SetUpPowerPanel(powerAnimCon);
                    }
                    else
                    {
                        gameView.GetPowerPanel().SetActive(true);
                    }
                    gameView.UpdatePowerPanelView(elementsPowerPoints2);
                }
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            if (!player2.GetComponent<PlayerScript>().IsDead)
            {
                gameView.UpdatePowerPointsView(elementsPowerPoints, moves);
                if (gameView.GetPowerPanel() == null)
                {
                    gameView.SetUpPowerPanel(powerAnimCon);
                }
                else
                {
                    gameView.GetPowerPanel().SetActive(true);
                }
                gameView.UpdatePowerPanelView(elementsPowerPoints);
            }
            else
            {
                GameOver();
            }
        }
    }

    /*
    //Block grid
    public void BlockGrid(bool enable)
    {
        for (int i = 0; i < CELL_COUNT; i++)
        {
            for (int j = 0; j < CELL_COUNT; j++)
            {
                if (tab[i,j] != null)
                {
                    tab[i, j].GetComponent<Button>().enabled = enable;
                }
            }
        }
    }

    //Create list of blocks to destroy
    private void CheckNeighbours(int x, int y, int colIndex, List<GameObject> toDestroy)
    {
        if (CheckBlock((x - 1), y, colIndex) && !toDestroy.Contains(tab[x - 1, y]))
        {
            toDestroy.Add(tab[(x - 1), y]);
            CheckNeighbours((x - 1),  y, colIndex, toDestroy);
        }
        if (CheckBlock((x + 1), y, colIndex) && !toDestroy.Contains(tab[x + 1, y]))
        {
            toDestroy.Add(tab[(x + 1), y]);
            CheckNeighbours((x + 1), y, colIndex, toDestroy);
        }
        if (CheckBlock(x, (y - 1), colIndex) && !toDestroy.Contains(tab[x, (y - 1)]))
        {
            toDestroy.Add(tab[x, (y - 1)]);
            CheckNeighbours(x, (y - 1), colIndex, toDestroy);
        }
        if (CheckBlock(x, (y + 1), colIndex) && !toDestroy.Contains(tab[x, (y + 1)]))
        {
            toDestroy.Add(tab[x, (y + 1)]);
            CheckNeighbours(x, (y + 1), colIndex, toDestroy);
        }
    }

    private bool CheckBlock(int x, int y, int colIndex)
    {
        if (x >= 0 && x < CELL_COUNT && y >= 0 && y < CELL_COUNT)
        {
            if (tab[x, y] != null)
            {
                if (tab[x, y].GetComponent<ButtonScript>().ColorIndex == colIndex)
                {
                    return true;
                }
            }
        }
        return false;
    }

    //Move row
    private void BlocksFall()
    {
        for (int x = 0; x < CELL_COUNT; x++)
        {
            for (int y = (CELL_COUNT - 1); y > 0 ; y--)
            {
                while (tab[x,y] == null && CanFall(x, y))
                {
                    Fall(x, y);
                }
            }
        }
    }

    private bool CanFall(int x, int y)
    {
        for (int i = 0; i < y; i++)
        {
            if (tab[x, i] != null)
                return true;
        }
        return false;
    }

    private void Fall(int x, int y)
    {
        for (int i = y; i > 0; i--)
        {
            tab[x, i] = tab[x, i - 1];
        }
        tab[x, 0] = null;               
    }

    //Move column
    private void ColumnMove()
    {
        for (int _x = 0; _x < CELL_COUNT - 1; _x++)
        {
            while (EmptyColumn(_x) && CanMove(_x))
            {
                MoveColumn(_x);
            }
        }
    }

    private bool EmptyColumn(int x)
    {
        if (tab[x, (CELL_COUNT - 1)] != null)
            return false;
        return true;
    }

    private bool CanMove(int x)
    {
        for (int i = x + 1; i < CELL_COUNT; i++)
        {
            if (!EmptyColumn(i))
                return true;
        }
        return false;
    }

    private void MoveColumn(int _x)
    {
        for (int x = _x; x < CELL_COUNT - 1; x++)
        {
            for (int y = 0; y < CELL_COUNT; y++)
            {
                tab[x, y] = tab[x + 1, y];
            }
        }
        for (int i = 0; i < CELL_COUNT; i++)
        {
            tab[CELL_COUNT - 1, i] = null;
        }
    }
    */
    //Buttons action
    public void Execute(GameObject button)
    {
        int x = button.GetComponent<ButtonScript>().X;
        int y = button.GetComponent<ButtonScript>().Y;
        int colIndx = button.GetComponent<ButtonScript>().ColorIndex;
        StartCoroutine(UpdateCoroutine(x, y, colIndx));
    }

    public IEnumerator UpdateCoroutine(int x, int y, int colIndex)
    {
        //Block Grid
        grid.BlockGrid(false, gameView.GetGameGrid(), CELL_COUNT);
        //List blocks to destroy
        List<GameObject> toDestroy = new List<GameObject>();
        toDestroy.Add(gameView.GetGameGrid()[x, y]);
        grid.CheckNeighbours(x, y, colIndex, toDestroy, gameView.GetGameGrid(), CELL_COUNT);
        //Update power points and moves
        moves--;
        AddPowerPoints(colIndex, toDestroy.Count, currentPlayer);
        if (currentPlayer != 1)
        {
            gameView.UpdatePowerPointsView(elementsPowerPoints2, moves);
        }
        else
        {
            gameView.UpdatePowerPointsView(elementsPowerPoints, moves);
        }
        //Destroy blocks
        foreach (GameObject gO in toDestroy)
        {
            gO.GetComponent<ButtonScript>().DestroyBut();
        }
        //Wait until destroy animation ends
        yield return new WaitForSeconds(0.8f);
        //Move blocks in table
        grid.BlocksFall(gameView.GetGameGrid(), CELL_COUNT);
        grid.ColumnMove(gameView.GetGameGrid(), CELL_COUNT);
        //Move blocks in view
        for (int i = 0; i < CELL_COUNT; i++)
        {
            for (int j = 0; j < CELL_COUNT; j++)
            {
                if (gameView.GetGameGrid()[i, j] != null)
                {
                    int _x = i;
                    int _y = j;
                    gameView.GetGameGrid()[i, j].GetComponent<ButtonScript>().SetIndexes(_x, _y);
                    gameView.GetGameGrid()[i, j].GetComponent<ButtonScript>().Move(_x, _y);
                }
            }
        }
        //Wait until blocks stop moving
        yield return new WaitForSeconds(0.7f);
        if (moves > 0)
        {
            //Unlock grid
            grid.BlockGrid(true, gameView.GetGameGrid(), CELL_COUNT);
        }
        else
        {
            Destroy(gameView.GetGamePanel());
            UpdatePowerView();
        }              
    }

    public IEnumerator EnemyCoroutine()
    {
        yield return new WaitForSeconds(1);

        int x = 0;
        int y = 0;
        int colIndex = 0;

        while (moves > 0)
        {
            int[] best = AISeekBestMove();
            x = best[0];
            y = best[1];
            colIndex = best[2];
            //List blocks to destroy
            List<GameObject> toDestroy = new List<GameObject>();
            toDestroy.Add(gameView.GetGameGrid()[x, y]);
            grid.CheckNeighbours(x, y, colIndex, toDestroy, gameView.GetGameGrid(), CELL_COUNT);
            //Update power points and moves
            moves--;
            AddPowerPoints(colIndex, toDestroy.Count, currentPlayer);
            gameView.UpdatePowerPointsView(elementsPowerPoints2, moves);

            //Destroy blocks
            foreach (GameObject gO in toDestroy)
            {
                gO.GetComponent<ButtonScript>().DestroyBut();
            }
            //Wait until destroy animation ends
            yield return new WaitForSeconds(0.8f);
            //Move blocks in table
            grid.BlocksFall(gameView.GetGameGrid(), CELL_COUNT);
            grid.ColumnMove(gameView.GetGameGrid(), CELL_COUNT);
            //Move blocks in view
            for (int i = 0; i < CELL_COUNT; i++)
            {
                for (int j = 0; j < CELL_COUNT; j++)
                {
                    if (gameView.GetGameGrid()[i, j] != null)
                    {
                        int _x = i;
                        int _y = j;
                        gameView.GetGameGrid()[i, j].GetComponent<ButtonScript>().SetIndexes(_x, _y);
                        gameView.GetGameGrid()[i, j].GetComponent<ButtonScript>().Move(_x, _y);
                    }
                }
            }
            //Wait until blocks stop moving
            yield return new WaitForSeconds(1);
        }
        Destroy(gameView.GetGamePanel());
        UpdatePowerView();

        bool end = false;
        int[] xY = new int[2];

        while (!end && !player.GetComponent<PlayerScript>().IsDead)
        {
            end = AIPowerChoice(xY);
            if (!end)
            {
                Attack(xY[0], xY[1]);
                yield return new WaitForSeconds(3);
            }
        }
        if (!player.GetComponent<PlayerScript>().IsDead)
        {
            EnemyMove();
        }        
    }

    //AI
    private int[] AISeekBestMove()
    {
        int[] coordinates = new int[3];
        int _x = 0;
        int _y = 0;
        int _index = 0;
        int max = 0;

        for (int x = 0; x < CELL_COUNT; x++)
        {
            for (int y = 0; y < CELL_COUNT; y++)
            {
                if (gameView.GetGameGrid()[x, y] != null)
                {
                    List<GameObject> list = new List<GameObject>();
                    list.Add(gameView.GetGameGrid()[x, y]);
                    grid.CheckNeighbours(x, y,
                        gameView.GetGameGrid()[x, y].GetComponent<ButtonScript>().ColorIndex,
                        list, gameView.GetGameGrid(), CELL_COUNT);

                    if (list.Count > max)
                    {
                        _x = x;
                        _y = y;
                        _index = gameView.GetGameGrid()[x, y].GetComponent<ButtonScript>().ColorIndex;
                        max = list.Count;
                    }
                }
            }
        }

        coordinates[0] = _x;
        coordinates[1] = _y;
        coordinates[2] = _index;

        return coordinates;
    }

    private bool AIPowerChoice(int[] _xY)
    {

        if (elementsPowerPoints2[0] < BASE_POWER_COST && elementsPowerPoints2[1] < BASE_POWER_COST &&
            elementsPowerPoints2[2] < BASE_POWER_COST && elementsPowerPoints2[3] < BASE_POWER_COST &&
            elementsPowerPoints2[4] < BASE_POWER_COST)
            return true;

        bool cankill = false;
        bool steal = false;
        int sum = 0;
        for (int i = 0; i < COLORS_COUNT; i++)
        {
            sum += Mathf.Max((((elementsPowerPoints2[i] / BASE_POWER_COST) * BASE_POWER_DAMAGE)
                - player.GetComponent<PlayerScript>().ElementalShield[i]), 0);
        }

        if (sum >= player.GetComponent<PlayerScript>().CurrentHp)
        {
            cankill = true;
        }

        sum = 0;
        for (int i = 0; i < (COLORS_COUNT - 1); i++)
        {
            sum += elementsPowerPoints[i];
        }

        if (sum >= 12)
        {
            steal = true;
        }
        //Fire
        if (elementsPowerPoints2[0] >= 3 * BASE_POWER_COST)
        {
            if(cankill)
            {
                _xY[0] = 0;
                _xY[1] = 0;
                return false;
            }
            else if (player2.GetComponent<PlayerScript>().CurrentHp <
                player2.GetComponent<PlayerScript>().MaxHp - player2.GetComponent<PlayerScript>().CurrentHp
                && (player.GetComponent<PlayerScript>().ElementalShield[0] +
                player.GetComponent<PlayerScript>().CurrentHp > (3 * BASE_POWER_DAMAGE)))
            {
                _xY[0] = 2;
                _xY[1] = 0;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 0;
                return false;
            }
        }
        else if(elementsPowerPoints2[0] >= 2 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 0;
                return false;
            }
            else if (player2.GetComponent<PlayerScript>().ElementalShield[0] == 0
                && Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 1;
                _xY[1] = 0;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 0;
                return false;
            }
        }
        else if (elementsPowerPoints2[0] >= BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 0;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 0;
                return false;
            }
        }
        //Earth
        if (elementsPowerPoints2[1] >= 3 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 1;
                return false;
            }
            else
            {
                _xY[0] = 2;
                _xY[1] = 1;
                return false;
            }
        }
        else if (elementsPowerPoints2[1] >= 2 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 1;
                return false;
            }
            else if (player2.GetComponent<PlayerScript>().ElementalShield[1] == 0
                && Random.Range(0.0f, 1.0f) > AI_PROBABILITY)
            {
                _xY[0] = 1;
                _xY[1] = 1;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 1;
                return false;
            }
        }
        else if (elementsPowerPoints2[1] >= BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 1;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 1;
                return false;
            }
        }
        //Water
        if (elementsPowerPoints2[2] >= 3 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 2;
                return false;
            }
            else
            {
                _xY[0] = 2;
                _xY[1] = 2;
                return false;
            }
        }
        else if (elementsPowerPoints2[2] >= 2 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 2;
                return false;
            }
            else if (player2.GetComponent<PlayerScript>().ElementalShield[2] == 0
                && Random.Range(0.0f, 1.0f) > AI_PROBABILITY)
            {
                _xY[0] = 1;
                _xY[1] = 2;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 2;
                return false;
            }
        }
        else if (elementsPowerPoints2[2] >= BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 2;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 2;
                return false;
            }
        }
        //electro
        if (elementsPowerPoints2[3] >= 3 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 3;
                return false;
            }
            else if(steal)
            {
                _xY[0] = 2;
                _xY[1] = 3;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 3;
                return false;
            }
        }
        else if (elementsPowerPoints2[3] >= 2 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 3;
                return false;
            }
            else if (player2.GetComponent<PlayerScript>().ElementalShield[3] == 0
                && Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 1;
                _xY[1] = 3;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 3;
                return false;
            }
        }
        else if (elementsPowerPoints2[3] >= BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 3;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 3;
                return false;
            }
        }
        //wind
        if (elementsPowerPoints2[4] >= 3 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 4;
                return false;
            }
            else if (player.GetComponent<PlayerScript>().ElementalShield[4] <= BASE_POWER_DAMAGE)
            {
                _xY[0] = 2;
                _xY[1] = 4;
                return false;
            }
            else
            {
                _xY[0] = 0;
                _xY[1] = 3;
                return false;
            }
        }
        else if (elementsPowerPoints2[4] >= 2 * BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 4;
                return false;
            }
            else if (player2.GetComponent<PlayerScript>().ElementalShield[3] == 0
                && Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 1;
                _xY[1] = 4;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 4;
                return false;
            }
        }
        else if (elementsPowerPoints2[4] >= BASE_POWER_COST)
        {
            if (cankill)
            {
                _xY[0] = 0;
                _xY[1] = 4;
                return false;
            }
            else if (Random.Range(0.0f, 1.0f) < AI_PROBABILITY)
            {
                _xY[0] = 0;
                _xY[1] = 4;
                return false;
            }
        }

        return true;
    }
}
