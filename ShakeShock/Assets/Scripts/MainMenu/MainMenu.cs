// --------------------------------------------------------------
// Shake Shock - MainMenu                               3/12/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainMenu : MonoBehaviourPunCallbacks
{
    #region Inspector Fields

    [Header("Buttons")]
    [SerializeField]
    private GameObject connectWalletButtonGameObject;
    [SerializeField]
    private GameObject playButtonGameObject;
    [SerializeField]
    private GameObject levelUpButtonGameObject;
    [SerializeField]
    private GameObject armorDropDownGameObject;
    [SerializeField]
    private GameObject throwableDropDownGameObject;
    [SerializeField]
    private GameObject statsButtonGameObject;
    [SerializeField]
    private GameObject notEnoughTokensButtonGameObject;
    [SerializeField]
    private GameObject createRoomButtonGameObject;
    [SerializeField]
    private GameObject createRoomWagerPanelGameObject;
    [SerializeField]
    private GameObject createRoomWagerRoomIdGameObject;
    [SerializeField]
    private GameObject joinRoomButtonGameObject;
    [SerializeField]
    private GameObject joinRoomInputPanelGameObject;
    [SerializeField]
    private GameObject roomBackButton;

    [Header("DropDown")]
    [SerializeField]
    private Dropdown armorDropdown;
    [SerializeField]
    private Dropdown throwableDropdown;

    [Header("Other")]
    [SerializeField]
    private GameObject titleTextGameObject;
    [SerializeField]
    private GameObject loadingGameObject;
    [SerializeField]
    private GameObject notEnoughTokensPanelGameObject;

    [Header("Stats Panel")]
    [SerializeField]
    private GameObject statsPanelGameObject;
    [SerializeField]
    private Text statsHealthText;
    [SerializeField]
    private Text statsArmorText;
    [SerializeField]
    private Text statsJumpText;
    [SerializeField]
    private Text statsSpeedText;
    [SerializeField]
    private Text statsTokenText;
    [SerializeField]
    private Text statsDashText;

    [Header("Rooms")]
    [SerializeField]
    private InputField etherWagerInputField;
    [SerializeField]
    private InputField roomJoinInputField;
    [SerializeField]
    private Text createRoomText;

    [Header("Debug")]
    [SerializeField]
    private bool debugMode;

    #endregion

    #region Run-time Fields

    private bool walletConnected;
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    private bool addedPlayer;

    #endregion

    #region Monobehaviors

    // Start is called before the first frame update
    void Start()
    {
        ShowHideMainScreen(true);
        ShowHideGameSetup(false);
        loadingGameObject.SetActive(false);
        statsPanelGameObject.SetActive(false);
        notEnoughTokensPanelGameObject.SetActive(false);
        connectWalletButtonGameObject.SetActive(false);
    }

    #endregion

    #region Public Methods

    // The connect wallet button is pressed
    public void OnClickConnectWallet()
    {
        DisableConnectWalletButton();

        // Connect wallet stuff here
        // I have a walletConnected bool to make sure the wallet is conencted before continuing. You can use this or something else
    }

    // The play button is pressed
    public void OnClickPlayButton()
    {
        // Check to make sure the wallet is connected
        if (walletConnected || debugMode)
        {
            GenerateRandomRoomString();
            ShowHideGameSetup(true);
            ShowHideMainScreen(false);
        }
    }

    // The Level up button is pressed
    public void OnClickLevelUPButton()
    {
        // Some contract stuff and then you'll need to update the player metadata

        // I'll add a panel for when we don't have enough tokens

        // contract stuff here

        // Update metadata
        PlayerMetadata player = GameManager.main.GetPlayerMetadata("wallet");
        PlayerMetadata option2 = GameManager.main.GetPlayerMetadata(0); // Get the index

        GameManager.main.DeletePlayerMetadata(player);
        // Update the value here like this
        // player.SetSpeedMultiplier(new value)
        
        // Add it back
        GameManager.main.AddNewPlayerMetadata(player);
    }

    // The stats button is pressed
    public void OnClickStatsButton()
    {
        // Set the player metadata stuff here
        PlayerMetadata player = GameManager.main.GetPlayerMetadata("wallet address");

        statsHealthText.text = player.GetHealth().ToString();
        statsArmorText.text = player.GetArmor().ToString();
        statsDashText.text = player.GetDashMultiplier().ToString();
        statsJumpText.text = player.GetJumpMultiplier().ToString();
        statsSpeedText.text = player.GetSpeedMultiplier().ToString();
        statsTokenText.text = player.GetTokens().ToString();

        statsPanelGameObject.SetActive(true);
        ShowHideMainScreen(false);
    }

    // Close the stats panel
    public void OnClickCloseStatsButton()
    {
        statsPanelGameObject.SetActive(false);
        ShowHideMainScreen(true);
    }

    public void OnClickNotEnoughTokensClose()
    {
        notEnoughTokensPanelGameObject.SetActive(false);
    }

    public void OnClickCreateRoomButton()
    {
        // Interact with escrow contract here:

        // Get the ether wager like this:
        string ether = etherWagerInputField.text;

        // Network stuff
        PhotonNetwork.CreateRoom(createRoomText.text);
        CreateNewPlayerMetadata();

        StartCoroutine(WaitToJoinRoom());
    }

    public void OnClickCloseSetupButton()
    {
        ShowHideMainScreen(true);
        ShowHideGameSetup(false);
    }

    // We're joining with another player
    public void OnClickJoinRoomButton()
    {
        // Need to interact with the escrow contract here as well to match the other user

        PhotonNetwork.JoinRoom(roomJoinInputField.text);
        CreateNewPlayerMetadata();
        StartCoroutine(WaitToJoinRoom());
    }

    #endregion

    #region Private Methods

    // Just shows and hides the main menu
    private void ShowHideMainScreen(bool status)
    {
        playButtonGameObject.SetActive(status);
        levelUpButtonGameObject.SetActive(status);
        statsButtonGameObject.SetActive(status);
        throwableDropDownGameObject.SetActive(status);
        armorDropDownGameObject.SetActive(status);
    }

    // Disables the connect wallet button once we've pressed it and shows loading screen
    // Basically waiting for player to connect wallet before we show the main menu
    private void DisableConnectWalletButton()
    {
        connectWalletButtonGameObject.SetActive(false);
        loadingGameObject.SetActive(true);
    }

    // Call this once the wallet has connected, may need some modification
    private void OnWalletConnected()
    {
        walletConnected = true;
        loadingGameObject.SetActive(false);
        ShowHideMainScreen(true);
        GameManager.main.SetWalletAddress("Give the game manager the wallet address pls");

        // Need to set what throwable the player can select
        List<string> throwableList = new List<string> { "Basic Apple", "Explosive Apple", "Electric Apple" };
        throwableDropdown.options.Clear();
        foreach (string option in throwableList)
        {
            // Probably need to check if the player has this option available to select?
            throwableDropdown.options.Add(new Dropdown.OptionData(option));
        }

        // Need to set what armor the player can select
        List<string> armorList = new List<string> { "No Armor", "Basic Armor", "Advanced Armor" };
        armorDropdown.options.Clear();
        foreach (string option in armorList)
        {
            // Probably need to check if the player has this option available to select?
            armorDropdown.options.Add(new Dropdown.OptionData(option));
        }
    }

    // Example of adding a player's metadata to the gamemanager to handle when generating the game on play
    private void CreateNewPlayerMetadata()
    {
        // Create the metadata class
        PlayerMetadata player = new PlayerMetadata();

        // Add some stuff
        player.SetSpeedMuliplier(1);
        player.SetDashMultiplier(1);
        player.SetJumpMultiplier(1);
        player.SetShieldTime(3);
        player.SetHealth(100);
        player.SetTokens(0);
        player.SetWallet("wallet address");
        // Sets the drop down selecion
        player.SetArmor(armorDropdown.value);
        // Also sets the drop down selection
        Throwable.ThrowableType type = (Throwable.ThrowableType)throwableDropdown.value;
        player.SetThrowableType(type);

        // Add the image and finish
        StartCoroutine(SetPlayerImage("This will be the IPFS URL????", player));
    }

    // Call this when the user doesn't have enough tokens to upgrade
    private void NotEnoughTokens()
    {
        notEnoughTokensPanelGameObject.SetActive(true);
    }

    private void ShowHideGameSetup(bool status)
    {
        createRoomButtonGameObject.SetActive(status);
        createRoomWagerPanelGameObject.SetActive(status);
        createRoomWagerRoomIdGameObject.SetActive(status);
        joinRoomButtonGameObject.SetActive(status);
        joinRoomInputPanelGameObject.SetActive(status);
        roomBackButton.SetActive(status);
    }

    private void GenerateRandomRoomString()
    {
        int charAmount = 10;
        string myString = "";
        for (int i = 0; i < charAmount; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }

        createRoomText.text = myString;
    }

    #endregion


    #region Coroutines

    private IEnumerator SetPlayerImage(string url, PlayerMetadata metaData)
    {
        // Web request to download the texture at the given url
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            GameManager.main.AddNewPlayerMetadata(metaData);
            addedPlayer = true;
        }
        else
        {
            // Convert to Texture 2d
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            // Make a sprite
            Sprite playerSprite = Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);

            // Add the texture retreieved to the metadata and submit to the game manager
            metaData.SetPlayerSprite(playerSprite);
            GameManager.main.AddNewPlayerMetadata(metaData);
            addedPlayer = true;
        }
    }

    private IEnumerator WaitToJoinRoom()
    {
        while (addedPlayer == false)
        {
            yield return new WaitForSeconds(1);
        }

        SceneManager.LoadScene(2);
    }

    #endregion
}
