using System;
using Amazon.Lambda.Core;
using RestSharp;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

public class Function
{
    public string SendEmailHandler(string sender, string recipient, string subject, string body, ILambdaContext context)
    {
        // Configurações do Mailgun
        var mailgunApiKey = Environment.GetEnvironmentVariable("MAILGUN_API_KEY");
        var mailgunDomain = Environment.GetEnvironmentVariable("MAILGUN_DOMAIN");
        var mailgunFrom = Environment.GetEnvironmentVariable("MAILGUN_FROM");
        var mailgunUrl = $"https://api.mailgun.net/v3/{mailgunDomain}/messages";

        // Construindo a requisição
        var client = new RestClient(mailgunUrl);
        var request = new RestRequest();
        request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes($"api:{mailgunApiKey}")));
        request.AddParameter("domain", mailgunDomain);
        request.AddParameter("from", mailgunFrom);
        request.AddParameter("to", recipient);
        request.AddParameter("subject", subject);
        request.AddParameter("text", body);

        // Enviando o e-mail
        var response = client.ExecutePost(request);

        if (response.IsSuccessful)
        {
            return "E-mail enviado com sucesso!";
        }
        else
        {
            return "Falha ao enviar e-mail.";
        }
    }
}
