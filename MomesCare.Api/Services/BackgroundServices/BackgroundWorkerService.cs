using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Helpers;
using MomesCare.Api.Helpers.Enums;
using MomesCare.Api.Repository;
using MomesCare.Api.Services;
using MomesCare.Api.Services.BackgroundServices.NewFolder;
using MomesCare.Api.Services.SubServices;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Threading;
using System.Timers;


public class BackgroundWorkerService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<BackgroundWorkerService> _logger;
    private readonly ServiceMessages _serviceMessages;

    public BackgroundWorkerService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<BackgroundWorkerService> logger,
        ServiceMessages serviceMessages)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _serviceMessages = serviceMessages;
    }

    protected async Task<bool> checkIfSentCareNotifyAsync(DailyCareTimes dailyCareTime, NotifyUserBabyWithFCMToken usersFcmTokens)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dailyBabyCareNotifySentService = scope.ServiceProvider.GetRequiredService<DailyBabyCareNotifySentService>();
            var itemsSent = await dailyBabyCareNotifySentService.getAllAsync();

            if (itemsSent.IsNullOrEmpty())
                return false;

            DateTime currentTime = Helper.GetCurrentTime();
            return itemsSent.Any(x => x.createdAt.Day == currentTime.Day 
                && x.dailyCareTimeId == dailyCareTime.id 
                && x.babyId == usersFcmTokens.baby.Id); 

                //&& itemsSent.Any(y => y.babyId == x.baby?.Id))
                //&& usersFcmTokens.Any(x => itemsSent.Any(y => y.babyId == x.baby?.Id));
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dailyCareTimesBackgroundServices = scope.ServiceProvider.GetRequiredService<DailyCareTimesBackgroundServices>();
                    var dailyCareTimes = await dailyCareTimesBackgroundServices.getAllByTimeAsync(Helper.GetCurrentTime().TimeOfDay);
                    //_logger.LogInformation($"dailyCareTimes: {Helper.GetCurrentTime().TimeOfDay.ToString()}");
             
                    if (dailyCareTimes == null) continue;

                    foreach (var item in dailyCareTimes)
                    {
                        var usersFcmTokens = await dailyCareTimesBackgroundServices.getUserFCMTokensByAgeGroupAsync(item.ageGroup);

                        

                        if (usersFcmTokens.IsNullOrEmpty())
                            continue;

                        foreach (var userFcmInfo in usersFcmTokens)
                        {
                            try
                            {

                                var isSentNotify= await checkIfSentCareNotifyAsync(item, userFcmInfo);
                                
                                if(isSentNotify)
                                    continue;

                                var msgBody = new
                                {
                                    babyId = userFcmInfo.baby.Id,
                                    babyName = userFcmInfo.baby.Name,
                                    message = item.descript,
                                };

                                string notifyBody = JsonSerializer.Serialize(msgBody);

                                if (notifyBody.IsNullOrEmpty()) continue;

                                await _serviceMessages.sendMessageAsync(
                                    token: userFcmInfo.fcmToken,
                                    title: $"مــوعد {item.careType.name}",
                                    body: notifyBody,
                                    topic: $"{userFcmInfo.userId}",
                                    tag: $"{NotificationType.CareBaby}");

                                DateTime dateDailyCare = DateTime.Today.Add(item.time);
                                var dailyBabyCareNotifySentService = scope.ServiceProvider.GetRequiredService<DailyBabyCareNotifySentService>();
                              
                                await dailyBabyCareNotifySentService.AddAsync(new BabyHealthCareNotificationsSent
                                {
                                    babyId = userFcmInfo.baby.Id,
                                    dailyCareTimeId = item.id,
                                    createdAt = dateDailyCare,
                                });

                                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogInformation($"Exception Services Worker: {ex.Message}");
                            }
                        }

                        await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                    }
                  
                }
                _logger.LogInformation($"Services Worker...");
                //TimeSpan.FromHours(1)
                await Task.Delay(10000, stoppingToken);
            }
            catch (TaskCanceledException)
            {

                _logger.LogInformation("Error Services Worker running...");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }
    }
}



