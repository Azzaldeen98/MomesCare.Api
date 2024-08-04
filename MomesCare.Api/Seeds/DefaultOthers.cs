
using MomesCare.Api.Repository;
using Firebase.Auth;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Globalization;
using System.Security.Claims;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Helpers.Enums;

namespace MomesCare.Api.Seeds
{
    public class DefaultOthers
    {



        public static async Task SeedDefaultDataAsync(DataContext context)
        {
            await SeedPostsAndCommentsAsync(context);
            await SeedCareTypeAsync(context);
            await SeedAgeGroupAsync(context);
            await SeedDailyCareTimesAsync(context);
           
        }


        public static async Task SeedCareTypeAsync(DataContext db)
        {

            if (await db.CareTypes.CountAsync() > 0)
                    return;

            string[] types = { "Foods", "Sleep" };
            for (int i=0;i< types.Length; i++)
            {
                await db.CareTypes.AddAsync(new CareType {
                
                name = types[i],
                state =true,
                });

                await db.SaveChangesAsync();
            }
        }
        public static async Task SeedAgeGroupAsync(DataContext db)
        {
            if (await db.AgeGroups.CountAsync() > 0)
            {
                //foreach (var item in (await db.AgeGroups.ToArrayAsync()))
                //{
                //    db.AgeGroups.Remove(item);
                //    await db.SaveChangesAsync();
                //}
                return;
                 
            }
              

            int min = 0,f=1;
            for (int i = 1; i <= 4; i++)
            {
                await db.AgeGroups.AddAsync(new AgeGroup
                {
                    min=f,
                    max=i*3,
                    timePeriodScale= TimePeriodScale.Month,
                });
                min += 3;
                f = min;

                await db.SaveChangesAsync();
            }
        }
        public static async Task SeedDailyCareTimesAsync(DataContext db)
        {
            if (await db.DailyCareTimes.CountAsync() > 0)
                     return;

            var ageGroups = db.AgeGroups;
            var caretypes = db.CareTypes;
            foreach (var type in caretypes)
            {
                foreach (var group in ageGroups)
                {
                    int minuites = 16;
                    for (int i = 0; i < 5; i++)
                    {
                        var item = new DailyCareTimes
                        {
                            descript = type.name,
                            time = TimeSpan.Parse($"12:{minuites}"),
                            state = true,
                            ageGroup = group,
                            careType = type
                        };

                        minuites += 2;

                        await db.DailyCareTimes.AddAsync(item);

                        await db.SaveChangesAsync();
                    }
                }
            }
        }
        public static async Task SeedPostsAndCommentsAsync(DataContext context)
        {

                
            if (await context.Posts.CountAsync() > 0)
            {
                return;
                //context.Posts.RemoveRange(await context.Students.ToListAsync());
                //await context.SaveChangesAsync();
            }


            var users = await context.Users.ToListAsync();
            int n = 0;
            foreach (var user in users)
            {

                var post = new Post
                {
                    Title = $"Post {n++}",
                    Body = "Post Contant",
                    PublishedAt = Helper.GetCurrentTime(),
                    user = user,


                };

                await context.Posts.AddAsync(post);
                await context.SaveChangesAsync();

                var _post = await context.Posts.OrderByDescending(p => p.Id).FirstAsync();

                int flag = 0;
                foreach (var _usr in users)
                {
                    var like = new PostLike { CreatedAt = Helper.GetCurrentTime(), post = _post, user = _usr };
                    await context.PostLikes.AddAsync(like);
                    await context.SaveChangesAsync();

                    var comment = new Comment
                    {
                        Contant = $" comment  {flag++}",
                        CreatedAt = Helper.GetCurrentTime(),
                        user = _usr,
                        post = _post,

                    };

                    await context.Comments.AddAsync(comment);
                    await context.SaveChangesAsync();
                }

            }
        }

    }
       


    
}