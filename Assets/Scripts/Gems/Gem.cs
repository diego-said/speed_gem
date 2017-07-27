using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gem : MonoBehaviour
{

	private GridManager grid;

	public int column;
	public int row;

	public GemType gemType;
	
	// Use this for initialization
	void Start ()
	{
		grid = GameObject.Find ("GridManager").GetComponent<GridManager> ();
	}

	// Update is called once per frame
	void Update ()
	{
		this.transform.Rotate (new Vector3 (0, 0.5f, 0));
	}
	
	public void Destroy() {
		Destroy(this);	
	}
	
	public void CheckNewPlace() {
		ArrayList gems = new ArrayList();
		
		Gem gemUp = grid.GetGemPosition(this, Direction.UP);
		Gem gemDown = grid.GetGemPosition(this, Direction.DOWN);
		Gem gemLeft = grid.GetGemPosition(this, Direction.LEFT);
		Gem gemRight = grid.GetGemPosition(this, Direction.RIGHT);
		
		CheckUpAndDown(gemUp, gemDown, ref gems);
		CheckLeftAndRight(gemLeft, gemRight, ref gems);
		
		grid.RemoveGems(gems);
	}
	
	private void CheckUpAndDown(Gem gemUp, Gem gemDown, ref ArrayList gems) {
		if(gemUp != null && gemDown != null) {
			if(gemType == gemUp.gemType && gemType == gemDown.gemType) {
				gems.Add(gemUp);
				gems.Add(this);
				gems.Add(gemDown);
				gemUp.CheckNextGem(gemUp, ref gems, Direction.UP);
				gemDown.CheckNextGem(gemDown, ref gems, Direction.DOWN);
			} else if(gemType == gemUp.gemType) {
				Gem gemNextUp = grid.GetGemPosition(gemUp, Direction.UP);
				if(gemNextUp != null && gemUp.gemType == gemNextUp.gemType) {
					gems.Add(this);
					gems.Add(gemUp);
					gems.Add(gemNextUp);
					gemNextUp.CheckNextGem(gemNextUp, ref gems, Direction.UP);
				}
			} else if(gemType == gemDown.gemType) {
				Gem gemNextDown = grid.GetGemPosition(gemDown, Direction.DOWN);
				if(gemNextDown != null && gemDown.gemType == gemNextDown.gemType) {
					gems.Add(this);
					gems.Add(gemDown);
					gems.Add(gemNextDown);
					gemNextDown.CheckNextGem(gemNextDown, ref gems, Direction.DOWN);
				}
			}
		} else if(gemUp != null) {
			if(gemType == gemUp.gemType) {
				Gem gemNextUp = grid.GetGemPosition(gemUp, Direction.UP);
				if(gemNextUp != null && gemUp.gemType == gemNextUp.gemType) {
					gems.Add(this);
					gems.Add(gemUp);
					gems.Add(gemNextUp);
					gemNextUp.CheckNextGem(gemNextUp, ref gems, Direction.UP);
				}
			}
		} else if(gemDown != null) {
			if(gemType == gemDown.gemType) {
				Gem gemNextDown = grid.GetGemPosition(gemDown, Direction.DOWN);
				if(gemNextDown != null && gemDown.gemType == gemNextDown.gemType) {
					gems.Add(this);
					gems.Add(gemDown);
					gems.Add(gemNextDown);
					gemNextDown.CheckNextGem(gemNextDown, ref gems, Direction.DOWN);
				}
			}
		}	
	}
	
	private void CheckLeftAndRight(Gem gemLeft, Gem gemRight, ref ArrayList gems) {
		if(gemLeft != null && gemRight != null) {
			if(gemType == gemLeft.gemType && gemType == gemRight.gemType) {
				gems.Add(gemLeft);
				gems.Add(this);
				gems.Add(gemRight);
				gemLeft.CheckNextGem(gemLeft, ref gems, Direction.LEFT);
				gemRight.CheckNextGem(gemRight, ref gems, Direction.RIGHT);
			} else if(gemType == gemLeft.gemType) {
				Gem gemNextLeft = grid.GetGemPosition(gemLeft, Direction.LEFT);
				if(gemNextLeft != null && gemLeft.gemType == gemNextLeft.gemType) {
					gems.Add(this);
					gems.Add(gemLeft);
					gems.Add(gemNextLeft);
					gemNextLeft.CheckNextGem(gemNextLeft, ref gems, Direction.LEFT);
				}
			} else if(gemType == gemRight.gemType) {
				Gem gemNextRight = grid.GetGemPosition(gemRight, Direction.RIGHT);
				if(gemNextRight != null && gemRight.gemType == gemNextRight.gemType) {
					gems.Add(this);
					gems.Add(gemRight);
					gems.Add(gemNextRight);
					gemNextRight.CheckNextGem(gemNextRight, ref gems, Direction.RIGHT);
				}
			}
		} else if(gemLeft != null) {
			if(gemType == gemLeft.gemType) {
				Gem gemNextLeft = grid.GetGemPosition(gemLeft, Direction.LEFT);
				if(gemNextLeft != null && gemLeft.gemType == gemNextLeft.gemType) {
					gems.Add(this);
					gems.Add(gemLeft);
					gems.Add(gemNextLeft);
					gemNextLeft.CheckNextGem(gemNextLeft, ref gems, Direction.LEFT);
				}
			}
		} else if(gemRight != null) {
			if(gemType == gemRight.gemType) {
				Gem gemNextRight = grid.GetGemPosition(gemRight, Direction.RIGHT);
				if(gemNextRight != null && gemRight.gemType == gemNextRight.gemType) {
					gems.Add(this);
					gems.Add(gemRight);
					gems.Add(gemNextRight);
					gemNextRight.CheckNextGem(gemNextRight, ref gems, Direction.RIGHT);
				}
			}
		}
	}
	
	public void CheckNextGem(Gem gem, ref ArrayList gems, Direction direction) {
		Gem nextGem = grid.GetGemPosition(gem, direction);
		if(nextGem != null && gem.gemType == nextGem.gemType) {
			gems.Add(nextGem);
			nextGem.CheckNextGem(nextGem, ref gems, direction);
		}
	}

}