public class BackgroundWorkerService2 : BackgroundService
{

    private readonly DailyCareTimesBackgroundServices dailyCareTimesBackgroundServices;
    private readonly DailyBabyCareNotifySentService dailyBabyCareNotifySentService;
    private readonly ServiceMessages _serviceMessages;
    private readonly ILogger<BackgroundWorkerService2> _logger;

    private List<BabyHealthCareNotificationsSent> itemsSent;

    public BackgroundWorkerService2(
                                DailyCareTimesBackgroundServices dailyCareTimesBackgroundServices,
                                DailyBabyCareNotifySentService dailyBabyCareNotifySentService,
                                ILogger<BackgroundWorkerService2> logger,
                                ServiceMessages serviceMessages)
    {
        this.dailyCareTimesBackgroundServices = dailyCareTimesBackgroundServices;
        this.dailyBabyCareNotifySentService = dailyBabyCareNotifySentService;
        this._logger = logger;
        _serviceMessages = serviceMessages;
    }




    protected  async  Task<bool> checkIfSentCareNotifyAsync(DailyCareTimes dailyCareTime, IEnumerable<NotifyUserBabyWithFCMToken> usersFcmTokens)
    {
        DateTime currentTime = Helper.GetCurrentTime();
        DateTime dateDailyCare = DateTime.Today.Add(dailyCareTime.time);

        //if (itemsSent == null && !itemsSent.All(x => x.createdAt.Day == currentTime.Day && x.createdAt.Day == currentTime.Day))
        //{
        //    itemsSent = await dailyBabyCareNotifySentService.getAllAsync();

        //}

        itemsSent = await dailyBabyCareNotifySentService.getAllAsync();

        if (itemsSent.IsNullOrEmpty())
            return false; 

        //&& Helper.CompareTimes(x.createdAt.TimeOfDay,)

        return itemsSent.All(x=> x.createdAt.Day == currentTime.Day)
            && (itemsSent.Any(x => x.dailyCareTimeId == dailyCareTime.id)
            && usersFcmTokens.Any(x => itemsSent.Any(y => y.babyId == x.baby?.Id)));
    }

    protected  async override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {

            try
            {
                var dailyCareTimes = await dailyCareTimesBackgroundServices.getAllByTimeAsync(Helper.GetCurrentTime().TimeOfDay);

                if (dailyCareTimes == null) continue;



                foreach (var item in dailyCareTimes)
                {

                    var usersFcmTokens = await dailyCareTimesBackgroundServices.getUserFCMTokensByAgeGroupAsync(item.ageGroup);

                    if (usersFcmTokens == null || await checkIfSentCareNotifyAsync(item, usersFcmTokens))
                        continue;


                    foreach (var userFcmInfo in usersFcmTokens)
                    {
                        try
                        {
                            var msgBody = new
                            {

                                babyId = userFcmInfo.baby.Id,
                                babyName = userFcmInfo.baby.Name,
                                message = item.descript,
                            };

                            string notifyBody = JsonSerializer.Serialize(msgBody);

                            if (notifyBody.IsNullOrEmpty()) continue;

                            await _serviceMessages.sendMessageAsync(
                                token: userFcmInfo.fcmToken,
                                title: $"مــوعد {item.careType.name}",
                                body: notifyBody,
                                topic: $"{userFcmInfo.userId}",
                                tag: $"{NotificationType.CareBaby}");


                            DateTime dateDailyCare = DateTime.Today.Add(item.time);
                            await dailyBabyCareNotifySentService.AddAsync(new BabyHealthCareNotificationsSent
                            {
                                babyId= userFcmInfo.baby.Id,
                                dailyCareTimeId=item.id,
                                createdAt=dateDailyCare,
                            });


                            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($" Exception Services Worker :{ex.Message}");
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);

                }


                _logger.LogInformation("Services Worker runing ...");


                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (TaskCanceledException)
            {
                // التجاوب مع إلغاء المهمة
                break;
            }
            catch (Exception ex) 
            {
                _logger.LogWarning(ex.ToString());
            }
        }
    }
}