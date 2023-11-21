using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{   

	//Referencias en para las secciones del menu de pausa
	public GameObject menuObj;
	public GameObject mainObj;
	public GameObject optionsObj;
	public GameObject newLvlObj;

    //Referencia al componente de audio para controlar la musica del juego 
	public AudioSource _music;
	//Arrays de textos que almacenan referencias de texto para para mostrar las opciones de musica, graficos y invertir el mouse
	public Text[] musicTexts;
	public Text[] graphicsTexts;
	public Text[] invMouseTexts;
	
	
	int mode = 0; // variable para controlar que seccion del menu de pausa se muestra

	[HideInInspector]

	//Variables para controlar la pausa del juego
	public bool paused = false; 

	public bool music = true;
	public bool invertMouse = false;

	public static PauseMenu pauseMenu;

	void Awake()
	{
		//se ejecuta al iniciar el juego si la musica esta desactivada detiene la reproduccion de la musica
		pauseMenu = this;

		if (!music) _music.Stop();
	}


	//gestiona la activacion y la desactivacion del menu de pausa al pulsar la tecla ESC 
	void Update()
	{
		if (!World.currentWorld.worldInitialized) return;

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
			mode = 0;

			if (paused)
			{
				menuObj.SetActive(true);
				mainObj.SetActive(true);
			}
			else
			{
				menuObj.SetActive(false);
				mainObj.SetActive(false);
				optionsObj.SetActive(false);
				newLvlObj.SetActive(false);
			}
		}

		if(mode == 1) SetOptionsText();
	}

	void SetTexts(Text[] texts, string msg)
	{
		foreach(Text t in texts)
		{
			t.text = msg;
		}
	}


	//despausa el juego y oculta todos los elementos de la seccion del menu de pausa
	public void UnPauseGame()
	{
		paused = false;
		mode = 0;

		menuObj.SetActive(false);
		mainObj.SetActive(false);
		optionsObj.SetActive(false);
	}


	//Cambio el modo de graficos desde el menu 
	public void ChangeGraphicsMode()
	{
		World.currentWorld.ChangeGraphicsMode();
	}

	//Cambia el estado de la musica
	public void ToggleMusic()
	{
		music = !music;

		if (!music && _music.isPlaying) _music.Pause();

		if (music) _music.Play();
	}

	//invierte el mouse
	public void ToggleInvertMouse()
	{
		invertMouse = !invertMouse;
	}

	//muestra el menu de opciones
	public void ViewOptions()
	{
		mode = 1;
		optionsObj.SetActive(true);
		mainObj.SetActive(false);
	}
	//cierra el menu de opciones
	public void CloseOptions()
	{
		mode = 0;
		optionsObj.SetActive(false);
		mainObj.SetActive(true);
	}
	//Guarda el mundo y carga la escena del menu principal despues de un breve retraso 
	public void QuitToTitle()
	{
		

		Invoke("Quitzies", 0.25f);
        
		
    }
	

    void Quitzies()
	{
		Vector3Int playerPos = World.currentWorld.GetPlayerPosition();
		World world = World.currentWorld;
		world.SavePlayerPosition(playerPos);

        World.currentWorld.SaveWorld();
		SceneManager.LoadScene(0);
	}

	void SetOptionsText()
	{
		//Establece el texto de la seccion de graficos
		if(World.currentWorld.gMode == GraphicsMode.Fast) SetTexts(graphicsTexts, "Graficos: Fast");
		else if (World.currentWorld.gMode == GraphicsMode.Fancy) SetTexts(graphicsTexts, "Graficos: Fancy");
		else if(World.currentWorld.gMode == GraphicsMode.Insane) SetTexts(graphicsTexts, "Graficos: Insane");

		//Establece el texto de la seccion de la musica
		if (music) SetTexts(musicTexts, "Musica: ON");
		else SetTexts(musicTexts, "Musica: OFF");
		//Establece el texto de la seccion de invertir el mouse
		if (invertMouse) SetTexts(invMouseTexts, "Invertir Mouse Y: ON");
		else SetTexts(invMouseTexts, "Invertir Mouse Y: OFF");
	}
}