﻿using UnityEngine;
using System.Collections;

public class ControllerState2D {
	public bool IsCollidingRight{ get; set;}
	public bool IsCollidingLeft{get; set;}
	public bool IsCollidingAbove{ get; set;}
	public bool IsCollidinBellow{ get; set;}
	public bool IsMovingDownSlope{ get; set;}
	public bool IsMovingUpSlope{ get; set;}
	public bool IsGrounded{get{return IsCollidinBellow;}}
	public float SlopeAngle{ get; set;}


	public bool HasCollisions{get{return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidinBellow;}}

	public void Reset()
	{
		IsMovingUpSlope =
			IsMovingDownSlope =
			IsCollidingLeft =
			IsCollidingRight =
			IsCollidingAbove =
				IsCollidinBellow = false;
		SlopeAngle = 0;

	}

	public override string ToString ()
	{
		return string.Format ("(controller: r:{0} l:{1} a:{2} b:{3} down-slope:{4} up-slope:{5} angle:{6})",
		                      IsCollidingRight, 
		                      IsCollidingLeft, 
		                      IsCollidingAbove, 
		                      IsCollidinBellow, 
		                      IsMovingDownSlope, 
		                      IsMovingUpSlope, 
		                      SlopeAngle);
	}

}