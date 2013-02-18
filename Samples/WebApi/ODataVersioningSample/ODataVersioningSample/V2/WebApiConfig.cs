﻿using AutoMapper;
using Microsoft.Data.Edm;
using ODataVersioningSample.Extensions;
using ODataVersioningSample.Models;
using ODataVersioningSample.V2.ViewModels;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.OData.Builder;

namespace ODataVersioningSample.V2
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Default routes
            config.Routes.MapODataRoute("DefaultODataRoute", null, GetModel());

            // Versioning by route prefix
            config.Routes.MapODataRoute("V2RouteVersioning", "versionbyroute/v2", GetModel());

            // Versioning by query string
            config.Routes.MapODataRoute("V2QueryStringVersioning", "versionbyquery", GetModel(), new { v = "2" }, null);

            // Versioning by header value
            config.Routes.MapODataRoute("V2HeaderVersioning", "versionbyheader", GetModel(), null, new { v = "2" });

            var controllerSelector = config.Services.GetService(typeof(IHttpControllerSelector)) as ODataVersionControllerSelector;
            // Mapping route name to controller versioning suffix
            controllerSelector.RouteVersionSuffixMapping.Add("DefaultODataRoute", "V2");
            controllerSelector.RouteVersionSuffixMapping.Add("V2RouteVersioning", "V2");
            controllerSelector.RouteVersionSuffixMapping.Add("V2QueryStringVersioning", "V2");
            controllerSelector.RouteVersionSuffixMapping.Add("V2HeaderVersioning", "V2");

            Mapper.CreateMap<DbProduct, Product>()
                .ForMember(dest => dest.Family, opt => opt.Ignore());
            Mapper.CreateMap<Product, DbProduct>();
            Mapper.CreateMap<DbProductFamily, ProductFamily>()
                .ForMember(dest => dest.Supplier, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());
            Mapper.CreateMap<ProductFamily, DbProductFamily>();
            Mapper.CreateMap<DbSupplier, Supplier>()
                .ForMember(dest => dest.ProductFamilies, opt => opt.Ignore());
            Mapper.CreateMap<Supplier, DbSupplier>();
            Mapper.CreateMap<DbAddress, Address>();
            Mapper.CreateMap<Address, DbAddress>();

            Mapper.AssertConfigurationIsValid();
        }

        private static IEdmModel GetModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            builder.EntitySet<ProductFamily>("ProductFamilies");
            builder.EntitySet<Supplier>("Suppliers");
            return builder.GetEdmModel();
        }
    }
}