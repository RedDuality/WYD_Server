using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Services.Interfaces;

namespace server.External;

public class FirebaseAuthService(IConfiguration configuration) : IAuthenticationService
{
    private FirebaseAuth GetInstance()
    {
        if (FirebaseApp.DefaultInstance == null)
        {
            var googleCredentialsJson = configuration.GetValue<string>("GOOGLE_CREDENTIALS")
                ?? throw new InvalidOperationException("Google credentials not found in the environment variable");

            GoogleCredential credential;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(googleCredentialsJson)))
            {
                credential = GoogleCredential.FromStream(stream);
            }

            FirebaseApp.Create(new AppOptions
            {
                Credential = credential,
                ProjectId = "wydaccounts",
            });
        }
        return FirebaseAuth.DefaultInstance;
    }

    public async Task<string> CheckTokenAsync(string token)
    {
        FirebaseToken decodedToken = await GetInstance().VerifyIdTokenAsync(token);
        string uid = decodedToken.Uid;
        return uid;
    }

    public async Task<UserLoginRecord> RetrieveAccount(string uid)
    {
        try
        {
            var userRecord = await GetInstance().GetUserAsync(uid);
            return GetUserRecord(userRecord);
        }
        catch (Exception)
        {
            throw new SecurityTokenValidationException("No Firebase user found");
        }

    }

    public async Task<UserLoginRecord> RetrieveAccountFromMail(string mail)
    {
        var userRecord = await GetInstance().GetUserByEmailAsync(mail);
        return GetUserRecord(userRecord);
    }

    private static UserLoginRecord GetUserRecord(UserRecord userRecord)
    {
        return new UserLoginRecord(userRecord.Email, userRecord.Uid);
    }

}