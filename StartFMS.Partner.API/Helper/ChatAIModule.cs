using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;
using OpenAI.GPT3.ObjectModels.ResponseModels.ImageResponseModel;
using OpenAI.GPT3.ObjectModels.SharedModels;
using System.Text;

namespace StartFMS.Partner.API.Helper
{
    public static class ChatAIModule 
    {

        public static async Task<string> ChatMessageAsync(CompletionCreateResponse completionResult)
        {
            StringBuilder @string = new StringBuilder();
            if (completionResult.Successful)
            {
                Console.WriteLine(completionResult.Choices.FirstOrDefault());
                @string.AppendLine(completionResult.Choices.Select(x=>x.Text).FirstOrDefault().TrimStart('?'));
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }
            return @string.ToString();
        }

        public static List<string> ChatImage(ImageCreateResponse imageResult)
        {
            List<string> strings = new List<string>() ;
            if (imageResult.Successful)
            {
                Console.WriteLine(string.Join("\n", imageResult.Results.Select(r => r.Url)));
                strings.AddRange(imageResult.Results.Select(r => r.Url));
            }
            return strings;
        }
    }
}
