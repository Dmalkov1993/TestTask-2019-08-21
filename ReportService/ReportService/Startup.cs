﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReportService.Controllers;
using ReportService.Objects;
using ReportService.ReportBuiders;

namespace ReportService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Настройка параметров и DI
            services.AddOptions();

            // создание объекта List<ReportSetting> (настроек отчётов) по ключам из конфигурации
            services.Configure<List<ReportSetting>>(Configuration.GetSection("ReportSettings"));

            // Чтение конфигурации URL-ов
            services.Configure<UrlSettings>(Configuration.GetSection("UrlSettings"));

            // Регистрация нашего билдера для тестового задания
            services.AddSingleton<IReportBuilder, TestTaskReportBuilder>();

            // Регистрация mock-билдера
            // services.AddSingleton<IReportBuilder, MockReportBuilder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
