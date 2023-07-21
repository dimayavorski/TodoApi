using TodoApi.Application.Common.Enums;

namespace TodoApi.Application.Common.Options
{
    public class AppSettings
	{
		public AppSettings()
		{
		}

        public EnvironmentType EnvironmentType { get; set; }
    }
}

