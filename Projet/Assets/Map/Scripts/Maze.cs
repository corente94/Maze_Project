﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Maze : NetworkBehaviour
{

	public int width, height;
	

	public GameObject BlocT;
	public GameObject BlocL;
	public GameObject BlocI;

	public GameObject Tree;
	public GameObject Statue;
	public GameObject Coin;

	private int max_artefact = 4;

	private Bloc[,] grid;
	

	private Vector2 _randomCellPos;

	protected GameObject visualBlocInit;

	public GameObject Postion1;
	public GameObject Postion2;
	public GameObject Postion3;
	public GameObject Postion4;

	public InputField ligne;
	public InputField direction;
	private string memoire_ligne;
	private string memoire_direction;
	
	
	private bool buton;

	private float timer;
	
	
	// Use this for initialization
	void Start () {
		grid = new Bloc[width,height];
		Init();
		Postion1.transform.position = grid[0, 0].obj.transform.position + new Vector3(0,3,0);
		Postion2.transform.position = grid[0, 8].obj.transform.position + new Vector3(0,3,0);
		Postion3.transform.position = grid[8, 0].obj.transform.position + new Vector3(0,3,0);
		Postion4.transform.position = grid[8, 8].obj.transform.position + new Vector3(0,3,0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.deltaTime - timer > 30)
		{
			buton = false;
			printGrid();
			detruire(memoire_ligne, memoire_direction);
			MajBloc(memoire_ligne,memoire_direction);
			ajout_Bloc(memoire_ligne,memoire_direction);
			printGrid();
			timer = 0;
		}
		if (buton)
		{
			dico(memoire_ligne,memoire_direction);
			
		}
		
	}

	
//######################################################################################################################################################################################
//     AUTRES
	
	
	
	
	public void clicked()
	{
		if (ligne.text.Length != 0 && direction.text.Length != 0)
		{
			buton = true;
			memoire_direction = direction.text;
			memoire_ligne = ligne.text;
			timer = Time.deltaTime;
		}
	}

	public void printGrid()
	{
		string ret = "";
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				ret = ret + grid[j,i].type + ",";
			}
			ret = ret + "\n";
		}
		Debug.Log(ret);
	}

	
	
