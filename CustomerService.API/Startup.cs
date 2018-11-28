using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerService.DAL.Contracts.Repositories;
using CustomerService.DAL.Entities;
using CustomerService.DAL.Entities.Contexts;
using CustomerService.DAL.Repositories;
using CustomerService.DTO.Creation;
using CustomerService.DTO.Output;
using CustomerService.DTO.Update;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerService.API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction=>
            {
                setupAction.ReturnHttpNotAcceptable = true;

                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                #pragma warning disable CS0618 
                setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter());
                #pragma warning restore CS0618 
            });

            services.AddDbContext<CustomersContext>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
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
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected fault, please try again later.");
                    });
                });
            }
            app.UseMvc();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDTO>()
                    .ForMember(dto => dto.Name, opt => opt.MapFrom(src =>
                     $"{src.FirstName} {src.LastName}"));

                cfg.CreateMap<ShippingAddress, ShippingAddressDTO>();

                cfg.CreateMap<CustomerCreationDTO, Customer>();

                cfg.CreateMap<ShippingAddressCreationDTO, ShippingAddress>();

                cfg.CreateMap<CustomerUpdateDTO, Customer>();

                cfg.CreateMap<ShippingAddressUpdateDTO, ShippingAddress>();
            });
        }
    }
}
