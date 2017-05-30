using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZGameFrame
{
	/// <summary>
	/// 日志等级，为不同输出配置用
	/// </summary>
	public enum LogLevel
	{
	    LOG = 0,
	    WARNING = 1,
	    ASSERT = 2,
	    ERROR = 3,
	    MAX = 4,
	}

	/// <summary>
	/// 日志数据类
	/// </summary>
	public class LogData
	{
	    public string Log { get; set; }
	    public string Track { get; set; }
	    public LogLevel Level { get; set; }
	}

	public interface ILogOutput
	{
		void Log(LogData logData);
		void Close();
	}
}