//#####################################################################################################################################################################################	
//     FONCTIONS QUI CREER LA MAP	
	
	
	
	
	void Init()
	{
		for (int j = 0; j < width; j++)
		{
			for (int i = 0; i < height; i++)
			{
				grid[j,i] = new Bloc();
				grid[j, i].xpos = j;
				grid[j, i].zpos = i;
			}
		}
		InitVisual();
		
	}

	

	void InitVisual()
	{
		
		for (int p = 0; p < grid.GetLength(0); p++)
		{
			for (int w = 0; w < grid.GetLongLength(1); w++)
			{
				Bloc bloc = grid[p, w];
				GameObject visualprefabbloc;

				if (p == 0 && w == 0)
				{
					visualprefabbloc = BlocL;
					bloc.type = "L";
					visualBlocInit = Instantiate(visualprefabbloc, new Vector3(bloc.xpos * 20, 0, (height - bloc.zpos)* 20), Quaternion.identity) as GameObject;
				
				
					visualBlocInit.transform.parent = transform;
					visualBlocInit.transform.name = bloc.xpos + "_" + bloc.zpos;

					bloc.obj = visualBlocInit;
				}
				else if (p == 0 && w == grid.GetLongLength(1) - 1)
				{
					visualprefabbloc = BlocL;
					bloc.type = "L";
					visualBlocInit = Instantiate(visualprefabbloc, new Vector3(bloc.xpos * 20, 0, (height - bloc.zpos)* 20), Quaternion.identity) as GameObject;
				
				
					visualBlocInit.transform.Rotate(Vector3.back + new Vector3(0, -90, 0));
					
					
					visualBlocInit.transform.parent = transform;
					visualBlocInit.transform.name = bloc.xpos + "_" + bloc.zpos;

					bloc.obj = visualBlocInit;
				}
				else if (p == grid.GetLength(0) - 1 && w == 0)
				{
					visualprefabbloc = BlocL;
					bloc.type = "L";
					visualBlocInit = Instantiate(visualprefabbloc, new Vector3(bloc.xpos * 20, 0, (height - bloc.zpos)* 20), Quaternion.identity) as GameObject;
				
				
					visualBlocInit.transform.Rotate(Vector3.back + new Vector3(0, 90, 0));
					
					
					visualBlocInit.transform.parent = transform;
					visualBlocInit.transform.name = bloc.xpos + "_" + bloc.zpos;

					bloc.obj = visualBlocInit;
				}
				else if (p == grid.GetLength(0) - 1 && w == grid.GetLength(1) - 1)
				{
					visualprefabbloc = BlocL;
					bloc.type = "L";
					visualBlocInit = Instantiate(visualprefabbloc, new Vector3(bloc.xpos * 20, 0, (height - bloc.zpos) * 20),
						Quaternion.identity) as GameObject;


					visualBlocInit.transform.Rotate(Vector3.back + new Vector3(0, 180, 0));


					visualBlocInit.transform.parent = transform;
					visualBlocInit.transform.name = bloc.xpos + "_" + bloc.zpos;

					bloc.obj = visualBlocInit;
				}
				else
				{
					int rd = Random.Range(1,4);
					if (rd == 1)
					{
						visualprefabbloc = BlocI;
						bloc.type = "I";
					}
					else if (rd == 2)
					{
						visualprefabbloc = BlocL;
						bloc.type = "L";
					}
					else
					{
						visualprefabbloc = BlocT;
						bloc.type = "T";
					}
				
				
					visualBlocInit = Instantiate(visualprefabbloc, new Vector3(bloc.xpos * 20, 0, (height - bloc.zpos)* 20), Quaternion.identity) as GameObject;
				
				
					int t = 0;
					while (t<bloc.rotate)
					{
						visualBlocInit.transform.Rotate(Vector3.back + new Vector3(0, -90, 0));
						t++;
					}
				
					visualBlocInit.transform.parent = transform;
					visualBlocInit.transform.name = bloc.xpos + "_" + bloc.zpos;

					bloc.obj = visualBlocInit;


					int rd2 = Random.Range(1, 101);
					if (rd2 < 10  && max_artefact >= 1)
					{
						int rd3 = Random.Range(1, 4);
						if (rd3 == 1)
						{
							bloc.AMB = Instantiate(Tree, new Vector3(bloc.xpos*20,3f,(height - bloc.zpos)* 20),Quaternion.identity);
						}
						else if (rd3 == 2)
						{
							bloc.AMB =Instantiate(Coin, new Vector3(bloc.xpos*20,3f,(height - bloc.zpos)* 20),Quaternion.identity);
						}
						else
						{
							bloc.AMB = Instantiate(Statue, new Vector3(bloc.xpos*20,3f,(height - bloc.zpos)* 20),Quaternion.identity);
						}
						max_artefact--;
					}
					else if (rd2 > 30 && rd2 < 60)
					{
					
					}
				}
			}
		}
	}
	
	
