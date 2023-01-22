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
		public StatusComplete statusComplete;
		public Light light;

		//-------------------------------------------------
		void Awake()
		{
			if ( light == null )
			{
				light = GetComponent<Light>();
                light.color = Color.red;
			}

			if ( statusComplete == null )
			{
				statusComplete = GetComponent<StatusComplete>();
			}
		}


		//-------------------------------------------------
		void Update()
		{
            light.color = statusComplete.getStatusComplete() ? Color.green : Color.red;
		}
	}
}
