using System;
using System.IO;
using System.Linq;
using AutoMapper;
using CMCore.Data;
using CMCore.DTO;
using CMCore.Helpers;
using CMCore.Interfaces;
using CMCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using File = CMCore.Models.File;

namespace CMCore.Services
{
    public class FileService : GenericService<File, FileDto>, IFileService
    {
        private readonly ITagService _tagService;
        private readonly ICompanyService _companyService;
        private readonly IClubService _clubService;
        private readonly FileSettings _fileSettings;
        private readonly IAwsS3Service _awsS3Service;

        public FileService(ContentManagerDbContext context, IOptions<FileSettings> fileSettings, 
            ITagService tagService, 
            ICompanyService companyService, 
            IClubService clubService, IAwsS3Service awsS3Service) 
            : base(context)
        {
            _tagService = tagService;
            _companyService = companyService;
            _clubService = clubService;
            _awsS3Service = awsS3Service;
            _fileSettings = fileSettings.Value;
        }

        public bool EraseFile(File fileInDb)
        {
            var res = _awsS3Service.DeleteFile(fileInDb.PathName, fileInDb.AwsRegion, !string.IsNullOrWhiteSpace(fileInDb.ThumbUrl));
            if (!string.IsNullOrWhiteSpace(res))
                return false;

            return true;
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

            // So you can't change the thumbUrl and Extension
            fileDto.ThumbUrl = fileInDb.ThumbUrl;

            return Mapper.Map<File, FileDto>(fileInDb);
        }

        public File CreateNew(IFormFile file, string fileName)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            var filePathName = Guid.NewGuid().ToString();

            // S3 File Save
            var resTuple = _awsS3Service.UploadFile(file, filePathName, fileExtension);
            
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
                AwsRegion = resTuple.Item1,
                ThumbUrl = resTuple.Item2,
                ExtensionId = extensionExist.Id
            };

            return AddEf(newFile) ? newFile : default(File);
        }

        public string DownloadFile(File fileInDb)
        {
            if (fileInDb == null)
                return null;

            var fileUrl = _awsS3Service.DownloadUrl(fileInDb.PathName, fileInDb.AwsRegion);
            return string.IsNullOrWhiteSpace(fileUrl) ? null : fileUrl;
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

        public string AddCompanyR(File fileInDb, FileDto fileDto)
        {
            if (fileDto.Companies != null)
            {
                foreach (var company in fileDto.Companies)
                {
                    var companyErrMsg = _companyService.AddFileR(company, fileInDb);
                    if (!string.IsNullOrWhiteSpace(companyErrMsg))
                        return companyErrMsg;
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

        public bool ClearRelations(File fileInDb)
        {
            try
            {
                fileInDb.FileTags.Clear();
                fileInDb.FileClubs.Clear();
                fileInDb.FileCompanies.Clear();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
