using Microsoft.EntityFrameworkCore;
using MomesCare.Api.Entities.Models;
using MomesCare.Api.Helpers;
using MomesCare.Api.Repository;
using MomesCare.Api.Services.SubServices;

namespace MomesCare.Api.Services.BackgroundServices.NewFolder
{
    public class DailyBabyCareNotifySentService
    {

        private readonly IDailyBabyCareNotifySentRepository _repository;
        private readonly ILogger<BackgroundWorkerService> _logger;

        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public DailyBabyCareNotifySentService(
                                ILogger<BackgroundWorkerService> logger,
                                ServiceMessages serviceMessages,
                                IDailyBabyCareNotifySentRepository repository)
        {
   
            this._logger = logger;
            _repository = repository;
        }


        private async Task _removeAllAsync()
        {
            try
            {
                var itemsToRemove = await _repository.GetAllAsync(x => x.createdAt.Day != Helper.GetCurrentTime().Day);
                   

                    foreach (var item in itemsToRemove)
                    {
                       await _repository.RemoveAsync(item);
                    }

     

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getAllAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<List<BabyHealthCareNotificationsSent>> getAllAsync()
        {
            await semaphore.WaitAsync();
            try
            {
                var items = _repository.GetQueryable();
                if (await items.AllAsync(x => x.createdAt.Day != Helper.GetCurrentTime().Day))
                {
                    await _removeAllAsync();
                    return new List<BabyHealthCareNotificationsSent>(); 
                }

    
                return await items.ToListAsync();
            }
            catch (Exception ex)
            {
                // سجل الاستثناء إذا حدث
                _logger.LogError($"Error in getAllAsync: {ex.Message}");
                throw; // إعادة رمي الاستثناء بعد تسجيله
            }
            finally
            {
                semaphore.Release();
            }
        }
        public async Task RemoveAllAsync()
        {
   
            await semaphore.WaitAsync();

            try
            {
                await _removeAllAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in RemoveAllAsync: {ex.Message}");
                throw; 
            }
            finally
            {
                semaphore.Release();
            }


        }

        public async Task AddAsync(BabyHealthCareNotificationsSent model)
        {
            await semaphore.WaitAsync();
            try
            {
                if (model == null)
                    throw new ArgumentNullException(nameof(model));


                if (model?.createdAt == null)
                    model!.createdAt = Helper.GetCurrentTime();

                await _repository.CreateAsync(model);
         
            }
            catch (Exception ex)
            {
                // سجل الاستثناء إذا حدث
                _logger.LogError($"Error in getAllAsync: {ex.Message}");
                throw; // إعادة رمي الاستثناء بعد تسجيله
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
