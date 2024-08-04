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
using MomesCare.Api.Entities.ViewModel.Courses;

namespace MomesCare.Api.Services
{
    public class CourseServices
    {

        private readonly IUserClaimsHelper _userClaimsHelper;
        private readonly ICourseRepository _repository;
        private readonly ICourseItemRepository _ItemRepository;
        private readonly IMapper _mapper;


        private bool hasAuothorize(string userId)
        {
            if (userId != _userClaimsHelper.UserId)
                throw new Exception("no authorize !!");
            return true;
        }

        public CourseServices(
            IUserClaimsHelper userClaimsHelper,
            ICourseRepository repository,
            ICourseItemRepository ItemRepository,
            IMapper mapper) 
        {

            this._repository = repository;
            this._ItemRepository = ItemRepository;
            this._mapper = mapper;
            this._userClaimsHelper = userClaimsHelper;  

        }





        //==================================================================
        // Course 
        //==================================================================
        public async Task createAsync(CreateCourse model)
        {
            var course = _mapper.Map<Course>(model);
            await _repository.CreateAsync(course);
        }
        public async Task<CourseIndex> updateAsync(UpdateCourse model)
        {
            var course = await _repository.GetAsync(x=>x.Id== model.Id);
            if (course == null)
                throw new  Exception("not found !!");

            course = _mapper.Map<Course>(model);

            var item = await _repository.UpdateAsync(course);
            var res = _mapper.Map<CourseIndex>(item);
    
            return res;

        }   
        public async Task<IEnumerable<CourseIndex>> getAllAsync()
        {
            var courses = (await _repository.GetAllAsync()).OrderByDescending(x => x.Id);
            if (courses == null)
                return new List<CourseIndex>();

            var items = _mapper.Map<List<CourseIndex>>(courses);
    
            return items;
        }      
        public async Task<CourseIndex> getOne(int id)
        {
            var item = await _repository.GetAsync(X => X.Id == id, includeProperties: "courseItems");
            
            if(item == null)
                throw new Exception("not found !!");

            var course = _mapper.Map<CourseIndex>(item);

            course.courseItemsIndex = _mapper.Map<List<CourseItemIndex>>(item.courseItems);


            return course;

            //new CourseIndex {
            //id=item.Id,
            //title=item.title,
            //descript=item.descript,
            //type=item.type,
            //urlImage=item.urlImage}; 

            //foreach (var Item in item.courseItems)
            //{
            //    var med = _mapper.Map<CourseItemIndex>(Item);
            //}

            //var CourseIndex = _mapper.Map<CourseIndex>(Baby);


        }      
        public async Task deleteAsync(int id)
        {
            var baby = await _repository.GetAsync(x => x.Id == id);
            if (baby == null)
                throw new  Exception("not found !!");


            await _repository.RemoveAsync(baby);
            
        }

        //==================================================================
        // Course Items
        //==================================================================

        public async Task createCourseItemAsync(CreateCourseItem model)
        {
            var Item = _mapper.Map<CourseItem>(model);

            Item.course =await _repository.GetAsync(x=>x.Id==model.courseId);

            await _ItemRepository.CreateAsync(Item);

        }
        public async Task<CourseItemIndex> updateCourseItemAsync(UpdateCourseItem model)
        {
            var item = await _ItemRepository.GetAsync(x => x.Id == model.Id);

            if (item == null)
                throw new Exception("not found !!");

            var Item = _mapper.Map<CourseItem>(model);

            item = await _ItemRepository.UpdateAsync(Item);
            var res = _mapper.Map<CourseItemIndex>(item);

            return res;
        }
        public async Task deleteCourseItemAsync(int id)
        {
            var item = await _ItemRepository.GetAsync(x => x.Id == id);
            if (item == null)
                throw new Exception("not found !!");

            await _ItemRepository.RemoveAsync(item);
        }
        public async Task<IEnumerable<CourseItemIndex>> getCourseItemsAsync(int courseId)
        {
            var item = (await _repository.GetAsync(x => x.Id == courseId, includeProperties: "courseItems"));
            if (item == null)
                throw new Exception("not found");


            var items = _mapper.Map<List<CourseItemIndex>>(item.courseItems);

            return items;



        }


    }
}
