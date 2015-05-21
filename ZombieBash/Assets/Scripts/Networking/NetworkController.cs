using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkController : MonoBehaviour {

	public GameObject PlayerPrefab;


	 public GroundCoverGenerator map;
	/* NETWORKING*/
	public Canvas ServerCanvas;
	public Canvas QuitRoomCanvas;
	public Button StartRoomButton;
	public Button JoinRoomButton;
	public Button NextRoomButton;
	public Button QuitRoomButton;
	public Button PreviousRoomButton;
	public Text NetworkStatusText;
	public Text JoinRoomText;
	public Text LobbyCountText;

	public CameraScript PlayerCamera;

	public string GameVersion = "ZombieBash";
	public string RoomName = "DEFAULT_ROOM_NAME";

	private RoomInfo[] _availableRooms;
	private int _currentlySelectedRoom;
	private bool _readyToPlay;

	private GameObject _player;
	public UiHubScript playerui;
	// Use this for initialization
	void Start () {

		_currentlySelectedRoom = 0;
		
		// disable our server buttons
		QuitRoomButton.gameObject.SetActive (false);
		StartRoomButton.gameObject.SetActive(false);
		JoinRoomButton.gameObject.SetActive(false);
		NextRoomButton.gameObject.SetActive(false);
		PreviousRoomButton.gameObject.SetActive(false);
		
		PhotonNetwork.ConnectUsingSettings(GameVersion);
		StartRoomButton.onClick.AddListener(OnStartButtonPressed);
		JoinRoomButton.onClick.AddListener(OnJoinButtonPressed);
		PreviousRoomButton.onClick.AddListener(OnPreviousRoomButtonPressed);
		NextRoomButton.onClick.AddListener(OnNextRoomButtonPressed);
		QuitRoomCanvas.gameObject.SetActive (false);
	}

	private void OnStartButtonPressed()
	{
		CreateRoom();
	}
	
	private void OnJoinButtonPressed()
	{
		if (_currentlySelectedRoom < _availableRooms.Length && -1 < _currentlySelectedRoom)
			PhotonNetwork.JoinRoom(_availableRooms[_currentlySelectedRoom].name);
	}
	
	private void OnNextRoomButtonPressed()
	{
		_currentlySelectedRoom ++;
		if (_currentlySelectedRoom > _availableRooms.Length - 1)
			_currentlySelectedRoom = _availableRooms.Length - 1;
		
		UpdateJoinRoomButtonText();
	}
	
	private void OnPreviousRoomButtonPressed()
	{
		_currentlySelectedRoom--;
		if (_currentlySelectedRoom < 0)
			_currentlySelectedRoom = 0;
		
		UpdateJoinRoomButtonText();
	}
	
	private void UpdateJoinRoomButtonText()
	{
		if (_currentlySelectedRoom < _availableRooms.Length && -1 < _currentlySelectedRoom)
			JoinRoomText.text = _availableRooms[_currentlySelectedRoom].name;
		else
			JoinRoomText.text = "no rooms available";
	}
	/* SERVER CREATION */
	private void CreateRoom()
	{
		Debug.Log("starting server");
		
		PhotonNetwork.CreateRoom(RoomName + Guid.NewGuid().ToString("N"),
		                         new RoomOptions
		                         {
			cleanupCacheOnLeave = true,
			isOpen = true,
			isVisible = true
		},
		new TypedLobby
		{
			Name = "",
			Type = LobbyType.Default
		});
	}
	
	/* SERVER JOINING */
	
	void OnReceivedRoomListUpdate()
	{
		_availableRooms = PhotonNetwork.GetRoomList();
		
		StartRoomButton.gameObject.SetActive(true);
		JoinRoomButton.gameObject.SetActive(true);
		
		_currentlySelectedRoom = 0;
		
		UpdateJoinRoomButtonText();
		
		// we can now leave if there aren't any available rooms
		if (_availableRooms.Length < 1) return;
		
		NextRoomButton.gameObject.SetActive(true);
		PreviousRoomButton.gameObject.SetActive(true);
	}
	public int GetRandomSeed(){
	

		return UnityEngine.Random.Range (0,10);
	
	}

	void OnQuitRoom(){
		QuitRoomButton.gameObject.SetActive (false);
		PhotonNetwork.LeaveRoom ();
		ServerCanvas.gameObject.SetActive(true);
		map.TearDownMap ();
	}
	void OnJoinedRoom()
	{ 
	
		var seed = GetRandomSeed (); 
           map.goMapGenerator (seed);
		Debug.Log("connected to room : " + PhotonNetwork.room.name);
		ServerCanvas.gameObject.SetActive(false);
		QuitRoomCanvas.gameObject.SetActive (true);
		QuitRoomButton.gameObject.SetActive (true);
		QuitRoomButton.onClick.AddListener(OnQuitRoom);


		_player = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3(0, 20, 0), Quaternion.identity, 0);
		PlayerCamera.Target = _player;

	
		//_player.GetComponent<PlayerController>().Initialize(AvailablePlayerColors[UnityEngine.Random.Range(0, AvailablePlayerColors.Count)]);
		//MainCamera.enabled = false;
		/*

		if (PhotonNetwork.isMasterClient)
		{
			Debug.Log("i am master client");
			PhotonNetwork.Instantiate(RandomPrefab.name, new Vector3(UnityEngine.Random.Range(-5, 5), 0,
			                                                           UnityEngine.Random.Range(-5, 5)),
			                          Quaternion.identity, 0);
		}*/
	}
	
	/* GAME STUFF */
	public void Update()
	{
		NetworkStatusText.text = PhotonNetwork.connectionStateDetailed.ToString();
		LobbyCountText.text = PhotonNetwork.countOfPlayersOnMaster + " people in lobby for " + GameVersion + "!";
		
		if (Input.GetButtonDown("Cancel"))
			Application.Quit();
	}

}
