using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemaster : MonoBehaviour {
	public GameObject Pipe;
	public GameObject Player;
	public string Game_State = "Title";

	public GameObject Game_Start;
	public GameObject Game_End;
	public UnityEngine.UI.Text Score;
	public UnityEngine.UI.Text End_Highscore;
	public UnityEngine.UI.Text End_Score;

	// Use this for initialization
	void Start () {
		Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) && Game_State == "Title") {
			Game_State = "Game";
			Game_Start.SetActive(false);
			Player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
			StartCoroutine(Pipe_Generation(10));
			StartCoroutine(Fade(Score, 2));
		}
			
		if (Mathf.Floor((Player.transform.position.x - 10) / 3.5f + 1) < 0) {
			Score.text = "0";
		}else {
			Score.text = Mathf.Floor((Player.transform.position.x - 10) / 3.5f + 1).ToString();
		}

		if (Game_State == "End") {
			Destroy(Player);
			Game_State = "Death";
			Game_End.SetActive(true);
			int Scores = int.Parse(Score.text);
			if (Scores > PlayerPrefs.GetInt("Highscore")) {
				PlayerPrefs.SetInt("Highscore", Scores);
			}
			End_Highscore.text = " Highscore: " + PlayerPrefs.GetInt("Highscore");
			End_Score.text = " Score: " + Scores;
		}
	}

	IEnumerator Pipe_Generation (float x) {
		if (Game_State == "Game") {
			int Gap = Random.Range (3, -3);
			for (float y = 6; y >= -6; y--) {
				if (y != Gap) {
					GameObject Creation = Instantiate(Pipe,  new Vector2(x, y), Quaternion.identity);
				}else {
					y -= 1f;
				}
			}
			while (Player.transform.position.x * 1f <= x - 8f){
				yield return new WaitForSecondsRealtime(.0001f);
			}
			StartCoroutine(Pipe_Generation(x + 3.5f));
		}
	}

	IEnumerator Fade (UnityEngine.UI.Text Text, float Type) { //If type is positive then fade in. Vice versa is vice versa.
		yield return new WaitForSecondsRealtime(3);

		for (int i = 255; i > 0; i += Mathf.RoundToInt(Type)) {
			Text.color = new Color(Text.color.r, Text.color.b, Text.color.b, Text.color.a + Type/255);
			yield return new WaitForSecondsRealtime(.01f);
		}
		Text.color = new Color(Text.color.r, Text.color.b, Text.color.b, 1);
		yield return null;
	}

	public void Restart () {
		Application.LoadLevel("Main");
	}
}
