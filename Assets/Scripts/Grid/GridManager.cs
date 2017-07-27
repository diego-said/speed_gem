using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

    private Hashtable grid;

    private const int COLUMN_SIZE = 10;
    private const int ROW_SIZE = 10;
	
    private const float INITIAL_POS_X = -9.0f;
    private const float INITIAL_POS_Y = 7.0f;

    private const float SPACE_GEMS = 1.5f;
	
	private Transform selGem1;
	private Transform selGem2;
	
	public bool selectEnable;
	
	// Use this for initialization
	void Start () {
		
        grid = new Hashtable(COLUMN_SIZE * ROW_SIZE);
		
		selectEnable = true;

        float posX = INITIAL_POS_X;
        float posY = INITIAL_POS_Y;

        for (int i = 0; i < COLUMN_SIZE; i++)
        {
            for (int j = 0; j < ROW_SIZE; j++)
            {
                int key = (j * 10) + i;
                
                GameObject gemTypes = GameObject.Find("GemTypes");

                Transform gem = null;
                while (gem == null)
                {
                    gem = gemTypes.GetComponent<GemTypes>().randomGem();
					GemType gemType = gem.GetComponent<Gem>().gemType;

                    Gem gemUp = GetGemUp(i, j);
                    if(gemUp != null)
                    {
                        GemType gemTypeUp = gemUp.gemType;
                        if(gemType == gemTypeUp) 
                        {
                            gem = null;
                        }
                    }

                    Gem gemLeft = GetGemLeft(i, j);
                    if (gemLeft != null)
                    {
                        GemType gemTypeLeft = gemLeft.gemType;
                        if (gemType == gemTypeLeft)
                        {
                            gem = null;
                        }
                    }
                }

                Transform newGem = (Transform)Instantiate(gem);
				newGem.GetComponent<Gem>().column = i;
				newGem.GetComponent<Gem>().row = j;
				newGem.transform.position = new Vector3(posX, posY, 0.0f);
                grid.Add(key, newGem.GetComponent<Gem>());

                posY -= SPACE_GEMS;
            }
            posX += SPACE_GEMS;
            posY = INITIAL_POS_Y;
        }
	}
	
	// Update is called once per frame
	void Update () {
			if(selGem1 != null) {
				selGem1.Rotate(new Vector3(0, 1.5f, 0));
			}
	}
	
	public void RemoveGems(ArrayList gems) {
		lock(grid) {
			if(gems.Count > 0) {
				Hashtable columnAmount = new Hashtable();
				for(int i=0; i<gems.Count; i++) {
					Gem gem = (Gem)gems[i];
					
					int key = (gem.row * 10) + gem.column;
					grid.Remove(key);
					
					if(columnAmount[gem.column] != null) {
						int count = (int)columnAmount[gem.column];
						columnAmount[gem.column] = count + 1;
					} else {
						columnAmount[gem.column] = 1;
					}
					Destroy(gem.gameObject);
				}
				ReallocateGems(columnAmount);
			}
		}
	}
	
	private void ReallocateGems(Hashtable columnAmount) {
		lock(grid) {
			foreach (object key in columnAmount.Keys) {
				int column = (int)key;
				int amountGems = (int)columnAmount[key];
				
				int row = 0;
				
				int gemKey = (row * 10) + column;
				while(grid[gemKey] != null) {
					row++;
					gemKey = (row * 10) + column;
				}
				
				if(row > 0) {
					MoveGem(column, row-1, amountGems);
				}
				GenerateGems(column, amountGems);
			}
		}
	}
	
	private void MoveGem(int column, int row, int distance) {
		lock(grid) {
			int key = (row * 10) + column;
			
			if(grid[key] != null)
			{
				Gem gem = (Gem)grid[key];
				grid.Remove(key);
				
				row += distance;
				
				gem.row = row;
				Vector3 position = gem.transform.position;
				position.y -= SPACE_GEMS * distance;
				
				iTween.MoveTo(gem.gameObject, position, 0.5f);
				
				int newKey = (row * 10) + column;
				grid.Add(newKey, gem);
				//gem.CheckNewPlace();
				
				MoveGem(column, row-(distance+1), distance);
			}
		}
	}
	
	private void GenerateGems(int column, int amount) {
		lock(grid) {
			GameObject gemTypes = GameObject.Find("GemTypes");
			Transform gem = null;
			for(int i=1; i<=amount; i++) {
				gem = gemTypes.GetComponent<GemTypes>().randomGem();
				
				Transform newGem = (Transform)Instantiate(gem);
				
				float posY = 9.0f;
				float posX = INITIAL_POS_X + (column * SPACE_GEMS);
				
				int key = ((amount - i) * 10) + column;
				
				newGem.GetComponent<Gem>().column = column;
				newGem.GetComponent<Gem>().row = amount - i;
				newGem.transform.position = new Vector3(posX, posY, 0.0f);
				grid.Add(key, newGem.GetComponent<Gem>());
				
				posY = INITIAL_POS_Y - ((amount - i) * SPACE_GEMS);
				
				Vector3 position = new Vector3(posX, posY, 0.0f);
				
				iTween.MoveTo(newGem.gameObject, position, 0.5f);
			}
		}
	}
	
	public void SwapGems()
	{
		if(selGem1 != null && selGem2 != null) {
			lock(grid) {
				Gem gem1 = selGem1.GetComponent<Gem>();
				Gem gem2 = selGem2.GetComponent<Gem>();
				
				int keyGem1 = (gem1.row * 10) + gem1.column;
				int keyGem2 = (gem2.row * 10) + gem2.column;
				
				int column = gem1.column;
				int row = gem1.row;
				
				gem1.column = gem2.column;
				gem1.row = gem2.row;
				
				gem2.column = column;
				gem2.row = row;
				
				grid[keyGem1] = gem2;
				grid[keyGem2] = gem1;
				
				selectEnable = true;
				
				gem1.CheckNewPlace();
				gem2.CheckNewPlace();
				
				selGem1 = null;
				selGem2 = null;
			}
		}
	}
		
	public Gem GetGemPosition(Gem gem, Direction direction)
	{
		lock(grid) {
			switch (direction) {
			case Direction.UP:
				return GetGemUp(gem.column, gem.row);
			case Direction.DOWN:
				return GetGemDown(gem.column, gem.row);
			case Direction.LEFT:
				return GetGemLeft(gem.column, gem.row);
			case Direction.RIGHT:
				return GetGemRight(gem.column, gem.row);
			}
			return null;
		}
	}
	
    public Gem GetGemUp(int column, int row)
    {
        lock(grid) {
			int key = ((row - 1) * 10) + column;
			return (Gem)grid[key];
		}
    }

    public Gem GetGemDown(int column, int row)
    {
        lock(grid) {
			int key = ((row + 1) * 10) + column;
			return (Gem)grid[key];
		}
    }

    public Gem GetGemLeft(int column, int row)
    {
		lock(grid) {
			int key = (row * 10) + (column - 1);
			if ((key / 10) < row)
			{
				return null;
			}
			else
			{
				return (Gem)grid[key];
			}
		}
    }

    public Gem GetGemRight(int column, int row)
    {
        lock(grid) {
			int key = (row * 10) + (column + 1);
			if ((key / 10) > row)
			{
				return null;
			}
			else 
			{
				return (Gem)grid[key];
			}
		}
    }
	
	public Transform SelGem1
	{
		get { return selGem1;  }
		set	{ selGem1 = value; }
	}
	
	public Transform SelGem2
	{
		get { return selGem2;  }
		set	{ selGem2 = value; }
	}
}