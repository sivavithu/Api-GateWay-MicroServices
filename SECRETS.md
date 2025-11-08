# Secret Management Guide

This guide explains how to properly configure secrets for the API Gateway application.

## Overview

The API Gateway requires JWT configuration for authentication. These sensitive values should **never** be committed to version control.

## Required Configuration

The following settings must be configured:

- `AppSettings:issuer` - JWT token issuer
- `AppSettings:Audience` - JWT token audience  
- `AppSettings:Key` - JWT signing key (minimum 32 characters)

## Local Development Setup

For local development, use .NET User Secrets:

### Initial Setup

1. Navigate to the ApiGateway project directory:
   ```bash
   cd ApiGateway
   ```

2. Set your JWT configuration using user secrets:
   ```bash
   dotnet user-secrets set "AppSettings:Key" "your-secure-jwt-secret-key-minimum-32-characters"
   dotnet user-secrets set "AppSettings:issuer" "loginapp"
   dotnet user-secrets set "AppSettings:Audience" "myAwesomeAudience"
   ```

3. Verify your secrets are set:
   ```bash
   dotnet user-secrets list
   ```

### Viewing/Managing Secrets

- **List all secrets**: `dotnet user-secrets list`
- **Remove a secret**: `dotnet user-secrets remove "AppSettings:Key"`
- **Clear all secrets**: `dotnet user-secrets clear`

## Production/Docker Deployment

For production environments, use environment variables:

### Environment Variable Format

Use double underscores (`__`) to represent nested configuration:

```bash
export AppSettings__Key="your-production-jwt-secret"
export AppSettings__issuer="loginapp"
export AppSettings__Audience="myAwesomeAudience"
```

### Docker Deployment

#### Using docker run:
```bash
docker run -d \
  -e AppSettings__Key="your-jwt-secret" \
  -e AppSettings__issuer="loginapp" \
  -e AppSettings__Audience="myAwesomeAudience" \
  -p 8080:8080 \
  api-gateway
```

#### Using docker-compose:
Create a `.env` file (add to `.gitignore`):
```env
JWT_KEY=your-jwt-secret
JWT_ISSUER=loginapp
JWT_AUDIENCE=myAwesomeAudience
```

Update your `docker-compose.yml`:
```yaml
version: '3.8'
services:
  api-gateway:
    image: api-gateway
    environment:
      - AppSettings__Key=${JWT_KEY}
      - AppSettings__issuer=${JWT_ISSUER}
      - AppSettings__Audience=${JWT_AUDIENCE}
    ports:
      - "8080:8080"
```

### Azure Container Apps

Set environment variables in the Azure Portal:
1. Go to your Container App
2. Navigate to "Containers" → "Environment variables"
3. Add:
   - Name: `AppSettings__Key`, Value: (your secret)
   - Name: `AppSettings__issuer`, Value: `loginapp`
   - Name: `AppSettings__Audience`, Value: `myAwesomeAudience`

Or use Azure CLI:
```bash
az containerapp update \
  --name api-gateway \
  --resource-group your-rg \
  --set-env-vars \
    "AppSettings__Key=secretref:jwt-key" \
    "AppSettings__issuer=loginapp" \
    "AppSettings__Audience=myAwesomeAudience"
```

## Security Best Practices

1. **Never commit secrets** to version control
2. **Use strong keys** - JWT keys should be at least 32 characters
3. **Rotate secrets regularly** in production
4. **Use different secrets** for different environments
5. **Restrict access** to production secrets
6. **Use managed secrets** services like Azure Key Vault for production

## Troubleshooting

### Error: "JWT configuration is missing"

This error occurs when the required configuration values are not set. Follow these steps:

1. **For local development**: Run `dotnet user-secrets list` to verify secrets are configured
2. **For production**: Verify environment variables are set correctly
3. **Check spelling**: Ensure exact key names match (case-sensitive)

### User Secrets Not Working?

Verify the UserSecretsId is set in the `.csproj` file:
```xml
<PropertyGroup>
  <UserSecretsId>0653a0f9-1cd0-4b66-a0f6-ddda7b27ebe5</UserSecretsId>
</PropertyGroup>
```

## Example Configuration

An example configuration file is provided at `appsettings.example.json`. **Do not rename** this to `appsettings.json` as it contains placeholder values only.

## Additional Resources

- [.NET User Secrets Documentation](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [ASP.NET Core Configuration](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)
- [Azure Key Vault with .NET](https://docs.microsoft.com/en-us/azure/key-vault/general/overview)
