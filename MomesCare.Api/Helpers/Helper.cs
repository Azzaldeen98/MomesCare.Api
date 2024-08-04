using Microsoft.AspNetCore.Mvc.ModelBinding;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel.Comment;
using MomesCare.Api.Entities.ViewModel.Post;
using MomesCare.Api.Entities.ViewModel;
using System.Globalization;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Xml.Linq;
using Microsoft.SqlServer.Server;

namespace MomesCare.Api.Helpers
{
    public class Helper
    {


        public static CommentIndex getCommentIndex(Comment comit,string current_userId, IMapper _mapper)
        {
            var com = _mapper.Map<CommentIndex>(comit);
            com.author = Author.fromUser(comit.user);
            com.likes = (comit == null || comit.likes.IsNullOrEmpty()) ? 0 : comit.likes.Count();
            com.userLiked = comit.likes == null ? false : comit.likes.Any(x => x.user.Id == current_userId);
            return com;
        }

        public static async Task<PostIndex> getPostIndexAsync(Post post, string current_userId, IMapper _mapper)
        {

            return await Task.Run<PostIndex>(() =>
            {

                var comments = post.comments.Select(x => x.Clone()).ToList();

                post.comments = null;

                var _post = _mapper.Map<PostIndex>(post);
                _post.likes = post == null ? 0 : post.likes.Count();
                _post.userLiked = post.likes == null ? false : post.likes.Any(x => x.user.Id == current_userId);
                _post.author = Author.fromUser(post.user);


                if (comments != null)
                {
                    _post.comments = new List<CommentIndex>();

                    foreach (var comit in comments)
                    {
                        var com = getCommentIndex(comit,current_userId,_mapper);
                        _post.comments.Add(com);

                    }
                }
                return _post;
            });

        }

        public static string[] GetModelErrors(ModelStateDictionary modelState)
        {
            return modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();
        }

        public static TimeSpan ConvertToTimeSpan(string time)
        {
            DateTime dateTime;
            string[] formats = { "hh:mm:ss tt", "hh:mm tt" };
            if (DateTime.TryParseExact(time, formats, null, System.Globalization.DateTimeStyles.None, out dateTime))
            {
                return  dateTime.TimeOfDay;
            }

            return TimeSpan.Zero;
        }
        public static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            // إذا لم يكن عيد الميلاد قد مر هذا العام، اطرح سنة واحدة
            if (today < birthDate.AddYears(age) && age>1)
            {
                age--;
            }

            return age;
        }


        public static string ConvertFirstLetterToCapital(string input)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input);
        }

        public static int GetMonthsDifference(DateTime startDate, DateTime endDate)
        {
            int yearDifference = endDate.Year - startDate.Year;
            int monthDifference = endDate.Month - startDate.Month;

            return (yearDifference * 12) + monthDifference;
        }

        public static DateTime GetCurrentTime()
        {
            DateTime serverTime = DateTime.Now;
            TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arabian Standard Time");
            TimeSpan timeOffset = new TimeSpan(hours: -1, minutes: 0, seconds: 0);
            DateTime ksaTime = serverTime.Add(timeOffset).ToUniversalTime();
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(ksaTime, localTimeZone);
            return localTime;
        }



        public static string  getTextFromUrl(string inputText)
        {
            (string text, string _) = getTextAndLink(inputText);
            return text;
        }
        public static string getUrlFromText(string inputText)
        {
            (string _, string url) = getTextAndLink(inputText);
            return url;
        }

        public static bool CurrentTimeEqualOrMoreThanTime(TimeSpan time)
        {
            var currentTime = GetCurrentTime().TimeOfDay;
            int result = TimeSpan.Compare(currentTime,time);
            return  result>=0;
           
        }

        public static (string text,string url) getTextAndLink(string inputText)
        {
            string pattern = @"(https?://[^\s]+)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(inputText);
            if(match.Success)
            {
                string url = match.Value;
                Console.WriteLine("Extracted URL: " + url);
                // استخراج النص بدون الرابط
                string text = inputText.Replace(url, "").Trim();
                Console.WriteLine("Text without URL: " + text);

                return (text,url);
            }
            return (inputText, "");
        }
        public static string GetCurrentDateAsString()
        {
            DateTime currentDate = GetCurrentTime();
            string year = currentDate.Year.ToString();
            string month = currentDate.Month.ToString().PadLeft(2, '0'); // Months are zero-based, so we don't need to add 1
            string day = currentDate.Day.ToString().PadLeft(2, '0');

            string hours = currentDate.Hour.ToString().PadLeft(2, '0');
            string minutes = currentDate.Minute.ToString().PadLeft(2, '0');
            string seconds = currentDate.Second.ToString().PadLeft(2, '0');

            string dateString = $"{year}-{month}-{day}T{hours}:{minutes}:{seconds}";
            // Console.WriteLine(dateString);

            return dateString;
        }

        public static object[] GetHoursAndMinutes(string timeString)
        {
            string[] splitTime = timeString.Split(' ');
            string[] time = splitTime[0].Split(':');
            string meridiem = splitTime[1];

            int hours = int.Parse(time[0]);
            int minutes = int.Parse(time[1]);

            return new object[] { hours, minutes, meridiem };
        }

        public  static (string firstName, string lastName) SplitName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("The full name cannot be empty or null.", nameof(fullName));
            }

            var nameParts = fullName.Split(' ');

            if (nameParts.Length == 1)
            {
                return (nameParts[0], string.Empty); // Only first name provided
            }

            var firstName = nameParts[0];
            var lastName = nameParts[nameParts.Length - 1];

            return (firstName, lastName);
        }

    }
}
