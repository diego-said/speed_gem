using UnityEngine;
using System.Collections;

public class SchedulerMoveGem
{
	
	private int column;
	private int row;
	
	private int distance;
	
	public int Column {
		get { return column; }
		set { column = value; }
	}
	
	
	public int Distance {
		get { return distance; }
		set { distance = value; }
	}
	
	public int Row {
		get { return row; }
		set { row = value; }
	}
}