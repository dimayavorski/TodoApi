using System;
using TodoApi.Infrastructure.Enums;

namespace TodoApi.Infrastructure.Options
{
	public class AppSettings
	{
		public AppSettings()
		{
		}
		public EnvironmentType EnvironmentType { get; set; }
	}
}

