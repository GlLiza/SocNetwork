using System.IO;
using System.Web;
using System;
using DAL.IRepository;
using DAL.Repository;
using DAL.EF;

namespace BLL
{
    public class ImageService
    {
        private readonly IImagesRepository _imagesRepository;
        private readonly IUserRepository _userRepository;


        public ImageService(ImagesRepository imgRepository,UserRepository userRepository)
        {
            _imagesRepository = imgRepository;
            _userRepository = userRepository;
        }

        public void InsertImage(Images image)
        {
            _imagesRepository.Add(image);
        }

        public void EditUser(Images image)
        {
            _imagesRepository.Update(image);
        }
        
        public Images SearchImg(int? id)
        {
            Images img = _imagesRepository.GetById(id);
            return img;
        }
        
        public byte[] GetProfilesPhoto(int UserId)
        {
            var user = _userRepository.GetUserById(UserId);
            var image = _imagesRepository.GetById(user.ImagesId);
            return image.Data;
        }

        public Images ConvertImage(HttpPostedFileBase file)
        {
            byte[] imageData = null;

            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                imageData = binaryReader.ReadBytes(file.ContentLength);
            }
            Images headerImage = new Images()
            {
                Name = file.FileName,
                Data = imageData,
                ContentType = file.ContentType,
                Date = DateTime.Now
            };

            return headerImage;

        }

    }
}