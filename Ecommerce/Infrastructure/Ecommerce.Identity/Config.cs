using Duende.IdentityServer.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.PortableExecutable;
using System.Text;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.IdentityModel.Tokens;
using Serilog.Filters;
using static Duende.IdentityServer.Events.TokenIssuedSuccessEvent;

namespace Ecommerce.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        // Associe les droits que le client peut demander
        new ApiScope[]
        {
            new ApiScope("catalogapi"),
            new ApiScope("basketapi"),
            new ApiScope("discountapi"),
            new ApiScope("orderingapi"),
            new ApiScope("catalogapi.read"),
            new ApiScope("catalogapi.write"),
            new ApiScope("ecommercegateway"),
        };
    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            // List of Microservices can go here : Définit l'API protégée
            new ApiResource("Catalog", "Catalog.API") //Name: Catalog (audience in Program.cs for Catalog) and Catalog.API : DisplayName
            {
                //Scopes = { "catalogapi.read", "catalogapi.write" }
                Scopes = { "catalogapi" }
            },
            new ApiResource("Basket", "Basket.API")
            {
                Scopes = {"basketapi"}
            },
            new ApiResource("Discount", "Discount.API")
            {
                Scopes = { "discountapi" }
            },
            new ApiResource("Ordering", "Ordering.API")
            {
                Scopes = { "orderingapi" }
            },
            new ApiResource("ECommerceGateway", "ECommerce Gateway")
            {
                Scopes = { "ecommercegateway" }
            }
        };
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            //m2m flow
            //Un client de type machine-to-machine (M2M), c’est une application ou un service backend (sans interface utilisateur humaine)
            //qui a besoin d'accéder à une API sécurisée, en s'identifiant par lui-même — sans login/password d’un utilisateur.
            // Exemple concret : [Service Commande] ────► [Service Catalogue]
            // Le Service Commande a besoin d’accéder à l’API Catalog pour récupérer les produits disponibles.
            // Il n’y a pas d’utilisateur humain connecté.
            // C’est le service lui-même qui s’identifie auprès du serveur d’identité (IdentityServer).
            // Il le fait via un Client ID + Client Secret.
            // Il obtient un token JWT qu’il utilise pour accéder à l’API Catalog.
            // C’est ça, un client machine-to-machine.
            new Client
            {
                ClientName = "Catalog API Client",
                ClientId = "CatalogApiClient",
                ClientSecrets = {new Secret("5c6eb3b4-4668-ac57-2b4591ec26d2".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials, //C’est le flow OAuth2 qui permet à une application autonome de demander un token d’accès à IdentityServer.
                AllowedScopes = {"catalogapi.read", "catalogapi.write"}
            },
            new Client
            {
                ClientName = "Basket API Client",
                ClientId = "BasketApiClient",
                ClientSecrets = {new Secret("3c6nb3b4-4667-ae57-2b4591ec26n2".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials, //C’est le flow OAuth2 qui permet à une application autonome de demander un token d’accès à IdentityServer.
                AllowedScopes = {"basketapi"}
            },
            new Client
            {
                ClientName = "ECommerce Gateway Client",
                ClientId = "ECommerceGatewayClient",
                ClientSecrets = {new Secret("3c6nb3b5-4667-az57-2b4691ed21n0".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials, //C’est le flow OAuth2 qui permet à une application autonome de demander un token d’accès à IdentityServer.
                AllowedScopes = { "ecommercegateway", "catalogapi", "basketapi", "discountapi", "orderingapi" }
            }
         };
}
