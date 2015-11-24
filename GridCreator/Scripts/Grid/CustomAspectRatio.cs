using UnityEngine;
using System.Collections;

public class CustomAspectRatio
{
	public int xRatio {get; private set;}
	public int yRatio {get; private set;}

	public float screenSizePercentage {get; private set;}

	public float useWidth {get; private set;}
	public float useHeight {get; private set;}

	//Calc Aspect Ratio for full screen
	public CustomAspectRatio(int xRatio, int yRatio)
	{
		this.xRatio = xRatio;
		this.yRatio = yRatio;

		screenSizePercentage = 100f;

		CalculateUseableSpace();
	}

	//Calc Aspect Ratio for a percentage of the full screen
	public CustomAspectRatio(int xRatio, int yRatio, float percentageOfScreenSpace)
	{
		this.xRatio = xRatio;
		this.yRatio = yRatio;
		
		screenSizePercentage = percentageOfScreenSpace;
		
		CalculateUseableSpace();
	}

	private void CalculateUseableSpace()
	{
		float desiredRatio = yRatio/(float)xRatio;
		float currentRatio = Screen.height/(float)Screen.width;
		float ratioModificationFactor = desiredRatio - currentRatio;
		
		if(currentRatio < desiredRatio)
		{
			useWidth = Screen.width - ((Screen.width/desiredRatio) * ratioModificationFactor);
			useHeight = Screen.height;
		}
		else if (currentRatio > desiredRatio)
		{
			useWidth = Screen.width;
			useHeight = Screen.height + ((Screen.height/currentRatio) * ratioModificationFactor);
		}
		else if(currentRatio == desiredRatio)
		{
			useWidth = Screen.width;
			useHeight = Screen.height;
		}

		//modify values to a percentage of the screen size
		useWidth = (useWidth/100) * screenSizePercentage;
		useHeight = (useHeight/100) * screenSizePercentage;
	}

	public override string ToString()
	{
		return xRatio + ", " + yRatio; 
	}
}

