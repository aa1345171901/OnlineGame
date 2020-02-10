using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour {

    public Text userName;
    public Text totalCount;
    public Text winCount;
    public Button joinButton;

    private int id;

    private RoomListPanel roomListPanel;

	// Use this for initialization
	void Start () {

        if (joinButton != null)
        {
            joinButton.onClick.AddListener(OnJoinClick);
        }
	}

    /// <summary>
    /// 设置RoomItem的Text参数
    /// </summary>
    /// <param name="username"></param>
    /// <param name="totalCount"></param>
    /// <param name="winCount"></param>
    public void SetRoomInfo(int id,string username,int totalCount,int winCount,RoomListPanel panel)
    {
        SetRoomInfo(id, username, totalCount.ToString(), winCount.ToString(), panel);
    }

    public void SetRoomInfo(int id, string username, string totalCount, string winCount, RoomListPanel panel)
    {
        this.id = id;
        this.userName.text = username;
        this.totalCount.text = "总场数：\n" + totalCount.ToString();
        this.winCount.text = "胜利场数：\n" + winCount.ToString();
        this.roomListPanel = panel;
    }

    public void OnJoinClick()
    {
        roomListPanel.OnJoinClick(id);
    }
	
    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
