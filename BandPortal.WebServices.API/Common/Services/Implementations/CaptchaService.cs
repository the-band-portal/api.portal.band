using BandPortal.WebServices.API.Common.Configuration;
using BandPortal.WebServices.API.Common.DataStructures;

namespace BandPortal.WebServices.API.Common.Services.Implementations
{
    public class CaptchaService : ICaptchaService
    {
        private readonly ConfigStructure _configuration;
        private readonly ILogger<CaptchaService> _logger;
        private readonly HttpClient _httpClient;

        public CaptchaService(
            IConfiguration configuration,
            ILogger<CaptchaService> logger,
            HttpClient httpClient
            )
        {
            _configuration = configuration.Get<ConfigStructure>()!;
            _logger = logger;

            _httpClient = httpClient;
        }

        public async Task<bool> VerifyRecaptchaAsync(string token, string? clientIp)
        {
            try
            {
                if (_configuration.Development.DisableReCAPTCHA)
                {
                    _logger.LogWarning("recaptcha verification has been disabled through config");
                    return true;
                }

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", _configuration.Captcha.SecretKey),
                    new KeyValuePair<string, string>("response", token),
                    new KeyValuePair<string, string>("remoteip", clientIp ?? "")
                });

                var response = await _httpClient.PostAsync(
                    "https://www.google.com/recaptcha/api/siteverify",
                    content
                );

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadFromJsonAsync<CaptchaResponse>();

                if (jsonResponse == null)
                {
                    _logger.LogError("invalid response from recaptcha");
                    return false;
                }

                if (!jsonResponse.Success)
                {
                    _logger.LogWarning(
                        "recaptcha verification has failed: {Errors}",
                        string.Join(", ", jsonResponse.ErrorCodes ?? Array.Empty<string>())
                    );
                    return false;
                }

                if (jsonResponse.Score.HasValue && jsonResponse.Score < _configuration.Captcha.ScoreThreshold)
                {
                    _logger.LogWarning(
                        "recaptcha score below score threshold({Score})",
                        jsonResponse.Score
                    );
                    return false;
                }

                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "could not connect to recaptcha service");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "unexpected error thrown during recaptcha verification");
                return false;
            }
        }
    }
}
