using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Middleware;
using ZeroGravity.Services;
using ZeroGravity.Infrastructure;
using ZeroGravity.Shared.Models.Dto;
using ZeroGravity.Validators;
using Hangfire;
using Hangfire.SqlServer;

namespace ZeroGravity
{
    public class Startup
    {
        private readonly CommonConfig _config;

        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                //.AddJsonFile("appsettings.Development.json", true)
                //.AddJsonFile("appsettings.LoadTest.json", true)
                //.AddJsonFile("appsettings.Production.json", true)  // Enable this for production deployment
                .Build();

            _config = Configuration.GetSection("Common").Get<CommonConfig>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureLocalization(services);

            services.AddCors();
            services.AddMemoryCache();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                })
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.IgnoreNullValues = true;
                });

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                //.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddRazorPages();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ZeroGravity API",
                    Description = "ZeroGravity API Swagger Surface"
                });

                // add authorize button on top right corner to tell swagger to use JWT in the Authorization header
                // after manually logging in using the authenticate api (returns a valid JWT)
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header, // tell swagger to use the JWT in the CRUD headers
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //Repository for Queries
            services.AddTransient<IRepository<ZeroGravityContext>, Repository<ZeroGravityContext>>();

            //Validators of Dtos
            services.AddTransient<IValidator<PersonalDataDto>, PersonalDataDtoValidator>();
            services.AddTransient<IValidator<PersonalGoalDto>, PersonalGoalDtoValidator>();
            services.AddTransient<IValidator<DietPreferencesDto>, DietPreferencesDtoValidator>();
            services.AddTransient<IValidator<MedicalConditionDto>, MedicalConditionsDtoValidator>();
            services.AddTransient<IValidator<ActivityDataDto>, ActivityDataDtoValidator>();
            services.AddTransient<IValidator<FastingSettingDto>, FastingSettingDtoValidator>();
            services.AddTransient<IValidator<FastingDataDto>, FastingDataDtoValidator>();
            services.AddTransient<IValidator<LiquidIntakeDto>, LiquidIntakeDtoValidator>();
            services.AddTransient<IValidator<MealDataDto>, MealDataDtoValidator>();

            // External Api Token Service
            services.AddSingleton<IExternalApiTokenService, ExternalApiTokenService>();

            // configure DI for application services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPersonalDataService, PersonalDataService>();
            services.AddScoped<IMedicalConditionService, MedicalConditionService>();
            services.AddScoped<IDietPreferenceService, DietPrefrerenceService>();
            services.AddScoped<IPersonalGoalsService, PersonalGoalsService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IFastingSettingsService, FastingSettingsService>();
            services.AddScoped<IFastingDataService, FastingDataService>();
            services.AddScoped<ILiquidIntakeService, LiquidIntakeService>();
            services.AddScoped<ILiquidNutritionService, LiquidNutritionService>();
            services.AddScoped<IMealDataService, MealDataService>();
            services.AddScoped<IMealNutritionService, MealNutritionService>();
            services.AddScoped<IBadgeinformationService, BadgeinformationService>();
            services.AddScoped<IStepCountDataService, StepCountDataService>();
            services.AddScoped<IWellbeingDataService, WellbeingDataService>();
            services.AddScoped<IIntegrationDataService, IntegrationDataService>();
            services.AddScoped<IFitbitClientService, FitbitClientService>();
            services.AddScoped<IStreamContentService, StreamContentService>();
            services.AddScoped<IVideoStreamService, VideoStreamService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ITrackedHistorieService, TrackedHistoryService>();
            services.AddScoped<IGlucoseDataService, GlucoseDataService>();
            services.AddScoped<IMeditationDataService, MeditationDataService>();
            services.AddScoped<ITestModeService, TestModeService>();
            services.AddScoped<IProgressSummaryService, ProgressSummaryService>();
            services.AddScoped<IInsightReportService, InsightReportService>();

            // Sugar Beat
            services.AddScoped<ISugarBeatSessionDataService, SugarBeatSessionDataService>();
            services.AddScoped<ISugarBeatAlertDataService, SugarBeatAlertDataService>();
            services.AddScoped<ISugarBeatGlucoseDataService, SugarBeatGlucoseDataService>();
            services.AddScoped<ISugarBeatSettingDataService, SugarBeatSettingDataService>();
            services.AddScoped<ISugarBeatEatingSessionDataService, SugarBeatEatingSessionDataService>();

            services.AddScoped<IEducationalInfoDataService, EducationalInfoDataService>();
            services.AddScoped<IQuestionDataService, QuestionDataService>();

            services.AddScoped<IWeightTrackerDataService, WeightTrackerDataService>();

            services.AddDbContext<ZeroGravityContext>(options =>
            {
                string devSpecificDbConnectionString = null; // _config?.ConnectionStrings?.FirstOrDefault();

                if (!string.IsNullOrEmpty(devSpecificDbConnectionString))
                {
                    options.UseSqlServer(devSpecificDbConnectionString, b => b.MigrationsAssembly("ZeroGravity.Db"));
                }
                else
                {
                    // use from appsettings.json
                    options.UseSqlServer(
                        Configuration.GetConnectionString("ZeroGravityDbConnection"), b => b.MigrationsAssembly("ZeroGravity.Db"));
                }
            }, ServiceLifetime.Transient);
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_CONNECTIONSTRING"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ZeroGravityContext dbContext)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.Equals("/apple-app-site-association"))
                {
                    context.Response.ContentType = "application/json";
                }
                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //auto migrate db
            //  dbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseHangfireDashboard("/hangfire");

            // generated swagger json and swagger ui middleware
            app.UseSwagger();
            app.UseSwaggerUI(x => { x.SwaggerEndpoint("/swagger/v1/swagger.json", "ZeroGravity API"); });
            app.UseRouting();

            var requestLocalizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(requestLocalizationOptions.Value);

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(
                x =>
                {
                    x.MapControllers();
                    x.MapRazorPages();
                    x.MapHangfireDashboard();
                });
        }

        /// <summary>
        ///     ConfigureLocalization
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureLocalization(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc().AddDataAnnotationsLocalization().AddFluentValidation();

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(typeof(ValidateModelStateAttribute));

            //}).AddDataAnnotationsLocalization().AddFluentValidation();

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    // a list of all available languages
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("de-DE")
                    };

                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;

                    opts.DefaultRequestCulture = new RequestCulture("en-US");

                    opts.RequestCultureProviders.Insert(0, new AcceptLanguageHeaderRequestCultureProvider());
                    // Annahme: swagger schickt vermutlich im "accept-language header" - "en-US" mit
                });
        }
    }
}
