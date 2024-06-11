using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using TMPro;
using UnityEngine;

public class GPGSMgr : MonoBehaviour
{
    public TextMeshProUGUI log;

    private string savedGameFilename = "save.bin";
    private string saveData = "세이브 로드 확인";

    void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(OnAuthentication);
    }

    void OnAuthentication(SignInStatus result)
    {
        if (result == SignInStatus.Success)
        {
            log.text = "Signed in successfully.";
            // Signed in successfully, we can now proceed with saving or loading
        }
        else
        {
            log.text = "Failed to sign in.";
        }
    }

    public void ShowSelectUI()
    {
        uint maxNumToDisplay = 5;
        bool allowCreateNew = false;
        bool allowDelete = true;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Select saved game",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            OnSavedGameSelected);
    }

    public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
            OpenSavedGame(game.Filename, OnSavedGameOpenedForLoad);


        }
        else
        {
            // handle cancel or error
        }
    }

    public void SaveGame()
    {
        OpenSavedGame(savedGameFilename, OnSavedGameOpenedForSave);
    }

    void OnSavedGameOpenedForSave(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(saveData);
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
                .WithUpdatedDescription("Saved at " + DateTime.Now.ToString())
                .Build();

            PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, data, OnSavedGameCommit);
        }
        else
        {
            log.text = "Failed to open saved game.";
        }
    }

    void OnSavedGameCommit(SavedGameRequestStatus commitStatus, ISavedGameMetadata updatedGame)
    {
        if (commitStatus == SavedGameRequestStatus.Success)
        {
            log.text = "Game saved successfully.";
        }
        else
        {
            log.text = "Failed to save game.";
        }
    }

    public void LoadGame()
    {
        OpenSavedGame(savedGameFilename, OnSavedGameOpenedForLoad);
    }

    void OnSavedGameOpenedForLoad(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, OnSavedGameDataRead);
        }
        else
        {
            log.text = "Failed to open saved game.";
        }
    }

    void OnSavedGameDataRead(SavedGameRequestStatus readStatus, byte[] data)
    {
        if (readStatus == SavedGameRequestStatus.Success)
        {
            string loadedData = System.Text.Encoding.UTF8.GetString(data);
            log.text = "Game loaded successfully: " + loadedData;
        }
        else
        {
            log.text = "Failed to load game.";
        }
    }

    private void OpenSavedGame(string filename, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
    {
        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
            filename,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            callback);
    }
}
