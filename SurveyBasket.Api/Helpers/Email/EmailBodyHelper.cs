
namespace SurveyBasket.Api.Helpers.Email
{
    public static class EmailBodyHelper
    {
        public static string GenerateEmailBody(string template, Dictionary<string,string> templateModel)
        {
            var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
            var reader = new StreamReader(templatePath);
            var emailBody = reader.ReadToEnd();
            reader.Close();

            foreach (var item in templateModel)
                emailBody = emailBody.Replace(item.Key, item.Value);

            return emailBody;
        }
    }
}
