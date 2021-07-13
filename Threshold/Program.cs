using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Threshold
{
    class Program
    {
        static void Main(string[] args)
        {
            //TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable("OUTPUT_PATH"), true);

            int threshold = Convert.ToInt32(Console.ReadLine().Trim());

            List<string> result = getUsernames(threshold);

            Console.WriteLine($"{string.Join("\n", result)}");

            //textWriter.WriteLine(String.Join("\n", result));

            //textWriter.Flush();
            //textWriter.Close();
        }

        public static List<string> getUsernames(int threshold)
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("https://jsonmock.hackerrank.com/api/"),
            };

            List<string> userNames = new List<string>();
            int pageNumber = 1;
            bool endLoop = false;
            while(!endLoop)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"article_users?page={pageNumber}");

                HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    UserSubmissionResponse responseData = response.Content.ReadAsAsync<UserSubmissionResponse>().GetAwaiter().GetResult();

                    foreach (UserSubmission submission in responseData.data)
                    {
                        if (submission.submitted > threshold)
                        {

                            if (userNames.Count == threshold)
                                break;

                            userNames.Add(submission.username);
                        }
                    }

                    endLoop = (pageNumber == responseData.total_pages) || (userNames.Count == threshold);
                }
                else
                {
                    Console.WriteLine("Failed to get results");
                }

                pageNumber++;
            }
            

            return userNames;
        }
    }


    public class UserSubmissionResponse
    {

        public int per_page { get; set; }

        public int total { get; set; }

        public int total_pages { get; set; }

        public IEnumerable<UserSubmission> data { get; set; }
    }

    public class UserSubmission
    {
        public int id { get; set; }

        public string username { get; set; }

        public int submitted { get; set; }

        public string about { get; set; }

       public DateTimeOffset updated_at { get; set; }

        public int submission_count { get; set; }

        public int comment_count { get; set; }

        //public DateTimeOffset created_at { get; set; }
    }
}