//#############################################################################################################################################################################################
//  FONCTIONS QUI POUSSE LES BLOCS
	
	void PushBloc(int i, int j, int direction)
		{
			if (direction == 1)
			{
				for (int p = 0; p <= j; p++)
				{
					GameObject tmp = grid[i, p].obj;
					Bloc bloc = grid[i, p];
					
					if (bloc.rotate != direction)
					{
						if (bloc.rotate == 2)
						{
							tmp.transform.Translate(0, 0,5 * Time.deltaTime);
						}
						else if (bloc.rotate == 3)
						{
							tmp.transform.Translate( 5 * Time.deltaTime,0, 0);
						}
						else if (bloc.rotate == 4)
						{
							tmp.transform.Translate( 0, 0,-5 * Time.deltaTime);
						}
					}
					else
					{
						tmp.transform.Translate(-5*Time.deltaTime,0,0);
					}
				}
			}
			else if (direction == 2)
			{
				for (int p = i; p >= 0; p--)
				{
					Bloc bloc = grid[p, j];
					GameObject tmp = grid[p, j].obj;

					if (bloc.rotate != direction)
					{
						if (bloc.rotate == 1)
						{
							tmp.transform.Translate(0, 0, 5 * Time.deltaTime);
						}
						else if (bloc.rotate == 3)
						{
							tmp.transform.Translate(0,0,-5*Time.deltaTime);
						}
						else if (bloc.rotate == 4)
						{
							tmp.transform.Translate(-5*Time.deltaTime,0,0);
						}
					}
					else
					{
						tmp.transform.Translate(5*Time.deltaTime,0,0);
					}
				}
			}
			else if (direction == 3)
			{
				for (int p = j ; p >= 0; p--)
				{
					GameObject tmp = grid[i, p].obj;
					Bloc bloc = grid[i, p];
					
					if (bloc.rotate != direction)
					{
						if (bloc.rotate == 2)
						{
							tmp.transform.Translate(0, 0,-5 * Time.deltaTime);
						}
						else if (bloc.rotate == 1)
						{
							tmp.transform.Translate( 5 * Time.deltaTime,0, 0);
						}
						else if (bloc.rotate == 4)
						{
							tmp.transform.Translate( 0, 0,5 * Time.deltaTime);
						}
					}
					else
					{
						tmp.transform.Translate(-5*Time.deltaTime,0,0);
					}
				}
			}
			else if (direction == 4)
			{
				for (int p = 0; p <= i; p++)
				{
					GameObject tmp = grid[p, j].obj;
					Bloc bloc = grid[p, j];
					
					if (bloc.rotate != direction)
					{
						if (bloc.rotate == 2)
						{
							tmp.transform.Translate(-5 * Time.deltaTime,0,0);
						}
						else if (bloc.rotate == 3)
						{
							tmp.transform.Translate(0, 0,5 * Time.deltaTime);
						}
						else if (bloc.rotate == 1)
						{
							tmp.transform.Translate(0, 0, -5 * Time.deltaTime);
						}
					}
					else
					{
						tmp.transform.Translate(5*Time.deltaTime,0,0);
					}
				}
			}
		}

	public void dico(string colone, string direction)
	{
		char test = colone[0];
		test = Char.ToLower(test);
		if(test >= '0' && test <= '8')
		{
			int i = test -48;
			if (direction == "bas")
			{
				PushBloc(i, 8,1);
			}
			else if (direction == "haut")
			{
				PushBloc(i, 8,3);
			}
			else
			{
				Console.WriteLine("La direction n'est pas haut ou bas");
			}
		}
		else if(test >= 'a' && test <= 'i')
		{
			int j = test -97;
			if (direction == "gauche")
			{
				PushBloc(8,j,2);
			}
			else if (direction == "droite")
			{
				PushBloc(8,j,4);
			}
			else
			{
				Console.WriteLine("La direction n'est pas gauche ou droite");
			}
		}
		else
		{
			Console.WriteLine("La colonne/ligne n'est pas comprise entre 0-7 / A-I");
		}
	}

	
	
