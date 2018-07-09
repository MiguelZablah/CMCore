using System;
using System.IO;
using System.Linq;
using System.Net;
using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Helpers;
using CMCore.Interfaces;
using CMCore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using File = CMCore.Models.File;

namespace CMCore.Services
{
    public class FileService : GenericService<File, FileDto>, IFileService
    {
        private readonly IHostingEnvironment _host;
        private readonly ITagService _tagService;
        private readonly ICompanieService _companieService;
        private readonly IClubService _clubService;
        private readonly FileSettings _fileSettings;
        private readonly IAwsS3Service _awsS3Service;

        public FileService(ContentManagerDbContext context, IHostingEnvironment host, IOptions<FileSettings> fileSettings, 
            ITagService tagService, 
            ICompanieService companieService, 
            IClubService clubService, IAwsS3Service awsS3Service) 
            : base(context)
        {
            _host = host;
            _tagService = tagService;
            _companieService = companieService;
            _clubService = clubService;
            _awsS3Service = awsS3Service;
            _fileSettings = fileSettings.Value;
        }

        public bool EraseFile(File fileInDb)
        {
            var filePath = Path.Combine(_host.WebRootPath, _fileSettings.FilesFolderName, fileInDb.PathName);

            if (!System.IO.File.Exists(filePath))
                return false;
            try
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public string ValidateFile(IFormFile file, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "No file name";

            var checkName = CheckSameName(fileName);
            if (!string.IsNullOrWhiteSpace(checkName))
                return checkName;

            if (file == null)
                return "No file";

            if (file.Length <= 0)
                return "Empty file";


            if (!_fileSettings.IsSupported(file.FileName))
                return "Invalid File Type";

            return null;
        }

        public override FileDto Edit(File fileInDb, FileDto fileDto)
        {
            if (CompareString(fileInDb.Name, fileDto.Name))
                fileInDb.Name = fileDto.Name;

            if (CompareString(fileInDb.Description, fileDto.Description))
                fileInDb.Description = fileDto.Description;

            if(string.IsNullOrWhiteSpace(fileInDb.Name))
                fileInDb.Name = fileDto.Name;

            if (string.IsNullOrWhiteSpace(fileInDb.Description))
                fileInDb.Description = fileDto.Description;

            return Mapper.Map<File, FileDto>(fileInDb);
        }

        public File CreateNew(IFormFile file, string fileName)
        {
            var uploadFolderUrl = Path.Combine(_host.WebRootPath, _fileSettings.FilesFolderName);
            if (!Directory.Exists(uploadFolderUrl))
                Directory.CreateDirectory(uploadFolderUrl);

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var filePathName = Guid.NewGuid().ToString();

            // S3 File Save
            var fileSave = _awsS3Service.UploadFile(file, filePathName + fileExtension);
            if (!string.IsNullOrWhiteSpace(fileSave))
                return default(File);

            var extensionExist =  Context.Extensions.SingleOrDefault(e => e.Name == fileExtension);
            if (extensionExist == null)
            {
                var newExtension = new Extension
                {
                    Name = fileExtension
                };
                Context.Extensions.Add(newExtension);
                extensionExist = newExtension;
            }

            var newFile = new File
            {
                Name = fileName,
                PathName = filePathName + fileExtension,
                ExtensionId = extensionExist.Id
            };

            return AddEf(newFile) ? newFile : default(File);
        }

        public Stream GetStreamFromUrl(string url)
        {
            byte[] imageData;

            using (var wc = new WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }

        public IActionResult DowloadFile(File fileInDb, Controller controller)
        {
            if (fileInDb == null)
                return controller.NotFound();

            var fileUrl = _awsS3Service.DowloadUrl(fileInDb.PathName);
            if (string.IsNullOrWhiteSpace(fileUrl))
                return controller.BadRequest("File Url not found!");

            var contentType = _fileSettings.GetContentType(fileInDb.PathName);
            if (string.IsNullOrWhiteSpace(contentType))
                return controller.BadRequest("Content Type dosen't exist!");

            var stream = GetStreamFromUrl(fileUrl);

            return controller.File(stream, contentType);
        }

        public string AddTagR(File fileInDb, FileDto fileDto)
        {
            if (fileDto.Tags != null)
            {
                foreach (var tag in fileDto.Tags)
                {
                    var tagErrMsg = _tagService.AddFileR(tag, fileInDb);
                    if (!string.IsNullOrWhiteSpace(tagErrMsg))
                        return tagErrMsg;
                }

                return null;
            }

            return null;
        }

        public string AddCompanieR(File fileInDb, FileDto fileDto)
        {
            if (fileDto.Companies != null)
            {
                foreach (var companie in fileDto.Companies)
                {
                    var companieErrMsg = _companieService.AddFileR(companie, fileInDb);
                    if (!string.IsNullOrWhiteSpace(companieErrMsg))
                        return companieErrMsg;
                }

                return null;
            }

            return null;
        }

        public string AddClubR(File fileInDb, FileDto fileDto)
        {
            if (fileDto.Clubs != null)
            {
                foreach (var club in fileDto.Clubs)
                {
                    var clubErrMsg = _clubService.AddFileR(club, fileInDb);
                    if (!string.IsNullOrWhiteSpace(clubErrMsg))
                        return clubErrMsg;
                }

                return null;
            }

            return null;
        }

    }
}
