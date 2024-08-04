using AutoMapper;
using Microsoft.EntityFrameworkCore;

using MomesCare.Api.Repository;
using IdentityModel;
using System.Collections.Generic;
using MomesCare.Api.Helpers;
using MomesCare.Api.Entities.ViewModel;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Entities.ViewModel.Baby;
using MomesCare.Api.Entities.ViewModel.Comment;
using MomesCare.Api.Entities.ViewModel.Post;
using MomesCare.Api.Exceptions;
using MomesCare.Api.Entities.ViewModel.BroadcastLive;
using MomesCare.Api.Services.Static;
using MomesCare.Api.Helpers.Enums;
using MomesCare.Api.Services.SubServices;

namespace MomesCare.Api.Services
{
    public class BroadcastLiveServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly IBroadcastLiveRepository _repository;
        private readonly IJoinBroadcastLiveRepository _joinBroadcastLive;
        private readonly IUserRepository _userRepository;
        private readonly UserServices _userServices;
        private readonly ServiceMessages _serviceMessages;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;


        private bool hasAuothorize(string userId)
        {
            if (userId != _userClaimsHelper.UserId)
                throw new Exception("no authorize !!");
            return true;
        }

        public BroadcastLiveServices(
            IUserClaimsHelper userClaimsHelper,
            IBroadcastLiveRepository repository,
            IJoinBroadcastLiveRepository joinBroadcastLive,
            IUserRepository userRepository,
            UserServices userServices,
            INotificationService notificationService,
            IMapper mapper,
            ServiceMessages serviceMessages)
        {

            _userServices = userServices;
            _notificationService = notificationService;
            _repository = repository;
            _joinBroadcastLive = joinBroadcastLive;
            _userRepository = userRepository;
            _mapper = mapper;
            _userClaimsHelper = userClaimsHelper;
            _serviceMessages = serviceMessages;
        }



        private async Task sendNotifyForAllUsersAsync(BroadcastLive broadcastLive)
        {
            await Task.Run(async () => {
              

                    Thread.Sleep(1000);
                    var tokens = await _userRepository.getAllFCMTokensAsync();
                    foreach (var token in tokens)
                    {
                        try
                        {
                            //if (token.UserId != _userClaimsHelper.UserId)
                            {
                                await _serviceMessages.sendMessageAsync(
                                    token: token.Token,
                                    title: $"الآن - بث مباشر لدكتور : {broadcastLive.user.FullName} . ",//في تمام الساعة  : {broadcastLive.startDateTime.ToString()}",
                                    body: $"رابط البث  : {broadcastLive.descript}",
                                    topic: $"{token.user.Id}",
                                    tag: $"{NotificationType.BroadcastLive}");
                            }
                      
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine($" Send Notify:{ex.Message}");
                        }

            }
                

                Console.WriteLine("Notifications sent to all users.");
            });
        }
        public async Task createAsync(CreateBroadcastLive model)
        {
            var broadcastLive = _mapper.Map<BroadcastLive>(model);
            //broadcastLive.descript = $"{model.descript} {model.link}";
            broadcastLive.user=await _repository.getCurrentUserAsync();
            await _repository.CreateAsync(broadcastLive);

            await sendNotifyForAllUsersAsync(broadcastLive);

        }
        public async Task<IndexBroadcastLive> updateAsync(UpdateBroadcastLive model)
        {
            var broadcastLive = await _repository.GetAsync(x => x.id == model.id);
            if (broadcastLive == null)
                throw new Exception("not found !!");

            broadcastLive = _mapper.Map<BroadcastLive>(model);
            broadcastLive.user = await _repository.getCurrentUserAsync();

            var item = await _repository.UpdateAsync(broadcastLive);
            var res = _mapper.Map<IndexBroadcastLive>(item);

            await sendNotifyForAllUsersAsync(broadcastLive);

            return res;

        }

        public async Task NotifyUsersOfBroadcastAsync(int id)
        {
            var broadcastLive = await _repository.GetAsync(x => x.id == id);
            if (broadcastLive == null)
                throw new Exception("not found !!");


            await sendNotifyForAllUsersAsync(broadcastLive);



            //await _notificationService.SendNotificationsAsync(async () =>
            //{

            //});

        }


        public async Task<IndexBroadcastLive> ActiveBroadcastLiveAsync(int id)
        {
            var broadcastLive = await _repository.GetAsync(x => x.id == id);
            if (broadcastLive == null)
                throw new Exception("not found !!");
            
            broadcastLive.status = BroadcastLiveStatus.Active;
            var item = await _repository.UpdateAsync(broadcastLive);
            var res = _mapper.Map<IndexBroadcastLive>(item);

            return res;

        }
        public async Task InActiveBroadcastLiveAsync(int id)
        {
            var broadcastLive = await _repository.GetAsync(x => x.id == id);
            if (broadcastLive == null)
                throw new Exception("not found !!");

            broadcastLive.status = BroadcastLiveStatus.Inactive;
            await _repository.UpdateAsync(broadcastLive);
          
        }

        public async Task<IEnumerable<IndexBroadcastLive>> getMyBroadcastLivesAsync()
        {
            var broadcastLive = (await _repository.GetAllAsync(x => x.user.Id == _userClaimsHelper.UserId, includeProperties: "user"))
                .OrderByDescending(x => x.id);

            if (broadcastLive == null)
                return new List<IndexBroadcastLive>();

            var items = _mapper.Map<List<IndexBroadcastLive>>(broadcastLive);

            return items;
        }
        public async Task<IEnumerable<IndexBroadcastLive>> getActivesBroadcastLivesAsync()
        {
            var broadcastLives = (await _repository.GetAllAsync(x => x.status==BroadcastLiveStatus.Active, 
                includeProperties: "user"))
                .OrderByDescending(x => x.id);

            if (broadcastLives == null)
                return new List<IndexBroadcastLive>();

            var broadcastLivesIndex = new List<IndexBroadcastLive>();// _mapper.Map<List<IndexBroadcastLive>>(broadcastLives);

      
            foreach ( var item in broadcastLives)
            {
                var user = item.user;
               
                var broadIndex = _mapper.Map<IndexBroadcastLive>(item);
                if(user!=null && broadIndex != null) 
                        broadIndex.user = await _userServices.getUserViewAsync(user.Id);
           
               
                broadcastLivesIndex.Add(broadIndex);
            }


            return broadcastLivesIndex;
        }
        public async Task<IndexBroadcastLive> getOne(int id)
        {
            var item = await _repository.GetQueryable(X => X.id == id)
                .Include(x => x.joinsBroadcastLives)
                .ThenInclude(x => x.user)
                .FirstOrDefaultAsync() ;

            if (item == null)
                throw new Exception("not found !!");

            var broadcastLive = _mapper.Map<IndexBroadcastLive>(item);

           //var userView=await _userServices.getUserViewAsync("");

            broadcastLive.liveStreamJoiners = _mapper.Map<List<IndexJoinBroadcastLive>>(item.joinsBroadcastLives);
            int i = 0;
            foreach( var itm in broadcastLive.liveStreamJoiners)
            {
                itm.user =await _userServices.getUserViewAsync(item.joinsBroadcastLives[i++].user.Id);
            }

                return broadcastLive;


        }
        public async Task deleteAsync(int id)
        {
            var baby = await _repository.GetAsync(x => x.id == id);
            if (baby == null)
                throw new Exception("not found !!");


            await _repository.RemoveAsync(baby);

        }


    }
}