//##################################################################################################################################################################################
//               BLOCS DES JOUEURS

	void creation(int i, int j)
	{
		Bloc bloc = grid[i, j];
		GameObject visualprefabbloc;
		
		int rd = Random.Range(1,4);
		if (rd == 1)
		{
			visualprefabbloc = BlocI;
			bloc.type = "I";
		}
		else if (rd == 2)
		{
			visualprefabbloc = BlocL;
			bloc.type = "L";
		}
		else
		{
			visualprefabbloc = BlocT;
			bloc.type = "T";
		}
				
		visualBlocInit = Instantiate(visualprefabbloc, new Vector3(bloc.xpos * 20, 0, (height - bloc.zpos)* 20), Quaternion.identity) as GameObject;

		
		int t = 0;
		while (t<bloc.rotate)
		{
			visualBlocInit.transform.Rotate(Vector3.back + new Vector3(0, -90, 0));
			t++;
		}
				
		visualBlocInit.transform.parent = transform;
		visualBlocInit.transform.name = bloc.xpos + "_" + bloc.zpos;
		
		grid[i,j].obj = visualBlocInit;
	}
	
	

	void ajout_Bloc(string colone, string direction)
	{
		char test = colone[0];
		test = Char.ToLower(test);
		if (test >= '0' && test <= '8')
		{
			int i = test - 48;
			if (direction == "bas")
			{
				creation(i,0);
			}
			else if (direction == "haut")
			{
				creation(i,8);
			}
		}
		else if (test >= 'a' && test <= 'i')
		{
			int j = test - 97;
			if (direction == "gauche")
			{
				creation(8,j);
			}
			else if (direction == "droite")
			{
				creation(0,j);
			}
		}
	}
	
//###########################################################################################################################################################################
// 								MAJ DE LA MAP
	
	public void MajBloc(string colone, string direction)
	{
		char test = colone[0];
		test = Char.ToLower(test);
		if(test >= '0' && test <= '8')
		{
			int i = test -48;
			if (direction == "bas")
			{
				 Changebloc(i, 8,1);
			}
			else if (direction == "haut")
			{
				Changebloc(i, 8,3);
			}
		}
		else if (test >= 'a' && test <= 'i')
		{
			int j = test - 97;
			if (direction == "gauche")
			{
				Changebloc(8, j, 2);
			}
			else if (direction == "droite")
			{
				Changebloc(8, j, 4);
			}
		}
	}
	
	void Changebloc(int i, int j, int direction)
	{
		if (direction == 1)
		{
			for (int p = j ; p > 0; p--)
			{
				grid[i, p] = grid[i, p - 1];
			}
		}
		else if (direction == 2)
		{
			for (int p = 0; p < i ; p++)
			{
				grid[p,j] = grid[p+1,j];
			}
		}
		else if (direction == 3)
		{
			for (int p = 0; p < j; p++)
			{
				grid[i,p] = grid[i,p+1];
			}
			
		}
		else if (direction == 4)
		{
			for (int p = i; p > 0; p--)
			{
		    	grid[p,j] = grid[p - 1,j];
			}
		}
	}
	
//############################################################################################################################################################################
//                         DESTRUCTION DE BLOCS

	void detruire(string colone, string direction)
	{
		int p = 100;
		char test = colone[0];
		test = Char.ToLower(test);
		if(test >= '0' && test <= '8')
		{
			int i = test -48;
			if (direction == "bas")
			{
				while (p > 0)
				{
					grid[i,8].obj.transform.Translate(0,0,-5);
					p--;
				}
				Destroy(grid[i,8].obj);
			}
			else if (direction == "haut")
			{
				while (p > 0)
				{
					grid[i,0].obj.transform.Translate(0,0,-5);
					p--;
				}
				Destroy(grid[i,0].obj);
			}
		}
		else if (test >= 'a' && test <= 'i')
		{
			int j = test - 97;
			if (direction == "gauche")
			{
				while (p > 0)
				{
					grid[0,j].obj.transform.Translate(0,0,-5);
					p--;
				}
				Destroy(grid[0,j].obj);
			}
			else if (direction == "droite")
			{
				while (p > 0)
				{
					grid[8,j].obj.transform.Translate(0,0,-5);
					p--;
				}
				Destroy(grid[8,j].obj);
			}
		}
	}
		
		
		
}


