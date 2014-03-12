using System;

public class Skill
{
	public enum Types {Paint, Repair, Cover}
	public Types Type;

	// The level this skill is for the worker, warning in the code we
	// use one level higher than we display to the player
	public int Level = 2;

	public Skill ()
	{

	}

	public Skill (Types Type)
	{
		this.Type = Type;
	}
}
