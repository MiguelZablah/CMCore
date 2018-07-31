using Amazon.S3;
using AutoMapper;
using CMCore.Data;
using CMCore.Helpers;
using CMCore.Interfaces;
using CMCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CMCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ContentManagerDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("LocalMySqlServer")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });
            services.Configure<FileSettings>(Configuration.GetSection("FileSettings"));
            services.Configure<AwsSettings>(Configuration.GetSection("AwsSettings"));
            services.AddCors(option =>
            {
                option.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddAutoMapper();
            // AWS
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            // Services for api
            services.AddTransient(typeof(GenericService<,>));
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<ICompanieService, CompanieService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IRegionService, RegionService>();
            services.AddTransient<ITypeService, TypeService>();
            services.AddTransient<IClubService, ClubService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IAwsS3Service, AwsS3Service>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseCors("AllowAllHeaders");
        }
    }
}
