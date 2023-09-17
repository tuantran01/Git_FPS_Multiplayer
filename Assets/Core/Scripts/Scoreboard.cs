using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] Transform holder;
    [SerializeField] Transform redholder;
    [SerializeField] Transform blueholder;

    [SerializeField] GameObject scoreboardItemPrefab;
    [SerializeField] GameObject redscoreboardItemPrefab;
    [SerializeField] GameObject bluescoreboardItemPrefab;

    public Text scoreboardRoomID;
    public bool set;

    Dictionary<Player, ScoreboardListItem> scoreboardItems = new Dictionary<Player, ScoreboardListItem>();
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!set)
        {
            set = true;
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                AddScoreboardListing(player);
            }
        }
        scoreboardRoomID.text = PhotonNetwork.CurrentRoom.Name;
    }

    void AddScoreboardListing(Player player)
    {
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] == 0)
        {
            ScoreboardListItem Listing = Instantiate(scoreboardItemPrefab, holder).GetComponent<ScoreboardListItem>();
            Listing.Connect(player);
            scoreboardItems[player] = Listing;
        }
        else
        {
            if ((int)player.CustomProperties["TEAM"] == 0)
            {
                ScoreboardListItem Listing = Instantiate(redscoreboardItemPrefab, redholder).GetComponent<ScoreboardListItem>();
                Listing.Connect(player);
                scoreboardItems[player] = Listing;
            }

            else if ((int)player.CustomProperties["TEAM"] == 1)
            {
                ScoreboardListItem Listing = Instantiate(bluescoreboardItemPrefab, blueholder).GetComponent<ScoreboardListItem>();
                Listing.Connect(player);
                scoreboardItems[player] = Listing;
            }
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(delayy(newPlayer));
    }

    IEnumerator delayy(Player player)
    {
        yield return new WaitForSeconds(1);
        AddScoreboardListing(player);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }
}
