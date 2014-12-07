using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Screen : MonoBehaviour {

	public GameObject pixelPrefab;

	private List<List<GameObject>> Pixels;

	// Use this for initialization
	void Start () {
		Pixels = new List<List<GameObject>>();
		for (int i = 0; i < 90; i += 3) {
			List<GameObject> pixelRow = new List<GameObject>();
			for (int j = 0; j < 160; j+= 3) {
				Vector3 spawnPosition = new Vector3(i - 490,j - 0,1);
				//GameObject newPixel = (GameObject) Instantiate(pixelPrefab, spawnPosition, pixelPrefab.transform.rotation);
				//newPixel.transform.parent = transform;
				//pixelRow.Add( newPixel);
			}
			//Pixels.Add(pixelRow);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
