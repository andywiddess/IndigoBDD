﻿using System;

namespace Indigo.CrossCutting.Utilities.Algorithms.Implementations
{
	public interface TarjanNodeProperties
	{
		int Index { get; set; }
		int LowLink { get; set; }
	}
}