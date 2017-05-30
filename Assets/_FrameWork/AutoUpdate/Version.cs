﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZGameFrame
{
	public class Version
	{
	    public int BigVersion { get; set; }
	    public int MiddleVersion { get; set; }
	    public int SmallVersion { get; set; }
	    public int ResVersion { get; set; }

	    public static Version CreateVersion(string verStr)
	    {
	        string[] versions = verStr.Split('.');
	        Log.Assert(versions.Length == 4, "Error Version Format : " + verStr);
	        Version version = new Version
	        {
	            BigVersion = int.Parse(versions[0]),
	            MiddleVersion = int.Parse(versions[1]),
	            SmallVersion = int.Parse(versions[2]),
	            ResVersion = int.Parse(versions[3]),
	        };
	        return version;
	    }
	}
}
