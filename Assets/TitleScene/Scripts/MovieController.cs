using UnityEngine;
using System.Collections;

public class MovieController : MonoBehaviour {
	public MovieTexture movie;
	// Use this for initialization
	void Start () {
		//guiTexture.renderer.material.mainTexture.Play
		//renderer.material.mainTexture.Play();
		guiTexture.texture = movie;
		movie.loop = true;
		movie.Play();

		//movie = (MovieTexture)guiTexture.texture;
		//movie.Play();
	}
	
	// Update is called once per frame
	void Update () {
		//if (!movie.isPlaying)
		//	movie.Play();
	}
}
