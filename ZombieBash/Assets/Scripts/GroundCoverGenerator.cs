using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundCoverGenerator : MonoBehaviour {
	public List<GameObject>GroundCoverprefabs;
	public List<GameObject>WallPrefabs;
	public List<GameObject>RiverPrefabs;
	public GameObject tree_prefab;
	//public  float AccesstoGround;
	private int AreaToCover=30;
	//private int MaxGroundCover=5;
//	private int MinGroundCover=0;
	public float GridSize=10;
	public float inner_radius=5;
	private float _centreAdjustments;
	// Use this for initialization
	public List<GameObject>AllGameObjects;

	public void goMapGenerator(int seed){

		Random.seed = seed;
		_centreAdjustments = AreaToCover * GridSize / 2;
		var blueprint=new string[AreaToCover,AreaToCover];
		
		//fill up blueprint with walls
		for (var x=0; x<AreaToCover;x++)
			for(var y=0; y<AreaToCover; y++)
				blueprint[x,y]="wall";
		/*

		for (var x=0; x<5;x++)
			for(var y=0; y<5; y++)
				blueprint[x,y]="dtree";
		
		for (var x=5; x<10;x++)
			for(var y=5; y<10; y++)
				blueprint[x,y]="dtree";
		
		for (var x=10; x<15;x++)
			for(var y=10; y<15; y++)
				blueprint[x,y]="mtree";
		
		
		for (var x=15; x<20;x++)
			for(var y=15; y<20; y++)
				blueprint[x,y]="mtree";
		
		for (var x=20; x<25;x++)
			for(var y=20; y<25; y++)
				blueprint[x,y]="dtree";
				*/
		
		for (var x=0; x<30;x++)
			for(var y=0; y<5; y++)
				blueprint[x,y]="dtree";
		
		for (var x=0; x<30;x++)
			for(var y=5; y<10; y++)
				blueprint[x,y]="dtree";
		
		for (var x=0; x<30;x++)
			for(var y=10; y<15; y++)
				blueprint[x,y]="mtree";
		
		
		for (var x=0; x<30;x++)
			for(var y=15; y<20; y++)
				blueprint[x,y]="mtree";
		
		for (var x=0; x<30;x++)
			for(var y=20; y<25; y++)
				blueprint[x,y]="ltree";
		
		for (var x=0; x<30;x++)
			for(var y=25; y<30; y++)
				blueprint[x,y]="ltree";
		
		//River
		for (var x=0; x<5;x++)
			for(var y=0; y<5; y++)
				blueprint[x,y]="river";
		
		for (var x=5; x<10;x++)
			for(var y=5; y<10; y++)
				blueprint[x,y]="river";
		
		for (var x=10; x<15;x++)
			for(var y=10; y<15; y++)
				blueprint[x,y]="river";
		
		
		for (var x=15; x<20;x++)
			for(var y=15; y<20; y++)
				blueprint[x,y]="river";
		
		for (var x=20; x<25;x++)
			for(var y=20; y<25; y++)
				blueprint[x,y]="river";
		
		for (var x=25; x<30;x++)
			for(var y=25; y<30; y++)
				blueprint[x,y]="river";
		
		/*

		for (var x=AreaToCover/2; x<20;x++)
			for(var y=AreaToCover/2; y<20; y++)
				blueprint[x,y]="mediumtree";
*/
		/*	var GroundCover = Random.Range (MinGroundCover,MaxGroundCover);
		for(var i=0; i<GroundCover; i++){

			spawnGroundCover();*/
		
		//shennnanigans begin
		/*
		blueprint [AreaToCover/2,AreaToCover/2]="empty";
		blueprint [AreaToCover/2+1,AreaToCover/2]="empty";
		blueprint [AreaToCover/2,AreaToCover/2+1]="empty";
		blueprint [AreaToCover/2+1,AreaToCover/2+1]="empty";
		*/
		
		//instantiate onjects as per the blueprint
		for (var x=0; x<AreaToCover; x++)
			for (var y=0; y<AreaToCover; y++)
				FillBlueprint (seed,blueprint[x,y],x,y);

	
	}
	void Start () {



	}
	
	
	private void FillBlueprint(int seed,string type,int x ,int y){
		if (type == "wall") {
			var wall = Instantiate (WallPrefabs [Random.Range (0, WallPrefabs.Count)]) as GameObject;
			SetObjectToCorrectTransform(wall,x,y);
			
		}
		else if(type=="empty"){
			
		}
		else if (type == "dtree") {
			//var tree = Instantiate (tree_prefab) as GameObject;
//			var groundCoverTospawn=Random.Range(MinGroundCover,MaxGroundCover);
			for (var i=0; i<8;i++){
//				Debug.Log("GroundCover Loop");
				spawnGroundCover(seed,x,y);
			}
			//SetObjectToCorrectTransform(tree,x,y);
		}
		else if (type == "mtree") {
			//var tree = Instantiate (tree_prefab) as GameObject;
//			var groundCoverTospawn=Random.Range(MinGroundCover,MaxGroundCover);
			for (var i=0; i<4;i++){
				spawnGroundCover(seed,x,y);
			}
			//SetObjectToCorrectTransform(tree,x,y);
		}
		else if (type == "ltree") {
			//var tree = Instantiate (tree_prefab) as GameObject;
			//var groundCoverTospawn=Random.Range(MinGroundCover,MaxGroundCover);
			for (var i=0; i<2;i++){
				spawnGroundCover(seed,x,y);
			}
			//SetObjectToCorrectTransform(tree,x,y);
		}
		else if (type == "river") {
			//var tree = Instantiate (tree_prefab) as GameObject;
//		var wall = Instantiate (RiverPrefabs [Random.Range (0, RiverPrefabs.Count)]) as GameObject;
			//SetObjectToCorrectTransform(wall,x,y);
			//SetObjectToCorrectTransform(tree,x,y);
		}
		
	}
	private void spawnGroundCover(int seed,int x , int y){


		var groundCover = Instantiate (GroundCoverprefabs [Random.Range (0, GroundCoverprefabs.Count)]) as GameObject;
		AllGameObjects.Add (groundCover);
		SetObjectToCorrectTransform (groundCover,x,y);
		groundCover.transform.localPosition+=new Vector3(Random.Range(-GridSize / 2,GridSize / 2),0,Random.Range(-GridSize / 2,GridSize / 2));
		groundCover.transform.localScale *= 1 / (GridSize+5);
	}
	private void SetObjectToCorrectTransform(GameObject subject,int x,int y){
		subject.transform.localPosition = new Vector3 ((x * GridSize)+GridSize / 2-_centreAdjustments, 0, (y * GridSize/2)+GridSize / 2-_centreAdjustments);
		subject.transform.localScale*=GridSize;
		subject.transform.parent = transform;
		
		
	}
	public void TearDownMap(){
		/*
		for (int i=0; i<=AllGameObjects.Count; i++) {
						Destroy (AllGameObjects[i]);
				}
*/
		foreach(var worldObject in AllGameObjects)
			Destroy (worldObject);
	}
	
}