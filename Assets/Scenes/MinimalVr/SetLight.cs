//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Animator whose speed is set based on a linear mapping
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class SetLight : MonoBehaviour
	{
		public LinearMapping linearMapping;
		public Light light;

		//-------------------------------------------------
		void Awake()
		{
			if ( light == null )
			{
				light = GetComponent<Light>();
                light.color = Color.red;
			}

			if ( linearMapping == null )
			{
				linearMapping = GetComponent<LinearMapping>();
			}
		}


		//-------------------------------------------------
		void Update()
		{
            light.color = linearMapping.value == 1 ? Color.green : Color.red;
		}
	}
}
