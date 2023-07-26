namespace TodoApi.Application.Common.Interfaces
{
    public interface ISecretsServiceFactory
	{
		ISecretsService GetSecretsService();
	}
}

