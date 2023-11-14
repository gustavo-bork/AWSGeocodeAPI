using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace AWSGeocodeAPI.Services;

public class SecretsManagerService
{
    public async Task<string> GetSecret(string secretName)
    {
        string region = "sa-east-1";
        IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT"
        };

        try
        {
            GetSecretValueResponse response = await client.GetSecretValueAsync(request);
            return response.SecretString;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
